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
            request.Proxy = new WebProxy();
            //set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.
            request.Credentials = FTPcreds;
            request.Method = method;
            request.ReadWriteTimeout = intSocketTimeout;
            request.Timeout = intSocketTimeout;
            return request.GetRequestStream();
        }

        public WebResponse ReturnFtpResponse(string uri, string method)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(uri);
            request.Proxy = new WebProxy();
            //set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.
            request.Credentials = FTPcreds;
            request.Method = method;
            request.ReadWriteTimeout = intSocketTimeout;
            request.Timeout = intSocketTimeout;
            return request.GetResponse();
        }

        #endregion

    }
}
