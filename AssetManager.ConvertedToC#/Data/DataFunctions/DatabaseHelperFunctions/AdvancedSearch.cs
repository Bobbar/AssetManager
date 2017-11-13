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
namespace AssetManager.AdvancedSearch
{

	public class Search
	{

		#region "Fields"

		private string _searchString;

		private List<TableInfo> _searchTables;
		#endregion

		#region "Constructors"

		public Search(string searchString, List<TableInfo> searchTables)
		{
			_searchString = searchString;
			_searchTables = searchTables;
		}

		#endregion

		#region "Methods"

		public List<string> GetColumns(string table)
		{
			List<string> colList = new List<string>();
			var SQLQry = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '" + AssetManager.ServerInfo.ServerInfo.CurrentDataBase.ToString() + "' AND TABLE_NAME = '" + table + "'";
			var results = AssetManager.DBFactory.GetDatabase().DataTableFromQueryString(SQLQry);
			//comms.ReturnMySqlTable(SQLQry)
			foreach (DataRow row in results.Rows) {
				colList.Add(row["COLUMN_NAME"].ToString());
			}
			return colList;
		}

		public List<DataTable> GetResults()
		{
			List<DataTable> resultsList = new List<DataTable>();
			foreach (TableInfo table_loopVariable in _searchTables) {
				table = table_loopVariable;
				string qry = "SELECT " + BuildSelectString(table) + " FROM " + table.TableName + " WHERE ";
				qry += BuildFieldString(table);
				using (MySQLDatabase MySQLDB = new MySQLDatabase()) {
					using (var cmd = MySQLDB.GetCommand(qry)) {
						cmd.AddParameterWithValue("@" + "SEARCHVAL", _searchString);
						var results = MySQLDB.DataTableFromCommand(cmd);
						results.TableName = table.TableName;
						resultsList.Add(results);
						results.Dispose();
					}
				}
			}
			return resultsList;
		}

		private string BuildFieldString(TableInfo table)
		{
			string Fields = "";
			foreach (string col_loopVariable in table.Columns) {
				col = col_loopVariable;
				Fields += table.TableName + "." + col + " LIKE CONCAT('%', @SEARCHVAL, '%')";
				if (table.Columns.IndexOf(col) != table.Columns.Count - 1)
					Fields += " OR ";
			}
			return Fields;
		}

		private string BuildSelectString(TableInfo table)
		{
			string SelectString = "";
			foreach (string column_loopVariable in table.Columns) {
				column = column_loopVariable;
				SelectString += column;
				if (table.Columns.IndexOf(column) != table.Columns.Count - 1)
					SelectString += ",";
			}
			return SelectString;
		}

		#endregion

	}

	#region "Structs"

	public struct TableInfo
	{

		#region "Fields"

		public List<string> Columns { get; }
		public string TableKey { get; set; }
		public string TableName { get; set; }

		#endregion

		#region "Constructors"

		public TableInfo(string name, List<string> cols)
		{
			TableName = name;
			Columns = cols;
		}

		#endregion

	}

	#endregion

}
