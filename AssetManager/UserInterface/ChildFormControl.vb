Public Module ChildFormControl

    Public Sub ActivateForm(form As ExtendedForm)
        If Not form.IsDisposed Then
            form.Show()
            form.Activate()
            form.WindowState = FormWindowState.Normal
        End If
    End Sub

    Public Function AttachmentsIsOpen(parentForm As ExtendedForm) As Boolean
        For Each frm In GetChildren(parentForm)
            If TypeOf frm Is AttachmentsForm And frm.ParentForm Is parentForm Then
                ActivateForm(frm)
                Return True
            End If
        Next
        Return False
    End Function

    Public Sub CloseChildren(parentForm As ExtendedForm)
        Dim Children = GetChildren(parentForm)
        If Children.Count > 0 Then
            For Each child As ExtendedForm In Children
                child.Dispose()
            Next
        End If
        Children.Clear()
    End Sub

    Public Function GetChildren(parentForm As ExtendedForm) As List(Of ExtendedForm)
        Return My.Application.OpenForms.OfType(Of ExtendedForm).ToList.FindAll(Function(f) f.ParentForm Is parentForm And Not f.IsDisposed)
    End Function

    Public Sub LookupDevice(parentForm As ExtendedForm, device As DeviceObject)
        If device.GUID IsNot Nothing Then
            If Not FormIsOpenByUID(GetType(ViewDeviceForm), device.GUID) Then
                Dim NewView As New ViewDeviceForm(parentForm, device.GUID)
            End If
        Else
            Message("Device not found.", vbOKOnly + vbExclamation, "Error", parentForm)
        End If
    End Sub

    Public Sub MinimizeChildren(parentForm As ExtendedForm)
        For Each child In GetChildren(parentForm)
            child.WindowState = FormWindowState.Minimized
        Next
    End Sub

    Public Sub RestoreChildren(parentForm As ExtendedForm)
        For Each child In GetChildren(parentForm)
            child.WindowState = FormWindowState.Normal
        Next
    End Sub

    Public Function SibiIsOpen() As Boolean
        If Application.OpenForms.OfType(Of SibiMainForm).Any Then
            Return True
        End If
        Return False
    End Function

    Public Function GetChildOfType(parentForm As ExtendedForm, childType As Type) As ExtendedForm
        Return GetChildren(parentForm).Find(Function(f) f.GetType = childType)
    End Function

    Public Function FormTypeIsOpen(formType As Type) As Boolean
        For Each frm As ExtendedForm In My.Application.OpenForms
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

    Public Function OKToCloseChildren(parentForm As ExtendedForm) As Boolean
        Dim CanClose As Boolean = True
        Dim frms = GetChildren(parentForm).ToArray
        For i = 0 To frms.Length - 1
            If Not frms(i).OKToClose Then CanClose = False
        Next
        Return CanClose
    End Function




End Module