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
using GKUpdaterLib;
namespace AssetManager
{

	public partial class GKProgressControl : IDisposable
	{

		public GK_Updater MyUpdater;
		public ProgressStatus ProgStatus;
		private bool bolShow = false;
		private GKUpdaterLib.GK_Updater.Status_Stats CurrentStatus;
		private DeviceObject CurDevice = new DeviceObject();
		private string LogBuff = "";
		private Form MyParentForm;

		private Color PrevColor;
		public DeviceObject Device {
			get { return CurDevice; }
		}

		public GKProgressControl()
		{
			Disposed += GK_Progress_Fragment_Disposed;
			// This call is required by the designer.
			InitializeComponent();

			// Add any initialization after the InitializeComponent() call.

		}


		public GKProgressControl(Form parentForm, DeviceObject device, bool createMissingDirs, string gkPath, int seq = 0)
		{
			Disposed += GK_Progress_Fragment_Disposed;
			InitializeComponent();
			AssetManager.ImageCaching.CachedImages.CacheControlImages(this);
			this.Size = this.MinimumSize;
			MyParentForm = parentForm;
			CurDevice = device;
			MyUpdater = new GK_Updater(CurDevice.HostName, gkPath);
			MyUpdater.CreateMissingDirectories = createMissingDirs;
			this.DoubleBuffered = true;
			lblInfo.Text = CurDevice.Serial + " - " + CurDevice.CurrentUser;
			lblTransRate.Text = "0.00MB/s";
			SetStatus(ProgressStatus.Queued);
			if (seq > 0) {
				lblSeq.Text = "#" + seq;
			} else {
				lblSeq.Text = "";
			}
			MyUpdater.LogEvent += GKLogEvent;
			MyUpdater.StatusUpdate += GKStatusUpdateEvent;
			MyUpdater.UpdateComplete += GKUpdate_Complete;
			MyUpdater.UpdateCanceled += GKUpdate_Cancelled;
			ExtendedMethods.DoubleBufferedPanel(Panel1, true);
		}

		public GKProgressControl(Form parentForm, DeviceObject device, bool createMissingDirs, string sourcePath, string destPath, int seq = 0)
		{
			Disposed += GK_Progress_Fragment_Disposed;
			InitializeComponent();
			AssetManager.ImageCaching.CachedImages.CacheControlImages(this);
			this.Size = this.MinimumSize;
			MyParentForm = parentForm;
			CurDevice = device;
			MyUpdater = new GK_Updater(CurDevice.HostName, sourcePath, destPath);
			MyUpdater.CreateMissingDirectories = createMissingDirs;
			this.DoubleBuffered = true;
			lblInfo.Text = CurDevice.Serial + " - " + CurDevice.CurrentUser;
			lblTransRate.Text = "0.00MB/s";
			SetStatus(ProgressStatus.Queued);
			if (seq > 0) {
				lblSeq.Text = "#" + seq;
			} else {
				lblSeq.Text = "";
			}
			MyUpdater.LogEvent += GKLogEvent;
			MyUpdater.StatusUpdate += GKStatusUpdateEvent;
			MyUpdater.UpdateComplete += GKUpdate_Complete;
			MyUpdater.UpdateCanceled += GKUpdate_Cancelled;
			ExtendedMethods.DoubleBufferedPanel(Panel1, true);
		}



		public event EventHandler CriticalStopError;

		public enum ProgressStatus
		{
			Starting,
			Running,
			Paused,
			Queued,
			Complete,
			CompleteWithErrors,
			Canceled,
			Errors
		}

		public void CancelUpdate()
		{
			if (!MyUpdater.IsDisposed)
				MyUpdater.CancelUpdate();
		}

		public void StartUpdate()
		{
			try {
				if (ProgStatus != ProgressStatus.Running) {
					LogBuff = "";
					SetStatus(ProgressStatus.Starting);
					MyUpdater.StartUpdate(AssetManager.SecurityTools.SecurityFunctions.AdminCreds);
				}
			} catch (Exception ex) {
				SetStatus(ProgressStatus.Errors);
				ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
			}
		}

		protected virtual void OnCriticalStopError(EventArgs e)
		{
			if (CriticalStopError != null) {
				CriticalStopError(this, e);
			}
		}

