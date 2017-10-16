Imports System.Data.Common

Public Interface IDataBase

    ''' <summary>
    ''' Returns a new <see cref="DbTransaction"/>.
    ''' </summary>
    ''' <returns></returns>
    Function StartTransaction() As DbTransaction

    ''' <summary>
    ''' Returns a DataTable from a SQL query string.
    ''' </summary>
    ''' <param name="query"></param>
    ''' <returns></returns>
    Function DataTableFromQueryString(query As String) As DataTable

    ''' <summary>
    ''' Returns a DataTable from a <see cref="DbCommand"/>.
    ''' </summary>
    ''' <param name="command"></param>
    ''' <returns></returns>
    Function DataTableFromCommand(command As DbCommand) As DataTable

    ''' <summary>
    ''' Returns a DataTable from a partial SQL query string and a <see cref="List(Of DBQueryParameter)"/>.
    ''' </summary>
    ''' <param name="query"></param>
    ''' <param name="params"></param>
    ''' <returns></returns>
    Function DataTableFromParameters(query As String, params As List(Of DBQueryParameter)) As DataTable

    ''' <summary>
    ''' Returns an object value from a <see cref="DbCommand"/>.
    ''' </summary>
    ''' <param name="command"></param>
    ''' <returns></returns>
    Function ExecuteScalarFromCommand(command As DbCommand) As Object

    ''' <summary>
    '''  Returns an object value from a SQL query string.
    ''' </summary>
    ''' <param name="query"></param>
    ''' <returns></returns>
    Function ExecuteScalarFromQueryString(query As String) As Object

    ''' <summary>
    ''' Executes a non query and returns the number of rows affected.
    ''' </summary>
    ''' <param name="query"></param>
    ''' <returns></returns>
    Function ExecuteQuery(query As String) As Integer

    ''' <summary>
    ''' Inserts a list of <see cref="DBParameter"/> into the specified table. Returns the number of rows affected.
    ''' </summary>
    ''' <param name="tableName"></param>
    ''' <param name="params"></param>
    ''' <param name="transaction"></param>
    ''' <returns></returns>
    Function InsertFromParameters(tableName As String, params As List(Of DBParameter), Optional transaction As DbTransaction = Nothing) As Integer

    ''' <summary>
    ''' Updates the table returned by the <paramref name="selectQuery"/> with the specified DataTable. Returns rows affected.
    ''' </summary>
    ''' <param name="selectQuery"></param>
    ''' <param name="table"></param>
    ''' <param name="transaction"></param>
    ''' <returns></returns>
    Function UpdateTable(selectQuery As String, table As DataTable, Optional transaction As DbTransaction = Nothing) As Integer

    ''' <summary>
    ''' Updates a single value in the database.
    ''' </summary>
    ''' <param name="tableName"></param>
    ''' <param name="fieldIn"></param>
    ''' <param name="valueIn"></param>
    ''' <param name="idField"></param>
    ''' <param name="idValue"></param>
    ''' <param name="transaction"></param>
    ''' <returns></returns>
    Function UpdateValue(tableName As String, fieldIn As String, valueIn As Object, idField As String, idValue As String, Optional transaction As DbTransaction = Nothing) As Integer

    ''' <summary>
    ''' Returns a new <see cref="DbCommand"/>.
    ''' </summary>
    ''' <param name="qryString"></param>
    ''' <returns></returns>
    Function GetCommand(Optional qryString As String = "") As DbCommand

    ''' <summary>
    ''' Returns a new <see cref="DbCommand"/> with the specified parameters added.
    ''' </summary>
    ''' <param name="query"></param>
    ''' <param name="params"></param>
    ''' <returns></returns>
    Function GetCommandFromParams(query As String, params As List(Of DBQueryParameter)) As DbCommand

End Interface