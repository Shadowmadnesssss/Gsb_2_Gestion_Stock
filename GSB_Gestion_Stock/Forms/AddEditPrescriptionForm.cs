using GSB_Gestion_Stock.DAO;
using GSB_Gestion_Stock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GSB_Gestion_Stock.Forms
{
    /// <summary>
    /// Formulaire pour ajouter ou modifier une ordonnance
    /// </summary>
    public partial class AddEditPrescriptionForm : Form
    {
        private readonly PrescriptionDAO prescriptionDAO = new PrescriptionDAO();
        private readonly UserDAO userDAO = new UserDAO();
        private readonly PatientDAO patientDAO = new PatientDAO();
        private readonly MedicineDAO medicineDAO = new MedicineDAO();
        private readonly AppartientDAO appartientDAO = new AppartientDAO();
        private Prescription prescriptionToEdit = null;
        private bool isEditMode = false;
        private List<Medicine> allMedicines = new List<Medicine>();

        /// <summary>
        /// Constructeur pour l'ajout d'une nouvelle ordonnance
        /// </summary>
        public AddEditPrescriptionForm()
        {
            InitializeComponent();
            this.Text = "Ajouter une ordonnance";
            btnSave.Text = "Ajouter";
            LoadUsers();
            LoadPatients();
            LoadMedicines();
            
            // Configurer le DataGridView pour permettre l'édition
            dgvMedicines.ReadOnly = false;
            dgvMedicines.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dgvMedicines.SelectionMode = DataGridViewSelectionMode.CellSelect;
            
            // Si un utilisateur est connecté, pré-sélectionner son ID
            if (CurrentUser.IsLoggedIn)
            {
                SelectCurrentUser();
            }
        }

        /// <summary>
        /// Constructeur pour la modification d'une ordonnance existante
        /// </summary>
        /// <param name="prescription">L'ordonnance à modifier</param>
        public AddEditPrescriptionForm(Prescription prescription) : this()
        {
            prescriptionToEdit = prescription;
            isEditMode = true;
            this.Text = "Modifier une ordonnance";
            btnSave.Text = "Modifier";
            LoadPrescriptionData();
        }

        /// <summary>
        /// Charge la liste des utilisateurs dans le ComboBox
        /// Si un utilisateur est connecté, pré-sélectionne son ID et le rend non modifiable
        /// </summary>
        private void LoadUsers()
        {
            var users = userDAO.GetAll();
            cmbUserId.Items.Clear();
            
            foreach (var user in users)
            {
                cmbUserId.Items.Add($"{user.Id} - {user.Name} {user.Firstname}");
            }

            // Si un utilisateur est connecté, pré-sélectionner son ID
            if (CurrentUser.IsLoggedIn)
            {
                SelectCurrentUser();
                // Rendre le ComboBox non modifiable pour les utilisateurs normaux
                // (ils ne peuvent créer que leurs propres prescriptions)
                cmbUserId.Enabled = false;
            }
            else if (cmbUserId.Items.Count > 0)
            {
                cmbUserId.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Sélectionne l'utilisateur actuellement connecté dans le ComboBox
        /// </summary>
        private void SelectCurrentUser()
        {
            if (!CurrentUser.IsLoggedIn) return;

            for (int i = 0; i < cmbUserId.Items.Count; i++)
            {
                string item = cmbUserId.Items[i].ToString();
                if (item.StartsWith(CurrentUser.User.Id.ToString()))
                {
                    cmbUserId.SelectedIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        /// Charge la liste des patients dans le ComboBox
        /// Tous les utilisateurs ont accès à tous les patients
        /// </summary>
        private void LoadPatients()
        {
            // Charger tous les patients (tous les utilisateurs ont accès à tous les patients)
            var patients = patientDAO.GetAll();
            
            cmbPatientId.Items.Clear();
            
            foreach (var patient in patients)
            {
                cmbPatientId.Items.Add($"{patient.Id} - {patient.Name} {patient.Firstname}");
            }

            if (cmbPatientId.Items.Count > 0)
            {
                cmbPatientId.SelectedIndex = 0;
                cmbPatientId.Enabled = true;
            }
            else
            {
                // Afficher un message si aucun patient n'est disponible
                cmbPatientId.Items.Add("Aucun patient disponible");
                cmbPatientId.Enabled = false;
            }
        }

        /// <summary>
        /// Charge la liste des médicaments dans le DataGridView
        /// </summary>
        private void LoadMedicines()
        {
            allMedicines = medicineDAO.GetAll();
            dgvMedicines.Columns.Clear();
            dgvMedicines.Rows.Clear();

            // Ajouter une colonne checkbox
            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.HeaderText = "Sélectionner";
            checkColumn.Name = "Selected";
            checkColumn.Width = 80;
            dgvMedicines.Columns.Add(checkColumn);

            // Ajouter une colonne pour l'ID (cachée)
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.HeaderText = "ID";
            idColumn.Name = "Id";
            idColumn.Visible = false;
            dgvMedicines.Columns.Add(idColumn);

            // Ajouter une colonne pour le nom
            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.HeaderText = "Nom";
            nameColumn.Name = "Name";
            nameColumn.ReadOnly = true;
            nameColumn.Width = 200;
            dgvMedicines.Columns.Add(nameColumn);

            // Ajouter une colonne pour la quantité (éditable)
            DataGridViewTextBoxColumn quantityColumn = new DataGridViewTextBoxColumn();
            quantityColumn.HeaderText = "Quantité";
            quantityColumn.Name = "Quantity";
            quantityColumn.Width = 80;
            quantityColumn.ReadOnly = false; // Permettre l'édition
            quantityColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            quantityColumn.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            quantityColumn.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvMedicines.Columns.Add(quantityColumn);
            
            // S'assurer que le DataGridView permet l'édition
            dgvMedicines.ReadOnly = false;
            dgvMedicines.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;

            // Remplir le DataGridView
            foreach (var medicine in allMedicines)
            {
                int rowIndex = dgvMedicines.Rows.Add();
                dgvMedicines.Rows[rowIndex].Cells["Selected"].Value = false;
                dgvMedicines.Rows[rowIndex].Cells["Id"].Value = medicine.Id;
                dgvMedicines.Rows[rowIndex].Cells["Name"].Value = medicine.Name;
                dgvMedicines.Rows[rowIndex].Cells["Quantity"].Value = 1;
                
                // S'assurer que la cellule Quantity est éditable
                dgvMedicines.Rows[rowIndex].Cells["Quantity"].ReadOnly = false;
            }

            dgvMedicines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Charge les données de l'ordonnance à modifier dans les champs du formulaire
        /// </summary>
        private void LoadPrescriptionData()
        {
            if (prescriptionToEdit == null) return;

            dtpValidity.Value = prescriptionToEdit.Validity;

            // Sélectionner l'utilisateur correspondant
            for (int i = 0; i < cmbUserId.Items.Count; i++)
            {
                string item = cmbUserId.Items[i].ToString();
                if (item.StartsWith(prescriptionToEdit.UserId.ToString()))
                {
                    cmbUserId.SelectedIndex = i;
                    break;
                }
            }

            // Sélectionner le patient correspondant
            for (int i = 0; i < cmbPatientId.Items.Count; i++)
            {
                string item = cmbPatientId.Items[i].ToString();
                if (item.StartsWith(prescriptionToEdit.PatientId.ToString()))
                {
                    cmbPatientId.SelectedIndex = i;
                    break;
                }
            }

            // Charger les médicaments associés à cette prescription
            var appartients = appartientDAO.GetByPrescriptionId(prescriptionToEdit.Id);
            foreach (DataGridViewRow row in dgvMedicines.Rows)
            {
                int medicineId = Convert.ToInt32(row.Cells["Id"].Value);
                var appartient = appartients.FirstOrDefault(a => a.IdMedicine == medicineId);
                if (appartient != null)
                {
                    row.Cells["Selected"].Value = true;
                    row.Cells["Quantity"].Value = appartient.Quantity;
                    // S'assurer que la cellule Quantity reste éditable
                    row.Cells["Quantity"].ReadOnly = false;
                }
            }
        }

        /// <summary>
        /// Valide les données saisies avant sauvegarde
        /// </summary>
        private bool ValidateInput()
        {
            // Vérifier l'utilisateur
            int userId = GetSelectedUserId();
            if (userId <= 0)
            {
                if (CurrentUser.IsLoggedIn)
                {
                    MessageBox.Show("Erreur : Aucun utilisateur connecté valide.", 
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur.", 
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbUserId.Focus();
                }
                return false;
            }

            if (cmbPatientId.SelectedIndex == -1 || cmbPatientId.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un patient.", 
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbPatientId.Focus();
                return false;
            }
            
            // Vérifier que ce n'est pas le message "Aucun patient disponible"
            string selectedPatient = cmbPatientId.SelectedItem.ToString();
            if (selectedPatient.Contains("Aucun patient disponible", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Aucun patient disponible. Veuillez d'abord créer un patient.", 
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            
            // Vérifier que l'ID patient est valide
            int patientId = GetSelectedPatientId();
            if (patientId <= 0)
            {
                MessageBox.Show("Le patient sélectionné n'est pas valide.", 
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbPatientId.Focus();
                return false;
            }

            if (dtpValidity.Value < DateTime.Now.Date)
            {
                DialogResult result = MessageBox.Show(
                    "La date de validité est dans le passé. Voulez-vous continuer ?", 
                    "Avertissement", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    dtpValidity.Focus();
                    return false;
                }
            }

            // Vérifier qu'au moins un médicament est sélectionné
            bool hasSelectedMedicine = false;
            string invalidQuantities = "";
            
            foreach (DataGridViewRow row in dgvMedicines.Rows)
            {
                if (row.Cells["Selected"].Value != null && (bool)row.Cells["Selected"].Value)
                {
                    hasSelectedMedicine = true;
                    string medicineName = row.Cells["Name"].Value?.ToString() ?? "Inconnu";
                    
                    // Vérifier que la quantité est valide
                    string quantityText = row.Cells["Quantity"].Value?.ToString() ?? "";
                    if (string.IsNullOrWhiteSpace(quantityText))
                    {
                        invalidQuantities += $"- {medicineName}: quantité vide\n";
                    }
                    else if (!int.TryParse(quantityText, out int qty) || qty <= 0)
                    {
                        invalidQuantities += $"- {medicineName}: quantité invalide (\"{quantityText}\")\n";
                    }
                }
            }

            if (!hasSelectedMedicine)
            {
                MessageBox.Show("Veuillez sélectionner au moins un médicament.", 
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgvMedicines.Focus();
                return false;
            }
            
            if (!string.IsNullOrEmpty(invalidQuantities))
            {
                MessageBox.Show($"Veuillez entrer une quantité valide (nombre entier supérieur à 0) pour tous les médicaments sélectionnés.\n\nErreurs :\n{invalidQuantities}", 
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgvMedicines.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Extrait l'ID utilisateur depuis le ComboBox
        /// </summary>
        private int GetSelectedUserId()
        {
            // Si un utilisateur est connecté, utiliser son ID directement
            if (CurrentUser.IsLoggedIn)
            {
                return CurrentUser.User.Id;
            }
            
            if (cmbUserId.SelectedItem == null) return 0;
            string selected = cmbUserId.SelectedItem.ToString();
            
            if (selected.Contains("-", StringComparison.Ordinal))
            {
                string idPart = selected.Split('-')[0].Trim();
                if (int.TryParse(idPart, out int userId))
                {
            return userId;
                }
            }
            
            return 0;
        }

        /// <summary>
        /// Extrait l'ID patient depuis le ComboBox
        /// </summary>
        private int GetSelectedPatientId()
        {
            if (cmbPatientId.SelectedItem == null) return 0;
            string selected = cmbPatientId.SelectedItem.ToString();
            
            // Vérifier si c'est le message "Aucun patient disponible"
            if (selected.Contains("Aucun patient disponible", StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }
            
            // Extraire l'ID (premier élément avant le '-')
            if (selected.Contains("-", StringComparison.Ordinal))
            {
                string idPart = selected.Split('-')[0].Trim();
                if (int.TryParse(idPart, out int patientId))
                {
            return patientId;
                }
            }
            
            return 0;
        }

        /// <summary>
        /// Gère le clic sur le bouton Enregistrer
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                Prescription prescription;

                if (isEditMode && prescriptionToEdit != null)
                {
                    // Mode modification - créer un nouvel objet Prescription avec les valeurs mises à jour
                    prescription = new Prescription(
                        prescriptionToEdit.Id,
                        GetSelectedUserId(),
                        GetSelectedPatientId(),
                        dtpValidity.Value
                    );

                    if (prescriptionDAO.Update(prescription))
                    {
                        // Supprimer les anciennes associations
                        appartientDAO.DeleteByPrescriptionId(prescription.Id);

                        // Créer les nouvelles associations
                        bool allMedicinesAdded = true;
                        string errorDetails = "";
                        int addedCount = 0;
                        
                        foreach (DataGridViewRow row in dgvMedicines.Rows)
                        {
                            if (row.Cells["Selected"].Value != null && (bool)row.Cells["Selected"].Value)
                            {
                                try
                                {
                                    int medicineId = Convert.ToInt32(row.Cells["Id"].Value);
                                    string quantityText = row.Cells["Quantity"].Value?.ToString() ?? "1";
                                    
                                    if (!int.TryParse(quantityText, out int quantity) || quantity <= 0)
                                    {
                                        quantity = 1; // Valeur par défaut si invalide
                                    }
                                    
                                    Appartient appartient = new Appartient(prescription.Id, medicineId, quantity);
                                    if (appartientDAO.Create(appartient))
                                    {
                                        addedCount++;
                                    }
                                    else
                                    {
                                        allMedicinesAdded = false;
                                        string medicineName = row.Cells["Name"].Value?.ToString() ?? "Inconnu";
                                        errorDetails += $"- {medicineName}: Erreur inconnue\n";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    allMedicinesAdded = false;
                                    string medicineName = row.Cells["Name"].Value?.ToString() ?? "Inconnu";
                                    string errorMsg = ex.Message;
                                    if (ex.InnerException != null)
                                    {
                                        errorMsg = ex.InnerException.Message;
                                    }
                                    
                                    // Message d'aide spécifique pour l'erreur de colonne manquante
                                    if (errorMsg.Contains("Unknown column") || errorMsg.Contains("doesn't exist"))
                                    {
                                        errorDetails += $"- {medicineName}: La colonne 'quantity' n'existe pas dans la table 'appartient'. Exécutez le script 'fix_appartient_table.sql'.\n";
                                    }
                                    else
                                    {
                                        errorDetails += $"- {medicineName}: {errorMsg}\n";
                                    }
                                    
                                    Console.WriteLine($"Erreur lors de l'ajout du médicament {medicineName}: {ex.Message}");
                                    if (ex.InnerException != null)
                                    {
                                        Console.WriteLine($"Détails: {ex.InnerException.Message}");
                                    }
                                }
                            }
                        }

                        if (allMedicinesAdded && addedCount > 0)
                        {
                            MessageBox.Show($"Ordonnance modifiée avec succès !\n{addedCount} médicament(s) associé(s).", 
                            "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        }
                        else if (addedCount > 0)
                        {
                            string message = $"Ordonnance modifiée mais erreur lors de la mise à jour de certains médicaments.\n\n";
                            message += $"Médicaments mis à jour avec succès : {addedCount}\n";
                            if (!string.IsNullOrEmpty(errorDetails))
                            {
                                message += $"\nMédicaments en erreur :\n{errorDetails}";
                            }
                            MessageBox.Show(message, 
                                "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            string message = "Ordonnance modifiée mais aucun médicament n'a pu être associé.\n\n";
                            if (!string.IsNullOrEmpty(errorDetails))
                            {
                                message += $"Erreurs :\n{errorDetails}";
                            }
                            MessageBox.Show(message, 
                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification de l'ordonnance.", 
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Mode ajout - Vérifier les valeurs avant de créer
                    int userId = GetSelectedUserId();
                    int patientId = GetSelectedPatientId();
                    
                    if (userId <= 0)
                    {
                        MessageBox.Show("Erreur : L'ID utilisateur n'est pas valide.", 
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    if (patientId <= 0)
                    {
                        MessageBox.Show("Erreur : L'ID patient n'est pas valide.", 
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    prescription = new Prescription(
                        0, // L'ID sera généré par la base de données
                        userId,
                        patientId,
                        dtpValidity.Value
                    );

                    if (prescriptionDAO.Create(prescription))
                    {
                        // Vérifier que l'ID a bien été récupéré
                        if (prescription.Id <= 0)
                        {
                            MessageBox.Show("Erreur : L'ID de l'ordonnance n'a pas été correctement généré.", 
                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        
                        Console.WriteLine($"Prescription créée avec l'ID: {prescription.Id}");
                        
                        // Créer les entrées dans la table appartient pour chaque médicament sélectionné
                        bool allMedicinesAdded = true;
                        string errorDetails = "";
                        int addedCount = 0;
                        
                        foreach (DataGridViewRow row in dgvMedicines.Rows)
                        {
                            if (row.Cells["Selected"].Value != null && (bool)row.Cells["Selected"].Value)
                            {
                                try
                                {
                                    int medicineId = Convert.ToInt32(row.Cells["Id"].Value);
                                    string quantityText = row.Cells["Quantity"].Value?.ToString() ?? "1";
                                    
                                    if (!int.TryParse(quantityText, out int quantity) || quantity <= 0)
                                    {
                                        quantity = 1; // Valeur par défaut si invalide
                                    }
                                    
                                    Console.WriteLine($"Tentative d'ajout: Prescription ID={prescription.Id}, Medicine ID={medicineId}, Quantity={quantity}");
                                    
                                    Appartient appartient = new Appartient(prescription.Id, medicineId, quantity);
                                    if (appartientDAO.Create(appartient))
                                    {
                                        addedCount++;
                                        Console.WriteLine($"Médicament ajouté avec succès: {row.Cells["Name"].Value}");
                                    }
                                    else
                                    {
                                        allMedicinesAdded = false;
                                        string medicineName = row.Cells["Name"].Value?.ToString() ?? "Inconnu";
                                        errorDetails += $"- {medicineName}: Erreur inconnue (aucune ligne affectée)\n";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    allMedicinesAdded = false;
                                    string medicineName = row.Cells["Name"].Value?.ToString() ?? "Inconnu";
                                    string errorMsg = ex.Message;
                                    if (ex.InnerException != null)
                                    {
                                        errorMsg = ex.InnerException.Message;
                                    }
                                    
                                    // Message d'aide spécifique pour l'erreur de colonne manquante
                                    if (errorMsg.Contains("Unknown column") || errorMsg.Contains("doesn't exist"))
                                    {
                                        errorDetails += $"- {medicineName}: La colonne 'quantity' n'existe pas dans la table 'appartient'. Exécutez le script 'fix_appartient_table.sql'.\n";
                                    }
                                    else
                                    {
                                        errorDetails += $"- {medicineName}: {errorMsg}\n";
                                    }
                                    
                                    Console.WriteLine($"Erreur lors de l'ajout du médicament {medicineName}: {ex.Message}");
                                    if (ex.InnerException != null)
                                    {
                                        Console.WriteLine($"Détails: {ex.InnerException.Message}");
                                    }
                                }
                            }
                        }

                        if (allMedicinesAdded && addedCount > 0)
                        {
                            MessageBox.Show($"Ordonnance ajoutée avec succès !\n{addedCount} médicament(s) associé(s).", 
                            "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        }
                        else if (addedCount > 0)
                        {
                            string message = $"Ordonnance créée mais erreur lors de l'ajout de certains médicaments.\n\n";
                            message += $"Médicaments ajoutés avec succès : {addedCount}\n";
                            if (!string.IsNullOrEmpty(errorDetails))
                            {
                                message += $"\nMédicaments en erreur :\n{errorDetails}";
                            }
                            MessageBox.Show(message, 
                                "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            string message = "Ordonnance créée mais aucun médicament n'a pu être ajouté.\n\n";
                            if (!string.IsNullOrEmpty(errorDetails))
                            {
                                message += $"Erreurs :\n{errorDetails}";
                            }
                            MessageBox.Show(message, 
                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        // Afficher un message d'erreur plus détaillé
                        string errorDetails = $"Erreur lors de l'ajout de l'ordonnance.\n\n";
                        errorDetails += $"Utilisateur ID: {GetSelectedUserId()}\n";
                        errorDetails += $"Patient ID: {GetSelectedPatientId()}\n";
                        errorDetails += $"Date de validité: {dtpValidity.Value:dd/MM/yyyy}\n\n";
                        errorDetails += "Vérifiez:\n";
                        errorDetails += "- Que la base de données est accessible\n";
                        errorDetails += "- Que les IDs utilisateur et patient sont valides\n";
                        errorDetails += "- Consultez la console pour plus de détails";
                        
                        MessageBox.Show(errorDetails, 
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Erreur : {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\n\nDétails : {ex.InnerException.Message}";
                }
                
                // Message d'aide spécifique pour l'erreur de colonne sans valeur par défaut
                if (ex.Message.Contains("doesn't have a default value") || 
                    (ex.InnerException != null && ex.InnerException.Message.Contains("doesn't have a default value")))
                {
                    errorMessage += "\n\n⚠️ SOLUTION :";
                    errorMessage += "\nLa colonne 'id_prescription' n'est pas configurée avec AUTO_INCREMENT.";
                    errorMessage += "\nExécutez le script SQL 'fix_prescription_table.sql' dans votre base de données";
                    errorMessage += "\npour corriger la structure de la table Prescription.";
                }
                
                MessageBox.Show(errorMessage, 
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gère le clic sur le bouton Annuler
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Gère le clic sur le bouton pour ajouter un nouveau patient
        /// </summary>
        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            using (var form = new AddEditPatientForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Recharger la liste des patients
                    LoadPatients();
                    
                    // Sélectionner le dernier patient ajouté (le plus récent)
                    if (cmbPatientId.Items.Count > 0)
                    {
                        cmbPatientId.SelectedIndex = cmbPatientId.Items.Count - 1;
                    }
                }
            }
        }
    }
}

