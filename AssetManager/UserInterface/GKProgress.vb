Public Class GKProgress
    Public MaxFiles As Integer = 1
    Public CurrentFileIdx As Integer = 0
    Public CurrentFile As String = ""
    Public PrevFile As String = ""
    Private Client As String
    Sub New(ClientName As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Client = ClientName
        Show()


    End Sub
    Private Sub RunUpdate()
        lstLog.Items.Add("Mapping drives...")
        txtUsername.ReadOnly = True
        txtPassword.ReadOnly = True
        Dim GKUpdate As New GK_Updater(Trim(txtUsername.Text), Trim(txtPassword.Text), Client)
        lblServerDrive.Text = "Server Map: Connecting..."
        If GKUpdate.ConnectServerDrive Then lblServerDrive.Text = "Server Map: Connected!"
        lblClientDrive.Text = "Client Map: Connecting..."
        If GKUpdate.ConnectClientDrive Then lblClientDrive.Text = "Client Map: Connected!"
        lstLog.Items.Add("Copying files...")
        If GKUpdate.CopyFiles(CurrentFile, MaxFiles, CurrentFileIdx) Then
            lstLog.Items.Add("Copy successful!")
            lstLog.Items.Add("Disconnecting Maps...")
            lblServerDrive.Text = "Server Map: Disconnecting..."
            If GKUpdate.RemoveServerDrive Then
                lblServerDrive.Text = "Server Map: Disconnected"
            Else
                lstLog.Items.Add("Server Map Disconnect Failed!")
            End If

            lblClientDrive.Text = "Client Map: Disconnecting..."
            If GKUpdate.RemoveClientDrive Then
                lblClientDrive.Text = "Client Map: Disconnected"
            Else
                lstLog.Items.Add("Client Map Disconnect Failed!")
            End If
            lstLog.Items.Add("All done!")

        Else

            lstLog.Items.Add("Copy Failed!")
        End If


    End Sub

    Private Sub tmrStatus_Tick(sender As Object, e As EventArgs) Handles tmrStatus.Tick
        pbarProgress.Maximum = MaxFiles
        pbarProgress.Value = CurrentFileIdx
        lblCurrentFile.Text = CurrentFile
        If PrevFile <> CurrentFile Then
            lstLog.Items.Add(CurrentFile)
            PrevFile = CurrentFile
        End If
        Application.DoEvents()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RunUpdate()
    End Sub
End Class