using AssetManager.UserInterface.CustomControls;
using System;
using System.Windows.Forms;

namespace AssetManager.UserInterface.Forms.AdminTools
{
    public partial class CrypterForm : ExtendedForm
    {
        public CrypterForm(ExtendedForm parentForm)
        {
            InitializeComponent();
            this.ParentForm = parentForm;
            Show();
        }

        private void cmdEncode_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtString.Text.Trim()))
            {
                string CryptKey = txtKey.Text.Trim();
                string CryptString = txtString.Text.Trim();
                using (Simple3Des wrapper = new Simple3Des(CryptKey))
                {
                    txtResult.Text = wrapper.EncryptData(CryptString);
                }
            }
            else if (!string.IsNullOrEmpty(txtResult.Text.Trim()))
            {
                using (Simple3Des wrapper = new Simple3Des(txtKey.Text.Trim()))
                {
                    txtString.Text = wrapper.DecryptData(txtResult.Text.Trim());
                }
            }
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in GroupBox1.Controls)
            {
                if ((ctl) is TextBox)
                {
                    var txt = (TextBox)ctl;
                    txt.Clear();
                }
            }
        }
    }
}