Option Explicit On
Imports System.ComponentModel

Public Class UpdateDev

    Public ReadOnly Property UpdateInfo As DeviceUpdateInfoStruct
        Get
            Return NewUpdateInfo
        End Get
    End Property

    Private NewUpdateInfo As DeviceUpdateInfoStruct

    Sub New(parentForm As ExtendedForm, Optional isNoteOnly As Boolean = False)
        InitializeComponent()
        Me.ParentForm = parentForm
        FillComboBox(DeviceAttribute.ChangeType, UpdateTypeCombo)
        If isNoteOnly Then
            UpdateTypeCombo.SelectedIndex = GetComboIndexFromCode(DeviceAttribute.ChangeType, "NOTE")
            UpdateTypeCombo.Enabled = False
            ValidateUpdateType()
        Else
            UpdateTypeCombo.SelectedIndex = -1
        End If
        ShowDialog(parentForm)
    End Sub

    Private Sub SubmitButton_Click(sender As Object, e As EventArgs) Handles SubmitButton.Click
        NewUpdateInfo.Note = Trim(NotesTextBox.Rtf)
        NewUpdateInfo.ChangeType = GetDBValue(DeviceAttribute.ChangeType, UpdateTypeCombo.SelectedIndex)
        NotesTextBox.Text = ""
        UpdateTypeCombo.Enabled = True
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Function ValidateUpdateType() As Boolean
        If UpdateTypeCombo.SelectedIndex > 0 Then
            ErrorProvider1.SetError(UpdateTypeCombo, "")
            SubmitButton.Enabled = True
            Return True
        Else
            SubmitButton.Enabled = False
            UpdateTypeCombo.Focus()
            ErrorProvider1.SetError(UpdateTypeCombo, "Please select a change type.")
        End If
        Return False
    End Function

    Private Sub UpdateTypeCombo_ChangeType_Validating(sender As Object, e As CancelEventArgs) Handles UpdateTypeCombo.Validating
        e.Cancel = Not ValidateUpdateType()
    End Sub

    Private Sub UpdateTypeCombo_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles UpdateTypeCombo.SelectionChangeCommitted
        ValidateUpdateType()
    End Sub
End Class