Imports System.IO
Imports MySql.Data.MySqlClient

Class Attachments
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()
        Dim strFileName As String

        fd.Title = "Open File Dialog"
        fd.InitialDirectory = "C:\"
        fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            strFileName = fd.FileName
        End If
        Debug.Print(fd.FileName)
        UploadFile(fd.FileName)
    End Sub

    Private Sub UploadFile(FilePath As String)

        Dim ConnID As String = Guid.NewGuid.ToString
        'Dim reader As MySqlDataReader
        Dim table As New DataTable
        Dim strQry = "" '"Select * FROM devices, historical WHERE dev_UID = hist_dev_UID And dev_UID = '" & DeviceUID & "' ORDER BY hist_action_datetime DESC"
        Dim cmd As New MySqlCommand '(strQry, GetConnection(ConnID).DBConnection)
        ' reader = cmd.ExecuteReader



        Dim conn As New MySqlConnection
        conn = GetConnection(ConnID).DBConnection
        'Dim cmd As New MySqlCommand

        Dim SQL As String

        Dim FileSize As UInt32
        Dim rawData() As Byte
        Dim fs As FileStream

        'conn.ConnectionString = "server=127.0.0.1;" _
        '    & "uid=root;" _
        '    & "pwd=12345;" _
        '    & "database=test"

        Try
            fs = New FileStream(FilePath, FileMode.Open, FileAccess.Read)
            FileSize = fs.Length
            Dim strFilename As String = Path.GetFileNameWithoutExtension(FilePath)
            Dim strFileType As String = Path.GetExtension(FilePath)
            rawData = New Byte(FileSize) {}
            fs.Read(rawData, 0, FileSize)
            fs.Close()

            'conn.Open()

            SQL = "INSERT INTO attachments VALUES(@UID, @attach_dev_UID, @attach_file_name, @attach_file_type, @attach_file_binary, @attach_file_size)"
            Debug.Print("Size: " & FileSize)
            cmd.Connection = conn
            cmd.CommandText = SQL
            cmd.Parameters.AddWithValue("@UID", "1")
            cmd.Parameters.AddWithValue("@attach_dev_UID", CurrentDevice.strGUID)
            cmd.Parameters.AddWithValue("@attach_file_name", strFilename)
            cmd.Parameters.AddWithValue("@attach_file_type", strFileType)
            cmd.Parameters.AddWithValue("@attach_file_binary", rawData)
            cmd.Parameters.AddWithValue("@attach_file_size", FileSize)

            cmd.ExecuteNonQuery()

            MessageBox.Show("File Inserted into database successfully!",
            "Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

            conn.Close()
        Catch ex As Exception
            MessageBox.Show("There was an error: " & ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    Private Sub ListAttachments(DeviceUID As String)


        On Error GoTo errs

        Dim ConnID As String = Guid.NewGuid.ToString
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        Dim strQry = "Select UID,attach_file_name,attach_file_type,attach_file_size FROM attachments WHERE attach_dev_UID='" & DeviceUID & "'"
        Dim cmd As New MySqlCommand(strQry, GetConnection(ConnID).DBConnection)
        reader = cmd.ExecuteReader
        ListBox1.Items.Clear()
        Dim strFilename As String, strFileType As String, strFullName As String




        With reader
            Do While .Read()
                strFullName = !attach_file_name +!attach_file_type

                ListBox1.Items.Add(strFullName)
            Loop
        End With
        CloseConnection(ConnID)



        Exit Sub
errs:
        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            'DoneWaiting()
            Resume Next
        Else
            EndProgram()
        End If


    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged

    End Sub
    Private Sub OpenAttachment(AttachUID As String)




    End Sub
End Class