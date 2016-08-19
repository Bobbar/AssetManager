Option Explicit On
Imports System.Environment
Imports System.IO
Imports System.Runtime.InteropServices
Module OtherFunctions
    'paths
    Public strLogDir As String = GetFolderPath(SpecialFolder.ApplicationData) & "\AssetManager\"
    Public strLogName As String = "log.log"
    Public strLogPath As String = strLogDir & strLogName
    Public strTempPath As String = strLogDir & "temp\"
    'colors
    Public colCurrentEntry As Color = ColorTranslator.FromHtml("#7AD1FF") '"#7AD1FF"
    Public colMissingField As Color = ColorTranslator.FromHtml("#82C1FF") '"#FF9827") '"#75BAFF")
    Public colCheckIn As Color = ColorTranslator.FromHtml("#B6FCC0")
    Public colCheckOut As Color = ColorTranslator.FromHtml("#FCB6B6")
    Public colHighlightOrange As Color = ColorTranslator.FromHtml("#FF9A26") '"#FF9827")
    Public colHighlightBlue As Color = ColorTranslator.FromHtml("#8BCEE8")
    Public colHighlightColor As Color = ColorTranslator.FromHtml("#FF6600")
    Public colSelectColor As Color = ColorTranslator.FromHtml("#FFB917")
    Public colEditColor As Color = ColorTranslator.FromHtml("#81EAAA")
    Public colFormBackColor As Color = Color.FromArgb(232, 232, 232)
    Public colStatusBarProblem As Color = ColorTranslator.FromHtml("#FF9696")
    Public colToolBarColor As Color = Color.FromArgb(249, 226, 166)
    Public DefGridBC As Color, DefGridSelCol As Color
    'misc
    Public GridStylez As System.Windows.Forms.DataGridViewCellStyle ' = New System.Windows.Forms.DataGridViewCellStyle()
    Public GridFont As Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Public stpw As New Stopwatch
    Public ProgramEnding As Boolean = False
    Private Const MAX_PATH As Int32 = 260
    Private Const SHGFI_ICON As Int32 = &H100
    Private Const SHGFI_USEFILEATTRIBUTES As Int32 = &H10
    Private Const FILE_ATTRIBUTE_NORMAL As Int32 = &H80
    Public Sub SetGridStyle(Grid As DataGridView)
        Grid.BackgroundColor = DefGridBC
        Grid.DefaultCellStyle = GridStylez
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
    <DllImport("shell32.dll", CharSet:=CharSet.Auto)>
    Private Function SHGetFileInfo(
                ByVal pszPath As String,
                ByVal dwFileAttributes As Int32,
                ByRef psfi As SHFILEINFO,
                ByVal cbFileInfo As Int32,
                ByVal uFlags As Int32) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Function DestroyIcon(ByVal hIcon As IntPtr) As Boolean
    End Function

    Public Function GetFileIcon(ByVal fileExt As String) As Bitmap ', Optional ByVal ICOsize As IconSize = IconSize.SHGFI_SMALLICON
        Dim ICOSize As IconSize = IconSize.SHGFI_SMALLICON
        Dim shinfo As New SHFILEINFO
        shinfo.szDisplayName = New String(Chr(0), MAX_PATH)
        shinfo.szTypeName = New String(Chr(0), 80)
        SHGetFileInfo(fileExt, FILE_ATTRIBUTE_NORMAL, shinfo, Marshal.SizeOf(shinfo), SHGFI_ICON Or ICOSize Or SHGFI_USEFILEATTRIBUTES)
        Dim bmp As Bitmap = System.Drawing.Icon.FromHandle(shinfo.hIcon).ToBitmap
        DestroyIcon(shinfo.hIcon) ' must destroy icon to avoid GDI leak!
        Return bmp ' return icon as a bitmap
    End Function
    Public Function AdjustComboBoxWidth(ByVal sender As Object, ByVal e As EventArgs)
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
        Return False
    End Function
    Public Sub StartTimer()
        stpw.Stop()
        stpw.Reset()
        stpw.Start()
    End Sub
    Public Sub StopTimer()
        stpw.Stop()
        Debug.Print("Stopwatch: MS:" & stpw.ElapsedMilliseconds & " Ticks: " & stpw.ElapsedTicks)
    End Sub
    Public Sub Logger(Message As String)
        Dim MaxLogSizeKiloBytes As Short = 100
        Dim DateStamp As String = DateTime.Now
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
        Return Grid.Item(GetColIndex(Grid, ColumnName), Grid.CurrentRow.Index).Value
    End Function
    Public Sub ConnectionNotReady()
        Dim blah = MsgBox("Not connected to server or connection is busy!", vbOKOnly + vbExclamation, "Cannot Connect")
    End Sub
    Public Sub EndProgram()
        ProgramEnding = True
        Logger("Ending Program...")
        PurgeTempDir()
        GlobalConn.Close()
        LiveConn.Close()
        Application.Exit()
        End
    End Sub
    Public Sub PurgeTempDir()
        On Error Resume Next
        Directory.Delete(strTempPath, True)
    End Sub
    Public Sub ConnectStatus(Message As String, FColor As Color)
        MainForm.ConnStatusLabel.Text = Message
        MainForm.ConnStatusLabel.ForeColor = FColor
        MainForm.Refresh()
    End Sub
    Public Function NoNull(DBVal As Object) As String
        Try
            Return IIf(IsDBNull(DBVal), "", DBVal.ToString)
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return ""
        End Try
    End Function
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
        Return " - " + Device.strDescription + " - " + Device.strCurrentUser
    End Function
End Module
