namespace ExchangePSAutomationTest
{
    partial class FormChooseAuthCertificate
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonBrowseForCertificate = new System.Windows.Forms.Button();
            this.textBoxCertificateFile = new System.Windows.Forms.TextBox();
            this.comboBoxStoreCertificates = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonLoadFromFile = new System.Windows.Forms.RadioButton();
            this.radioButtonSelectFromStore = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBoxCertificateInfo = new System.Windows.Forms.ListBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Password:";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPassword.Location = new System.Drawing.Point(98, 118);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(222, 20);
            this.textBoxPassword.TabIndex = 5;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // buttonBrowseForCertificate
            // 
            this.buttonBrowseForCertificate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseForCertificate.Location = new System.Drawing.Point(296, 90);
            this.buttonBrowseForCertificate.Name = "buttonBrowseForCertificate";
            this.buttonBrowseForCertificate.Size = new System.Drawing.Size(24, 23);
            this.buttonBrowseForCertificate.TabIndex = 4;
            this.buttonBrowseForCertificate.Text = "...";
            this.buttonBrowseForCertificate.UseVisualStyleBackColor = true;
            this.buttonBrowseForCertificate.Click += new System.EventHandler(this.buttonBrowseForCertificate_Click);
            // 
            // textBoxCertificateFile
            // 
            this.textBoxCertificateFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCertificateFile.Location = new System.Drawing.Point(25, 92);
            this.textBoxCertificateFile.Name = "textBoxCertificateFile";
            this.textBoxCertificateFile.ReadOnly = true;
            this.textBoxCertificateFile.Size = new System.Drawing.Size(271, 20);
            this.textBoxCertificateFile.TabIndex = 3;
            // 
            // comboBoxStoreCertificates
            // 
            this.comboBoxStoreCertificates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxStoreCertificates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStoreCertificates.FormattingEnabled = true;
            this.comboBoxStoreCertificates.Location = new System.Drawing.Point(25, 42);
            this.comboBoxStoreCertificates.Name = "comboBoxStoreCertificates";
            this.comboBoxStoreCertificates.Size = new System.Drawing.Size(295, 21);
            this.comboBoxStoreCertificates.TabIndex = 2;
            this.comboBoxStoreCertificates.SelectedIndexChanged += new System.EventHandler(this.comboBoxStoreCertificates_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxPassword);
            this.groupBox1.Controls.Add(this.buttonBrowseForCertificate);
            this.groupBox1.Controls.Add(this.textBoxCertificateFile);
            this.groupBox1.Controls.Add(this.comboBoxStoreCertificates);
            this.groupBox1.Controls.Add(this.radioButtonLoadFromFile);
            this.groupBox1.Controls.Add(this.radioButtonSelectFromStore);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(326, 152);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Certificate";
            // 
            // radioButtonLoadFromFile
            // 
            this.radioButtonLoadFromFile.AutoSize = true;
            this.radioButtonLoadFromFile.Location = new System.Drawing.Point(6, 69);
            this.radioButtonLoadFromFile.Name = "radioButtonLoadFromFile";
            this.radioButtonLoadFromFile.Size = new System.Drawing.Size(88, 17);
            this.radioButtonLoadFromFile.TabIndex = 1;
            this.radioButtonLoadFromFile.TabStop = true;
            this.radioButtonLoadFromFile.Text = "Load from file";
            this.radioButtonLoadFromFile.UseVisualStyleBackColor = true;
            this.radioButtonLoadFromFile.CheckedChanged += new System.EventHandler(this.radioButtonLoadFromFile_CheckedChanged);
            // 
            // radioButtonSelectFromStore
            // 
            this.radioButtonSelectFromStore.AutoSize = true;
            this.radioButtonSelectFromStore.Checked = true;
            this.radioButtonSelectFromStore.Location = new System.Drawing.Point(6, 19);
            this.radioButtonSelectFromStore.Name = "radioButtonSelectFromStore";
            this.radioButtonSelectFromStore.Size = new System.Drawing.Size(104, 17);
            this.radioButtonSelectFromStore.TabIndex = 0;
            this.radioButtonSelectFromStore.TabStop = true;
            this.radioButtonSelectFromStore.Text = "Select from store";
            this.radioButtonSelectFromStore.UseVisualStyleBackColor = true;
            this.radioButtonSelectFromStore.CheckedChanged += new System.EventHandler(this.radioButtonSelectFromStore_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.listBoxCertificateInfo);
            this.groupBox2.Location = new System.Drawing.Point(12, 170);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(326, 177);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Certificate Information";
            // 
            // listBoxCertificateInfo
            // 
            this.listBoxCertificateInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxCertificateInfo.FormattingEnabled = true;
            this.listBoxCertificateInfo.Location = new System.Drawing.Point(3, 16);
            this.listBoxCertificateInfo.Name = "listBoxCertificateInfo";
            this.listBoxCertificateInfo.Size = new System.Drawing.Size(320, 158);
            this.listBoxCertificateInfo.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(182, 353);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(263, 353);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // FormChooseAuthCertificate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 384);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(365, 423);
            this.Name = "FormChooseAuthCertificate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Authentication Certificate";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button buttonBrowseForCertificate;
        private System.Windows.Forms.TextBox textBoxCertificateFile;
        private System.Windows.Forms.ComboBox comboBoxStoreCertificates;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonLoadFromFile;
        private System.Windows.Forms.RadioButton radioButtonSelectFromStore;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBoxCertificateInfo;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}