Public Module Form_Control
    Public Sub ActivateForm(strGUID As String)
        For Each frm As Form In My.Application.OpenForms
            Select Case frm.GetType
                Case GetType(frmView)
                    Dim vw As frmView = frm
                    If vw.CurrentViewDevice.strGUID = strGUID Then
                        vw.Activate()
                        vw.WindowState = FormWindowState.Normal
                        vw.Show()
                    End If
                Case GetType(frmManageRequest)
                    Dim vw As frmManageRequest = frm
                    If vw.CurrentRequest.strUID = strGUID Then
                        vw.Activate()
                        vw.WindowState = FormWindowState.Normal
                        vw.Show()
                    End If
                Case GetType(frmAttachments)
                    Dim vw As frmAttachments = frm
                    If vw.AttachFolderID = strGUID Then
                        vw.Activate()
                        vw.WindowState = FormWindowState.Normal
                        vw.Show()
                    End If
            End Select
        Next
    End Sub
    Public Sub MinimizeAll()
        For Each frm As Form In My.Application.OpenForms
            frm.WindowState = FormWindowState.Minimized
        Next
    End Sub
    Public Sub RestoreAll()
        For Each frm As Form In My.Application.OpenForms
            frm.WindowState = FormWindowState.Normal
        Next
    End Sub
    Public Function GetChildren(ParentForm As Form) As List(Of Form)
        Dim Children As New List(Of Form)
        For Each frms As Form In My.Application.OpenForms
            If frms.Tag Is ParentForm Then
                Children.Add(frms)
            End If
        Next
        Return Children
    End Function
    Public Sub CloseChildren(ParentForm As Form)
        Dim Children As List(Of Form) = GetChildren(ParentForm)
        If Children.Count > 0 Then
            For Each child As Form In Children
                child.Dispose()
            Next
        End If
        Children.Clear()
    End Sub
    Public Sub RestoreChildren(ParentForm As Form)
        Dim Children As List(Of Form) = GetChildren(ParentForm)
        If Children.Count > 0 Then
            For Each chld As Form In Children
                chld.WindowState = FormWindowState.Normal
            Next
        End If
        Children.Clear()
    End Sub
    Public Sub MinimizeChildren(ParentForm As Form)
        Dim Children As List(Of Form) = GetChildren(ParentForm)
        If Children.Count > 0 Then
            For Each chld As Form In Children
                chld.WindowState = FormWindowState.Minimized
            Next
        End If
        Children.Clear()
    End Sub
    Public Function WindowIsOpen(WindowName As String, ParentForm As Form)
        For Each frm As Form In My.Application.OpenForms
            If frm.Name = WindowName And frm.Tag Is ParentForm Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function SibiIsOpen() As Boolean
        If Application.OpenForms.OfType(Of frmSibiMain).Any Then
            Return True
        End If
        Return False
    End Function
    Public Function DeviceIsOpen(strGUID As String) As Boolean
        For Each frm As Form In My.Application.OpenForms
            If TypeOf frm Is frmView Then
                Dim vw As frmView = frm
                If vw.CurrentViewDevice.strGUID = strGUID Then Return True
            End If
        Next
        Return False
    End Function
    Public Function RequestIsOpen(strGUID As String) As Boolean
        For Each frm As Form In My.Application.OpenForms
            If TypeOf frm Is frmManageRequest Then
                Dim vw As frmManageRequest = frm
                If vw.CurrentRequest.strUID = strGUID Then Return True
            End If
        Next
        Return False
    End Function
    Public Class WindowList
        Private CurrentWindows As New List(Of Form)
        Private MyParentForm As Form
        Private DropDownControl As ToolStripDropDownButton
        Sub New(ParentForm As Form, DropDownCtl As ToolStripDropDownButton)
            MyParentForm = ParentForm
            DropDownControl = DropDownCtl
            Init()
        End Sub
        Private Sub Init()
            AddHandler DropDownControl.DropDownItemClicked, AddressOf WindowSelectClick
        End Sub
        Public Sub RefreshWindowList()
            CurrentWindows.Clear()
            For Each frm As Form In My.Application.OpenForms
                If frm.Tag Is MyParentForm And Not frm.IsDisposed Then
                    CurrentWindows.Add(frm)
                End If
            Next
            DropDownControl.DropDownItems.Clear()
            For Each frm As Form In CurrentWindows
                If frm.GetType Is GetType(frmView) Then
                    Dim vw As frmView = frm
                    Dim newitem As New ToolStripMenuItem
                    newitem.Text = vw.Text
                    newitem.Image = My.Resources.inventory_small_fw
                    newitem.Tag = vw
                    DropDownControl.DropDownItems.Add(newitem)
                ElseIf frm.GetType Is GetType(frmManageRequest) Then
                    Dim req As frmManageRequest = frm
                    Dim newitem As New ToolStripMenuItem
                    newitem.Text = req.Text
                    newitem.Image = My.Resources.Acquire_new_shadow_small
                    newitem.Tag = req
                    DropDownControl.DropDownItems.Add(newitem)
                ElseIf frm.GetType Is GetType(frmSibiMain) Then
                    Dim sibi As frmSibiMain = frm
                    Dim newitem As New ToolStripMenuItem
                    newitem.Text = sibi.Text
                    newitem.Image = My.Resources.Acquire_new_shadow_small
                    newitem.Tag = sibi
                    DropDownControl.DropDownItems.Insert(0, newitem)
                End If
            Next
        End Sub
        Private Sub WindowSelectClick(sender As Object, e As ToolStripItemClickedEventArgs)
            Dim item As ToolStripItem = e.ClickedItem
            If item.Tag.GetType Is GetType(frmView) Then
                Dim vw As frmView = item.Tag
                ActivateForm(vw.CurrentViewDevice.strGUID)
            ElseIf item.Tag.GetType Is GetType(frmManageRequest) Then
                Dim req As frmManageRequest = item.Tag
                ActivateForm(req.CurrentRequest.strUID)
            ElseIf item.Tag.GetType Is GetType(frmSibiMain) Then
                frmSibiMain.Show()
                frmSibiMain.Activate()
                frmSibiMain.WindowState = FormWindowState.Normal
            End If
        End Sub
    End Class
End Module
