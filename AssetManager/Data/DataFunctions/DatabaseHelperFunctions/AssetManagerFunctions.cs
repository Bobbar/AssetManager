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
using AssetManager.UserInterface.CustomControls;
using AssetManager.UserInterface.Forms.AssetManagement;

namespace AssetManager
{
    public class AssetManagerFunctions
    {

        #region "Methods"

        public void AddNewEmp(MunisEmployeeStruct empInfo)
        {
            try
            {
                if (!IsEmployeeInDB(empInfo.Number))
                {
                    string UID = Guid.NewGuid().ToString();
                    List<DBParameter> InsertEmployeeParams = new List<DBParameter>();
                    InsertEmployeeParams.Add(new DBParameter(EmployeesCols.Name, empInfo.Name));
                    InsertEmployeeParams.Add(new DBParameter(EmployeesCols.Number, empInfo.Number));
                    InsertEmployeeParams.Add(new DBParameter(EmployeesCols.UID, UID));
                    AssetManager.DBFactory.GetDatabase().InsertFromParameters(EmployeesCols.TableName, InsertEmployeeParams);
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }


        /// <summary>
        /// Searches the database for the best possible match to the specified search name using a Levenshtein distance algorithm.
        /// </summary>
        /// <param name="empSearchName"></param>
        /// <param name="MinSearchDistance"></param>
        /// <returns></returns>
        public MunisEmployeeStruct SmartEmployeeSearch(string empSearchName, int MinSearchDistance = 10)
        {

            if (empSearchName.Trim() != "")
            {
                string[] SplitName = empSearchName.Split(char.Parse(" "));
                // Dim LevDist As New List(Of Integer)
                List<SmartEmpSearchStruct> Results = new List<SmartEmpSearchStruct>();

                //Get results for complete name from employees table
                Results.AddRange(GetEmpSearchResults(EmployeesCols.TableName, empSearchName, EmployeesCols.Name, EmployeesCols.Number));

                //Get results for complete name from devices table
                Results.AddRange(GetEmpSearchResults(DevicesCols.TableName, empSearchName, DevicesCols.CurrentUser, DevicesCols.MunisEmpNum));

                foreach (string s in SplitName)
                {

                    //Get results for partial name from employees table
                    Results.AddRange(GetEmpSearchResults(EmployeesCols.TableName, s, EmployeesCols.Name, EmployeesCols.Number));

                    //Get results for partial name from devices table
                    Results.AddRange(GetEmpSearchResults(DevicesCols.TableName, s, DevicesCols.CurrentUser, DevicesCols.MunisEmpNum));

                }

                if (Results.Count > 0)
                {
                    Results = NarrowResults(Results);
                    var BestMatch = FindBestSmartSearchMatch(Results);
                    if (BestMatch.MatchDistance < MinSearchDistance)
                    {
                        return BestMatch.SearchResult;
                    }
                }
            }
            return new MunisEmployeeStruct();
        }




        /// <summary>
        /// Reprocesses the search results to obtain a more accurate Levenshtein distance calculation.
        /// </summary>
        /// <param name="results"></param>
        /// <remarks>This is done because the initial calculations are performed against the full length
        /// of the returned names (First and last name), and the distance between the search string and name string may be inaccurate.</remarks>
        /// <returns></returns>
        private List<SmartEmpSearchStruct> NarrowResults(List<SmartEmpSearchStruct> results)
        {
            List<SmartEmpSearchStruct> newResults = new List<SmartEmpSearchStruct>();
            //Iterate through results
            foreach (var result in results)
            {
                //Split the results returned string by spaces
                var resultSplit = result.SearchResult.Name.Split(char.Parse(" "));
                if (resultSplit.Count() > 0)
                {
                    //Iterate through the separate strings
                    foreach (var item in resultSplit)
                    {
                        //Make sure the result string contains the search string
                        if (item.Contains(result.SearchString) && item.StartsWith(result.SearchString))
                        {
                            //Get a new Levenshtein distance.
                            var NewDistance = Fastenshtein.Levenshtein.Distance(item, result.SearchString);
                            //If the strings are closer together, add the new data.
                            if (NewDistance < result.MatchDistance)
                            {
                                newResults.Add(new SmartEmpSearchStruct(result.SearchResult, result.SearchString, NewDistance));
                            }
                            else
                            {
                                newResults.Add(result);
                            }
                        }
                        else
                        {
                            newResults.Add(result);
                        }
                    }
                }
            }
            return newResults;
        }

        /// <summary>
        /// Finds the best match within the results. The item with shortest Levenshtein distance and the longest match length (string length) is preferred.
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private SmartEmpSearchStruct FindBestSmartSearchMatch(List<SmartEmpSearchStruct> results)
        {
            //Initial minimum distance
            int MinDist = System.Convert.ToInt32(results.First().MatchDistance);
            //Initial minimum match
            SmartEmpSearchStruct MinMatch = results.First();
            SmartEmpSearchStruct LongestMatch = new SmartEmpSearchStruct();
            List<SmartEmpSearchStruct> DeDupDist = new List<SmartEmpSearchStruct>();
            //Iterate through the results and determine the result with the shortest Levenshtein distance.
            foreach (var result in results)
            {
                if (result.MatchDistance < MinDist)
                {
                    MinDist = System.Convert.ToInt32(result.MatchDistance);
                    MinMatch = result;

                }
            }
            //De-duplicate the results and iterate to determine which result of the Levenshtein shortest distances has the longest match length. (Greatest number of matches)
            DeDupDist = results.Distinct().ToList();
            if (DeDupDist.Count > 0)
            {
                int MaxMatchLen = 0;
                foreach (var dup in DeDupDist)
                {
                    if (dup.MatchDistance == MinDist)
                    {
                        if (dup.MatchLength > MaxMatchLen)
                        {
                            MaxMatchLen = System.Convert.ToInt32(dup.MatchLength);
                            LongestMatch = dup;
                        }
                    }
                }
                //Return best match by length and Levenshtein distance.
                return LongestMatch;
            }
            //Return best match by Levenshtein distance only. (If no duplicates)
            return MinMatch;
        }


        /// <summary>
        /// Queries the database for a list of results that contains the employee name result and computed Levenshtein distance to the search string.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="searchEmpName"></param>
        /// <param name="empNameColumn"></param>
        /// <param name="empNumColumn"></param>
        /// <returns></returns>
        private List<SmartEmpSearchStruct> GetEmpSearchResults(string tableName, string searchEmpName, string empNameColumn, string empNumColumn)
        {
            List<SmartEmpSearchStruct> tmpResults = new List<SmartEmpSearchStruct>();
            List<DBQueryParameter> EmpSearchParams = new List<DBQueryParameter>();
            EmpSearchParams.Add(new DBQueryParameter(empNameColumn, searchEmpName, false));
            using (DataTable data = DBFactory.GetDatabase().DataTableFromParameters("SELECT * FROM " + tableName + " WHERE", EmpSearchParams))
            {
                foreach (DataRow row in data.Rows)
                {
                    tmpResults.Add(new SmartEmpSearchStruct(new MunisEmployeeStruct(row[empNameColumn].ToString(), row[empNumColumn].ToString()), searchEmpName, Fastenshtein.Levenshtein.Distance(searchEmpName.ToUpper(), row[empNameColumn].ToString().ToUpper())));
                }
            }
            return tmpResults;
        }

        public bool DeleteMasterSqlEntry(string sqlGUID, EntryType type)
        {
            try
            {
                string DeleteQuery = "";
                switch (type)
                {
                    case EntryType.Device:
                        DeleteQuery = "DELETE FROM " + DevicesCols.TableName + " WHERE " + DevicesCols.DeviceUID + "='" + sqlGUID + "'";
                        break;
                    case EntryType.Sibi:
                        DeleteQuery = "DELETE FROM " + SibiRequestCols.TableName + " WHERE " + SibiRequestCols.UID + "='" + sqlGUID + "'";
                        break;
                }
                if (AssetManager.DBFactory.GetDatabase().ExecuteQuery(DeleteQuery) > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return false;
            }
        }

        public bool DeleteFtpAndSql(string sqlGUID, EntryType type)
        {
            try
            {
                if (GlobalInstances.FTPFunc.HasFtpFolder(sqlGUID))
                {
                    if (GlobalInstances.FTPFunc.DeleteFtpFolder(sqlGUID))
                        return DeleteMasterSqlEntry(sqlGUID, type);
                    // if has attachments, delete ftp directory, then delete the sql records.
                }
                else
                {
                    return DeleteMasterSqlEntry(sqlGUID, type);
                    //delete sql records
                }
            }
            catch (Exception ex)
            {
                return ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            return false;
        }

        public int DeleteSqlAttachment(Attachment attachment)
        {
            try
            {
                var AttachmentFolderID = GetSqlValue(attachment.AttachTable.TableName, attachment.AttachTable.FileUID, attachment.FileUID, attachment.AttachTable.FKey);
                //Delete FTP Attachment
                if (GlobalInstances.FTPFunc.DeleteFtpAttachment(attachment.FileUID, AttachmentFolderID))
                {
                    //delete SQL entry
                    var SQLDeleteQry = "DELETE FROM " + attachment.AttachTable.TableName + " WHERE " + attachment.AttachTable.FileUID + "='" + attachment.FileUID + "'";
                    return AssetManager.DBFactory.GetDatabase().ExecuteQuery(SQLDeleteQry);
                }
                return -1;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return -1;
            }
        }

        public bool DeviceExists(string assetTag, string serial)
        {
            bool bolAsset = false;
            bool bolSerial = false;
            //Allow NA value because users do not always have an Asset Tag for new devices.
            if (assetTag == "NA")
            {
                bolAsset = false;
            }
            else
            {
                string CheckAsset = GetSqlValue(DevicesCols.TableName, DevicesCols.AssetTag, assetTag, DevicesCols.AssetTag);
                if (!string.IsNullOrEmpty(CheckAsset))
                {
                    bolAsset = true;
                }
                else
                {
                    bolAsset = false;
                }
            }

            string CheckSerial = GetSqlValue(DevicesCols.TableName, DevicesCols.Serial, serial, DevicesCols.Serial);
            if (!string.IsNullOrEmpty(CheckSerial))
            {
                bolSerial = true;
            }
            else
            {
                bolSerial = false;
            }
            if (bolSerial | bolAsset)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable DevicesBySupervisor(ExtendedForm parentForm)
        {
            try
            {
                MunisEmployeeStruct SupInfo = default(MunisEmployeeStruct);
                using (MunisUserForm NewMunisSearch = new MunisUserForm(parentForm))
                {
                    if (NewMunisSearch.DialogResult == DialogResult.Yes)
                    {
                        OtherFunctions.SetWaitCursor(true, parentForm);
                        SupInfo = NewMunisSearch.EmployeeInfo;
                        using (DataTable DeviceList = new DataTable())
                        {
                            using (DataTable EmpList = GlobalInstances.MunisFunc.ListOfEmpsBySup(SupInfo.Number))
                            {
                                foreach (DataRow r in EmpList.Rows)
                                {
                                    string strQRY = "SELECT * FROM " + DevicesCols.TableName + " WHERE " + DevicesCols.MunisEmpNum + "='" + r["a_employee_number"].ToString() + "'";
                                    using (DataTable tmpTable = AssetManager.DBFactory.GetDatabase().DataTableFromQueryString(strQRY))
                                    {
                                        DeviceList.Merge(tmpTable);
                                    }
                                }
                                return DeviceList;
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            finally
            {
                OtherFunctions.SetWaitCursor(false, parentForm);
            }
        }

        public bool IsEmployeeInDB(string empNum)
        {
            string EmpName = GetSqlValue(EmployeesCols.TableName, EmployeesCols.Number, empNum, EmployeesCols.Name);
            if (!string.IsNullOrEmpty(EmpName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DeviceObject FindDeviceFromAssetOrSerial(string searchVal, FindDevType type)
        {
            try
            {
                if (type == FindDevType.AssetTag)
                {
                    List<DBQueryParameter> Params = new List<DBQueryParameter>();
                    Params.Add(new DBQueryParameter(DevicesCols.AssetTag, searchVal, true));
                    return new DeviceObject(AssetManager.DBFactory.GetDatabase().DataTableFromParameters("SELECT * FROM " + DevicesCols.TableName + " WHERE ", Params));
                }
                else if (type == FindDevType.Serial)
                {
                    List<DBQueryParameter> Params = new List<DBQueryParameter>();
                    Params.Add(new DBQueryParameter(DevicesCols.Serial, searchVal, true));
                    return new DeviceObject(AssetManager.DBFactory.GetDatabase().DataTableFromParameters("SELECT * FROM " + DevicesCols.TableName + " WHERE ", Params));
                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return null;
            }
        }

        public string GetTVApiToken()
        {
            using (DataTable results = DBFactory.GetDatabase().DataTableFromQueryString("SELECT apitoken FROM teamviewer_info"))
            {
                var token = results.Rows[0]["apitoken"].ToString();
                return token;
            }
        }


        public DeviceObject GetDeviceInfoFromGUID(string deviceGUID)
        {
            return new DeviceObject(AssetManager.DBFactory.GetDatabase().DataTableFromQueryString("SELECT * FROM " + DevicesCols.TableName + " WHERE " + DevicesCols.DeviceUID + "='" + deviceGUID + "'"));
        }

        public string GetMunisCodeFromAssetCode(string assetCode)
        {
            return GetSqlValue("munis_codes", "asset_man_code", assetCode, "munis_code");
        }

        public string GetSqlValue(string table, string fieldIn, string valueIn, string fieldOut)
        {
            string sqlQRY = "SELECT " + fieldOut + " FROM " + table + " WHERE ";
            List<DBQueryParameter> Params = new List<DBQueryParameter>();
            Params.Add(new DBQueryParameter(fieldIn, valueIn, true));
            var Result = AssetManager.DBFactory.GetDatabase().ExecuteScalarFromCommand(AssetManager.DBFactory.GetDatabase().GetCommandFromParams(sqlQRY, Params));
            if (Result != null)
            {
                return Result.ToString();
            }
            else
            {
                return "";
            }
        }

        public int GetAttachmentCount(string attachFolderUID, AttachmentsBaseCols attachTable)
        {
            try
            {
                return Convert.ToInt32(GetSqlValue(attachTable.TableName, attachTable.FKey, attachFolderUID, "COUNT(*)"));
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return 0;
            }
        }

        public int UpdateSqlValue(string table, string fieldIn, string valueIn, string idField, string idValue)
        {
            return AssetManager.DBFactory.GetDatabase().UpdateValue(table, fieldIn, valueIn, idField, idValue);
        }

        #endregion

    }
}
