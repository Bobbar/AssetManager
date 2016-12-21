Public Module Form_Control
    Public Sub ActivateFormByUID(strGUID As String)
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
    Public Sub ActivateFormByHandle(form As Form)
        form.Show()
        form.Activate()
        form.WindowState = FormWindowState.Normal
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
        Private WithEvents RefreshTimer As Timer
        Private MyParentForm As Form
        Private DropDownControl As ToolStripDropDownButton
        Private intFormCount As Integer
        Sub New(ParentForm As Form, DropDownCtl As ToolStripDropDownButton)
            MyParentForm = ParentForm
            DropDownControl = DropDownCtl
            Init()
        End Sub
        Private Sub Init()
            InitializeTimer()
        End Sub
        Private Sub InitializeTimer()
            RefreshTimer = New Timer
            RefreshTimer.Interval = 100
            RefreshTimer.Enabled = True
            AddHandler RefreshTimer.Tick, AddressOf RefreshTimer_Tick
        End Sub
        Private Sub RefreshTimer_Tick(sender As Object, e As EventArgs) Handles RefreshTimer.Tick
            If FormCount() <> intFormCount Then
                If Not DropDownControl.DropDown.Focused Then
                    DropDownControl.DropDownItems.Clear()
                    RefreshWindowList(MyParentForm, DropDownControl.DropDownItems)
                    intFormCount = FormCount()
                End If
            End If
        End Sub
        Private Function FormCount() As Integer
            Dim i As Integer = 0
            For Each frm As Form In My.Application.OpenForms
                If Not frm.IsDisposed And Not frm.Modal Then i += 1
            Next
            Return i
        End Function
        Public Sub RefreshWindowList(ParentForm As Form, ByRef TargetMenuItem As ToolStripItemCollection)
            For Each frm As Form In ListOfChilden(ParentForm)
                If HasChildren(frm) Then
                    Dim NewDropDown As ToolStripMenuItem = NewMenuItem(frm)
                    TargetMenuItem.Add(NewDropDown)
                    RefreshWindowList(frm, NewDropDown.DropDownItems)
                Else
                    TargetMenuItem.Add(NewMenuItem(frm))
                End If
            Next
        End Sub
        Private Function NewMenuItem(frm As Form) As ToolStripMenuItem
            If TypeOf frm Is frmView Then
                Dim vw As frmView = frm
                Dim newitem As New ToolStripMenuItem
                newitem.Text = vw.Text
                newitem.Image = My.Resources.inventory_small_fw
                newitem.Tag = vw
                newitem.ToolTipText = "Right-Click to close."
                AddHandler newitem.MouseDown, AddressOf WindowClick
                Return newitem
            ElseIf TypeOf frm Is frmManageRequest Then
                Dim req As frmManageRequest = frm
                Dim newitem As New ToolStripMenuItem
                newitem.Text = req.Text
                newitem.Image = My.Resources.Acquire_new_shadow_small
                newitem.Tag = req
                newitem.ToolTipText = "Right-Click to close."
                AddHandler newitem.MouseDown, AddressOf WindowClick
                Return newitem
            ElseIf TypeOf frm Is frmSibiMain Then
                Dim sibi As frmSibiMain = frm
                Dim newitem As New ToolStripMenuItem
                newitem.Text = sibi.Text
                newitem.Image = My.Resources.Acquire_new_shadow_small
                newitem.Tag = sibi
                newitem.ToolTipText = "Right-Click to close."
                AddHandler newitem.MouseDown, AddressOf WindowClick
                Return newitem
            Else
                Dim genfrm As Form = frm
                Dim newitem As New ToolStripMenuItem
                newitem.Text = genfrm.Text
                newitem.Image = My.Resources.inventory_small_fw
                newitem.Tag = genfrm
                newitem.ToolTipText = "Right-Click to close."
                AddHandler newitem.MouseDown, AddressOf WindowClick
                Return newitem
            End If
        End Function
        Private Function HasChildren(ParentForm As Form) As Boolean
            For Each frm As Form In My.Application.OpenForms
                If frm.Tag Is ParentForm And Not frm.IsDisposed Then
                    Return True
                End If
            Next
            Return False
        End Function
        Private Function ListOfChilden(ParentForm As Form) As List(Of Form)
            Dim tmpList As New List(Of Form)
            For Each frm As Form In My.Application.OpenForms
                If frm.Tag Is ParentForm And Not frm.IsDisposed Then
                    tmpList.Add(frm)
                End If
            Next
            Return tmpList
        End Function
        Private Sub WindowClick(sender As ToolStripItem, e As MouseEventArgs)
            If e.Button = MouseButtons.Right Then
                Dim item As ToolStripItem = sender
                Dim frm As Form = item.Tag
                item.Dispose()
                frm.Dispose()
                intFormCount = FormCount()
                If DropDownControl.DropDownItems.Count = 0 Then
                    DropDownControl.HideDropDown()
                End If
            ElseIf e.Button = MouseButtons.Left Then
                ActivateFormByHandle(sender.Tag)
            End If
        End Sub
    End Class
End Module
