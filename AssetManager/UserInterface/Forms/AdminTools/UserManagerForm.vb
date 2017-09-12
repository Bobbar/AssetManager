
Public Class UserManagerForm
    Private ModuleIndex As New List(Of AccessGroupStruct)
    Private CurrentUser As LocalUserInfoStruct
    Private UserDataQuery As String = "SELECT * FROM " & UsersCols.TableName
    Private SelectedRow As Integer

    Sub New(parentForm As Form)
        InitializeComponent()
        Tag = parentForm
        Icon = parentForm.Icon
        Show()
    End Sub

    Private Sub frmUserManager_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoadUserData()
    End Sub

    Private Sub LoadUserData()
        ListUsers()
        ModuleIndex = AssetFunc.BuildModuleIndex()
        LoadModuleBoxes()
        UpdateAccessLabel()
    End Sub

    Private Sub ListUsers()
        SendToGrid()
    End Sub

    Private Sub SendToGrid()
        UserGrid.DataSource = DBFunc.GetDatabase.DataTableFromQueryString(UserDataQuery)
        UserGrid.Columns(UsersCols.UID).ReadOnly = True
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
            clbModules.SetItemCheckState(i, clbItemStates(i))
        Next
        UpdateAccessLabel()
        AutoSizeCLBColumns(clbModules)
    End Sub

    Private Sub LoadModuleBoxes()
        Dim chkModuleBox As CheckBox
        clbModules.Items.Clear()
        For Each ModuleBox As AccessGroupStruct In ModuleIndex
            chkModuleBox = New CheckBox
            With chkModuleBox
                .Text = ModuleBox.Description
                .Name = ModuleBox.AccessModule
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
        If TypeOf UserGrid.Item(GetColIndex(UserGrid, UsersCols.AccessLevel), UserGrid.CurrentRow.Index).Value Is Integer Then
            DisplayAccess(CInt(UserGrid.Item(GetColIndex(UserGrid, UsersCols.AccessLevel), UserGrid.CurrentRow.Index).Value))
            GetUserInfo()
        Else
            DisplayAccess(0)
        End If
        SelectedRow = e.RowIndex
    End Sub

    Private Sub GetUserInfo()
        With CurrentUser
            .AccessLevel = CInt(GetCurrentCellValue(UserGrid, UsersCols.AccessLevel))
            .UserName = GetCurrentCellValue(UserGrid, UsersCols.UserName)
            .GUID = GetCurrentCellValue(UserGrid, UsersCols.UID)
            .Fullname = GetCurrentCellValue(UserGrid, UsersCols.FullName)
        End With
    End Sub

    Private Sub cmdUpdate_Click(sender As Object, e As EventArgs) Handles cmdUpdate.Click
        Try
            Dim blah = Message("Are you sure?  Committed changes cannot be undone.", vbYesNo + vbQuestion, "Commit Changes", Me)
            If blah = DialogResult.Yes Then
                UserGrid.EndEdit()
                AddGUIDs()
                DBFunc.GetDatabase.UpdateTable(UserDataQuery, DirectCast(UserGrid.DataSource, DataTable))
                ListUsers()
                GetUserAccess()
            Else

            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub AddGUIDs()
        For Each rows As DataGridViewRow In UserGrid.Rows
            If rows.Cells(GetColIndex(UserGrid, UsersCols.UID)).EditedFormattedValue.ToString = "" Then
                Dim UserUID As String = Guid.NewGuid.ToString
                rows.Cells(GetColIndex(UserGrid, UsersCols.UID)).Value = UserUID
            End If
        Next
    End Sub

    Private Sub AddAccessLevelToGrid()
        UserGrid.Rows(SelectedRow).Cells(GetColIndex(UserGrid, UsersCols.AccessLevel)).Selected = True
        UserGrid.BeginEdit(False)
        UserGrid.Rows(SelectedRow).Cells(GetColIndex(UserGrid, UsersCols.AccessLevel)).Value = CalcAccessLevel()
        UserGrid.EndEdit()
    End Sub

    Private Sub AutoSizeCLBColumns(CLB As CheckedListBox)
        CLB.ColumnWidth = CInt(CLB.Width / 2)
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