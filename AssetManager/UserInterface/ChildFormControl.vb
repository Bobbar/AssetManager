Public Module ChildFormControl

    Public Sub ActivateForm(form As Form)
        If Not form.IsDisposed Then
            form.Show()
            form.Activate()
            form.WindowState = FormWindowState.Normal
        End If
    End Sub

    Public Function AttachmentsIsOpen(parentForm As Form) As Boolean
        For Each frm As Form In GetChildren(parentForm)
            If TypeOf frm Is AttachmentsForm And frm.Tag Is parentForm Then
                ActivateForm(frm)
                Return True
            End If
        Next
        Return False
    End Function

    Public Sub CloseChildren(parentForm As Form)
        Dim Children As List(Of Form) = GetChildren(parentForm)
        If Children.Count > 0 Then
            For Each child As Form In Children
                child.Dispose()
            Next
        End If
        Children.Clear()
    End Sub

    Public Function GetChildren(parentForm As Form, Optional includeParent As Boolean = False) As List(Of Form)
        Dim Children As New List(Of Form)
        If includeParent Then Children.Add(parentForm)
        For Each frms As Form In My.Application.OpenForms
            If frms.Tag Is parentForm Then
                Children.Add(frms)
            End If
        Next
        Return Children
    End Function

    Public Sub LookupDevice(parentForm As ExtendedForm, device As DeviceStruct)
        If device.GUID IsNot Nothing Then
            If Not FormIsOpenByUID(GetType(ViewDeviceForm), device.GUID) Then
                Dim NewView As New ViewDeviceForm(parentForm, device.GUID)
            End If
        Else
            Message("Device not found.", vbOKOnly + vbExclamation, "Error", parentForm)
        End If
    End Sub

    Public Sub MinimizeChildren(parentForm As Form)
        For Each chld As Form In GetChildren(parentForm)
            chld.WindowState = FormWindowState.Minimized
        Next
    End Sub

    Public Sub RestoreChildren(parentForm As Form)
        For Each chld As Form In GetChildren(parentForm)
            chld.WindowState = FormWindowState.Normal
        Next
    End Sub

    Public Function SibiIsOpen() As Boolean
        If Application.OpenForms.OfType(Of SibiMainForm).Any Then
            Return True
        End If
        Return False
    End Function

    Public Function FormTypeIsOpen(formType As Type) As Boolean
        For Each frm As Form In My.Application.OpenForms
            If frm.GetType = formType Then Return True
        Next
        Return False
    End Function

    Public Function FormIsOpenByUID(formType As Type, UID As String) As Boolean
        For Each frm As ExtendedForm In My.Application.OpenForms
            If frm.GetType = formType AndAlso frm.FormUID = UID Then
                ActivateForm(frm)
                Return True
            End If
        Next
        Return False
    End Function

    Public Function GetActiveAttachmentForms(Optional parentForm As Form = Nothing) As List(Of AttachmentsForm)
        Dim ActiveTransfers As New List(Of AttachmentsForm)
        For Each frm As Form In My.Application.OpenForms
            If TypeOf frm Is AttachmentsForm Then
                Dim AttachForm As AttachmentsForm = DirectCast(frm, AttachmentsForm)
                If parentForm IsNot Nothing Then
                    If AttachForm.ActiveTransfer AndAlso DirectCast(AttachForm.Tag, Form) Is parentForm Then
                        ActiveTransfers.Add(AttachForm)
                    End If
                Else
                    If AttachForm.ActiveTransfer Then
                        ActiveTransfers.Add(AttachForm)
                    End If
                End If
            End If
        Next
        Return ActiveTransfers
    End Function

    Public Function OKToCloseChildren(parentForm As Form) As Boolean
        Dim CanClose As Boolean = True
        Dim frms = My.Application.OpenForms.OfType(Of ExtendedForm).ToList.FindAll(Function(f) f.Tag Is parentForm).ToArray
        For i = 0 To frms.Length - 1
            If Not frms(i).OKToClose Then CanClose = False
        Next
        Return CanClose
    End Function




End Module