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
        '  Private CurrentWindows As New List(Of Form)
        Private MyParentForm As Form
        Private DropDownControl As ToolStripDropDownButton
        Sub New(ParentForm As Form, DropDownCtl As ToolStripDropDownButton)
            MyParentForm = ParentForm
            DropDownControl = DropDownCtl
            Init()
        End Sub
        Private Sub Init()
            ' AddHandler DropDownControl.DropDownItemClicked, AddressOf WindowSelectClick
        End Sub
        Public Sub RefreshWindowList(Optional ParentForm As Form = Nothing)
            'CurrentWindows.Clear()
            'For Each frm As Form In My.Application.OpenForms
            '    If frm.Tag Is MyParentForm And Not frm.IsDisposed Then
            '        CurrentWindows.Add(frm)
            '    End If
            'Next
            DropDownControl.DropDownItems.Clear()
            For Each frm As Form In My.Application.OpenForms 'CurrentWindows

                If frm.Tag Is MyParentForm And Not frm.IsDisposed Then


                    If HasChildren(frm) Then



                        Dim NewDropDown As ToolStripMenuItem = WindowListButton(frm)


                        For Each Childfrm As Form In ListOfChilden(frm)
                            If Not Childfrm.IsDisposed Then
                                AddDDItem(NewDropDown.DropDownItems, Childfrm)
                            End If

                        Next
                        DropDownControl.DropDownItems.Add(NewDropDown)
                    Else
                        ' Dim NewDropDown As ToolStripMenuItem = WindowListButton(frm)
                        AddDDItem(DropDownControl.DropDownItems, frm)



                    End If


                End If


                'If TypeOf frm Is frmView Then
                '    Dim vw As frmView = frm
                '    Dim newitem As New ToolStripMenuItem
                '    newitem.Text = vw.Text
                '    newitem.Image = My.Resources.inventory_small_fw
                '    newitem.Tag = vw
                '    newitem.ToolTipText = "Right-Click to close."
                '    AddHandler newitem.MouseDown, AddressOf WindowCloseClick

                '    DropDownControl.DropDownItems.Add(newitem)
                'ElseIf TypeOf frm Is frmManageRequest Then
                '    Dim req As frmManageRequest = frm
                '    Dim newitem As New ToolStripMenuItem
                '    newitem.Text = req.Text
                '    newitem.Image = My.Resources.Acquire_new_shadow_small
                '    newitem.Tag = req
                '    newitem.ToolTipText = "Right-Click to close."
                '    AddHandler newitem.MouseDown, AddressOf WindowCloseClick
                '    DropDownControl.DropDownItems.Add(newitem)
                'ElseIf TypeOf frm Is frmSibiMain Then
                '    Dim sibi As frmSibiMain = frm
                '    Dim newitem As New ToolStripMenuItem
                '    newitem.Text = sibi.Text
                '    newitem.Image = My.Resources.Acquire_new_shadow_small
                '    newitem.Tag = sibi
                '    newitem.ToolTipText = "Right-Click to close."
                '    AddHandler newitem.MouseDown, AddressOf WindowCloseClick
                '    DropDownControl.DropDownItems.Insert(0, newitem)
                'End If
            Next
        End Sub
        Private Function WindowListButton(frm As Form) As ToolStripMenuItem
            ' Dim tmpButton As New ToolStripDropDownButton

            If TypeOf frm Is frmView Then
                Dim vw As frmView = frm
                Dim newitem As New ToolStripMenuItem
                newitem.Text = vw.Text
                newitem.Image = My.Resources.inventory_small_fw
                newitem.Tag = vw
                newitem.ToolTipText = "Right-Click to close."
                AddHandler newitem.MouseDown, AddressOf WindowClick
                Return newitem
                'DropDownControl.DropDownItems.Add(newitem)
            ElseIf TypeOf frm Is frmManageRequest Then
                Dim req As frmManageRequest = frm
                Dim newitem As New ToolStripMenuItem
                newitem.Text = req.Text
                newitem.Image = My.Resources.Acquire_new_shadow_small
                newitem.Tag = req
                newitem.ToolTipText = "Right-Click to close."
                AddHandler newitem.MouseDown, AddressOf WindowClick
                Return newitem
                ' DropDownControl.DropDownItems.Add(newitem)
            ElseIf TypeOf frm Is frmSibiMain Then
                Dim sibi As frmSibiMain = frm
                Dim newitem As New ToolStripMenuItem
                newitem.Text = sibi.Text
                newitem.Image = My.Resources.Acquire_new_shadow_small
                newitem.Tag = sibi
                newitem.ToolTipText = "Right-Click to close."
                AddHandler newitem.MouseDown, AddressOf WindowClick
                Return newitem
                ' DropDownControl.DropDownItems.Insert(0, newitem)
            End If


        End Function
        Private Function AddDDItem(ByRef DDMenuItem As ToolStripItemCollection, frm As Form) As ToolStripMenuItem
            If TypeOf frm Is frmView Then
                Dim vw As frmView = frm
                Dim newitem As New ToolStripMenuItem
                newitem.Text = vw.Text
                newitem.Image = My.Resources.inventory_small_fw
                newitem.Tag = vw
                newitem.ToolTipText = "Right-Click to close."
                AddHandler newitem.MouseDown, AddressOf WindowClick
                DDMenuItem.Add(newitem)

                ' Return DDMenuItem
            ElseIf TypeOf frm Is frmManageRequest Then
                Dim req As frmManageRequest = frm
                Dim newitem As New ToolStripMenuItem
                newitem.Text = req.Text
                newitem.Image = My.Resources.Acquire_new_shadow_small
                newitem.Tag = req
                newitem.ToolTipText = "Right-Click to close."
                AddHandler newitem.MouseDown, AddressOf WindowClick
                DDMenuItem.Add(newitem)
                'Return DDMenuItem
            ElseIf TypeOf frm Is frmSibiMain Then
                Dim sibi As frmSibiMain = frm
                Dim newitem As New ToolStripMenuItem
                newitem.Text = sibi.Text
                newitem.Image = My.Resources.Acquire_new_shadow_small
                newitem.Tag = sibi
                newitem.ToolTipText = "Right-Click to close."
                AddHandler newitem.MouseDown, AddressOf WindowClick
                DDMenuItem.Insert(0, newitem)
                ' Return DDMenuItem
            Else
                Dim genfrm As Form = frm
                Dim newitem As New ToolStripMenuItem
                newitem.Text = genfrm.Text
                newitem.Image = My.Resources.Acquire_new_shadow_small
                newitem.Tag = genfrm
                newitem.ToolTipText = "Right-Click to close."
                AddHandler newitem.MouseDown, AddressOf WindowClick
                'DDMenuItem.Insert(0, newitem)
                DDMenuItem.Add(newitem)
                ' Return DDMenuItem
            End If

        End Function
        Private Function HasChildren(ParentForm As Form) As Boolean
            For Each frm As Form In My.Application.OpenForms
                If frm.Tag Is ParentForm And Not frm.IsDisposed Then
                    'CurrentWindows.Add(frm)
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
        Private Sub WindowSelectClick(sender As Object, e As ToolStripItemClickedEventArgs)
            Dim item As ToolStripItem = e.ClickedItem
            If TypeOf item.Tag Is frmView Then
                Dim vw As frmView = item.Tag
                ActivateForm(vw.CurrentViewDevice.strGUID)
            ElseIf TypeOf item.Tag Is frmManageRequest Then
                Dim req As frmManageRequest = item.Tag
                ActivateForm(req.CurrentRequest.strUID)
            ElseIf TypeOf item.Tag Is frmSibiMain Then
                frmSibiMain.Show()
                frmSibiMain.Activate()
                frmSibiMain.WindowState = FormWindowState.Normal
            End If
        End Sub
        Private Sub WindowClick(sender As ToolStripItem, e As MouseEventArgs)
            If e.Button = MouseButtons.Right Then
                Dim item As ToolStripItem = sender
                If TypeOf item.Tag Is frmView Then
                    DropDownControl.DropDownItems.Remove(item)
                    Dim vw As frmView = item.Tag
                    vw.Dispose()
                ElseIf TypeOf item.Tag Is frmManageRequest Then
                    DropDownControl.DropDownItems.Remove(item)
                    Dim req As frmManageRequest = item.Tag
                    req.Dispose()
                ElseIf TypeOf item.Tag Is frmSibiMain Then
                    DropDownControl.DropDownItems.Remove(item)
                    frmSibiMain.Dispose()
                ElseIf TypeOf item.Tag Is frmAttachments Then
                    DropDownControl.DropDownItems.Remove(item)
                    Dim attach As frmAttachments = item.Tag
                    attach.Dispose()

                End If


                If DropDownControl.DropDownItems.Count = 0 Then
                    DropDownControl.HideDropDown()
                End If
            ElseIf e.Button = MouseButtons.Left Then
                Dim item As ToolStripItem = sender 'e.ClickedItem
                If TypeOf item.Tag Is frmView Then
                    Dim vw As frmView = item.Tag
                    ActivateForm(vw.CurrentViewDevice.strGUID)
                ElseIf TypeOf item.Tag Is frmManageRequest Then
                    Dim req As frmManageRequest = item.Tag
                    ActivateForm(req.CurrentRequest.strUID)
                ElseIf TypeOf item.Tag Is frmSibiMain Then
                    frmSibiMain.Show()
                    frmSibiMain.Activate()
                    frmSibiMain.WindowState = FormWindowState.Normal
                End If



            End If
        End Sub


    End Class
End Module
