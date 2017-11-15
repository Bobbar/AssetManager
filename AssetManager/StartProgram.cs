using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AssetManager.UserInterface.Forms;



namespace AssetManager
{
    static class StartProgram
    {
        private static SplashScreenForm SplashScreen;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new Form1());

            Application.ThreadException += MyApplication_UnhandledException;

            ProcessCommandArgs();

            GlobalConstants.LocalDomainUser = Environment.UserName;

            bool ConnectionSuccessful = false;
            bool CacheAvailable = false;
            SplashScreen = new SplashScreenForm();
            SplashScreen.Show();
            // My.MyProject.Forms.SplashScreenForm.Show();
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
                // e.Cancel = true;
                Application.Exit();
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
            if (!SecurityTools.CanAccess(SecurityTools.AccessGroup.CanRun))
            {
                OtherFunctions.Message("You do not have permission to run this software.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Access Denied");
                // e.Cancel = true;
                Application.Exit();
                return;
            }
            Status("Ready!");
            Application.Run(new UserInterface.Forms.AssetManagement.MainForm());
            SplashScreen.Dispose();
        }


      
        private static void Status(string Text)
        {
            //My.MyProject.Forms.SplashScreenForm.SetStatus(Text);
            SplashScreen.SetStatus(Text);
        }

        private static bool CheckConnection()
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
        private static void ProcessCommandArgs()
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

        private static void MyApplication_UnhandledException(object sender, ThreadExceptionEventArgs e)
        {
            ErrorHandling.ErrHandle(e.Exception, System.Reflection.MethodInfo.GetCurrentMethod());
        }

    }
}
