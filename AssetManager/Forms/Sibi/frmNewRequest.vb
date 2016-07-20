Public Class frmNewRequest
    Private cmbDataGridLocation As New DataGridViewComboBoxColumn
    Private cmbDataItemGridStatus As New DataGridViewComboBoxColumn
    Private Sub frmNewRequest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FillCombos()
        SetupGrid()
    End Sub
    Private Sub SetupGrid()
        ' Dim table As New DataTable


        'Dim cmb As New DataGridViewComboBoxColumn
        'cmb.HeaderText = "Location"
        'cmb.Name = "cmb"

        'cmb.Items.Add("Here")
        'cmb.Items.Add("There")
        'cmb.Items.Add("That One Place")
        'cmb.Items.Add("That Other Place")



        With RequestItemsGrid.Columns
            .Add("User", "User")
            .Add("Description", "Description")
            .Add(cmbDataGridLocation) '.Add("Location")
            .Add("Status", "Status")
            .Add("Replace Asset", "Replace Asset")
            .Add("Replace Serial", "Replace Serial")




        End With

        'RequestItemsGrid.DataSource = table
        'table.Dispose()


    End Sub
    Private Sub FillCombos()
        FillLocationCombo()
        FillStatusCombo()
        FillRequestTypeCombo()

        FillItemStatusCombo()

    End Sub
    Private Sub FillLocationCombo()
        Dim i As Integer
        cmbDataGridLocation.Items.Clear()
        cmbDataGridLocation.HeaderText = "Location"
        cmbDataGridLocation.Name = "cmbDataGridLocation"
        For i = 0 To UBound(Locations)
            cmbDataGridLocation.Items.Insert(i, Locations(i).strLong)
        Next
    End Sub
    Private Sub FillStatusCombo()
        Dim i As Integer

        cmbStatus.Items.Clear()
        cmbStatus.Text = ""


        For i = 0 To UBound(Sibi_StatusType)
            cmbStatus.Items.Insert(i, Sibi_StatusType(i).strLong)

        Next
    End Sub
    Private Sub FillRequestTypeCombo()
        Dim i As Integer

        cmbType.Items.Clear()
        cmbType.Text = ""


        For i = 0 To UBound(Sibi_RequestType)
            cmbType.Items.Insert(i, Sibi_RequestType(i).strLong)

        Next
    End Sub
    Private Sub FillItemStatusCombo()
        Dim i As Integer
        cmbDataItemGridStatus.Items.Clear()
        cmbDataItemGridStatus.HeaderText = "Status"
        cmbDataItemGridStatus.Name = "cmbDataGridItemStatus"


        For i = 0 To UBound(Sibi_ItemStatusType)

            cmbDataItemGridStatus.Items.Insert(i, Sibi_ItemStatusType(i).strLong)
        Next
    End Sub


End Class