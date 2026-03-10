using GSB_Gestion_Stock.DAO;
using GSB_Gestion_Stock.Models;
using System;
using System.Windows.Forms;

namespace GSB_Gestion_Stock.Forms
{
    /// <summary>
    /// Formulaire pour ajouter ou modifier un médicament
    /// </summary>
    public partial class AddEditMedicineForm : Form
    {
        private readonly MedicineDAO medicineDAO = new();
        private readonly UserDAO userDAO = new();
        private Medicine? medicineToEdit = null;
        private bool isEditMode = false;

        /// <summary>
        /// Constructeur pour l'ajout d'un nouveau médicament
        /// </summary>
        public AddEditMedicineForm()
        {
            InitializeComponent();
            this.Text = "Ajouter un médicament";
            btnSave.Text = "Ajouter";
            LoadUsers();
            
            // Si un utilisateur est connecté, pré-sélectionner son ID
            if (CurrentUser.IsLoggedIn)
            {
                SelectCurrentUser();
            }
        }

        /// <summary>
        /// Constructeur pour la modification d'un médicament existant
        /// </summary>
        /// <param name="medicine">Le médicament à modifier</param>
        public AddEditMedicineForm(Medicine medicine) : this()
        {
            medicineToEdit = medicine;
            isEditMode = true;
            this.Text = "Modifier un médicament";
            btnSave.Text = "Modifier";
            LoadMedicineData();
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
        /// Charge les données du médicament à modifier dans les champs du formulaire
        /// </summary>
        private void LoadMedicineData()
        {
            if (medicineToEdit == null) return;

            txtName.Text = medicineToEdit.Name ?? "";
            txtDosage.Text = medicineToEdit.Dosage ?? "";
            txtMolecule.Text = medicineToEdit.Molecule ?? "";
            txtDescription.Text = medicineToEdit.Description ?? "";

            // Sélectionner l'utilisateur dans le ComboBox
            for (int i = 0; i < cmbUserId.Items.Count; i++)
            {
                string item = cmbUserId.Items[i].ToString();
                if (item.StartsWith(medicineToEdit.UserId.ToString()))
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
                MessageBox.Show("Le nom du médicament est obligatoire.", 
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDosage.Text))
            {
                MessageBox.Show("Le dosage est obligatoire.", 
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDosage.Focus();
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

                Medicine medicine = new Medicine
                {
                    Name = txtName.Text.Trim(),
                    Dosage = txtDosage.Text.Trim(),
                    Molecule = txtMolecule.Text?.Trim() ?? "",
                    Description = txtDescription.Text?.Trim() ?? "",
                    UserId = userId
                };

                if (isEditMode && medicineToEdit != null)
                {
                    // Mode modification
                    medicine.Id = medicineToEdit.Id;
                    if (medicineDAO.Update(medicine))
                    {
                        MessageBox.Show("Médicament modifié avec succès.", 
                            "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification du médicament.", 
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Mode ajout
                    if (medicineDAO.Create(medicine))
                    {
                        MessageBox.Show("Médicament ajouté avec succès.", 
                            "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout du médicament.", 
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
