Public Class MunisUserForm

    Public ReadOnly Property EmployeeInfo As MunisEmployeeStruct
        Get
            Using Me
                If DialogResult = DialogResult.Yes Then
                    Return SelectedEmpInfo
                End If
                Return Nothing
            End Using
        End Get
    End Property

    Private SelectedEmpInfo As MunisEmployeeStruct
    Private Const intMaxResults As Integer = 50

    Sub New(parentForm As Form)
        InitializeComponent()
        Tag = parentForm
        Icon = parentForm.Icon
        ShowDialog(parentForm)
    End Sub

    Private Async Sub EmpNameSearch(Name As String)
        Try
            MunisResults.DataSource = Nothing
            Dim strColumns As String = "a_employee_number,a_name_last,a_name_first,a_org_primary,a_object_primary,a_location_primary,a_location_p_desc,a_location_p_short"
            Dim strQRY As String = "SELECT TOP " & intMaxResults & " " & strColumns & " FROM pr_employee_master WHERE a_name_last LIKE '%" & UCase(Name) & "%' OR a_name_first LIKE '" & UCase(Name) & "'"
            Dim MunisComms As New MunisComms
            SetWorking(True)
            Using results As DataTable = Await MunisComms.ReturnSqlTableAsync(strQRY)
                If results.Rows.Count < 1 Then Exit Sub
                MunisResults.DataSource = results
                MunisResults.ClearSelection()
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            SetWorking(False)
        End Try
    End Sub

    Private Sub SetWorking(Working As Boolean)
        pbWorking.Visible = Working
    End Sub

    Private Sub MunisUser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MunisResults.DefaultCellStyle = GridStyles
    End Sub

    Private Sub MunisResults_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles MunisResults.CellClick
        With SelectedEmpInfo
            .Name = MunisResults.Item(GetColIndex(MunisResults, "a_name_first"), MunisResults.CurrentRow.Index).Value.ToString & " " & MunisResults.Item(GetColIndex(MunisResults, "a_name_last"), MunisResults.CurrentRow.Index).Value.ToString.ToString
            .Number = MunisResults.Item(GetColIndex(MunisResults, "a_employee_number"), MunisResults.CurrentRow.Index).Value.ToString
        End With
        lblSelectedEmp.Text = "Selected Emp: " & SelectedEmpInfo.Name & " - " & SelectedEmpInfo.Number
    End Sub

    Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        Me.Cursor = Cursors.WaitCursor
        EmpNameSearch(Trim(txtSearchName.Text))
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub SelectEmp()
        If SelectedEmpInfo.Name <> "" AndAlso SelectedEmpInfo.Number <> "" Then
            AssetFunc.AddNewEmp(SelectedEmpInfo)
            Me.DialogResult = DialogResult.Yes
            Me.Close()
        Else
            Me.DialogResult = DialogResult.Abort
            Me.Close()
        End If
    End Sub

    Private Sub cmdAccept_Click(sender As Object, e As EventArgs) Handles cmdAccept.Click
        SelectEmp()
    End Sub

    Private Sub txtSearchName_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearchName.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.Cursor = Cursors.WaitCursor
            EmpNameSearch(Trim(txtSearchName.Text))
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub MunisResults_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles MunisResults.CellDoubleClick
        SelectEmp()
    End Sub

    Private Sub MunisUserForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        txtSearchName.Focus()
    End Sub
End Class