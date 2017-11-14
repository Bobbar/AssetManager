using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Management.Automation.Runspaces;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Microsoft.VisualBasic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Diagnostics;


namespace AssetManager
{
    public class TeamViewerDeploy
    {
        public TeamViewerDeploy()
        {
            // VBConversions Note: Non-static class variable initialization is below.  Class variables cannot be initially assigned non-static values in C#.
            WatchDogTask = new Task(() => WatchDog());

        }

        #region Fields

        private const string DeploymentFilesDirectory = "\\\\core.co.fairfield.oh.us\\dfs1\\fcdd\\files\\Information Technology\\Software\\Tools\\TeamViewer\\Deploy";
        private const string DeployTempDirectory = "\\Temp\\TVDeploy";
        private bool CancelOperation = false;
        private bool Finished = false;
        private long LastActivity;
        private ExtendedForm LogView;
        private ExtendedForm ParentForm;
        private PowerShellWrapper PSWrapper = new PowerShellWrapper();
        private RichTextBox RTBLog;
        private int TimeoutSeconds = 120;
        private Task WatchDogTask; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        #endregion

        #region Delegates

        delegate void RTBLogDelegate(string message);

        #endregion

        #region Methods

        public async Task<bool> DeployToDevice(ExtendedForm parentForm, DeviceObject targetDevice)
        {
            try
            {
                if (targetDevice != null && targetDevice.HostName != "")
                {
                    ActivityTick();
                    WatchDogTask.Start();
                    InitLogWindow(parentForm);

                    DepLog("Starting new TeamViewer deployment to " + targetDevice.HostName);
                    DepLog("-------------------");
                    using (CopyFilesForm PushForm = new CopyFilesForm(parentForm, targetDevice, DeploymentFilesDirectory, DeployTempDirectory))
                    {
                        bool TVExists = false;

                        DepLog("Pushing files to target computer...");
                        ActivityTick();
                        if (await PushForm.StartCopy())
                        {
                            DepLog("Push successful!");
                            PushForm.Dispose();
                        }
                        else
                        {
                            Finished = true;
                            DepLog("Push failed!");
                            OtherFunctions.Message("Error occurred while pushing deployment files to device!");
                            return false;
                        }

                        ActivityTick();
                        DepLog("Checking for previous installation...");
                        TVExists = System.Convert.ToBoolean(await TeamViewerInstalled(targetDevice));
                        if (TVExists)
                        {
                            DepLog("TeamViewer already installed.");
                        }
                        else
                        {
                            DepLog("TeamViewer not installed.");
                        }

                        ActivityTick();
                        if (TVExists)
                        {
                            DepLog("Reinstalling TeamViewer...");

                            if (await PSWrapper.InvokePowerShellCommand(targetDevice.HostName, GetTVReinstallCommand()))
                            {
                                DepLog("Deployment complete!");
                            }
                            else
                            {
                                Finished = true;
                                DepLog("Deployment failed!");
                                OtherFunctions.Message("Error occurred while executing deployment command!");
                                return false;
                            }

                        }
                        else
                        {
                            DepLog("Starting TeamViewer deployment...");
                            if (await PSWrapper.InvokePowerShellCommand(targetDevice.HostName, GetTVInstallCommand()))
                            {
                                DepLog("Deployment complete!");
                            }
                            else
                            {
                                Finished = true;
                                DepLog("Deployment failed!");
                                OtherFunctions.Message("Error occurred while executing deployment command!");
                                return false;
                            }
                        }

                        DepLog("Waiting 10 seconds.");
                        for (var i = 10; i >= 1; i--)
                        {
                            ActivityTick();
                            await Task.Delay(1000);
                            DepLog(i + "...");
                        }

                        ActivityTick();
                        DepLog("Starting TeamViewer assignment...");
                        if (await PSWrapper.InvokePowerShellCommand(targetDevice.HostName, GetTVAssignCommand()))
                        {
                            DepLog("Assignment complete!");
                        }
                        else
                        {
                            Finished = true;
                            DepLog("Assignment failed!");
                            OtherFunctions.Message("Error occurred while executing assignment command!");
                            return false;
                        }

                        ActivityTick();
                        DepLog("Deleting temp files...");
                        string ClientPath = "\\\\" + targetDevice.HostName + "\\c$";
                        using (NetworkConnection NetCon = new NetworkConnection(ClientPath, SecurityTools.AdminCreds))
                        {
                            Directory.Delete(ClientPath + DeployTempDirectory, true);
                        }

                        Finished = true;
                        DepLog("Done.");

                    }

                    DepLog("-------------------");
                    DepLog("TeamView deployment is complete!");
                    DepLog("NOTE: The target computer may need rebooted or the user may need to open the application before TeamViewer will connect.");
                    return true;
                }
                Finished = true;
                OtherFunctions.Message("The target device is null or does not have a hostname.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Missing Info", parentForm);
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Finished = true;
            }
        }

