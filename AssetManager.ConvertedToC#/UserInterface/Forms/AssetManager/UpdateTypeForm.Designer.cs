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
namespace AssetManager.UserInterface.Forms.AssetManager
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class UpdateDev
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
			this.components = new System.ComponentModel.Container();
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.CancelButton = new System.Windows.Forms.Button();
			this.NotesTextBox = new System.Windows.Forms.RichTextBox();
			this.SubmitButton = new System.Windows.Forms.Button();
			this.Label2 = new System.Windows.Forms.Label();
			this.Label1 = new System.Windows.Forms.Label();
			this.UpdateTypeCombo = new System.Windows.Forms.ComboBox();
			this.ErrorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			this.GroupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.ErrorProvider1).BeginInit();
			this.SuspendLayout();
			//
			//GroupBox1
			//
			this.GroupBox1.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.GroupBox1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.GroupBox1.Controls.Add(this.CancelButton);
			this.GroupBox1.Controls.Add(this.NotesTextBox);
			this.GroupBox1.Controls.Add(this.SubmitButton);
			this.GroupBox1.Controls.Add(this.Label2);
			this.GroupBox1.Controls.Add(this.Label1);
			this.GroupBox1.Controls.Add(this.UpdateTypeCombo);
			this.GroupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.GroupBox1.Location = new System.Drawing.Point(12, 12);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(456, 297);
			this.GroupBox1.TabIndex = 0;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "More Info.";
			//
			//CancelButton
			//
			this.CancelButton.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.CancelButton.Location = new System.Drawing.Point(9, 254);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(66, 32);
			this.CancelButton.TabIndex = 3;
			this.CancelButton.Text = "Cancel";
			this.CancelButton.UseVisualStyleBackColor = true;
			//
			//NotesTextBox
			//
			this.NotesTextBox.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.NotesTextBox.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.NotesTextBox.Location = new System.Drawing.Point(9, 85);
			this.NotesTextBox.Name = "NotesTextBox";
			this.NotesTextBox.Size = new System.Drawing.Size(438, 163);
			this.NotesTextBox.TabIndex = 1;
			this.NotesTextBox.Text = "";
			//
			//SubmitButton
			//
			this.SubmitButton.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.SubmitButton.Enabled = false;
			this.SubmitButton.Location = new System.Drawing.Point(291, 254);
			this.SubmitButton.Name = "SubmitButton";
			this.SubmitButton.Size = new System.Drawing.Size(156, 32);
			this.SubmitButton.TabIndex = 2;
			this.SubmitButton.Text = "Submit";
			this.SubmitButton.UseVisualStyleBackColor = true;
			//
			//Label2
			//
			this.Label2.AutoSize = true;
			this.Label2.Location = new System.Drawing.Point(6, 66);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(47, 16);
			this.Label2.TabIndex = 3;
			this.Label2.Text = "Notes:";
			//
			//Label1
			//
			this.Label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.Label1.AutoSize = true;
			this.Label1.Location = new System.Drawing.Point(69, 37);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(93, 16);
			this.Label1.TabIndex = 1;
			this.Label1.Text = "Change Type:";
			//
			//UpdateTypeCombo
			//
			this.UpdateTypeCombo.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.UpdateTypeCombo.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.UpdateTypeCombo.FormattingEnabled = true;
			this.UpdateTypeCombo.Location = new System.Drawing.Point(165, 34);
			this.UpdateTypeCombo.Name = "UpdateTypeCombo";
			this.UpdateTypeCombo.Size = new System.Drawing.Size(178, 23);
			this.UpdateTypeCombo.TabIndex = 0;
			//
			//ErrorProvider1
			//
			this.ErrorProvider1.ContainerControl = this;
			//
			//UpdateDev
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.ClientSize = new System.Drawing.Size(480, 322);
			this.Controls.Add(this.GroupBox1);
			this.DoubleBuffered = true;
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(426, 287);
			this.Name = "UpdateDev";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Update Device";
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.ErrorProvider1).EndInit();
			this.ResumeLayout(false);

		}
		internal GroupBox GroupBox1;
		internal Label Label1;
		private ComboBox withEventsField_UpdateTypeCombo;
		internal ComboBox UpdateTypeCombo {
			get { return withEventsField_UpdateTypeCombo; }
			set {
				if (withEventsField_UpdateTypeCombo != null) {
					withEventsField_UpdateTypeCombo.Validating -= UpdateTypeCombo_ChangeType_Validating;
					withEventsField_UpdateTypeCombo.SelectionChangeCommitted -= UpdateTypeCombo_SelectionChangeCommitted;
				}
				withEventsField_UpdateTypeCombo = value;
				if (withEventsField_UpdateTypeCombo != null) {
					withEventsField_UpdateTypeCombo.Validating += UpdateTypeCombo_ChangeType_Validating;
					withEventsField_UpdateTypeCombo.SelectionChangeCommitted += UpdateTypeCombo_SelectionChangeCommitted;
				}
			}
		}
		private Button withEventsField_SubmitButton;
		internal Button SubmitButton {
			get { return withEventsField_SubmitButton; }
			set {
				if (withEventsField_SubmitButton != null) {
					withEventsField_SubmitButton.Click -= SubmitButton_Click;
				}
				withEventsField_SubmitButton = value;
				if (withEventsField_SubmitButton != null) {
					withEventsField_SubmitButton.Click += SubmitButton_Click;
				}
			}
		}
		internal Label Label2;
		private Button withEventsField_CancelButton;
		internal Button CancelButton {
			get { return withEventsField_CancelButton; }
			set {
				if (withEventsField_CancelButton != null) {
					withEventsField_CancelButton.Click -= CancelButton_Click;
				}
				withEventsField_CancelButton = value;
				if (withEventsField_CancelButton != null) {
					withEventsField_CancelButton.Click += CancelButton_Click;
				}
			}
		}
		internal RichTextBox NotesTextBox;
		internal ErrorProvider ErrorProvider1;
	}
}
