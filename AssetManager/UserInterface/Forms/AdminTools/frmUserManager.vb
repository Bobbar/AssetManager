Imports MySql.Data.MySqlClient
Public Class frmUserManager
    Private ModuleIndex As New List(Of Access_Info)
    Private CurrentUser As User_Info
    Private DataBinder As New BindingSource
    Private Qry As String = "SELECT * FROM " & users.TableName
    Private myAdapter As MySqlDataAdapter = SQLComms.Return_Adapter(Qry)
    Private SelectedRow As Integer
    Private Sub frmUserManager_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoadUserData()
    End Sub
    Private Sub LoadUserData()
        UserGrid.DataSource = DataBinder
        ListUsers()
        ModuleIndex = Asset.BuildModuleIndex()
        LoadModuleBoxes()
        UpdateAccessLabel()
    End Sub
    Private Sub ListUsers()
        SendToGrid(Asset.User_GetUserList) 'Comm.Return_SQLTable("SELECT * FROM users"))
    End Sub
    Private Sub SendToGrid(Results As List(Of User_Info)) ' Data() As Device_Info)
        Dim cmdBuilder As New MySqlCommandBuilder(myAdapter)
        Dim table As New DataTable
        table.Locale = System.Globalization.CultureInfo.InvariantCulture
        myAdapter.Fill(table)
        DataBinder.DataSource = table
        UserGrid.Columns(users.UID).ReadOnly = True
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
        AutoSizeCLBColumns(clbModules)
    End Sub
    Private Sub LoadModuleBoxes()
        Dim i As Integer = 0
        Dim intTopOffset As Integer = 0
        Dim chkModuleBox As CheckBox
        clbModules.Items.Clear()
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
        If TypeOf UserGrid.Item(GetColIndex(UserGrid, users.AccessLevel), UserGrid.CurrentRow.Index).Value Is Integer Then
            DisplayAccess(UserGrid.Item(GetColIndex(UserGrid, users.AccessLevel), UserGrid.CurrentRow.Index).Value)
            GetUserInfo()
        Else
            DisplayAccess(0)
        End If
        SelectedRow = e.RowIndex
    End Sub
    Private Sub GetUserInfo()
        With CurrentUser
            .intAccessLevel = GetCellValue(UserGrid, users.AccessLevel)
            .strUsername = GetCellValue(UserGrid, users.UserName)
            .strUID = GetCellValue(UserGrid, users.UID)
            .strFullname = GetCellValue(UserGrid, users.FullName)
        End With
    End Sub
    Private Sub cmdUpdate_Click(sender As Object, e As EventArgs) Handles cmdUpdate.Click
        Dim blah = Message("Are you sure?  Committed changes cannot be undone.", vbYesNo + vbQuestion, "Commit Changes", Me)
        If blah = DialogResult.Yes Then
            UserGrid.EndEdit()
            AddGUIDs()
            myAdapter.Update(CType(DataBinder.DataSource, DataTable))
            ListUsers()
            Asset.GetUserAccess()
        Else

        End If
    End Sub
    Private Sub AddGUIDs()
        For Each rows As DataGridViewRow In UserGrid.Rows
            If rows.Cells(GetColIndex(UserGrid, users.UID)).EditedFormattedValue = "" Then
                Dim UserUID As String = Guid.NewGuid.ToString
                rows.Cells(GetColIndex(UserGrid, users.UID)).Value = UserUID
            End If
        Next
    End Sub
    Private Sub AddAccessLevelToGrid()
        UserGrid.Rows(SelectedRow).Cells(GetColIndex(UserGrid, users.AccessLevel)).Selected = True
        UserGrid.BeginEdit(False)
        UserGrid.Rows(SelectedRow).Cells(GetColIndex(UserGrid, users.AccessLevel)).Value = CalcAccessLevel()
        UserGrid.EndEdit()
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
        AddAccessLevelToGrid()
    End Sub
    Private Sub UserGrid_KeyDown(sender As Object, e As KeyEventArgs) Handles UserGrid.KeyDown
        If e.KeyCode = Keys.Delete Then
            UserGrid.Rows.RemoveAt(SelectedRow)
        End If
    End Sub
    Private Sub UserGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles UserGrid.CellDoubleClick
        UserGrid.BeginEdit(False)
    End Sub

    Private Sub cmdRefresh_Click(sender As Object, e As EventArgs) Handles cmdRefresh.Click
        LoadUserData()
    End Sub
End Class