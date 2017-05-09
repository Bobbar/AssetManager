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
            If DBInfo.ParseType <> ParseType.DisplayOnly Then
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
            End If
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
Public Class DBControlInfo
    Private db_column As String
    Private db_required As Boolean
    Private db_attrib_index As Combo_Data()
    Private db_parse_type As ParseType
    Public Property ParseType As ParseType
        Get
            Return db_parse_type
        End Get
        Set(value As ParseType)
            db_parse_type = value
        End Set
    End Property
    Public Property DataColumn As String
        Get
            Return db_column
        End Get
        Set(value As String)
            db_column = value
        End Set
    End Property
    Public Property Required As Boolean
        Get
            Return db_required
        End Get
        Set(value As Boolean)
            db_required = value
        End Set
    End Property
    Public Property AttribIndex As Combo_Data()
        Get
            Return db_attrib_index
        End Get
        Set(value As Combo_Data())
            db_attrib_index = value
        End Set
    End Property
    Sub New()
        db_column = ""
        db_required = False
        db_parse_type = ParseType.DisplayOnly
        db_attrib_index = Nothing
    End Sub
    Sub New(DataColumn As String, ParseType As ParseType, Optional Required As Boolean = False)
        db_column = DataColumn
        db_required = Required
        db_parse_type = ParseType
        db_attrib_index = Nothing
    End Sub
    Sub New(DataColumn As String, Optional Required As Boolean = False)
        db_column = DataColumn
        db_required = Required
        db_parse_type = ParseType.UpdateAndDisplay
        db_attrib_index = Nothing
    End Sub

    Sub New(DataColumn As String, AttribIndex As Combo_Data(), Optional Required As Boolean = False)
        db_column = DataColumn
        db_required = Required
        db_parse_type = ParseType.UpdateAndDisplay
        db_attrib_index = AttribIndex
    End Sub
End Class
''' <summary>
''' Determines how the parser handles the updating and filling of a control.
''' </summary>
Public Enum ParseType
    ''' <summary>
    ''' The control is filled only.
    ''' </summary>
    DisplayOnly
    ''' <summary>
    ''' The control is filled and will also be added to Update and Insert tables.
    ''' </summary>
    UpdateAndDisplay
End Enum