Imports System.ComponentModel

Public Class GridForm

#Region "Fields"

    Private bolGridFilling As Boolean = True
    Private GridList As New List(Of DataGridView)
    Private LastDoubleClickRow As DataGridViewRow

#End Region

#Region "Constructors"

    Sub New(parentForm As ExtendedForm, Optional title As String = "")
        Me.ParentForm = parentForm
        ' This call is required by the designer.
        InitializeComponent()
        If title <> "" Then Me.Text = title
        ' Add any initialization after the InitializeComponent() call.
        DoubleBufferedTableLayout(GridPanel, True)
        DoubleBufferedPanel(Panel1, True)
        GridPanel.RowStyles.Clear()
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property GridCount As Integer
        Get
            Return GridList.Count
        End Get
    End Property

    Public ReadOnly Property SelectedValue As DataGridViewRow
        Get
            Return LastDoubleClickRow
        End Get
    End Property

#End Region

#Region "Methods"

    Public Sub AddGrid(name As String, label As String, datatable As DataTable)
        Dim NewGrid = GetNewGrid(name, label & " (" & datatable.Rows.Count.ToString & " rows)")
        FillGrid(NewGrid, datatable)
        GridList.Add(NewGrid)
    End Sub

    Private Sub CopySelectedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopySelectedToolStripMenuItem.Click
        GridFunctions.CopySelectedGridData(GetActiveGrid)
    End Sub

    Private Sub AddGridsToForm()
        Me.SuspendLayout()
        Panel1.SuspendLayout()
        GridPanel.SuspendLayout()
        For Each grid As DataGridView In GridList
            Dim GridBox As New GroupBox
            GridBox.Text = DirectCast(grid.Tag, String)
            GridBox.Dock = DockStyle.Fill
            GridBox.Controls.Add(grid)
            GridPanel.RowStyles.Add(New RowStyle(SizeType.Absolute, GridHeight))
            GridPanel.Controls.Add(GridBox)
        Next
        Me.ResumeLayout()
        Panel1.ResumeLayout()
        GridPanel.ResumeLayout()
    End Sub

    Private Sub FillGrid(grid As DataGridView, datatable As DataTable)
        If datatable IsNot Nothing Then grid.DataSource = datatable
    End Sub

    Private Function GetActiveGrid() As DataGridView
        If TypeOf Me.ActiveControl Is DataGridView Then
            Return DirectCast(Me.ActiveControl, DataGridView)
        End If
        Return Nothing
    End Function

    Private Function GetNewGrid(name As String, label As String) As DataGridView
        Dim NewGrid As New DataGridView
        NewGrid.Name = name
        NewGrid.Tag = label
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
        NewGrid.ContextMenuStrip = PopUpMenu
        AddHandler NewGrid.CellLeave, AddressOf GridLeaveCell
        AddHandler NewGrid.CellEnter, AddressOf GridEnterCell
        AddHandler NewGrid.CellDoubleClick, AddressOf GridDoubleClickCell
        DoubleBufferedDataGrid(NewGrid, True)
        Return NewGrid
    End Function
    Private Sub GridDoubleClickCell(sender As Object, e As EventArgs)
        Dim SenderGrid As DataGridView = DirectCast(sender, DataGridView)
        LastDoubleClickRow = SenderGrid.CurrentRow
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub GridEnterCell(sender As Object, e As DataGridViewCellEventArgs)
        If Not bolGridFilling Then
            HighlightRow(DirectCast(sender, DataGridView), Me.GridTheme, e.RowIndex)
        End If
    End Sub

    Private Sub GridForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Not Modal Then Me.Dispose()
    End Sub

    Private Sub GridForm_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Not bolGridFilling Then ResizeGridPanel()
    End Sub

    Private Function GridHeight() As Integer
        Dim MinHeight As Integer = 200
        Dim CalcHeight As Integer = CInt((Me.ClientSize.Height - 30) / GridList.Count)
        If CalcHeight < MinHeight Then
            Return MinHeight
        Else
            Return CalcHeight
        End If
    End Function
    Private Sub GridLeaveCell(sender As Object, e As DataGridViewCellEventArgs)
        LeaveRow(DirectCast(sender, DataGridView), Me.GridTheme, e.RowIndex)
    End Sub
    Private Sub ResizeGridPanel()
        Dim NewHeight = GridHeight()
        For Each grid In GridList
            Dim row = GridList.IndexOf(grid)
            GridPanel.RowStyles(row).Height = NewHeight
        Next
    End Sub

    Private Sub ResizeGrids()
        For Each grid In GridList
            For Each c As DataGridViewColumn In grid.Columns
                c.Width = c.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, True)
            Next
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            grid.AllowUserToResizeColumns = True
        Next
    End Sub

    Private Sub SendToNewGridForm_Click(sender As Object, e As EventArgs) Handles SendToNewGridForm.Click
        GridFunctions.CopyToGridForm(GetActiveGrid, ParentForm)
    End Sub

    Private Sub GridForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        For Each grid In GridList
            DirectCast(grid.DataSource, DataTable).Dispose()
            grid.Dispose()
        Next
        If LastDoubleClickRow IsNot Nothing Then LastDoubleClickRow.Dispose()
    End Sub

    Private Sub GridForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        AddGridsToForm()
        ResizeGrids()
        bolGridFilling = False
    End Sub
#End Region

End Class