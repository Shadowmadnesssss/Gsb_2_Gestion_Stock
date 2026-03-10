using GSB_Gestion_Stock.DAO;
using GSB_Gestion_Stock.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GSB_Gestion_Stock.Forms
{
    public partial class AdminViewForm : Form
    {
        // DAO pour accéder aux données
        private readonly UserDAO userDAO = new UserDAO();
        private readonly MedicineDAO medicineDAO = new MedicineDAO();
        private readonly PatientDAO patientDAO = new PatientDAO();
        private readonly PrescriptionDAO prescriptionDAO = new PrescriptionDAO();
        private readonly AppartientDAO appartientDAO = new AppartientDAO();

        // Données actuelles chargées
        private List<User> users = new List<User>();
        private List<Medicine> medicines = new List<Medicine>();
        private List<Patient> patients = new List<Patient>();
        private List<Prescription> prescriptions = new List<Prescription>();

        // Type de données actuellement affiché
        private string currentDataType = "Utilisateurs";

        public AdminViewForm()
        {
            InitializeComponent();
        }

        private void AdminViewForm_Load(object sender, EventArgs e)
        {
            // Initialiser le ComboBox avec les types de données
            cmbDataType.Items.AddRange(new string[] { "Utilisateurs", "Medicine", "Patients", "Prescription" });
            cmbDataType.SelectedIndex = 0;

            // Désactiver les boutons d'ajout pour Patients et Prescription
            // (seuls les utilisateurs normaux peuvent les créer)
            UpdateButtonStates();

            // Charger les données initiales
            LoadData();
        }

        /// <summary>
        /// Met à jour l'état des boutons selon le type de données sélectionné
        /// (les administrateurs ont tous les droits)
        /// </summary>
        private void UpdateButtonStates()
        {
            // En mode administrateur, on autorise l'ajout pour tous les types
            btnAdd.Enabled = true;
            btnAdd.Text = "➕ Ajouter";
            
            // Activer les boutons edit et delete pour tous les types
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
        }

        /// <summary>
        /// Charge les données selon le type sélectionné dans le ComboBox
        /// </summary>
        private void LoadData()
        {
            try
            {
                currentDataType = cmbDataType.SelectedItem?.ToString() ?? "Utilisateurs";

                switch (currentDataType)
                {
                    case "Utilisateurs":
                        LoadUsers();
                        break;
                    case "Medicine":
                        LoadMedicines();
                        break;
                    case "Patients":
                        LoadPatients();
                        break;
                    case "Prescription":
                        LoadPrescriptions();
                        break;
                }

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
        /// Charge tous les utilisateurs dans le DataGridView
        /// </summary>
        private void LoadUsers()
        {
            users = userDAO.GetAll();
            dgvData.DataSource = null;
            dgvData.Columns.Clear();

            dgvData.Columns.Add("Name", "Nom");
            dgvData.Columns.Add("Firstname", "Prénom");
            dgvData.Columns.Add("Role", "Rôle");
            dgvData.Columns["Role"].DefaultCellStyle.Format = "Admin;Utilisateur";

            foreach (var user in users)
            {
                dgvData.Rows.Add(user.Name, user.Firstname, user.Role ? "Admin" : "Utilisateur");
            }

            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Charge tous les médicaments dans le DataGridView
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
                MessageBox.Show($"Erreur lors du chargement des medicines : {ex.Message}\n\nDétails : {ex.StackTrace}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Charge tous les patients dans le DataGridView
        /// </summary>
        private void LoadPatients()
        {
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
        /// Charge toutes les prescriptions dans le DataGridView
        /// </summary>
        private void LoadPrescriptions()
        {
            try
            {
                prescriptions = prescriptionDAO.GetAll();
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

                    int rowIndex = dgvData.Rows.Add(patientName, prescription.Validity.ToString("dd/MM/yyyy"), medicinesText);
                    
                    // Colorer en rouge la cellule de validité si la prescription est expirée
                    if (prescription.Validity < DateTime.Now)
                    {
                        dgvData.Rows[rowIndex].Cells["Validity"].Style.BackColor = Color.FromArgb(255, 200, 200); // Rouge clair
                        dgvData.Rows[rowIndex].Cells["Validity"].Style.ForeColor = Color.DarkRed;
                    }
                }

                dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des prescriptions : {ex.Message}\n\nDétails : {ex.StackTrace}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        /// Gère le changement de type de données dans le ComboBox
        /// </summary>
        private void cmbDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Clear();
            UpdateButtonStates();
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
            switch (currentDataType)
            {
                case "Utilisateurs":
                    using (var form = new AddEditUserForm())
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            LoadData();
                        }
                    }
                    break;
                case "Medicine":
                    using (var form = new AddEditMedicineForm())
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            LoadData();
                        }
                    }
                    break;
                case "Patients":
                    using (var form = new AddEditPatientForm())
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            LoadPatients();
                        }
                    }
                    break;
                case "Prescription":
                    using (var form = new AddEditPrescriptionForm())
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            LoadPrescriptions();
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
                case "Utilisateurs":
                    if (selectedIndex < users.Count)
                    {
                        selectedId = users[selectedIndex].Id;
                        var user = userDAO.GetById(selectedId);
                        if (user != null)
                        {
                            using (var form = new AddEditUserForm(user))
                            {
                                if (form.ShowDialog() == DialogResult.OK)
                                {
                                    LoadData();
                                }
                            }
                        }
                    }
                    break;
                case "Medicine":
                    if (selectedIndex < medicines.Count)
                    {
                        selectedId = medicines[selectedIndex].Id;
                        var medicine = medicineDAO.GetById(selectedId);
                        if (medicine != null)
                        {
                            using (var form = new AddEditMedicineForm(medicine))
                            {
                                if (form.ShowDialog() == DialogResult.OK)
                                {
                                    LoadData();
                                }
                            }
                        }
                    }
                    break;
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
                case "Utilisateurs":
                    if (selectedIndex < users.Count)
                    {
                        selectedId = users[selectedIndex].Id;
                        itemName = $"{users[selectedIndex].Firstname} {users[selectedIndex].Name}";
                    }
                    break;
                case "Medicine":
                    if (selectedIndex < medicines.Count)
                    {
                        selectedId = medicines[selectedIndex].Id;
                        itemName = medicines[selectedIndex].Name ?? "";
                    }
                    break;
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
                    case "Utilisateurs":
                        success = userDAO.Delete(selectedId);
                        break;
                    case "Medicine":
                        success = medicineDAO.Delete(selectedId);
                        break;
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

        private void pnlTop_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
