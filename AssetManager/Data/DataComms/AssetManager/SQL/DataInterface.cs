using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Common;
namespace AssetManager
{

	public interface IDataBase
	{

		/// <summary>
		/// Returns a new <see cref="DbTransaction"/>.
		/// </summary>
		/// <returns></returns>
		DbTransaction StartTransaction();

		/// <summary>
		/// Returns a DataTable from a SQL query string.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		DataTable DataTableFromQueryString(string query);

		/// <summary>
		/// Returns a DataTable from a <see cref="DbCommand"/>.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		DataTable DataTableFromCommand(DbCommand command, DbTransaction transaction = null);

		/// <summary>
		/// Returns a DataTable from a partial SQL query string and a <see cref="List(Of DBQueryParameter)"/>.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="params"></param>
		/// <returns></returns>
		DataTable DataTableFromParameters(string query, List<DBQueryParameter> @params);

		/// <summary>
		/// Returns an object value from a <see cref="DbCommand"/>.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		object ExecuteScalarFromCommand(DbCommand command);

		/// <summary>
		///  Returns an object value from a SQL query string.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		object ExecuteScalarFromQueryString(string query);

		/// <summary>
		/// Executes a non query and returns the number of rows affected.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		int ExecuteQuery(string query);

		/// <summary>
		/// Inserts a list of <see cref="DBParameter"/> into the specified table. Returns the number of rows affected.
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="params"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		int InsertFromParameters(string tableName, List<DBParameter> @params, DbTransaction transaction = null);

		/// <summary>
		/// Updates the table returned by the <paramref name="selectQuery"/> with the specified DataTable. Returns rows affected.
		/// </summary>
		/// <param name="selectQuery"></param>
		/// <param name="table"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		int UpdateTable(string selectQuery, DataTable table, DbTransaction transaction = null);

		/// <summary>
		/// Updates a single value in the database.
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="fieldIn"></param>
		/// <param name="valueIn"></param>
		/// <param name="idField"></param>
		/// <param name="idValue"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		int UpdateValue(string tableName, string fieldIn, object valueIn, string idField, string idValue, DbTransaction transaction = null);

		/// <summary>
		/// Returns a new <see cref="DbCommand"/>.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		DbCommand GetCommand(string query = "");

		/// <summary>
		/// Returns a new <see cref="DbCommand"/> with the specified parameters added.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="params"></param>
		/// <returns></returns>
		DbCommand GetCommandFromParams(string query, List<DBQueryParameter> @params);

	}
}
