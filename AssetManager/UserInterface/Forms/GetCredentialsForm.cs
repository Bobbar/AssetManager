using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Security;
using AssetManager.UserInterface.CustomControls;

namespace AssetManager.UserInterface.Forms
{

    public partial class GetCredentialsForm : ExtendedForm
    {
        private NetworkCredential MyCreds;

        private SecureString SecurePwd = new SecureString();
        public NetworkCredential Credentials
        {
            get { return MyCreds; }
        }

        public GetCredentialsForm()
        {
            InitializeComponent();
        }

        public GetCredentialsForm(string credentialDescription)
        {
            InitializeComponent();
            CredDescriptionLabel.Text = credentialDescription;
        }

        private void Accept()
        {
            string Username = null;
            Username = Strings.Trim(txtUsername.Text);
            if (!string.IsNullOrEmpty(Username) & SecurePwd.Length > 0)
            {
                SecurePwd.MakeReadOnly();
                MyCreds = new NetworkCredential(Username, SecurePwd, NetworkInfo.CurrentDomain);
                SecurePwd.Dispose();
                DialogResult = DialogResult.OK;
            }
            else
            {
                OtherFunctions.Message("Username or Password incomplete.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Missing Info", this);
            }
        }

        private void cmdAccept_Click(object sender, EventArgs e)
        {
            Accept();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Accept();
            }
            if (e.KeyCode == Keys.Back)
            {
                if (SecurePwd.Length > 0)
                {
                    SecurePwd.RemoveAt(SecurePwd.Length - 1);
                }
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != Convert.ToChar(Keys.Back) & e.KeyChar != Convert.ToChar(Keys.Enter))
            {
                SecurePwd.AppendChar(e.KeyChar);
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            SetText();
        }

        private void SetText()
        {
            txtPassword.Text = BlankText(SecurePwd.Length);
            txtPassword.SelectionStart = txtPassword.Text.Length + 1;
        }

        private string BlankText(int length)
        {
            if (length > 0)
            {
                return new string(char.Parse("*"), length);
            }
            return string.Empty;
        }
    }
}
