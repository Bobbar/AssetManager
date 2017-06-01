Imports System.ComponentModel

Public Class GridForm
    Private GridList As New List(Of DataGridView)
    Private MyParent As MyForm
    Private bolGridFilling As Boolean = True
    Private LastDoubleClickRow As DataGridViewRow
    Private bolSelectMode As Boolean
    Public ReadOnly Property SelectedValue As DataGridViewRow
        Get
            Return LastDoubleClickRow
        End Get
    End Property
    Public ReadOnly Property GridCount As Integer
        Get
            Return GridList.Count
        End Get
    End Property
    Sub New(ParentForm As Form, Optional Title As String = "", Optional SelectMode As Boolean = False)
        MyParent = DirectCast(ParentForm, MyForm)
        Me.Tag = ParentForm
        Me.Icon = MyParent.Icon
        Me.GridTheme = MyParent.GridTheme
        bolSelectMode = SelectMode

        ' This call is required by the designer.
        InitializeComponent()
        If Title <> "" Then Me.Text = Title
        ' Add any initialization after the InitializeComponent() call.
        ' Me.Show()
        DoubleBufferedTableLayout(GridPanel, True)

        GridPanel.RowStyles.Clear()
    End Sub
    Public Sub AddGrid(Name As String, Label As String, Data As DataTable)
        Dim NewGrid = GetNewGrid(Name, Label)
        FillGrid(NewGrid, Data)
        GridList.Add(NewGrid)
    End Sub
    Private Function GetNewGrid(Name As String, Label As String) As DataGridView
        Dim NewGrid As New DataGridView
        NewGrid.Name = Name
        NewGrid.Tag = Label
        NewGrid.Dock = DockStyle.Fill
        NewGrid.DefaultCellStyle = GridStyles
        NewGrid.DefaultCellStyle.SelectionBackColor = Me.GridTheme.CellSelectColor
        NewGrid.RowHeadersVisible = False
        NewGrid.EditMode = DataGridViewEditMode.EditProgrammatically
        NewGrid.AllowUserToResizeRows = False
        NewGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        NewGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        NewGrid.AllowUserToAddRows = False
        NewGrid.AllowUserToDeleteRows = False
        NewGrid.Padding = New Padding(0, 0, 0, 10)
        AddHandler NewGrid.CellLeave, AddressOf GridLeaveCell
        AddHandler NewGrid.CellEnter, AddressOf GridEnterCell
        AddHandler NewGrid.CellDoubleClick, AddressOf GridDoubleClickCell
        DoubleBufferedDataGrid(NewGrid, True)
        Return NewGrid
    End Function
    Private Sub FillGrid(Grid As DataGridView, Data As DataTable)
        If Data IsNot Nothing Then Grid.DataSource = Data
    End Sub
    Private Sub DisplayGrids()
        For Each grid As DataGridView In GridList
            Dim GridBox As New GroupBox
            GridBox.Text = DirectCast(grid.Tag, String)
            GridBox.Dock = DockStyle.Fill
            GridBox.Controls.Add(grid)
            GridPanel.RowStyles.Add(New RowStyle(SizeType.Percent, Convert.ToSingle(100 / GridList.Count)))
            GridPanel.Controls.Add(GridBox)
        Next
        bolGridFilling = False
    End Sub
    Private Sub GridForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        DisplayGrids()
    End Sub
    Private Sub GridLeaveCell(sender As Object, e As DataGridViewCellEventArgs)
        LeaveRow(DirectCast(sender, DataGridView), Me.GridTheme, e.RowIndex)
    End Sub
    Private Sub GridEnterCell(sender As Object, e As DataGridViewCellEventArgs)
        If Not bolGridFilling Then
            HighlightRow(DirectCast(sender, DataGridView), Me.GridTheme, e.RowIndex)
        End If
    End Sub
    Private Sub GridDoubleClickCell(sender As Object, e As EventArgs)
        Dim SenderGrid As DataGridView = DirectCast(sender, DataGridView)
        LastDoubleClickRow = SenderGrid.CurrentRow
        Me.DialogResult = DialogResult.OK
    End Sub
    Private Sub GridForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Not Modal Then Me.Dispose()
    End Sub
End Class