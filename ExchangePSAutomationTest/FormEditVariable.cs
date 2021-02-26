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
