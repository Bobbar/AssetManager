Imports System.ComponentModel
Public Class frmSibiSelector
    Public ReadOnly Property SibiUID As String
        Get
            Return SelectedUID
        End Get
    End Property
    Private SelectedUID As String
    Private Sub frmSibiSelector_Load(sender As Object, e As EventArgs) Handles Me.Load
        ExtendedMethods.DoubleBuffered(ResultGrid, True)
        ShowAll()
    End Sub
    Private Sub SendToGrid(Results As DataTable) ' Data() As Device_Info)
        Try
            'StatusBar(strLoadingGridMessage)
            Dim table As New DataTable
            table.Columns.Add("Request #", GetType(String))
            table.Columns.Add("Status", GetType(String))
            table.Columns.Add("Description", GetType(String))
            table.Columns.Add("Request User", GetType(String))
            table.Columns.Add("Request Type", GetType(String))
            table.Columns.Add("Need By", GetType(String))
            table.Columns.Add("PO Number", GetType(String))
            table.Columns.Add("Req. Number", GetType(String))
            table.Columns.Add("UID", GetType(String))
            'table.Columns.Add("Location", GetType(String))
            'table.Columns.Add("Purchase Date", GetType(String))
            'table.Columns.Add("Replace Year", GetType(String))
            'table.Columns.Add("GUID", GetType(String))
            For Each r As DataRow In Results.Rows
                table.Rows.Add(NoNull(r.Item("sibi_request_number")),
                               GetHumanValue(ComboType.SibiStatusType, r.Item("sibi_status")),
                               NoNull(r.Item("sibi_description")),
                               NoNull(r.Item("sibi_request_user")),
                               GetHumanValue(ComboType.SibiRequestType, r.Item("sibi_type")),
                               NoNull(r.Item("sibi_need_by")),
                               NoNull(r.Item("sibi_PO")),
                               NoNull(r.Item("sibi_requisition_number")),
                               NoNull(r.Item("sibi_uid")))
            Next
            'bolGridFilling = True
            ResultGrid.DataSource = table
            ResultGrid.ClearSelection()
            'bolGridFilling = False
            table.Dispose()
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub ShowAll()
        SendToGrid(ReturnSQLTable("SELECT * FROM sibi_requests ORDER BY sibi_need_by"))
    End Sub
    Private Sub ResultGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellDoubleClick
        SelectedUID = ResultGrid.Item(GetColIndex(ResultGrid, "UID"), ResultGrid.CurrentRow.Index).Value
        Me.DialogResult = DialogResult.OK
    End Sub
End Class