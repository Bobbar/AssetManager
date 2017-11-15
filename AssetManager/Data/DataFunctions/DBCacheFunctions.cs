using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace AssetManager
{
    public static class DBCacheFunctions
    {

        public static List<string> SQLiteTableHashes;

        public static List<string> RemoteTableHashes;
        public static void RefreshLocalDBCache()
        {
            try
            {
                // GlobalSwitches.BuildingCache = True
                using (SQLiteDatabase conn = new SQLiteDatabase(false))
                {
                    conn.RefreshSqlCache();
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                GlobalSwitches.BuildingCache = false;
            }
        }

        /// <summary>
        /// Builds local hash list and compares to previously built remote hash list. Returns False for mismatch.
        /// </summary>
        /// <param name="cachedMode">When true, only checks for Schema Version since a remote table hash will likely be unavailable.</param>
        /// <returns></returns>
        public static async Task<bool> VerifyLocalCacheHashOnly(bool cachedMode)
        {
            if (!cachedMode)
            {
                if (RemoteTableHashes == null)
                    return false;
                return await Task.Run(() =>
                {
                    List<string> LocalHashes = new List<string>();
                    using (SQLiteDatabase SQLiteComms = new SQLiteDatabase())
                    {
                        LocalHashes = SQLiteComms.LocalTableHashList();
                        return SQLiteComms.CompareTableHashes(LocalHashes, RemoteTableHashes);
                    }
                });
            }
            else
            {
                using (SQLiteDatabase SQLiteComms = new SQLiteDatabase())
                {
                    if (SQLiteComms.GetSchemaVersion() > 0)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Builds hash lists for both local and remote tables and compares them.  Returns False for mismatch.
        /// </summary>
        /// <param name="connectedToDB"></param>
        /// <returns></returns>
        public static bool VerifyCacheHashes(bool connectedToDB = true)
        {
            try
            {
                using (SQLiteDatabase SQLiteComms = new SQLiteDatabase())
                {
                    if (SQLiteComms.GetSchemaVersion() > 0)
                    {
                        if (connectedToDB)
                        {
                            SQLiteTableHashes = SQLiteComms.LocalTableHashList();
                            RemoteTableHashes = SQLiteComms.RemoteTableHashList();
                            return SQLiteComms.CompareTableHashes(SQLiteTableHashes, RemoteTableHashes);
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        SQLiteTableHashes = null;
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
