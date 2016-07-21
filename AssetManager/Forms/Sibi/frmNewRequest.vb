Public Class frmNewRequest
    Private Sub frmNewRequest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ExtendedMethods.DoubleBuffered(RequestItemsGrid, True)
        FillCombos()
        SetupGrid()
    End Sub
    Private Sub SetupGrid()
        Dim table As New DataTable


        'Dim cmb As New DataGridViewComboBoxColumn
        'cmb.HeaderText = "Location"
        'cmb.Name = "cmb"

        'cmb.Items.Add("Here")
        'cmb.Items.Add("There")
        'cmb.Items.Add("That One Place")
        'cmb.Items.Add("That Other Place")


        RequestItemsGrid.DataSource = table
        With RequestItemsGrid.Columns
            .Add("User", "User")
            .Add("Description", "Description")
            .Add(DataGridCombo(Locations, "Location")) '.Add("Location")
            .Add(DataGridCombo(Sibi_ItemStatusType, "Status"))
            .Add("Replace Asset", "Replace Asset")
            .Add("Replace Serial", "Replace Serial")




        End With


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
    Private Sub AddNewRequest()



    End Sub
    Private Function CollectData() As Request_Info
        Dim info As Request_Info
        Dim GridTable = TryCast(RequestItemsGrid.DataSource, DataTable)
        With info
            .strDescription = Trim(txtDescription.Text)
            .strUser = Trim(txtUser.Text)
            .strType = GetDBValue(Sibi_RequestType, cmbType.SelectedIndex)
            .dtNeedBy = dtNeedBy.Value.ToString(strDBDateFormat)
            .strStatus = GetDBValue(Sibi_StatusType, cmbStatus.SelectedIndex)
            .strPO = Trim(txtPO.Text)
            .strRequisitionNumber = Trim(txtReqNumber.Text)
            .RequstItems = GridTable


        End With

        Dim r As DataRow
        For Each r In info.RequstItems.Rows
            Debug.Print(r.Item("Description").ToString)
        Next


        Return info
    End Function

    Private Sub cmdAddNew_Click(sender As Object, e As EventArgs) Handles cmdAddNew.Click
        CollectData()
    End Sub
End Class
