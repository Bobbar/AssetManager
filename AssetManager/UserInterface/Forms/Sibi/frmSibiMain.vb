Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class frmSibiMain
    Private bolGridFilling As Boolean = False
    Private MyWindowList As WindowList
    Private LastCmd As MySqlCommand
    Private bolRebuildingCombo As Boolean = False
    Public Sub RefreshResults()
        ExecuteCmd(LastCmd)
    End Sub
    Private Sub ClearAll(TopControl As Control.ControlCollection)
        For Each ctl As Control In TopControl
            If TypeOf ctl Is TextBox Then
                Dim txt As TextBox = ctl
                txt.Clear()
            ElseIf TypeOf ctl Is ComboBox Then
                Dim cmb As ComboBox = ctl
                cmb.SelectedIndex = 0
            ElseIf ctl.Controls.Count > 0 Then
                ClearAll(ctl.Controls)
            End If
        Next
    End Sub
    Private Sub cmdShowAll_Click(sender As Object, e As EventArgs) Handles cmdShowAll.Click
        ClearAll(Me.Controls)
        If SetDisplayYears() Then
            ShowAll()
        End If
    End Sub
    Private Sub frmSibiMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        ExtendedMethods.DoubleBufferedDataGrid(ResultGrid, True)
        If SetDisplayYears() Then
            ShowAll("All")
            GridTheme.BackColor = ResultGrid.DefaultCellStyle.BackColor
            GridTheme.CellSelectColor = colSibiSelectColor
            GridTheme.RowHighlightColor = colHighlightBlue
            ToolStrip1.BackColor = colSibiToolBarColor
            MyWindowList = New WindowList(Me, ToolStrip1)
        Else
            Me.Dispose()
        End If
    End Sub
    Private Function BuildSearchListNew() As List(Of SearchVal)
        Dim tmpList As New List(Of SearchVal)
        tmpList.Add(New SearchVal(sibi_requests.RT_Number, Trim(txtRTNum.Text)))
        tmpList.Add(New SearchVal(sibi_requests.Description, Trim(txtDescription.Text)))
        tmpList.Add(New SearchVal(sibi_requests.PO, txtPO.Text))
        tmpList.Add(New SearchVal(sibi_requests.RequisitionNumber, txtReq.Text))
        Return tmpList
    End Function
    Private Sub DynamicSearch() 'dynamically creates sql query using any combination of search filters the users wants
        Dim table As New DataTable
        Dim cmd As New MySqlCommand
        Dim strStartQry As String
        strStartQry = "SELECT * FROM " & sibi_requests.TableName & " WHERE"
        Dim strDynaQry As String = ""
        Dim SearchValCol As List(Of SearchVal) = BuildSearchListNew()
        For Each fld As SearchVal In SearchValCol
            If Not IsNothing(fld.Value) Then
                If fld.Value.ToString <> "" Then
                    strDynaQry = strDynaQry + " " + fld.FieldName + " LIKE CONCAT('%', @" + fld.FieldName + ", '%') AND"
                    cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                End If
            End If
        Next
        If strDynaQry = "" Then
            Exit Sub
        End If
        Dim strQry = strStartQry & strDynaQry
        If Strings.Right(strQry, 3) = "AND" Then 'remove trailing AND from dynamic query
            strQry = Strings.Left(strQry, Strings.Len(strQry) - 3)
        End If
        strQry += " ORDER BY " & sibi_requests.RequestNumber & " DESC"
        cmd.CommandText = strQry
        ExecuteCmd(cmd)
    End Sub
    Private Sub ExecuteCmd(cmd As MySqlCommand)
        Try
            LastCmd = cmd
            Using SQLComms As New clsMySQL_Comms, QryComm As MySqlCommand = cmd, ds As New DataSet, da As New MySqlDataAdapter
                QryComm.Connection = SQLComms.Connection
                da.SelectCommand = QryComm
                da.Fill(ds)
                SendToGrid(ds.Tables(0))
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            ' ConnectionReady()
        End Try
    End Sub
    Public Sub SendToGrid(Results As DataTable)
        Try
            Dim table As New DataTable
            table.Columns.Add("Request #", GetType(Integer))
            table.Columns.Add("Status", GetType(String))
            table.Columns.Add("Description", GetType(String))
            table.Columns.Add("Request User", GetType(String))
            table.Columns.Add("Request Type", GetType(String))
            table.Columns.Add("Need By", GetType(Date))
            table.Columns.Add("PO Number", GetType(String))
            table.Columns.Add("Req. Number", GetType(String))
            table.Columns.Add("RT Number", GetType(String))
            table.Columns.Add("Create Date", GetType(Date))
            table.Columns.Add("UID", GetType(String))
            For Each r As DataRow In Results.Rows
                table.Rows.Add(NoNull(r.Item(sibi_requests.RequestNumber)),
               GetHumanValue(SibiIndex.StatusType, r.Item(sibi_requests.Status).ToString),
               NoNull(r.Item(sibi_requests.Description)),
               NoNull(r.Item(sibi_requests.RequestUser)),
               GetHumanValue(SibiIndex.RequestType, r.Item(sibi_requests.Type).ToString),
               NoNull(r.Item(sibi_requests.NeedBy)),
               NoNull(r.Item(sibi_requests.PO)),
               NoNull(r.Item(sibi_requests.RequisitionNumber)),
               NoNull(r.Item(sibi_requests.RT_Number)),
               NoNull(r.Item(sibi_requests.DateStamp)),
               NoNull(r.Item(sibi_requests.UID)))
            Next
            bolGridFilling = True
            ResultGrid.DataSource = table
            ResultGrid.ClearSelection()
            bolGridFilling = False
            table.Dispose()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Function SetDisplayYears() As Boolean
        Try
            bolRebuildingCombo = True
            Dim strQRY As String = "SELECT DISTINCT YEAR(" & sibi_requests.DateStamp & ") FROM " & sibi_requests.TableName & " ORDER BY " & sibi_requests.DateStamp & " DESC"
            Using SQLComms As New clsMySQL_Comms, results As DataTable = SQLComms.Return_SQLTable(strQRY)
                '  If Not SQLComms.ConnectionReady Then Return False
                cmbDisplayYear.Items.Clear()
                cmbDisplayYear.Items.Add("All")
                For Each r As DataRow In results.Rows
                    cmbDisplayYear.Items.Add(r.Item("YEAR(" & sibi_requests.DateStamp & ")"))
                Next
                cmbDisplayYear.SelectedIndex = 0
                bolRebuildingCombo = False
                Return True
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function
    Private Sub ShowAll(Optional Year As String = "")
        Using SQLComms As New clsMySQL_Comms
            If Year = "" Then Year = cmbDisplayYear.Text
            If Year = "All" Then
                ExecuteCmd(SQLComms.Return_SQLCommand("SELECT * FROM " & sibi_requests.TableName & " ORDER BY " & sibi_requests.RequestNumber & " DESC"))
            Else
                ExecuteCmd(SQLComms.Return_SQLCommand("SELECT * FROM " & sibi_requests.TableName & " WHERE YEAR(" & sibi_requests.DateStamp & ") = " & Year & " ORDER BY " & sibi_requests.RequestNumber & " DESC"))
            End If
        End Using
    End Sub
    Private Sub cmdManage_Click(sender As Object, e As EventArgs) Handles cmdManage.Click
        Dim NewRequest As New frmManageRequest(Me)
    End Sub
    Private Sub ResultGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellDoubleClick
        OpenRequest(ResultGrid.Item(GetColIndex(ResultGrid, "UID"), ResultGrid.CurrentRow.Index).Value.ToString)
    End Sub
    Private Sub OpenRequest(strUID As String)
        If Not RequestIsOpen(strUID) Then
            Dim ManRequest As New frmManageRequest(Me, strUID)
        Else
            ActivateFormByUID(strUID)
        End If
    End Sub
    Private Sub ResultGrid_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles ResultGrid.RowPostPaint
        If e.RowIndex > -1 Then
            Dim dvgCell As DataGridViewCell = ResultGrid.Rows(e.RowIndex).Cells("Status")
            Dim dvgRow As DataGridViewRow = ResultGrid.Rows(e.RowIndex)
            Dim BackCol, ForeCol As Color
            BackCol = GetRowColor(dvgRow.Cells("Status").Value.ToString)
            ForeCol = GetFontColor(BackCol)
            dvgCell.Style.BackColor = BackCol
            dvgCell.Style.ForeColor = ForeCol
            dvgCell.Style.SelectionBackColor = ColorAlphaBlend(BackCol, Color.FromArgb(87, 87, 87))
        End If
    End Sub
    Private Function GetRowColor(Value As String) As Color
        Dim DBVal As String = GetDBValueFromHuman(SibiIndex.StatusType, Value)
        Dim DarkColor As Color = Color.FromArgb(222, 222, 222) 'gray color
        Select Case DBVal
            Case "NEW"
                Return ColorAlphaBlend(Color.FromArgb(0, 255, 30), DarkColor)
            Case "QTN"
                Return ColorAlphaBlend(Color.FromArgb(242, 255, 0), DarkColor)
            Case "QTR"
                Return ColorAlphaBlend(Color.FromArgb(255, 208, 0), DarkColor)
            Case "QRC"
                Return ColorAlphaBlend(Color.FromArgb(255, 162, 0), DarkColor)
            Case "RQN"
                Return ColorAlphaBlend(Color.FromArgb(0, 255, 251), DarkColor)
            Case "RQR"
                Return ColorAlphaBlend(Color.FromArgb(0, 140, 255), DarkColor)
            Case "POS"
                Return ColorAlphaBlend(Color.FromArgb(197, 105, 255), DarkColor)
            Case "SHD"
                Return ColorAlphaBlend(Color.FromArgb(255, 79, 243), DarkColor)
            Case "ORC"
                Return ColorAlphaBlend(Color.FromArgb(79, 144, 255), DarkColor)
            Case "NPAY"
                Return ColorAlphaBlend(Color.FromArgb(255, 36, 36), DarkColor)
            Case "RCOMP"
                Return ColorAlphaBlend(Color.FromArgb(158, 158, 158), DarkColor)
            Case "ONH"
                Return ColorAlphaBlend(Color.FromArgb(255, 255, 255), DarkColor)
        End Select
    End Function
    Private Function ColorAlphaBlend(InColor As Color, BlendColor As Color) As Color 'blend colors with darker color so they aren't so intense
        Dim OutColor As Color
        OutColor = Color.FromArgb((CInt(InColor.A) + CInt(BlendColor.A)) / 2,
                                    (CInt(InColor.R) + CInt(BlendColor.R)) / 2,
                                    (CInt(InColor.G) + CInt(BlendColor.G)) / 2,
                                    (CInt(InColor.B) + CInt(BlendColor.B)) / 2)
        Return OutColor
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
    Private Sub HighlightCurrentRow(Row As Integer)
        Try
            If Not bolGridFilling Then
                HighlightRow(ResultGrid, GridTheme, Row)
            End If
        Catch
        End Try
    End Sub
    Private Sub ResultGrid_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellEnter
        HighlightCurrentRow(e.RowIndex)
    End Sub
    Private Sub ResultGrid_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellLeave
        LeaveRow(ResultGrid, GridTheme, e.RowIndex)
    End Sub
    Private Sub cmbDisplayYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDisplayYear.SelectedIndexChanged
        If cmbDisplayYear.Text IsNot Nothing And Not bolRebuildingCombo Then
            ShowAll(cmbDisplayYear.Text)
        End If
    End Sub
    Private Sub frmSibiMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Dim CancelClose As Boolean = CheckForActiveTransfers()
        If CancelClose Then
            e.Cancel = True
        Else
            CloseChildren(Me)
        End If
    End Sub
    Private Sub frmSibiMain_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        CloseChildren(Me)
    End Sub
    Private Sub txtPO_TextChanged(sender As Object, e As EventArgs) Handles txtPO.TextChanged
        DynamicSearch()
    End Sub
    Private Sub txtReq_TextChanged(sender As Object, e As EventArgs) Handles txtReq.TextChanged
        DynamicSearch()
    End Sub
    Private Sub txtDescription_TextChanged(sender As Object, e As EventArgs) Handles txtDescription.TextChanged
        DynamicSearch()
    End Sub
    Private Sub txtRTNum_TextChanged(sender As Object, e As EventArgs) Handles txtRTNum.TextChanged
        DynamicSearch()
    End Sub
    Private Sub Waiting()
        SetCursor(Cursors.WaitCursor)
    End Sub
    Private Sub DoneWaiting()
        SetCursor(Cursors.Default)
    End Sub
End Class