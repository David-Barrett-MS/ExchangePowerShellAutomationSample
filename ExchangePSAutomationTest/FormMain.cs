/*
 * By David Barrett, Microsoft Ltd. 2014. Use at your own risk.  No warranties are given.
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Reflection;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Collections;
using System.IO;
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


        public FormMain()
        {
            InitializeComponent();
            _errorLog = new ClassLogger("error.log");
            _verboseLog = new ClassLogger("verbose.log");
            _outputLog = new ClassLogger("output.log");
            comboBoxAuthMethod.SelectedIndex = 0;
            _variablesManager.UpdateListBox(listBoxVariables);
        }

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

        private void radioButtonDefaultCredentials_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAuthState();
        }

        private void radioButtonSpecificCredentials_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAuthState();
        }

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
            catch { }

            return result;
        }

        private void LogError(string Message)
        {
            textBoxErrors.Text += String.Format("{0}{1}", Message, Environment.NewLine);
            textBoxErrors.SelectionStart = textBoxErrors.Text.Length;
            textBoxErrors.ScrollToCaret();
            _errorLog.Log(Message);
            if (tabControl1.SelectedTab != tabPageErrors)
                tabControl1.SelectedTab = tabPageErrors;
        }
        
        private bool ConnectWithRemotePowerShell()
        {
            try
            {
                PSCredential Credential = ExchangeCredentials();
                WSManConnectionInfo ConnInfo = null;

                if (radioButtonSpecificCredentials.Checked)
                {
                    ConnInfo = new WSManConnectionInfo((new Uri(textBoxExchangePSUrl.Text)), "http://schemas.microsoft.com/powershell/Microsoft.Exchange", Credential);
                }
                else
                    ConnInfo = new WSManConnectionInfo((new Uri(textBoxExchangePSUrl.Text)), "http://schemas.microsoft.com/powershell/Microsoft.Exchange", _authCertificate.Thumbprint);

                ConnInfo.AuthenticationMechanism = AuthMethod();
                ConnInfo.MaximumConnectionRedirectionCount = 0;
                if (checkBoxAllowRedirection.Checked)
                    ConnInfo.MaximumConnectionRedirectionCount = 10;
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

        private bool ConnectWithLocalPowerShell()
        {
            // Create a remote runspace
            try
            {
                PSCredential Credential = ExchangeCredentials();
                _exchangeRunspace = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace();

                PowerShell powershell = PowerShell.Create();
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

                _exchangeRunspace.Open();
                powershell.Runspace = _exchangeRunspace;

                Collection<object> result = InvokeAndReport(powershell);
                if (result.Count != 1)
                {
                    _exchangeRunspace.Close();
                    throw new Exception("Unexpected number of Remote Runspace connections returned.");
                }
                // Set the runspace as a local variable on the runspace
                powershell = PowerShell.Create();
                command = new PSCommand();
                command.AddCommand("Set-Variable");
                command.AddParameter("Name", "ra");
                command.AddParameter("Value", result[0]);
                powershell.Commands = command;
                powershell.Runspace = _exchangeRunspace;
                InvokeAndReport(powershell, true);

                // We'll import the session too (so we can use it as we would a PowerShell command window)
                command = new PSCommand();
                command.AddCommand("Set-ExecutionPolicy");
                command.AddParameter("Scope", "Process");
                command.AddParameter("ExecutionPolicy", "Unrestricted");
                powershell.Commands = command;
                powershell.Runspace = _exchangeRunspace;
                InvokeAndReport(powershell, true);

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
            return ConnectWithRemotePowerShell();
        }

        private bool IsValidPSUrl()
        {
            if (textBoxExchangePSUrl.Text.Contains("<server>"))
            {
                System.Windows.Forms.MessageBox.Show("Please update the PowerShell Url", "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxExchangePSUrl.Focus();
                return false;
            }
            return true;
        }

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

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsValidPSUrl())
                return;
            if (!HaveScript())
                return;
            button1.Enabled = false;
            Cursor cursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            if (ConnectExchangeRunspace())
            {
                ProcessScript();
            }

            this.Cursor = cursor;
            button1.Enabled = true;
        }

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
                    for (int i = 0; i < textBoxPowerShell.Lines.Count<string>(); i++)
                    {
                        powershell.Commands = ParseCommand(textBoxPowerShell.Lines[i]);
                        InvokeAndReport(powershell);
                    }
                }
                else
                {
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

        private PSCommand ParseCommand(string commandLine)
        {
            PSCommand command = new PSCommand();
            int i = commandLine.IndexOf(" ");
            if (i<0)
            {
                command.AddCommand(commandLine);
                return command;
            }

            // Add the command
            command.AddCommand(commandLine.Substring(0, i));

            // Now parse and add parameters
            try
            {
                i = commandLine.IndexOf("-", i);
                while ((i > 0) && (i < commandLine.Length))
                {
                    int j = commandLine.IndexOf("-", i + 1);
                    if (j < 0) j = commandLine.Length;
                    int p = commandLine.IndexOf(" ", i + 1);
                    string sParamName = commandLine.Substring(i + 1, p - i - 1);
                    string sParamValue = commandLine.Substring(p + 1, j - p - 2).Trim();
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

        private void radioButtonUseLocalPowerShell_CheckedChanged(object sender, EventArgs e)
        {
            CloseRunspace();
        }

        private void radioButtonUseRemotePowerShell_CheckedChanged(object sender, EventArgs e)
        {
            CloseRunspace();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
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

        private void buttonOffice365PowerShell_Click(object sender, EventArgs e)
        {
            textBoxExchangePSUrl.Text = "https://outlook.office365.com/powershell-liveid/";
            comboBoxAuthMethod.SelectedIndex = 1; // Basic auth
        }
    }
}
