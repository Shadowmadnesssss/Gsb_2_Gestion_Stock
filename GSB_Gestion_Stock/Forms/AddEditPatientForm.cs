using GSB_Gestion_Stock.DAO;
using GSB_Gestion_Stock.Models;
using System;
using System.Windows.Forms;

namespace GSB_Gestion_Stock.Forms
{
    /// <summary>
    /// Formulaire pour ajouter ou modifier un patient
    /// </summary>
    public partial class AddEditPatientForm : Form
    {
        private readonly PatientDAO patientDAO = new();
        private readonly UserDAO userDAO = new();
        private Patient? patientToEdit = null;
        private bool isEditMode = false;

        /// <summary>
        /// Constructeur pour l'ajout d'un nouveau patient
        /// </summary>
        public AddEditPatientForm()
        {
            InitializeComponent();
            this.Text = "Ajouter un patient";
            btnSave.Text = "Ajouter";
            LoadUsers();
            
            // Si un utilisateur est connecté, pré-sélectionner son ID et le rendre non modifiable
            if (CurrentUser.IsLoggedIn)
            {
                SelectCurrentUser();
                // Les utilisateurs normaux ne peuvent créer que leurs propres patients
                cmbUserId.Enabled = false;
            }
        }

        /// <summary>
        /// Constructeur pour la modification d'un patient existant
        /// </summary>
        /// <param name="patient">Le patient à modifier</param>
        public AddEditPatientForm(Patient patient) : this()
        {
            patientToEdit = patient;
            isEditMode = true;
            this.Text = "Modifier un patient";
            btnSave.Text = "Modifier";
            LoadPatientData();
        }

        /// <summary>
        /// Charge la liste des utilisateurs dans le ComboBox
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
        /// Charge les données du patient à modifier dans les champs du formulaire
        /// </summary>
        private void LoadPatientData()
        {
            if (patientToEdit == null) return;

            txtName.Text = patientToEdit.Name ?? "";
            txtFirstname.Text = patientToEdit.Firstname ?? "";
            numAge.Value = patientToEdit.Age > 0 ? patientToEdit.Age : 1;
            
            // Sélectionner le genre dans le ComboBox
            if (!string.IsNullOrEmpty(patientToEdit.Gender))
            {
                for (int i = 0; i < cmbGender.Items.Count; i++)
                {
                    if (cmbGender.Items[i].ToString() == patientToEdit.Gender)
                    {
                        cmbGender.SelectedIndex = i;
                        break;
                    }
                }
            }

            // Sélectionner l'utilisateur dans le ComboBox
            for (int i = 0; i < cmbUserId.Items.Count; i++)
            {
                string item = cmbUserId.Items[i].ToString();
                if (item.StartsWith(patientToEdit.UserId.ToString()))
                {
                    cmbUserId.SelectedIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        /// Valide les données saisies
        /// </summary>
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Le nom du patient est obligatoire.", 
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFirstname.Text))
            {
                MessageBox.Show("Le prénom du patient est obligatoire.", 
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFirstname.Focus();
                return false;
            }

            if (numAge.Value < 1)
            {
                MessageBox.Show("L'âge doit être supérieur à 0.", 
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numAge.Focus();
                return false;
            }

            if (cmbGender.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez sélectionner un genre.", 
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbGender.Focus();
                return false;
            }

            if (cmbUserId.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur.", 
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbUserId.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Récupère l'ID de l'utilisateur sélectionné dans le ComboBox
        /// </summary>
        private int GetSelectedUserId()
        {
            if (cmbUserId.SelectedIndex < 0) return 0;

            string selectedItem = cmbUserId.SelectedItem.ToString();
            if (string.IsNullOrEmpty(selectedItem)) return 0;

            // Extraire l'ID (format: "ID - Nom Prénom")
            string[] parts = selectedItem.Split('-');
            if (parts.Length > 0 && int.TryParse(parts[0].Trim(), out int userId))
            {
                return userId;
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
                int userId = GetSelectedUserId();
                if (userId <= 0)
                {
                    MessageBox.Show("Erreur : Impossible de récupérer l'ID de l'utilisateur.", 
                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Patient patient = new Patient
                {
                    Name = txtName.Text.Trim(),
                    Firstname = txtFirstname.Text.Trim(),
                    Age = (int)numAge.Value,
                    Gender = cmbGender.SelectedItem?.ToString() ?? "",
                    UserId = userId
                };

                if (isEditMode && patientToEdit != null)
                {
                    // Mode modification
                    patient.Id = patientToEdit.Id;
                    if (patientDAO.Update(patient))
                    {
                        MessageBox.Show("Patient modifié avec succès.", 
                            "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification du patient.", 
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Mode ajout
                    if (patientDAO.Create(patient))
                    {
                        MessageBox.Show("Patient ajouté avec succès.", 
                            "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout du patient.", 
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}\n\nDétails : {ex.StackTrace}", 
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
    }
}

