Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Module LiveBox
    'Private LiveWorker As BackgroundWorker = New BackgroundWorker
    Private WithEvents LiveWorker As BackgroundWorker
    Private LiveBox As ListBox = New ListBox
    Private ActiveControl As Control
    Private strPrevSearchString As String
    Private dtLiveBoxData As DataTable
    Private CurrentLiveBoxType As LiveBoxType
    Public Class LiveBoxType
        Public Const DynamicSearch As String = "DYNA"
        Public Const InstaLoad As String = "INSTA"
        Public Const SelectValue As String = "SELE"
    End Class
    Public Sub InitializeLiveBox()
        LiveWorker = New BackgroundWorker
        AddHandler LiveWorker.DoWork, AddressOf LiveWorker_DoWork
        AddHandler LiveWorker.RunWorkerCompleted, AddressOf LiveWorkerr_RunWorkerCompleted
        AddHandler LiveBox.MouseClick, AddressOf LiveBox_MouseClick
        With LiveWorker
            .WorkerReportsProgress = True
            .WorkerSupportsCancellation = True
        End With
    End Sub
    Private Sub LiveBox_MouseClick(sender As Object, e As MouseEventArgs)
        LiveBoxSelect(ActiveControl, CurrentLiveBoxType)
    End Sub
    Private Sub LiveBoxSelect(Control As Control, Type As LiveBoxType)
        Select Case Type.ToString
            Case LiveBoxType.DynamicSearch
                Control.Text = LiveBox.Text
                MainForm.DynamicSearch()
            Case LiveBoxType.InstaLoad
                MainForm.LoadDevice(dtLiveBoxData.Rows(LiveBox.SelectedIndex).Item("dev_UID"))
            Case LiveBoxType.SelectValue
                Control.Text = LiveBox.Text
                'HideLiveBox()



                'Case "txtDescription"
                '    Control.Text = LiveBox.Text
                '    MainForm.DynamicSearch()
                'Case "txtCurUser"
                '    Control.Text = LiveBox.Text
                '    MainForm.DynamicSearch()
                'Case "txtReplaceYear"
                '    Control.Text = LiveBox.Text
                '    MainForm.DynamicSearch()
                'Case Else
                '    MainForm.LoadDevice(dtLiveBoxData.Rows(LiveBox.SelectedIndex).Item("dev_UID"))
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
            Select Case ActiveControl.Name
                Case "txtAssetTag"
                    strQryRow = "dev_asset_tag, dev_UID"
                Case "txtSerial"
                    strQryRow = "dev_serial, dev_UID"
                Case "txtCurUser"
                    strQryRow = "dev_cur_user"
                Case "txtDescription"
                    strQryRow = "dev_description"
                Case "txtReplaceYear"
                    strQryRow = "dev_replacement_year"
                Case "txtCurUser_View_REQ"
                    strQryRow = "dev_cur_user"
            End Select
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
            Select Case TypeName(ActiveControl.Parent)
                Case "GroupBox"
                    Dim CntGroup As GroupBox
                    CntGroup = ActiveControl.Parent
                Case "Panel"
                    Dim CntGroup As Panel
                    CntGroup = ActiveControl.Parent
            End Select
            If Not PositionOnly Then
                If dtResults.Rows.Count < 1 Then
                    LiveBox.Visible = False
                    Exit Sub
                End If
                Select Case ActiveControl.Name
                    Case "txtAssetTag"
                        strQryRow = "dev_asset_tag"
                    Case "txtSerial"
                        strQryRow = "dev_serial"
                    Case "txtCurUser"
                        strQryRow = "dev_cur_user"
                    Case "txtDescription"
                        strQryRow = "dev_description"
                    Case "txtReplaceYear"
                        strQryRow = "dev_replacement_year"
                    Case "txtCurUser_View_REQ"
                        strQryRow = "dev_cur_user"
                End Select
                LiveBox.Items.Clear()
                With dr
                    For Each dr In dtResults.Rows
                        LiveBox.Items.Add(dr.Item(strQryRow))
                    Next
                End With
            End If
            Dim ParentForm As Form = ActiveControl.FindForm
            LiveBox.Parent = ParentForm 'ActiveControl.Parent '
            LiveBox.BringToFront()
            SetStyle()
            Dim ScreenPos As Point = ParentForm.PointToClient(ActiveControl.Parent.PointToScreen(ActiveControl.Location))
            ScreenPos.Y = ScreenPos.Y + ActiveControl.Height
            LiveBox.Location = ScreenPos

            'LiveBox.Left = ActiveControl.Left
            'LiveBox.Top = ActiveControl.Top + ActiveControl.Height
            LiveBox.Width = ActiveControl.Width
            LiveBox.Height = LiveBox.PreferredHeight
            If dtResults.Rows.Count > 0 Then
                LiveBox.Visible = True
            Else
                LiveBox.Visible = False
            End If
            If strPrevSearchString <> ActiveControl.Text Then
                'strSearchString = ActiveControl.Text
                StartLiveSearch(ActiveControl) 'if search string has changed since last completetion, run again.
            End If
            dtLiveBoxData = dtResults
            Exit Sub
        Catch
            LiveBox.Visible = False
            LiveBox.Items.Clear()
        End Try
    End Sub
    Private Sub GiveLiveBoxFocus()
        LiveBox.Focus()
        If LiveBox.SelectedIndex = -1 Then
            LiveBox.SelectedIndex = 0
        End If
    End Sub
    Public Sub StartLiveSearch(Control As Control, ByVal Type As LiveBoxType)
        'StartingControl = ActiveControl
        ActiveControl = Control
        CurrentLiveBoxType = Type
        Dim strSearchString As String = ActiveControl.Text
        If Trim(strSearchString) <> "" Then
            If Not LiveWorker.IsBusy And ConnectionReady() Then
                LiveWorker.RunWorkerAsync(strSearchString)
            End If
        Else
            HideLiveBox()
        End If
    End Sub
    Private Sub HideLiveBox()
        Try
            LiveBox.Visible = False
            LiveBox.Items.Clear()
            ' If ActiveControl.Parent.Name = "InstantGroup" Then
            'ActiveControl.Text = ""
            '  End If
        Catch
        End Try
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
