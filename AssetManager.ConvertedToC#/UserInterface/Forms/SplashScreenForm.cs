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
using System.Deployment.Application;
namespace AssetManager
{

    public partial class SplashScreenForm
    {

        private void SplashScreen1_Load(object sender, System.EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            using (System.Drawing.Drawing2D.GraphicsPath p = new System.Drawing.Drawing2D.GraphicsPath())
            {
                p.StartFigure();
                p.AddArc(new Rectangle(0, 0, 40, 40), 180, 90);
                p.AddLine(40, 0, this.Width - 40, 0);
                p.AddArc(new Rectangle(this.Width - 40, 0, 40, 40), -90, 90);
                p.AddLine(this.Width, 40, this.Width, this.Height - 40);
                p.AddArc(new Rectangle(this.Width - 40, this.Height - 40, 40, 40), 0, 90);
                p.AddLine(this.Width - 40, this.Height, 40, this.Height);
                p.AddArc(new Rectangle(0, this.Height - 40, 40, 40), 90, 90);
                p.CloseFigure();
                this.Region = new Region(p);
            }
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Version.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            else
            {
                Version.Text = "Debug";
            }
            //TODO: Do this another way.
            //Copyright.Text = Application.Info.Copyright;
        }

        public void Hide()
        {
            this.Dispose();
        }

        public void SetStatus(string text)
        {
            lblStatus.Text = text;
            this.Refresh();
        }
        public SplashScreenForm()
        {
            Load += SplashScreen1_Load;
            InitializeComponent();
        }

    }
}
