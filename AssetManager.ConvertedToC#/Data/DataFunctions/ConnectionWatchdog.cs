using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace AssetManager
{

    public class ConnectionWatchdog : IDisposable
    {

        public ConnectionWatchdog(bool cachedMode)
        {
            InCachedMode = cachedMode;
        }

        public event EventHandler StatusChanged;

        public event EventHandler RebuildCache;

        public event EventHandler WatcherTick;

        protected virtual void OnStatusChanged(WatchDogStatusEventArgs e)
        {
            if (StatusChanged != null)
            {
                StatusChanged(this, e);
            }
        }

        protected virtual void OnRebuildCache(EventArgs e)
        {
            if (RebuildCache != null)
            {
                RebuildCache(this, e);
            }
        }

        protected virtual void OnWatcherTick(EventArgs e)
        {
            if (WatcherTick != null)
            {
                WatcherTick(this, e);
            }
        }

        private int FailedPings = 0;

        private int Cycles = 0;
        private bool ServerIsOnline;
        private bool CacheIsAvailable;

        private bool InCachedMode;
        private WatchDogConnectionStatus CurrentWatchdogStatus = WatchDogConnectionStatus.Online;

        private WatchDogConnectionStatus PreviousWatchdogStatus;
        const int MaxFailedPings = 2;
        const int CyclesTillHashCheck = 60;
        const int WatcherInterval = 5000;

        private Task WatcherTask;// = new Task(() => Watcher());
        public void StartWatcher()
        {
            WatcherTask = new Task(() => Watcher());
            WatcherTask.Start();
            if (InCachedMode)
            {
                OnStatusChanged(new WatchDogStatusEventArgs(WatchDogConnectionStatus.CachedMode));
            }
        }

        private async void Watcher()
        {
            while (!(disposedValue))
            {
                ServerIsOnline = await GetServerStatus();
                CacheIsAvailable = await DBCacheFunctions.VerifyLocalCacheHashOnly(InCachedMode);

                var Status = GetWatchdogStatus();
                if (Status != CurrentWatchdogStatus)
                {
                    CurrentWatchdogStatus = Status;
                    OnStatusChanged(new WatchDogStatusEventArgs(CurrentWatchdogStatus));
                }

                if (ServerIsOnline)
                {
                    //Fire tick event to update server datatime.
                    OnWatcherTick(new WatchDogTickEventArgs(await GetServerTime()));
                }

                CheckForCacheRebuild();

                await Task.Delay(WatcherInterval);
            }
        }

        private void CheckForCacheRebuild()
        {
            if (CurrentWatchdogStatus == WatchDogConnectionStatus.Online)
            {
                if (CacheIsAvailable)
                {
                    Cycles += 1;
                    if (Cycles >= CyclesTillHashCheck)
                    {
                        Cycles = 0;
                        OnRebuildCache(new EventArgs());
                    }
                }
                else
                {
                    OnRebuildCache(new EventArgs());
                }
                if (PreviousWatchdogStatus == WatchDogConnectionStatus.CachedMode | PreviousWatchdogStatus == WatchDogConnectionStatus.Offline)
                {
                    OnRebuildCache(new EventArgs());
                }
            }
            PreviousWatchdogStatus = CurrentWatchdogStatus;
        }

        private async Task<string> GetServerTime()
        {
            try
            {
                return await Task.Run(() =>
                {
                    using (MySQLDatabase MySQLDB = new MySQLDatabase())
                    {
                        return MySQLDB.ExecuteScalarFromQueryString("SELECT NOW()").ToString();
                    }
                });
            }
            catch
            {
                return string.Empty;
            }
        }

        private async Task<bool> GetServerStatus()
        {
            return await Task.Run(() =>
            {
                for (int i = 0; i <= MaxFailedPings; i++)
                {
                    if (!CanTalkToServer())
                    {
                        FailedPings += 1;
                        if (FailedPings >= MaxFailedPings)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        FailedPings = 0;
                        return true;
                    }
                    Task.Delay(1000);
                }
                return true;
            });
        }

        private bool CanTalkToServer()
        {
            try
            {
                using (Ping ServerPing = new Ping())
                {
                    bool CanPing = false;
                    var Reply = ServerPing.Send(ServerInfo.MySQLServerIP);
                    if (Reply.Status == IPStatus.Success)
                    {
                        CanPing = true;
                    }
                    else
                    {
                        CanPing = false;
                    }

                    Reply = null;

                    //If server pinging, try to open a connection.
                    if (CanPing)
                    {
                        using (MySQLDatabase MySQLDB = new MySQLDatabase())
                        using (MySqlConnection conn = MySQLDB.NewConnection())
                        {
                            return MySQLDB.OpenConnection(conn, true);
                        }
                    }
                    else
                    {
                        //Not pinging. Return false.
                        return false;
                    }
                }
            }
            catch
            {
                //Catch ping or SQL exceptions, and return false.
                return false;
            }
        }

        private WatchDogConnectionStatus GetWatchdogStatus()
        {

            if (ServerIsOnline & !InCachedMode)
            {
                return WatchDogConnectionStatus.Online;
            }
            else if (ServerIsOnline & InCachedMode)
            {
                InCachedMode = false;
                return WatchDogConnectionStatus.Online;
            }
            else if (!ServerIsOnline & CacheIsAvailable)
            {
                InCachedMode = true;
                return WatchDogConnectionStatus.CachedMode;
            }
            else
            {
                return WatchDogConnectionStatus.Offline;
            }


            //switch (true)
            //{

            //    case ServerIsOnline & !InCachedMode:

            //        return WatchDogConnectionStatus.Online;
            //    case ServerIsOnline & InCachedMode:
            //        InCachedMode = false;

            //        return WatchDogConnectionStatus.Online;
            //    case !ServerIsOnline & CacheIsAvailable:
            //        InCachedMode = true;

            //        return WatchDogConnectionStatus.CachedMode;
            //    case !ServerIsOnline & !CacheIsAvailable:
            //        InCachedMode = false;

            //        return WatchDogConnectionStatus.Offline;
            //    default:

            //        return WatchDogConnectionStatus.Offline;
            //}
        }

        #region "IDisposable Support"

        // To detect redundant calls
        private bool disposedValue;

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    WatcherTask.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                // TODO: set large fields to null.
            }
            disposedValue = true;
        }

        // TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        //Protected Overrides Sub Finalize()
        //    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        //    Dispose(False)
        //    MyBase.Finalize()
        //End Sub

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            // TODO: uncomment the following line if Finalize() is overridden above.
            // GC.SuppressFinalize(Me)
        }

        #endregion

    }

    public class WatchDogTickEventArgs : EventArgs
    {
        public string ServerTime { get; }

        public WatchDogTickEventArgs(string serverTime)
        {
            this.ServerTime = serverTime;
        }

    }

    public class WatchDogStatusEventArgs : EventArgs
    {
        public WatchDogConnectionStatus ConnectionStatus { get; set; }

        public WatchDogStatusEventArgs(WatchDogConnectionStatus connectionStatus)
        {
            this.ConnectionStatus = connectionStatus;
        }

    }

    public enum WatchDogConnectionStatus
    {
        Online,
        Offline,
        CachedMode
    }

}
