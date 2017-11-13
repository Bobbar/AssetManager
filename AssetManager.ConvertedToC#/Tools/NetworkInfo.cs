using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManager
{
    public static class NetworkInfo
    {
        private static string _currentDomain = "core.co.fairfield.oh.us";
        public static string CurrentDomain
        {
            get { return _currentDomain; }
        }

        private static Dictionary<Databases, string> DomainNames = new Dictionary<Databases, string>
        {
            {Databases.test_db, "core.co.fairfield.oh.us"},
            {Databases.asset_manager, "core.co.fairfield.oh.us"},
            {Databases.vintondd, "vinton.local"}
        };

        private static Dictionary<string, string> SubnetLocations = new Dictionary<string, string>
        {
            {"10.10.80.0", "Admin"},
            {"10.10.81.0", "OC"},
            {"10.10.82.0", "DiscoverU"},
            {"10.10.83.0", "FRS"},
            {"10.10.84.0", "PRO"},
            {"10.10.85.0", "Art & Clay"}
        };

        public static string LocationOfIP(string ip)
        {
        
            try
            {
                string Subnet = ip.Substring(0, 8) + ".0";
                if (SubnetLocations.ContainsKey(Subnet))
                {
                    
                    return SubnetLocations[Subnet];
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string SetCurrentDomain(Databases database)
        {
            _currentDomain = DomainNames[database];
            SecurityTools.ClearAdminCreds();
            if (database == Databases.vintondd)
            {
                SecurityTools.VerifyAdminCreds("Credentials for Vinton AD");
            }
            return DomainNames(database);
        }


    }
}
