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
    
    partial class GKUpdaterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GKUpdaterForm));
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.Updater_Table = new System.Windows.Forms.FlowLayoutPanel();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.MaxUpdates = new System.Windows.Forms.NumericUpDown();
            this.cmdCancelAll = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.cmdPauseResume = new System.Windows.Forms.Button();
            this.lblTotUpdates = new System.Windows.Forms.Label();
            this.lblComplete = new System.Windows.Forms.Label();
            this.lblRunning = new System.Windows.Forms.Label();
            this.lblQueued = new System.Windows.Forms.Label();
            this.QueueChecker = new System.Windows.Forms.Timer(this.components);
            this.lblTransferRate = new System.Windows.Forms.Label();
            this.cmdSort = new System.Windows.Forms.Button();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.OptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCreateDirs = new System.Windows.Forms.ToolStripMenuItem();
            this.FunctionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GKPackageVeriToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GroupBox1.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxUpdates)).BeginInit();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.GroupBox1.Controls.Add(this.Updater_Table);
            this.GroupBox1.Location = new System.Drawing.Point(12, 114);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(1070, 504);
            this.GroupBox1.TabIndex = 2;
            this.GroupBox1.TabStop = false;
            // 
            // Updater_Table
            // 
            this.Updater_Table.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Updater_Table.AutoScroll = true;
            this.Updater_Table.BackColor = System.Drawing.SystemColors.Control;
            this.Updater_Table.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Updater_Table.Location = new System.Drawing.Point(9, 21);
            this.Updater_Table.Margin = new System.Windows.Forms.Padding(10);
            this.Updater_Table.Name = "Updater_Table";
            this.Updater_Table.Size = new System.Drawing.Size(1053, 473);
            this.Updater_Table.TabIndex = 2;
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.MaxUpdates);
            this.GroupBox3.Controls.Add(this.cmdCancelAll);
            this.GroupBox3.Controls.Add(this.Label1);
            this.GroupBox3.Controls.Add(this.cmdPauseResume);
            this.GroupBox3.Controls.Add(this.lblTotUpdates);
            this.GroupBox3.Controls.Add(this.lblComplete);
            this.GroupBox3.Controls.Add(this.lblRunning);
            this.GroupBox3.Controls.Add(this.lblQueued);
            this.GroupBox3.Location = new System.Drawing.Point(12, 36);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(577, 79);
            this.GroupBox3.TabIndex = 0;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Queue Control";
            // 
            // MaxUpdates
            // 
            this.MaxUpdates.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaxUpdates.Location = new System.Drawing.Point(302, 40);
            this.MaxUpdates.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.MaxUpdates.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MaxUpdates.Name = "MaxUpdates";
            this.MaxUpdates.Size = new System.Drawing.Size(59, 22);
            this.MaxUpdates.TabIndex = 5;
            this.MaxUpdates.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MaxUpdates.ValueChanged += new System.EventHandler(this.MaxUpdates_ValueChanged);
            // 
            // cmdCancelAll
            // 
            this.cmdCancelAll.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancelAll.Location = new System.Drawing.Point(441, 45);
            this.cmdCancelAll.Name = "cmdCancelAll";
            this.cmdCancelAll.Size = new System.Drawing.Size(120, 27);
            this.cmdCancelAll.TabIndex = 7;
            this.cmdCancelAll.Text = "Cancel All";
            this.cmdCancelAll.UseVisualStyleBackColor = true;
            this.cmdCancelAll.Click += new System.EventHandler(this.cmdCancelAll_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(277, 18);
            this.Label1.Name = "Label1";
            this.Label1.Padding = new System.Windows.Forms.Padding(5);
            this.Label1.Size = new System.Drawing.Size(115, 24);
            this.Label1.TabIndex = 6;
            this.Label1.Text = "Max Concurrent";
            // 
            // cmdPauseResume
            // 
            this.cmdPauseResume.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdPauseResume.Location = new System.Drawing.Point(441, 12);
            this.cmdPauseResume.Name = "cmdPauseResume";
            this.cmdPauseResume.Size = new System.Drawing.Size(120, 27);
            this.cmdPauseResume.TabIndex = 4;
            this.cmdPauseResume.Text = "Pause Queue";
            this.cmdPauseResume.UseVisualStyleBackColor = true;
            this.cmdPauseResume.Click += new System.EventHandler(this.cmdPauseResume_Click);
            // 
            // lblTotUpdates
            // 
            this.lblTotUpdates.AutoSize = true;
            this.lblTotUpdates.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotUpdates.Location = new System.Drawing.Point(6, 18);
            this.lblTotUpdates.Name = "lblTotUpdates";
            this.lblTotUpdates.Padding = new System.Windows.Forms.Padding(5);
            this.lblTotUpdates.Size = new System.Drawing.Size(94, 24);
            this.lblTotUpdates.TabIndex = 3;
            this.lblTotUpdates.Text = "[# Updates]";
            // 
            // lblComplete
            // 
            this.lblComplete.AutoSize = true;
            this.lblComplete.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComplete.Location = new System.Drawing.Point(145, 42);
            this.lblComplete.Name = "lblComplete";
            this.lblComplete.Padding = new System.Windows.Forms.Padding(5);
            this.lblComplete.Size = new System.Drawing.Size(87, 24);
            this.lblComplete.TabIndex = 2;
            this.lblComplete.Text = "[Complete]";
            // 
            // lblRunning
            // 
            this.lblRunning.AutoSize = true;
            this.lblRunning.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRunning.Location = new System.Drawing.Point(145, 18);
            this.lblRunning.Name = "lblRunning";
            this.lblRunning.Padding = new System.Windows.Forms.Padding(5);
            this.lblRunning.Size = new System.Drawing.Size(80, 24);
            this.lblRunning.TabIndex = 1;
            this.lblRunning.Text = "[Running]";
            // 
            // lblQueued
            // 
            this.lblQueued.AutoSize = true;
            this.lblQueued.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQueued.Location = new System.Drawing.Point(6, 42);
            this.lblQueued.Name = "lblQueued";
            this.lblQueued.Padding = new System.Windows.Forms.Padding(5);
            this.lblQueued.Size = new System.Drawing.Size(73, 24);
            this.lblQueued.TabIndex = 0;
            this.lblQueued.Text = "[Queued]";
            // 
            // QueueChecker
            // 
            this.QueueChecker.Enabled = true;
            this.QueueChecker.Interval = 250;
            this.QueueChecker.Tick += new System.EventHandler(this.QueueChecker_Tick);
            // 
            // lblTransferRate
            // 
            this.lblTransferRate.AutoSize = true;
            this.lblTransferRate.Location = new System.Drawing.Point(695, 68);
            this.lblTransferRate.Name = "lblTransferRate";
            this.lblTransferRate.Size = new System.Drawing.Size(112, 14);
            this.lblTransferRate.TabIndex = 3;
            this.lblTransferRate.Text = "[Transfer Rate]";
            // 
            // cmdSort
            // 
            this.cmdSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSort.Location = new System.Drawing.Point(996, 91);
            this.cmdSort.Name = "cmdSort";
            this.cmdSort.Size = new System.Drawing.Size(86, 24);
            this.cmdSort.TabIndex = 4;
            this.cmdSort.Text = "Sort";
            this.cmdSort.UseVisualStyleBackColor = true;
            this.cmdSort.Click += new System.EventHandler(this.cmdSort_Click);
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OptionsToolStripMenuItem,
            this.FunctionsToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(1094, 24);
            this.MenuStrip.TabIndex = 5;
            this.MenuStrip.Text = "MenuStrip1";
            // 
            // OptionsToolStripMenuItem
            // 
            this.OptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmCreateDirs});
            this.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem";
            this.OptionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.OptionsToolStripMenuItem.Text = "Options";
            // 
            // tsmCreateDirs
            // 
            this.tsmCreateDirs.CheckOnClick = true;
            this.tsmCreateDirs.Name = "tsmCreateDirs";
            this.tsmCreateDirs.Size = new System.Drawing.Size(211, 22);
            this.tsmCreateDirs.Text = "Create Missing Directories";
            this.tsmCreateDirs.Click += new System.EventHandler(this.tsmCreateDirs_Click);
            // 
            // FunctionsToolStripMenuItem
            // 
            this.FunctionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GKPackageVeriToolStripMenuItem});
            this.FunctionsToolStripMenuItem.Name = "FunctionsToolStripMenuItem";
            this.FunctionsToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.FunctionsToolStripMenuItem.Text = "Functions";
            // 
            // GKPackageVeriToolStripMenuItem
            // 
            this.GKPackageVeriToolStripMenuItem.Name = "GKPackageVeriToolStripMenuItem";
            this.GKPackageVeriToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.GKPackageVeriToolStripMenuItem.Text = "Manage GK Package";
            this.GKPackageVeriToolStripMenuItem.Click += new System.EventHandler(this.GKPackageVeriToolStripMenuItem_Click);
            // 
            // GKUpdaterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1094, 638);
            this.Controls.Add(this.cmdSort);
            this.Controls.Add(this.lblTransferRate);
            this.Controls.Add(this.GroupBox3);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.MenuStrip);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip;
            this.MinimumSize = new System.Drawing.Size(1010, 428);
            this.Name = "GKUpdaterForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gatekeeper Updater";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GKUpdaterForm_FormClosing);
            this.Shown += new System.EventHandler(this.GKUpdaterForm_Shown);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxUpdates)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal GroupBox GroupBox1;
        internal Timer QueueChecker;
        internal GroupBox GroupBox3;
        internal Label lblQueued;
        internal Label lblRunning;
        internal Label lblComplete;
        internal Label lblTotUpdates;
        internal Button cmdPauseResume;
        internal Label Label1;
        internal NumericUpDown MaxUpdates;
        internal Button cmdCancelAll;
        internal FlowLayoutPanel Updater_Table;
        internal Label lblTransferRate;
        internal Button cmdSort;
        internal MenuStrip MenuStrip;
        internal ToolStripMenuItem OptionsToolStripMenuItem;
        internal ToolStripMenuItem tsmCreateDirs;
        internal ToolStripMenuItem FunctionsToolStripMenuItem;
        internal ToolStripMenuItem GKPackageVeriToolStripMenuItem;
    }
}
