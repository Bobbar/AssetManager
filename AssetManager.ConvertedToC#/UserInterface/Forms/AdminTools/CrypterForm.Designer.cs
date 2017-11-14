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
namespace AssetManager.UserInterface.Forms.AdminTools
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class CrypterForm
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
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.cmdClear = new System.Windows.Forms.Button();
			this.Label3 = new System.Windows.Forms.Label();
			this.txtResult = new System.Windows.Forms.TextBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.txtKey = new System.Windows.Forms.TextBox();
			this.Label1 = new System.Windows.Forms.Label();
			this.txtString = new System.Windows.Forms.TextBox();
			this.cmdEncode = new System.Windows.Forms.Button();
			this.GroupBox1.SuspendLayout();
			this.SuspendLayout();
			//
			//GroupBox1
			//
			this.GroupBox1.Controls.Add(this.cmdClear);
			this.GroupBox1.Controls.Add(this.Label3);
			this.GroupBox1.Controls.Add(this.txtResult);
			this.GroupBox1.Controls.Add(this.Label2);
			this.GroupBox1.Controls.Add(this.txtKey);
			this.GroupBox1.Controls.Add(this.Label1);
			this.GroupBox1.Controls.Add(this.txtString);
			this.GroupBox1.Controls.Add(this.cmdEncode);
			this.GroupBox1.Location = new System.Drawing.Point(10, 12);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(490, 328);
			this.GroupBox1.TabIndex = 0;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "Encode Text";
			//
			//cmdClear
			//
			this.cmdClear.Location = new System.Drawing.Point(169, 267);
			this.cmdClear.Name = "cmdClear";
			this.cmdClear.Size = new System.Drawing.Size(141, 30);
			this.cmdClear.TabIndex = 7;
			this.cmdClear.Text = "Clear";
			this.cmdClear.UseVisualStyleBackColor = true;
			//
			//Label3
			//
			this.Label3.AutoSize = true;
			this.Label3.Location = new System.Drawing.Point(31, 159);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(37, 13);
			this.Label3.TabIndex = 6;
			this.Label3.Text = "Result";
			//
			//txtResult
			//
			this.txtResult.Location = new System.Drawing.Point(34, 175);
			this.txtResult.Name = "txtResult";
			this.txtResult.Size = new System.Drawing.Size(421, 20);
			this.txtResult.TabIndex = 5;
			//
			//Label2
			//
			this.Label2.AutoSize = true;
			this.Label2.Location = new System.Drawing.Point(31, 77);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(25, 13);
			this.Label2.TabIndex = 4;
			this.Label2.Text = "Key";
			//
			//txtKey
			//
			this.txtKey.Location = new System.Drawing.Point(34, 93);
			this.txtKey.Name = "txtKey";
			this.txtKey.Size = new System.Drawing.Size(421, 20);
			this.txtKey.TabIndex = 3;
			//
			//Label1
			//
			this.Label1.AutoSize = true;
			this.Label1.Location = new System.Drawing.Point(31, 26);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(34, 13);
			this.Label1.TabIndex = 2;
			this.Label1.Text = "String";
			//
			//txtString
			//
			this.txtString.Location = new System.Drawing.Point(34, 42);
			this.txtString.Name = "txtString";
			this.txtString.Size = new System.Drawing.Size(421, 20);
			this.txtString.TabIndex = 1;
			//
			//cmdEncode
			//
			this.cmdEncode.Location = new System.Drawing.Point(169, 212);
			this.cmdEncode.Name = "cmdEncode";
			this.cmdEncode.Size = new System.Drawing.Size(141, 49);
			this.cmdEncode.TabIndex = 0;
			this.cmdEncode.Text = "Go!";
			this.cmdEncode.UseVisualStyleBackColor = true;
			//
			//frmEncrypter
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(515, 352);
			this.Controls.Add(this.GroupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "frmEncrypter";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Encoder";
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox1.PerformLayout();
			this.ResumeLayout(false);

		}
		internal GroupBox GroupBox1;
		internal Label Label3;
		internal TextBox txtResult;
		internal Label Label2;
		internal TextBox txtKey;
		internal Label Label1;
		internal TextBox txtString;
		private Button withEventsField_cmdEncode;
		internal Button cmdEncode {
			get { return withEventsField_cmdEncode; }
			set {
				if (withEventsField_cmdEncode != null) {
					withEventsField_cmdEncode.Click -= cmdEncode_Click;
				}
				withEventsField_cmdEncode = value;
				if (withEventsField_cmdEncode != null) {
					withEventsField_cmdEncode.Click += cmdEncode_Click;
				}
			}
		}
		private Button withEventsField_cmdClear;
		internal Button cmdClear {
			get { return withEventsField_cmdClear; }
			set {
				if (withEventsField_cmdClear != null) {
					withEventsField_cmdClear.Click -= cmdClear_Click;
				}
				withEventsField_cmdClear = value;
				if (withEventsField_cmdClear != null) {
					withEventsField_cmdClear.Click += cmdClear_Click;
				}
			}
		}
	}
}
