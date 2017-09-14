Imports System.Data.Common

Public Interface IDataBase

    Function StartTransaction() As DbTransaction
    Function DataTableFromQueryString(query As String) As DataTable
    Function DataTableFromCommand(command As DbCommand) As DataTable
    Function DataTableFromParameters(query As String, params As List(Of DBQueryParameter)) As DataTable
    Function ExecuteScalarFromCommand(command As DbCommand) As Object
    Function ExecuteScalarFromQueryString(query As String) As Object
    Function ExecuteQuery(query As String) As Integer
    Function InsertFromParameters(tableName As String, params As List(Of DBParameter)) As Integer
    Function UpdateTable(selectQuery As String, table As DataTable, Optional transaction As DbTransaction = Nothing) As Integer
    Function UpdateValue(tableName As String, fieldIn As String, valueIn As Object, idField As String, idValue As String) As Integer
    Function GetCommand(Optional qryString As String = "") As DbCommand
    Function GetCommandFromParams(query As String, params As List(Of DBQueryParameter)) As DbCommand

End Interface
