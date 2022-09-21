namespace ExchangePSAutomationTest
{
    partial class FormMain
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
            this.textBoxPowerShell = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonRunPowerShell = new System.Windows.Forms.Button();
            this.checkBoxProcessAsCommand = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.buttonClearCredentials = new System.Windows.Forms.Button();
            this.buttonRemoveCredential = new System.Windows.Forms.Button();
            this.buttonAddCredential = new System.Windows.Forms.Button();
            this.listBoxVariables = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageOutput = new System.Windows.Forms.TabPage();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.tabPageErrors = new System.Windows.Forms.TabPage();
            this.textBoxErrors = new System.Windows.Forms.TextBox();
            this.tabPageVerbose = new System.Windows.Forms.TabPage();
            this.textBoxVerbose = new System.Windows.Forms.TextBox();
            this.tabPageOptions = new System.Windows.Forms.TabPage();
            this.buttonClearLogs = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPagePowerShell = new System.Windows.Forms.TabPage();
            this.checkBoxEXOv2 = new System.Windows.Forms.CheckBox();
            this.checkBoxOffice365 = new System.Windows.Forms.CheckBox();
            this.checkBoxIgnoreSSLErrors = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowRedirection = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxAuthMethod = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxExchangePSUrl = new System.Windows.Forms.TextBox();
            this.radioButtonUseRemotePowerShell = new System.Windows.Forms.RadioButton();
            this.radioButtonUseLocalPowerShell = new System.Windows.Forms.RadioButton();
            this.tabPageAuthv1 = new System.Windows.Forms.TabPage();
            this.checkBoxUseCertificateThumprint = new System.Windows.Forms.CheckBox();
            this.buttonChooseCertificate = new System.Windows.Forms.Button();
            this.textBoxAuthCertificate = new System.Windows.Forms.TextBox();
            this.radioButtonCertificateCredential = new System.Windows.Forms.RadioButton();
            this.radioButtonSpecificCredentials = new System.Windows.Forms.RadioButton();
            this.radioButtonDefaultCredentials = new System.Windows.Forms.RadioButton();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.timerTaskMonitor = new System.Windows.Forms.Timer(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageOutput.SuspendLayout();
            this.tabPageErrors.SuspendLayout();
            this.tabPageVerbose.SuspendLayout();
            this.tabPageOptions.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPagePowerShell.SuspendLayout();
            this.tabPageAuthv1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxPowerShell
            // 
            this.textBoxPowerShell.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPowerShell.Location = new System.Drawing.Point(3, 16);
            this.textBoxPowerShell.Multiline = true;
            this.textBoxPowerShell.Name = "textBoxPowerShell";
            this.textBoxPowerShell.Size = new System.Drawing.Size(539, 153);
            this.textBoxPowerShell.TabIndex = 4;
            this.textBoxPowerShell.Text = "Get-Mailbox";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.buttonRunPowerShell);
            this.groupBox2.Controls.Add(this.checkBoxProcessAsCommand);
            this.groupBox2.Controls.Add(this.textBoxPowerShell);
            this.groupBox2.Location = new System.Drawing.Point(12, 105);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(548, 204);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PowerShell Script";
            // 
            // buttonRunPowerShell
            // 
            this.buttonRunPowerShell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRunPowerShell.Location = new System.Drawing.Point(460, 175);
            this.buttonRunPowerShell.Name = "buttonRunPowerShell";
            this.buttonRunPowerShell.Size = new System.Drawing.Size(82, 23);
            this.buttonRunPowerShell.TabIndex = 7;
            this.buttonRunPowerShell.Text = "Run script";
            this.buttonRunPowerShell.UseVisualStyleBackColor = true;
            this.buttonRunPowerShell.Click += new System.EventHandler(this.buttonRunPowerShell_Click);
            // 
            // checkBoxProcessAsCommand
            // 
            this.checkBoxProcessAsCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxProcessAsCommand.AutoSize = true;
            this.checkBoxProcessAsCommand.Location = new System.Drawing.Point(3, 179);
            this.checkBoxProcessAsCommand.Name = "checkBoxProcessAsCommand";
            this.checkBoxProcessAsCommand.Size = new System.Drawing.Size(217, 17);
            this.checkBoxProcessAsCommand.TabIndex = 5;
            this.checkBoxProcessAsCommand.Text = "Process each line as separate command";
            this.checkBoxProcessAsCommand.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.buttonClearCredentials);
            this.groupBox6.Controls.Add(this.buttonRemoveCredential);
            this.groupBox6.Controls.Add(this.buttonAddCredential);
            this.groupBox6.Controls.Add(this.listBoxVariables);
            this.groupBox6.Location = new System.Drawing.Point(566, 105);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(303, 204);
            this.groupBox6.TabIndex = 11;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Script Variables";
            // 
            // buttonClearCredentials
            // 
            this.buttonClearCredentials.Enabled = false;
            this.buttonClearCredentials.Location = new System.Drawing.Point(228, 175);
            this.buttonClearCredentials.Name = "buttonClearCredentials";
            this.buttonClearCredentials.Size = new System.Drawing.Size(69, 23);
            this.buttonClearCredentials.TabIndex = 3;
            this.buttonClearCredentials.Text = "Clear";
            this.buttonClearCredentials.UseVisualStyleBackColor = true;
            // 
            // buttonRemoveCredential
            // 
            this.buttonRemoveCredential.Enabled = false;
            this.buttonRemoveCredential.Location = new System.Drawing.Point(228, 48);
            this.buttonRemoveCredential.Name = "buttonRemoveCredential";
            this.buttonRemoveCredential.Size = new System.Drawing.Size(69, 23);
            this.buttonRemoveCredential.TabIndex = 2;
            this.buttonRemoveCredential.Text = "Remove";
            this.buttonRemoveCredential.UseVisualStyleBackColor = true;
            // 
            // buttonAddCredential
            // 
            this.buttonAddCredential.Location = new System.Drawing.Point(228, 19);
            this.buttonAddCredential.Name = "buttonAddCredential";
            this.buttonAddCredential.Size = new System.Drawing.Size(69, 23);
            this.buttonAddCredential.TabIndex = 1;
            this.buttonAddCredential.Text = "Add...";
            this.buttonAddCredential.UseVisualStyleBackColor = true;
            this.buttonAddCredential.Click += new System.EventHandler(this.buttonAddCredential_Click);
            // 
            // listBoxVariables
            // 
            this.listBoxVariables.FormattingEnabled = true;
            this.listBoxVariables.Location = new System.Drawing.Point(6, 19);
            this.listBoxVariables.Name = "listBoxVariables";
            this.listBoxVariables.Size = new System.Drawing.Size(215, 173);
            this.listBoxVariables.Sorted = true;
            this.listBoxVariables.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageOutput);
            this.tabControl1.Controls.Add(this.tabPageErrors);
            this.tabControl1.Controls.Add(this.tabPageVerbose);
            this.tabControl1.Controls.Add(this.tabPageOptions);
            this.tabControl1.Location = new System.Drawing.Point(12, 315);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(858, 221);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPageOutput
            // 
            this.tabPageOutput.Controls.Add(this.textBoxOutput);
            this.tabPageOutput.Location = new System.Drawing.Point(4, 22);
            this.tabPageOutput.Name = "tabPageOutput";
            this.tabPageOutput.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPageOutput.Size = new System.Drawing.Size(850, 195);
            this.tabPageOutput.TabIndex = 0;
            this.tabPageOutput.Text = "Output";
            this.tabPageOutput.UseVisualStyleBackColor = true;
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOutput.Location = new System.Drawing.Point(3, 3);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ReadOnly = true;
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxOutput.Size = new System.Drawing.Size(844, 189);
            this.textBoxOutput.TabIndex = 1;
            // 
            // tabPageErrors
            // 
            this.tabPageErrors.Controls.Add(this.textBoxErrors);
            this.tabPageErrors.Location = new System.Drawing.Point(4, 22);
            this.tabPageErrors.Name = "tabPageErrors";
            this.tabPageErrors.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPageErrors.Size = new System.Drawing.Size(850, 195);
            this.tabPageErrors.TabIndex = 1;
            this.tabPageErrors.Text = "Errors";
            this.tabPageErrors.UseVisualStyleBackColor = true;
            // 
            // textBoxErrors
            // 
            this.textBoxErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxErrors.Location = new System.Drawing.Point(3, 3);
            this.textBoxErrors.Multiline = true;
            this.textBoxErrors.Name = "textBoxErrors";
            this.textBoxErrors.ReadOnly = true;
            this.textBoxErrors.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxErrors.Size = new System.Drawing.Size(844, 189);
            this.textBoxErrors.TabIndex = 1;
            // 
            // tabPageVerbose
            // 
            this.tabPageVerbose.Controls.Add(this.textBoxVerbose);
            this.tabPageVerbose.Location = new System.Drawing.Point(4, 22);
            this.tabPageVerbose.Name = "tabPageVerbose";
            this.tabPageVerbose.Size = new System.Drawing.Size(850, 195);
            this.tabPageVerbose.TabIndex = 2;
            this.tabPageVerbose.Text = "Verbose";
            this.tabPageVerbose.UseVisualStyleBackColor = true;
            // 
            // textBoxVerbose
            // 
            this.textBoxVerbose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVerbose.Location = new System.Drawing.Point(0, 0);
            this.textBoxVerbose.Multiline = true;
            this.textBoxVerbose.Name = "textBoxVerbose";
            this.textBoxVerbose.ReadOnly = true;
            this.textBoxVerbose.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxVerbose.Size = new System.Drawing.Size(850, 195);
            this.textBoxVerbose.TabIndex = 1;
            // 
            // tabPageOptions
            // 
            this.tabPageOptions.Controls.Add(this.buttonClearLogs);
            this.tabPageOptions.Location = new System.Drawing.Point(4, 22);
            this.tabPageOptions.Name = "tabPageOptions";
            this.tabPageOptions.Size = new System.Drawing.Size(850, 195);
            this.tabPageOptions.TabIndex = 3;
            this.tabPageOptions.Text = "Options";
            this.tabPageOptions.UseVisualStyleBackColor = true;
            // 
            // buttonClearLogs
            // 
            this.buttonClearLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearLogs.Location = new System.Drawing.Point(393, 77);
            this.buttonClearLogs.Name = "buttonClearLogs";
            this.buttonClearLogs.Size = new System.Drawing.Size(119, 21);
            this.buttonClearLogs.TabIndex = 14;
            this.buttonClearLogs.Text = "Clear log windows";
            this.buttonClearLogs.UseVisualStyleBackColor = true;
            this.buttonClearLogs.Click += new System.EventHandler(this.buttonClearLogs_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPagePowerShell);
            this.tabControl2.Controls.Add(this.tabPageAuthv1);
            this.tabControl2.Location = new System.Drawing.Point(12, 12);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(859, 87);
            this.tabControl2.TabIndex = 13;
            // 
            // tabPagePowerShell
            // 
            this.tabPagePowerShell.Controls.Add(this.checkBoxEXOv2);
            this.tabPagePowerShell.Controls.Add(this.checkBoxOffice365);
            this.tabPagePowerShell.Controls.Add(this.checkBoxIgnoreSSLErrors);
            this.tabPagePowerShell.Controls.Add(this.checkBoxAllowRedirection);
            this.tabPagePowerShell.Controls.Add(this.label4);
            this.tabPagePowerShell.Controls.Add(this.comboBoxAuthMethod);
            this.tabPagePowerShell.Controls.Add(this.label1);
            this.tabPagePowerShell.Controls.Add(this.textBoxExchangePSUrl);
            this.tabPagePowerShell.Controls.Add(this.radioButtonUseRemotePowerShell);
            this.tabPagePowerShell.Controls.Add(this.radioButtonUseLocalPowerShell);
            this.tabPagePowerShell.Location = new System.Drawing.Point(4, 22);
            this.tabPagePowerShell.Name = "tabPagePowerShell";
            this.tabPagePowerShell.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPagePowerShell.Size = new System.Drawing.Size(851, 61);
            this.tabPagePowerShell.TabIndex = 1;
            this.tabPagePowerShell.Text = "PowerShell";
            this.tabPagePowerShell.UseVisualStyleBackColor = true;
            // 
            // checkBoxEXOv2
            // 
            this.checkBoxEXOv2.AutoSize = true;
            this.checkBoxEXOv2.Checked = true;
            this.checkBoxEXOv2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEXOv2.Location = new System.Drawing.Point(495, 9);
            this.checkBoxEXOv2.Name = "checkBoxEXOv2";
            this.checkBoxEXOv2.Size = new System.Drawing.Size(44, 17);
            this.checkBoxEXOv2.TabIndex = 17;
            this.checkBoxEXOv2.Text = "v2+";
            this.checkBoxEXOv2.UseVisualStyleBackColor = true;
            this.checkBoxEXOv2.CheckedChanged += new System.EventHandler(this.checkBoxEXOv2_CheckedChanged);
            // 
            // checkBoxOffice365
            // 
            this.checkBoxOffice365.AutoSize = true;
            this.checkBoxOffice365.Location = new System.Drawing.Point(385, 9);
            this.checkBoxOffice365.Name = "checkBoxOffice365";
            this.checkBoxOffice365.Size = new System.Drawing.Size(107, 17);
            this.checkBoxOffice365.TabIndex = 16;
            this.checkBoxOffice365.Text = "Exchange Online";
            this.checkBoxOffice365.UseVisualStyleBackColor = true;
            this.checkBoxOffice365.CheckedChanged += new System.EventHandler(this.checkBoxOffice365_CheckedChanged);
            // 
            // checkBoxIgnoreSSLErrors
            // 
            this.checkBoxIgnoreSSLErrors.AutoSize = true;
            this.checkBoxIgnoreSSLErrors.Enabled = false;
            this.checkBoxIgnoreSSLErrors.Location = new System.Drawing.Point(291, 32);
            this.checkBoxIgnoreSSLErrors.Name = "checkBoxIgnoreSSLErrors";
            this.checkBoxIgnoreSSLErrors.Size = new System.Drawing.Size(134, 17);
            this.checkBoxIgnoreSSLErrors.TabIndex = 14;
            this.checkBoxIgnoreSSLErrors.Text = "Ignore certificate errors";
            this.checkBoxIgnoreSSLErrors.UseVisualStyleBackColor = true;
            // 
            // checkBoxAllowRedirection
            // 
            this.checkBoxAllowRedirection.AutoSize = true;
            this.checkBoxAllowRedirection.Checked = true;
            this.checkBoxAllowRedirection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAllowRedirection.Location = new System.Drawing.Point(431, 32);
            this.checkBoxAllowRedirection.Name = "checkBoxAllowRedirection";
            this.checkBoxAllowRedirection.Size = new System.Drawing.Size(103, 17);
            this.checkBoxAllowRedirection.TabIndex = 13;
            this.checkBoxAllowRedirection.Text = "Allow redirection";
            this.checkBoxAllowRedirection.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Authentication method:";
            // 
            // comboBoxAuthMethod
            // 
            this.comboBoxAuthMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAuthMethod.FormattingEnabled = true;
            this.comboBoxAuthMethod.Items.AddRange(new object[] {
            "Default",
            "Basic",
            "CredSSP",
            "Digest",
            "Kerberos",
            "Negotiate",
            "NegotiateWithImplicitCredential"});
            this.comboBoxAuthMethod.Location = new System.Drawing.Point(128, 32);
            this.comboBoxAuthMethod.Name = "comboBoxAuthMethod";
            this.comboBoxAuthMethod.Size = new System.Drawing.Size(157, 21);
            this.comboBoxAuthMethod.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Exchange PS Url:";
            // 
            // textBoxExchangePSUrl
            // 
            this.textBoxExchangePSUrl.Location = new System.Drawing.Point(103, 6);
            this.textBoxExchangePSUrl.Name = "textBoxExchangePSUrl";
            this.textBoxExchangePSUrl.Size = new System.Drawing.Size(276, 20);
            this.textBoxExchangePSUrl.TabIndex = 9;
            this.textBoxExchangePSUrl.Text = "http://exchange/powershell";
            // 
            // radioButtonUseRemotePowerShell
            // 
            this.radioButtonUseRemotePowerShell.AutoSize = true;
            this.radioButtonUseRemotePowerShell.Location = new System.Drawing.Point(554, 31);
            this.radioButtonUseRemotePowerShell.Name = "radioButtonUseRemotePowerShell";
            this.radioButtonUseRemotePowerShell.Size = new System.Drawing.Size(291, 17);
            this.radioButtonUseRemotePowerShell.TabIndex = 8;
            this.radioButtonUseRemotePowerShell.Text = "Use remote PowerShell session (cmdlets all run remotely)";
            this.radioButtonUseRemotePowerShell.UseVisualStyleBackColor = true;
            this.radioButtonUseRemotePowerShell.CheckedChanged += new System.EventHandler(this.radioButtonUseRemotePowerShell_CheckedChanged);
            // 
            // radioButtonUseLocalPowerShell
            // 
            this.radioButtonUseLocalPowerShell.AutoSize = true;
            this.radioButtonUseLocalPowerShell.Checked = true;
            this.radioButtonUseLocalPowerShell.Location = new System.Drawing.Point(554, 8);
            this.radioButtonUseLocalPowerShell.Name = "radioButtonUseLocalPowerShell";
            this.radioButtonUseLocalPowerShell.Size = new System.Drawing.Size(273, 17);
            this.radioButtonUseLocalPowerShell.TabIndex = 7;
            this.radioButtonUseLocalPowerShell.TabStop = true;
            this.radioButtonUseLocalPowerShell.Text = "Use local PowerShell session (import remote session)";
            this.radioButtonUseLocalPowerShell.UseVisualStyleBackColor = true;
            this.radioButtonUseLocalPowerShell.CheckedChanged += new System.EventHandler(this.radioButtonUseLocalPowerShell_CheckedChanged);
            // 
            // tabPageAuthv1
            // 
            this.tabPageAuthv1.Controls.Add(this.checkBoxUseCertificateThumprint);
            this.tabPageAuthv1.Controls.Add(this.buttonChooseCertificate);
            this.tabPageAuthv1.Controls.Add(this.textBoxAuthCertificate);
            this.tabPageAuthv1.Controls.Add(this.radioButtonCertificateCredential);
            this.tabPageAuthv1.Controls.Add(this.radioButtonSpecificCredentials);
            this.tabPageAuthv1.Controls.Add(this.radioButtonDefaultCredentials);
            this.tabPageAuthv1.Controls.Add(this.labelPassword);
            this.tabPageAuthv1.Controls.Add(this.textBoxUsername);
            this.tabPageAuthv1.Controls.Add(this.textBoxPassword);
            this.tabPageAuthv1.Controls.Add(this.labelUsername);
            this.tabPageAuthv1.Location = new System.Drawing.Point(4, 22);
            this.tabPageAuthv1.Name = "tabPageAuthv1";
            this.tabPageAuthv1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPageAuthv1.Size = new System.Drawing.Size(851, 61);
            this.tabPageAuthv1.TabIndex = 0;
            this.tabPageAuthv1.Text = "Authentication";
            this.tabPageAuthv1.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseCertificateThumprint
            // 
            this.checkBoxUseCertificateThumprint.AutoSize = true;
            this.checkBoxUseCertificateThumprint.Location = new System.Drawing.Point(494, 6);
            this.checkBoxUseCertificateThumprint.Name = "checkBoxUseCertificateThumprint";
            this.checkBoxUseCertificateThumprint.Size = new System.Drawing.Size(244, 17);
            this.checkBoxUseCertificateThumprint.TabIndex = 29;
            this.checkBoxUseCertificateThumprint.Text = "Specify certificate using -Thumprint parameter:";
            this.checkBoxUseCertificateThumprint.UseVisualStyleBackColor = true;
            this.checkBoxUseCertificateThumprint.CheckedChanged += new System.EventHandler(this.checkBoxUseCertificateThumprint_CheckedChanged);
            // 
            // buttonChooseCertificate
            // 
            this.buttonChooseCertificate.Location = new System.Drawing.Point(773, 29);
            this.buttonChooseCertificate.Name = "buttonChooseCertificate";
            this.buttonChooseCertificate.Size = new System.Drawing.Size(75, 20);
            this.buttonChooseCertificate.TabIndex = 28;
            this.buttonChooseCertificate.Text = "Choose...";
            this.buttonChooseCertificate.UseVisualStyleBackColor = true;
            this.buttonChooseCertificate.Click += new System.EventHandler(this.buttonChooseCertificate_Click);
            // 
            // textBoxAuthCertificate
            // 
            this.textBoxAuthCertificate.Location = new System.Drawing.Point(456, 29);
            this.textBoxAuthCertificate.Name = "textBoxAuthCertificate";
            this.textBoxAuthCertificate.ReadOnly = true;
            this.textBoxAuthCertificate.Size = new System.Drawing.Size(317, 20);
            this.textBoxAuthCertificate.TabIndex = 27;
            // 
            // radioButtonCertificateCredential
            // 
            this.radioButtonCertificateCredential.AutoSize = true;
            this.radioButtonCertificateCredential.Location = new System.Drawing.Point(345, 6);
            this.radioButtonCertificateCredential.Name = "radioButtonCertificateCredential";
            this.radioButtonCertificateCredential.Size = new System.Drawing.Size(142, 17);
            this.radioButtonCertificateCredential.TabIndex = 20;
            this.radioButtonCertificateCredential.TabStop = true;
            this.radioButtonCertificateCredential.Text = "Certificate authentication";
            this.radioButtonCertificateCredential.UseVisualStyleBackColor = true;
            this.radioButtonCertificateCredential.CheckedChanged += new System.EventHandler(this.radioButtonCertificateCredential_CheckedChanged);
            // 
            // radioButtonSpecificCredentials
            // 
            this.radioButtonSpecificCredentials.AutoSize = true;
            this.radioButtonSpecificCredentials.Location = new System.Drawing.Point(193, 6);
            this.radioButtonSpecificCredentials.Name = "radioButtonSpecificCredentials";
            this.radioButtonSpecificCredentials.Size = new System.Drawing.Size(126, 17);
            this.radioButtonSpecificCredentials.TabIndex = 15;
            this.radioButtonSpecificCredentials.Text = "Specified credentials:";
            this.radioButtonSpecificCredentials.UseVisualStyleBackColor = true;
            this.radioButtonSpecificCredentials.CheckedChanged += new System.EventHandler(this.radioButtonSpecificCredentials_CheckedChanged);
            // 
            // radioButtonDefaultCredentials
            // 
            this.radioButtonDefaultCredentials.AutoSize = true;
            this.radioButtonDefaultCredentials.Checked = true;
            this.radioButtonDefaultCredentials.Location = new System.Drawing.Point(6, 6);
            this.radioButtonDefaultCredentials.Name = "radioButtonDefaultCredentials";
            this.radioButtonDefaultCredentials.Size = new System.Drawing.Size(160, 17);
            this.radioButtonDefaultCredentials.TabIndex = 14;
            this.radioButtonDefaultCredentials.TabStop = true;
            this.radioButtonDefaultCredentials.Text = "Logged on user\'s credentials";
            this.radioButtonDefaultCredentials.UseVisualStyleBackColor = true;
            this.radioButtonDefaultCredentials.CheckedChanged += new System.EventHandler(this.radioButtonDefaultCredentials_CheckedChanged);
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(230, 32);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(56, 13);
            this.labelPassword.TabIndex = 19;
            this.labelPassword.Text = "Password:";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Enabled = false;
            this.textBoxUsername.Location = new System.Drawing.Point(70, 29);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(154, 20);
            this.textBoxUsername.TabIndex = 16;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Enabled = false;
            this.textBoxPassword.Location = new System.Drawing.Point(296, 29);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(154, 20);
            this.textBoxPassword.TabIndex = 17;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(6, 32);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(58, 13);
            this.labelUsername.TabIndex = 18;
            this.labelUsername.Text = "Username:";
            // 
            // timerTaskMonitor
            // 
            this.timerTaskMonitor.Interval = 1000;
            this.timerTaskMonitor.Tick += new System.EventHandler(this.timerTaskMonitor_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 549);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox2);
            this.MinimumSize = new System.Drawing.Size(893, 550);
            this.Name = "FormMain";
            this.Text = "Exchange PowerShell Automation Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageOutput.ResumeLayout(false);
            this.tabPageOutput.PerformLayout();
            this.tabPageErrors.ResumeLayout(false);
            this.tabPageErrors.PerformLayout();
            this.tabPageVerbose.ResumeLayout(false);
            this.tabPageVerbose.PerformLayout();
            this.tabPageOptions.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPagePowerShell.ResumeLayout(false);
            this.tabPagePowerShell.PerformLayout();
            this.tabPageAuthv1.ResumeLayout(false);
            this.tabPageAuthv1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxPowerShell;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxProcessAsCommand;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button buttonClearCredentials;
        private System.Windows.Forms.Button buttonRemoveCredential;
        private System.Windows.Forms.Button buttonAddCredential;
        private System.Windows.Forms.ListBox listBoxVariables;
        private System.Windows.Forms.Button buttonRunPowerShell;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageOutput;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.TabPage tabPageErrors;
        private System.Windows.Forms.TextBox textBoxErrors;
        private System.Windows.Forms.TabPage tabPageVerbose;
        private System.Windows.Forms.TextBox textBoxVerbose;
        private System.Windows.Forms.TabPage tabPageOptions;
        private System.Windows.Forms.Button buttonClearLogs;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPageAuthv1;
        private System.Windows.Forms.RadioButton radioButtonSpecificCredentials;
        private System.Windows.Forms.RadioButton radioButtonDefaultCredentials;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.TabPage tabPagePowerShell;
        private System.Windows.Forms.CheckBox checkBoxAllowRedirection;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxAuthMethod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxExchangePSUrl;
        private System.Windows.Forms.RadioButton radioButtonUseRemotePowerShell;
        private System.Windows.Forms.RadioButton radioButtonUseLocalPowerShell;
        private System.Windows.Forms.CheckBox checkBoxIgnoreSSLErrors;
        private System.Windows.Forms.RadioButton radioButtonCertificateCredential;
        private System.Windows.Forms.Button buttonChooseCertificate;
        private System.Windows.Forms.TextBox textBoxAuthCertificate;
        private System.Windows.Forms.CheckBox checkBoxOffice365;
        private System.Windows.Forms.CheckBox checkBoxEXOv2;
        private System.Windows.Forms.Timer timerTaskMonitor;
        private System.Windows.Forms.CheckBox checkBoxUseCertificateThumprint;
    }
}

