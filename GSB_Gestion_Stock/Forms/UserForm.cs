using GSB_Gestion_Stock.DAO;
using GSB_Gestion_Stock.Models;
using GSB_Gestion_Stock.Services;
using System;
using System.Collections.Generic;
using System.Drawing; // Ajouter pour Color, Font, etc.
using System.Windows.Forms;

namespace GSB_Gestion_Stock.Forms
{
    /// <summary>
    /// Formulaire principal pour les utilisateurs normaux (non-admin)
    /// Permet de créer des patients et des prescriptions
    /// </summary>
    public partial class UserForm : Form
    {
        private readonly PatientDAO patientDAO = new PatientDAO();
        private readonly PrescriptionDAO prescriptionDAO = new PrescriptionDAO();
        private readonly MedicineDAO medicineDAO = new MedicineDAO();
        private readonly AppartientDAO appartientDAO = new AppartientDAO();
        private readonly PrescriptionPDFExporter pdfExporter = new PrescriptionPDFExporter();
        private readonly PatientDossierPDFExporter dossierExporter = new PatientDossierPDFExporter();

        // Données actuelles
        private List<Patient> patients = new List<Patient>();
        private List<Prescription> prescriptions = new List<Prescription>();
        private List<Medicine> medicines = new List<Medicine>();

        // Type de données actuellement affiché
        private string currentDataType = "Patients";

        // Variable pour suivre le patient sélectionné et son panneau de prescriptions
        private int? selectedPatientId = null;
        private Panel? prescriptionsPanel = null;
        private DataGridView? prescriptionsDGV = null;

