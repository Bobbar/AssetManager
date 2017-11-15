using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace AssetManager
{
    public class ActiveDirectoryWrapper
    {
        private string _hostname;
        private SearchResult _searchResults;

        // Public Property FoundInAD As Boolean = False
        public ActiveDirectoryWrapper(string hostname)
        {
            _hostname = hostname;
        }

        /// <summary>
        /// Executes the Active Directory search and populates search results. Returns true if results were found.
        /// </summary>
        /// <returns></returns>
        public bool LoadResults()
        {
            _searchResults = ReturnSearchResult();
            if (_searchResults != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Asynchronously executes the Active Directory search and populates search results. Returns true if results were found.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LoadResultsAsync()
        {
            return await Task.Run(() =>
            {
                _searchResults = ReturnSearchResult();
                if (_searchResults != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// Get the OU path of a domain computer.
        /// </summary>
        /// <returns></returns>
        public string GetDeviceOU()
        {
            return ParsePath(GetDistinguishedName());
        }

        /// <summary>
        /// Queries Active Directory for a hostname and returns the Distinguished Name.
        /// </summary>
        /// <returns></returns>
        public string GetDistinguishedName()
        {
            var directorySearchResult = _searchResults;
            if (directorySearchResult != null)
            {
                if (directorySearchResult.Properties["distinguishedName"].Count > 0)
                {
                    return directorySearchResult.Properties["distinguishedName"][0].ToString();
                }
            }
            return string.Empty;
        }

        private SearchResult ReturnSearchResult()
        {
            try
            {
                using (var rootDSE = new DirectoryEntry("LDAP://" + NetworkInfo.CurrentDomain + "/RootDSE"))
                {
                    if (ServerInfo.CurrentDataBase == Databases.vintondd)
                    {
                        if (!SecurityTools.VerifyAdminCreds("Credentials for Vinton AD"))
                        {
                            return null;
                        }
                        rootDSE.Username = SecurityTools.AdminCreds.UserName;
                        rootDSE.Password = SecurityTools.AdminCreds.Password;
                    }

                    var defaultNamingContext = rootDSE.Properties["defaultNamingContext"].Value.ToString();
                    var domainRootADsPath = string.Format("LDAP://" + NetworkInfo.CurrentDomain + "/{0}", defaultNamingContext);

                    using (var searchRoot = new DirectoryEntry(domainRootADsPath))
                    {
                        if (ServerInfo.CurrentDataBase == Databases.vintondd)
                        {
                            searchRoot.Username = SecurityTools.AdminCreds.UserName;
                            searchRoot.Password = SecurityTools.AdminCreds.Password;
                        }
                        var filter = "(&(objectCategory=computer)(name=" + _hostname + "))";
                        using (var directorySearch = new DirectorySearcher(searchRoot, filter))
                        {
                            var directorySearchResult = directorySearch.FindOne();
                            if (directorySearchResult != null)
                            {
                                return directorySearch.FindOne();
                            }
                            return null;
                        }

                    }

                }

            }
            catch
            {
                return null;
            }
        }

        //TODO: Fix this at some point. It's not currently referenced.
        //public List<string> GetListOfAttribsAndValues()
        //{
        //    var directorySearchResult = _searchResults;
        //    List<string> AttribList = new List<string>();
       
        //    for (var i = 0; i <= directorySearchResult.Properties.Count - 1; i++)
        //    {
        //        AttribList.Add(directorySearchResult.Properties.PropertyNames[i].ToString() + " = " + directorySearchResult.Properties[directorySearchResult.Properties.PropertyNames(i).ToString()][0].ToString()); // & directorySearchResult.Properties.Values.)
        //    }
        //    return AttribList;
        //}

        public string GetAttributeValue(string attribName)
        {
            var directorySearchResult = _searchResults;
            if (directorySearchResult != null)
            {
                if (directorySearchResult.Properties[attribName].Count > 0)
                {
                    return AttribValueToString(directorySearchResult.Properties[attribName][0]);
                }
            }
            return string.Empty;
        }

        private string AttribValueToString(object value)
        {
            try
            {
                if (value.GetType() == typeof(long))
                {
                    var longTime = System.Convert.ToInt64(value);
                    return DateTime.FromFileTime(longTime).ToString();
                }
                else
                {
                    return value.ToString();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// Parses a Distinguished Name string into a shorter more readable string.
        /// </summary>
        /// <param name="distName"></param>
        /// <returns></returns>
        private string ParsePath(string distName)
        {
            //Replace unwanted OU= text and split the string by commas.
            List<string> Elements = (distName.Replace("OU=", "")).Split(',').ToList();
            //OU Name of the topmost desired OU.
            string RootOUName = "FCAccounts";
            //Find the index of the RootOUName using a lambda expression.
            var RootIndex = Elements.FindIndex(e => e.Contains(RootOUName));
            string Path = "";
            //Iterate through the elements starting at 1 to skip the device name, stop at the RootIndex.
            for (var i = 1; i <= RootIndex; i++)
            {
                //Concant the user friendly path string.
                Path += Elements[i] + "/";
            }
            return Path;
        }

    }
}