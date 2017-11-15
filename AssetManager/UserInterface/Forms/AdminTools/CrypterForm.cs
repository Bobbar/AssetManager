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
using AssetManager.UserInterface.CustomControls;


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
			if (!string.IsNullOrEmpty(Strings.Trim(txtString.Text))) {
				string CryptKey = Strings.Trim(txtKey.Text);
				string CryptString = Strings.Trim(txtString.Text);
				using (Simple3Des wrapper = new Simple3Des(CryptKey)) {
					txtResult.Text = wrapper.EncryptData(CryptString);
				}
			} else if (!string.IsNullOrEmpty(Strings.Trim(txtResult.Text))) {
				using (Simple3Des wrapper = new Simple3Des(Strings.Trim(txtKey.Text))) {
					txtString.Text = wrapper.DecryptData(Strings.Trim(txtResult.Text));
				}
			}
		}

		private void cmdClear_Click(object sender, EventArgs e)
		{
			foreach (Control ctl in GroupBox1.Controls) {
				if ((ctl) is TextBox) {
					var txt = (TextBox)ctl;
					txt.Clear();
				}
			}
		}

	}
}
