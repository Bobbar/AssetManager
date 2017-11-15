using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;

namespace AssetManager
{
    /// <summary>
    /// Credit to: http://stackoverflow.com/questions/295538/how-to-provide-user-name-and-password-when-connecting-to-a-network-share
    /// </summary>
    public class NetworkConnection : IDisposable
    {

        private string _networkName;

        public NetworkConnection(string networkName, NetworkCredential credentials)
        {
            _networkName = networkName;

            var netResource = new NetResource
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = networkName
            };

            var userName = string.IsNullOrEmpty(credentials.Domain) ? credentials.UserName : string.Format("{0}\\{1}", credentials.Domain, credentials.UserName);

            var result = WNetAddConnection2(netResource, credentials.Password, userName, 0);

            if (result != 0)
            {
                throw new Win32Exception(result);
            }
        }

        //protected override void Finalize()
        //{
        //    try
        //    {
        //        Dispose(false);
        //    }
        //    finally
        //    {
        //        base.Finalize();
        //    }
        //}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            WNetCancelConnection2(_networkName, 0, true);
        }

        [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
        private static extern int WNetAddConnection2(NetResource netResource, string password, string username, int flags);


        [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
        private static extern int WNetCancelConnection2(string name, int flags, bool force);
      

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class NetResource
    {
        public ResourceScope Scope;
        public ResourceType ResourceType;
        public ResourceDisplaytype DisplayType;

        public int Usage;
        [MarshalAs(UnmanagedType.LPWStr)]

        public string LocalName;
        [MarshalAs(UnmanagedType.LPWStr)]

        public string RemoteName;
        [MarshalAs(UnmanagedType.LPWStr)]

        public string Comment;
        [MarshalAs(UnmanagedType.LPWStr)]

        public string Provider;
    }

    public enum ResourceScope : int
    {
        Connected = 1,
        GlobalNetwork,
        Remembered,
        Recent,
        Context
    }

    public enum ResourceType : int
    {
        Any = 0,
        Disk = 1,
        Print = 2,
        Reserved = 8
    }

    public enum ResourceDisplaytype : int
    {
        Generic = 0x0,
        Domain = 0x1,
        Server = 0x2,
        Share = 0x3,
        File = 0x4,
        Group = 0x5,
        Network = 0x6,
        Root = 0x7,
        Shareadmin = 0x8,
        Directory = 0x9,
        Tree = 0xa,
        Ndscontainer = 0xb
    }
}