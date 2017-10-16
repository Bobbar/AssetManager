Public Class WindowList : Implements IDisposable

#Region "Fields"

    Private WithEvents RefreshTimer As Timer
    Private DropDownControl As New ToolStripDropDownButton
    Private intFormCount As Integer
    Private MyParentForm As ExtendedForm

#End Region

#Region "Constructors"

    Sub New(parentForm As ExtendedForm)
        MyParentForm = parentForm
    End Sub

#End Region

#Region "Methods"

    Public Sub InsertWindowList(targetToolStrip As OneClickToolStrip)
        InitializeDropDownButton(targetToolStrip)
        InitializeTimer()
        RefreshWindowList()
    End Sub

    Private Sub AddParentMenu()
        If MyParentForm.ParentForm IsNot Nothing Then
            Dim ParentDropDown As ToolStripMenuItem = NewMenuItem(MyParentForm.ParentForm)
            ParentDropDown.Text = "[Parent] " & ParentDropDown.Text
            ParentDropDown.ToolTipText = "Parent Form"
            DropDownControl.DropDownItems.Insert(0, ParentDropDown)
            DropDownControl.DropDownItems.Add(New ToolStripSeparator)
        End If
    End Sub

    ''' <summary>
    ''' Recursively build ToolStripItemCollections of Forms and their Children and add them to the ToolStrip. Making sure to add SibiMain to the top of the list.
    ''' </summary>
    ''' <param name="ParentForm">Form to add to ToolStrip.</param>
    ''' <param name="TargetMenuItem">Item to add the Form item to.</param>
    Private Sub BuildWindowList(parentForm As ExtendedForm, ByRef targetMenuItem As ToolStripItemCollection)
        For Each frm In GetChildren(parentForm)
            If HasChildren(frm) Then
                Dim NewDropDown As ToolStripMenuItem = NewMenuItem(frm)
                If TypeOf frm Is SibiMainForm Then
                    targetMenuItem.Insert(0, NewDropDown)
                Else
                    targetMenuItem.Add(NewDropDown)
                End If
                BuildWindowList(frm, NewDropDown.DropDownItems)
            Else
                If TypeOf frm Is SibiMainForm Then
                    targetMenuItem.Insert(0, NewMenuItem(frm))
                Else
                    targetMenuItem.Add(NewMenuItem(frm))
                End If
            End If
        Next
    End Sub

    Private Function GetFormFromTag(tag As Object) As Form
        If TypeOf tag Is Form Then
            Return DirectCast(tag, Form)
        End If
        Return Nothing
    End Function

    Private Function CountText(count As Integer) As String
        Dim MainText As String = "Select Window"
        If count > 0 Then
            Return MainText & " (" & count & ")"
        Else
            Return MainText
        End If
    End Function

    Private Function FormCount(parentForm As ExtendedForm) As Integer
        Dim i As Integer = 0
        For Each frm In GetChildren(parentForm)
            If Not frm.Modal And frm IsNot parentForm Then
                i += FormCount(frm) + 1
            End If
        Next
        Return i
    End Function

    Private Function HasChildren(parentForm As ExtendedForm) As Boolean
        Dim Children = GetChildren(parentForm)
        If Children.Count = 0 Then
            Return False
        Else
            For Each frm In Children
                If Not frm.IsDisposed Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Private Sub InitializeDropDownButton(targetToolStrip As OneClickToolStrip)
        DropDownControl.Visible = False
        DropDownControl.Font = targetToolStrip.Font
        DropDownControl.Text = "Select Window"
        DropDownControl.Image = AssetManager.My.Resources.Resources.CascadeIcon
        AddParentMenu()
        targetToolStrip.Items.Insert(targetToolStrip.Items.Count, DropDownControl)
    End Sub

    Private Sub InitializeTimer()
        RefreshTimer = New Timer
        RefreshTimer.Interval = 500
        RefreshTimer.Enabled = True
    End Sub

    Private Function NewMenuItem(frm As Form) As ToolStripMenuItem
        Dim newitem As New ToolStripMenuItem
        newitem.Font = DropDownControl.Font
        newitem.Text = frm.Text
        newitem.Image = frm.Icon.ToBitmap
        newitem.Tag = frm
        newitem.ToolTipText = "Right-Click to close."
        AddHandler newitem.MouseDown, AddressOf WindowClick
        Return newitem
    End Function

    Private Sub RefreshTimer_Tick(sender As Object, e As EventArgs) Handles RefreshTimer.Tick
        RefreshWindowList()
    End Sub

    Private Sub RefreshWindowList()
        Dim NumOfForms = FormCount(MyParentForm)
        If MyParentForm.ParentForm Is Nothing And NumOfForms < 1 Then
            DropDownControl.Visible = False
        Else
            DropDownControl.Visible = True
        End If
        If NumOfForms <> intFormCount Then
            If Not DropDownControl.DropDown.Focused Then
                DropDownControl.DropDownItems.Clear()
                AddParentMenu()
                BuildWindowList(MyParentForm, DropDownControl.DropDownItems)
                intFormCount = NumOfForms
                DropDownControl.Text = CountText(intFormCount)
            End If
        End If
    End Sub

    Private Sub WindowClick(sender As Object, e As MouseEventArgs)
        Dim item As ToolStripItem = CType(sender, ToolStripItem)
        Dim frm = CType(item.Tag, ExtendedForm)
        If e.Button = MouseButtons.Right Then
            If Not frm Is MyParentForm.ParentForm Then
                frm.Close()
                If frm.Disposing Or frm.IsDisposed Then
                    DisposeDropDownItem(item)
                End If
            End If
        ElseIf e.Button = MouseButtons.Left Then
            If Not frm.IsDisposed Then
                ActivateForm(frm)
            Else
                DisposeDropDownItem(item)
            End If
        End If
    End Sub

    Private Sub DisposeDropDownItem(item As ToolStripItem)
        If DropDownControl.DropDownItems.Count < 1 Then
            DropDownControl.Visible = False
            DropDownControl.DropDownItems.Clear()
            item.Dispose()
        Else
            item.Visible = False
            item.Dispose()
            DropDownControl.DropDownItems.Remove(item)
            intFormCount = FormCount(MyParentForm)
            DropDownControl.Text = CountText(intFormCount)
        End If
    End Sub

#End Region

#Region "IDisposable Support"

    Private disposedValue As Boolean ' To detect redundant calls

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                RefreshTimer.Dispose()
                DropDownControl.Dispose()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

#End Region

End Class