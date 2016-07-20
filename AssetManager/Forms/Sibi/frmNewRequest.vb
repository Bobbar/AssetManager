Public Class frmNewRequest
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtReqNumber.TextChanged

    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click

    End Sub

    Private Sub frmNewRequest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupGrid()
    End Sub
    Private Sub SetupGrid()
        ' Dim table As New DataTable


        Dim cmb As New DataGridViewComboBoxColumn
        cmb.HeaderText = "Location"
        cmb.Name = "cmb"
        'cmb.MaxDropDownItems = 4
        cmb.Items.Add("Here")
        cmb.Items.Add("There")
        cmb.Items.Add("That One Place")
        cmb.Items.Add("That Other Place")



        With RequestItemsGrid.Columns
            .Add("User", "User")
            .Add("Description", "Description")
            .Add(cmb) '.Add("Location")
            .Add("Status", "Status")
            .Add("Replace Asset", "Replace Asset")
            .Add("Replace Serial", "Replace Serial")




        End With

        'RequestItemsGrid.DataSource = table
        'table.Dispose()


    End Sub
End Class