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
namespace AssetManager
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class SibiNotesForm : ExtendedForm
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
			this.rtbNotes = new System.Windows.Forms.RichTextBox();
			this.cmdClose = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			//
			//rtbNotes
			//
			this.rtbNotes.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.rtbNotes.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.rtbNotes.Location = new System.Drawing.Point(12, 12);
			this.rtbNotes.Name = "rtbNotes";
			this.rtbNotes.Size = new System.Drawing.Size(448, 205);
			this.rtbNotes.TabIndex = 0;
			this.rtbNotes.Text = "";
			//
			//cmdClose
			//
			this.cmdClose.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.cmdClose.Location = new System.Drawing.Point(365, 234);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(95, 30);
			this.cmdClose.TabIndex = 1;
			this.cmdClose.Text = "Close";
			this.cmdClose.UseVisualStyleBackColor = true;
			//
			//cmdOK
			//
			this.cmdOK.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.cmdOK.Location = new System.Drawing.Point(12, 234);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(95, 30);
			this.cmdOK.TabIndex = 2;
			this.cmdOK.Text = "OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			//
			//SibiNotesForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(472, 276);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.cmdClose);
			this.Controls.Add(this.rtbNotes);
			this.MinimumSize = new System.Drawing.Size(253, 160);
			this.Name = "SibiNotesForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Sibi Note";
			this.ResumeLayout(false);

		}
		private RichTextBox withEventsField_rtbNotes;
		internal RichTextBox rtbNotes {
			get { return withEventsField_rtbNotes; }
			set {
				if (withEventsField_rtbNotes != null) {
					withEventsField_rtbNotes.LinkClicked -= rtbNotes_LinkClicked;
				}
				withEventsField_rtbNotes = value;
				if (withEventsField_rtbNotes != null) {
					withEventsField_rtbNotes.LinkClicked += rtbNotes_LinkClicked;
				}
			}
		}
		private Button withEventsField_cmdClose;
		internal Button cmdClose {
			get { return withEventsField_cmdClose; }
			set {
				if (withEventsField_cmdClose != null) {
					withEventsField_cmdClose.Click -= cmdClose_Click;
				}
				withEventsField_cmdClose = value;
				if (withEventsField_cmdClose != null) {
					withEventsField_cmdClose.Click += cmdClose_Click;
				}
			}
		}
		private Button withEventsField_cmdOK;
		internal Button cmdOK {
			get { return withEventsField_cmdOK; }
			set {
				if (withEventsField_cmdOK != null) {
					withEventsField_cmdOK.Click -= cmdOK_Click;
				}
				withEventsField_cmdOK = value;
				if (withEventsField_cmdOK != null) {
					withEventsField_cmdOK.Click += cmdOK_Click;
				}
			}
		}
	}
}