		private void DrawLight(Color Color)
		{
			if (Color != PrevColor) {
				PrevColor = Color;
				Bitmap bm = new Bitmap(pbStatus.Width, pbStatus.Height);
				using (SolidBrush MyBrush = new SolidBrush(Color)) {
					using (Pen StrokePen = new Pen(Color.Black, 1.5)) {
						using (Graphics gr = Graphics.FromImage(bm)) {
							gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
							float XLoc = 0;
							float YLoc = 0;
							float Size = 0;
							Size = 20;
							XLoc = Convert.ToSingle(pbStatus.Width / 2 - Size / 2);
							YLoc = Convert.ToSingle(pbStatus.Height / 2 - Size / 2);
							gr.FillEllipse(MyBrush, XLoc, YLoc, Size, Size);
							gr.DrawEllipse(StrokePen, XLoc, YLoc, Size, Size);
							pbStatus.Image = bm;
						}
					}
				}
			}
		}

		private void GK_Progress_Fragment_Disposed(object sender, EventArgs e)
		{
			MyUpdater.LogEvent -= GKLogEvent;
			MyUpdater.StatusUpdate -= GKStatusUpdateEvent;
			MyUpdater.UpdateComplete -= GKUpdate_Complete;
			MyUpdater.UpdateCanceled -= GKUpdate_Cancelled;
			MyUpdater.Dispose();

		}

		/// <summary>
		/// Log message event from GKUpdater.  This even can fire very rapidly. So the result is stored in a buffer to be added to the rtbLog control in a more controlled manner.
		/// </summary>
		private void GKLogEvent(object sender, EventArgs e)
		{
			var LogEvent = (GK_Updater.LogEvents)e;
			Log(LogEvent.LogData.Message);
		}

		private void Log(string Message)
		{
			LogBuff += Message + Constants.vbCrLf;
		}

		private void GKStatusUpdateEvent(object sender, EventArgs e)
		{
			var UpdateEvent = (GK_Updater.GKUpdateEvents)e;
			SetStatus(ProgressStatus.Running);
			CurrentStatus = UpdateEvent.CurrentStatus;
			pbarProgress.Maximum = CurrentStatus.TotFiles;
			pbarProgress.Value = CurrentStatus.CurFileIdx;
			lblStatus.Text = CurrentStatus.CurFileName;
		}

		private void GKUpdate_Cancelled(object sender, EventArgs e)
		{
			SetStatus(ProgressStatus.Canceled);
		}

		private void GKUpdate_Complete(object sender, EventArgs e)
		{
			var CompleteEvent = (GK_Updater.GKUpdateCompleteEvents)e;
			if (CompleteEvent.HasErrors) {
				SetStatus(ProgressStatus.Errors);
				ErrorHandling.ErrHandle(CompleteEvent.Errors, System.Reflection.MethodInfo.GetCurrentMethod());

				if (CompleteEvent.Errors is Win32Exception) {
					var err = (Win32Exception)CompleteEvent.Errors;
					//Check for invalid credentials error and fire critical stop event.
					//We want to stop all updates if the credentials are wrong as to avoid locking the account.

					//TODO: Try to let these errors bubble up to ErrHandler.
					if (err.NativeErrorCode == 1326 | err.NativeErrorCode == 86) {
						OnCriticalStopError(new EventArgs());
					}
				} else {
					switch (true) {
						case CompleteEvent.Errors is GK_Updater.MissingDirectoryException:
							Log("Enable 'Create Missing Directories' option and re-enqueue this device to force creation.");
							break;
					}
				}
			} else {
				if (MyUpdater.ErrorList.Count == 0) {
					SetStatus(ProgressStatus.Complete);
				} else {
					SetStatus(ProgressStatus.CompleteWithErrors);
				}
			}
		}

		private void HideLog()
		{
			this.Size = this.MinimumSize;
			bolShow = false;
			lblShowHide.Text = "s";
			//"+"
		}

		private void ShowLog()
		{
			UpdateLogBox();
			this.Size = this.MaximumSize;
			bolShow = true;
			lblShowHide.Text = "r";
			//"-"
		}

		private void lblInfo_Click(object sender, EventArgs e)
		{
			ChildFormControl.LookupDevice(MainForm, CurDevice);
		}

		private void lblShowHide_Click(object sender, EventArgs e)
		{
			if (!bolShow) {
				ShowLog();
			} else {
				HideLog();
			}
		}

		private void pbCancelClose_Click(object sender, EventArgs e)
		{
			if (ProgStatus == ProgressStatus.Running | ProgStatus == ProgressStatus.Paused) {
				if (!MyUpdater.IsDisposed) {
					MyUpdater.CancelUpdate();
					SetStatus(ProgressStatus.Canceled);
				} else {
					this.Dispose();
				}
			} else {
				this.Dispose();
			}
		}

