using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace AssetManager
{

    public class MunisComms
    {

        #region Fields

        private const string MSSQLConnectString = "server=svr-munis5.core.co.fairfield.oh.us; database=mu_live; trusted_connection=True;";

        #endregion

        #region Methods

        public SqlCommand ReturnSqlCommand(string sqlQry)
        {
            SqlConnection conn = new SqlConnection(MSSQLConnectString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sqlQry;
            return cmd;
        }

        public DataTable ReturnSqlTable(string sqlQry)
        {
            using (SqlConnection conn = new SqlConnection(MSSQLConnectString))
            {
                using (DataTable NewTable = new DataTable())
                {
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.SelectCommand = new SqlCommand(sqlQry);
                        da.SelectCommand.Connection = conn;
                        da.Fill(NewTable);
                        return NewTable;
                    }
                }
            }

        }

        public async Task<DataTable> ReturnSqlTableAsync(string sqlQry)
        {
            using (SqlConnection conn = new SqlConnection(MSSQLConnectString))
            {
                using (DataTable NewTable = new DataTable())
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQry, conn))
                    {
                        await conn.OpenAsync();
                        SqlDataReader dr = await cmd.ExecuteReaderAsync();
                        NewTable.Load(dr);
                        return NewTable;
                    }
                }
            }

        }

        public DataTable ReturnSqlTableFromCmd(SqlCommand cmd)
        {
            using (DataTable NewTable = new DataTable())
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(NewTable);
                    cmd.Dispose();
                    return NewTable;
                }
            }

        }

        public async Task<DataTable> ReturnSqlTableFromCmdAsync(SqlCommand cmd)
        {
            using (var conn = cmd.Connection)
            {
                using (DataTable NewTable = new DataTable())
                {
                    await conn.OpenAsync();
                    SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    NewTable.Load(dr);
                    cmd.Dispose();
                    return NewTable;
                }
            }

        }

        public object ReturnSqlValue(string table, object fieldIn, object valueIn, string fieldOut, object fieldIn2 = null, object valueIn2 = null)
        {
            string sqlQRY = "";
            List<DBQueryParameter> Params = new List<DBQueryParameter>();
            if (fieldIn2 != null && valueIn2 != null)
            {
                sqlQRY = "SELECT TOP 1 " + fieldOut + " FROM " + table; // & fieldIN.ToString & " = '" & valueIN.ToString & "' AND " & fieldIN2.ToString & " = '" & ValueIN2.ToString & "'"
                @Params.Add(new DBQueryParameter(fieldIn.ToString(), valueIn.ToString(), true));
                @Params.Add(new DBQueryParameter(fieldIn2.ToString(), valueIn2.ToString(), true));
            }
            else
            {
                sqlQRY = "SELECT TOP 1 " + fieldOut + " FROM " + table; // & fieldIN.ToString & " = '" & valueIN.ToString & "'"
                @Params.Add(new DBQueryParameter(fieldIn.ToString(), valueIn.ToString(), true));
            }
            using (var cmd = GetSqlCommandFromParams(sqlQRY, @Params))
            {
                using (var conn = cmd.Connection)
                {
                    cmd.Connection.Open();
                    return cmd.ExecuteScalar();
                }
            }

        }

        public async Task<string> ReturnSqlValueAsync(string table, object fieldIn, object valueIn, string fieldOut, object fieldIn2 = null, object valueIn2 = null)
        {
            try
            {
                string sqlQRY = "";
                List<DBQueryParameter> Params = new List<DBQueryParameter>();
                if (fieldIn2 != null && valueIn2 != null)
                {
                    sqlQRY = "SELECT TOP 1 " + fieldOut + " FROM " + table; // & fieldIN.ToString & " = '" & valueIN.ToString & "' AND " & fieldIN2.ToString & " = '" & ValueIN2.ToString & "'"
                    @Params.Add(new DBQueryParameter(fieldIn.ToString(), valueIn.ToString(), true));
                    @Params.Add(new DBQueryParameter(fieldIn2.ToString(), valueIn2.ToString(), true));
                }
                else
                {
                    sqlQRY = "SELECT TOP 1 " + fieldOut + " FROM " + table; // & fieldIN.ToString & " = '" & valueIN.ToString & "'"
                    @Params.Add(new DBQueryParameter(fieldIn.ToString(), valueIn.ToString(), true));
                }
                using (var cmd = GetSqlCommandFromParams(sqlQRY, @Params))
                {
                    using (var conn = cmd.Connection)
                    {
                        await cmd.Connection.OpenAsync();
                        var Value = await cmd.ExecuteScalarAsync();
                        //StopTimer()
                        if (Value != null)
                        {
                            return Value.ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            return string.Empty;
        }

        /// <summary>
        /// Takes a partial query string without the WHERE operator, and a list of <see cref="DBQueryParameter"/> and returns a parameterized <see cref="SqlCommand"/>.
        /// </summary>
        /// <param name="partialQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlCommand GetSqlCommandFromParams(string partialQuery, List<DBQueryParameter> parameters)
        {
            var cmd = ReturnSqlCommand(partialQuery);
            cmd.CommandText += " WHERE";
            string ParamString = "";
            int ValSeq = 1;
            foreach (var fld in parameters)
            {
                if (fld.IsExact)
                {
                    ParamString += " " + fld.FieldName + "=@Value" + ValSeq.ToString() + " " + fld.OperatorString;
                    cmd.Parameters.AddWithValue("@Value" + ValSeq.ToString(), fld.Value);
                }
                else
                {
                    ParamString += " " + fld.FieldName + " LIKE CONCAT('%', @Value" + ValSeq.ToString() + ", '%') " + fld.OperatorString;
                    cmd.Parameters.AddWithValue("@Value" + ValSeq.ToString(), fld.Value);
                }
                ValSeq++;
            }
            if (ParamString.Substring(ParamString.Length - 3, 3) == "AND") //remove trailing AND from query string
            {
                ParamString = ParamString.Substring(0, ParamString.Length - 3);
            }

            if (ParamString.Substring(ParamString.Length - 2, 2) == "OR") //remove trailing AND from query string
            {
                ParamString = ParamString.Substring(0, ParamString.Length - 2);
            }
            cmd.CommandText += ParamString;
            return cmd;
        }

        #endregion

    }
}