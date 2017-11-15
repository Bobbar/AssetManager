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
namespace AssetManager.UserInterface.Forms.GK_Updater
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    partial class PackFileForm
    {

        //Form overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
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
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackFileForm));
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.ProgressTimer = new System.Windows.Forms.Timer(this.components);
            this.StatusLabel = new System.Windows.Forms.Label();
            this.VerifyPackButton = new System.Windows.Forms.Button();
            this.NewPackButton = new System.Windows.Forms.Button();
            this.FunctionPanel = new System.Windows.Forms.Panel();
            this.TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.StatusPanel = new System.Windows.Forms.Panel();
            this.SpeedLabel = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.FunctionPanel.SuspendLayout();
            this.TableLayoutPanel.SuspendLayout();
            this.StatusPanel.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(19, 33);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(419, 23);
            this.ProgressBar.TabIndex = 0;
            // 
            // ProgressTimer
            // 
            this.ProgressTimer.Enabled = true;
            this.ProgressTimer.Tick += new System.EventHandler(this.ProgressTimer_Tick);
            // 
            // StatusLabel
            // 
            this.StatusLabel.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusLabel.Location = new System.Drawing.Point(19, 5);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(419, 25);
            this.StatusLabel.TabIndex = 1;
            this.StatusLabel.Text = "{STATUS}";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VerifyPackButton
            // 
            this.VerifyPackButton.Location = new System.Drawing.Point(153, 6);
            this.VerifyPackButton.Name = "VerifyPackButton";
            this.VerifyPackButton.Size = new System.Drawing.Size(150, 30);
            this.VerifyPackButton.TabIndex = 2;
            this.VerifyPackButton.Text = "Verify Pack File";
            this.VerifyPackButton.UseVisualStyleBackColor = true;
            this.VerifyPackButton.Click += new System.EventHandler(this.VerifyPackButton_Click);
            // 
            // NewPackButton
            // 
            this.NewPackButton.Location = new System.Drawing.Point(153, 42);
            this.NewPackButton.Name = "NewPackButton";
            this.NewPackButton.Size = new System.Drawing.Size(150, 26);
            this.NewPackButton.TabIndex = 3;
            this.NewPackButton.Text = "Upload New Pack";
            this.NewPackButton.UseVisualStyleBackColor = true;
            this.NewPackButton.Click += new System.EventHandler(this.NewPackButton_Click);
            // 
            // FunctionPanel
            // 
            this.FunctionPanel.Controls.Add(this.NewPackButton);
            this.FunctionPanel.Controls.Add(this.VerifyPackButton);
            this.FunctionPanel.Location = new System.Drawing.Point(3, 83);
            this.FunctionPanel.Name = "FunctionPanel";
            this.FunctionPanel.Size = new System.Drawing.Size(459, 74);
            this.FunctionPanel.TabIndex = 4;
            // 
            // TableLayoutPanel
            // 
            this.TableLayoutPanel.AutoSize = true;
            this.TableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TableLayoutPanel.ColumnCount = 1;
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel.Controls.Add(this.StatusPanel, 0, 0);
            this.TableLayoutPanel.Controls.Add(this.FunctionPanel, 0, 1);
            this.TableLayoutPanel.Location = new System.Drawing.Point(4, 14);
            this.TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel.Name = "TableLayoutPanel";
            this.TableLayoutPanel.RowCount = 2;
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel.Size = new System.Drawing.Size(465, 160);
            this.TableLayoutPanel.TabIndex = 5;
            // 
            // StatusPanel
            // 
            this.StatusPanel.Controls.Add(this.SpeedLabel);
            this.StatusPanel.Controls.Add(this.ProgressBar);
            this.StatusPanel.Controls.Add(this.StatusLabel);
            this.StatusPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatusPanel.Location = new System.Drawing.Point(3, 3);
            this.StatusPanel.Name = "StatusPanel";
            this.StatusPanel.Size = new System.Drawing.Size(459, 74);
            this.StatusPanel.TabIndex = 6;
            // 
            // SpeedLabel
            // 
            this.SpeedLabel.Location = new System.Drawing.Point(19, 59);
            this.SpeedLabel.Name = "SpeedLabel";
            this.SpeedLabel.Size = new System.Drawing.Size(419, 14);
            this.SpeedLabel.TabIndex = 2;
            this.SpeedLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // GroupBox1
            // 
            this.GroupBox1.AutoSize = true;
            this.GroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GroupBox1.Controls.Add(this.TableLayoutPanel);
            this.GroupBox1.Location = new System.Drawing.Point(6, 4);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(472, 193);
            this.GroupBox1.TabIndex = 6;
            this.GroupBox1.TabStop = false;
            // 
            // PackFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(491, 242);
            this.Controls.Add(this.GroupBox1);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PackFileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pack File Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PackFileForm_FormClosing);
            this.FunctionPanel.ResumeLayout(false);
            this.TableLayoutPanel.ResumeLayout(false);
            this.StatusPanel.ResumeLayout(false);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal ProgressBar ProgressBar;
        internal Timer ProgressTimer;
        internal Label StatusLabel;
        internal Button VerifyPackButton;
        internal Button NewPackButton;
        internal Panel FunctionPanel;
        internal TableLayoutPanel TableLayoutPanel;
        internal Panel StatusPanel;
        internal GroupBox GroupBox1;
        internal Label SpeedLabel;
    }
}
