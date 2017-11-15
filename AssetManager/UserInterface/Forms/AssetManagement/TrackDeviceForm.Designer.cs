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
namespace AssetManager.UserInterface.Forms.AssetManagement
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class TrackDeviceForm
	{
		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try {
				if (disposing && components != null) {
					components.Dispose();
				}
			} finally {
				base.Dispose(disposing);
			}
		}
		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.CheckInBox = new System.Windows.Forms.GroupBox();
			this.cmdCheckIn = new System.Windows.Forms.Button();
			this.Label11 = new System.Windows.Forms.Label();
			this.txtCheckInNotes = new System.Windows.Forms.TextBox();
			this.Label10 = new System.Windows.Forms.Label();
			this.dtCheckIn = new System.Windows.Forms.DateTimePicker();
			this.CheckOutBox = new System.Windows.Forms.GroupBox();
			this.cmdCheckOut = new System.Windows.Forms.Button();
			this.Label9 = new System.Windows.Forms.Label();
			this.txtUseReason = new System.Windows.Forms.TextBox();
			this.Label8 = new System.Windows.Forms.Label();
			this.txtUser = new System.Windows.Forms.TextBox();
			this.Label7 = new System.Windows.Forms.Label();
			this.txtUseLocation = new System.Windows.Forms.TextBox();
			this.Label6 = new System.Windows.Forms.Label();
			this.dtDueBack = new System.Windows.Forms.DateTimePicker();
			this.Label5 = new System.Windows.Forms.Label();
			this.dtCheckOut = new System.Windows.Forms.DateTimePicker();
			this.DeviceInfoBox = new System.Windows.Forms.GroupBox();
			this.Label4 = new System.Windows.Forms.Label();
			this.txtDeviceType = new System.Windows.Forms.TextBox();
			this.Label3 = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.txtSerial = new System.Windows.Forms.TextBox();
			this.Label1 = new System.Windows.Forms.Label();
			this.txtAssetTag = new System.Windows.Forms.TextBox();
			this.CheckInBox.SuspendLayout();
			this.CheckOutBox.SuspendLayout();
			this.DeviceInfoBox.SuspendLayout();
			this.SuspendLayout();
			//
			//CheckInBox
			//
			this.CheckInBox.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.CheckInBox.Controls.Add(this.cmdCheckIn);
			this.CheckInBox.Controls.Add(this.Label11);
			this.CheckInBox.Controls.Add(this.txtCheckInNotes);
			this.CheckInBox.Controls.Add(this.Label10);
			this.CheckInBox.Controls.Add(this.dtCheckIn);
			this.CheckInBox.Location = new System.Drawing.Point(12, 307);
			this.CheckInBox.Name = "CheckInBox";
			this.CheckInBox.Size = new System.Drawing.Size(794, 152);
			this.CheckInBox.TabIndex = 0;
			this.CheckInBox.TabStop = false;
			this.CheckInBox.Text = "Check In";
			//
			//cmdCheckIn
			//
			this.cmdCheckIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdCheckIn.Location = new System.Drawing.Point(601, 58);
			this.cmdCheckIn.Name = "cmdCheckIn";
			this.cmdCheckIn.Size = new System.Drawing.Size(160, 51);
			this.cmdCheckIn.TabIndex = 13;
			this.cmdCheckIn.Text = "Check In";
			this.cmdCheckIn.UseVisualStyleBackColor = true;
			//
			//Label11
			//
			this.Label11.AutoSize = true;
			this.Label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label11.Location = new System.Drawing.Point(354, 27);
			this.Label11.Name = "Label11";
			this.Label11.Size = new System.Drawing.Size(98, 16);
			this.Label11.TabIndex = 12;
			this.Label11.Text = "Check In Notes";
			//
			//txtCheckInNotes
			//
			this.txtCheckInNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtCheckInNotes.Location = new System.Drawing.Point(270, 46);
			this.txtCheckInNotes.MaxLength = 200;
			this.txtCheckInNotes.Multiline = true;
			this.txtCheckInNotes.Name = "txtCheckInNotes";
			this.txtCheckInNotes.Size = new System.Drawing.Size(256, 68);
			this.txtCheckInNotes.TabIndex = 11;
			//
			//Label10
			//
			this.Label10.AutoSize = true;
			this.Label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label10.Location = new System.Drawing.Point(28, 36);
			this.Label10.Name = "Label10";
			this.Label10.Size = new System.Drawing.Size(91, 16);
			this.Label10.TabIndex = 4;
			this.Label10.Text = "Check In Date";
			//
			//dtCheckIn
			//
			this.dtCheckIn.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.dtCheckIn.CustomFormat = "MM-dd-yyyy hh:mm tt";
			this.dtCheckIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.dtCheckIn.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtCheckIn.Location = new System.Drawing.Point(31, 58);
			this.dtCheckIn.Name = "dtCheckIn";
			this.dtCheckIn.Size = new System.Drawing.Size(164, 22);
			this.dtCheckIn.TabIndex = 3;
			//
			//CheckOutBox
			//
			this.CheckOutBox.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.CheckOutBox.Controls.Add(this.cmdCheckOut);
			this.CheckOutBox.Controls.Add(this.Label9);
			this.CheckOutBox.Controls.Add(this.txtUseReason);
			this.CheckOutBox.Controls.Add(this.Label8);
			this.CheckOutBox.Controls.Add(this.txtUser);
			this.CheckOutBox.Controls.Add(this.Label7);
			this.CheckOutBox.Controls.Add(this.txtUseLocation);
			this.CheckOutBox.Controls.Add(this.Label6);
			this.CheckOutBox.Controls.Add(this.dtDueBack);
			this.CheckOutBox.Controls.Add(this.Label5);
			this.CheckOutBox.Controls.Add(this.dtCheckOut);
			this.CheckOutBox.Location = new System.Drawing.Point(12, 120);
			this.CheckOutBox.Name = "CheckOutBox";
			this.CheckOutBox.Size = new System.Drawing.Size(794, 181);
			this.CheckOutBox.TabIndex = 1;
			this.CheckOutBox.TabStop = false;
			this.CheckOutBox.Text = "Check Out";
			//
			//cmdCheckOut
			//
			this.cmdCheckOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdCheckOut.Location = new System.Drawing.Point(601, 109);
			this.cmdCheckOut.Name = "cmdCheckOut";
			this.cmdCheckOut.Size = new System.Drawing.Size(160, 51);
			this.cmdCheckOut.TabIndex = 11;
			this.cmdCheckOut.Text = "Check Out";
			this.cmdCheckOut.UseVisualStyleBackColor = true;
			//
			//Label9
			//
			this.Label9.AutoSize = true;
			this.Label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label9.Location = new System.Drawing.Point(353, 74);
			this.Label9.Name = "Label9";
			this.Label9.Size = new System.Drawing.Size(84, 16);
			this.Label9.TabIndex = 10;
			this.Label9.Text = "Use Reason";
			//
			//txtUseReason
			//
			this.txtUseReason.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtUseReason.Location = new System.Drawing.Point(270, 93);
			this.txtUseReason.MaxLength = 200;
			this.txtUseReason.Multiline = true;
			this.txtUseReason.Name = "txtUseReason";
			this.txtUseReason.Size = new System.Drawing.Size(256, 68);
			this.txtUseReason.TabIndex = 9;
			//
			//Label8
			//
			this.Label8.AutoSize = true;
			this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label8.Location = new System.Drawing.Point(17, 116);
			this.Label8.Name = "Label8";
			this.Label8.Size = new System.Drawing.Size(37, 16);
			this.Label8.TabIndex = 8;
			this.Label8.Text = "User";
			this.Label8.Visible = false;
			//
			//txtUser
			//
			this.txtUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtUser.Location = new System.Drawing.Point(15, 138);
			this.txtUser.Name = "txtUser";
			this.txtUser.ReadOnly = true;
			this.txtUser.Size = new System.Drawing.Size(175, 22);
			this.txtUser.TabIndex = 7;
			this.txtUser.Visible = false;
			//
			//Label7
			//
			this.Label7.AutoSize = true;
			this.Label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label7.Location = new System.Drawing.Point(494, 16);
			this.Label7.Name = "Label7";
			this.Label7.Size = new System.Drawing.Size(87, 16);
			this.Label7.TabIndex = 6;
			this.Label7.Text = "Use Location";
			//
			//txtUseLocation
			//
			this.txtUseLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtUseLocation.Location = new System.Drawing.Point(492, 38);
			this.txtUseLocation.Name = "txtUseLocation";
			this.txtUseLocation.Size = new System.Drawing.Size(184, 22);
			this.txtUseLocation.TabIndex = 5;
			//
			//Label6
			//
			this.Label6.AutoSize = true;
			this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label6.Location = new System.Drawing.Point(305, 16);
			this.Label6.Name = "Label6";
			this.Label6.Size = new System.Drawing.Size(99, 16);
			this.Label6.TabIndex = 4;
			this.Label6.Text = "Due Back Date";
			//
			//dtDueBack
			//
			this.dtDueBack.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.dtDueBack.CustomFormat = "MM-dd-yyyy hh:mm tt";
			this.dtDueBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.dtDueBack.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtDueBack.Location = new System.Drawing.Point(308, 38);
			this.dtDueBack.Name = "dtDueBack";
			this.dtDueBack.Size = new System.Drawing.Size(167, 22);
			this.dtDueBack.TabIndex = 3;
			//
			//Label5
			//
			this.Label5.AutoSize = true;
			this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label5.Location = new System.Drawing.Point(128, 16);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(101, 16);
			this.Label5.TabIndex = 2;
			this.Label5.Text = "Check Out Date";
			//
			//dtCheckOut
			//
			this.dtCheckOut.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.dtCheckOut.CustomFormat = "MM-dd-yyyy hh:mm tt";
			this.dtCheckOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.dtCheckOut.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtCheckOut.Location = new System.Drawing.Point(131, 38);
			this.dtCheckOut.Name = "dtCheckOut";
			this.dtCheckOut.Size = new System.Drawing.Size(164, 22);
			this.dtCheckOut.TabIndex = 0;
			//
			//DeviceInfoBox
			//
			this.DeviceInfoBox.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.DeviceInfoBox.Controls.Add(this.Label4);
			this.DeviceInfoBox.Controls.Add(this.txtDeviceType);
			this.DeviceInfoBox.Controls.Add(this.Label3);
			this.DeviceInfoBox.Controls.Add(this.txtDescription);
			this.DeviceInfoBox.Controls.Add(this.Label2);
			this.DeviceInfoBox.Controls.Add(this.txtSerial);
			this.DeviceInfoBox.Controls.Add(this.Label1);
			this.DeviceInfoBox.Controls.Add(this.txtAssetTag);
			this.DeviceInfoBox.Location = new System.Drawing.Point(12, 12);
			this.DeviceInfoBox.Name = "DeviceInfoBox";
			this.DeviceInfoBox.Size = new System.Drawing.Size(794, 93);
			this.DeviceInfoBox.TabIndex = 2;
			this.DeviceInfoBox.TabStop = false;
			this.DeviceInfoBox.Text = "Device Info";
			//
			//Label4
			//
			this.Label4.AutoSize = true;
			this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label4.Location = new System.Drawing.Point(604, 27);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(86, 16);
			this.Label4.TabIndex = 7;
			this.Label4.Text = "Device Type";
			//
			//txtDeviceType
			//
			this.txtDeviceType.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtDeviceType.Location = new System.Drawing.Point(607, 49);
			this.txtDeviceType.Name = "txtDeviceType";
			this.txtDeviceType.ReadOnly = true;
			this.txtDeviceType.Size = new System.Drawing.Size(139, 23);
			this.txtDeviceType.TabIndex = 6;
			//
			//Label3
			//
			this.Label3.AutoSize = true;
			this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label3.Location = new System.Drawing.Point(334, 27);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(76, 16);
			this.Label3.TabIndex = 5;
			this.Label3.Text = "Description";
			//
			//txtDescription
			//
			this.txtDescription.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtDescription.Location = new System.Drawing.Point(332, 49);
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ReadOnly = true;
			this.txtDescription.Size = new System.Drawing.Size(244, 23);
			this.txtDescription.TabIndex = 4;
			//
			//Label2
			//
			this.Label2.AutoSize = true;
			this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label2.Location = new System.Drawing.Point(167, 26);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(43, 16);
			this.Label2.TabIndex = 3;
			this.Label2.Text = "Serial";
			//
			//txtSerial
			//
			this.txtSerial.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtSerial.Location = new System.Drawing.Point(165, 48);
			this.txtSerial.Name = "txtSerial";
			this.txtSerial.ReadOnly = true;
			this.txtSerial.Size = new System.Drawing.Size(139, 23);
			this.txtSerial.TabIndex = 2;
			//
			//Label1
			//
			this.Label1.AutoSize = true;
			this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label1.Location = new System.Drawing.Point(28, 26);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(70, 16);
			this.Label1.TabIndex = 1;
			this.Label1.Text = "Asset Tag";
			//
			//txtAssetTag
			//
			this.txtAssetTag.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtAssetTag.Location = new System.Drawing.Point(26, 48);
			this.txtAssetTag.Name = "txtAssetTag";
			this.txtAssetTag.ReadOnly = true;
			this.txtAssetTag.Size = new System.Drawing.Size(114, 23);
			this.txtAssetTag.TabIndex = 0;
			//
			//TrackDeviceForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.ClientSize = new System.Drawing.Size(816, 470);
			this.Controls.Add(this.DeviceInfoBox);
			this.Controls.Add(this.CheckOutBox);
			this.Controls.Add(this.CheckInBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "TrackDeviceForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Tracking";
			this.CheckInBox.ResumeLayout(false);
			this.CheckInBox.PerformLayout();
			this.CheckOutBox.ResumeLayout(false);
			this.CheckOutBox.PerformLayout();
			this.DeviceInfoBox.ResumeLayout(false);
			this.DeviceInfoBox.PerformLayout();
			this.ResumeLayout(false);

		}
		internal GroupBox CheckInBox;
		internal GroupBox CheckOutBox;
		internal GroupBox DeviceInfoBox;
		internal Label Label4;
		internal TextBox txtDeviceType;
		internal Label Label3;
		internal TextBox txtDescription;
		internal Label Label2;
		internal TextBox txtSerial;
		internal Label Label1;
		internal TextBox txtAssetTag;
		internal Label Label5;
		internal DateTimePicker dtCheckOut;
		internal Label Label7;
		private TextBox withEventsField_txtUseLocation;
		internal TextBox txtUseLocation {
			get { return withEventsField_txtUseLocation; }
			set {
				if (withEventsField_txtUseLocation != null) {
					withEventsField_txtUseLocation.LostFocus -= txtUseLocation_LostFocus;
				}
				withEventsField_txtUseLocation = value;
				if (withEventsField_txtUseLocation != null) {
					withEventsField_txtUseLocation.LostFocus += txtUseLocation_LostFocus;
				}
			}
		}
		internal Label Label6;
		internal DateTimePicker dtDueBack;
		internal Label Label8;
		internal TextBox txtUser;
		private Button withEventsField_cmdCheckOut;
		internal Button cmdCheckOut {
			get { return withEventsField_cmdCheckOut; }
			set {
				if (withEventsField_cmdCheckOut != null) {
					withEventsField_cmdCheckOut.Click -= cmdCheckOut_Click;
				}
				withEventsField_cmdCheckOut = value;
				if (withEventsField_cmdCheckOut != null) {
					withEventsField_cmdCheckOut.Click += cmdCheckOut_Click;
				}
			}
		}
		internal Label Label9;
		private TextBox withEventsField_txtUseReason;
		internal TextBox txtUseReason {
			get { return withEventsField_txtUseReason; }
			set {
				if (withEventsField_txtUseReason != null) {
					withEventsField_txtUseReason.LostFocus -= txtUseReason_LostFocus;
				}
				withEventsField_txtUseReason = value;
				if (withEventsField_txtUseReason != null) {
					withEventsField_txtUseReason.LostFocus += txtUseReason_LostFocus;
				}
			}
		}
		internal Label Label10;
		internal DateTimePicker dtCheckIn;
		internal Label Label11;
		private TextBox withEventsField_txtCheckInNotes;
		internal TextBox txtCheckInNotes {
			get { return withEventsField_txtCheckInNotes; }
			set {
				if (withEventsField_txtCheckInNotes != null) {
					withEventsField_txtCheckInNotes.LostFocus -= txtCheckInNotes_LostFocus;
				}
				withEventsField_txtCheckInNotes = value;
				if (withEventsField_txtCheckInNotes != null) {
					withEventsField_txtCheckInNotes.LostFocus += txtCheckInNotes_LostFocus;
				}
			}
		}
		private Button withEventsField_cmdCheckIn;
		internal Button cmdCheckIn {
			get { return withEventsField_cmdCheckIn; }
			set {
				if (withEventsField_cmdCheckIn != null) {
					withEventsField_cmdCheckIn.Click -= cmdCheckIn_Click;
				}
				withEventsField_cmdCheckIn = value;
				if (withEventsField_cmdCheckIn != null) {
					withEventsField_cmdCheckIn.Click += cmdCheckIn_Click;
				}
			}
		}
	}
}
