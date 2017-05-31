Option Explicit On
Imports System.Environment
Imports System.IO
Imports System.Runtime.InteropServices
Imports MyDialogLib
Module OtherFunctions
    Public GridStyles As System.Windows.Forms.DataGridViewCellStyle ' = New System.Windows.Forms.DataGridViewCellStyle()
    Public GridFont As Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Public stpw As New Stopwatch
    Public ProgramEnding As Boolean = False
    Private Const MAX_PATH As Int32 = 260
    Private Const SHGFI_ICON As Int32 = &H100
    Private Const SHGFI_USEFILEATTRIBUTES As Int32 = &H10
    Private Const FILE_ATTRIBUTE_NORMAL As Int32 = &H80
    Public Function SelectedCellValue(ByRef GridRow As DataGridViewRow, Optional Column As String = Nothing) As String
        For Each cell As DataGridViewCell In GridRow.Cells
            If Column = "" Then
                If cell.Selected Then Return cell.Value.ToString
            Else
                If cell.OwningColumn.Name = Column Then Return cell.Value.ToString
            End If
        Next
        Return Nothing
    End Function
    Public Sub SetGridStyle(Grid As DataGridView)
        Grid.BackgroundColor = DefGridBC
        Grid.DefaultCellStyle = GridStyles
        Grid.DefaultCellStyle.Font = GridFont
    End Sub
    Private Structure SHFILEINFO
        Public hIcon As IntPtr
        Public iIcon As Int32
        Public dwAttributes As Int32
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=MAX_PATH)>
        Public szDisplayName As String
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)>
        Public szTypeName As String
    End Structure
    Private Enum IconSize
        SHGFI_LARGEICON = 0
        SHGFI_SMALLICON = 1
    End Enum
    Private Declare Ansi Function SHGetFileInfo Lib "shell32.dll" (
                ByVal pszPath As String,
                ByVal dwFileAttributes As Int32,
                ByRef psfi As SHFILEINFO,
                ByVal cbFileInfo As Int32,
                ByVal uFlags As Int32) As IntPtr
    <DllImport("user32.dll", SetLastError:=True)>
    Private Function DestroyIcon(ByVal hIcon As IntPtr) As Boolean
    End Function
    Public Function GetFileIcon(ByVal fileExt As String) As Bitmap ', Optional ByVal ICOsize As IconSize = IconSize.SHGFI_SMALLICON
        Try
            Dim ICOSize As IconSize = IconSize.SHGFI_SMALLICON
            Dim shinfo As New SHFILEINFO
            shinfo.szDisplayName = New String(Chr(0), MAX_PATH)
            shinfo.szTypeName = New String(Chr(0), 80)
            SHGetFileInfo(fileExt, FILE_ATTRIBUTE_NORMAL, shinfo, Marshal.SizeOf(shinfo), SHGFI_ICON Or ICOSize Or SHGFI_USEFILEATTRIBUTES)
            Dim bmp As Bitmap = System.Drawing.Icon.FromHandle(shinfo.hIcon).ToBitmap
            DestroyIcon(shinfo.hIcon) ' must destroy icon to avoid GDI leak!
            Return bmp ' return icon as a bitmap
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
        Return Nothing
    End Function
    Public Sub AdjustComboBoxWidth(ByVal sender As Object, ByVal e As EventArgs)
        Dim senderComboBox = DirectCast(sender, ComboBox)
        Dim width As Integer = senderComboBox.DropDownWidth
        Dim g As Graphics = senderComboBox.CreateGraphics()
        Dim font As Font = senderComboBox.Font
        Dim vertScrollBarWidth As Integer = If((senderComboBox.Items.Count > senderComboBox.MaxDropDownItems), SystemInformation.VerticalScrollBarWidth, 0)
        Dim newWidth As Integer
        For Each s As String In DirectCast(sender, ComboBox).Items
            newWidth = CInt(g.MeasureString(s, font).Width) + vertScrollBarWidth
            If width < newWidth Then
                width = newWidth
            End If
        Next
        senderComboBox.DropDownWidth = width
    End Sub
    Public Function SetBarColor(UID As String) As Color
        Dim hash As Integer = UID.GetHashCode
        Dim r, g, b As Integer
        r = (hash And &HFF0000) >> 16
        g = (hash And &HFF00) >> 8
        b = hash And &HFF
        Return Color.FromArgb(r, g, b)
    End Function
    Public Function GetFontColor(color As Color) As Color 'get contrasting font color
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
    Public Sub StartTimer()
        stpw.Stop()
        stpw.Reset()
        stpw.Start()
    End Sub
    Private intTimerHits As Integer = 0
    Public Function StopTimer() As String
        stpw.Stop()
        intTimerHits += 1
        Dim Results As String = intTimerHits & "  Stopwatch: MS:" & stpw.ElapsedMilliseconds & " Ticks: " & stpw.ElapsedTicks
        Debug.Print(Results)
        Return Results
    End Function
    Public Sub Logger(Message As String)
        Dim MaxLogSizeKiloBytes As Short = 500
        Dim DateStamp As String = DateTime.Now.ToString
        Dim infoReader As FileInfo
        infoReader = My.Computer.FileSystem.GetFileInfo(strLogPath)
        If Not File.Exists(strLogPath) Then
            Dim di As DirectoryInfo = Directory.CreateDirectory(strLogDir)
            Using sw As StreamWriter = File.CreateText(strLogPath)
                sw.WriteLine(DateStamp & ": Log Created...")
                sw.WriteLine(DateStamp & ": " & Message)
            End Using
        Else
            If (infoReader.Length / 1000) < MaxLogSizeKiloBytes Then
                Using sw As StreamWriter = File.AppendText(strLogPath)
                    sw.WriteLine(DateStamp & ": " & Message)
                End Using
            Else
                If RotateLogs() Then
                    Using sw As StreamWriter = File.AppendText(strLogPath)
                        sw.WriteLine(DateStamp & ": " & Message)
                    End Using
                End If
            End If
        End If
    End Sub
    Private Function RotateLogs() As Boolean
        Try
            File.Copy(strLogPath, strLogPath + ".old", True)
            File.Delete(strLogPath)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function GetColIndex(ByVal Grid As DataGridView, ByVal strColName As String) As Integer
        Try
            Return Grid.Columns.Item(strColName).Index
        Catch ex As Exception
            Return -1
        End Try
    End Function
    Public Function GetCellValue(ByVal Grid As DataGridView, ColumnName As String) As String
        Return NoNull(Grid.Item(GetColIndex(Grid, ColumnName), Grid.CurrentRow.Index).Value.ToString)
    End Function
    Public Sub EndProgram()
        If OKToEnd() Then
            ProgramEnding = True
            Logger("Ending Program...")
            PurgeTempDir()
            Application.Exit()
        End If
    End Sub
    Public Sub PurgeTempDir()
        Try
            Directory.Delete(strTempPath, True)
        Catch
        End Try
    End Sub
    Public Sub ConnectStatus(Message As String, FColor As Color)
        MainForm.ConnStatusLabel.Text = Message
        MainForm.ConnStatusLabel.ForeColor = FColor
        MainForm.Refresh()
    End Sub
    Public Function YearFromDate(dtDate As Date) As String
        Return dtDate.Year.ToString
    End Function
    Public Function MouseIsOverControl(ByVal c As Control) As Boolean
        Return c.ClientRectangle.Contains(c.PointToClient(Control.MousePosition))
    End Function
    Public Sub FillComboBox(IndexType() As Combo_Data, ByRef cmb As ComboBox)
        cmb.Items.Clear()
        cmb.Text = ""
        Dim i As Integer = 0
        For Each ComboItem As Combo_Data In IndexType
            cmb.Items.Insert(i, ComboItem.strLong)
            i += 1
        Next
    End Sub
    Public Sub FillToolComboBox(IndexType() As Combo_Data, ByRef cmb As ToolStripComboBox)
        cmb.Items.Clear()
        cmb.Text = ""
        Dim i As Integer = 0
        For Each ComboItem As Combo_Data In IndexType
            cmb.Items.Insert(i, ComboItem.strLong)
            i += 1
        Next
    End Sub
    Public Function FormTitle(Device As Device_Info) As String
        Return " - " + Device.strCurrentUser + " - " + Device.strAssetTag + " - " + Device.strDescription
    End Function
    Public Function NotePreview(Note As String, Optional CharLimit As Integer = 50) As String
        If Note <> "" Then
            Return Strings.Left(Note, CharLimit) & IIf(Len(Note) > CharLimit, "...", "").ToString
        Else
            Return ""
        End If
    End Function
    Public Function Message(ByVal Prompt As String, Optional ByVal Buttons As Integer = vbOKOnly + vbInformation, Optional ByVal Title As String = Nothing, Optional ByVal ParentFrm As Form = Nothing) As MsgBoxResult
        Dim NewMessage As New MyDialog(ParentFrm)
        Return NewMessage.DialogMessage(Prompt, Buttons, Title, ParentFrm)
    End Function
    Public Function OKToEnd() As Boolean
        If CheckForActiveTransfers() Then Return False
        If GKUpdater_Form.UpdatesRunning() Then Return False
        Return True
    End Function
    Public Function CheckForActiveTransfers() As Boolean
        Dim ActiveTransfers As New List(Of frmAttachments)
        For Each frm As Form In My.Application.OpenForms
            If TypeOf frm Is frmAttachments Then
                Dim Attachments As frmAttachments = DirectCast(frm, frmAttachments)
                If Attachments.UploadWorker.IsBusy Or Attachments.DownloadWorker.IsBusy Then
                    ActiveTransfers.Add(Attachments)
                End If
            End If
        Next
        If ActiveTransfers.Count > 0 Then
            Dim blah = Message("There are " & ActiveTransfers.Count.ToString & " active uploads/downloads. Do you wish to cancel the current operations?", MessageBoxIcon.Warning + vbYesNo, "Worker Busy")
            If blah = vbYes Then
                For Each AttachForm As frmAttachments In ActiveTransfers
                    If AttachForm.UploadWorker.IsBusy Then AttachForm.UploadWorker.CancelAsync()
                    If AttachForm.DownloadWorker.IsBusy Then AttachForm.DownloadWorker.CancelAsync()
                Next
                Return False
            Else
                For Each AttachForm As frmAttachments In ActiveTransfers
                    AttachForm.WindowState = FormWindowState.Normal
                    AttachForm.Activate()
                Next
                Return True
            End If
        Else
            Return False
        End If
    End Function
    Public Sub HighlightRow(ByRef Grid As DataGridView, Theme As Grid_Theme, Row As Integer)
        Try
            Dim BackColor As Color = Theme.BackColor 'DefGridBC
            Dim SelectColor As Color = Theme.CellSelectColor 'DefGridSelCol
            Dim c1 As Color = Theme.RowHighlightColor 'colHighlightColor 'highlight color
            If Row > -1 Then
                For Each cell As DataGridViewCell In Grid.Rows(Row).Cells
                    Dim c2 As Color = Color.FromArgb(SelectColor.R, SelectColor.G, SelectColor.B)
                    Dim BlendColor As Color
                    BlendColor = Color.FromArgb(CInt((CInt(c1.A) + CInt(c2.A)) / 2),
                                                    CInt((CInt(c1.R) + CInt(c2.R)) / 2),
                                                    CInt((CInt(c1.G) + CInt(c2.G)) / 2),
                                                    CInt((CInt(c1.B) + CInt(c2.B)) / 2))
                    cell.Style.SelectionBackColor = BlendColor
                    c2 = Color.FromArgb(BackColor.R, BackColor.G, BackColor.B)
                    BlendColor = Color.FromArgb(CInt((CInt(c1.A) + CInt(c2.A)) / 2),
                                                    CInt((CInt(c1.R) + CInt(c2.R)) / 2),
                                                    CInt((CInt(c1.G) + CInt(c2.G)) / 2),
                                                    CInt((CInt(c1.B) + CInt(c2.B)) / 2))
                    cell.Style.BackColor = BlendColor
                Next
            End If
        Catch
        End Try
    End Sub
    Public Sub LeaveRow(ByRef Grid As DataGridView, Theme As Grid_Theme, Row As Integer)
        Dim BackColor As Color = Theme.BackColor
        Dim SelectColor As Color = Theme.CellSelectColor
        If Row > -1 Then
            For Each cell As DataGridViewCell In Grid.Rows(Row).Cells
                cell.Style.SelectionBackColor = SelectColor
                cell.Style.BackColor = BackColor
            Next
        End If
    End Sub
    Public Sub SetCursor(CursorType As Cursor)
        If Cursor.Current IsNot CursorType Then
            Cursor.Current = CursorType
        End If
    End Sub
    Public Function ValidPhoneNumber(PhoneNum As String) As Boolean
        If Trim(PhoneNum) <> "" Then
            Const nDigits As Integer = 10
            Dim fPhoneNum As String = ""
            Dim NumArray() As Char = PhoneNum.ToCharArray()
            For Each num As Char In NumArray
                If Char.IsDigit(num) Then fPhoneNum += num.ToString
            Next
            If Len(fPhoneNum) <> nDigits Then
                Return False
            Else
                Return True
            End If
        Else
            Return True
        End If
    End Function
End Module
