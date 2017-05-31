Public Class DBControlParser
    ''' <summary>
    ''' Collects a DataTable via a SQL Select statement and modifies the topmost row with data parsed from <seealso cref="UpdateDBControlRow(Form, ByRef DataRow)"/>
    ''' </summary>
    ''' <remarks>
    ''' The SQL SELECT statement should return a single row only. This is will be the table row that you wish to update.
    ''' </remarks>
    ''' <param name="ParentForm">This form and its containing controls will be traveresd recursively via <see cref="GetDBControls(Control, ByRef List(Of Control))"/> </param>
    ''' <param name="SelectQry">A SQL Select query string that will return the table row that is to be updated.</param>
    ''' <returns>
    ''' Returns a DataTable modified by the controls identified by <see cref="GetDBControls(Control, ByRef List(Of Control))"/>
    ''' </returns>
    Public Function ReturnUpdateTable(ParentForm As Form, SelectQry As String) As DataTable
        Dim tmpTable As New DataTable
        Using SQLComm As New clsMySQL_Comms
            tmpTable = SQLComm.Return_SQLTable(SelectQry)
        End Using
        tmpTable.TableName = "UpdateTable"
        UpdateDBControlRow(ParentForm, tmpTable.Rows(0))
        Return tmpTable
    End Function
    ''' <summary>
    ''' Collects an EMPTY DataTable via a SQL Select statement and adds a new row for SQL Insertion.
    ''' </summary>
    ''' <remarks>
    ''' The SQL SELECT statement should return an EMPTY table. A new row will be added to this table via <see cref="UpdateDBControlRow(Form, ByRef DataRow)"/>
    ''' </remarks>
    ''' <param name="ParentForm">Form that contains controls initiated with <see cref="DBControlInfo"/> </param>
    ''' <param name="SelectQry">A SQL Select query string that will return an EMPTY table. Ex: "SELECT * FROM table LIMIT 0"</param>
    ''' <returns>
    ''' Returns a DataTable with a new row added via <see cref="UpdateDBControlRow(Form, ByRef DataRow)"/>
    ''' </returns>
    Public Function ReturnInsertTable(ParentForm As Form, SelectQry As String) As DataTable
        Dim tmpTable As DataTable
        Using SQLComm As New clsMySQL_Comms
            tmpTable = SQLComm.Return_SQLTable(SelectQry)
        End Using
        tmpTable.Rows.Add()
        UpdateDBControlRow(ParentForm, tmpTable.Rows(0))
        Return tmpTable
    End Function
    ''' <summary>
    ''' Modifies a DataRow with data parsed from controls collected by <see cref="GetDBControls(Control, ByRef List(Of Control))"/>
    ''' </summary>
    ''' <param name="ParentForm">Form that contains controls initiated with <see cref="DBControlInfo"/> </param>
    ''' <param name="DBRow">DataRow to be modified.</param>
    Private Sub UpdateDBControlRow(ParentForm As Form, ByRef DBRow As DataRow)
        Dim DBCtlList As New List(Of Control)
        GetDBControls(ParentForm, DBCtlList)
        For Each ctl As Control In DBCtlList
            Dim DBInfo As DBControlInfo = DirectCast(ctl.Tag, DBControlInfo)
            If DBInfo.ParseType <> ParseType.DisplayOnly Then
                Select Case True
                    Case TypeOf ctl Is TextBox
                        Dim dbTxt As TextBox = DirectCast(ctl, TextBox)
                        DBRow(DBInfo.DataColumn) = CleanDBValue(dbTxt.Text)

                    Case TypeOf ctl Is MaskedTextBox
                        Dim dbMaskTxt As MaskedTextBox = DirectCast(ctl, MaskedTextBox)
                        DBRow(DBInfo.DataColumn) = CleanDBValue(dbMaskTxt.Text)

                    Case TypeOf ctl Is DateTimePicker
                        Dim dbDtPick As DateTimePicker = DirectCast(ctl, DateTimePicker)
                        DBRow(DBInfo.DataColumn) = dbDtPick.Value

                    Case TypeOf ctl Is ComboBox
                        Dim dbCmb As ComboBox = DirectCast(ctl, ComboBox)
                        DBRow(DBInfo.DataColumn) = GetDBValue(DBInfo.AttribIndex, dbCmb.SelectedIndex)

                    Case TypeOf ctl Is CheckBox
                        Dim dbChk As CheckBox = DirectCast(ctl, CheckBox)
                        DBRow(DBInfo.DataColumn) = dbChk.Checked
                End Select
            End If
        Next
    End Sub
    ''' <summary>
    ''' Recursively collects list of controls initiated with <see cref="DBControlInfo"/> tags within Parent control.
    ''' </summary>
    ''' <param name="Parent">Parent control. Usually a Form to being.</param>
    ''' <param name="ControlList">Blank List of Control to be filled.</param>
    Private Sub GetDBControls(Parent As Control, ByRef ControlList As List(Of Control))
        For Each ctl As Control In Parent.Controls
            Select Case True
                Case TypeOf ctl.Tag Is DBControlInfo
                    ControlList.Add(ctl)
            End Select
            If ctl.HasChildren Then GetDBControls(ctl, ControlList)
        Next
    End Sub
    ''' <summary>
    ''' Populates all Controls in the ParentForm that have been initiated via <see cref="DBControlInfo"/> with their corresponding column names.
    ''' </summary>
    ''' <param name="ParentForm">Form that contains controls initiated with <see cref="DBControlInfo"/></param>
    ''' <param name="Data">DataTable that contains the rows and columns associated with the controls.</param>
    Public Sub FillDBFields(ParentForm As Form, Data As DataTable)
        Dim DBCtlList As New List(Of Control)
        GetDBControls(ParentForm, DBCtlList)
        Dim Row As DataRow = Data.Rows(0)
        For Each ctl As Control In DBCtlList
            Dim DBInfo As DBControlInfo = DirectCast(ctl.Tag, DBControlInfo)
            Select Case True
                Case TypeOf ctl Is TextBox
                    Dim dbTxt As TextBox = DirectCast(ctl, TextBox)
                    If DBInfo.AttribIndex IsNot Nothing Then
                        dbTxt.Text = GetHumanValue(DBInfo.AttribIndex, Row.Item(DBInfo.DataColumn).ToString)
                    Else
                        dbTxt.Text = Row.Item(DBInfo.DataColumn).ToString
                    End If
                Case TypeOf ctl Is MaskedTextBox
                    Dim dbMaskTxt As MaskedTextBox = DirectCast(ctl, MaskedTextBox)
                    dbMaskTxt.Text = Row.Item(DBInfo.DataColumn).ToString

                Case TypeOf ctl Is DateTimePicker
                    Dim dbDtPick As DateTimePicker = DirectCast(ctl, DateTimePicker)
                    dbDtPick.Value = DateTime.Parse(Row.Item(DBInfo.DataColumn).ToString)
                Case TypeOf ctl Is ComboBox
                    Dim dbCmb As ComboBox = DirectCast(ctl, ComboBox)
                    dbCmb.SelectedIndex = GetComboIndexFromShort(DBInfo.AttribIndex, Row.Item(DBInfo.DataColumn).ToString)

                Case TypeOf ctl Is Label
                    Dim dbLbl As Label = DirectCast(ctl, Label)
                    dbLbl.Text = Row.Item(DBInfo.DataColumn).ToString

                Case TypeOf ctl Is CheckBox
                    Dim dbChk As CheckBox = DirectCast(ctl, CheckBox)
                    dbChk.Checked = CBool(Row.Item(DBInfo.DataColumn))
            End Select

        Next

    End Sub
End Class

''' <summary>
''' Instantiate and assign to <see cref="Control.Tag"/> property to enable DBParsing functions.
''' </summary>
Public Class DBControlInfo
    Private db_column As String
    Private db_required As Boolean
    Private db_attrib_index As Combo_Data()
    Private db_parse_type As ParseType
    ''' <summary>
    ''' Gets or sets <seealso cref="ParseType"/>
    ''' </summary>
    ''' <returns></returns>
    Public Property ParseType As ParseType
        Get
            Return db_parse_type
        End Get
        Set(value As ParseType)
            db_parse_type = value
        End Set
    End Property
    ''' <summary>
    ''' Gets or sets the Database Column used to update and/or populate the assigned control.
    ''' </summary>
    ''' <returns></returns>
    Public Property DataColumn As String
        Get
            Return db_column
        End Get
        Set(value As String)
            db_column = value
        End Set
    End Property
    ''' <summary>
    ''' Is the Control a required field?
    ''' </summary>
    ''' <returns></returns>
    Public Property Required As Boolean
        Get
            Return db_required
        End Get
        Set(value As Boolean)
            db_required = value
        End Set
    End Property
    ''' <summary>
    ''' Gets or sets the <see cref="Combo_Data"/> index for <see cref="ComboBox"/> controls.
    ''' </summary>
    ''' <returns></returns>
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
    Sub New(DataColumn As String, AttribIndex As Combo_Data(), ParseType As ParseType, Optional Required As Boolean = False)
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