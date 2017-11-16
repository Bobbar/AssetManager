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
    
    partial class ViewTrackingForm
    {
        //Form overrides dispose to clean up the component list.
        
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        //Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        
        private void InitializeComponent()
        {
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.txtCheckInUser = new System.Windows.Forms.TextBox();
            this.Label17 = new System.Windows.Forms.Label();
            this.txtEntryGUID = new System.Windows.Forms.TextBox();
            this.cmdClose = new System.Windows.Forms.Button();
            this.Label15 = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.Label14 = new System.Windows.Forms.Label();
            this.txtCheckUser = new System.Windows.Forms.TextBox();
            this.Label13 = new System.Windows.Forms.Label();
            this.txtTimeStamp = new System.Windows.Forms.TextBox();
            this.Label12 = new System.Windows.Forms.Label();
            this.txtGUID = new System.Windows.Forms.TextBox();
            this.Label10 = new System.Windows.Forms.Label();
            this.txtDueBack = new System.Windows.Forms.TextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.txtCheckInTime = new System.Windows.Forms.TextBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.txtCheckOutTime = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.txtAssetTag = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtCheckOutUser = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtCheckType = new System.Windows.Forms.TextBox();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.GroupBox1.Controls.Add(this.Label9);
            this.GroupBox1.Controls.Add(this.txtCheckInUser);
            this.GroupBox1.Controls.Add(this.Label17);
            this.GroupBox1.Controls.Add(this.txtEntryGUID);
            this.GroupBox1.Controls.Add(this.cmdClose);
            this.GroupBox1.Controls.Add(this.Label15);
            this.GroupBox1.Controls.Add(this.txtNotes);
            this.GroupBox1.Controls.Add(this.Label14);
            this.GroupBox1.Controls.Add(this.txtCheckUser);
            this.GroupBox1.Controls.Add(this.Label13);
            this.GroupBox1.Controls.Add(this.txtTimeStamp);
            this.GroupBox1.Controls.Add(this.Label12);
            this.GroupBox1.Controls.Add(this.txtGUID);
            this.GroupBox1.Controls.Add(this.Label10);
            this.GroupBox1.Controls.Add(this.txtDueBack);
            this.GroupBox1.Controls.Add(this.Label8);
            this.GroupBox1.Controls.Add(this.txtCheckInTime);
            this.GroupBox1.Controls.Add(this.Label7);
            this.GroupBox1.Controls.Add(this.txtCheckOutTime);
            this.GroupBox1.Controls.Add(this.Label6);
            this.GroupBox1.Controls.Add(this.txtLocation);
            this.GroupBox1.Controls.Add(this.Label5);
            this.GroupBox1.Controls.Add(this.txtDescription);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.txtSerial);
            this.GroupBox1.Controls.Add(this.Label3);
            this.GroupBox1.Controls.Add(this.txtAssetTag);
            this.GroupBox1.Controls.Add(this.Label2);
            this.GroupBox1.Controls.Add(this.txtCheckOutUser);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Controls.Add(this.txtCheckType);
            this.GroupBox1.Location = new System.Drawing.Point(13, 14);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(821, 391);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Tracking Entry";
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label9.Location = new System.Drawing.Point(492, 214);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(91, 16);
            this.Label9.TabIndex = 36;
            this.Label9.Text = "Check In User";
            // 
            // txtCheckInUser
            // 
            this.txtCheckInUser.BackColor = System.Drawing.Color.Silver;
            this.txtCheckInUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCheckInUser.Location = new System.Drawing.Point(495, 233);
            this.txtCheckInUser.Name = "txtCheckInUser";
            this.txtCheckInUser.ReadOnly = true;
            this.txtCheckInUser.Size = new System.Drawing.Size(142, 22);
            this.txtCheckInUser.TabIndex = 35;
            // 
            // Label17
            // 
            this.Label17.AutoSize = true;
            this.Label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label17.Location = new System.Drawing.Point(560, 75);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(64, 16);
            this.Label17.TabIndex = 34;
            this.Label17.Text = "Entry UID";
            // 
            // txtEntryGUID
            // 
            this.txtEntryGUID.BackColor = System.Drawing.Color.Silver;
            this.txtEntryGUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEntryGUID.Location = new System.Drawing.Point(563, 94);
            this.txtEntryGUID.Name = "txtEntryGUID";
            this.txtEntryGUID.ReadOnly = true;
            this.txtEntryGUID.Size = new System.Drawing.Size(228, 22);
            this.txtEntryGUID.TabIndex = 33;
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(339, 351);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(128, 28);
            this.cmdClose.TabIndex = 30;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // Label15
            // 
            this.Label15.AutoSize = true;
            this.Label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label15.Location = new System.Drawing.Point(174, 297);
            this.Label15.Name = "Label15";
            this.Label15.Size = new System.Drawing.Size(44, 16);
            this.Label15.TabIndex = 29;
            this.Label15.Text = "Notes";
            // 
            // txtNotes
            // 
            this.txtNotes.BackColor = System.Drawing.Color.Silver;
            this.txtNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNotes.Location = new System.Drawing.Point(224, 278);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ReadOnly = true;
            this.txtNotes.Size = new System.Drawing.Size(391, 56);
            this.txtNotes.TabIndex = 28;
            // 
            // Label14
            // 
            this.Label14.AutoSize = true;
            this.Label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label14.Location = new System.Drawing.Point(14, 119);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(78, 16);
            this.Label14.TabIndex = 27;
            this.Label14.Text = "Check User";
            this.Label14.Visible = false;
            // 
            // txtCheckUser
            // 
            this.txtCheckUser.BackColor = System.Drawing.Color.Silver;
            this.txtCheckUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCheckUser.Location = new System.Drawing.Point(17, 138);
            this.txtCheckUser.Name = "txtCheckUser";
            this.txtCheckUser.ReadOnly = true;
            this.txtCheckUser.Size = new System.Drawing.Size(108, 22);
            this.txtCheckUser.TabIndex = 26;
            this.txtCheckUser.Visible = false;
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label13.Location = new System.Drawing.Point(14, 28);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(81, 16);
            this.Label13.TabIndex = 25;
            this.Label13.Text = "Time Stamp";
            // 
            // txtTimeStamp
            // 
            this.txtTimeStamp.BackColor = System.Drawing.Color.Silver;
            this.txtTimeStamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimeStamp.Location = new System.Drawing.Point(17, 47);
            this.txtTimeStamp.Name = "txtTimeStamp";
            this.txtTimeStamp.ReadOnly = true;
            this.txtTimeStamp.Size = new System.Drawing.Size(155, 22);
            this.txtTimeStamp.TabIndex = 24;
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label12.Location = new System.Drawing.Point(560, 28);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(77, 16);
            this.Label12.TabIndex = 23;
            this.Label12.Text = "Device UID";
            // 
            // txtGUID
            // 
            this.txtGUID.BackColor = System.Drawing.Color.Silver;
            this.txtGUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGUID.Location = new System.Drawing.Point(563, 47);
            this.txtGUID.Name = "txtGUID";
            this.txtGUID.ReadOnly = true;
            this.txtGUID.Size = new System.Drawing.Size(228, 22);
            this.txtGUID.TabIndex = 22;
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label10.Location = new System.Drawing.Point(335, 170);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(67, 16);
            this.Label10.TabIndex = 19;
            this.Label10.Text = "Due Back";
            // 
            // txtDueBack
            // 
            this.txtDueBack.BackColor = System.Drawing.Color.Silver;
            this.txtDueBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDueBack.Location = new System.Drawing.Point(338, 189);
            this.txtDueBack.Name = "txtDueBack";
            this.txtDueBack.ReadOnly = true;
            this.txtDueBack.Size = new System.Drawing.Size(137, 22);
            this.txtDueBack.TabIndex = 18;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.Location = new System.Drawing.Point(492, 170);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(93, 16);
            this.Label8.TabIndex = 15;
            this.Label8.Text = "Check In Time";
            // 
            // txtCheckInTime
            // 
            this.txtCheckInTime.BackColor = System.Drawing.Color.Silver;
            this.txtCheckInTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCheckInTime.Location = new System.Drawing.Point(495, 189);
            this.txtCheckInTime.Name = "txtCheckInTime";
            this.txtCheckInTime.ReadOnly = true;
            this.txtCheckInTime.Size = new System.Drawing.Size(142, 22);
            this.txtCheckInTime.TabIndex = 14;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Location = new System.Drawing.Point(181, 170);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(103, 16);
            this.Label7.TabIndex = 13;
            this.Label7.Text = "Check Out Time";
            // 
            // txtCheckOutTime
            // 
            this.txtCheckOutTime.BackColor = System.Drawing.Color.Silver;
            this.txtCheckOutTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCheckOutTime.Location = new System.Drawing.Point(184, 189);
            this.txtCheckOutTime.Name = "txtCheckOutTime";
            this.txtCheckOutTime.ReadOnly = true;
            this.txtCheckOutTime.Size = new System.Drawing.Size(136, 22);
            this.txtCheckOutTime.TabIndex = 12;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(317, 75);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(123, 16);
            this.Label6.TabIndex = 11;
            this.Label6.Text = "Check Out Location";
            // 
            // txtLocation
            // 
            this.txtLocation.BackColor = System.Drawing.Color.Silver;
            this.txtLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocation.Location = new System.Drawing.Point(320, 94);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.ReadOnly = true;
            this.txtLocation.Size = new System.Drawing.Size(229, 22);
            this.txtLocation.TabIndex = 10;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Location = new System.Drawing.Point(317, 28);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(76, 16);
            this.Label5.TabIndex = 9;
            this.Label5.Text = "Description";
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.Silver;
            this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(320, 47);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(229, 22);
            this.txtDescription.TabIndex = 8;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(191, 75);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(43, 16);
            this.Label4.TabIndex = 7;
            this.Label4.Text = "Serial";
            // 
            // txtSerial
            // 
            this.txtSerial.BackColor = System.Drawing.Color.Silver;
            this.txtSerial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerial.Location = new System.Drawing.Point(194, 94);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.ReadOnly = true;
            this.txtSerial.Size = new System.Drawing.Size(108, 22);
            this.txtSerial.TabIndex = 6;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(191, 28);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(70, 16);
            this.Label3.TabIndex = 5;
            this.Label3.Text = "Asset Tag";
            // 
            // txtAssetTag
            // 
            this.txtAssetTag.BackColor = System.Drawing.Color.Silver;
            this.txtAssetTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAssetTag.Location = new System.Drawing.Point(194, 47);
            this.txtAssetTag.Name = "txtAssetTag";
            this.txtAssetTag.ReadOnly = true;
            this.txtAssetTag.Size = new System.Drawing.Size(108, 22);
            this.txtAssetTag.TabIndex = 4;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(181, 214);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(101, 16);
            this.Label2.TabIndex = 3;
            this.Label2.Text = "Check Out User";
            // 
            // txtCheckOutUser
            // 
            this.txtCheckOutUser.BackColor = System.Drawing.Color.Silver;
            this.txtCheckOutUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCheckOutUser.Location = new System.Drawing.Point(184, 233);
            this.txtCheckOutUser.Name = "txtCheckOutUser";
            this.txtCheckOutUser.ReadOnly = true;
            this.txtCheckOutUser.Size = new System.Drawing.Size(136, 22);
            this.txtCheckOutUser.TabIndex = 2;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(14, 72);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(81, 16);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Check Type";
            // 
            // txtCheckType
            // 
            this.txtCheckType.BackColor = System.Drawing.Color.Silver;
            this.txtCheckType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCheckType.Location = new System.Drawing.Point(17, 91);
            this.txtCheckType.Name = "txtCheckType";
            this.txtCheckType.ReadOnly = true;
            this.txtCheckType.Size = new System.Drawing.Size(110, 22);
            this.txtCheckType.TabIndex = 0;
            this.txtCheckType.TabStop = false;
            this.txtCheckType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ViewTrackingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ClientSize = new System.Drawing.Size(846, 415);
            this.Controls.Add(this.GroupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ViewTrackingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Tracking Entry";
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }
        internal GroupBox GroupBox1;
        internal Label Label2;
        internal TextBox txtCheckOutUser;
        internal Label Label1;
        internal TextBox txtCheckType;
        internal Label Label3;
        internal TextBox txtAssetTag;
        internal Label Label12;
        internal TextBox txtGUID;
        internal Label Label10;
        internal TextBox txtDueBack;
        internal Label Label8;
        internal TextBox txtCheckInTime;
        internal Label Label7;
        internal TextBox txtCheckOutTime;
        internal Label Label6;
        internal TextBox txtLocation;
        internal Label Label5;
        internal TextBox txtDescription;
        internal Label Label4;
        internal TextBox txtSerial;
        internal Label Label13;
        internal TextBox txtTimeStamp;
        internal Label Label14;
        internal TextBox txtCheckUser;
        internal Label Label15;
        internal TextBox txtNotes;
        internal Button cmdClose;
        internal Label Label17;
        internal TextBox txtEntryGUID;
        internal Label Label9;
        internal TextBox txtCheckInUser;
    }
}
