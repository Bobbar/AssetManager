using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using AssetManager.UserInterface.Forms.Attachments;
using AssetManager.UserInterface.Forms.AssetManagement;
using AssetManager.UserInterface.Forms.Sibi;
using AssetManager.UserInterface.Forms.GK_Updater;
using AssetManager.UserInterface.Forms.AdminTools;


namespace AssetManager
{
    public class LiveBox : IDisposable
    {

        #region "Fields"

        private LiveBoxArgs CurrentLiveBoxArgs;
        private ListBox LiveListBox;
        private List<LiveBoxArgs> LiveBoxControls = new List<LiveBoxArgs>();
        private bool QueryRunning = false;
        private int RowLimit = 30;

        private string strPrevSearchString;
        #endregion

        #region "Constructors"

        public LiveBox(Form parentForm)
        {
            InitializeControl(parentForm);
        }

        #endregion

        #region "Methods"

        public void AttachToControl(TextBox control, string displayMember, LiveBoxType type, string valueMember = null)
        {
            LiveBoxArgs ControlArgs = new LiveBoxArgs(control, displayMember, type, valueMember);
            LiveBoxControls.Add(ControlArgs);
            ControlArgs.Control.KeyUp += Control_KeyUp;
            ControlArgs.Control.KeyDown += Control_KeyDown;
            ControlArgs.Control.LostFocus += Control_LostFocus;
            ControlArgs.Control.ReadOnlyChanged += Control_LostFocus;
        }

        public void GiveLiveBoxFocus()
        {
            LiveListBox.Focus();
            if (LiveListBox.SelectedIndex == -1)
            {
                LiveListBox.SelectedIndex = 0;
            }
        }

