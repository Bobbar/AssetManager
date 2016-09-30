Public Class frmUserManager
    Private ModuleIndex As New List(Of Access_Info)

    Private CurrentUser As User_Info
    Private Sub frmUserManager_Load(sender As Object, e As EventArgs) Handles Me.Load
        ListUsers()
        ModuleIndex = Asset.BuildModuleIndex()
        LoadModuleBoxes()
    End Sub
    Private Sub ListUsers()
        Dim Comm As New clsMySQL_Comms
        SendToGrid(Comm.Return_SQLTable("SELECT * FROM users"))
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
        Dim clbItemStates(clbModules.Items.Count - 1) As CheckState
        For Each chkBox As CheckBox In clbModules.Items
            If CanAccess(chkBox.Name, intAccessLevel) Then
                clbItemStates(clbModules.Items.IndexOf(chkBox)) = CheckState.Checked
            Else
                clbItemStates(clbModules.Items.IndexOf(chkBox)) = CheckState.Unchecked
            End If
        Next
        For i As Integer = 0 To clbItemStates.Count - 1
            clbModules.SetItemChecked(i, clbItemStates(i))
        Next
        UpdateAccessLabel()
        GetUserInfo()
        AutoSizeCLBColumns(clbModules)
    End Sub
    Private Sub LoadModuleBoxes()
        Dim i As Integer = 0
        Dim intTopOffset As Integer = 0
        Dim chkModuleBox As CheckBox
        For Each ModuleBox As Access_Info In ModuleIndex
            chkModuleBox = New CheckBox
            With chkModuleBox
                .Text = ModuleBox.strDesc
                .Name = ModuleBox.strModule
            End With
            clbModules.DisplayMember = "Text"
            clbModules.Items.Add(chkModuleBox)
        Next
        AutoSizeCLBColumns(clbModules)
    End Sub

    Private Function CalcAccessLevel() As Integer
        Dim intAccessLevel As Integer = 0
        For Each chkBox As CheckBox In clbModules.Items
            If clbModules.GetItemCheckState(clbModules.Items.IndexOf(chkBox)) = CheckState.Checked Then
                intAccessLevel += GetSecGroupValue(chkBox.Name)
            End If
        Next
        Return intAccessLevel
    End Function

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
    Private Sub cmdUpdate_Click(sender As Object, e As EventArgs) Handles cmdUpdate.Click
        Asset.UpdateUser(CurrentUser, CalcAccessLevel)
        ListUsers()
        Asset.GetUserAccess()
    End Sub
    Private Sub AutoSizeCLBColumns(CLB As CheckedListBox)
        Dim intMaxLen As Integer = 0
        Dim fntCheckBoxFont As Font = CLB.Font
        Dim intTextSize As Size
        For Each item As CheckBox In CLB.Items
            intTextSize = TextRenderer.MeasureText(item.Text, fntCheckBoxFont)
            If Size.Width > intMaxLen Then intMaxLen = Size.Width
        Next
        CLB.ColumnWidth = intMaxLen / 4
    End Sub
    Private Sub UpdateAccessLabel()
        lblAccessValue.Text = "Selected Access Level: " & CalcAccessLevel()
    End Sub
    Private Sub clbModules_MouseUp(sender As Object, e As MouseEventArgs) Handles clbModules.MouseUp
        UpdateAccessLabel()
    End Sub
End Class