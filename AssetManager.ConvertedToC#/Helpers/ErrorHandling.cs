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
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System.Net;
using System.Net.NetworkInformation;

namespace AssetManager
{

	static class ErrorHandling
	{

		//Suppress additional messages to user.

		private static bool SuppressAdditionalMessages = false;
		public static bool ErrHandle(Exception ex, MethodBase Method)
		{
			//Recursive error handler. Returns False for undesired or dangerous errors, True if safe to continue.
			try {
				Logging.Logger("ERR STACK TRACE: " + ex.ToString());
				bool ErrorResult = false;


                if (ex is WebException)
                {
                    var WebEx = (WebException)ex;
                    ErrorResult = handleWebException(WebEx, Method);
                }
                else if (ex is IndexOutOfRangeException)
                {
                    IndexOutOfRangeException handEx = (IndexOutOfRangeException)ex;
                    Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + handEx.HResult + "  Message:" + handEx.Message);
                    ErrorResult = true;
                }
                else if (ex is MySqlException)
                {
                    var MySQLEx = (MySqlException)ex;
                    ErrorResult = handleMySQLException(MySQLEx, Method);
                }
                else if (ex is SqlException)
                {
                    var SQLEx = (SqlException)ex;
                    ErrorResult = handleSQLException(SQLEx, Method);
                }
                else if (ex is SQLiteException)
                {
                    var SQLiteEx = (SQLiteException)ex;
                    ErrorResult = handleSQLiteException(SQLiteEx, Method);
                }
                else if (ex is InvalidCastException)
                {
                    InvalidCastException handEx = (InvalidCastException)ex;
                    Logging.Logger("CAST ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + handEx.HResult + "  Message:" + handEx.Message);
                    //  PromptUser("An object was cast to an unmatched type.  See log for details.  Log: " + Paths.LogPath, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Invalid Cast Error");
                    PromptUser("An object was cast to an unmatched type.  See log for details.  Log: " + Paths.LogPath, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Invalid Cast Error");
                    if (handEx.HResult == -2147467262)
                    {
                        ErrorResult = true;
                    }

                }
                else if (ex is IOException)
                {
                    var IOEx = (IOException)ex;
                    ErrorResult = handleIOException(IOEx, Method);
                }
                else if (ex is PingException)
                {
                    var PingEx = (PingException)ex;
                    ErrorResult = handlePingException(PingEx, Method);
                }
                else if (ex is SocketException)
                {
                    var SocketEx = (SocketException)ex;
                    ErrorResult = handleSocketException(SocketEx, Method);
                }
                else if (ex is FormatException)
                {
                    var FormatEx = (FormatException)ex;
                    ErrorResult = handleFormatException(FormatEx, Method);
                }
                else if (ex is Win32Exception)
                {
                    var Win32Ex = (Win32Exception)ex;
                    ErrorResult = handleWin32Exception(Win32Ex, Method);
                }
                else if (ex is InvalidOperationException)
                {
                    var InvalidOpEx = (InvalidOperationException)ex;
                    ErrorResult = handleOperationException(InvalidOpEx, Method);
                }
                else if (ex is NoPingException)
                {
                    var NoPingEx = (NoPingException)ex;
                    ErrorResult = handleNoPingException(NoPingEx, Method);
                }
                else if (ex is NullReferenceException)
                {
                    var NullRefEx = (NullReferenceException)ex;
                    ErrorResult = handleNullReferenceException(NullRefEx, Method);
                }
                else if (ex is NotImplementedException)
                {
                    var NotImplementedEx = (NotImplementedException)ex;
                    ErrorResult = handleNotImplementedException(NotImplementedEx, Method);
                }
                else
                {
                    UnHandledError(ex, ex.HResult, Method);
                    ErrorResult = false;
                }












     //           switch (true) {
					//case ex is System.Net.WebException:
					//	var WebEx = (Net.WebException)ex;
					//	ErrorResult = handleWebException(WebEx, Method);

					//	break;
					//case ex is IndexOutOfRangeException:
					//	IndexOutOfRangeException handEx = (IndexOutOfRangeException)ex;
					//	Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + handEx.HResult + "  Message:" + handEx.Message);
					//	ErrorResult = true;

					//	break;
					//case ex is MySqlException:
					//	var MySQLEx = (MySqlException)ex;
					//	ErrorResult = handleMySQLException(MySQLEx, Method);

					//	break;
					//case ex is SqlException:
					//	var SQLEx = (SqlException)ex;
					//	ErrorResult = handleSQLException(SQLEx, Method);

					//	break;
					//case ex is SQLiteException:
					//	var SQLiteEx = (SQLiteException)ex;
					//	ErrorResult = handleSQLiteException(SQLiteEx, Method);

					//	break;
					//case ex is InvalidCastException:
					//	InvalidCastException handEx = (InvalidCastException)ex;
					//	Logging.Logger("CAST ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + handEx.HResult + "  Message:" + handEx.Message);
					//	PromptUser("An object was cast to an unmatched type.  See log for details.  Log: " + Paths.LogPath, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Invalid Cast Error");
					//	switch (handEx.HResult) {
					//		case -2147467262:
					//			//DBNull to String type error. These are pretty ubiquitous and not a big deal. Move along.
					//			ErrorResult = true;
					//			break;
					//	}

					//	break;
					//case ex is IOException:
					//	var IOEx = (IOException)ex;
					//	ErrorResult = handleIOException(IOEx, Method);

					//	break;
					//case ex is Net.NetworkInformation.PingException:
					//	var PingEx = (Net.NetworkInformation.PingException)ex;
					//	ErrorResult = handlePingException(PingEx, Method);

					//	break;
					//case ex is SocketException:
					//	var SocketEx = (SocketException)ex;
					//	ErrorResult = handleSocketException(SocketEx, Method);

					//	break;
					//case ex is FormatException:
					//	var FormatEx = (FormatException)ex;
					//	ErrorResult = handleFormatException(FormatEx, Method);

					//	break;
					//case ex is Win32Exception:
					//	var Win32Ex = (Win32Exception)ex;
					//	ErrorResult = handleWin32Exception(Win32Ex, Method);

					//	break;
					//case ex is InvalidOperationException:
					//	var InvalidOpEx = (InvalidOperationException)ex;
					//	ErrorResult = handleOperationException(InvalidOpEx, Method);

					//	break;
					//case ex is NoPingException:
					//	var NoPingEx = (NoPingException)ex;
					//	ErrorResult = handleNoPingException(NoPingEx, Method);

					//	break;
					//case ex is NullReferenceException:
					//	var NullRefEx = (NullReferenceException)ex;
					//	ErrorResult = handleNullReferenceException(NullRefEx, Method);

					//	break;
					//case ex is NotImplementedException:
					//	var NotImplementedEx = (NotImplementedException)ex;
					//	ErrorResult = handleNotImplementedException(NotImplementedEx, Method);

					//	break;
					//default:
					//	UnHandledError(ex, ex.HResult, Method);
					//	ErrorResult = false;
					//	break;
				    //}
				if (ex.InnerException != null) {
					ErrorResult = ErrHandle(ex.InnerException, Method);
				}
				return ErrorResult;
			} finally {
				SuppressAdditionalMessages = false;
			}
		}

		private static bool handleNotImplementedException(NotImplementedException ex, MethodBase Method)
		{
			Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.HResult + "  Message:" + ex.Message);
			PromptUser("ERROR:  Method not implemented.  See log for details: file://" + Paths.LogPath, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Method Not Implemented");
			return true;
		}

		private static bool handleWin32Exception(Win32Exception ex, MethodBase Method)
		{
			Logging.Logger("ERROR: MethodName =" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.NativeErrorCode + "  Message:" + ex.Message);
			switch (ex.NativeErrorCode) {
				case 1909:
					//Locked account
					PromptUser("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.NativeErrorCode + "  Message:" + ex.Message, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Network Error");
					SecurityTools.ClearAdminCreds();
					return true;
				case 1326:
					//Bad credentials error. Clear AdminCreds
					PromptUser("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.NativeErrorCode + "  Message:" + ex.Message, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Network Error");
                    SecurityTools.ClearAdminCreds();
                    return true;
				case 86:
					//Bad credentials error. Clear AdminCreds
					PromptUser("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.NativeErrorCode + "  Message:" + ex.Message, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Network Error");
                    SecurityTools.ClearAdminCreds();
                    return true;
				case 5:
					//Access denied error. Clear AdminCreds
					PromptUser("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.NativeErrorCode + "  Message:" + ex.Message, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Network Error");
                    SecurityTools.ClearAdminCreds();
                    return true;
				default:
					PromptUser("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.NativeErrorCode + "  Message:" + ex.Message, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Network Error");
					return true;
			}
		}

		private static bool handleFormatException(FormatException ex, MethodBase Method)
		{
			Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.HResult + "  Message:" + ex.Message);
			switch (ex.HResult) {
				case -2146233033:
					return true;
				default:
					UnHandledError(ex, ex.HResult, Method);
					break;
			}
			return false;
		}

