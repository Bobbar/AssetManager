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
namespace AssetManager.UserInterface.Forms
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class SplashScreenForm
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
			this.Version = new System.Windows.Forms.Label();
			this.Copyright = new System.Windows.Forms.Label();
			this.PictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblStatus = new System.Windows.Forms.Label();
			this.PictureBox2 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)this.PictureBox1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.PictureBox2).BeginInit();
			this.SuspendLayout();
			//
			//Version
			//
			this.Version.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.Version.BackColor = System.Drawing.Color.Transparent;
			this.Version.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Version.Location = new System.Drawing.Point(5, 196);
			this.Version.Name = "Version";
			this.Version.Size = new System.Drawing.Size(492, 20);
			this.Version.TabIndex = 4;
			this.Version.Text = "Version {0}.{1:00}";
			this.Version.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			//Copyright
			//
			this.Copyright.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.Copyright.BackColor = System.Drawing.Color.Transparent;
			this.Copyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Copyright.Location = new System.Drawing.Point(5, 216);
			this.Copyright.Name = "Copyright";
			this.Copyright.Size = new System.Drawing.Size(492, 22);
			this.Copyright.TabIndex = 5;
			this.Copyright.Text = "Copyright";
			this.Copyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			//PictureBox1
			//
			this.PictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.PictureBox1.Image = Properties.Resources.AssetManagerIcon;
			this.PictureBox1.Location = new System.Drawing.Point(53, 47);
			this.PictureBox1.Name = "PictureBox1";
			this.PictureBox1.Size = new System.Drawing.Size(132, 132);
			this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PictureBox1.TabIndex = 6;
			this.PictureBox1.TabStop = false;
			//
			//lblStatus
			//
			this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblStatus.Location = new System.Drawing.Point(2, 256);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(495, 24);
			this.lblStatus.TabIndex = 7;
			this.lblStatus.Text = "Label1";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			//PictureBox2
			//
			this.PictureBox2.Image = Properties.Resources.Title_Text_Orange;
			this.PictureBox2.Location = new System.Drawing.Point(215, 52);
			this.PictureBox2.Name = "PictureBox2";
			this.PictureBox2.Size = new System.Drawing.Size(215, 121);
			this.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.PictureBox2.TabIndex = 8;
			this.PictureBox2.TabStop = false;
			//
			//SplashScreenForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(236)), Convert.ToInt32(Convert.ToByte(114)), Convert.ToInt32(Convert.ToByte(37)));
			this.ClientSize = new System.Drawing.Size(496, 303);
			this.ControlBox = false;
			this.Controls.Add(this.PictureBox2);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.Version);
			this.Controls.Add(this.Copyright);
			this.Controls.Add(this.PictureBox1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SplashScreenForm";
			this.Opacity = 0.9;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			((System.ComponentModel.ISupportInitialize)this.PictureBox1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.PictureBox2).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		internal Label Version;
		internal Label Copyright;
		internal PictureBox PictureBox1;
		internal Label lblStatus;
		internal PictureBox PictureBox2;
	}
}
