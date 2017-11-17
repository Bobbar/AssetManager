using System.Windows.Forms;
namespace AssetManager.UserInterface.CustomControls
{

    partial class SliderLabel : UserControl
	{

		//UserControl overrides dispose to clean up the component list.
		
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

		private System.ComponentModel.IContainer components  = null;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		
		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // SliderLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Name = "SliderLabel";
            this.Size = new System.Drawing.Size(141, 25);
            this.Load += new System.EventHandler(this.SliderLabelLoad);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SliderTextBoxPaint);
            this.ResumeLayout(false);

		}

	}
}