        private void DepLog(string message)
        {
            if (RTBLog.InvokeRequired)
            {
                RTBLogDelegate d = new RTBLogDelegate(DepLog);
                RTBLog.Invoke(d, message);
            }
            else
            {
                RTBLog.AppendText(DateTime.Now.ToString() + ": " + message + "\r\n");
            }
        }

        private Command GetTVAssignCommand()
        {
            string ApiToken = System.Convert.ToString(GlobalInstances.AssetFunc.GetTVApiToken());
            var cmd = new Command("Start-Process", false, true);
            cmd.Parameters.Add("FilePath", "C:\\Temp\\TVDeploy\\Assignment\\TeamViewer_Assignment.exe");
            cmd.Parameters.Add("ArgumentList", "-apitoken " + ApiToken + " -datafile ${ProgramFiles}\\TeamViewer\\AssignmentData.json");
            cmd.Parameters.Add("Wait");
            cmd.Parameters.Add("NoNewWindow");
            return cmd;
        }

        private Command GetTVReinstallCommand()
        {
            var cmd = new Command("Start-Process", false, true);
            cmd.Parameters.Add("FilePath", "msiexec.exe");
            cmd.Parameters.Add("ArgumentList", "/i C:\\Temp\\TVDeploy\\TeamViewer_Host-idcjnfzfgb.msi REINSTALL=ALL REINSTALLMODE=omus /qn");
            cmd.Parameters.Add("Wait");
            cmd.Parameters.Add("NoNewWindow");
            return cmd;
        }

        private Command GetTVInstallCommand()
        {
            var cmd = new Command("Start-Process", false, true);
            cmd.Parameters.Add("FilePath", "msiexec.exe");
            cmd.Parameters.Add("ArgumentList", "/i C:\\Temp\\TVDeploy\\TeamViewer_Host-idcjnfzfgb.msi /qn");
            cmd.Parameters.Add("Wait");
            cmd.Parameters.Add("NoNewWindow");
            return cmd;
        }

        private async Task<bool> TeamViewerInstalled(DeviceObject targetDevice)
        {
            try
            {
                var resultString = await Task.Run(() =>
                {
                    return PSWrapper.ExecuteRemotePSScript(targetDevice.HostName, Properties.Resources.CheckForTVRegistryValue, SecurityTools.AdminCreds);
                });
                var result = Convert.ToBoolean(resultString);
                return result;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void InitLogWindow(ExtendedForm parentForm)
        {
            this.ParentForm = parentForm;
            LogView = new ExtendedForm(parentForm);
            LogView.FormClosing += new FormClosingEventHandler(LogClosed);
            LogView.Text = "Deployment Log (Close to cancel)";
            LogView.Width = 500;
            LogView.Owner = parentForm;
            RTBLog = new RichTextBox();
            RTBLog.Dock = DockStyle.Fill;
            RTBLog.Font = StyleFunctions.GridFont;
            LogView.Controls.Add(RTBLog);
            LogView.Show();
        }

        private void LogClosed(object sender, CancelEventArgs e)
        {
            if (!Finished)
            {
                if (!CancelOperation)
                {
                    if (OtherFunctions.Message("Cancel the current operation?", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Question, "Cancel?", ParentForm) == MsgBoxResult.Yes)
                    {
                        CancelOperation = true;
                        PSWrapper.StopPowerShellCommand();
                        PSWrapper.StopPiplineCommand();
                    }
                    e.Cancel = true;
                }
                else
                {
                    if (Finished)
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        if (SecondsSinceLastActivity() > TimeoutSeconds)
                        {
                            PSWrapper.StopPowerShellCommand();
                            PSWrapper.StopPiplineCommand();
                        }

                        e.Cancel = true;
                    }
                }
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void ActivityTick()
        {
            LastActivity = DateTime.Now.Ticks;
            if (CancelOperation)
            {
                DepLog("The deployment has been canceled!");
                throw (new DeploymentCanceledException());
            }
        }

        private int SecondsSinceLastActivity()
        {
            return System.Convert.ToInt32(System.Convert.ToInt32(System.Convert.ToInt32(DateTime.Now.Ticks - LastActivity) / 10000) / 1000);
        }

        private void WatchDog()
        {
            bool TimeoutMessageSent = false;
            bool CancelMessageSent = false;
            while (!Finished)
            {
                if (SecondsSinceLastActivity() > TimeoutSeconds)
                {
                    if (!TimeoutMessageSent)
                    {
                        DepLog("The operation is taking a long time...");
                        TimeoutMessageSent = true;
                    }
                }
                else
                {
                    TimeoutMessageSent = false;
                }
                if (CancelOperation && !CancelMessageSent)
                {
                    DepLog("Cancelling the operation...");
                    CancelMessageSent = true;
                }
                Task.Delay(1000).Wait();
            }
        }

        #endregion
        private class DeploymentCanceledException : Exception
        {

            public DeploymentCanceledException()
            {
            }

            public DeploymentCanceledException(string message) : base(message)
            {
            }

            public DeploymentCanceledException(string message, Exception inner) : base(message, inner)
            {
            }

        }


    }
}