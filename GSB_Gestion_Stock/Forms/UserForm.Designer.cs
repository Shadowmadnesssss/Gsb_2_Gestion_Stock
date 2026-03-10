namespace GSB_Gestion_Stock.Forms
{
    partial class UserForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cmbDataType = new ComboBox();
            lblDataType = new Label();
            txtSearch = new TextBox();
            lblSearch = new Label();
            dgvData = new DataGridView();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnExportPDF = new Button();
            btnExportDossier = new Button(); // Nouveau bouton
            pnlTop = new Panel();
            lblWelcome = new Label();
            pnlButtons = new Panel();
            btnLogout = new Button();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvData).BeginInit();
            pnlTop.SuspendLayout();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // cmbDataType
            // 
            cmbDataType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDataType.Font = new Font("Microsoft Sans Serif", 10F);
            cmbDataType.FormattingEnabled = true;
            cmbDataType.Location = new Point(140, 17);
            cmbDataType.Margin = new Padding(4, 3, 4, 3);
            cmbDataType.Name = "cmbDataType";
            cmbDataType.Size = new Size(233, 24);
            cmbDataType.TabIndex = 0;
            cmbDataType.SelectedIndexChanged += cmbDataType_SelectedIndexChanged;
            // 
            // lblDataType
            // 
            lblDataType.AutoSize = true;
            lblDataType.Font = new Font("Microsoft Sans Serif", 10F);
            lblDataType.Location = new Point(18, 21);
            lblDataType.Margin = new Padding(4, 0, 4, 0);
            lblDataType.Name = "lblDataType";
            lblDataType.Size = new Size(127, 17);
            lblDataType.TabIndex = 1;
            lblDataType.Text = "Type de données :";
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Microsoft Sans Serif", 10F);
            txtSearch.Location = new Point(525, 17);
            txtSearch.Margin = new Padding(4, 3, 4, 3);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(349, 23);
            txtSearch.TabIndex = 2;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Microsoft Sans Serif", 10F);
            lblSearch.Location = new Point(443, 21);
            lblSearch.Margin = new Padding(4, 0, 4, 0);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(85, 17);
            lblSearch.TabIndex = 3;
            lblSearch.Text = "Recherche :";
            // 
            // dgvData
            // 
            dgvData.AllowUserToAddRows = false;
            dgvData.AllowUserToDeleteRows = false;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvData.Dock = DockStyle.Fill;
            dgvData.Location = new Point(0, 115);
            dgvData.Margin = new Padding(4, 3, 4, 3);
            dgvData.MultiSelect = false;
            dgvData.Name = "dgvData";
            dgvData.ReadOnly = true;
            dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvData.Size = new Size(1167, 578);
            dgvData.TabIndex = 4;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(46, 125, 50);
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(18, 12);
            btnAdd.Margin = new Padding(4, 3, 4, 3);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(140, 46);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "➕ Ajouter";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.FromArgb(25, 118, 210);
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(175, 12);
            btnEdit.Margin = new Padding(4, 3, 4, 3);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(140, 46);
            btnEdit.TabIndex = 6;
            btnEdit.Text = "✏️ Modifier";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(211, 47, 47);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(332, 12);
            btnDelete.Margin = new Padding(4, 3, 4, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(140, 46);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "🗑️ Supprimer";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnExportPDF
            // 
            btnExportPDF.BackColor = Color.FromArgb(156, 39, 176);
            btnExportPDF.FlatStyle = FlatStyle.Flat;
            btnExportPDF.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            btnExportPDF.ForeColor = Color.White;
            btnExportPDF.Location = new Point(489, 12);
            btnExportPDF.Margin = new Padding(4, 3, 4, 3);
            btnExportPDF.Name = "btnExportPDF";
            btnExportPDF.Size = new Size(140, 46);
            btnExportPDF.TabIndex = 8;
            btnExportPDF.Text = "📄 Exporter PDF";
            btnExportPDF.UseVisualStyleBackColor = false;
            btnExportPDF.Enabled = true;
            btnExportPDF.Visible = true;
            btnExportPDF.Click += btnExportPDF_Click;
            // 
            // btnExportDossier
            // 
            btnExportDossier.BackColor = Color.FromArgb(255, 152, 0);
            btnExportDossier.FlatStyle = FlatStyle.Flat;
            btnExportDossier.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            btnExportDossier.ForeColor = Color.White;
            btnExportDossier.Location = new Point(646, 12);
            btnExportDossier.Margin = new Padding(4, 3, 4, 3);
            btnExportDossier.Name = "btnExportDossier";
            btnExportDossier.Size = new Size(140, 46);
            btnExportDossier.TabIndex = 10;
            btnExportDossier.Text = "�� Exporter Dossier";
            btnExportDossier.UseVisualStyleBackColor = false;
            btnExportDossier.Enabled = false;
            btnExportDossier.Visible = true;
            btnExportDossier.Click += btnExportDossier_Click;
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(label1);
            pnlTop.Controls.Add(lblWelcome);
            pnlTop.Controls.Add(lblDataType);
            pnlTop.Controls.Add(cmbDataType);
            pnlTop.Controls.Add(lblSearch);
            pnlTop.Controls.Add(txtSearch);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Margin = new Padding(4, 3, 4, 3);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1167, 115);
            pnlTop.TabIndex = 8;
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            lblWelcome.Location = new Point(18, 58);
            lblWelcome.Margin = new Padding(4, 0, 4, 0);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(93, 20);
            lblWelcome.TabIndex = 4;
            lblWelcome.Text = "Bienvenue";
            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(btnLogout);
            pnlButtons.Controls.Add(btnAdd);
            pnlButtons.Controls.Add(btnEdit);
            pnlButtons.Controls.Add(btnDelete);
            pnlButtons.Controls.Add(btnExportPDF);
            pnlButtons.Controls.Add(btnExportDossier); // Ajouter le nouveau bouton
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(0, 693);
            pnlButtons.Margin = new Padding(4, 3, 4, 3);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(1167, 69);
            pnlButtons.TabIndex = 9;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.FromArgb(158, 158, 158);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Microsoft Sans Serif", 10F);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(992, 12);
            btnLogout.Margin = new Padding(4, 3, 4, 3);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(140, 46);
            btnLogout.TabIndex = 9;
            btnLogout.Text = "🚪 Déconnexion";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold);
            label1.Location = new Point(1105, 0);
            label1.Name = "label1";
            label1.Size = new Size(62, 26);
            label1.TabIndex = 5;
            label1.Text = "GSB";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // UserForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1167, 762);
            Controls.Add(dgvData);
            Controls.Add(pnlButtons);
            Controls.Add(pnlTop);
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(931, 571);
            Name = "UserForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Vue Utilisateur - Gestion Stock GSB";
            Load += UserForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvData).EndInit();
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ComboBox cmbDataType;
        private System.Windows.Forms.Label lblDataType;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExportPDF;
        private System.Windows.Forms.Button btnExportDossier; // Nouveau bouton
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnLogout;
        private Label label1;
    }
}
