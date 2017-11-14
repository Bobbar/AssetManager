using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using Microsoft.VisualBasic;
using AssetManager.UserInterface.CustomControls;

namespace AssetManager.UserInterface.Forms.AssetManager
{
    public partial class TrackDeviceForm : ExtendedForm
    {
        private DeviceObject CurrentTrackingDevice = new DeviceObject();
        private DeviceTrackingObject CheckData = new DeviceTrackingObject();

        public TrackDeviceForm(DeviceObject device, ExtendedForm parentForm)
        {
            InitializeComponent();
            CurrentTrackingDevice = device;
            this.ParentForm = parentForm;
            ClearAll();
            SetDates();
            SetGroups();
            GetCurrentTracking(System.Convert.ToString(CurrentTrackingDevice.GUID));
            LoadTracking();
            Show();
        }

        private bool GetCheckData()
        {
            if (!CurrentTrackingDevice.Tracking.IsCheckedOut)
            {
                Control c = default(Control);
                foreach (Control tempLoopVar_c in CheckOutBox.Controls)
                {
                    c = tempLoopVar_c;
                    if (c is TextBox)
                    {
                        if (c.Visible)
                        {
                            if (Strings.Trim(System.Convert.ToString(c.Text)) == "")
                            {
                                OtherFunctions.Message("Please complete all fields.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Missing Data", this);
                                return false;
                            }
                        }
                    }
                }
            }
            else
            {
                Control c = default(Control);
                foreach (Control tempLoopVar_c in CheckInBox.Controls)
                {
                    c = tempLoopVar_c;
                    if (c is TextBox)
                    {
                        if (Strings.Trim(System.Convert.ToString(c.Text)) == "")
                        {
                            OtherFunctions.Message("Please complete all fields.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Missing Data", this);
                            return false;
                        }
                    }
                }
            }
            CheckData.CheckoutTime = dtCheckOut.Value; //.ToString(strDBDateTimeFormat)
            CheckData.DueBackTime = dtDueBack.Value; //.ToString(strDBDateTimeFormat)
            CheckData.UseLocation = Strings.Trim(System.Convert.ToString(txtUseLocation.Text)).ToUpper();
            CheckData.UseReason = Strings.Trim(System.Convert.ToString(txtUseReason.Text)).ToUpper();
            CheckData.CheckinNotes = Strings.Trim(System.Convert.ToString(txtCheckInNotes.Text)).ToUpper();
            CheckData.DeviceGUID = CurrentTrackingDevice.GUID;
            CheckData.CheckoutUser = GlobalConstants.LocalDomainUser;
            CheckData.CheckinTime = dtCheckIn.Value; //.ToString(strDBDateTimeFormat)
            CheckData.CheckinUser = GlobalConstants.LocalDomainUser;
            return true;
        }

        private void GetCurrentTracking(string strGUID)
        {
            using (DataTable results = DBFactory.GetDatabase().DataTableFromQueryString("SELECT * FROM " + TrackablesCols.TableName + " WHERE " + TrackablesCols.DeviceUID + "='" + strGUID + "' ORDER BY " + TrackablesCols.DateStamp + " DESC LIMIT 1")) //ds.Tables(0)
            {
                if (results.Rows.Count > 0)
                {
                    CurrentTrackingDevice.MapClassProperties(CurrentTrackingDevice, results);
                }
            }

        }

        private void LoadTracking()
        {
            txtAssetTag.Text = CurrentTrackingDevice.AssetTag;
            txtDescription.Text = CurrentTrackingDevice.Description;
            txtSerial.Text = CurrentTrackingDevice.Serial;
            txtDeviceType.Text = AttribIndexFunctions.GetDisplayValueFromCode(GlobalInstances.DeviceAttribute.EquipType, CurrentTrackingDevice.EquipmentType);
            if (CurrentTrackingDevice.Tracking.IsCheckedOut)
            {
                dtCheckOut.Value = CurrentTrackingDevice.Tracking.CheckoutTime;
                dtDueBack.Value = CurrentTrackingDevice.Tracking.DueBackTime;
                txtUseLocation.Text = CurrentTrackingDevice.Tracking.UseLocation;
                txtUseReason.Text = CurrentTrackingDevice.Tracking.UseReason;
            }
        }

        private void ClearAll()
        {
            Control c = default(Control);
            foreach (Control tempLoopVar_c in this.Controls)
            {
                c = tempLoopVar_c;
                if (c is GroupBox)
                {
                    Control gc = default(Control);
                    foreach (Control tempLoopVar_gc in c.Controls)
                    {
                        gc = tempLoopVar_gc;
                        if (gc is TextBox)
                        {
                            TextBox txt = (TextBox)gc;
                            txt.Text = "";
                        }
                    }
                }
            }
        }

        private void SetDates()
        {
            dtCheckOut.Value = DateTime.Now;
            dtCheckIn.Value = DateTime.Now;
            dtCheckOut.Enabled = false;
            dtCheckIn.Enabled = false;
        }

        private void SetGroups()
        {
            CheckInBox.Enabled = CurrentTrackingDevice.Tracking.IsCheckedOut;
            CheckOutBox.Enabled = !CurrentTrackingDevice.Tracking.IsCheckedOut;
        }

