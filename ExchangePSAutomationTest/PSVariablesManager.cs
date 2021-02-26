using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Windows.Forms;

namespace ExchangePSAutomationTest
{
    /// <summary>
    /// Manages PSCredentials for use in the scripts (assigns PSCredentials to script variables)
    /// </summary>
    public class PSVariablesManager
    {
        private Dictionary<string, PSCredential> _credentials;
        private ListBox _displayListBox;

        public PSVariablesManager()
        {
            _credentials = new Dictionary<string, PSCredential>();
            _displayListBox = null;
        }

        /// <summary>
        /// Applies stored credentials to the given PowerShell object
        /// </summary>
        /// <param name="ps">PowerShell object on which credential variables will be set</param>
        public void ApplyCredentialsToPowerShell(PowerShell ps)
        {
            if (_credentials.Count == 0)
                return;

            foreach (string varName in _credentials.Keys)
            {
                Command setVariable = new Command("Set-Variable", false, true);
                setVariable.Parameters.Add(new CommandParameter("Name", varName));
                setVariable.Parameters.Add(new CommandParameter("Value", _credentials[varName]));
                ps.Commands.AddCommand(setVariable);
            }
            ps.Invoke();
        }

        /// <summary>
        /// Applies stored credentials to the given pipeline
        /// </summary>
        /// <param name="pl">Pipeline on which the credential variables will be set</param>
        public void ApplyCredentialsToPowerShell(Pipeline pl)
        {
            foreach (string varName in _credentials.Keys)
            {
                Command setVariable = new Command("Set-Variable", false, true);
                setVariable.Parameters.Add(new CommandParameter("Name", varName));
                setVariable.Parameters.Add(new CommandParameter("Value", _credentials[varName]));
                pl.Commands.Add(setVariable);
            }
        }

        /// <summary>
        /// Converts a string to SecureString (note: this is inherently insecure...)
        /// </summary>
        /// <param name="insecureString">The string to convert</param>
        /// <returns></returns>
        public SecureString StringToSecureString(string insecureString)
        {
            SecureString sString = new SecureString();
            for (int i = 0; i < insecureString.Length; i++)
            {
                sString.AppendChar(insecureString[i]);
            }
            return sString;
        }

        /// <summary>
        /// Add credential variable to our list
        /// </summary>
        /// <param name="varName">Variable name</param>
        /// <param name="Username">Username of PSCredential</param>
        /// <param name="Password">Password of PSCredential</param>
        /// <returns></returns>
        public bool AddCredential(string varName, string Username, string Password)
        {
            return AddCredential(varName, Username, StringToSecureString(Password));
        }

        /// <summary>
        /// Add credential variable to our list
        /// </summary>
        /// <param name="varName">Variable name</param>
        /// <param name="Username">Username of PSCredential</param>
        /// <param name="Password">Password of PSCredential</param>
        /// <returns></returns>
        public bool AddCredential(string varName, string Username, SecureString Password)
        {
            try
            {
                PSCredential cred = new PSCredential(Username, Password);
                if (_credentials.ContainsKey(varName))
                {
                    _credentials.Remove(varName);
                    RemoveCredentialFromListBox(varName);
                }
                _credentials.Add(varName, cred);
                if (_displayListBox != null)
                    _displayListBox.Items.Add(varName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void RemoveCredentialFromListBox(string varName)
        {
            if (_displayListBox != null)
            {
                for (int i = 0; i < _displayListBox.Items.Count; i++)
                {
                    if ((string)_displayListBox.Items[i] == varName)
                    {
                        _displayListBox.Items.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Remove the given credentials from our variable list
        /// </summary>
        /// <param name="varName">Name of the credentials variable to remove</param>
        public void RemoveCredential(string varName)
        {
            if (_credentials.ContainsKey(varName))
                _credentials.Remove(varName);
            RemoveCredentialFromListBox(varName);
        }

        /// <summary>
        /// Shows all credentials in the given listbox
        /// </summary>
        public void UpdateListBox(ListBox listBox = null)
        {
            if (listBox != null)
                _displayListBox = listBox;

            if (_displayListBox == null)
                return;

            _displayListBox.Items.Clear();
            foreach (string varName in _credentials.Keys)
            {
                _displayListBox.Items.Add(varName);
            }
        }

        /// <summary>
        /// Clear all stored credentials
        /// </summary>
        public void Clear()
        {
            _credentials = new Dictionary<string, PSCredential>();
            if (_displayListBox != null)
                _displayListBox.Items.Clear();
        }
    }
}