        public UserForm()
        {
            InitializeComponent();
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            // Vérifier qu'un utilisateur est connecté
            if (!CurrentUser.IsLoggedIn)
            {
                MessageBox.Show("Aucun utilisateur connecté.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            // Afficher le nom de l'utilisateur
            lblWelcome.Text = $"Bienvenue {CurrentUser.User.Firstname} {CurrentUser.User.Name}";

            // Initialiser le ComboBox avec les types de données disponibles
            cmbDataType.Items.AddRange(new string[] { "Patients", "Prescription", "Medicine" });
            cmbDataType.SelectedIndex = 0;

            // Mettre à jour l'état des boutons selon le type de données
            UpdateButtonStates();

            // Charger les données initiales
            LoadData();

            // Ajouter un gestionnaire d'événement pour le clic sur une ligne
            dgvData.CellClick += DgvData_CellClick;
            dgvData.SelectionChanged += DgvData_SelectionChanged;
        }

        /// <summary>
        /// Charge les données selon le type sélectionné dans le ComboBox
        /// </summary>
        private void LoadData()
        {
            try
            {
                currentDataType = cmbDataType.SelectedItem?.ToString() ?? "Patients";

                switch (currentDataType)
                {
                    case "Patients":
                        LoadPatients();
                        break;
                    case "Prescription":
                        LoadPrescriptions();
                        break;
                    case "Medicine":
                        LoadMedicines();
                        break;
                }

                // Mettre à jour l'état des boutons après le chargement
                UpdateButtonStates();

                // Appliquer la recherche si du texte est présent
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    ApplySearch();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}", 
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Charge tous les patients dans le DataGridView
        /// Tous les utilisateurs ont accès à tous les patients
        /// </summary>
        private void LoadPatients()
        {
            if (!CurrentUser.IsLoggedIn)
            {
                MessageBox.Show("Aucun utilisateur connecté.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Charger tous les patients (tous les utilisateurs ont accès à tous les patients)
            patients = patientDAO.GetAll();
            dgvData.DataSource = null;
            dgvData.Columns.Clear();

            dgvData.Columns.Add("Name", "Nom");
            dgvData.Columns.Add("Firstname", "Prénom");
            dgvData.Columns.Add("Age", "Âge");
            dgvData.Columns.Add("Gender", "Genre");

            foreach (var patient in patients)
            {
                dgvData.Rows.Add(patient.Name, patient.Firstname, 
                    patient.Age, patient.Gender);
            }

            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Charge toutes les medicines (en lecture seule) dans le DataGridView
        /// </summary>
        private void LoadMedicines()
        {
            try
            {
                medicines = medicineDAO.GetAll();
                dgvData.DataSource = null;
                dgvData.Columns.Clear();

                dgvData.Columns.Add("Name", "Nom");
                dgvData.Columns.Add("Dosage", "Dosage");
                dgvData.Columns.Add("Molecule", "Molécule");
                dgvData.Columns.Add("Description", "Description");

                foreach (var medicine in medicines)
                {
                    dgvData.Rows.Add(medicine.Name, medicine.Dosage, 
                        medicine.Molecule, medicine.Description);
                }

                dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des medicines : {ex.Message}", 
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Charge toutes les prescriptions de l'utilisateur connecté dans le DataGridView
        /// </summary>
        private void LoadPrescriptions()
        {
            if (!CurrentUser.IsLoggedIn)
            {
                MessageBox.Show("Aucun utilisateur connecté.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            prescriptions = prescriptionDAO.GetByUserId(CurrentUser.User.Id);
            dgvData.DataSource = null;
            dgvData.Columns.Clear();

            dgvData.Columns.Add("Patient", "Patient");
            dgvData.Columns.Add("Validity", "Validité");
            dgvData.Columns.Add("Medicines", "Médicaments");

            foreach (var prescription in prescriptions)
            {
                // Récupérer les informations du patient
                var patient = patientDAO.GetById(prescription.PatientId);
                string patientName = "Patient inconnu";
                if (patient != null)
                {
                    patientName = $"{patient.Firstname} {patient.Name}";
                }
                
                // Récupérer les médicaments associés à cette prescription
                var appartients = appartientDAO.GetByPrescriptionId(prescription.Id);
                string medicinesText = "";
                
                if (appartients != null && appartients.Count > 0)
                {
                    var medicineNames = new List<string>();
                    foreach (var appartient in appartients)
                    {
                        var medicine = medicineDAO.GetById(appartient.IdMedicine);
                        if (medicine != null)
                        {
                            medicineNames.Add($"{medicine.Name} (x{appartient.Quantity})");
                        }
                    }
                    medicinesText = string.Join(", ", medicineNames);
                }
                else
                {
                    medicinesText = "Aucun médicament";
                }
                
                dgvData.Rows.Add(patientName, prescription.Validity.ToString("dd/MM/yyyy"), medicinesText);
            }

            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Applique le filtre de recherche sur les données affichées
        /// </summary>
        private void ApplySearch()
        {
            string searchText = txtSearch.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadData();
                return;
            }

            // Filtrer les lignes du DataGridView
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                bool visible = false;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    {
                        visible = true;
                        break;
                    }
                }
                row.Visible = visible;
            }
        }

        /// <summary>
        /// Met à jour l'état des boutons selon le type de données sélectionné
        /// Les utilisateurs peuvent seulement consulter Medicine (lecture seule)
        /// </summary>
        private void UpdateButtonStates()
        {
            if (currentDataType == "Medicine")
            {
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnExportPDF.Enabled = false;
                btnExportDossier.Enabled = false;
                btnAdd.Text = "❌ Consultation seule";
            }
            else if (currentDataType == "Prescription")
            {
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnExportPDF.Enabled = true; // Activer l'export PDF pour les prescriptions
                btnExportDossier.Enabled = false; // Désactiver pour les prescriptions
                btnAdd.Text = "➕ Ajouter";
            }
            else if (currentDataType == "Patients")
            {
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnExportPDF.Enabled = false; // Désactiver l'export PDF pour les patients
                btnExportDossier.Enabled = true; // Activer l'export dossier pour les patients
                btnAdd.Text = "➕ Ajouter";
            }
        }

        /// <summary>
        /// Gère le changement de type de données dans le ComboBox
        /// </summary>
        private void cmbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadData();
        }

        /// <summary>
        /// Gère la recherche en temps réel
        /// </summary>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplySearch();
        }

        /// <summary>
        /// Ouvre le formulaire d'ajout selon le type de données
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (currentDataType == "Medicine")
            {
                MessageBox.Show("La consultation des medicines est en lecture seule. Vous ne pouvez pas les modifier.", 
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            switch (currentDataType)
            {
                case "Patients":
                    using (var form = new AddEditPatientForm())
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            LoadData();
                        }
                    }
                    break;
                case "Prescription":
                    using (var form = new AddEditPrescriptionForm())
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            LoadData();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Ouvre le formulaire de modification selon le type de données
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (currentDataType == "Medicine")
            {
                MessageBox.Show("La consultation des medicines est en lecture seule. Vous ne pouvez pas les modifier.", 
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvData.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une ligne à modifier.", 
                    "Aucune sélection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Récupérer l'ID depuis la liste en mémoire (car les colonnes ID ne sont plus affichées)
            int selectedIndex = dgvData.SelectedRows[0].Index;
            int selectedId = 0;

            switch (currentDataType)
            {
                case "Patients":
                    if (selectedIndex < patients.Count)
                    {
                        selectedId = patients[selectedIndex].Id;
                        var patient = patientDAO.GetById(selectedId);
                        if (patient != null)
                        {
                            using (var form = new AddEditPatientForm(patient))
                            {
                                if (form.ShowDialog() == DialogResult.OK)
                                {
                                    LoadData();
                                }
                            }
                        }
                    }
                    break;
                case "Prescription":
                    if (selectedIndex < prescriptions.Count)
                    {
                        selectedId = prescriptions[selectedIndex].Id;
                        var prescription = prescriptionDAO.GetById(selectedId);
                        if (prescription != null)
                        {
                            using (var form = new AddEditPrescriptionForm(prescription))
                            {
                                if (form.ShowDialog() == DialogResult.OK)
                                {
                                    LoadData();
                                }
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Supprime l'élément sélectionné après confirmation
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentDataType == "Medicine")
            {
                MessageBox.Show("La consultation des medicines est en lecture seule. Vous ne pouvez pas les supprimer.", 
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvData.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une ligne à supprimer.", 
                    "Aucune sélection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Récupérer l'ID depuis la liste en mémoire (car les colonnes ID ne sont plus affichées)
            int selectedIndex = dgvData.SelectedRows[0].Index;
            int selectedId = 0;
            string itemName = "";

            switch (currentDataType)
            {
                case "Patients":
                    if (selectedIndex < patients.Count)
                    {
                        selectedId = patients[selectedIndex].Id;
                        itemName = $"{patients[selectedIndex].Firstname} {patients[selectedIndex].Name}";
                    }
                    break;
                case "Prescription":
                    if (selectedIndex < prescriptions.Count)
                    {
                        selectedId = prescriptions[selectedIndex].Id;
                        itemName = $"Prescription #{selectedId}";
                    }
                    break;
            }

            if (selectedId == 0)
            {
                MessageBox.Show("Erreur : Impossible de récupérer l'ID de l'élément sélectionné.",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string message = string.IsNullOrEmpty(itemName)
                ? "Êtes-vous sûr de vouloir supprimer cet élément ?"
                : $"Êtes-vous sûr de vouloir supprimer \"{itemName}\" ?";

            DialogResult result = MessageBox.Show(
                message, 
                "Confirmation de suppression", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = false;

                switch (currentDataType)
                {
                    case "Patients":
                        // Supprimer d'abord les prescriptions liées pour éviter les erreurs de contrainte
                        var patientPrescriptions = prescriptionDAO.GetByPatientId(selectedId);
                        foreach (var prescription in patientPrescriptions)
                        {
                            prescriptionDAO.Delete(prescription.Id);
                        }
                        success = patientDAO.Delete(selectedId);
                        break;
                    case "Prescription":
                        success = prescriptionDAO.Delete(selectedId);
                        break;
                }

                if (success)
                {
                    MessageBox.Show("Élément supprimé avec succès.", 
                        "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression.", 
                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Exporte la prescription sélectionnée en PDF
        /// </summary>
        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            if (currentDataType != "Prescription")
            {
                MessageBox.Show("L'export PDF est uniquement disponible pour les prescriptions.",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvData.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une prescription à exporter.",
                    "Aucune sélection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Récupérer l'ID depuis la liste en mémoire
            int selectedIndex = dgvData.SelectedRows[0].Index;
            if (selectedIndex >= prescriptions.Count)
            {
                MessageBox.Show("Erreur : Impossible de récupérer la prescription sélectionnée.",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Prescription selectedPrescription = prescriptions[selectedIndex];

            // Ouvrir une boîte de dialogue pour choisir l'emplacement de sauvegarde
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Fichiers PDF (*.pdf)|*.pdf";
                saveFileDialog.FileName = $"Prescription_{selectedPrescription.Id}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                saveFileDialog.Title = "Exporter la prescription en PDF";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Exporter en PDF
                        bool success = pdfExporter.ExportToPDF(selectedPrescription, saveFileDialog.FileName);

                        if (success)
                        {
                            MessageBox.Show($"La prescription a été exportée avec succès vers :\n{saveFileDialog.FileName}",
                                "Export réussi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Optionnel : ouvrir le fichier PDF
                            DialogResult openFile = MessageBox.Show("Voulez-vous ouvrir le fichier PDF maintenant ?",
                                "Ouvrir le fichier", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (openFile == DialogResult.Yes)
                            {
                                var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                {
                                    FileName = saveFileDialog.FileName,
                                    UseShellExecute = true
                                });
                                
                                // Disposer le processus pour libérer les ressources système
                                // Note: Avec UseShellExecute = true, le processus peut être null
                                // Le Dispose() libère le handle du processus, pas le processus lui-même
                                process?.Dispose();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de l'export de la prescription en PDF.",
                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de l'export PDF : {ex.Message}",
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Exporte le dossier complet du patient sélectionné en PDF
        /// </summary>
        private void btnExportDossier_Click(object sender, EventArgs e)
        {
            if (currentDataType != "Patients")
            {
                MessageBox.Show("L'export de dossier est uniquement disponible pour les patients.",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvData.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un patient pour exporter son dossier.",
                    "Aucune sélection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Récupérer l'ID depuis la liste en mémoire
            int selectedIndex = dgvData.SelectedRows[0].Index;
            if (selectedIndex >= patients.Count)
            {
                MessageBox.Show("Erreur : Impossible de récupérer le patient sélectionné.",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Patient selectedPatient = patients[selectedIndex];

            // Ouvrir une boîte de dialogue pour choisir l'emplacement de sauvegarde
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Fichiers PDF (*.pdf)|*.pdf";
                saveFileDialog.FileName = $"Dossier_{selectedPatient.Name}_{selectedPatient.Firstname}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                saveFileDialog.Title = "Exporter le dossier patient en PDF";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Exporter en PDF
                        bool success = dossierExporter.ExportToPDF(selectedPatient.Id, saveFileDialog.FileName);

                        if (success)
                        {
                            MessageBox.Show($"Le dossier patient a été exporté avec succès vers :\n{saveFileDialog.FileName}",
                                "Export réussi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Optionnel : ouvrir le fichier PDF
                            DialogResult openFile = MessageBox.Show("Voulez-vous ouvrir le fichier PDF maintenant ?",
                                "Ouvrir le fichier", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (openFile == DialogResult.Yes)
                            {
                                var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                {
                                    FileName = saveFileDialog.FileName,
                                    UseShellExecute = true
                                });
                                process?.Dispose();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de l'export du dossier patient en PDF.",
                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de l'export PDF : {ex.Message}",
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Déconnexion de l'utilisateur
        /// </summary>
        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Êtes-vous sûr de vouloir vous déconnecter ?", 
                "Déconnexion", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                CurrentUser.Logout();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Gère le clic sur une ligne du DataGridView
        /// </summary>
        private void DgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (currentDataType == "Patients" && e.RowIndex >= 0)
            {
                int selectedIndex = e.RowIndex;
                if (selectedIndex < patients.Count)
                {
                    int patientId = patients[selectedIndex].Id;
                    TogglePatientPrescriptions(patientId, selectedIndex);
                }
            }
        }

        /// <summary>
        /// Gère le changement de sélection dans le DataGridView
        /// </summary>
        private void DgvData_SelectionChanged(object sender, EventArgs e)
        {
            // Si on change de type de données, masquer le panneau de prescriptions
            if (currentDataType != "Patients")
            {
                HidePrescriptionsPanel();
            }
        }

        /// <summary>
        /// Affiche ou masque le panneau des prescriptions d'un patient
        /// </summary>
        private void TogglePatientPrescriptions(int patientId, int rowIndex)
        {
            // Si le même patient est cliqué, masquer le panneau
            if (selectedPatientId == patientId && prescriptionsPanel != null && prescriptionsPanel.Visible)
            {
                HidePrescriptionsPanel();
                return;
            }

            // Masquer le panneau précédent s'il existe
            HidePrescriptionsPanel();

            // Récupérer les prescriptions du patient
            var patientPrescriptions = prescriptionDAO.GetByPatientId(patientId);
            
            if (patientPrescriptions.Count == 0)
            {
                MessageBox.Show("Ce patient n'a aucune prescription enregistrée.", 
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Créer le panneau pour afficher les prescriptions
            selectedPatientId = patientId;
            prescriptionsPanel = new Panel();
            prescriptionsPanel.BackColor = Color.FromArgb(240, 240, 240);
            prescriptionsPanel.Dock = DockStyle.Bottom;
            prescriptionsPanel.Height = 200;
            prescriptionsPanel.Padding = new Padding(10);

            // Créer un label pour le titre
            Label titleLabel = new Label();
            titleLabel.Text = $"Prescriptions de {patients[rowIndex].Firstname} {patients[rowIndex].Name} ({patientPrescriptions.Count} prescription(s))";
            titleLabel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(10, 10);

            // Créer le DataGridView pour les prescriptions
            prescriptionsDGV = new DataGridView();
            prescriptionsDGV.AllowUserToAddRows = false;
            prescriptionsDGV.ReadOnly = true;
            prescriptionsDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            prescriptionsDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            prescriptionsDGV.Dock = DockStyle.Fill;
            prescriptionsDGV.Margin = new Padding(0, 30, 0, 0);

            // Ajouter les colonnes
            prescriptionsDGV.Columns.Add("Id", "ID");
            prescriptionsDGV.Columns.Add("Validity", "Validité");
            prescriptionsDGV.Columns.Add("Medicines", "Médicaments");
            prescriptionsDGV.Columns["Id"].Width = 50;

            // Remplir avec les prescriptions
            foreach (var prescription in patientPrescriptions)
            {
                // Récupérer les médicaments associés
                var appartients = appartientDAO.GetByPrescriptionId(prescription.Id);
                string medicinesText = "";
                
                if (appartients != null && appartients.Count > 0)
                {
                    var medicineNames = new List<string>();
                    foreach (var appartient in appartients)
                    {
                        var medicine = medicineDAO.GetById(appartient.IdMedicine);
                        if (medicine != null)
                        {
                            medicineNames.Add($"{medicine.Name} (x{appartient.Quantity})");
                        }
                    }
                    medicinesText = string.Join(", ", medicineNames);
                }
                else
                {
                    medicinesText = "Aucun médicament";
                }

                prescriptionsDGV.Rows.Add(
                    prescription.Id,
                    prescription.Validity.ToString("dd/MM/yyyy"),
                    medicinesText
                );
            }

            // Ajouter les contrôles au panneau
            prescriptionsPanel.Controls.Add(titleLabel);
            prescriptionsPanel.Controls.Add(prescriptionsDGV);

            // Ajouter le panneau au formulaire
            this.Controls.Add(prescriptionsPanel);
            prescriptionsPanel.BringToFront();

            // Ajuster la hauteur du DataGridView principal
            dgvData.Height = this.ClientSize.Height - pnlTop.Height - pnlButtons.Height - prescriptionsPanel.Height;
        }

        /// <summary>
        /// Masque le panneau des prescriptions
        /// </summary>
        private void HidePrescriptionsPanel()
        {
            if (prescriptionsPanel != null)
            {
                this.Controls.Remove(prescriptionsPanel);
                prescriptionsPanel.Dispose();
                prescriptionsPanel = null;
                prescriptionsDGV = null;
                selectedPatientId = null;
                
                // Restaurer la hauteur du DataGridView principal
                dgvData.Height = this.ClientSize.Height - pnlTop.Height - pnlButtons.Height;
            }
        }
    }
}
