Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Private Sub LoadSplash(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            Dim splash As SplashScreen = CType(My.Application.SplashScreen, SplashScreen)

            ' DateTimeLabel.ToolTipText = My.Application.Info.Version.ToString
            'ToolStrip1.BackColor = colAssetToolBarColor
            Logger("Starting AssetManager...")
            splash.Status = "Loading..."
            '  Status("Loading...", splash)
            ' SplashScreen.Show()
            Status("Checking Server Connection...", splash)
            Using SQLComms As New clsMySQL_Comms
                If SQLComms.OpenConnection() Then
                    ' ConnectionReady()
                Else
                    Dim blah = Message("Error connecting to server!", vbOKOnly + vbExclamation, "Could not connect", splash)
                    EndProgram()
                End If
            End Using
            ' ExtendedMethods.DoubleBuffered(ResultGrid, True)
            Status("Loading Indexes...", splash)
            BuildIndexes()
            Status("Checking Access Level...", splash)
            Asset.GetAccessLevels()
            Asset.GetUserAccess()
            If Not CanAccess(AccessGroup.CanRun, UserAccess.intAccessLevel) Then
                Message("You do not have permission to run this software.", vbOKOnly + vbExclamation, "Access Denied", splash)
                EndProgram()
            End If
            'If CanAccess(AccessGroup.IsAdmin, UserAccess.intAccessLevel) Then
            '    AdminDropDown.Visible = True
            'Else
            '    AdminDropDown.Visible = False
            'End If
            'GetGridStyles()
            'SetGridStyle(ResultGrid)
            'ConnectionWatchDog.RunWorkerAsync()
            Status("Ready!", splash)

        End Sub
        Public Sub Status(Text As String, splash As SplashScreen)
            splash.lblStatus.Text = Text
            splash.Refresh()
        End Sub
    End Class
End Namespace
