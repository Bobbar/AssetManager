Public Class DBControlParser
    Public Function ReturnUpdateTable(ParentForm As Form, SelectQry As String) As DataTable
        Dim tmpTable As New DataTable
        Using SQLComm As New clsMySQL_Comms
            tmpTable = SQLComm.Return_SQLTable(SelectQry)
        End Using
        tmpTable.TableName = "UpdateTable"
        Dim DBRow = GetDBControlRow(ParentForm, tmpTable.Rows(0))
        Return tmpTable
    End Function
    Public Function ReturnInsertTable(ParentForm As Form, SelectQry As String) As DataTable
        Dim tmpTable As DataTable
        Using SQLComm As New clsMySQL_Comms
            tmpTable = SQLComm.Return_SQLTable(SelectQry)
        End Using
        tmpTable.Rows.Add()
        Dim DBRow = GetDBControlRow(ParentForm, tmpTable.Rows(0))
        Return tmpTable
    End Function
    Private Function GetDBControlRow(ParentForm As Form, ByRef DBRow As DataRow) As DataRow
        Dim DBCtlList As New List(Of Control)
        GetDBControls(ParentForm, DBCtlList)
        For Each ctl As Control In DBCtlList
            Dim DBInfo As DBControlInfo = DirectCast(ctl.Tag, DBControlInfo)
            Select Case True
                Case TypeOf ctl Is TextBox
                    Dim dbTxt As TextBox = ctl
                    DBRow(DBInfo.DataColumn) = Trim(dbTxt.Text)

                Case TypeOf ctl Is MaskedTextBox
                    Dim dbMaskTxt As MaskedTextBox = ctl
                    DBRow(DBInfo.DataColumn) = dbMaskTxt.Text

                Case TypeOf ctl Is DateTimePicker
                    Dim dbDtPick As DateTimePicker = ctl
                    DBRow(DBInfo.DataColumn) = dbDtPick.Value

                Case TypeOf ctl Is ComboBox
                    Dim dbCmb As ComboBox = ctl
                    DBRow(DBInfo.DataColumn) = GetDBValue(DBInfo.AttribIndex, dbCmb.SelectedIndex)

                Case TypeOf ctl Is CheckBox
                    Dim dbChk As CheckBox = ctl
                    DBRow(DBInfo.DataColumn) = dbChk.Checked
            End Select
        Next
        Return DBRow
    End Function
    Private Sub GetDBControls(Parent As Control, ByRef ControlList As List(Of Control))
        For Each ctl As Control In Parent.Controls
            Select Case True
                Case TypeOf ctl.Tag Is DBControlInfo
                    ControlList.Add(ctl)
            End Select
            If ctl.HasChildren Then GetDBControls(ctl, ControlList)
        Next
    End Sub
    Public Sub FillDBFields(ParentForm As Form, Data As DataTable)
        Dim DBCtlList As New List(Of Control)
        GetDBControls(ParentForm, DBCtlList)
        Dim Row As DataRow = Data.Rows(0)
        For Each ctl As Control In DBCtlList
            Dim DBInfo As DBControlInfo = DirectCast(ctl.Tag, DBControlInfo)
            Select Case True
                Case TypeOf ctl Is TextBox
                    Dim dbTxt As TextBox = ctl
                    dbTxt.Text = Row.Item(DBInfo.DataColumn).ToString

                Case TypeOf ctl Is MaskedTextBox
                    Dim dbMaskTxt As MaskedTextBox = ctl
                    dbMaskTxt.Text = Row.Item(DBInfo.DataColumn).ToString

                Case TypeOf ctl Is DateTimePicker
                    Dim dbDtPick As DateTimePicker = ctl
                    dbDtPick.Value = Row.Item(DBInfo.DataColumn)
                Case TypeOf ctl Is ComboBox
                    Dim dbCmb As ComboBox = ctl
                    dbCmb.SelectedIndex = GetComboIndexFromShort(DBInfo.AttribIndex, Row.Item(DBInfo.DataColumn))

                Case TypeOf ctl Is Label
                    Dim dbLbl As Label = ctl
                    dbLbl.Text = Row.Item(DBInfo.DataColumn).ToString

                Case TypeOf ctl Is CheckBox
                    Dim dbChk As CheckBox = ctl
                    dbChk.Checked = Row.Item(DBInfo.DataColumn)
            End Select

        Next

    End Sub
End Class
