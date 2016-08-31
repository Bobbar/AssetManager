Public Class frmSibiMain
    Private Sub cmdShowAll_Click(sender As Object, e As EventArgs) Handles cmdShowAll.Click
        ShowAll()
    End Sub
    Private Sub frmSibiMain_Load(sender As Object, e As EventArgs) Handles Me.Load
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
            table.Columns.Add("RT Number", GetType(String))
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
                               NoNull(r.Item("sibi_RT_number")),
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
    Public Sub ShowAll()
        SendToGrid(MySQLDB.Return_SQLTable("SELECT * FROM sibi_requests ORDER BY sibi_request_number DESC"))
    End Sub
    Private Sub cmdManage_Click(sender As Object, e As EventArgs) Handles cmdManage.Click
        frmManageRequest.ClearAll()
        frmManageRequest.NewRequest()
        frmManageRequest.Show()
    End Sub
    Private Sub ResultGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellDoubleClick
        Dim ManRequest As New frmManageRequest
        ManRequest.OpenRequest(ResultGrid.Item(GetColIndex(ResultGrid, "UID"), ResultGrid.CurrentRow.Index).Value)
    End Sub
    Private Sub ResultGrid_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles ResultGrid.RowPostPaint
        If e.RowIndex > -1 Then
            Dim dvgRow As DataGridViewRow = ResultGrid.Rows(e.RowIndex)
            dvgRow.DefaultCellStyle.BackColor = GetRowColor(dvgRow.Cells("Status").Value.ToString)
            dvgRow.DefaultCellStyle.ForeColor = GetFontColor(GetRowColor(dvgRow.Cells("Status").Value.ToString))
        End If
    End Sub
    Private Function GetRowColor(Value As String) As Color
        Dim DBVal As String = GetDBValueFromHuman(ComboType.SibiStatusType, Value)
        Select Case DBVal
            Case "NEW"
                Return ColorAlphaBlend(Color.FromArgb(0, 255, 30))
            Case "QTN"
                Return ColorAlphaBlend(Color.FromArgb(242, 255, 0))
            Case "QTR"
                Return ColorAlphaBlend(Color.FromArgb(255, 208, 0))
            Case "QRC"
                Return ColorAlphaBlend(Color.FromArgb(255, 162, 0))
            Case "RQN"
                Return ColorAlphaBlend(Color.FromArgb(0, 255, 251))
            Case "RQR"
                Return ColorAlphaBlend(Color.FromArgb(0, 140, 255))
            Case "POS"
                Return ColorAlphaBlend(Color.FromArgb(197, 105, 255))
            Case "SHD"
                Return ColorAlphaBlend(Color.FromArgb(255, 79, 243))
            Case "ORC"
                Return ColorAlphaBlend(Color.FromArgb(79, 144, 255))
            Case "NPAY"
                Return ColorAlphaBlend(Color.FromArgb(255, 36, 36))
            Case "RCOMP"
                Return ColorAlphaBlend(Color.FromArgb(158, 158, 158))
            Case "ONH"
                Return ColorAlphaBlend(Color.FromArgb(255, 255, 255))
        End Select
    End Function
    Private Function ColorAlphaBlend(InColor As Color) As Color 'blend colors with darker color so they aren't so intense
        Dim DarkColor As Color = Color.FromArgb(222, 222, 222) 'gray color
        Dim BlendColor As Color
        BlendColor = Color.FromArgb((CInt(InColor.A) + CInt(DarkColor.A)) / 2,
                                    (CInt(InColor.R) + CInt(DarkColor.R)) / 2,
                                    (CInt(InColor.G) + CInt(DarkColor.G)) / 2,
                                    (CInt(InColor.B) + CInt(DarkColor.B)) / 2)
        Return BlendColor
    End Function
    Private Function GetFontColor(color As Color) As Color 'get contrasting font color
        Dim d As Integer = 0
        Dim a As Double
        a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255
        If a < 0.5 Then
            d = 0
        Else
            d = 255
        End If
        Return Color.FromArgb(d, d, d)
    End Function
End Class