namespace GSB_Gestion_Stock.Forms
{
    partial class AdminViewForm
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
            pnlTop = new Panel();
            pnlButtons = new Panel();
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
            dgvData.Location = new Point(0, 69);
            dgvData.Margin = new Padding(4, 3, 4, 3);
            dgvData.MultiSelect = false;
            dgvData.Name = "dgvData";
            dgvData.ReadOnly = true;
            dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvData.Size = new Size(1167, 624);
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
            // pnlTop
            // 
            pnlTop.Controls.Add(label1);
            pnlTop.Controls.Add(lblDataType);
            pnlTop.Controls.Add(cmbDataType);
            pnlTop.Controls.Add(lblSearch);
            pnlTop.Controls.Add(txtSearch);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Margin = new Padding(4, 3, 4, 3);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1167, 69);
            pnlTop.TabIndex = 8;
            pnlTop.Paint += pnlTop_Paint;
            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(btnAdd);
            pnlButtons.Controls.Add(btnEdit);
            pnlButtons.Controls.Add(btnDelete);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(0, 693);
            pnlButtons.Margin = new Padding(4, 3, 4, 3);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(1167, 69);
            pnlButtons.TabIndex = 9;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold);
            label1.Location = new Point(1105, 0);
            label1.Name = "label1";
            label1.Size = new Size(62, 26);
            label1.TabIndex = 4;
            label1.Text = "GSB";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AdminViewForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1167, 762);
            Controls.Add(dgvData);
            Controls.Add(pnlButtons);
            Controls.Add(pnlTop);
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(931, 571);
            Name = "AdminViewForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Vue Administrateur - Gestion Stock GSB";
            Load += AdminViewForm_Load;
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
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlButtons;
        private Label label1;
    }
}