using System.Windows.Forms;

namespace AssetManager.UserInterface.Forms.AdminTools
{
    
    partial class AdvancedSearchForm
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
            this.TableTree = new CorrectedTreeView();
            this.SearchButton = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.SearchStringTextBox = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox1.Controls.Add(this.TableTree);
            this.GroupBox1.Controls.Add(this.SearchButton);
            this.GroupBox1.Controls.Add(this.Label2);
            this.GroupBox1.Controls.Add(this.SearchStringTextBox);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Location = new System.Drawing.Point(12, 12);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(669, 415);
            this.GroupBox1.TabIndex = 1;
            this.GroupBox1.TabStop = false;
            // 
            // TableTree
            // 
            this.TableTree.CheckBoxes = true;
            this.TableTree.Location = new System.Drawing.Point(27, 45);
            this.TableTree.Name = "TableTree";
            this.TableTree.Size = new System.Drawing.Size(242, 349);
            this.TableTree.TabIndex = 5;
            this.TableTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TableTree_AfterCheck);
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(393, 189);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(146, 57);
            this.SearchButton.TabIndex = 4;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(311, 93);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(98, 15);
            this.Label2.TabIndex = 3;
            this.Label2.Text = "Search String";
            // 
            // SearchStringTextBox
            // 
            this.SearchStringTextBox.Location = new System.Drawing.Point(314, 111);
            this.SearchStringTextBox.Name = "SearchStringTextBox";
            this.SearchStringTextBox.Size = new System.Drawing.Size(325, 23);
            this.SearchStringTextBox.TabIndex = 2;
            this.SearchStringTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchStringTextBox_KeyDown);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(23, 27);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(203, 15);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Tables And Columns To Search";
            // 
            // AdvancedSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 441);
            this.Controls.Add(this.GroupBox1);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AdvancedSearchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Advanced Search";
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }
        internal GroupBox GroupBox1;
        internal Button SearchButton;
        internal Label Label2;
        internal TextBox SearchStringTextBox;
        internal Label Label1;
        internal CorrectedTreeView TableTree;
    }
}
