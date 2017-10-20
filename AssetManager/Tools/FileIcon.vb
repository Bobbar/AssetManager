Imports System.Runtime.InteropServices
Public Class FileIcon
    Private Const MAX_PATH As Int32 = 260
    Private Const SHGFI_ICON As Int32 = &H100
    Private Const SHGFI_USEFILEATTRIBUTES As Int32 = &H10
    Private Const FILE_ATTRIBUTE_NORMAL As Int32 = &H80

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
    Private Shared Function DestroyIcon(ByVal hIcon As IntPtr) As Boolean
    End Function

    Public Shared Function GetFileIcon(ByVal fileExt As String) As Bitmap ', Optional ByVal ICOsize As IconSize = IconSize.SHGFI_SMALLICON
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

End Class
