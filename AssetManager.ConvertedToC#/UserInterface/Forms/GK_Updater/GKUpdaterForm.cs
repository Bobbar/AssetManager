using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.Security;
using System.ComponentModel;

namespace AssetManager
{
    public partial class GKUpdaterForm
    {
        private bool QueueIsRunning = false;
        private int MaxSimUpdates = 4;
        private List<GKProgressControl> MyUpdates = new List<GKProgressControl>();
        private bool bolStarting = true;
        private bool bolCheckForDups = true;
        private bool bolCreateMissingDirs = false;

        private bool PackFileReady = false;

        public GKUpdaterForm()
        {
            // This call is required by the designer.
            InitializeComponent();
            bolStarting = false;
            MaxUpdates.Value = MaxSimUpdates;
            // Add any initialization after the InitializeComponent() call.

            ExtendedMethods.DoubleBufferedFlowLayout(Updater_Table, true);

        }

        public void AddMultipleUpdates(List<DeviceObject> devices)
        {
            try
            {
                OtherFunctions.SetWaitCursor(true, this);
                foreach (DeviceObject device in devices)
                {
                    if (bolCheckForDups && !Exists(device))
                    {
                        GKProgressControl NewProgCtl = new GKProgressControl(this, device, bolCreateMissingDirs, Paths.GKExtractDir, MyUpdates.Count + 1);
                        MyUpdates.Add(NewProgCtl);
                        NewProgCtl.CriticalStopError += CriticalStop;
                    }
                }

                Updater_Table.SuspendLayout();
                this.SuspendLayout();

                Updater_Table.Controls.AddRange(MyUpdates.ToArray());

                Updater_Table.ResumeLayout();
                this.ResumeLayout();

                ProcessUpdates();
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
            finally
            {
                OtherFunctions.SetWaitCursor(false, this);
            }
        }

        public void AddUpdate(DeviceObject device)
        {
            try
            {
                if (bolCheckForDups && !Exists(device))
                {
                    GKProgressControl NewProgCtl = new GKProgressControl(this, device, bolCreateMissingDirs, Paths.GKExtractDir, MyUpdates.Count + 1);
                    Updater_Table.Controls.Add(NewProgCtl);
                    MyUpdates.Add(NewProgCtl);
                    NewProgCtl.CriticalStopError += CriticalStop;
                    ProcessUpdates();
                }
                else
                {
                    var blah = OtherFunctions.Message("An update for device " + device.Serial + " already exists.  Do you want to restart the update for this device?", (int)MessageBoxButtons.OKCancel + (int)MessageBoxIcon.Exclamation, "Duplicate Update", this);
                    if (blah == MsgBoxResult.Ok)
                    {
                        StartUpdateByDevice(device);

                    }
                }
            }
            finally
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }

        private void StartUpdateByDevice(DeviceObject device)
        {
            MyUpdates.Find(upd => upd.Device.GUID == device.GUID).StartUpdate();
        }

        private bool Exists(DeviceObject device)
        {
            return MyUpdates.Exists(upd => upd.Device.GUID == device.GUID);
        }

        public bool ActiveUpdates()
        {
            return MyUpdates.Exists(upd => upd.ProgStatus == GKProgressControl.ProgressStatus.Running);
        }

        private void CancelAll()
        {
            foreach (GKProgressControl upd in MyUpdates)
            {
                if (upd.ProgStatus == GKProgressControl.ProgressStatus.Running)
                {
                    upd.CancelUpdate();
                }
            }
        }

        private void DisposeUpdates()
        {
            foreach (GKProgressControl upd in MyUpdates)
            {
                if (!upd.IsDisposed)
                {
                    upd.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns True if number of running updates is less than the maximum simultaneous allowed updates and RunQueue is True.
        /// </summary>
        /// <returns></returns>
        private bool CanRunMoreUpdates()
        {
            if (QueueIsRunning)
            {
                int RunningUpdates = 0;
                foreach (GKProgressControl upd in MyUpdates)
                {

                    if (upd.ProgStatus == GKProgressControl.ProgressStatus.Running || upd.ProgStatus == GKProgressControl.ProgressStatus.Starting || upd.ProgStatus == GKProgressControl.ProgressStatus.Paused)
                    {
                        if (!upd.IsDisposed)
                            RunningUpdates += 1;
                    }
                    //switch (upd.ProgStatus)
                    //{
                    //    case GKProgressControl.ProgressStatus.Running:
                    //    case GKProgressControl.ProgressStatus.Starting:
                    //    case GKProgressControl.ProgressStatus.Paused:
                    //        if (!upd.IsDisposed)
                    //            RunningUpdates += 1;
                    //}
                }
                if (RunningUpdates < MaxSimUpdates)
                    return true;
            }
            return false;
        }

        // ERROR: Handles clauses are not supported in C#
        private void cmdCancelAll_Click(object sender, EventArgs e)
        {
            CancelAll();
            StopQueue();
        }

        // ERROR: Handles clauses are not supported in C#
        private void cmdPauseResume_Click(object sender, EventArgs e)
        {
            if (QueueIsRunning)
            {
                StopQueue();
            }
            else
            {
                StartQueue();
            }
        }

        // ERROR: Handles clauses are not supported in C#
        private void cmdSort_Click(object sender, EventArgs e)
        {
            SortUpdates();
        }

        private void CriticalStop(object sender, EventArgs e)
        {
            StopQueue();
            OtherFunctions.Message("The queue was stopped because of an access error. Please re-enter your credentials.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Queue Stopped", this);
            SecurityTools.ClearAdminCreds();

            if (SecurityTools.VerifyAdminCreds())
            {
                StartQueue();
            }
        }

        // ERROR: Handles clauses are not supported in C#
        private void GKUpdater_Form_Closing(object sender, CancelEventArgs e)
        {
            if (!OKToClose())
            {
                e.Cancel = true;
            }
            else
            {
                DisposeUpdates();
                this.Dispose();
            }
        }

        public override bool OKToClose()
        {
            if (ActiveUpdates())
            {
                OtherFunctions.Message("There are still updates running!  Cancel the updates or wait for them to finish.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Close Aborted", this);
                this.Activate();
                this.WindowState = FormWindowState.Normal;
                return false;
            }
            return true;
        }

        // ERROR: Handles clauses are not supported in C#
        private void MaxUpdates_ValueChanged(object sender, EventArgs e)
        {
            if (!bolStarting)
                MaxSimUpdates = (int)MaxUpdates.Value;
        }

        private void ProcessUpdates()
        {
            if (CanRunMoreUpdates())
            {
                StartNextUpdate();
            }
            PruneQueue();
            SetStats();
        }

        /// <summary>
        /// Removes disposed update fragments from list.
        /// </summary>
        private void PruneQueue()
        {
            MyUpdates = MyUpdates.FindAll(upd => !upd.IsDisposed);
        }

        // ERROR: Handles clauses are not supported in C#
        private void QueueChecker_Tick(object sender, EventArgs e)
        {
            ProcessUpdates();
        }

        private void SetStats()
        {
            int intQueued = 0;
            int intRunning = 0;
            int intComplete = 0;
            double TransferRateSum = 0;
            foreach (GKProgressControl upd in MyUpdates)
            {

                if (upd.ProgStatus == GKProgressControl.ProgressStatus.Queued)
                {
                    intQueued += 1;
                }
                else if (upd.ProgStatus == GKProgressControl.ProgressStatus.Running)
                {
                    TransferRateSum += upd.MyUpdater.UpdateStatus.CurTransferRate;
                    intRunning += 1;
                }
                else if (upd.ProgStatus == GKProgressControl.ProgressStatus.Complete)
                {
                    intComplete += 1;
                }

                //switch (upd.ProgStatus)
                //{
                //    case GKProgressControl.ProgressStatus.Queued:
                //        intQueued += 1;
                //    case GKProgressControl.ProgressStatus.Running:
                //        TransferRateSum += upd.MyUpdater.UpdateStatus.CurTransferRate;
                //        intRunning += 1;
                //    case GKProgressControl.ProgressStatus.Complete:
                //        intComplete += 1;
                //}
            }
            lblQueued.Text = "Queued: " + intQueued;
            lblRunning.Text = "Running: " + intRunning;
            lblComplete.Text = "Complete: " + intComplete;
            lblTotUpdates.Text = "Tot Updates: " + MyUpdates.Count;
            lblTransferRate.Text = "Total Transfer Rate: " + TransferRateSum.ToString("0.00") + " MB/s";
        }

        /// <summary>
        /// Sorts all the GKProgressControls in order of the <see cref="GKProgressControl.ProgressStatus"/> enum.
        /// </summary>
        private void SortUpdates()
        {
            List<GKProgressControl> sortUpdates = new List<GKProgressControl>();
            foreach (GKProgressControl.ProgressStatus status in Enum.GetValues(typeof(GKProgressControl.ProgressStatus)))
            {
                sortUpdates.AddRange(MyUpdates.FindAll(upd => upd.ProgStatus == status));
            }
            Updater_Table.Controls.Clear();
            Updater_Table.Controls.AddRange(sortUpdates.ToArray());
        }

        /// <summary>
        /// Starts the next update that has a queued status.
        /// </summary>
        private void StartNextUpdate()
        {
            GKProgressControl NextUpd = MyUpdates.Find(upd => upd.ProgStatus == GKProgressControl.ProgressStatus.Queued);
            if (NextUpd != null)
                NextUpd.StartUpdate();
        }

        private void RunQueue(bool canRunQueue)
        {
            if (canRunQueue)
            {
                StartQueue();
            }
            else
            {
                StopQueue();
            }
        }

        private void StartQueue()
        {
            QueueIsRunning = true;
            SetQueueButton();
        }

        private void StopQueue()
        {
            QueueIsRunning = false;
            SetQueueButton();
        }

        private void SetQueueButton()
        {
            if (QueueIsRunning)
            {
                cmdPauseResume.Text = "Pause Queue";
            }
            else
            {
                cmdPauseResume.Text = "Resume Queue";
            }
        }

        // ERROR: Handles clauses are not supported in C#
        private void tsmCreateDirs_Click(object sender, EventArgs e)
        {
            bolCreateMissingDirs = tsmCreateDirs.Checked;
        }

        // ERROR: Handles clauses are not supported in C#
        private async void GKUpdaterForm_Shown(object sender, EventArgs e)
        {
            SetQueueButton();
            await CheckPackFile();
        }

        private void ProcessPackFile()
        {
            using (PackFileForm NewUnPack = new PackFileForm(false))
            {
                NewUnPack.ShowDialog(this);
                PackFileReady = NewUnPack.PackVerified;
                RunQueue(PackFileReady);
            }
        }

        private async Task<bool> CheckPackFile()
        {
            ManagePackFile PackFileManager = new ManagePackFile();
            PackFileReady = await PackFileManager.VerifyPackFile();
            RunQueue(PackFileReady);
            if (!PackFileReady)
            {
                CancelAll();
                OtherFunctions.Message("The local pack file does not match the server. All running updates will be stopped and a new copy will now be downloaded and unpacked.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Pack file out of date", this);
                ProcessPackFile();
            }
            return true;
        }

        // ERROR: Handles clauses are not supported in C#
        private void GKPackageVeriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ChildFormControl.FormTypeIsOpen(typeof(PackFileForm)))
            {
                PackFileForm NewUnPack = new PackFileForm(true);
                NewUnPack.Show();
            }
        }

    }
}
