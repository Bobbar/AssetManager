Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active.
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.

    Partial Friend Class MyApplication

        Private Sub LoadSplash(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            '  Try
            SplashScreenForm.Show()
            Logger("Starting AssetManager...")
            Status("Checking Server Connection...")
            Using SQLComms As New MySQL_Comms(False)
                OfflineMode = Not SQLComms.OpenConnection
                'check connection
            End Using
            If Not OfflineMode Then
                Status("Building Cache DB...")
                RefreshLocalDBCache()
            Else
                Status("Checking Local Cache...")
                CacheAvailable = VerifyLocalCache(False)
            End If

            If OfflineMode And Not CacheAvailable Then
                Message("Could not connect to server and the local DB cache is unavailable.  The application will now close.", vbOKOnly + vbExclamation, "No Connection")
                e.Cancel = True
                EndProgram()
                Exit Sub
            ElseIf OfflineMode And CacheAvailable Then
                Message("Could not connect to server. Running from local DB cache.", vbOKOnly + vbExclamation, "Cached Mode")
            End If

            Status("Loading Indexes...")
            BuildIndexes()
            Status("Checking Access Level...")
            GetAccessLevels()
            GetUserAccess()
            If Not CanAccess(AccessGroup.CanRun) Then
                Message("You do not have permission to run this software.", vbOKOnly + vbExclamation, "Access Denied", SplashScreenForm)
                EndProgram()
            End If
            Status("Ready!")
            'Catch ex As Exception
            '    ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())

            '    Using SQLiteComm As New SQLite_Comms(False)
            '        If SQLiteComm.OpenConnection Then
            '            OfflineMode = True
            '        End If
            '    End Using

            '    'e.Cancel = True
            '    'EndProgram()
            'End Try
        End Sub

        Public Sub Status(Text As String)
            SplashScreenForm.lblStatus.Text = Text
            SplashScreenForm.Refresh()
        End Sub

    End Class

End Namespace