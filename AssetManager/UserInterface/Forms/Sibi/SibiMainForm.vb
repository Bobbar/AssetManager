Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Imports System.Data.Common
Public Class SibiMainForm
    Private bolGridFilling As Boolean = False
    Private MyWindowList As New WindowList(Me)
    Private LastCmd As DbCommand
    Private bolRebuildingCombo As Boolean = False
    Public Sub RefreshResults()
        ExecuteCmd(LastCmd)
    End Sub
    Private Sub ClearAll(TopControl As Control.ControlCollection)
        For Each ctl As Control In TopControl
            If TypeOf ctl Is TextBox Then
                Dim txt As TextBox = DirectCast(ctl, TextBox)
                txt.Clear()
            ElseIf TypeOf ctl Is ComboBox Then
                Dim cmb As ComboBox = DirectCast(ctl, ComboBox)
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
            MyWindowList.InsertWindowList(ToolStrip1)
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
        Dim cmd = DBFunc.GetCommand()
        Dim strStartQry As String
        strStartQry = "SELECT * FROM " & sibi_requests.TableName & " WHERE"
        Dim strDynaQry As String = ""
        Dim SearchValCol As List(Of SearchVal) = BuildSearchListNew()
        For Each fld As SearchVal In SearchValCol
            If Not IsNothing(fld.Value) Then
                If fld.Value.ToString <> "" Then
                    strDynaQry = strDynaQry + " " + fld.FieldName + " LIKE @" + fld.FieldName + " AND"
                    Dim Value As String = "%" & fld.Value.ToString & "%"
                    cmd.AddParameterWithValue("@" & fld.FieldName, Value)
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
    Private Sub ExecuteCmd(ByRef cmd As DbCommand)
        Try
            LastCmd = cmd
            SendToGrid(DBFunc.DataTableFromCommand(cmd))
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
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
            Dim strQRY As String = "SELECT DISTINCT " & sibi_requests.DateStamp & " FROM " & sibi_requests.TableName & " ORDER BY " & sibi_requests.DateStamp & " DESC"
            Using results As DataTable = DBFunc.DataTableFromQueryString(strQRY)
                Dim Years As New List(Of String)
                Years.Add("All")
                For Each r As DataRow In results.Rows
                    Dim yr = YearFromDate(DateTime.Parse(r.Item(sibi_requests.DateStamp).ToString))
                    If Not Years.Contains(yr) Then
                        Years.Add(yr)
                    End If
                Next
                cmbDisplayYear.DataSource = Years
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
        If Year = "" Then Year = cmbDisplayYear.Text
        If Year = "All" Then
            ExecuteCmd(DBFunc.GetCommand("SELECT * FROM " & sibi_requests.TableName & " ORDER BY " & sibi_requests.RequestNumber & " DESC"))
        Else
            ExecuteCmd(DBFunc.GetCommand("SELECT * FROM " & sibi_requests.TableName & " WHERE " & sibi_requests.DateStamp & " LIKE '%" & Year & "%' ORDER BY " & sibi_requests.RequestNumber & " DESC"))
        End If
    End Sub
    Private Sub cmdManage_Click(sender As Object, e As EventArgs) Handles cmdManage.Click
        Dim NewRequest As New SibiManageRequestForm(Me)
    End Sub
    Private Sub ResultGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellDoubleClick
        OpenRequest(ResultGrid.Item(GetColIndex(ResultGrid, "UID"), ResultGrid.CurrentRow.Index).Value.ToString)
    End Sub
    Private Sub OpenRequest(strUID As String)
        If Not RequestIsOpen(strUID) Then
            Dim ManRequest As New SibiManageRequestForm(Me, strUID)
        Else
            ActivateFormByUID(strUID, Me)
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
    Private Function GetRowColor(Value As String) As Color 'TODO: Change this to stop referencing a "Human" DisplayValue
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
        OutColor = Color.FromArgb(CInt((CInt(InColor.A) + CInt(BlendColor.A)) / 2),
                                    CInt((CInt(InColor.R) + CInt(BlendColor.R)) / 2),
                                    CInt((CInt(InColor.G) + CInt(BlendColor.G)) / 2),
                                    CInt((CInt(InColor.B) + CInt(BlendColor.B)) / 2))
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
            LastCmd.Dispose()
            MyWindowList.Dispose()
            CloseChildren(Me)
        End If
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
        SetWaitCursor(True)
    End Sub
    Private Sub DoneWaiting()
        SetWaitCursor(False)
    End Sub
End Class