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
using System.ComponentModel;
using AssetManager.UserInterface.CustomControls;

namespace AssetManager.UserInterface.Forms.AssetManager
{

	public partial class UpdateDev : ExtendedForm
	{

		public DeviceUpdateInfoStruct UpdateInfo {
			get { return NewUpdateInfo; }
		}


		private DeviceUpdateInfoStruct NewUpdateInfo;
		public UpdateDev(ExtendedForm parentForm, bool isNoteOnly = false)
		{
			InitializeComponent();
			this.ParentForm = parentForm;
			AttribIndexFunctions.FillComboBox(GlobalInstances.DeviceAttribute.ChangeType,  UpdateTypeCombo);
			if (isNoteOnly) {
				UpdateTypeCombo.SelectedIndex = AttribIndexFunctions.GetComboIndexFromCode(GlobalInstances.DeviceAttribute.ChangeType, "NOTE");
				UpdateTypeCombo.Enabled = false;
				ValidateUpdateType();
			} else {
				UpdateTypeCombo.SelectedIndex = -1;
			}
			ShowDialog(parentForm);
		}

		private void SubmitButton_Click(object sender, EventArgs e)
		{
			NewUpdateInfo.Note = Strings.Trim(NotesTextBox.Rtf);
			NewUpdateInfo.ChangeType = AttribIndexFunctions.GetDBValue(GlobalInstances.DeviceAttribute.ChangeType, UpdateTypeCombo.SelectedIndex);
			NotesTextBox.Text = "";
			UpdateTypeCombo.Enabled = true;
			this.DialogResult = DialogResult.OK;
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private bool ValidateUpdateType()
		{
			if (UpdateTypeCombo.SelectedIndex > 0) {
				ErrorProvider1.SetError(UpdateTypeCombo, "");
				SubmitButton.Enabled = true;
				return true;
			} else {
				SubmitButton.Enabled = false;
				UpdateTypeCombo.Focus();
				ErrorProvider1.SetError(UpdateTypeCombo, "Please select a change type.");
			}
			return false;
		}

		private void UpdateTypeCombo_ChangeType_Validating(object sender, CancelEventArgs e)
		{
			e.Cancel = !ValidateUpdateType();
		}

		private void UpdateTypeCombo_SelectionChangeCommitted(object sender, EventArgs e)
		{
			ValidateUpdateType();
		}

	}
}
