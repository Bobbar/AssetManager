Option Explicit On
Imports MySql.Data.MySqlClient
Public Class AddNew
    Private Sub cmdAdd_Click(sender As Object, e As EventArgs) Handles cmdAdd.Click
        cn_global.Open()
        Dim strSqlQry1 = "INSERT INTO devices (dev_description,dev_location,dev_cur_user,dev_serial,dev_asset_tag,dev_purchase_date,dev_replacement_year) VALUES ('" & txtDescription.Text & "','" & GetShortLocation(cmbLocation.SelectedIndex) & "','" & txtCurUser.Text & "','" & txtSerial.Text & "','" & txtAssetTag.Text & "','" & dtPurchaseDate.Text & "','" & txtReplaceYear.Text & "')"
        Dim strSqlQry2 = "INSERT INTO devices (hist_change_type,hist_notes,hist_change_reason,hist_serial,hist_description,hist_location,hist_cur_user,hist_asset_tag,hist_purchase_date,hist_replacement_year,hist_po,hist_osversion) VALUES ('INIT','" & txtNotes.Text & "',' "

        Debug.Print(strSqlQry1)



        Dim cmd As New MySqlCommand
        cmd.Connection = cn_global
        cmd.CommandText = strSqlQry1
        cmd.ExecuteNonQuery()


        cn_global.Close()

    End Sub

    Private Sub cmbLocation_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLocation.SelectedIndexChanged

    End Sub

    Private Sub AddNew_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClearAll()



    End Sub
    Private Sub ClearAll()
        LoadCombos()
        txtSerial.Text = ""
        txtCurUser.Text = ""
        txtReplaceYear.Text = ""
        txtAssetTag.Text = ""
        txtDescription.Text = ""



    End Sub
    Private Sub LoadCombos()
        Dim i As Integer
        cmbLocation.Items.Clear()
        cmbLocation.Text = ""

        For i = 0 To UBound(Locations)
            cmbLocation.Items.Insert(i, Locations(i).strLocationLong)


        Next


    End Sub
End Class