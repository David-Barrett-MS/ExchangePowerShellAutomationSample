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
