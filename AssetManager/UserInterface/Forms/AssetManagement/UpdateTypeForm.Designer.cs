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
    
    partial class UpdateDev
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
            this.components = new System.ComponentModel.Container();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.CancelUpdateButton = new System.Windows.Forms.Button();
            this.NotesTextBox = new System.Windows.Forms.RichTextBox();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.UpdateTypeCombo = new System.Windows.Forms.ComboBox();
            this.ErrorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.GroupBox1.Controls.Add(this.CancelUpdateButton);
            this.GroupBox1.Controls.Add(this.NotesTextBox);
            this.GroupBox1.Controls.Add(this.SubmitButton);
            this.GroupBox1.Controls.Add(this.Label2);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Controls.Add(this.UpdateTypeCombo);
            this.GroupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox1.Location = new System.Drawing.Point(12, 12);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(456, 297);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "More Info.";
            // 
            // CancelUpdateButton
            // 
            this.CancelUpdateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelUpdateButton.Location = new System.Drawing.Point(9, 254);
            this.CancelUpdateButton.Name = "CancelUpdateButton";
            this.CancelUpdateButton.Size = new System.Drawing.Size(66, 32);
            this.CancelUpdateButton.TabIndex = 3;
            this.CancelUpdateButton.Text = "Cancel";
            this.CancelUpdateButton.UseVisualStyleBackColor = true;
            this.CancelUpdateButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // NotesTextBox
            // 
            this.NotesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NotesTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NotesTextBox.Location = new System.Drawing.Point(9, 85);
            this.NotesTextBox.Name = "NotesTextBox";
            this.NotesTextBox.Size = new System.Drawing.Size(438, 163);
            this.NotesTextBox.TabIndex = 1;
            this.NotesTextBox.Text = "";
            // 
            // SubmitButton
            // 
            this.SubmitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SubmitButton.Enabled = false;
            this.SubmitButton.Location = new System.Drawing.Point(291, 254);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(156, 32);
            this.SubmitButton.TabIndex = 2;
            this.SubmitButton.Text = "Submit";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(6, 66);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(47, 16);
            this.Label2.TabIndex = 3;
            this.Label2.Text = "Notes:";
            // 
            // Label1
            // 
            this.Label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(69, 37);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(93, 16);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Change Type:";
            // 
            // UpdateTypeCombo
            // 
            this.UpdateTypeCombo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.UpdateTypeCombo.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateTypeCombo.FormattingEnabled = true;
            this.UpdateTypeCombo.Location = new System.Drawing.Point(165, 34);
            this.UpdateTypeCombo.Name = "UpdateTypeCombo";
            this.UpdateTypeCombo.Size = new System.Drawing.Size(178, 23);
            this.UpdateTypeCombo.TabIndex = 0;
            this.UpdateTypeCombo.SelectionChangeCommitted += new System.EventHandler(this.UpdateTypeCombo_SelectionChangeCommitted);
            this.UpdateTypeCombo.Validating += new System.ComponentModel.CancelEventHandler(this.UpdateTypeCombo_ChangeType_Validating);
            // 
            // ErrorProvider1
            // 
            this.ErrorProvider1.ContainerControl = this;
            // 
            // UpdateDev
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
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
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        internal GroupBox GroupBox1;
        internal Label Label1;
        internal ComboBox UpdateTypeCombo;
        internal Button SubmitButton;
        internal Label Label2;
        internal Button CancelUpdateButton;
        internal RichTextBox NotesTextBox;
        internal ErrorProvider ErrorProvider1;
    }
}
