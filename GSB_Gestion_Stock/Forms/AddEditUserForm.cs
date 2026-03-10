using GSB_Gestion_Stock.DAO;
using GSB_Gestion_Stock.Models;
using System;
using System.Windows.Forms;

namespace GSB_Gestion_Stock.Forms
{
    /// <summary>
    /// Formulaire pour ajouter ou modifier un utilisateur (réservé aux admins)
    /// </summary>
    public partial class AddEditUserForm : Form
    {
        private readonly UserDAO userDAO = new();
        private User? userToEdit = null;
        private bool isEditMode = false;

        /// <summary>
        /// Constructeur pour l'ajout d'un nouvel utilisateur
        /// </summary>
        public AddEditUserForm()
        {
            InitializeComponent();
            this.Text = "Ajouter un utilisateur";
            btnSave.Text = "Ajouter";
        }

        /// <summary>
        /// Constructeur pour la modification d'un utilisateur existant
        /// </summary>
        /// <param name="user">L'utilisateur à modifier</param>
        public AddEditUserForm(User user) : this()
        {
            userToEdit = user;
            isEditMode = true;
            this.Text = "Modifier un utilisateur";
            btnSave.Text = "Modifier";
            LoadUserData();
        }

        /// <summary>
        /// Charge les données de l'utilisateur à modifier dans les champs du formulaire
        /// </summary>
        private void LoadUserData()
        {
            if (userToEdit == null) return;

            txtName.Text = userToEdit.Name ?? "";
            txtFirstname.Text = userToEdit.Firstname ?? "";
            chkIsAdmin.Checked = userToEdit.Role;
            
            // Récupérer l'email depuis la base de données
            string email = userDAO.GetEmailById(userToEdit.Id);
            txtEmail.Text = email ?? "";
            
            txtPassword.Text = ""; // Le mot de passe n'est jamais affiché pour des raisons de sécurité
            txtPassword.Enabled = true; // Permettre la modification du mot de passe
            lblPassword.Text = "Mot de passe (laisser vide pour ne pas modifier)";
        }

        /// <summary>
        /// Valide les données saisies
        /// </summary>
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Le nom de l'utilisateur est obligatoire.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFirstname.Text))
            {
                MessageBox.Show("Le prénom de l'utilisateur est obligatoire.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFirstname.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("L'email de l'utilisateur est obligatoire.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Vérifier le format de l'email (basique)
            if (!txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Veuillez saisir un email valide.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // En mode ajout, le mot de passe est obligatoire
            if (!isEditMode && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Le mot de passe est obligatoire pour un nouvel utilisateur.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            return true;
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
                User user = new User
                {
                    Name = txtName.Text.Trim(),
                    Firstname = txtFirstname.Text.Trim(),
                    Role = chkIsAdmin.Checked
                };

                if (isEditMode && userToEdit != null)
                {
                    // Mode modification
                    user.Id = userToEdit.Id;
                    
                    // Si un nouveau mot de passe est fourni, on le met à jour
                    string password = string.IsNullOrWhiteSpace(txtPassword.Text) ? null : txtPassword.Text.Trim();
                    
                    if (userDAO.Update(user, txtEmail.Text.Trim(), password))
                    {
                        MessageBox.Show("Utilisateur modifié avec succès.",
                            "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification de l'utilisateur.",
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Mode ajout
                    if (userDAO.Create(user, txtEmail.Text.Trim(), txtPassword.Text.Trim()))
                    {
                        MessageBox.Show("Utilisateur ajouté avec succès.",
                            "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout de l'utilisateur.",
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

