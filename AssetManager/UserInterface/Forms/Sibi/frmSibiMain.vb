Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class frmSibiMain
    Private bolGridFilling As Boolean = False
    Private MyWindowList As WindowList
    Private LastCmd As MySqlCommand

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
        SetDisplayYears()
        ShowAll()
    End Sub
    Private Sub frmSibiMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        ExtendedMethods.DoubleBuffered(ResultGrid, True)
        SetDisplayYears()
        ShowAll("All")
        MyWindowList = New WindowList(Me, tsdSelectWindow)
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
            Dim LocalSQLComm As New clsMySQL_Comms
            Dim QryComm As MySqlCommand = cmd
            Dim ds As New DataSet
            Dim da As New MySqlDataAdapter
            QryComm.Connection = GlobalConn
            da.SelectCommand = QryComm
            da.Fill(ds)
            da.Dispose()
            SendToGrid(ds.Tables(0))
            ds.Dispose()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            ConnectionReady()
        End Try
    End Sub
    Public Sub SendToGrid(Results As DataTable)
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
            table.Columns.Add("RT Number", GetType(String))
            table.Columns.Add("Create Date", GetType(String))
            table.Columns.Add("UID", GetType(String))
            For Each r As DataRow In Results.Rows
                table.Rows.Add(NoNull(r.Item(sibi_requests.RequestNumber)),
               GetHumanValue(SibiIndex.StatusType, r.Item(sibi_requests.Status)),
               NoNull(r.Item(sibi_requests.Description)),
               NoNull(r.Item(sibi_requests.RequestUser)),
               GetHumanValue(SibiIndex.RequestType, r.Item(sibi_requests.Type)),
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
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub SetDisplayYears()
        Dim strQRY As String = "SELECT DISTINCT YEAR(" & sibi_requests.DateStamp & ") FROM " & sibi_requests.TableName & " ORDER BY " & sibi_requests.DateStamp & " DESC"
        Dim results As DataTable = SQLComms.Return_SQLTable(strQRY)
        cmbDisplayYear.Items.Clear()
        cmbDisplayYear.Items.Add("All")
        For Each r As DataRow In results.Rows
            cmbDisplayYear.Items.Add(r.Item("YEAR(" & sibi_requests.DateStamp & ")"))
        Next
        cmbDisplayYear.SelectedIndex = 0
    End Sub
    Private Sub ShowAll(Optional Year As String = "")
        If Year = "" Then Year = cmbDisplayYear.Text
        If Year = "All" Then
            ExecuteCmd(SQLComms.Return_SQLCommand("SELECT * FROM " & sibi_requests.TableName & " ORDER BY " & sibi_requests.RequestNumber & " DESC"))
        Else
            ExecuteCmd(SQLComms.Return_SQLCommand("SELECT * FROM " & sibi_requests.TableName & " WHERE YEAR(" & sibi_requests.DateStamp & ") = " & Year & " ORDER BY " & sibi_requests.RequestNumber & " DESC"))
        End If
    End Sub
    Private Sub cmdManage_Click(sender As Object, e As EventArgs) Handles cmdManage.Click
        Dim NewRequest As New frmManageRequest(Me)
    End Sub
    Private Sub ResultGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellDoubleClick
        OpenRequest(ResultGrid.Item(GetColIndex(ResultGrid, "UID"), ResultGrid.CurrentRow.Index).Value)
    End Sub
    Private Sub OpenRequest(strUID As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
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
        On Error Resume Next
        If Not bolGridFilling Then
            Dim BackColor As Color = DefGridBC
            Dim SelectColor As Color = colSibiSelectColor 'DefGridSelCol
            Dim c1 As Color = colHighlightBlue 'colHighlightColor 'highlight color
            If Row > -1 Then
                For Each cell As DataGridViewCell In ResultGrid.Rows(Row).Cells
                    If cell.ColumnIndex <> GetColIndex(ResultGrid, "Status") Then 'skip the colored status column
                        Dim c2 As Color = Color.FromArgb(SelectColor.R, SelectColor.G, SelectColor.B)
                        Dim BlendColor As Color
                        BlendColor = Color.FromArgb((CInt(c1.A) + CInt(c2.A)) / 2,
                                                (CInt(c1.R) + CInt(c2.R)) / 2,
                                                (CInt(c1.G) + CInt(c2.G)) / 2,
                                                (CInt(c1.B) + CInt(c2.B)) / 2)
                        cell.Style.SelectionBackColor = BlendColor
                        c2 = Color.FromArgb(BackColor.R, BackColor.G, BackColor.B)
                        BlendColor = Color.FromArgb((CInt(c1.A) + CInt(c2.A)) / 2,
                                                (CInt(c1.R) + CInt(c2.R)) / 2,
                                                (CInt(c1.G) + CInt(c2.G)) / 2,
                                                (CInt(c1.B) + CInt(c2.B)) / 2)
                        cell.Style.BackColor = BlendColor
                    End If

                Next
            End If
        End If
    End Sub
    Private Sub ResultGrid_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellEnter
        HighlightCurrentRow(e.RowIndex)
    End Sub
    Private Sub ResultGrid_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellLeave
        Dim BackColor As Color = DefGridBC
        Dim SelectColor As Color = colSibiSelectColor 'DefGridSelCol
        If e.RowIndex > -1 Then
            For Each cell As DataGridViewCell In ResultGrid.Rows(e.RowIndex).Cells
                If cell.ColumnIndex <> GetColIndex(ResultGrid, "Status") Then 'skip the colored status column
                    cell.Style.SelectionBackColor = SelectColor
                    cell.Style.BackColor = BackColor
                End If
            Next
        End If
    End Sub
    Private Sub cmbDisplayYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDisplayYear.SelectedIndexChanged
        If Not IsNothing(cmbDisplayYear.Text) Then
            ShowAll(cmbDisplayYear.Text)
        End If
    End Sub
    Private Sub frmSibiMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        CloseChildren(Me)
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
End Class