		private void pbRestart_Click(object sender, EventArgs e)
		{
			switch (ProgStatus) {
				case ProgressStatus.Paused:
					MyUpdater.ResumeUpdate();
					SetStatus(ProgressStatus.Running);
					break;
				case ProgressStatus.Running:
					MyUpdater.PauseUpdate();
					SetStatus(ProgressStatus.Paused);
					break;
				case ProgressStatus.Queued:
					var blah = Message("This update is queued. Starting it may exceed the maximum concurrent updates. Are you sure you want to start it?", Constants.vbYesNo + Constants.vbQuestion, "Warning", MyParentForm);
					if (blah == MsgBoxResult.Yes) {
						StartUpdate();
					}
					break;
				default:
					StartUpdate();
					break;
			}
		}

		private void SetStatus(ProgressStatus Status)
		{
			if (ProgStatus != Status) {
				ProgStatus = Status;
				SetStatusLight(Status);
				SetButtons(Status);
				SetStatusLabel(Status);
			}
		}

		private void SetStatusLight(ProgressStatus Status)
		{
			switch (Status) {
				case ProgressStatus.Running:
				case ProgressStatus.Starting:
					DrawLight(Color.LimeGreen);
					break;
				case ProgressStatus.Queued:
				case ProgressStatus.Paused:
					DrawLight(Color.Yellow);
					break;
				default:
					DrawLight(Color.Red);
					break;
			}
		}

		private void SetButtons(ProgressStatus Status)
		{
			switch (Status) {
				case ProgressStatus.Running:
					pbRestart.Image = AssetManager.ImageCaching.CachedImages.ImageCache("PauseIcon", AssetManager.My.Resources.Resources.PauseIcon);
					MyToolTip.SetToolTip(pbRestart, "Pause");
					break;
				case ProgressStatus.Paused:
				case ProgressStatus.Queued:
					pbRestart.Image = AssetManager.ImageCaching.CachedImages.ImageCache("PlayIcon", AssetManager.My.Resources.Resources.PlayIcon);
					MyToolTip.SetToolTip(pbRestart, "Resume");
					break;
				default:
					pbRestart.Image = AssetManager.ImageCaching.CachedImages.ImageCache("RestartIcon", AssetManager.My.Resources.Resources.RestartIcon);
					MyToolTip.SetToolTip(pbRestart, "Restart");
					break;
			}
		}

		private void SetStatusLabel(ProgressStatus Status)
		{
			switch (Status) {
				case ProgressStatus.Queued:
					lblStatus.Text = "Queued...";
					break;
				case ProgressStatus.Canceled:
					lblStatus.Text = "Canceled!";
					break;
				case ProgressStatus.Errors:
					lblStatus.Text = "ERROR!";
					break;
				case ProgressStatus.CompleteWithErrors:
					lblStatus.Text = "Completed with errors: " + MyUpdater.ErrorList.Count;
					break;
				case ProgressStatus.Complete:
					lblStatus.Text = "Complete!";
					break;
				case ProgressStatus.Starting:
					lblStatus.Text = "Starting...";
					break;
				case ProgressStatus.Paused:
					lblStatus.Text = "Paused.";
					break;
			}
		}

		/// <summary>
		/// Timer that updates the rtbLog control with chunks of data from the log buffer.
		/// </summary>
		private void UI_Timer_Tick(object sender, EventArgs e)
		{
			if (bolShow & !string.IsNullOrEmpty(LogBuff)) {
				UpdateLogBox();
			}
			if (ProgStatus == ProgressStatus.Running) {
				pbarFileProgress.Value = MyUpdater.UpdateStatus.CurFileProgress;
				if (pbarFileProgress.Value > 1)
					pbarFileProgress.Value = pbarFileProgress.Value - 1;
				//doing this bypasses the progressbar control animation. This way it doesn't lag behind and fills completely
				pbarFileProgress.Value = MyUpdater.UpdateStatus.CurFileProgress;
				lblTransRate.Text = MyUpdater.UpdateStatus.CurTransferRate.ToString("0.00") + "MB/s";
				this.Update();
			}
		}

		private void UpdateLogBox()
		{
			rtbLog.AppendText(LogBuff);
			rtbLog.Refresh();
			LogBuff = "";
		}

	}
}
