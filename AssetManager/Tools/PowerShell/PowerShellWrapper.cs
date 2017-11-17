using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Net;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetManager
{
    public class PowerShellWrapper
    {
        private PowerShell CurrentPowerShellObject;

        private Pipeline CurrentPipelineObject;
        /// <summary>
        /// Execute the specified PowerShell script on the specified host.
        /// </summary>
        /// <param name="hostname">Hostname of the remote computer.</param>
        /// <param name="scriptBytes">PowerShell script as a byte array.</param>
        /// <param name="credentials">Credentials used when creating the remote runspace.</param>
        /// <returns>Returns any error messages.</returns>
        public string ExecuteRemotePSScript(string hostname, byte[] scriptBytes, NetworkCredential credentials)
        {
            try
            {
                var psCreds = new PSCredential(credentials.UserName, credentials.SecurePassword);
                string scriptText = LoadScript(scriptBytes);
                string shellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
                WSManConnectionInfo connInfo = new WSManConnectionInfo(false, hostname, 5985, "/wsman", shellUri, psCreds);

                using (Runspace remoteRunSpace = RunspaceFactory.CreateRunspace(connInfo))
                {
                    remoteRunSpace.Open();
                    using (Pipeline MyPipline = remoteRunSpace.CreatePipeline())
                    {
                        MyPipline.Commands.AddScript(scriptText);
                        MyPipline.Commands.Add("Out-String");

                        CurrentPipelineObject = MyPipline;

                        Collection<PSObject> results = MyPipline.Invoke();
                        StringBuilder stringBuilder = new StringBuilder();

                        foreach (var obj in results)
                        {
                            stringBuilder.AppendLine(obj.ToString());
                        }

                        return DataConsistency.CleanDBValue((stringBuilder.ToString())).ToString();

                    }

                }
            }
            catch (Exception ex)
            {
                //Check for incorrect username/password error and rethrow a Win32Exception to be caught in the error handler.
                //Makes sure that the global admin credentials variable is cleared so that a new prompt will be shown on the next attempt. See: VerifyAdminCreds method.
                if (ex is PSRemotingTransportException)
                {
                    var transportEx = (PSRemotingTransportException)ex;
                    if (transportEx.ErrorCode == 1326)
                    {
                        throw new Win32Exception(1326);
                    }
                }
                return ex.Message;
            }
        }

        public string InvokeRemotePSCommand(string hostname, NetworkCredential credentials, Command PScommand)
        {
            try
            {
                var psCreds = new PSCredential(credentials.UserName, credentials.SecurePassword);

                string shellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";
                WSManConnectionInfo connInfo = new WSManConnectionInfo(false, hostname, 5985, "/wsman", shellUri, psCreds);

                using (Runspace remoteRunSpace = RunspaceFactory.CreateRunspace(connInfo))
                {
                    //(connInfo)
                    remoteRunSpace.Open();
                    remoteRunSpace.SessionStateProxy.SetVariable("cred", psCreds);

                    using (var powerSh = PowerShell.Create())
                    {
                        powerSh.Runspace = remoteRunSpace;
                        powerSh.Streams.Error.DataAdded += PSEventHandler;
                        powerSh.Commands.AddCommand(PScommand);
                        CurrentPowerShellObject = powerSh;

                        Collection<PSObject> results = powerSh.Invoke();
                        //Task.Delay(10000).Wait()
                        StringBuilder stringBuilder = new StringBuilder();

                        foreach (var obj in results)
                        {
                            stringBuilder.AppendLine(obj.ToString());
                        }

                        return DataConsistency.CleanDBValue((stringBuilder.ToString())).ToString();


                    }

                }
            }
            catch (Exception ex)
            {
                //Check for incorrect username/password error and rethrow a Win32Exception to be caught in the error handler.
                //Makes sure that the global admin credentials variable is cleared so that a new prompt will be shown on the next attempt. See: VerifyAdminCreds method.
                if (ex is PSRemotingTransportException)
                {
                    var transportEx = (PSRemotingTransportException)ex;
                    if (transportEx.ErrorCode == 1326)
                    {
                        throw new Win32Exception(1326);
                    }
                }
                return ex.Message;
            }
        }

        public async Task<bool> ExecutePowerShellScript(string hostname, byte[] scriptByte)
        {
            //  Dim PSWrapper As New PowerShellWrapper
            var UpdateResult = await Task.Run(() => { return ExecuteRemotePSScript(hostname, scriptByte, SecurityTools.AdminCreds); });
            if (UpdateResult != "")
            {
                OtherFunctions.Message(UpdateResult, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Error Running Script");
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> InvokePowerShellCommand(string hostname, Command PScommand)
        {
            // Dim PSWrapper As New PowerShellWrapper
            var UpdateResult = await Task.Run(() => { return InvokeRemotePSCommand(hostname, SecurityTools.AdminCreds, PScommand); });
            if (UpdateResult != "")
            {
                OtherFunctions.Message(UpdateResult, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Error Running Script");
                return false;
            }
            else
            {
                return true;
            }
        }

        public void StopPowerShellCommand()
        {
            try
            {
                if (CurrentPowerShellObject != null)
                {
                    CurrentPowerShellObject.Stop();
                }
            }
            catch
            {
                //don't care about errors here
            }
        }

        public void StopPiplineCommand()
        {
            try
            {
                if (CurrentPipelineObject != null)
                {
                    CurrentPipelineObject.Stop();
                }
            }
            catch
            {
                //don't care about errors here
            }
        }


        private void PSEventHandler(object sender, DataAddedEventArgs e)
        {
           //TODO: Fix or remove this.
            //ErrorRecord newRecord = (PSDataCollection<ErrorRecord>)sender(e.Index);

            //Debug.Print(newRecord.Exception.Message);
        }

        private string LoadScript(byte[] scriptBytes)
        {
            try
            {
                // Create an instance of StreamReader to read from our file.
                // The using statement also closes the StreamReader.
                // Dim sr As New StreamReader(filename)
                using (StreamReader sr = new StreamReader(new MemoryStream(scriptBytes), Encoding.ASCII))
                {
                    // use a string builder to get all our lines from the file
                    StringBuilder fileContents = new StringBuilder();

                    // string to hold the current line
                    string curLine = "";

                    // loop through our file and read each line into our
                    // stringbuilder as we go along
                    do
                    {
                        // read each line and MAKE SURE YOU ADD BACK THE
                        // LINEFEED THAT IT THE ReadLine() METHOD STRIPS OFF
                        curLine = sr.ReadLine();
                        fileContents.Append(curLine + Environment.NewLine);
                    } while (!(curLine == null));

                    // close our reader now that we are done
                    sr.Close();

                    // call RunScript and pass in our file contents
                    // converted to a string
                    return fileContents.ToString();
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                string errorText = "The file could not be read:";
                errorText += e.Message + "\\n";
                return errorText;
            }

        }

    }

}
