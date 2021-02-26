using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExchangePSAutomationTest
{
    public partial class FormEditVariable : Form
    {
        private PSVariablesManager _credentialsManager;
        private bool _cancel = false;

        public FormEditVariable()
        {
            InitializeComponent();
            comboBoxVariableType.SelectedIndex = 0;
        }

        public FormEditVariable(string VarName, string Username)
            : this()
        {
            textBoxVariableName.Text = VarName;
            textBoxUsername.Text = Username;
        }

        public void AddCredential(PSVariablesManager CredentialsManager, IWin32Window Parent)
        {
            _credentialsManager = CredentialsManager;
            _cancel = false;
            this.ShowDialog(Parent);
            if (_cancel)
            {
                this.Dispose();
                return;
            }

            _credentialsManager.AddCredential(textBoxVariableName.Text, textBoxUsername.Text, textBoxPassword.Text);
            this.Dispose();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _cancel = true;
            this.Hide();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
