Imports MySql.Data.MySqlClient
Public Class ReportView
    Private Sub ReportView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim da As New MySqlDataAdapter
        'Dim rows As Integer
        'da.SelectCommand = Return_SQLCommand(strLastQry) 'New MySqlCommand(strLastQry) '"SELECT * FROM devices")
        ' da.SelectCommand.Connection = GlobalConn
        dt = Return_SQLTable(strLastQry)
        'da.Fill(dt)
        'rows = ds.Tables(0).Rows.Count
        'For Each row As DataTable In dt.Rows
        '    blah = row("dev_eq_type")
        'Next
        Dim i As Integer
        For i = 0 To dt.Rows.Count - 1
            dt.Rows(i)("dev_eq_type") = GetHumanValue(ComboType.EquipType, dt.Rows(i)("dev_eq_type"))
            dt.Rows(i)("dev_location") = GetHumanValue(ComboType.Location, dt.Rows(i)("dev_location"))
        Next
        With ReportViewer1.LocalReport
            .DataSources.Clear()
            '.ReportPath = My.Resources.YearsSincePurchase 'Application.StartupPath() & "\Reports\YearsSincePurchase.rdlc"
            .DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt))
        End With
        Me.ReportViewer1.RefreshReport()
    End Sub
End Class