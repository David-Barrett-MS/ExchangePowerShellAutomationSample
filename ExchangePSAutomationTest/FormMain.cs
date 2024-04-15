/*
 * By David Barrett, Microsoft Ltd. 2014-2022. Use at your own risk.  No warranties are given.
 * 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
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
using System.Threading.Tasks;

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
        private Task _scriptRunnerTask = null;
        private DateTime _scriptRunStart = DateTime.MinValue;
        private DateTime _scriptRunEnd = DateTime.MinValue;
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
            checkBoxOffice365.Checked = true;
            checkBoxEXOv2_CheckedChanged(this, null);
            SetStatus("");
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
            textBoxAuthCertificate.Enabled = radioButtonCertificateCredential.Checked;
            buttonChooseCertificate.Enabled = radioButtonCertificateCredential.Checked;
            if (checkBoxOffice365.Checked)
            {
                //radioButtonUseLocalPowerShell.Checked = true;
                if (radioButtonDefaultCredentials.Checked)
                    radioButtonSpecificCredentials.Checked = true;
                if (radioButtonCertificateCredential.Checked)
                {
                    labelUsername.Text = "AppID:";
                    labelPassword.Text = "Organization:";
                    labelPassword.Visible = true;
                    textBoxPassword.Visible = true;
                    checkBoxUseCertificateThumprint.Enabled = true;
                    if (checkBoxUseCertificateThumprint.Checked)
                    {
                        buttonChooseCertificate.Visible = false;
                        textBoxAuthCertificate.ReadOnly = false;
                    }
                    else
                    {
                        buttonChooseCertificate.Visible = true;
                        textBoxAuthCertificate.ReadOnly = true;
                    }
                }
                else
                {
                    labelUsername.Text = "Username:";
                    labelPassword.Text = "Password:";
                    labelPassword.Visible = false;
                    textBoxPassword.Visible = false;
                    checkBoxUseCertificateThumprint.Enabled = false;
                }
                textBoxPassword.UseSystemPasswordChar = false;
                textBoxPassword.Enabled = radioButtonCertificateCredential.Checked;
                textBoxUsername.Enabled = radioButtonCertificateCredential.Checked || radioButtonSpecificCredentials.Checked;
            }
            else
            {
                labelUsername.Text = "Username:";
                labelPassword.Text = "Password:";
                labelPassword.Visible = true;
                textBoxPassword.Visible = true;
                textBoxPassword.Enabled = radioButtonSpecificCredentials.Checked;
                textBoxUsername.Enabled = radioButtonSpecificCredentials.Checked;
                textBoxPassword.UseSystemPasswordChar = true;
            }

            radioButtonDefaultCredentials.Enabled = !checkBoxEXOv2.Checked;

            // As the auth state has changed, we ensure any existing runspace is closed so that
            // new auth settings will be applied
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
        /// Add the given data to the end of the text in the specified TextBox.
        /// Defaults to textBoxOutput if none specified.
        /// </summary>
        /// <param name="data">The string data to append.</param>
        /// <param name="targetTextBox">The textbox to which to append the data.</param>
        private void LogOutput(string data, TextBox targetTextBox = null, bool scrollToBottom = false)
        {
            if (targetTextBox == null)
                targetTextBox = textBoxOutput;

            Action action = new Action(() =>
            {
                targetTextBox.Text += data;
                if (scrollToBottom)
                {
                    targetTextBox.SelectionStart = targetTextBox.Text.Length;
                    targetTextBox.ScrollToCaret();
                }
            });

            if (targetTextBox.InvokeRequired)
                targetTextBox.Invoke(action, null);
            else
                action();
        }

        /// <summary>
        /// Sets the status text.  If status is blank, the status textbox is hidden.
        /// </summary>
        /// <param name="status">Status to display (blank to hide).</param>
        private void SetStatus(string status)
        {
            Action action = new Action(() =>
            {
                textBoxStatus.Text = status;
                if (String.IsNullOrEmpty(status))
                    textBoxStatus.Visible = false;
                else if (textBoxStatus.Visible == false)
                    textBoxStatus.Visible = true;
            });
            if (textBoxStatus.InvokeRequired)
                textBoxStatus.Invoke(action, null);
            else
                action();
        }

        /// <summary>
        /// Invoke the PowerShell command/script and write the streams to the UI
        /// </summary>
        /// <param name="powershell">PowerShell object to invoke</param>
        /// <param name="ErrorsOnly">If true, only errors and output will be displayed (default is false)</param>
        Collection<PSObject> InvokeAndReport(PowerShell powershell, bool ErrorsOnly = false)
        {
            Collection<PSObject> result = null;
            _scriptRunStart = DateTime.Now;
            try
            {
                result = powershell.Invoke();
            }
            catch (CommandNotFoundException ex)
            {
                // This error occurs if the Exchange Online Management module was not loaded
                LogError(ex.Message);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
            _scriptRunEnd = DateTime.Now;

            try
            {
                SetStatus("Parsing error records");
                foreach (ErrorRecord record in powershell.Streams.Error)
                    LogError(record.Exception.ToString());

                SetStatus("Parsing verbose records");
                foreach (VerboseRecord record in powershell.Streams.Verbose)
                {
                    LogOutput($"{record.Message}{Environment.NewLine}", textBoxVerbose);
                    _verboseLog.Log(record.Message.ToString());
                }

                SetStatus("Parsing information records");
                foreach (InformationRecord record in powershell.Streams.Information)
                {
                    LogOutput($"{record.MessageData}{Environment.NewLine}", textBoxVerbose);
                    _verboseLog.Log(record.MessageData.ToString());
                }

                if (!ErrorsOnly)
                {
                    if (result != null)
                    {
                        SetStatus("Parsing cmdlet output");
                        StringBuilder propInfo = new StringBuilder();
                        foreach (PSObject obj in result)
                        {
                            string objType = obj.BaseObject.GetType().FullName;
                            if (objType.StartsWith("System.") && !objType.StartsWith("System.Management"))
                            {
                                propInfo.AppendLine(obj.ToString());
                            }
                            else
                            {
                                foreach (PSPropertyInfo psPropInfo in obj.Properties)
                                {
                                    propInfo.AppendLine($"{psPropInfo.Name}: {psPropInfo.Value}");
                                }
                                propInfo.AppendLine();
                            }
                        }
                        LogOutput(propInfo.ToString());
                        _outputLog.Log(propInfo.ToString());
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
            LogOutput($"{Message}{Environment.NewLine}", textBoxErrors);
            _errorLog.Log(Message);

            Action action = new Action(() =>
            {
                if (tabControl1.SelectedTab != tabPageErrors)
                    tabControl1.SelectedTab = tabPageErrors;
            });
            if (tabControl1.InvokeRequired)
                tabControl1.Invoke(action, null);
            else
                action();
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

                if (radioButtonCertificateCredential.Checked)
                {
                    // Configure certificate authentication
                }
                else
                {
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
                }

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
        /// Return the currently selected authentication mechanism
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
        /// Add the authentication parameters to the connection command
        /// </summary>
        private void AddAuthParameterForSession(PSCommand command)
        {
            if (radioButtonSpecificCredentials.Checked)
            {
                if (checkBoxOffice365.Checked)
                {
                    if (!String.IsNullOrEmpty(textBoxUsername.Text))
                        command.AddParameter("UserPrincipalName", textBoxUsername.Text);
                }
                else
                    command.AddParameter("Credential", ExchangeCredentials());
            }
            else if (radioButtonCertificateCredential.Checked)
            {
                if (checkBoxEXOv2.Checked)
                {
                    // Certificate auth for EXO also require AppId and Organisation Id e.g.
                    // Connect-ExchangeOnline -Certificate [Cert] -AppID [AppId] -Organization [OrganisationId]
                    // Connect-ExchangeOnline -CertificateThumbprint [Thumbprint] -AppID [AppId] -Organization [OrganisationId]
                    if (checkBoxUseCertificateThumprint.Checked)
                        command.AddParameter("CertificateThumbprint", textBoxAuthCertificate.Text);
                    else
                        command.AddParameter("Certificate", _authCertificate);
                    command.AddParameter("AppID", textBoxUsername.Text);
                    command.AddParameter("Organization", textBoxPassword.Text);
                }
            else
                command.AddParameter("CertificateThumbprint", _authCertificate.Thumbprint);
            }
        }

        /// <summary>
        /// Create the New-PSSession command
        /// </summary>
        private PSCommand NewPSSession()
        {
            PSCommand command = new PSCommand();
            command.AddCommand("New-PSSession");
            command.AddParameter("ConfigurationName", "Microsoft.Exchange");
            command.AddParameter("ConnectionUri", new Uri(textBoxExchangePSUrl.Text));
            command.AddParameter("Authentication", comboBoxAuthMethod.Text);

            AddAuthParameterForSession(command);

            if (checkBoxAllowRedirection.Checked)
                command.AddParameter("AllowRedirection");

            return command;
        }

        /// <summary>
        /// Create the Connect-ExchangeOnline command
        /// </summary>
        private PSCommand ConnectExchangeOnline(PSCredential Credential = null)
        {
            PSCommand command = new PSCommand();
            command.AddCommand("Connect-ExchangeOnline");
            AddAuthParameterForSession(command);

            return command;
        }

        /// <summary>
        /// Import the remote Exchange PowerShell session into a local PowerShell session
        /// This allows scripts and other cmdlets to be run as well as Exchange cmdlets
        /// </summary>
        private bool ConnectWithLocalPowerShell()
        {
            try
            {
                // Set the initial session state
                InitialSessionState sessionState = InitialSessionState.CreateDefault();
                if (checkBoxOffice365.Checked && checkBoxEXOv2.Checked)
                    sessionState.ImportPSModule(new string[] { "ExchangeOnlineManagement" });

                // Create the runspace
                _exchangeRunspace = RunspaceFactory.CreateRunspace(sessionState);
                _exchangeRunspace.Open();

                PowerShell powershell = PowerShell.Create();
                powershell.Runspace = _exchangeRunspace;


                if (checkBoxOffice365.Checked && checkBoxEXOv2.Checked)
                {
                    // Connect to EXO v2                    
                    powershell.Commands = ConnectExchangeOnline();
                    InvokeAndReport(powershell);
                }
                else
                {
                    // Create New-PSSession command
                    powershell.Commands = NewPSSession();

                    // Run the New-PSSession command in the local PowerShell runspace
                    Collection<PSObject> result = InvokeAndReport(powershell);
                    if (result == null)
                    {
                        _exchangeRunspace.Close();
                        throw new Exception("Failed to connect to remote runspace.");
                    }

                    if (result.Count != 1)
                    {
                        _exchangeRunspace.Close();
                        throw new Exception("Unexpected number of Remote Runspace connections returned.");
                    }

                    // Set the remote Exchange runspace (returned by New-PSSession) as a local variable on the runspace
                    powershell = PowerShell.Create();
                    PSCommand command = new PSCommand();
                    command.AddCommand("Set-Variable");
                    command.AddParameter("Name", "ExchangeSession");
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
            SetStatus("Connecting to Exchange runspace");
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
                SetStatus("Creating PowerShell session");
                PowerShell powershell = PowerShell.Create();
                powershell.Runspace = _exchangeRunspace;

                // Apply any credentials we have to the runspace
                _variablesManager.ApplyCredentialsToPowerShell(powershell);

                SetStatus("Processing PowerShell");
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
            SetStatus("");
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

            i = parameters.IndexOf("-");
            if (i<0)
            {
                // We have parameters, but they are not named - we just add them to the command
                command.AddArgument(parameters);
                return command;
            }

            // Now parse and add parameters
            try
            {
                while ((i >= 0) && (i < parameters.Length))
                {
                    int j = parameters.IndexOf("-", i + 1);
                    if (j > 0)
                    {
                        // Check that the '-' isn't part of a value (it will be preceded by a space if it is a real parameter)
                        while (j>0 && parameters[j - 1] != ' ')
                        {
                            j = parameters.IndexOf("-", j + 1);
                        }
                    }
                    if (j < 0)
                        j = parameters.Length;

                    int p = parameters.IndexOf(" ", i + 1); // Note that parameters can also be split using ':'.  We don't check for this.
                    string sParamName = parameters.Substring(i + 1, p - i - 1);
                    string sParamValue = parameters.Substring(p + 1, j - p - 1).Trim();

                    if (sParamValue.StartsWith("\"") && sParamValue.EndsWith("\""))
                        sParamValue = sParamValue.Substring(1, sParamValue.Length - 2);

                    // We currently only check for boolean parameters and treat everything else as a string
                    if (sParamValue.Equals("$false", StringComparison.OrdinalIgnoreCase))
                        command.AddParameter(sParamName, false);
                    else if (sParamValue.Equals("$true", StringComparison.OrdinalIgnoreCase))
                        command.AddParameter(sParamName, true);
                    else 
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
            if (_scriptRunnerTask != null)
            {
                if (_scriptRunnerTask.Status == TaskStatus.Running)
                    return;
                _scriptRunnerTask.Dispose();
                _scriptRunnerTask = null;
            }
            if (!IsValidPSUrl())
                return;
            if (!HaveScript())
                return;
            buttonRunPowerShell.Enabled = false;
            Cursor cursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            if (checkBoxNewSession.Checked && _exchangeRunspace != null)
                CloseRunspace();

            if (ConnectExchangeRunspace())
            {
                _scriptRunnerTask = new Task(new Action(() => { ProcessScript(); }));
                _scriptRunnerTask.Start();
                _scriptRunnerTask.GetAwaiter().OnCompleted(() => { ScriptTaskComplete(); });
            }
            else
                buttonRunPowerShell.Enabled = true;

            this.Cursor = cursor;
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
            else
            {
                if (textBoxExchangePSUrl.Text == "https://outlook.office365.com/powershell-liveid/")
                {
                    textBoxExchangePSUrl.Text = "http://<exchange>/powershell/";
                    textBoxExchangePSUrl.Focus();
                    textBoxExchangePSUrl.SelectionStart = 7;
                    textBoxExchangePSUrl.SelectionLength = 10;
                }
                comboBoxAuthMethod.SelectedIndex = 4; // Kerberos
            }
            comboBoxAuthMethod.Enabled = !checkBoxOffice365.Checked;
            textBoxExchangePSUrl.ReadOnly = checkBoxOffice365.Checked;
            checkBoxEXOv2.Enabled = checkBoxOffice365.Checked;
            UpdateAuthState();
        }

        private void checkBoxEXOv2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAuthState();
        }

        private void radioButtonCertificateCredential_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAuthState();
        }

        #endregion

        private void ScriptTaskComplete()
        {
            LogOutput($"{Environment.NewLine}Script completed in {_scriptRunEnd.Subtract(_scriptRunStart)}{Environment.NewLine}");
            LogOutput($"Results processing completed in {DateTime.Now.Subtract(_scriptRunEnd)}{Environment.NewLine}", null, true);
            _scriptRunStart = DateTime.MinValue;
            _scriptRunEnd = DateTime.MinValue;

            _scriptRunnerTask.Dispose();
            _scriptRunnerTask = null;
            buttonRunPowerShell.Enabled = true;
        }

        private void checkBoxUseCertificateThumprint_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAuthState();
        }

    }
}
