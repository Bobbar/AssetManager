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

''' <summary>
''' Instantiate and assign to <see cref="Control.Tag"/> property to enable DBParsing functions.
''' </summary>
Public Class DBControlInfo

#Region "Fields"

    Private db_attrib_index As ComboboxDataStruct()
    Private db_column As String
    Private db_parse_type As ParseType
    Private db_required As Boolean

#End Region

#Region "Constructors"

    Sub New()
        db_column = ""
        db_required = False
        db_parse_type = ParseType.DisplayOnly
        db_attrib_index = Nothing
    End Sub

    Sub New(dataColumn As String, parseType As ParseType, Optional required As Boolean = False)
        db_column = dataColumn
        db_required = required
        db_parse_type = parseType
        db_attrib_index = Nothing
    End Sub

    Sub New(dataColumn As String, Optional required As Boolean = False)
        db_column = dataColumn
        db_required = required
        db_parse_type = ParseType.UpdateAndDisplay
        db_attrib_index = Nothing
    End Sub

    Sub New(dataColumn As String, attribIndex As ComboboxDataStruct(), Optional required As Boolean = False)
        db_column = dataColumn
        db_required = required
        db_parse_type = ParseType.UpdateAndDisplay
        db_attrib_index = attribIndex
    End Sub

    Sub New(dataColumn As String, attribIndex As ComboboxDataStruct(), parseType As ParseType, Optional required As Boolean = False)
        db_column = dataColumn
        db_required = required
        db_parse_type = parseType
        db_attrib_index = attribIndex
    End Sub

#End Region

#Region "Properties"

    ''' <summary>
    ''' Gets or sets the <see cref="ComboboxDataStruct"/> index for <see cref="ComboBox"/> controls.
    ''' </summary>
    ''' <returns></returns>
    Public Property AttribIndex As ComboboxDataStruct()
        Get
            Return db_attrib_index
        End Get
        Set(value As ComboboxDataStruct())
            db_attrib_index = value
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

#End Region

End Class

Public Class DBControlParser

#Region "Fields"

    Private ParentForm As Form

#End Region

#Region "Constructors"

    ''' <summary>
    ''' Instantiate new instance of <see cref="DBControlParser"/>
    ''' </summary>
    ''' <param name="parentForm">Form that contains controls initiated with <see cref="DBControlInfo"/> </param>
    Sub New(parentForm As Form)
        Me.ParentForm = parentForm
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Populates all Controls in the ParentForm that have been initiated via <see cref="DBControlInfo"/> with their corresponding column names.
    ''' </summary>
    ''' <param name="data">DataTable that contains the rows and columns associated with the controls.</param>
    Public Sub FillDBFields(data As DataTable)
        Dim Row As DataRow = data.Rows(0)
        For Each ctl As Control In GetDBControls(ParentForm)
            Dim DBInfo As DBControlInfo = DirectCast(ctl.Tag, DBControlInfo)
            Select Case True
                Case TypeOf ctl Is TextBox
                    Dim dbTxt As TextBox = DirectCast(ctl, TextBox)
                    If DBInfo.AttribIndex IsNot Nothing Then
                        dbTxt.Text = GetDisplayValueFromCode(DBInfo.AttribIndex, Row.Item(DBInfo.DataColumn).ToString)
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
                    dbCmb.SelectedIndex = GetComboIndexFromCode(DBInfo.AttribIndex, Row.Item(DBInfo.DataColumn).ToString)

                Case TypeOf ctl Is Label
                    Dim dbLbl As Label = DirectCast(ctl, Label)
                    dbLbl.Text = Row.Item(DBInfo.DataColumn).ToString

                Case TypeOf ctl Is CheckBox
                    Dim dbChk As CheckBox = DirectCast(ctl, CheckBox)
                    dbChk.Checked = CBool(Row.Item(DBInfo.DataColumn))

                Case TypeOf ctl Is RichTextBox
                    Dim dbRtb As RichTextBox = DirectCast(ctl, RichTextBox)
                    SetRichTextBox(dbRtb, Row.Item(DBInfo.DataColumn).ToString)

                Case Else
                    Throw New Exception("Unexpected type.")
            End Select

        Next

    End Sub

    ''' <summary>
    ''' Recursively collects list of controls initiated with <see cref="DBControlInfo"/> tags within Parent control.
    ''' </summary>
    ''' <param name="parentForm">Parent control. Usually a Form to being.</param>
    ''' <param name="controlList">Blank List of Control to be filled.</param>
    Public Function GetDBControls(parentForm As Control, Optional controlList As List(Of Control) = Nothing) As List(Of Control)
        If controlList Is Nothing Then controlList = New List(Of Control)
        For Each ctl As Control In parentForm.Controls
            Select Case True
                Case TypeOf ctl.Tag Is DBControlInfo
                    controlList.Add(ctl)
            End Select
            If ctl.HasChildren Then
                controlList.AddRange(GetDBControls(ctl))
            End If
        Next
        Return controlList
    End Function

    Public Function GetDBControlValue(dbControl As Control) As Object
        Dim DBInfo = DirectCast(dbControl.Tag, DBControlInfo)
        Select Case True
            Case TypeOf dbControl Is TextBox
                Dim dbTxt As TextBox = DirectCast(dbControl, TextBox)
                Return CleanDBValue(dbTxt.Text)

            Case TypeOf dbControl Is MaskedTextBox
                Dim dbMaskTxt As MaskedTextBox = DirectCast(dbControl, MaskedTextBox)
                Return CleanDBValue(dbMaskTxt.Text)

            Case TypeOf dbControl Is DateTimePicker
                Dim dbDtPick As DateTimePicker = DirectCast(dbControl, DateTimePicker)
                Return dbDtPick.Value

            Case TypeOf dbControl Is ComboBox
                Dim dbCmb As ComboBox = DirectCast(dbControl, ComboBox)
                Return GetDBValue(DBInfo.AttribIndex, dbCmb.SelectedIndex)

            Case TypeOf dbControl Is CheckBox
                Dim dbChk As CheckBox = DirectCast(dbControl, CheckBox)
                Return dbChk.Checked
            Case Else
                Throw New Exception("Unexpected type.")
        End Select
        Return Nothing
    End Function

    ''' <summary>
    ''' Collects an EMPTY DataTable via a SQL Select statement and adds a new row for SQL Insertion.
    ''' </summary>
    ''' <remarks>
    ''' The SQL SELECT statement should return an EMPTY table. A new row will be added to this table via <see cref="UpdateDBControlRow(ByRef DataRow)"/>
    ''' </remarks>
    ''' <param name="selectQry">A SQL Select query string that will return an EMPTY table. Ex: "SELECT * FROM table LIMIT 0"</param>
    ''' <returns>
    ''' Returns a DataTable with a new row added via <see cref="UpdateDBControlRow(ByRef DataRow)"/>
    ''' </returns>
    Public Function ReturnInsertTable(selectQry As String) As DataTable
        Dim tmpTable As DataTable
        tmpTable = DBFunc.GetDatabase.DataTableFromQueryString(selectQry)
        tmpTable.Rows.Add()
        UpdateDBControlRow(tmpTable.Rows(0))
        Return tmpTable
    End Function

    ''' <summary>
    ''' Collects a DataTable via a SQL Select statement and modifies the topmost row with data parsed from <seealso cref="UpdateDBControlRow(ByRef DataRow)"/>
    ''' </summary>
    ''' <remarks>
    ''' The SQL SELECT statement should return a single row only. This is will be the table row that you wish to update.
    ''' </remarks>
    ''' <param name="selectQry">A SQL Select query string that will return the table row that is to be updated.</param>
    ''' <returns>
    ''' Returns a DataTable modified by the controls identified by <see cref="GetDBControls(Control, List(Of Control))"/>
    ''' </returns>
    Public Function ReturnUpdateTable(selectQry As String) As DataTable
        Dim tmpTable As New DataTable
        tmpTable = DBFunc.GetDatabase.DataTableFromQueryString(selectQry)
        tmpTable.TableName = "UpdateTable"
        UpdateDBControlRow(tmpTable.Rows(0))
        Return tmpTable
    End Function
    ''' <summary>
    ''' Modifies a DataRow with data parsed from controls collected by <see cref="GetDBControls(Control, List(Of Control))"/>
    ''' </summary>
    ''' <param name="DBRow">DataRow to be modified.</param>
    Private Sub UpdateDBControlRow(ByRef DBRow As DataRow)
        For Each ctl As Control In GetDBControls(ParentForm)
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
                    Case Else
                        Throw New Exception("Unexpected type.")
                End Select
            End If
        Next
    End Sub

#End Region

End Class
