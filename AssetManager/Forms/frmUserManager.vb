Public Class frmUserManager
    Private ModuleArray() As Access_Info

    Private Structure User_Info
        Public strUsername As String
        Public strFullname As String
        Public intAccessLevel As Integer
        Public strUID As String
    End Structure
    Private CurrentUser As User_Info
    Private Sub frmUserManager_Load(sender As Object, e As EventArgs) Handles Me.Load
        ListUsers()
        BuildModuleArray()
        LoadModuleBoxes()
    End Sub
    Private Sub ListUsers()
        SendToGrid(ReturnSQLTable("SELECT * FROM users"))
    End Sub
    Private Sub SendToGrid(Results As DataTable) ' Data() As Device_Info)
        Try
            Dim table As New DataTable
            table.Columns.Add("Username", GetType(String))
            table.Columns.Add("Full Name", GetType(String))
            table.Columns.Add("Access Level", GetType(String))
            table.Columns.Add("UID", GetType(String))
            For Each r As DataRow In Results.Rows
                table.Rows.Add(r.Item("usr_username"),
                               r.Item("usr_fullname"),
                               r.Item("usr_access_level"),
                               r.Item("usr_UID"))
            Next
            UserGrid.DataSource = table
            UserGrid.ClearSelection()
            table.Dispose()
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub DisplayAccess(intAccessLevel As Integer)
        Dim i As Integer = 0

        For i = 0 To ModuleArray.Count - 1

            If CanAccess(ModuleArray(i).strModule, intAccessLevel) Then
                clbModules.SetItemCheckState(i, CheckState.Checked)
            Else
                clbModules.SetItemCheckState(i, CheckState.Unchecked)
            End If

        Next

        GetUserInfo()


    End Sub
    Private Sub LoadModuleBoxes()
        ' Dim chkModuleBox(ModuleArray.Count - 1) As CheckBox
        Dim i As Integer = 0
        Dim intTopOffset As Integer = 0
        Dim chkModuleBox As CheckBox
        For Each ModuleBox As Access_Info In ModuleArray

            chkModuleBox = New CheckBox
            With chkModuleBox

                'pnlModule.Controls.Add(chkModuleBox(i))
                '.Parent = pnlModule.
                .Text = ModuleBox.strDesc
                .Name = ModuleBox.strModule
                '.Top = pnlModule.Location.X + intTopOffset



            End With

            'intTopOffset += 20 'chkModuleBox(i).Height
            'i += 1
            clbModules.DisplayMember = "Text"
            clbModules.Items.Add(chkModuleBox)


        Next


    End Sub
    Private Sub BuildModuleArray()
        Dim ModuleTable As DataTable = ReturnSQLTable("SELECT * FROM security ORDER BY sec_access_level")
        Dim i As Integer = 0
        ReDim ModuleArray(ModuleTable.Rows.Count - 1)
        For Each row As DataRow In ModuleTable.Rows
            With ModuleArray(i)
                .intLevel = row.Item("sec_access_level")
                .strModule = row.Item("sec_module")
                .strDesc = row.Item("sec_desc")
            End With
            i += 1
            ' ReDim Preserve ModuleArray(ModuleArray.Count)
        Next
    End Sub
    Private Function CalcAccessLevel() As Integer
        Dim intAccessLevel As Integer = 0
        For i As Integer = 0 To ModuleArray.Count - 1
            If clbModules.GetItemCheckState(i) = CheckState.Checked Then
                intAccessLevel += ModuleArray(i).intLevel
            End If
        Next
        Return intAccessLevel
    End Function
    Private Sub UpdateUser(User As User_Info)
        CurrentUser.intAccessLevel = CalcAccessLevel()
        UpdateSQLValue("users", "usr_access_level", CurrentUser.intAccessLevel, "usr_UID", CurrentUser.strUID)
        ListUsers()
    End Sub

    Private Sub UserGrid_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles UserGrid.CellContentDoubleClick
        'DisplayAccess(UserGrid.Item(GetColIndex(UserGrid, "Access Level"), UserGrid.CurrentRow.Index).Value)
    End Sub

    Private Sub UserGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles UserGrid.CellClick
        DisplayAccess(UserGrid.Item(GetColIndex(UserGrid, "Access Level"), UserGrid.CurrentRow.Index).Value)
    End Sub
    Private Sub GetUserInfo()
        With CurrentUser
            .intAccessLevel = GetCellValue(UserGrid, "Access Level")
            .strUsername = GetCellValue(UserGrid, "Username")
            .strUID = GetCellValue(UserGrid, "UID")
            .strFullname = GetCellValue(UserGrid, "Full Name")
        End With
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Debug.Print(CalcAccessLevel)
    End Sub

    Private Sub cmdUpdate_Click(sender As Object, e As EventArgs) Handles cmdUpdate.Click
        UpdateUser(CurrentUser)
        GetUserAccess()
    End Sub
End Class