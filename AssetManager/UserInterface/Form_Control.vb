Public Module Form_Control

    Public Sub ActivateFormByHandle(form As Form)
        form.Show()
        form.Activate()
        form.WindowState = FormWindowState.Normal
    End Sub

    Public Function AttachmentsIsOpen(ParentForm As Form) As Boolean
        For Each frm As Form In GetChildren(ParentForm)
            If TypeOf frm Is AttachmentsForm And frm.Tag Is ParentForm Then
                ActivateFormByHandle(frm)
                Return True
            End If
        Next
        Return False
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

    Public Function GetChildren(ParentForm As Form, Optional IncludeParent As Boolean = False) As List(Of Form)
        Dim Children As New List(Of Form)
        If IncludeParent Then Children.Add(ParentForm)
        For Each frms As Form In My.Application.OpenForms
            If frms.Tag Is ParentForm Then
                Children.Add(frms)
            End If
        Next
        Return Children
    End Function

    Public Sub LookupDevice(ParentForm As MyForm, Device As Device_Info)
        If Device.strGUID IsNot Nothing Then
            If Not FormIsOpenByUID(GetType(ViewDeviceForm), Device.strGUID) Then
                Dim NewView As New ViewDeviceForm(ParentForm, Device.strGUID)
            End If
        Else
            Message("Device not found.", vbOKOnly + vbExclamation, "Error", ParentForm)
        End If
    End Sub

    Public Sub MinimizeChildren(ParentForm As Form)
        For Each chld As Form In GetChildren(ParentForm)
            chld.WindowState = FormWindowState.Minimized
        Next
    End Sub

    Public Sub RestoreChildren(ParentForm As Form)
        For Each chld As Form In GetChildren(ParentForm)
            chld.WindowState = FormWindowState.Normal
        Next
    End Sub

    Public Function SibiIsOpen() As Boolean
        If Application.OpenForms.OfType(Of SibiMainForm).Any Then
            Return True
        End If
        Return False
    End Function

    Public Function FormTypeIsOpen(FormType As Type) As Boolean
        For Each frm As Form In My.Application.OpenForms
            If frm.GetType = FormType Then Return True
        Next
        Return False
    End Function

    Public Function FormIsOpenByUID(FormType As Type, UID As String) As Boolean
        For Each frm As MyForm In My.Application.OpenForms
            If frm.GetType = FormType AndAlso frm.FormUID = UID Then
                ActivateFormByHandle(frm)
                Return True
            End If
        Next
        Return False
    End Function

End Module