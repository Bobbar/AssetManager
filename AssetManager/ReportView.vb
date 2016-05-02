Imports MySql.Data.MySqlClient
Public Class ReportView
    Private Sub ReportView_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim ConnID As String = Guid.NewGuid.ToString
        Dim ds As New DataSet
        Dim dt As New DataTable

        Dim da As New MySqlDataAdapter
        Dim rows As Integer
        da.SelectCommand = New MySqlCommand(strLastQry) '"SELECT * FROM devices")
        da.SelectCommand.Connection = GetConnection(ConnID).DBConnection
        da.Fill(dt)
        CloseConnection(ConnID)
        'rows = ds.Tables(0).Rows.Count


        With ReportViewer1.LocalReport
            .DataSources.Clear()
            .ReportPath = Application.StartupPath() & "\Reports\YearsSincePurchase.rdlc"
            .DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt))

        End With



        Me.ReportViewer1.RefreshReport()
    End Sub
End Class