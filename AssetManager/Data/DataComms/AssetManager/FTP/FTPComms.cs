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
using System.Net;
using System.IO;

namespace AssetManager
{

	public class FtpComms
	{

		#region "Fields"

		private const string EncFTPUserPass = "BzPOHPXLdGu9CxaHTAEUCXY4Oa5EVM2B/G7O9En28LQ=";
		private const string strFTPUser = "asset_manager";
		private NetworkCredential FTPcreds = new NetworkCredential(strFTPUser, SecurityTools.DecodePassword(EncFTPUserPass));

		private int intSocketTimeout = 5000;
		#endregion

		#region "Methods"

		public Stream ReturnFtpRequestStream(string uri, string method)
		{
			FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(uri);
			var _with1 = request;
			_with1.Proxy = new WebProxy();
			//set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.
			_with1.Credentials = FTPcreds;
			_with1.Method = method;
			_with1.ReadWriteTimeout = intSocketTimeout;
			_with1.Timeout = intSocketTimeout;
			return _with1.GetRequestStream();
		}

		public WebResponse ReturnFtpResponse(string uri, string method)
		{
			FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(uri);
			var _with2 = request;
			_with2.Proxy = new WebProxy();
			//set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.
			_with2.Credentials = FTPcreds;
			_with2.Method = method;
			_with2.ReadWriteTimeout = intSocketTimeout;
			_with2.Timeout = intSocketTimeout;
			return _with2.GetResponse();
		}

		#endregion

	}
}
