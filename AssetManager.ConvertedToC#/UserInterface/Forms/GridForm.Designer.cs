using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
//using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
namespace AssetManager.UserInterface.Forms
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class GridForm
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
            this.GridPanel = new System.Windows.Forms.TableLayoutPanel();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.PopUpMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CopySelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SendToNewGridForm = new System.Windows.Forms.ToolStripMenuItem();
            this.Panel1.SuspendLayout();
            this.PopUpMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // GridPanel
            // 
            this.GridPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GridPanel.AutoSize = true;
            this.GridPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GridPanel.ColumnCount = 1;
            this.GridPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GridPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GridPanel.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GridPanel.Location = new System.Drawing.Point(3, 3);
            this.GridPanel.Name = "GridPanel";
            this.GridPanel.RowCount = 1;
            this.GridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GridPanel.Size = new System.Drawing.Size(1053, 0);
            this.GridPanel.TabIndex = 0;
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.Location = new System.Drawing.Point(0, 533);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(1067, 22);
            this.StatusStrip1.TabIndex = 1;
            this.StatusStrip1.Text = "StatusStrip1";
            // 
            // Panel1
            // 
            this.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel1.AutoScroll = true;
            this.Panel1.AutoScrollMargin = new System.Drawing.Size(0, 20);
            this.Panel1.Controls.Add(this.GridPanel);
            this.Panel1.Location = new System.Drawing.Point(4, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(1059, 530);
            this.Panel1.TabIndex = 2;
            // 
            // PopUpMenu
            // 
            this.PopUpMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopySelectedToolStripMenuItem,
            this.SendToNewGridForm});
            this.PopUpMenu.Name = "PopUpMenu";
            this.PopUpMenu.Size = new System.Drawing.Size(167, 70);
            // 
            // CopySelectedToolStripMenuItem
            // 
            this.CopySelectedToolStripMenuItem.Image = global::AssetManager.Properties.Resources.CopyIcon;
            this.CopySelectedToolStripMenuItem.Name = "CopySelectedToolStripMenuItem";
            this.CopySelectedToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.CopySelectedToolStripMenuItem.Text = "Copy Selected";
            // 
            // SendToNewGridForm
            // 
            this.SendToNewGridForm.Image = global::AssetManager.Properties.Resources.TransferArrowsIcon;
            this.SendToNewGridForm.Name = "SendToNewGridForm";
            this.SendToNewGridForm.Size = new System.Drawing.Size(166, 22);
            this.SendToNewGridForm.Text = "Send to New Grid";
            this.SendToNewGridForm.Click += new System.EventHandler(this.SendToNewGridForm_Click);
            // 
            // GridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 555);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.StatusStrip1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(439, 282);
            this.Name = "GridForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GridForm";
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            this.PopUpMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal System.Windows.Forms.TableLayoutPanel GridPanel;
        internal System.Windows.Forms.StatusStrip StatusStrip1;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.ContextMenuStrip PopUpMenu;
        private System.Windows.Forms.ToolStripMenuItem CopySelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SendToNewGridForm;


    }
}