		private static bool handleMySQLException(MySqlException ex, MethodBase Method)
		{
			Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.Number + "  Message:" + ex.Message);
			switch (ex.Number) {
				case 1042:
					// ConnectionReady()
					PromptUser("Unable to connect to server.  Check connection and try again.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Connection Lost");
					return true;
				case 0:
					// ConnectionReady()
					PromptUser("Unable to connect to server.  Check connection and try again.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Connection Lost");
					return true;
				case 1064:
					PromptUser("Something went wrong with the SQL command. See log for details.  Log: " + Paths.LogPath, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Error, "SQL Syntax Error");
					return true;
				case 1406:
					PromptUser(ex.Message + Constants.vbCrLf + Constants.vbCrLf + "Log: " + Paths.LogPath, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Error, "SQL Error");
					return true;
				case 1292:
					PromptUser("Something went wrong with the SQL command. See log for details.  Log: " + Paths.LogPath, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Error, "SQL Syntax Error");
					return true;
				default:
					UnHandledError(ex, ex.Number, Method);
					break;
			}
			return false;
		}

		private static bool handlePingException(System.Net.NetworkInformation.PingException ex, MethodBase Method)
		{
			switch (ex.HResult) {
				case -2146233079:
					return false;
				default:
					UnHandledError(ex, ex.HResult, Method);
					return false;
			}
		}

