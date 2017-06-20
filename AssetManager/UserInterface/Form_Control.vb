Public Module Form_Control
    Public Sub ActivateFormByUID(strGUID As String)
        For Each frm As Form In My.Application.OpenForms
            Select Case frm.GetType
                Case GetType(ViewDeviceForm)
                    Dim vw As ViewDeviceForm = DirectCast(frm, ViewDeviceForm)
                    If vw.CurrentViewDevice.strGUID = strGUID Then
                        vw.Activate()
                        vw.WindowState = FormWindowState.Normal
                        vw.Show()
                        Exit For
                    End If
                Case GetType(SibiManageRequestForm)
                    Dim vw As SibiManageRequestForm = DirectCast(frm, SibiManageRequestForm)
                    If vw.CurrentRequest.strUID = strGUID Then
                        vw.Activate()
                        vw.WindowState = FormWindowState.Normal
                        vw.Show()
                        Exit For
                    End If
                Case GetType(AttachmentsForm)
                    Dim vw As AttachmentsForm = DirectCast(frm, AttachmentsForm)
                    If vw.AttachFolderUID = strGUID Then
                        vw.Activate()
                        vw.WindowState = FormWindowState.Normal
                        vw.Show()
                        Exit For
                    End If
                Case GetType(ViewHistoryForm)
                    Dim vw As ViewHistoryForm = DirectCast(frm, ViewHistoryForm)
                    If vw.EntryGUID = strGUID Then
                        vw.Activate()
                        vw.WindowState = FormWindowState.Normal
                        vw.Show()
                        Exit For
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
    Public Function SibiIsOpen() As Boolean
        If Application.OpenForms.OfType(Of SibiMainForm).Any Then
            Return True
        End If
        Return False
    End Function
    Public Function DeviceIsOpen(strGUID As String) As Boolean
        For Each frm As Form In My.Application.OpenForms
            If TypeOf frm Is ViewDeviceForm Then
                Dim vw As ViewDeviceForm = DirectCast(frm, ViewDeviceForm)
                If vw.CurrentViewDevice.strGUID = strGUID Then Return True
            End If
        Next
        Return False
    End Function
    Public Function RequestIsOpen(strGUID As String) As Boolean
        For Each frm As Form In My.Application.OpenForms
            If TypeOf frm Is SibiManageRequestForm Then
                Dim vw As SibiManageRequestForm = DirectCast(frm, SibiManageRequestForm)
                If vw.CurrentRequest.strUID = strGUID Then Return True
            End If
        Next
        Return False
    End Function
    Public Function EntryIsOpen(EntryUID As String) As Boolean
        For Each frm As Form In My.Application.OpenForms
            If TypeOf frm Is ViewHistoryForm Then
                Dim vw As ViewHistoryForm = DirectCast(frm, ViewHistoryForm)
                If vw.EntryGUID = EntryUID Then Return True
            End If
        Next
        Return False
    End Function
    Public Sub LookupDevice(ParentForm As MyForm, Device As Device_Info)
        If Device.strGUID IsNot Nothing Then
            If Not DeviceIsOpen(Device.strGUID) Then
                Dim NewView As New ViewDeviceForm(ParentForm, Device.strGUID)
            Else
                ActivateFormByUID(Device.strGUID)
            End If
        Else
            Message("Device not found.", vbOKOnly + vbExclamation, "Error", ParentForm)
        End If
    End Sub
End Module
