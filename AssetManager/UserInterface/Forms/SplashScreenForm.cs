using AssetManager.UserInterface.CustomControls;
using System.Deployment.Application;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace AssetManager.UserInterface.Forms
{
    public partial class SplashScreenForm : ExtendedForm
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
            Assembly asm = Assembly.GetExecutingAssembly();
            string copyright = ((AssemblyCopyrightAttribute)asm.GetCustomAttribute(typeof(AssemblyCopyrightAttribute))).Copyright;
            Copyright.Text = copyright;
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