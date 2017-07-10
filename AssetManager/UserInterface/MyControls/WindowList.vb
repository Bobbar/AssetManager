Public Class WindowList : Implements IDisposable

#Region "Fields"

    Private WithEvents RefreshTimer As Timer
    Private DropDownControl As New ToolStripDropDownButton
    Private intFormCount As Integer
    Private MyParentForm As Form

#End Region

#Region "Constructors"

    Sub New(ParentForm As Form)
        MyParentForm = ParentForm
    End Sub

#End Region

#Region "Methods"

    Public Sub InsertWindowList(TargetToolStrip As MyToolStrip)
        InitializeDropDownButton(TargetToolStrip)
        InitializeTimer()
    End Sub
    Private Sub AddParentMenu()
        If MyParentForm.Tag IsNot Nothing Then
            Dim ParentDropDown As ToolStripMenuItem = NewMenuItem(GetFormFromTag(MyParentForm.Tag))
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
    Private Sub BuildWindowList(ParentForm As Form, ByRef TargetMenuItem As ToolStripItemCollection)
        For Each frm As Form In ListOfChilden(ParentForm)
            If HasChildren(frm) Then
                Dim NewDropDown As ToolStripMenuItem = NewMenuItem(frm)
                If TypeOf frm Is SibiMainForm Then
                    TargetMenuItem.Insert(0, NewDropDown)
                Else
                    TargetMenuItem.Add(NewDropDown)
                End If
                BuildWindowList(frm, NewDropDown.DropDownItems)
            Else
                If TypeOf frm Is SibiMainForm Then
                    TargetMenuItem.Insert(0, NewMenuItem(frm))
                Else
                    TargetMenuItem.Add(NewMenuItem(frm))
                End If
            End If
        Next
    End Sub
    Private Function GetFormFromTag(Tag As Object) As Form
        If TypeOf Tag Is Form Then
            Return DirectCast(Tag, Form)
        End If
    End Function
    Private Function CountText(count As Integer) As String
        Dim MainText As String = "Select Window"
        If count > 0 Then
            Return MainText & " (" & count & ")"
        Else
            Return MainText
        End If
    End Function

    Private Function FormCount(ParentForm As Form) As Integer
        Dim i As Integer = 0
        For Each frm As Form In My.Application.OpenForms
            If Not frm.IsDisposed And Not frm.Modal And frm IsNot ParentForm Then
                If frm.Tag Is ParentForm Then
                    i += FormCount(frm) + 1
                End If
            End If
        Next
        Return i
    End Function

    Private Function HasChildren(ParentForm As Form) As Boolean
        For Each frm As Form In My.Application.OpenForms
            If frm.Tag Is ParentForm And Not frm.IsDisposed Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub InitializeDropDownButton(TargetToolStrip As MyToolStrip)
        DropDownControl.Visible = False
        DropDownControl.Font = TargetToolStrip.Font
        DropDownControl.Text = "Select Window"
        DropDownControl.Image = AssetManager.My.Resources.Resources.CascadeIcon
        AddParentMenu()
        TargetToolStrip.Items.Insert(TargetToolStrip.Items.Count, DropDownControl)
    End Sub

    Private Sub InitializeTimer()
        RefreshTimer = New Timer
        RefreshTimer.Interval = 200
        RefreshTimer.Enabled = True
        AddHandler RefreshTimer.Tick, AddressOf RefreshTimer_Tick
    End Sub

    Private Function ListOfChilden(ParentForm As Form) As List(Of Form)
        Dim tmpList As New List(Of Form)
        For Each frm As Form In My.Application.OpenForms
            If frm.Tag Is ParentForm And Not frm.IsDisposed Then
                tmpList.Add(frm)
            End If
        Next
        Return tmpList
    End Function

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
    Private Function IsParentForm(Form As Form) As Boolean
        If MyParentForm.Tag IsNot Nothing Then
            If Form Is GetFormFromTag(MyParentForm.Tag) Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function
    Private Sub RefreshTimer_Tick(sender As Object, e As EventArgs) Handles RefreshTimer.Tick
        If MyParentForm.Tag Is Nothing And FormCount(MyParentForm) < 1 Then
            DropDownControl.Visible = False
        Else
            DropDownControl.Visible = True
        End If
        If FormCount(MyParentForm) <> intFormCount Then
            If Not DropDownControl.DropDown.Focused Then
                DropDownControl.DropDownItems.Clear()
                AddParentMenu()
                BuildWindowList(MyParentForm, DropDownControl.DropDownItems)
                intFormCount = FormCount(MyParentForm)
                DropDownControl.Text = CountText(intFormCount)
            End If
        End If
    End Sub
    Private Sub WindowClick(sender As Object, e As MouseEventArgs)
        Dim item As ToolStripItem = CType(sender, ToolStripItem)
        If e.Button = MouseButtons.Right Then
            Dim frm As Form = CType(item.Tag, Form)
            If Not IsParentForm(frm) Then
                frm.Dispose()
                GC.Collect()
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
            End If
        ElseIf e.Button = MouseButtons.Left Then
            ActivateFormByHandle(CType(item.Tag, Form))
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