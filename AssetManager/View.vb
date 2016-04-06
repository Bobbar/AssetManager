Option Explicit On

Imports MySql.Data.MySqlClient
Public Class View
    Private Structure UserInput
        Public strAssetTag As String
        Public strDescription As String
        Public intEqType As Integer
        Public strSerial As String
        Public intLocation As Integer
        Public strCurrentUser As String
        Public dtPurchaseDate As Date
        Public strReplaceYear As String


    End Structure
    Private OldData As UserInput
    Private NewData As UserInput




    Private Sub View_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Private Sub GetCurrentValues()
        OldData.strAssetTag = Trim(txtAssetTag_View.Text)
        OldData.strDescription = Trim(txtDescription_View.Text)
        OldData.intEqType = cmbEquipType_View.SelectedIndex
        OldData.strSerial = Trim(txtSerial_View.Text)
        OldData.intLocation = cmbLocation_View.SelectedIndex
        OldData.strCurrentUser = Trim(txtCurUser_View.Text)
        OldData.dtPurchaseDate = dtPurchaseDate_View.Value
        OldData.strReplaceYear = Trim(txtReplacementYear_View.Text)



    End Sub
    Private Sub EnableControls()
        Dim c As Control

        For Each c In pnlViewControls.Controls

            If TypeOf c IsNot Label Then c.Enabled = True


        Next
        cmdUpdate.Visible = True


    End Sub
    Private Sub DisableControls()
        Dim c As Control

        For Each c In pnlViewControls.Controls

            If TypeOf c IsNot Label Then c.Enabled = False


        Next


        cmdUpdate.Visible = False


    End Sub



    Public Sub ViewDevice(ByVal strAssetTag As String)
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        cn_global.Open()
        Dim strQry = "SELECT * FROM devices, historical WHERE dev_UID = hist_dev_UID AND dev_asset_tag = '" & strAssetTag & "'"
        Debug.Print(strQry)
        Dim cmd As New MySqlCommand(strQry, cn_global)
        reader = cmd.ExecuteReader
        table.Columns.Add("Action Type", GetType(String))
        table.Columns.Add("User", GetType(String))
        table.Columns.Add("Asset ID", GetType(String))
        table.Columns.Add("Serial", GetType(String))
        table.Columns.Add("Description", GetType(String))
        table.Columns.Add("Location", GetType(String))
        table.Columns.Add("Purchase Date", GetType(String))
        With reader
            Do While .Read()
                Debug.Print(!dev_description)
                txtAssetTag_View.Text = !dev_asset_tag
                txtDescription_View.Text = !dev_description
                cmbEquipType_View.SelectedIndex = GetComboIndexFromShort(ComboType.EquipType,!dev_eq_type)
                txtSerial_View.Text = !dev_serial
                cmbLocation_View.SelectedIndex = GetComboIndexFromShort(ComboType.Location,!dev_location)
                txtCurUser_View.Text = !dev_cur_user
                dtPurchaseDate_View.Value = !dev_purchase_date
                txtReplacementYear_View.Text = !dev_replacement_year
                table.Rows.Add(GetHumanValue(ComboType.ChangeType,!hist_change_type),!dev_cur_user,!dev_asset_tag,!dev_serial,!dev_description,!dev_location,!dev_purchase_date)
            Loop
        End With
        DataGridHistory.DataSource = table
        cn_global.Close()


    End Sub
    Private Sub ModifyDevice()
        GetCurrentValues()
        EnableControls()



    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        EnableControls()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        DisableControls()

    End Sub

    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click

    End Sub
End Class