		private static bool handleOperationException(InvalidOperationException ex, MethodBase Method)
		{
			switch (ex.HResult) {
				case -2146233079:
					Logging.Logger("UNHANDLED ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.HResult + "  Message:" + ex.Message);
					return false;
				default:
					UnHandledError(ex, ex.HResult, Method);
					return false;
			}
		}

		private static bool handleWebException(WebException ex, MethodBase Method)
		{
			System.Net.FtpWebResponse handResponse = (FtpWebResponse)ex.Response;
			switch (handResponse.StatusCode) {
				case System.Net.FtpStatusCode.ActionNotTakenFileUnavailable:
					PromptUser("FTP File was not found, or access was denied.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Cannot Access FTP File");
					return true;
				default:
					switch (ex.HResult) {
						case -2146233079:
							Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.HResult + "  Message:" + ex.Message);
							PromptUser("Could not connect to FTP server.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Connection Failure");
							return true;
					}
					UnHandledError(ex, ex.HResult, Method);
					break;
			}
			return false;
		}

		private static bool handleIOException(IOException ex, MethodBase Method)
		{
			switch (ex.HResult) {
				case -2147024864:
					Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.HResult + "  Message:" + ex.Message);
					//PromptUser("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "IO Error")
					return true;
				case -2146232800:
					Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.HResult + "  Message:" + ex.Message);
					return true;
				default:
					UnHandledError(ex, ex.HResult, Method);
					break;
			}
			return false;
		}

		private static bool handleSocketException(SocketException ex, MethodBase Method)
		{
			switch (ex.SocketErrorCode) {
				//FTPSocket timeout
				case SocketError.TimedOut:
					//10060
					Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.SocketErrorCode + "  Message:" + ex.Message);
					PromptUser("Lost connection to the server or the server took too long to respond.  See Log.  '" + Paths.LogPath + "'", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Network Socket Timeout");
					return true;
				case SocketError.HostUnreachable:
					//10065 'host unreachable
					Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.SocketErrorCode + "  Message:" + ex.Message);
					//Dim blah = PromptUser("Lost connection to the server or the server took too long to respond.  See Log.  '" & strLogPath & "'", vbOKOnly + vbExclamation, "Network Socket Timeout")
					return true;
				case SocketError.ConnectionAborted:
					//10053
					Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.SocketErrorCode + "  Message:" + ex.Message);
					PromptUser("Lost connection to the server or the server took too long to respond.  See Log.  '" + Paths.LogPath + "'", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Connection Aborted");
					return true;
				case SocketError.ConnectionReset:
					//10054 'connection reset
					Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.SocketErrorCode + "  Message:" + ex.Message);
					PromptUser("Lost connection to the server or the server took too long to respond.  See Log.  '" + Paths.LogPath + "'", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Connection Reset");
					return true;
				case SocketError.NetworkUnreachable:
					Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.SocketErrorCode + "  Message:" + ex.Message);
					PromptUser("Could not connect to server.  See Log.  '" + Paths.LogPath + "'", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Network Unreachable");
					return true;
				case SocketError.HostNotFound:
					//11001 'host not found.
					return false;
				default:
					UnHandledError(ex, (int)ex.SocketErrorCode, Method);
					return false;
			}
		}

		private static bool handleSQLException(SqlException ex, MethodBase Method)
		{
			switch (ex.Number) {
				case 18456:
					Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.Number + "  Message:" + ex.Message);
					PromptUser("Error connecting to MUNIS Database.  Your username may not have access.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "MUNIS Error");
					return false;
				case 102:
					Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.Number + "  Message:" + ex.Message);
					PromptUser("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.Number + "  Message:" + ex.Message, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "ERROR");
					return false;
				case 121:
					Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.Number + "  Message:" + ex.Message);
					PromptUser("Could not connect to MUNIS database.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Network Error");
					return false;
				case 245:
					Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.Number + "  Message:" + ex.Message);
					//Dim blah = PromptUser("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "ERROR")
					return false;
				case 248:
					Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.Number + "  Message:" + ex.Message);
					PromptUser("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.Number + "  Message:" + ex.Message, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "ERROR");
					return false;
				case 53:
					Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.Number + "  Message:" + ex.Message);
					PromptUser("Could not connect to MUNIS database.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Network Error");
					SuppressAdditionalMessages = true;
					return true;
				default:
					UnHandledError(ex, ex.Number, Method);
					return false;
			}
		}

		private static bool handleSQLiteException(SQLiteException ex, MethodBase Method)
		{

			switch (ex.ErrorCode) {
				case 1:
					Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.ErrorCode + "  Message:" + ex.Message);
					return true;
				default:
					UnHandledError(ex, ex.ErrorCode, Method);
					return false;
			}

		}


		private static bool handleNoPingException(NoPingException ex, MethodBase Method)
		{
			switch (ex.HResult) {
				case -2146233088:
					Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.HResult + "  Message:" + ex.Message);
					PromptUser("Unable to connect to server.  Check connection and try again.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Connection Lost");
					return true;
				default:
					UnHandledError(ex, ex.HResult, Method);
					return false;
			}
		}

		private static bool handleNullReferenceException(Exception ex, MethodBase Method)
		{
			if (ServerInfo.ServerPinging) {
				Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.HResult + "  Message:" + ex.Message);
				PromptUser("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.HResult + "  Message:" + ex.Message, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "ERROR");
				OtherFunctions.EndProgram();
				return false;
			} else {
				Logging.Logger("ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ex.HResult + "  Message:" + ex.Message);
				return true;
			}
		}

		private static void UnHandledError(Exception ex, int ErrorCode, MethodBase Method)
		{
			Logging.Logger("UNHANDLED ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ErrorCode + "  Message:" + ex.Message);
			PromptUser("UNHANDLED ERROR:  MethodName=" + Method.Name + "  Type: " + Information.TypeName(ex) + "  #:" + ErrorCode + "  Message:" + ex.Message + Constants.vbCrLf + Constants.vbCrLf + "file://" + Paths.LogPath, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Error, "ERROR");
			OtherFunctions.EndProgram();
		}

		private static void PromptUser(string Prompt, int Buttons = (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, string Title = null, Form ParentFrm = null)
		{
			if (!SuppressAdditionalMessages) {
				OtherFunctions.Message(Prompt, Buttons, Title, ParentFrm);
			}
		}

	}
}
