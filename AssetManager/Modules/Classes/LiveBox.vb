Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class LiveBox
    Private RowLimit As Integer = 15
    Private WithEvents LiveWorker As BackgroundWorker
    Private LiveBox As ListBox
    Private strPrevSearchString As String
    Private dtLiveBoxData As DataTable
    Private MySQLComms As New MySQL_Comms
    Private LiveConn As New MySqlConnection
    Private Structure LiveBoxArgs
        Public Control As Control
        Public Type As String
        Public DBColumn As String
    End Structure
    Private CurrentLiveBoxArgs As LiveBoxArgs
    Public Class LiveBoxType
        Public Const DynamicSearch As String = "DYNA"
        Public Const InstaLoad As String = "INSTA"
        Public Const SelectValue As String = "SELE"
    End Class
    Public Sub InitializeLiveBox()
        If OpenConnection() Then
            InitializeWorker()
            InitializeControl()
        End If
    End Sub
    Private Sub InitializeWorker()
        LiveWorker = New BackgroundWorker
        AddHandler LiveWorker.DoWork, AddressOf LiveWorker_DoWork
        AddHandler LiveWorker.RunWorkerCompleted, AddressOf LiveWorkerr_RunWorkerCompleted
        With LiveWorker
            .WorkerReportsProgress = False
            .WorkerSupportsCancellation = False
        End With
    End Sub
    Private Sub InitializeControl()
        LiveBox = New ListBox
        'AddHandler LiveBox.MouseClick, AddressOf LiveBox_MouseClick
        AddHandler LiveBox.MouseDown, AddressOf LiveBox_MouseDown
        AddHandler LiveBox.MouseMove, AddressOf LiveBox_MouseMove
        AddHandler LiveBox.KeyDown, AddressOf LiveBox_KeyDown
        ExtendedMethods.DoubleBufferedListBox(LiveBox, True)
        LiveBox.Visible = False
        SetStyle()
    End Sub
    Private Function OpenConnection() As Boolean
        Try
            If LiveConn.State = ConnectionState.Open Then
                Return True
            Else
                LiveConn = MySQLComms.NewConnection
                LiveConn.Open()
                Return True
            End If
        Catch ex As Exception
            Return False
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Function
    Public Sub Unload()
        Try
            HideLiveBox()
            MySQLDB.CloseConnection(LiveConn)
            LiveBox.Dispose()
        Catch
        End Try
    End Sub
    Private Sub LiveBoxSelect(Control As Control, Type As String)
        Select Case Type.ToString
            Case LiveBoxType.DynamicSearch
                Control.Text = LiveBox.Text
                MainForm.DynamicSearch()
            Case LiveBoxType.InstaLoad
                MainForm.LoadDevice(dtLiveBoxData.Rows(LiveBox.SelectedIndex).Item("dev_UID"))
                CurrentLiveBoxArgs.Control.Text = ""
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
            Dim strQryRow As String
            Dim strQry As String
            strQryRow = CurrentLiveBoxArgs.DBColumn
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
    Private Sub DrawLiveBox(dtResults As DataTable)
        Try
            Dim dr As DataRow
            Dim strQryRow As String
            Select Case TypeName(CurrentLiveBoxArgs.Control.Parent)
                Case "GroupBox"
                    Dim CntGroup As GroupBox
                    CntGroup = CurrentLiveBoxArgs.Control.Parent
                Case "Panel"
                    Dim CntGroup As Panel
                    CntGroup = CurrentLiveBoxArgs.Control.Parent
            End Select
            If dtResults.Rows.Count < 1 Then
                LiveBox.Visible = False
                Exit Sub
            End If
            strQryRow = CurrentLiveBoxArgs.DBColumn
            LiveBox.Items.Clear()
            With dr
                For Each dr In dtResults.Rows
                    LiveBox.Items.Add(dr.Item(strQryRow))
                Next
            End With
            PosistionLiveBox()
            If dtResults.Rows.Count > 0 Then
                LiveBox.Visible = True
            Else
                LiveBox.Visible = False
            End If
            If strPrevSearchString <> CurrentLiveBoxArgs.Control.Text Then
                StartLiveSearch(CurrentLiveBoxArgs.Control, CurrentLiveBoxArgs.Type, CurrentLiveBoxArgs.DBColumn) 'if search string has changed since last completetion, run again.
            End If
            dtLiveBoxData = dtResults
            Exit Sub
        Catch
            LiveBox.Visible = False
            LiveBox.Items.Clear()
        End Try
    End Sub
    Private Sub PosistionLiveBox()
        LiveBox.Visible = False
        Dim ParentForm As Form = CurrentLiveBoxArgs.Control.FindForm
        LiveBox.Parent = ParentForm
        LiveBox.BringToFront()
        Dim ScreenPos As Point = ParentForm.PointToClient(CurrentLiveBoxArgs.Control.Parent.PointToScreen(CurrentLiveBoxArgs.Control.Location))
        ScreenPos.Y = ScreenPos.Y + CurrentLiveBoxArgs.Control.Height
        LiveBox.Location = ScreenPos
        LiveBox.Width = CurrentLiveBoxArgs.Control.Width
        Dim FormBounds As Rectangle = ParentForm.ClientRectangle
        If LiveBox.PreferredHeight + LiveBox.Top > FormBounds.Bottom Then
            LiveBox.Height = FormBounds.Bottom - LiveBox.Top
        Else
            LiveBox.Height = LiveBox.PreferredHeight
        End If
    End Sub
    Public Sub GiveLiveBoxFocus()
        LiveBox.Focus()
        If LiveBox.SelectedIndex = -1 Then
            LiveBox.SelectedIndex = 0
        End If
    End Sub
    Public Sub StartLiveSearch(Control As Control, Type As String, DBColumn As String)
        CollectLiveBoxArgs(Control, Type, DBColumn)
        If LiveBox.IsDisposed Then InitializeControl()
        Dim strSearchString As String = CurrentLiveBoxArgs.Control.Text
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
    Private Sub CollectLiveBoxArgs(Control As Control, Type As String, DBColumn As String)
        With CurrentLiveBoxArgs
            .Control = Control
            .Type = Type
            .DBColumn = DBColumn
        End With
    End Sub
    Private Sub LiveBox_MouseMove(sender As Object, e As MouseEventArgs)
        LiveBox.SelectedIndex = LiveBox.IndexFromPoint(e.Location)
    End Sub
    Private Sub LiveBox_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then LiveBoxSelect(CurrentLiveBoxArgs.Control, CurrentLiveBoxArgs.Type)
    End Sub
    Private Sub LiveBox_MouseDown(sender As Object, e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then LiveBoxSelect(CurrentLiveBoxArgs.Control, CurrentLiveBoxArgs.Type)
        If e.Button = MouseButtons.Right Then HideLiveBox()
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
End Class
