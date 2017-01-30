Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Private Sub LoadSplash(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            Try
                SplashScreen1.Show()
                Logger("Starting AssetManager...")
                Status("Loading...")
                Status("Checking Server Connection...")
                Using SQLComms As New clsMySQL_Comms
                    'If SQLComms.OpenConnection() Then
                    '    ' ConnectionReady()
                    'Else
                    '    Dim blah = Message("Error connecting to server!", vbOKOnly + vbExclamation, "Could not connect", SplashScreen1)
                    '    EndProgram()
                    'End If
                End Using
                Status("Loading Indexes...")
                BuildIndexes()
                Status("Checking Access Level...")
                Asset.GetAccessLevels()
                Asset.GetUserAccess()
                If Not CanAccess(AccessGroup.CanRun, UserAccess.intAccessLevel) Then
                    Message("You do not have permission to run this software.", vbOKOnly + vbExclamation, "Access Denied", SplashScreen1)
                    EndProgram()
                End If
                Status("Ready!")
            Catch ex As Exception
                ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
                e.Cancel = True
                EndProgram()
            End Try
        End Sub
        Public Sub Status(Text As String)
            SplashScreen1.lblStatus.Text = Text
            SplashScreen1.Refresh()
        End Sub
    End Class
End Namespace
