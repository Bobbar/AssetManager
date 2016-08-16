Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Module LiveBox
    Private WithEvents LiveWorker As BackgroundWorker
    Private LiveBox As ListBox ' = New ListBox
    Private CurrentControl As Control
    Private strPrevSearchString As String
    Private dtLiveBoxData As DataTable
    Private CurrentLiveBoxType As String
    Public Class LiveBoxType
        Public Const DynamicSearch As String = "DYNA"
        Public Const InstaLoad As String = "INSTA"
        Public Const SelectValue As String = "SELE"
    End Class
    Public Sub InitializeLiveBox()
        InitializeWorker()
        InitializeControl()
    End Sub
    Private Sub InitializeWorker()
        LiveWorker = New BackgroundWorker
        AddHandler LiveWorker.DoWork, AddressOf LiveWorker_DoWork
        AddHandler LiveWorker.RunWorkerCompleted, AddressOf LiveWorkerr_RunWorkerCompleted
        With LiveWorker
            .WorkerReportsProgress = True
            .WorkerSupportsCancellation = True
        End With
    End Sub
    Private Sub InitializeControl()
        LiveBox = New ListBox
        AddHandler LiveBox.MouseClick, AddressOf LiveBox_MouseClick
        AddHandler LiveBox.MouseMove, AddressOf LiveBox_MouseMove
        AddHandler LiveBox.KeyDown, AddressOf LiveBox_KeyDown
        ExtendedMethods.DoubleBufferedListBox(LiveBox, True)
        LiveBox.Visible = False
    End Sub
    Private Sub LiveBoxSelect(Control As Control, Type As String)
        Select Case Type.ToString
            Case LiveBoxType.DynamicSearch
                Control.Text = LiveBox.Text
                MainForm.DynamicSearch()
            Case LiveBoxType.InstaLoad
                MainForm.LoadDevice(dtLiveBoxData.Rows(LiveBox.SelectedIndex).Item("dev_UID"))
                CurrentControl.Text = ""
            Case LiveBoxType.SelectValue
                Control.Text = LiveBox.Text
        End Select
        HideLiveBox()
    End Sub
    Private Sub LiveWorker_DoWork(sender As Object, e As DoWorkEventArgs)
        Try
            Dim strSearchString As String = DirectCast(e.Argument, String)
            strPrevSearchString = strSearchString
            Dim ds As New DataSet
            Dim da As New MySqlDataAdapter
            Dim cmd As New MySqlCommand
            Dim RowLimit As Integer = 15
            Dim strQryRow As String
            Dim strQry As String
            strQryRow = GetDBColumn(CurrentControl.Name)
            strQry = "SELECT dev_UID," & strQryRow & " FROM devices WHERE " & strQryRow & " LIKE CONCAT('%', @Search_Value, '%') GROUP BY " & strQryRow & " ORDER BY " & strQryRow & " LIMIT " & RowLimit
            cmd.Connection = LiveConn
            cmd.CommandText = strQry
            cmd.Parameters.AddWithValue("@Search_Value", strSearchString)
            da.SelectCommand = cmd
            da.Fill(ds)
            e.Result = ds.Tables(0)
            da.Dispose()
            ds.Dispose()
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            ConnectionReady()
        End Try
    End Sub
    Private Sub LiveWorkerr_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        DrawLiveBox(e.Result)
    End Sub
    Private Sub DrawLiveBox(dtResults As DataTable, Optional PositionOnly As Boolean = False)
        Try
            Dim dr As DataRow
            Dim strQryRow As String
            Select Case TypeName(CurrentControl.Parent)
                Case "GroupBox"
                    Dim CntGroup As GroupBox
                    CntGroup = CurrentControl.Parent
                Case "Panel"
                    Dim CntGroup As Panel
                    CntGroup = CurrentControl.Parent
            End Select
            If Not PositionOnly Then
                If dtResults.Rows.Count < 1 Then
                    LiveBox.Visible = False
                    Exit Sub
                End If
                strQryRow = GetDBColumn(CurrentControl.Name)
                LiveBox.Items.Clear()
                With dr
                    For Each dr In dtResults.Rows
                        LiveBox.Items.Add(dr.Item(strQryRow))
                    Next
                End With
            End If
            Dim ParentForm As Form = CurrentControl.FindForm
            LiveBox.Parent = ParentForm
            LiveBox.BringToFront()
            SetStyle()
            Dim ScreenPos As Point = ParentForm.PointToClient(CurrentControl.Parent.PointToScreen(CurrentControl.Location))
            ScreenPos.Y = ScreenPos.Y + CurrentControl.Height
            LiveBox.Location = ScreenPos
            LiveBox.Width = CurrentControl.Width
            LiveBox.Height = LiveBox.PreferredHeight
            If dtResults.Rows.Count > 0 Then
                LiveBox.Visible = True
            Else
                LiveBox.Visible = False
            End If
            If strPrevSearchString <> CurrentControl.Text Then
                StartLiveSearch(CurrentControl, CurrentLiveBoxType) 'if search string has changed since last completetion, run again.
            End If
            dtLiveBoxData = dtResults
            Exit Sub
        Catch
            LiveBox.Visible = False
            LiveBox.Items.Clear()
        End Try
    End Sub
    Private Function GetDBColumn(strControlName As String) As String
        Select Case strControlName
            Case "txtAssetTag"
                Return "dev_asset_tag"
            Case "txtSerial"
                Return "dev_serial"
            Case "txtCurUser"
                Return "dev_cur_user"
            Case "txtDescription"
                Return "dev_description"
            Case "txtReplaceYear"
                Return "dev_replacement_year"
            Case "txtCurUser_View_REQ"
                Return "dev_cur_user"
        End Select
        Return "NOT_FOUND"
    End Function
    Public Sub GiveLiveBoxFocus()
        LiveBox.Focus()
        If LiveBox.SelectedIndex = -1 Then
            LiveBox.SelectedIndex = 0
        End If
    End Sub
    Public Sub StartLiveSearch(Control As Control, Type As String)
        CurrentControl = Control
        CurrentLiveBoxType = Type
        If LiveBox.IsDisposed Then InitializeControl()
        Dim strSearchString As String = CurrentControl.Text
        If Trim(strSearchString) <> "" Then
            If Not LiveWorker.IsBusy And ConnectionReady() Then
                LiveWorker.RunWorkerAsync(strSearchString)
            End If
        Else
            HideLiveBox()
        End If
    End Sub
    Public Sub HideLiveBox()
        Try
            LiveBox.Visible = False
            LiveBox.Items.Clear()
        Catch
        End Try
    End Sub
    Private Sub LiveBox_MouseMove(sender As Object, e As MouseEventArgs)
        LiveBox.SelectedIndex = LiveBox.IndexFromPoint(e.Location)
    End Sub
    Private Sub LiveBox_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then LiveBoxSelect(CurrentControl, CurrentLiveBoxType)
    End Sub
    Private Sub LiveBox_MouseClick(sender As Object, e As MouseEventArgs)
        LiveBoxSelect(CurrentControl, CurrentLiveBoxType)
    End Sub
    Private Sub SetStyle()
        Dim LiveBoxFont As Font = New Font("Consolas", 11.25, FontStyle.Bold)
        With LiveBox
            .BackColor = Color.FromArgb(255, 208, 99)
            .BorderStyle = BorderStyle.FixedSingle
            .Font = LiveBoxFont
            .ForeColor = Color.Black
        End With
    End Sub
End Module
