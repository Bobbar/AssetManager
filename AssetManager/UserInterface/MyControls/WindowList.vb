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
    Private Sub RefreshWindowList(ParentForm As Form, ByRef TargetMenuItem As ToolStripItemCollection)
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
        Dim newitem As New ToolStripMenuItem
        newitem.Text = frm.Text
        newitem.Image = frm.Icon.ToBitmap
        newitem.Tag = frm
        newitem.ToolTipText = "Right-Click to close."
        AddHandler newitem.MouseDown, AddressOf WindowClick
        Return newitem
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
