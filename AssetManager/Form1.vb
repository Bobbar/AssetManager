Option Explicit On
Imports System.ComponentModel
Imports MySql.Data.MySqlClient


Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Clear_All()
        DBConnect()
        BuildLocationIndex()

    End Sub
    Private Sub Clear_All()
        txtAssetTag.Text = ""
        txtSerial.Text = ""

    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        cn_global.Close()

    End Sub

    Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        'Dim cmd As New MySqlCommand
        'Dim ds As New DataSet
        'Dim adapter As New MySqlDataAdapter(strGetDevices, cn_global)
        Dim reader As MySqlDataReader

        Dim table As New DataTable
        cn_global.Open()
        Dim strGetDevices = "SELECT * FROM devices WHERE dev_serial LIKE '%" & txtSerial.Text & "%'"

        Dim cmd As New MySqlCommand(strGetDevices, cn_global)





        reader = cmd.ExecuteReader

        table.Columns.Add("User", GetType(String))
        table.Columns.Add("Asset ID", GetType(String))
        table.Columns.Add("Serial", GetType(String))
        table.Columns.Add("Description", GetType(String))
        table.Columns.Add("Location", GetType(String))
        table.Columns.Add("Purchase Date", GetType(String))

        With reader
            Do While .Read()
                Debug.Print(!dev_description)
                table.Rows.Add(!dev_cur_user,!dev_asset_tag,!dev_serial,!dev_description,!dev_location,!dev_purchase_date)


            Loop




        End With

        ResultGrid.DataSource = table


        cn_global.Close()


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        BuildLocationIndex()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AddNew.Show()

    End Sub

    Private Sub ResultGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellContentClick

    End Sub

    Private Sub ResultGrid_DoubleClick(sender As Object, e As EventArgs) Handles ResultGrid.DoubleClick
        Debug.Print("DOUBLE CLICK!")
    End Sub
End Class
