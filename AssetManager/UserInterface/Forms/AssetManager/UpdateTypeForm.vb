Option Explicit On

Public Class UpdateDev

    Public ReadOnly Property UpdateInfo As DeviceUpdateInfoStruct
        Get
            Return NewUpdateInfo
        End Get
    End Property

    Private NewUpdateInfo As DeviceUpdateInfoStruct

    Sub New(ParentForm As Form, Optional bolNewNote As Boolean = False)
        InitializeComponent()
        Icon = ParentForm.Icon
        Me.Tag = ParentForm
        FillComboBox(DeviceIndex.ChangeType, cmbUpdate_ChangeType)
        If bolNewNote Then
            cmbUpdate_ChangeType.SelectedIndex = GetComboIndexFromShort(DeviceIndex.ChangeType, "NOTE")
            cmbUpdate_ChangeType.Enabled = False
        Else
            cmbUpdate_ChangeType.SelectedIndex = -1
        End If
        ShowDialog(ParentForm)
    End Sub

    Private Sub cmdSubmit_Click(sender As Object, e As EventArgs) Handles cmdSubmit.Click
        If Not CheckFields() Then
            Message("Please select a change type.", vbOKOnly + vbExclamation, "Missing Field", Me)
            Exit Sub
        End If
        NewUpdateInfo.Note = Trim(txtUpdate_Note.Text)
        NewUpdateInfo.ChangeType = GetDBValue(DeviceIndex.ChangeType, cmbUpdate_ChangeType.SelectedIndex)
        txtUpdate_Note.Text = ""
        cmbUpdate_ChangeType.Enabled = True
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Function CheckFields() As Boolean
        If cmbUpdate_ChangeType.SelectedIndex = -1 Then
            Return False
        Else
            Return True
        End If
    End Function

End Class