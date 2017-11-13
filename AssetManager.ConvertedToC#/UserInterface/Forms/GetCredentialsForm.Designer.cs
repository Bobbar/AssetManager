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
	partial class GetCredentialsForm : ExtendedForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetCredentialsForm));
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.CredDescriptionLabel = new System.Windows.Forms.Label();
			this.cmdAccept = new System.Windows.Forms.Button();
			this.Label2 = new System.Windows.Forms.Label();
			this.Label1 = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.GroupBox1.SuspendLayout();
			this.SuspendLayout();
			//
			//GroupBox1
			//
			this.GroupBox1.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.GroupBox1.Controls.Add(this.CredDescriptionLabel);
			this.GroupBox1.Controls.Add(this.cmdAccept);
			this.GroupBox1.Controls.Add(this.Label2);
			this.GroupBox1.Controls.Add(this.Label1);
			this.GroupBox1.Controls.Add(this.txtPassword);
			this.GroupBox1.Controls.Add(this.txtUsername);
			this.GroupBox1.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.GroupBox1.Location = new System.Drawing.Point(7, 9);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(279, 224);
			this.GroupBox1.TabIndex = 0;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "Enter Credentials";
			//
			//CredDescriptionLabel
			//
			this.CredDescriptionLabel.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.CredDescriptionLabel.ForeColor = System.Drawing.Color.Gray;
			this.CredDescriptionLabel.Location = new System.Drawing.Point(2, 19);
			this.CredDescriptionLabel.Name = "CredDescriptionLabel";
			this.CredDescriptionLabel.Size = new System.Drawing.Size(274, 19);
			this.CredDescriptionLabel.TabIndex = 5;
			this.CredDescriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			//cmdAccept
			//
			this.cmdAccept.Location = new System.Drawing.Point(36, 161);
			this.cmdAccept.Name = "cmdAccept";
			this.cmdAccept.Size = new System.Drawing.Size(199, 36);
			this.cmdAccept.TabIndex = 4;
			this.cmdAccept.Text = "Accept";
			this.cmdAccept.UseVisualStyleBackColor = true;
			//
			//Label2
			//
			this.Label2.AutoSize = true;
			this.Label2.Location = new System.Drawing.Point(33, 99);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(63, 15);
			this.Label2.TabIndex = 3;
			this.Label2.Text = "Password";
			this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			//Label1
			//
			this.Label1.AutoSize = true;
			this.Label1.Location = new System.Drawing.Point(33, 42);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(63, 15);
			this.Label1.TabIndex = 2;
			this.Label1.Text = "Username";
			this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			//txtPassword
			//
			this.txtPassword.Location = new System.Drawing.Point(35, 117);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(200, 23);
			this.txtPassword.TabIndex = 1;
			this.txtPassword.UseSystemPasswordChar = true;
			//
			//txtUsername
			//
			this.txtUsername.Location = new System.Drawing.Point(35, 62);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(200, 23);
			this.txtUsername.TabIndex = 0;
			//
			//GetCredentialsForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(295, 245);
			this.Controls.Add(this.GroupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GetCredentialsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "LA Credentials";
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		internal GroupBox GroupBox1;
		private Button withEventsField_cmdAccept;
		internal Button cmdAccept {
			get { return withEventsField_cmdAccept; }
			set {
				if (withEventsField_cmdAccept != null) {
					withEventsField_cmdAccept.Click -= cmdAccept_Click;
				}
				withEventsField_cmdAccept = value;
				if (withEventsField_cmdAccept != null) {
					withEventsField_cmdAccept.Click += cmdAccept_Click;
				}
			}
		}
		internal Label Label2;
		internal Label Label1;
		private TextBox withEventsField_txtPassword;
		internal TextBox txtPassword {
			get { return withEventsField_txtPassword; }
			set {
				if (withEventsField_txtPassword != null) {
					withEventsField_txtPassword.KeyDown -= txtPassword_KeyDown;
					withEventsField_txtPassword.KeyPress -= txtPassword_KeyPress;
					withEventsField_txtPassword.KeyUp -= txtPassword_KeyUp;
				}
				withEventsField_txtPassword = value;
				if (withEventsField_txtPassword != null) {
					withEventsField_txtPassword.KeyDown += txtPassword_KeyDown;
					withEventsField_txtPassword.KeyPress += txtPassword_KeyPress;
					withEventsField_txtPassword.KeyUp += txtPassword_KeyUp;
				}
			}
		}
		internal TextBox txtUsername;
		internal Label CredDescriptionLabel;
	}
}
