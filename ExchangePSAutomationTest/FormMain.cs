/*
 * By David Barrett, Microsoft Ltd. 2014-2021. Use at your own risk.  No warranties are given.
 * 
 * DISCLAIMER:
 * THIS CODE IS SAMPLE CODE. THESE SAMPLES ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND.
 * MICROSOFT FURTHER DISCLAIMS ALL IMPLIED WARRANTIES INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OF MERCHANTABILITY OR OF FITNESS FOR
 * A PARTICULAR PURPOSE. THE ENTIRE RISK ARISING OUT OF THE USE OR PERFORMANCE OF THE SAMPLES REMAINS WITH YOU. IN NO EVENT SHALL
 * MICROSOFT OR ITS SUPPLIERS BE LIABLE FOR ANY DAMAGES WHATSOEVER (INCLUDING, WITHOUT LIMITATION, DAMAGES FOR LOSS OF BUSINESS PROFITS,
 * BUSINESS INTERRUPTION, LOSS OF BUSINESS INFORMATION, OR OTHER PECUNIARY LOSS) ARISING OUT OF THE USE OF OR INABILITY TO USE THE
 * SAMPLES, EVEN IF MICROSOFT HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES. BECAUSE SOME STATES DO NOT ALLOW THE EXCLUSION OR LIMITATION
 * OF LIABILITY FOR CONSEQUENTIAL OR INCIDENTAL DAMAGES, THE ABOVE LIMITATION MAY NOT APPLY TO YOU.
 * */

using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;


namespace ExchangePSAutomationTest
{
    public partial class FormMain : Form
    {
        private Runspace _exchangeRunspace = null;
        private ClassLogger _errorLog = null;
        private ClassLogger _verboseLog = null;
        private ClassLogger _outputLog = null;
        private PSVariablesManager _variablesManager = new PSVariablesManager();
        private X509Certificate2 _authCertificate = null;
        static bool _ignoreSSLErrors = false;
        static bool _confirmIgnoreSSLErrors = true;

        /// <summary>
        /// Form constructor
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            _errorLog = new ClassLogger("error.log");
            _verboseLog = new ClassLogger("verbose.log");
            _outputLog = new ClassLogger("output.log");
            comboBoxAuthMethod.SelectedIndex = 0;
            _variablesManager.UpdateListBox(listBoxVariables);
        }

        /// <summary>
        /// Callback for certificate handling
        /// </summary>
        private static bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (_ignoreSSLErrors || (errors == SslPolicyErrors.None))
                return true;

