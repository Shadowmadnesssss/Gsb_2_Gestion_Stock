using System;
using System.Windows.Forms;
using GSB_Gestion_Stock.DAO;
using GSB_Gestion_Stock.Models;
using GSB_Gestion_Stock.Utils;

namespace GSB_Gestion_Stock.Forms
{
    public partial class LoginForm : Form
    {
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblEmail;
        private Label lblPassword;
        private Label label1;
        private Label lblTitle;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            lblEmail = new Label();
            lblPassword = new Label();
            lblTitle = new Label();
            label1 = new Label();
            SuspendLayout();
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(50, 120);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(300, 23);
            txtEmail.TabIndex = 0;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(50, 180);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(300, 23);
            txtPassword.TabIndex = 1;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(150, 230);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(100, 30);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Se connecter";
            btnLogin.Click += BtnLogin_Click;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(50, 100);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(39, 15);
            lblEmail.TabIndex = 1;
            lblEmail.Text = "Email:";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(50, 160);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(80, 15);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Mot de passe:";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold);
            lblTitle.Location = new Point(134, 56);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(125, 26);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Connexion";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold);
            label1.Location = new Point(165, 9);
            label1.Name = "label1";
            label1.Size = new Size(62, 26);
            label1.TabIndex = 3;
            label1.Text = "GSB";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LoginForm
            // 
            ClientSize = new Size(400, 300);
            Controls.Add(label1);
            Controls.Add(lblTitle);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GSB - Connexion";
            ResumeLayout(false);
            PerformLayout();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Envoyer le mot de passe en clair, Login gérera le hachage si nécessaire
                UserDAO userDAO = new UserDAO();
                var user = userDAO.Login(email, password);

                if (user != null)
                {
                    // Stocker l'utilisateur connecté
                    CurrentUser.User = user;
                    
                    MessageBox.Show($"Bienvenue {user.Firstname} {user.Name} !", "Connexion réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Rediriger selon le rôle
                    if (user.Role) // Admin
                    {
                        // Ouvrir la vue administrateur
                        AdminViewForm adminForm = new AdminViewForm();
                        adminForm.Show();
                    }
                    else // Utilisateur normal
                    {
                        // Ouvrir la vue utilisateur
                        UserForm userForm = new UserForm();
                        userForm.Show();
                    }
                    
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Email ou mot de passe incorrect.", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtEmail.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la connexion : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

