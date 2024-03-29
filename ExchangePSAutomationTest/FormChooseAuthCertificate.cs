﻿/*
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
using System.Security.Cryptography.X509Certificates;

namespace ExchangePSAutomationTest
{
    public partial class FormChooseAuthCertificate : Form
    {
        private X509Certificate2 _certificate = null;

        public FormChooseAuthCertificate()
        {
            InitializeComponent();

            PopulateCertificates();
            UpdateUI();
        }

        /// <summary>
        /// Populate certificate combobox with any valid certificates found in user's personal store
        /// </summary>
        private void PopulateCertificates()
        {
            X509Store x509Store = null;
            comboBoxStoreCertificates.Items.Clear();
            try
            {
                x509Store = new X509Store("MY", StoreLocation.CurrentUser);
                x509Store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            }
            catch { }

            if (x509Store == null)
                return;

            // Store opened ok, so now we read the certificates
            foreach (X509Certificate2 x509Cert in x509Store.Certificates)
            {
                try
                {
                    comboBoxStoreCertificates.Items.Add(x509Cert);
                }
                catch { }
            }

            x509Store.Close();
        }

        /// <summary>
        /// Return the selected certificate
        /// </summary>
        public X509Certificate2 Certificate
        {
            get { return _certificate; }
        }

        /// <summary>
        /// Handle selection of certificate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxStoreCertificates_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _certificate = (X509Certificate2)comboBoxStoreCertificates.SelectedItem;
            }
            catch { }
            ShowCertificateInfo();
        }

        /// <summary>
        /// Display information about the selected certificate
        /// </summary>
        private void ShowCertificateInfo()
        {
            listBoxCertificateInfo.Items.Clear();
            if (_certificate == null)
                return;

            try
            {
                byte[] rawData = Certificate.RawData;
                listBoxCertificateInfo.Items.Add($"Content type: {X509Certificate2.GetCertContentType(rawData)}");
                listBoxCertificateInfo.Items.Add($"Friendly name: {_certificate.FriendlyName}");
                listBoxCertificateInfo.Items.Add($"Thumbprint: {_certificate.Thumbprint}");
                listBoxCertificateInfo.Items.Add($"Subject: {_certificate.Subject}");
                listBoxCertificateInfo.Items.Add($"Verified: {_certificate.Verify()}");
                listBoxCertificateInfo.Items.Add($"Simple name: {_certificate.GetNameInfo(X509NameType.SimpleName, true)}");
                listBoxCertificateInfo.Items.Add($"Signature algorithm: {_certificate.SignatureAlgorithm.FriendlyName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to read certificate: {Environment.NewLine}{ex.Message}", "Certificate Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            buttonOK.Enabled = (_certificate != null);
        }

        /// <summary>
        /// Handle OK button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (_certificate == null)
            {
                MessageBox.Show("Please select a certificate", "No certificate chosen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Dispose();
        }

        /// <summary>
        /// Handle Cancel button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// Open certificate from file system (prompts user for certificate file to open)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBrowseForCertificate_Click(object sender, EventArgs e)
        {
            OpenFileDialog oDialog = new OpenFileDialog();
            oDialog.Filter = "Certificates (*.pfx)|*.pfx|All Files|*.*";
            oDialog.DefaultExt = "pfx";
            oDialog.Title = "Select certificate to use (must have access to private key)";
            oDialog.CheckFileExists = true;
            if (oDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                _certificate = (X509Certificate2)X509Certificate2.CreateFromCertFile(oDialog.FileName);
                textBoxCertificateFile.Text = oDialog.FileName;
                ShowCertificateInfo();
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147024810)
                {
                    // File is password protected
                    try
                    {
                        byte[] certData = System.IO.File.ReadAllBytes(oDialog.FileName);
                        _certificate = new X509Certificate2(certData, textBoxPassword.Text);
                        textBoxCertificateFile.Text = oDialog.FileName;
                        ShowCertificateInfo();
                        textBoxPassword.Focus();
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show($"Unable to load certificate: {Environment.NewLine}{ex2.Message}", "Load failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _certificate = null;
                    }
                }
                else
                {
                    MessageBox.Show($"Unable to load certificate: {Environment.NewLine}{ex.Message}", "Load failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _certificate = null;
                }
            }
        }

        /// <summary>
        /// Update UI based on selected options
        /// </summary>
        private void UpdateUI()
        {
            bool bSelectFromStoreEnabled = radioButtonSelectFromStore.Checked;

            textBoxCertificateFile.Enabled = !bSelectFromStoreEnabled;
            buttonBrowseForCertificate.Enabled = !bSelectFromStoreEnabled;
            textBoxPassword.Enabled = !bSelectFromStoreEnabled;
            comboBoxStoreCertificates.Enabled = bSelectFromStoreEnabled;

            buttonOK.Enabled = (_certificate != null);
        }

        private void radioButtonSelectFromStore_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void radioButtonLoadFromFile_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

    }
}