            if (_confirmIgnoreSSLErrors)
            {
                if (MessageBox.Show(String.Format("Ignore SSL errors?{0}{1}", Environment.NewLine, errors.ToString()), "Invalid Certificate", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _confirmIgnoreSSLErrors = false;
                    _ignoreSSLErrors = true;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Update Auth UI elements
        /// </summary>
        private void UpdateAuthState()
        {
            textBoxUsername.Enabled = radioButtonSpecificCredentials.Checked;
            textBoxPassword.Enabled = radioButtonSpecificCredentials.Checked;
            labelPassword.Enabled = radioButtonSpecificCredentials.Checked;
            labelUsername.Enabled = radioButtonSpecificCredentials.Checked;
            textBoxAuthCertificate.Enabled = radioButtonCertificateCredential.Checked;
            buttonChooseCertificate.Enabled = radioButtonCertificateCredential.Checked;
            CloseRunspace();
        }

        /// <summary>
        /// Uses Reflection to read the properties of the passed object and returns them as a string
        /// </summary>
        private string GetObjectInfo(object Object)
        {
            // Use Reflection to read and show object properties
            StringBuilder propInfo = new StringBuilder();
            PropertyInfo[] props = Object.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                propInfo.Append(prop.ReflectedType.Name);
                propInfo.Append("=");
                propInfo.AppendLine(Object.GetType().GetProperty(prop.Name, BindingFlags.Public | BindingFlags.Instance).ToString());
            }
            return propInfo.ToString();
        }

        /// <summary>
        /// Invoke the PowerShell command/script and write the streams to the UI
        /// </summary>
        /// <param name="powershell">PowerShell object to invoke</param>
        /// <param name="ErrorsOnly">If true, only errors and output will be displayed (default is false)</param>
        Collection<object> InvokeAndReport(PowerShell powershell, bool ErrorsOnly = false)
        {
            Collection<object> result = null;
            try
            {
                result = powershell.Invoke<object>();
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }

            try
            {
                foreach (ErrorRecord record in powershell.Streams.Error)
                    LogError(record.Exception.ToString());

                foreach (VerboseRecord record in powershell.Streams.Verbose)
                {
                    textBoxVerbose.Text += String.Format("{0}{1}", record.Message.ToString(), Environment.NewLine);
                    textBoxVerbose.SelectionStart = textBoxVerbose.Text.Length;
                    textBoxVerbose.ScrollToCaret();
                    _verboseLog.Log(record.Message.ToString());
                }

                if (!ErrorsOnly)
                {
                    if (result != null)
                    {
                        foreach (PSObject obj in result)
                        {
                            StringBuilder propInfo = new StringBuilder();
                            foreach (PSPropertyInfo psPropInfo in obj.Properties)
                            {
                                propInfo.AppendLine(String.Format("{0}: {1}", psPropInfo.Name, psPropInfo.Value));
                            }

                            textBoxOutput.Text += String.Format("{0}{1}", propInfo.ToString(), Environment.NewLine);
                            textBoxOutput.SelectionStart = textBoxOutput.Text.Length;
                            textBoxOutput.ScrollToCaret();
                            _outputLog.Log(propInfo.ToString());
                        }
                    }
                }
            }
            catch { }

            return result;
        }

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="Message">The error message</param>
        private void LogError(string Message)
        {
            textBoxErrors.Text += String.Format("{0}{1}", Message, Environment.NewLine);
            textBoxErrors.SelectionStart = textBoxErrors.Text.Length;
            textBoxErrors.ScrollToCaret();
            _errorLog.Log(Message);
            if (tabControl1.SelectedTab != tabPageErrors)
                tabControl1.SelectedTab = tabPageErrors;
        }

        /// <summary>
        /// Connect to remote runspace (for an Exchange runspace, this will be in No-Language mode, which will only allow Exchange cmdlets to run
        /// </summary>
        private bool ConnectToRemotePowerShell()
        {
            try
            {
                // Set our ConnectionInfo for the Exchange remote session
                WSManConnectionInfo ConnInfo = null;
                PSCredential Credential = ExchangeCredentials();
                if (radioButtonSpecificCredentials.Checked)
                {
                    ConnInfo = new WSManConnectionInfo((new Uri(textBoxExchangePSUrl.Text)), "http://schemas.microsoft.com/powershell/Microsoft.Exchange", Credential);
                }
                else if (radioButtonCertificateCredential.Checked)
                {
                    ConnInfo = new WSManConnectionInfo((new Uri(textBoxExchangePSUrl.Text)), "http://schemas.microsoft.com/powershell/Microsoft.Exchange", _authCertificate.Thumbprint);
                }
                else // use logged on user's credentials
                { 
                    ConnInfo = new WSManConnectionInfo(new Uri(textBoxExchangePSUrl.Text));
                    ConnInfo.ShellUri = "http://schemas.microsoft.com/powershell/Microsoft.Exchange";
                }
                
                ConnInfo.AuthenticationMechanism = AuthMethod();
                ConnInfo.MaximumConnectionRedirectionCount = 0;
                if (checkBoxAllowRedirection.Checked)
                    ConnInfo.MaximumConnectionRedirectionCount = 10;

                // Create the remote runspace
                _verboseLog.Log("Creating and opening remote runspace");
                _exchangeRunspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace(ConnInfo);
                _exchangeRunspace.Open();
                _verboseLog.Log("Remote runspace opened");
                return true;
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Return the currently select authentication mechanism
        /// </summary>
        private AuthenticationMechanism AuthMethod()
        {
            switch (comboBoxAuthMethod.Text.ToLower())
            {
                case "basic": return AuthenticationMechanism.Basic;
                case "credssp": return AuthenticationMechanism.Credssp;
                case "digest": return AuthenticationMechanism.Digest;
                case "kerberos": return AuthenticationMechanism.Kerberos;
                case "negotiate": return AuthenticationMechanism.Negotiate;
                case "negotiatewithimplicitcredential": return AuthenticationMechanism.NegotiateWithImplicitCredential;
            }
            return AuthenticationMechanism.Default;
        }

        /// <summary>
        /// Return the currently configured credentials as a PSCredential object
        /// </summary>
        private PSCredential ExchangeCredentials()
        {
            if (radioButtonDefaultCredentials.Checked)
                return null;

            string sPassword = textBoxPassword.Text;
            SecureString sSPassword = new SecureString();

            foreach (char X in sPassword)
                sSPassword.AppendChar(X);

            PSCredential Credential = new PSCredential(textBoxUsername.Text, sSPassword);
            return Credential;
        }

        /// <summary>
        /// Import the remote Exchange PowerShell session into a local PowerShell session
        /// This allows scripts and other cmdlets to be run as well as Exchange cmdlets
        /// </summary>
        private bool ConnectWithLocalPowerShell()
        {
            // Create a remote runspace
            try
            {
                PSCredential Credential = ExchangeCredentials();
                _exchangeRunspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace();

                PowerShell powershell = PowerShell.Create();

                // Create New-PSSession command
                PSCommand command = new PSCommand();
                command.AddCommand("New-PSSession");
                command.AddParameter("ConfigurationName", "Microsoft.Exchange");
                command.AddParameter("ConnectionUri", new Uri(textBoxExchangePSUrl.Text));
                command.AddParameter("Authentication", comboBoxAuthMethod.Text);
                if (!(Credential==null))
                    command.AddParameter("Credential", Credential);

                if (checkBoxAllowRedirection.Checked) 
                    command.AddParameter("AllowRedirection");
                powershell.Commands = command;

                // Open the local PowerShell runspace
                _exchangeRunspace.Open();
                powershell.Runspace = _exchangeRunspace;

                // Run the New-PSSession command in the local PowerShell runspace
                Collection<object> result = InvokeAndReport(powershell);
                if (result.Count != 1)
                {
                    _exchangeRunspace.Close();
                    throw new Exception("Unexpected number of Remote Runspace connections returned.");
                }
                // Set the remote Exchange runspace (returned by New-PSSession) as a local variable on the runspace
                powershell = PowerShell.Create();
                command = new PSCommand();
                command.AddCommand("Set-Variable");
                command.AddParameter("Name", "ra");
                command.AddParameter("Value", result[0]);
                powershell.Commands = command;
                powershell.Runspace = _exchangeRunspace;
                InvokeAndReport(powershell, true);

                // Import the session into the local runspace (which then makes the Exchange cmdlets available directly in the local runspace)
                command = new PSCommand();
                command.AddCommand("Import-PSSession");
                command.AddParameter("Session", result[0]);
                powershell.Commands = command;
                powershell.Runspace = _exchangeRunspace;
                InvokeAndReport(powershell, true);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Connect to Exchange runspace (using currently selected configuration)
        /// Returns true if successful
        /// </summary>
        private bool ConnectExchangeRunspace()
        {
            if (!(_exchangeRunspace == null))
            {
                if (_exchangeRunspace.RunspaceStateInfo.State == RunspaceState.Opened)
                    return true;
            }
            if (radioButtonUseLocalPowerShell.Checked)
            {
                return ConnectWithLocalPowerShell();
            }
            return ConnectToRemotePowerShell();
        }

        /// <summary>
        /// Checks that a PowerShell Url is valid
        /// Currently this is just a basic check that the value has been set
        /// </summary>
        private bool IsValidPSUrl()
        {
            if (textBoxExchangePSUrl.Text.Contains("<server>") || !textBoxExchangePSUrl.Text.ToLower().StartsWith("http"))
            {
                System.Windows.Forms.MessageBox.Show("Please update the PowerShell Url", "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxExchangePSUrl.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check if there is text (i.e. PowerShell code) available in the UI
        /// Returns true if text is present (does not do any validation)
        /// </summary>
        private bool HaveScript()
        {
            if (String.IsNullOrEmpty(textBoxPowerShell.Text))
            {
                System.Windows.Forms.MessageBox.Show("Please enter some PowerShell", "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPowerShell.Focus();
                return false;
            }
            return true;
        }


        /// <summary>
        /// Process the PowerShell
        /// Obtains the PowerShell session per config and then invokes the script
        /// </summary>
        private void ProcessScript()
        {
            try
            {
                PowerShell powershell = PowerShell.Create();
                powershell.Runspace = _exchangeRunspace;

                // Apply any credentials we have to the runspace
                _variablesManager.ApplyCredentialsToPowerShell(powershell);

                if (checkBoxProcessAsCommand.Checked)
                {
                    // If we connect directly to the Exchange remote runspace, we can't run a script.  Each command (which
                    // must be an Exchange cmdlet) must be added individually.
                    for (int i = 0; i < textBoxPowerShell.Lines.Count<string>(); i++)
                    {
                        powershell.Commands = ParseCommand(textBoxPowerShell.Lines[i]);
                        InvokeAndReport(powershell);
                    }
                }
                else
                {
                    // This method will only work when we import the remote Exchange runspace into the local runspace.
                    // In this case, FullLanguage is available in the local session, and only Exchange cmdlets are sent to the remote.
                    PSCommand command = new PSCommand();
                    command.AddScript(textBoxPowerShell.Text);
                    powershell.Commands = command;
                    InvokeAndReport(powershell);
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        /// <summary>
        /// Parse a PowerShell text string into a PSCommand with parameters
        /// This is a very basic parser and won't work with complex cmdlet calls
        /// </summary>
        /// <param name="commandLine">The PowerShell code</param>
        private PSCommand ParseCommand(string commandLine)
        {
            PSCommand command = new PSCommand();
            int i = commandLine.IndexOf(" ");
            if (i<0)
            {
                // The command has no parameters, so add as is
                command.AddCommand(commandLine);
                return command;
            }

            // Add the command, then we need to deal with parameters
            
            command.AddCommand(commandLine.Substring(0, i));
            string parameters = commandLine.Substring(i+1);

            i = parameters.IndexOf("-", i);
            if (i<0)
            {
                // We have parameters, but they are not named - we just add them to the command
                command.AddArgument(parameters);
                return command;
            }

            // Now parse and add parameters
            try
            {
                while ((i > 0) && (i < commandLine.Length))
                {
                    int j = parameters.IndexOf("-", i + 1);
                    if (j < 0) j = parameters.Length;
                    int p = parameters.IndexOf(" ", i + 1);
                    string sParamName = parameters.Substring(i + 1, p - i - 1);
                    string sParamValue = parameters.Substring(p + 1, j - p - 2).Trim();
                    if (sParamValue.StartsWith("\"") && sParamValue.EndsWith("\""))
                        sParamValue = sParamValue.Substring(1, sParamValue.Length - 2);
                    command.AddParameter(sParamName, sParamValue);
                    i = j;
                }
            }
            catch (Exception ex)
            {
                LogError(String.Format("Unable to parse command parameters: {0}", ex.Message));
            }
            return command;
        }

        /// <summary>
        /// Close the runspace
        /// </summary>
        private void CloseRunspace()
        {
            try
            {
                if (!(_exchangeRunspace == null))
                    _exchangeRunspace.Close();
            }
            catch { }
            _exchangeRunspace = null;
        }

        #region UIEvents

        private void radioButtonDefaultCredentials_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAuthState();
        }

        private void radioButtonSpecificCredentials_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAuthState();
        }

        private void buttonRunPowerShell_Click(object sender, EventArgs e)
        {
            if (!IsValidPSUrl())
                return;
            if (!HaveScript())
                return;
            buttonRunPowerShell.Enabled = false;
            Cursor cursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            if (ConnectExchangeRunspace())
            {
                ProcessScript();
            }

            this.Cursor = cursor;
            buttonRunPowerShell.Enabled = true;
        }

        private void radioButtonUseLocalPowerShell_CheckedChanged(object sender, EventArgs e)
        {
            CloseRunspace();
            checkBoxProcessAsCommand.Enabled = radioButtonUseLocalPowerShell.Checked;
        }

        private void radioButtonUseRemotePowerShell_CheckedChanged(object sender, EventArgs e)
        {
            CloseRunspace();
            checkBoxProcessAsCommand.Checked = radioButtonUseRemotePowerShell.Checked;
            checkBoxProcessAsCommand.Enabled = !radioButtonUseRemotePowerShell.Checked;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseRunspace();
        }

        private void textBoxExchangePSUrl_TextChanged(object sender, EventArgs e)
        {
            CloseRunspace();
        }

        private void buttonClearLogs_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxErrors.ResetText();
                textBoxOutput.ResetText();
                textBoxVerbose.ResetText();
            }
            catch { }
        }

        private void buttonAddCredential_Click(object sender, EventArgs e)
        {
            FormEditVariable form = new FormEditVariable();
            form.AddCredential(_variablesManager, this);
        }

        private void buttonRemoveCredential_Click(object sender, EventArgs e)
        {

        }

        private void buttonClearCredentials_Click(object sender, EventArgs e)
        {

        }

        private void buttonChooseCertificate_Click(object sender, EventArgs e)
        {
            FormChooseAuthCertificate oForm = new FormChooseAuthCertificate();
            DialogResult result = oForm.ShowDialog(this);
            if (result == DialogResult.Cancel)
                return;

            _authCertificate = oForm.Certificate;
            textBoxAuthCertificate.Text = _authCertificate.Subject;
        }

        private void checkBoxOffice365_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOffice365.Checked)
            {
                textBoxExchangePSUrl.Text = "https://outlook.office365.com/powershell-liveid/";
                comboBoxAuthMethod.SelectedIndex = 1; // Basic auth
                if (radioButtonUseRemotePowerShell.Checked)
                {
                    checkBoxProcessAsCommand.Checked = true;
                    checkBoxProcessAsCommand.Enabled = false;
                }              
            }
            comboBoxAuthMethod.Enabled = !checkBoxOffice365.Checked;
            textBoxExchangePSUrl.ReadOnly = checkBoxOffice365.Checked;
        }

        #endregion

    }
}
