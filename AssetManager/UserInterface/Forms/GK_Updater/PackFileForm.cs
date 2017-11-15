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
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using AssetManager.UserInterface.CustomControls;

namespace AssetManager.UserInterface.Forms.GK_Updater
{


    public partial class PackFileForm : ExtendedForm
    {
        public bool PackVerified { get; set; }
        private bool Working = false;

        private ManagePackFile PackFunc = new ManagePackFile();
        public PackFileForm(bool showFunctions)
        {
            InitializeComponent();
            FunctionPanel.Visible = showFunctions;
            if (!showFunctions)
            {
                CheckPackFile();
            }
        }

        private async void CheckPackFile()
        {
            try
            {
                Working = true;
                PackVerified = await PackFunc.ProcessPackFile();
                Working = false;
                if (this.Modal)
                    this.Close();
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (PackFunc.Progress.Percent > 0)
                {
                    ProgressBar.Value = PackFunc.Progress.Percent;
                    PackFunc.Progress.Tick();
                    if (PackFunc.Progress.Throughput > 0 & PackFunc.Progress.Percent < 100)
                    {
                        if (!SpeedLabel.Visible)
                            SpeedLabel.Visible = true;
                        SpeedLabel.Text = PackFunc.Progress.Throughput.ToString() + " MB/s";
                    }
                    else
                    {
                        SpeedLabel.Visible = false;
                    }
                }
                StatusLabel.Text = PackFunc.Status;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void VerifyPackButton_Click(object sender, EventArgs e)
        {
            try
            {
                var GKFormInstance = Helpers.ChildFormControl.GKUpdaterInstance();
                if (!GKFormInstance.ActiveUpdates())
                {
                    CheckPackFile();
                }
                else
                {
                    OtherFunctions.Message("This process will interfere with the active running updates. Please stop all updates and try again.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Cannot Continue", this);
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private async void NewPackFile()
        {
            try
            {
                Working = true;
                if (!await PackFunc.CreateNewPackFile())
                {
                    OtherFunctions.Message("Error while creating a new pack file.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Error", this);
                }
                Working = false;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void NewPackButton_Click(object sender, EventArgs e)
        {
            try
            {
                NewPackFile();
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void PackFileForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = Working;
        }
    }
}