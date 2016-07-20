Public Class frmNewRequest
    Private Sub frmNewRequest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ExtendedMethods.DoubleBuffered(RequestItemsGrid, True)
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
            .Add(DataGridCombo(Locations, "Location")) '.Add("Location")
            .Add(DataGridCombo(Sibi_ItemStatusType, "Status"))
            .Add("Replace Asset", "Replace Asset")
            .Add("Replace Serial", "Replace Serial")




        End With

        'RequestItemsGrid.DataSource = table
        'table.Dispose()


    End Sub
    Private Sub FillCombos()


        FillComboBox(Sibi_StatusType, cmbStatus)
        FillComboBox(Sibi_RequestType, cmbType)


    End Sub
    Private Function DataGridCombo(IndexType() As Combo_Data, HeaderText As String) As DataGridViewComboBoxColumn
        Dim tmpCombo As New DataGridViewComboBoxColumn
        tmpCombo.Items.Clear()
        tmpCombo.HeaderText = HeaderText
        tmpCombo.Name = "cmb" & HeaderText
        Dim i As Integer = 0
        For Each ComboItem As Combo_Data In IndexType
            tmpCombo.Items.Insert(i, ComboItem.strLong)
            i += 1
        Next
        Return tmpCombo
    End Function
End Class
