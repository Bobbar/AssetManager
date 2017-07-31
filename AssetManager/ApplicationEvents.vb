Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active.
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.

    Partial Friend Class MyApplication

        Private Sub LoadSplash(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            SplashScreenForm.Show()
            Logger("Starting AssetManager...")
            Status("Checking Server Connection...")
            Using SQLComms As New MySqlComms(False)
                OfflineMode = Not SQLComms.OpenConnection
                'check connection
            End Using
            Status("Checking Local Cache...")
            If Not OfflineMode Then
                If Not VerifyCacheHashes(OfflineMode) Then
                    Status("Building Cache DB...")
                    RefreshLocalDBCache()
                End If
            Else
                CacheAvailable = VerifyCacheHashes(OfflineMode)
            End If

            If OfflineMode And Not CacheAvailable Then
                Message("Could not connect to server and the local DB cache is unavailable.  The application will now close.", vbOKOnly + vbExclamation, "No Connection")
                e.Cancel = True
                Exit Sub
            ElseIf OfflineMode And CacheAvailable Then
                Message("Could not connect to server. Running from local DB cache.", vbOKOnly + vbExclamation, "Cached Mode")
            End If

            Status("Loading Indexes...")
            BuildIndexes()
            Status("Checking Access Level...")
            PopulateAccessGroups()
            GetUserAccess()
            If Not CanAccess(AccessGroup.CanRun) Then
                Message("You do not have permission to run this software.", vbOKOnly + vbExclamation, "Access Denied", SplashScreenForm)
                e.Cancel = True
            End If
            Status("Ready!")
        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
            ErrHandle(e.Exception, System.Reflection.MethodInfo.GetCurrentMethod())
        End Sub

        Private Sub Status(Text As String)
            SplashScreenForm.SetStatus(Text)
        End Sub

    End Class

End Namespace