        public void HideLiveBox()
        {
            LiveListBox.Visible = false;
            LiveListBox.DataSource = null;
        }

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                GiveLiveBoxFocus();
            }
        }

        private void Control_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                HideLiveBox();
            }
            else
            {
                //don't respond to non-alpha keys
                if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.Alt || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Menu)
                {
                    //do nothing             
                }
                else
                {
                    LiveBoxArgs arg = GetSenderArgs(sender);
                    if (!arg.Control.ReadOnly)
                    {
                        StartLiveSearch(arg);
                    }
                }

                //switch (e.KeyCode)
                //{
                //    case Keys.ShiftKey:
                //    case Keys.Alt:
                //    case Keys.ControlKey:
                //    case Keys.Menu:
                //    //do nothing
                //    default:
                //        object arg = GetSenderArgs(sender);
                //        if (!arg.Control.ReadOnly)
                //        {
                //            StartLiveSearch(arg);
                //        }
                //}
            }
        }

        private void Control_LostFocus(object sender, EventArgs e)
        {
            if (CurrentLiveBoxArgs.Control != null)
            {
                if (!CurrentLiveBoxArgs.Control.Focused & !LiveListBox.Focused)
                {
                    if (LiveListBox.Visible)
                        HideLiveBox();
                }
                if (!CurrentLiveBoxArgs.Control.Enabled)
                {
                    if (LiveListBox.Visible)
                        HideLiveBox();
                }
                if (CurrentLiveBoxArgs.Control is TextBox)
                {
                    TextBox txt = CurrentLiveBoxArgs.Control;
                    if (txt.ReadOnly)
                    {
                        if (LiveListBox.Visible)
                            HideLiveBox();
                    }
                }
            }
        }

        private void DrawLiveBox(DataTable dtResults)
        {
            try
            {
                if (dtResults.Rows.Count > 0)
                {
                    LiveListBox.SuspendLayout();
                    LiveListBox.BeginUpdate();
                    LiveListBox.DataSource = dtResults;
                    LiveListBox.DisplayMember = CurrentLiveBoxArgs.DisplayMember;
                    LiveListBox.ValueMember = CurrentLiveBoxArgs.ValueMember;
                    LiveListBox.ClearSelected();
                    PosistionLiveBox();
                    LiveListBox.Visible = true;
                    LiveListBox.EndUpdate();
                    LiveListBox.ResumeLayout();
                    if (strPrevSearchString != CurrentLiveBoxArgs.Control.Text.Trim())
                    {
                        StartLiveSearch(CurrentLiveBoxArgs);
                        //if search string has changed since last completion, run again.
                    }
                }
                else
                {
                    LiveListBox.Visible = false;
                }
            }
            catch
            {
                HideLiveBox();
            }
        }

        private LiveBoxArgs GetSenderArgs(object sender)
        {
            foreach (LiveBoxArgs arg in LiveBoxControls)
            {
                if (object.ReferenceEquals(arg.Control, (TextBox)sender))
                {
                    return arg;
                }
            }
            return null;

        }

        private void InitializeControl(Form parentForm)
        {
            LiveListBox = new ListBox();
            LiveListBox.Parent = parentForm;
            LiveListBox.BringToFront();
            //AddHandler LiveBox.MouseClick, AddressOf LiveBox_MouseClick
           
            LiveListBox.MouseDown += LiveBox_MouseDown;
            LiveListBox.MouseMove += LiveBox_MouseMove;
            LiveListBox.KeyDown += LiveBox_KeyDown;
            LiveListBox.LostFocus += LiveBox_LostFocus;
            ExtendedMethods.DoubleBufferedListBox(LiveListBox, true);
            LiveListBox.Visible = false;
            SetStyle();
            CurrentLiveBoxArgs = new LiveBoxArgs();
        }

        private void LiveBox_LostFocus(object sender, EventArgs e)
        {
            HideLiveBox();
        }

        private void LiveBox_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.KeyCode == Keys.Enter)
            {
                LiveBoxSelect();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideLiveBox();
            }


            //switch (e.KeyCode)
            //{
            //    case Keys.Enter:
            //        LiveBoxSelect();
            //    case Keys.Escape:
            //        HideLiveBox();
            //}
        }

        private void LiveBox_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                LiveBoxSelect();
            }
            else if (e.Button == MouseButtons.Right)
            {
                HideLiveBox();
            }



            //switch (e.Button)
            //{
            //    case MouseButtons.Left:
            //        LiveBoxSelect();
            //    case MouseButtons.Right:
            //        HideLiveBox();
            //}
        }

        private void LiveBox_MouseMove(object sender, MouseEventArgs e)
        {
            LiveListBox.SelectedIndex = LiveListBox.IndexFromPoint(e.Location);
        }

        private void LiveBoxSelect()
        {
            string SelectedText = LiveListBox.Text;
            string SelectedValue = LiveListBox.SelectedValue.ToString();
            HideLiveBox();


            if (CurrentLiveBoxArgs.Type == LiveBoxType.DynamicSearch)
            {
                CurrentLiveBoxArgs.Control.Text = SelectedText;
                //MainForm.DynamicSearch(); //TODO: Find another way to call this.
            }
            else if (CurrentLiveBoxArgs.Type == LiveBoxType.InstaLoad)
            {
                CurrentLiveBoxArgs.Control.Text = "";
                //MainForm.LoadDevice(SelectedValue); //TODO: Find another way to call this.
            }
            else if (CurrentLiveBoxArgs.Type == LiveBoxType.SelectValue)
            {
                CurrentLiveBoxArgs.Control.Text = SelectedText;
            }
            else if (CurrentLiveBoxArgs.Type == LiveBoxType.UserSelect)
            {
                CurrentLiveBoxArgs.Control.Text = SelectedText;
                if (CurrentLiveBoxArgs.Control.FindForm() is ViewDeviceForm)
                {
                    ViewDeviceForm FrmSetData = (ViewDeviceForm)CurrentLiveBoxArgs.Control.FindForm();
                    FrmSetData.MunisUser.Name = SelectedText;
                    FrmSetData.MunisUser.Number = SelectedValue;
                }
                else if (CurrentLiveBoxArgs.Control.FindForm() is NewDeviceForm)
                {
                    NewDeviceForm FrmSetData = (NewDeviceForm)CurrentLiveBoxArgs.Control.FindForm();
                    FrmSetData.MunisUser = new MunisEmployeeStruct(SelectedText, SelectedValue);
                }
            }



            //switch (CurrentLiveBoxArgs.Type)
            //{
            //    case LiveBoxType.DynamicSearch:
            //        CurrentLiveBoxArgs.Control.Text = SelectedText;
            //        MainForm.DynamicSearch();
            //    case LiveBoxType.InstaLoad:
            //        CurrentLiveBoxArgs.Control.Text = "";
            //        MainForm.LoadDevice(SelectedValue);
            //    case LiveBoxType.SelectValue:
            //        CurrentLiveBoxArgs.Control.Text = SelectedText;
            //    case LiveBoxType.UserSelect:
            //        CurrentLiveBoxArgs.Control.Text = SelectedText;
            //        if (CurrentLiveBoxArgs.Control.FindForm is ViewDeviceForm)
            //        {
            //            ViewDeviceForm FrmSetData = (ViewDeviceForm)CurrentLiveBoxArgs.Control.FindForm;
            //            FrmSetData.MunisUser.Name = SelectedText;
            //            FrmSetData.MunisUser.Number = SelectedValue;
            //        }
            //        else if (CurrentLiveBoxArgs.Control.FindForm is NewDeviceForm)
            //        {
            //            NewDeviceForm FrmSetData = (NewDeviceForm)CurrentLiveBoxArgs.Control.FindForm;
            //            FrmSetData.MunisUser = new MunisEmployeeStruct(SelectedText, SelectedValue);
            //        }
            //}
        }

        private void PosistionLiveBox()
        {
            Point ScreenPos = LiveListBox.Parent.PointToClient(CurrentLiveBoxArgs.Control.Parent.PointToScreen(CurrentLiveBoxArgs.Control.Location));
            ScreenPos.Y += CurrentLiveBoxArgs.Control.Height - 1;
            LiveListBox.Location = ScreenPos;
            LiveListBox.Width = PreferredWidth();
            Rectangle FormBounds = LiveListBox.Parent.ClientRectangle;
            if (LiveListBox.PreferredHeight + LiveListBox.Top > FormBounds.Bottom)
            {
                LiveListBox.Height = FormBounds.Bottom - LiveListBox.Top - LiveListBox.Padding.Bottom;
            }
            else
            {
                LiveListBox.Height = LiveListBox.PreferredHeight;
            }
        }

        private int PreferredWidth()
        {
            using (Graphics gfx = LiveListBox.CreateGraphics())
            {
                int MaxLen = 0;
                foreach (DataRowView row in LiveListBox.Items)
                {
                    int ItemLen = (int)gfx.MeasureString(row[CurrentLiveBoxArgs.DisplayMember].ToString(), LiveListBox.Font).Width;
                    if (ItemLen > MaxLen)
                    {
                        MaxLen = ItemLen;
                    }
                }
                if (MaxLen > CurrentLiveBoxArgs.Control.Width)
                {
                    return MaxLen;
                }
                else
                {
                    return CurrentLiveBoxArgs.Control.Width;
                }
            }
        }

        /// <summary>
        /// Runs the DB query Asynchronously.
        /// </summary>
        /// <param name="SearchString"></param>
        private async void ProcessSearch(string searchString)
        {
            strPrevSearchString = searchString;
            try
            {
                DataTable Results = await Task.Run(() =>
                {
                    string strQry;
                    //strQry = "SELECT " + DevicesCols.DeviceUID + "," + IIf(CurrentLiveBoxArgs.ValueMember == null, CurrentLiveBoxArgs.DisplayMember, CurrentLiveBoxArgs.DisplayMember + "," + CurrentLiveBoxArgs.ValueMember).ToString + " FROM " + DevicesCols.TableName + " WHERE " + CurrentLiveBoxArgs.DisplayMember + " LIKE  @Search_Value  GROUP BY " + CurrentLiveBoxArgs.DisplayMember + " ORDER BY " + CurrentLiveBoxArgs.DisplayMember + " LIMIT " + RowLimit;

                    if (CurrentLiveBoxArgs.ValueMember == null)
                    {
                        strQry = "SELECT " + DevicesCols.DeviceUID + "," + CurrentLiveBoxArgs.DisplayMember + " FROM " + DevicesCols.TableName + " WHERE " + CurrentLiveBoxArgs.DisplayMember + " LIKE  @Search_Value  GROUP BY " + CurrentLiveBoxArgs.DisplayMember + " ORDER BY " + CurrentLiveBoxArgs.DisplayMember + " LIMIT " + RowLimit;
                    }
                    else
                    {
                        strQry = "SELECT " + DevicesCols.DeviceUID + "," + CurrentLiveBoxArgs.DisplayMember + "," + CurrentLiveBoxArgs.ValueMember + " FROM " + DevicesCols.TableName + " WHERE " + CurrentLiveBoxArgs.DisplayMember + " LIKE  @Search_Value  GROUP BY " + CurrentLiveBoxArgs.DisplayMember + " ORDER BY " + CurrentLiveBoxArgs.DisplayMember + " LIMIT " + RowLimit;
                    }

                    using (var cmd = DBFactory.GetDatabase().GetCommand(strQry))
                    {
                        cmd.AddParameterWithValue("@Search_Value", "%" + searchString + "%");
                        return DBFactory.GetDatabase().DataTableFromCommand(cmd);
                    }
                });

                DrawLiveBox(Results);
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void SetStyle()
        {
            Font LiveBoxFont = new Font(new FontFamily("Consolas"), 11.25f, FontStyle.Bold);
            LiveListBox.BackColor = Color.FromArgb(255, 208, 99);
            LiveListBox.BorderStyle = BorderStyle.FixedSingle;
            LiveListBox.Font = LiveBoxFont;
            LiveListBox.ForeColor = Color.Black;
            LiveListBox.Padding = new Padding(0, 0, 0, 10);

        }

        private void StartLiveSearch(LiveBoxArgs args)
        {
            CurrentLiveBoxArgs = args;
            string strSearchString = CurrentLiveBoxArgs.Control.Text.Trim();
            if (strSearchString != "")
            {
                ProcessSearch(strSearchString);
            }
            else
            {
                HideLiveBox();
            }
        }

        private void RemovedHandlers()
        {
            foreach (var control in LiveBoxControls)
            {
                control.Control.KeyUp -= Control_KeyUp;
                control.Control.KeyDown -= Control_KeyDown;
                control.Control.LostFocus -= Control_LostFocus;
                control.Control.ReadOnlyChanged -= Control_LostFocus;
            }
        }

        #endregion

        #region "Structs"

        public class LiveBoxArgs
        {

            public TextBox Control { get; set; }
            public string DisplayMember { get; set; }
            public Nullable<LiveBoxType> Type { get; set; }
            public string ValueMember { get; set; }

            public LiveBoxArgs(TextBox control, string displayMember, AssetManager.LiveBoxType type, string valueMember)
            {
                this.Control = control;
                this.DisplayMember = displayMember;
                this.Type = type;
                this.ValueMember = valueMember;
            }

            public LiveBoxArgs()
            {
                this.Control = null;
                this.DisplayMember = null;
                this.Type = null;
                this.ValueMember = null;
            }


        }

        #endregion

        #region "IDisposable Support"

        // To detect redundant calls
        private bool disposedValue;

        // TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        //Protected Overrides Sub Finalize()
        //    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        //    Dispose(False)
        //    MyBase.Finalize()
        //End Sub
        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            // TODO: uncomment the following line if Finalize() is overridden above.
            // GC.SuppressFinalize(Me)
        }

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    RemovedHandlers();
                    LiveListBox.Dispose();
                    LiveBoxControls.Clear();
                }
                // TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                // TODO: set large fields to null.
            }
            disposedValue = true;
        }

        #endregion

    }
}