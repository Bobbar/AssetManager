﻿Imports System.ComponentModel
Public Class frmSibiSelector
    Public ReadOnly Property SibiUID As String
        Get
            Return SelectedUID
        End Get
    End Property
    Private SelectedUID As String
    Sub New(ParentForm As Form)
        InitializeComponent()
        Icon = ParentForm.Icon
        ShowDialog(ParentForm)
    End Sub
    Private Sub frmSibiSelector_Load(sender As Object, e As EventArgs) Handles Me.Load
        ExtendedMethods.DoubleBufferedDataGrid(ResultGrid, True)
        ShowAll()
    End Sub
    Private Sub SendToGrid(Results As DataTable) ' Data() As Device_Info)
        Try
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
            For Each r As DataRow In Results.Rows
                table.Rows.Add(NoNull(r.Item("sibi_request_number")),
                               GetHumanValue(SibiIndex.StatusType, r.Item("sibi_status").ToString),
                               NoNull(r.Item("sibi_description")),
                               NoNull(r.Item("sibi_request_user")),
                               GetHumanValue(SibiIndex.RequestType, r.Item("sibi_type").ToString),
                               NoNull(r.Item("sibi_need_by")),
                               NoNull(r.Item("sibi_PO")),
                               NoNull(r.Item("sibi_requisition_number")),
                               NoNull(r.Item("sibi_uid")))
            Next
            ResultGrid.DataSource = table
            ResultGrid.ClearSelection()
            table.Dispose()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub ShowAll()
        Using SQLComms As New clsMySQL_Comms
            SendToGrid(SQLComms.Return_SQLTable("SELECT * FROM sibi_requests ORDER BY sibi_need_by"))
        End Using
    End Sub
    Private Sub ResultGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellDoubleClick
        SelectedUID = ResultGrid.Item(GetColIndex(ResultGrid, "UID"), ResultGrid.CurrentRow.Index).Value.ToString
        Me.DialogResult = DialogResult.OK
    End Sub
End Class