        private void CheckOut()
        {
            using (var trans = DBFactory.GetDatabase().StartTransaction())
            {
                using (var conn = trans.Connection)
                {
                    try
                    {
                        if (!GetCheckData())
                        {
                            return;
                        }
                        OtherFunctions.SetWaitCursor(true, this);
                        int rows = 0;
                        rows += System.Convert.ToInt32(DBFactory.GetDatabase().UpdateValue(DevicesCols.TableName, DevicesCols.CheckedOut, 1, DevicesCols.DeviceUID, CurrentTrackingDevice.GUID, trans));

                        List<DBParameter> CheckOutParams = new List<DBParameter>();
                        CheckOutParams.Add(new DBParameter(TrackablesCols.CheckType, CheckType.Checkout));
                        CheckOutParams.Add(new DBParameter(TrackablesCols.CheckoutTime, CheckData.CheckoutTime));
                        CheckOutParams.Add(new DBParameter(TrackablesCols.DueBackDate, CheckData.DueBackTime));
                        CheckOutParams.Add(new DBParameter(TrackablesCols.CheckoutUser, CheckData.CheckoutUser));
                        CheckOutParams.Add(new DBParameter(TrackablesCols.UseLocation, CheckData.UseLocation));
                        CheckOutParams.Add(new DBParameter(TrackablesCols.Notes, CheckData.UseReason));
                        CheckOutParams.Add(new DBParameter(TrackablesCols.DeviceUID, CheckData.DeviceGUID));
                        rows += System.Convert.ToInt32(DBFactory.GetDatabase().InsertFromParameters(TrackablesCols.TableName, CheckOutParams, trans));

                        if (rows == 2)
                        {
                            trans.Commit();
                            OtherFunctions.Message("Device Checked Out!", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Success", this);
                        }
                        else
                        {
                            trans.Rollback();
                            OtherFunctions.Message("Unsuccessful! The number of affected rows was not expected.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Unexpected Result", this);
                        }
                        ParentForm.RefreshData();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                    }
                    finally
                    {
                        this.Dispose();
                    }
                }
            }

        }

        private void CheckIn()
        {
            using (var trans = DBFactory.GetDatabase().StartTransaction())
            {
                using (var conn = trans.Connection)
                {
                    try
                    {
                        if (!GetCheckData())
                        {
                            return;
                        }
                        OtherFunctions.SetWaitCursor(true, this);
                        int rows = 0;
                        rows += System.Convert.ToInt32(DBFactory.GetDatabase().UpdateValue(DevicesCols.TableName, DevicesCols.CheckedOut, 0, DevicesCols.DeviceUID, CurrentTrackingDevice.GUID, trans));

                        List<DBParameter> CheckOutParams = new List<DBParameter>();
                        CheckOutParams.Add(new DBParameter(TrackablesCols.CheckType, CheckType.Checkin));
                        CheckOutParams.Add(new DBParameter(TrackablesCols.CheckoutTime, CheckData.CheckoutTime));
                        CheckOutParams.Add(new DBParameter(TrackablesCols.DueBackDate, CheckData.DueBackTime));
                        CheckOutParams.Add(new DBParameter(TrackablesCols.CheckinTime, CheckData.CheckinTime));
                        CheckOutParams.Add(new DBParameter(TrackablesCols.CheckoutUser, CheckData.CheckoutUser));
                        CheckOutParams.Add(new DBParameter(TrackablesCols.CheckinUser, CheckData.CheckinUser));
                        CheckOutParams.Add(new DBParameter(TrackablesCols.UseLocation, CheckData.UseLocation));
                        CheckOutParams.Add(new DBParameter(TrackablesCols.Notes, CheckData.CheckinNotes));
                        CheckOutParams.Add(new DBParameter(TrackablesCols.DeviceUID, CheckData.DeviceGUID));
                        rows += System.Convert.ToInt32(DBFactory.GetDatabase().InsertFromParameters(TrackablesCols.TableName, CheckOutParams, trans));

                        if (rows == 2)
                        {
                            trans.Commit();
                            OtherFunctions.Message("Device Checked In!", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Success", this);
                        }
                        else
                        {
                            trans.Rollback();
                            OtherFunctions.Message("Unsuccessful! The number of affected rows was not what was expected.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Unexpected Result", this);
                        }
                        ParentForm.RefreshData();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                    }
                    finally
                    {
                        this.Dispose();
                    }
                }
            }

        }

        private void cmdCheckOut_Click(object sender, EventArgs e)
        {
            CheckOut();
        }

        private void cmdCheckIn_Click(object sender, EventArgs e)
        {
            CheckIn();
        }

        private void txtUseLocation_LostFocus(object sender, EventArgs e)
        {
            txtUseLocation.Text = Strings.Trim(System.Convert.ToString(txtUseLocation.Text)).ToUpper();
        }

        private void txtUseReason_LostFocus(object sender, EventArgs e)
        {
            txtUseReason.Text = Strings.Trim(System.Convert.ToString(txtUseReason.Text)).ToUpper();
        }

        private void txtCheckInNotes_LostFocus(object sender, EventArgs e)
        {
            txtCheckInNotes.Text = Strings.Trim(System.Convert.ToString(txtCheckInNotes.Text)).ToUpper();
        }

    }
}