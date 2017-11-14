//using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.VisualBasic.ApplicationServices;

namespace AssetManager
{
    // The following events are available for MyApplication:
    // Startup: Raised when the application starts, before the startup form is created.
    // Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    // UnhandledException: Raised if the application encounters an unhandled exception.
    // StartupNextInstance: Raised when launching a single-instance application and the application is already active.
    // NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.

    internal partial class MyApplication
    {

        private void LoadSplash(object sender, Microsoft.VisualBasic.ApplicationServices.StartupEventArgs e)
        {
            ProcessCommandArgs();
            bool ConnectionSuccessful = false;
            bool CacheAvailable = false;
            My.MyProject.Forms.SplashScreenForm.Show();
            Logging.Logger("Starting AssetManager...");
            Status("Checking Server Connection...");

            //check connection
            ConnectionSuccessful = CheckConnection();

            ServerInfo.ServerPinging = ConnectionSuccessful;

            Status("Checking Local Cache...");
            if (ConnectionSuccessful)
            {
                if (!DBCacheFunctions.VerifyCacheHashes())
                {
                    Status("Building Cache DB...");
                    DBCacheFunctions.RefreshLocalDBCache();
                }
            }
            else
            {
                CacheAvailable = DBCacheFunctions.VerifyCacheHashes(ConnectionSuccessful);
            }
            if (!ConnectionSuccessful & !CacheAvailable)
            {
                OtherFunctions.Message("Could not connect to server and the local DB cache is unavailable.  The application will now close.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "No Connection");
                e.Cancel = true;
                return;
            }
            else if (!ConnectionSuccessful & CacheAvailable)
            {
                GlobalSwitches.CachedMode = true;
                OtherFunctions.Message("Could not connect to server. Running from local DB cache.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Cached Mode");
            }

            Status("Loading Indexes...");
            AttribIndexFunctions.PopulateAttributeIndexes();
            Status("Checking Access Level...");
           SecurityTools.PopulateAccessGroups();
            SecurityTools.GetUserAccess();
            if (!SecurityTools.CanAccess(AssetManager.SecurityTools.AccessGroup.CanRun))
            {
                OtherFunctions.Message("You do not have permission to run this software.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Access Denied");
                e.Cancel = true;
            }
            Status("Ready!");
        }

        private bool CheckConnection()
        {
            try
            {
                using (MySQLDatabase SQLComms = new MySQLDatabase())
                {
                    using (var conn = SQLComms.NewConnection())
                    {
                        return SQLComms.OpenConnection(conn, true);
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void MyApplication_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ErrorHandling.ErrHandle((Exception)e.ExceptionObject, System.Reflection.MethodInfo.GetCurrentMethod());
        }

        private void Status(string Text)
        {
            My.MyProject.Forms.SplashScreenForm.SetStatus(Text);
        }

        private void ProcessCommandArgs()
        {
            try
            {
                var Args = Environment.GetCommandLineArgs();
                for (int i = 1; i <= Args.Length - 1; i++)
                {
                    try
                    {
                        var ArgToEnum = (CommandArgs)CommandArgs.Parse(typeof(CommandArgs), (Args[i]).ToUpper());
                        switch (ArgToEnum)
                        {
                            case CommandArgs.TESTDB:
                                ServerInfo.CurrentDataBase = Databases.test_db;
                                break;
                            case CommandArgs.VINTONDD:
                               ServerInfo.CurrentDataBase = Databases.vintondd;
                                break;
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        Logging.Logger("Invalid argument: " + Args[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

    }

}
