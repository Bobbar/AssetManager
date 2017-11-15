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

namespace AssetManager.UserInterface.Forms.AssetManagement
{

	public partial class ViewTrackingForm : ExtendedForm
	{

		public ViewTrackingForm(ExtendedForm parentForm, string entryGUID, DeviceObject device)
		{
			InitializeComponent();
			this.ParentForm = parentForm;
			FormUID = entryGUID;
			ViewTrackingEntry(entryGUID, device);
			Show();
		}

		private void ViewTrackingEntry(string entryUID, DeviceObject device)
		{
			try {
				OtherFunctions.SetWaitCursor(true, this);
				var strQry = "Select * FROM " + TrackablesCols.TableName + " WHERE  " + TrackablesCols.UID + " = '" + entryUID + "'";
				using (DataTable results = DBFactory.GetDatabase().DataTableFromQueryString(strQry)) {
					foreach (DataRow r in results.Rows) {
						txtTimeStamp.Text = DataConsistency.NoNull(r[TrackablesCols.DateStamp]);
						txtCheckType.Text = DataConsistency.NoNull(r[TrackablesCols.CheckType]);
						if (txtCheckType.Text == "IN") {
							txtCheckType.BackColor = Colors.CheckIn;
						} else if (txtCheckType.Text == "OUT") {
							txtCheckType.BackColor = Colors.CheckOut;
						}
						txtDescription.Text = device.Description;
						txtGUID.Text = DataConsistency.NoNull(r[TrackablesCols.DeviceUID]);
						txtCheckOutUser.Text = DataConsistency.NoNull(r[TrackablesCols.CheckoutUser]);
						txtCheckInUser.Text = DataConsistency.NoNull(r[TrackablesCols.CheckinUser]);
						txtLocation.Text = DataConsistency.NoNull(r[TrackablesCols.UseLocation]);
						txtAssetTag.Text = device.AssetTag;
						txtCheckOutTime.Text = DataConsistency.NoNull(r[TrackablesCols.CheckoutTime]);
						txtDueBack.Text = DataConsistency.NoNull(r[TrackablesCols.DueBackDate]);
						txtSerial.Text = device.Serial;
						txtCheckInTime.Text = DataConsistency.NoNull(r[TrackablesCols.CheckinTime]);
						txtNotes.Text = DataConsistency.NoNull(r[TrackablesCols.Notes]);
						txtEntryGUID.Text = DataConsistency.NoNull(r[TrackablesCols.UID]);
						this.Text = this.Text + " - " + DataConsistency.NoNull(r[TrackablesCols.DateStamp]);
					}
				}
			} catch (Exception ex) {
				ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
			} finally {
				OtherFunctions.SetWaitCursor(false, this);
			}
		}

		private void cmdClose_Click(object sender, EventArgs e)
		{
			this.Dispose();
		}

	}
}
