namespace GSB_Gestion_Stock.Forms
{
    partial class AddEditPrescriptionForm
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
            this.lblUserId = new System.Windows.Forms.Label();
            this.cmbUserId = new System.Windows.Forms.ComboBox();
            this.lblPatientId = new System.Windows.Forms.Label();
            this.cmbPatientId = new System.Windows.Forms.ComboBox();
            this.lblValidity = new System.Windows.Forms.Label();
            this.dtpValidity = new System.Windows.Forms.DateTimePicker();
            this.btnAddPatient = new System.Windows.Forms.Button();
            this.lblMedicines = new System.Windows.Forms.Label();
            this.dgvMedicines = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMedicines)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblUserId.Location = new System.Drawing.Point(20, 20);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(78, 17);
            this.lblUserId.TabIndex = 0;
            this.lblUserId.Text = "Utilisateur *";
            // 
            // cmbUserId
            // 
            this.cmbUserId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserId.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cmbUserId.FormattingEnabled = true;
            this.cmbUserId.Location = new System.Drawing.Point(150, 17);
            this.cmbUserId.Name = "cmbUserId";
            this.cmbUserId.Size = new System.Drawing.Size(400, 24);
            this.cmbUserId.TabIndex = 1;
            // 
            // lblPatientId
            // 
            this.lblPatientId.AutoSize = true;
            this.lblPatientId.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblPatientId.Location = new System.Drawing.Point(20, 60);
            this.lblPatientId.Name = "lblPatientId";
            this.lblPatientId.Size = new System.Drawing.Size(63, 17);
            this.lblPatientId.TabIndex = 2;
            this.lblPatientId.Text = "Patient *";
            // 
            // cmbPatientId
            // 
            this.cmbPatientId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPatientId.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cmbPatientId.FormattingEnabled = true;
            this.cmbPatientId.Location = new System.Drawing.Point(150, 57);
            this.cmbPatientId.Name = "cmbPatientId";
            this.cmbPatientId.Size = new System.Drawing.Size(320, 24);
            this.cmbPatientId.TabIndex = 3;
            // 
            // btnAddPatient
            // 
            this.btnAddPatient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnAddPatient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddPatient.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnAddPatient.ForeColor = System.Drawing.Color.White;
            this.btnAddPatient.Location = new System.Drawing.Point(476, 57);
            this.btnAddPatient.Name = "btnAddPatient";
            this.btnAddPatient.Size = new System.Drawing.Size(74, 24);
            this.btnAddPatient.TabIndex = 8;
            this.btnAddPatient.Text = "+ Nouveau";
            this.btnAddPatient.UseVisualStyleBackColor = false;
            this.btnAddPatient.Click += new System.EventHandler(this.btnAddPatient_Click);
            // 
            // lblMedicines
            // 
            this.lblMedicines.AutoSize = true;
            this.lblMedicines.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblMedicines.Location = new System.Drawing.Point(20, 140);
            this.lblMedicines.Name = "lblMedicines";
            this.lblMedicines.Size = new System.Drawing.Size(120, 17);
            this.lblMedicines.TabIndex = 9;
            this.lblMedicines.Text = "Médicaments *";
            // 
            // dgvMedicines
            // 
            this.dgvMedicines.AllowUserToAddRows = false;
            this.dgvMedicines.AllowUserToDeleteRows = false;
            this.dgvMedicines.ReadOnly = false;
            this.dgvMedicines.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2;
            this.dgvMedicines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMedicines.Location = new System.Drawing.Point(150, 140);
            this.dgvMedicines.MultiSelect = false;
            this.dgvMedicines.Name = "dgvMedicines";
            this.dgvMedicines.RowHeadersVisible = false;
            this.dgvMedicines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvMedicines.Size = new System.Drawing.Size(400, 200);
            this.dgvMedicines.TabIndex = 10;
            // 
            // lblValidity
            // 
            this.lblValidity.AutoSize = true;
            this.lblValidity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblValidity.Location = new System.Drawing.Point(20, 100);
            this.lblValidity.Name = "lblValidity";
            this.lblValidity.Size = new System.Drawing.Size(108, 17);
            this.lblValidity.TabIndex = 4;
            this.lblValidity.Text = "Date de validité *";
            // 
            // dtpValidity
            // 
            this.dtpValidity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpValidity.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpValidity.Location = new System.Drawing.Point(150, 97);
            this.dtpValidity.Name = "dtpValidity";
            this.dtpValidity.Size = new System.Drawing.Size(200, 23);
            this.dtpValidity.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(300, 15);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 40);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Enregistrer";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(430, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 40);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Annuler";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lblUserId);
            this.pnlMain.Controls.Add(this.cmbUserId);
            this.pnlMain.Controls.Add(this.lblPatientId);
            this.pnlMain.Controls.Add(this.cmbPatientId);
            this.pnlMain.Controls.Add(this.btnAddPatient);
            this.pnlMain.Controls.Add(this.lblValidity);
            this.pnlMain.Controls.Add(this.dtpValidity);
            this.pnlMain.Controls.Add(this.lblMedicines);
            this.pnlMain.Controls.Add(this.dgvMedicines);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(20);
            this.pnlMain.Size = new System.Drawing.Size(580, 360);
            this.pnlMain.TabIndex = 8;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 360);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(580, 70);
            this.pnlButtons.TabIndex = 9;
            // 
            // AddEditPrescriptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 430);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditPrescriptionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ajouter une ordonnance";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMedicines)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.ComboBox cmbUserId;
        private System.Windows.Forms.Label lblPatientId;
        private System.Windows.Forms.ComboBox cmbPatientId;
        private System.Windows.Forms.Button btnAddPatient;
        private System.Windows.Forms.Label lblMedicines;
        private System.Windows.Forms.DataGridView dgvMedicines;
        private System.Windows.Forms.Label lblValidity;
        private System.Windows.Forms.DateTimePicker dtpValidity;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlButtons;
    }
}

