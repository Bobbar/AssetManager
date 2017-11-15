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
namespace AssetManager
{
    static class AttribIndexFunctions
    {

        public static void FillComboBox(AttributeDataStruct[] IndexType, ComboBox cmb)
        {
            cmb.Items.Clear();
            cmb.Text = "";
            foreach (AttributeDataStruct ComboItem in IndexType)
            {
                cmb.Items.Add(ComboItem.DisplayValue);
            }
        }

        public static void FillToolComboBox(AttributeDataStruct[] IndexType, ref ToolStripComboBox cmb)
        {
            cmb.Items.Clear();
            cmb.Text = "";
            int i = 0;
            foreach (AttributeDataStruct ComboItem in IndexType)
            {
                cmb.Items.Insert(i, ComboItem.DisplayValue);
                i += 1;
            }
        }

        public static string GetDBValue(AttributeDataStruct[] codeIndex, int index)
        {
            try
            {
                if (index > -1)
                {
                    return codeIndex[index].Code;
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetDisplayValueFromCode(AttributeDataStruct[] codeIndex, string code)
        {
            foreach (AttributeDataStruct item in codeIndex)
            {
                if (item.Code == code)
                    return item.DisplayValue;
            }
            return string.Empty;
        }

        public static string GetDisplayValueFromIndex(AttributeDataStruct[] codeIndex, int index)
        {
            return codeIndex[index].DisplayValue;
        }

        public static int GetComboIndexFromCode(AttributeDataStruct[] codeIndex, string code)
        {
            for (int i = 0; i <= codeIndex.Length - 1; i++)
            {
                if (codeIndex[i].Code == code)
                    return i;
            }
            return -1;
        }

        public static void PopulateAttributeIndexes()
        {
            var BuildIdxs = Task.Run(() =>
            {
                GlobalInstances.DeviceAttribute.Locations = BuildIndex(DevicesBaseCols.AttribTable, DeviceAttribType.Location);
                GlobalInstances.DeviceAttribute.ChangeType = BuildIndex(DevicesBaseCols.AttribTable, DeviceAttribType.ChangeType);
                GlobalInstances.DeviceAttribute.EquipType = BuildIndex(DevicesBaseCols.AttribTable, DeviceAttribType.EquipType);
                GlobalInstances.DeviceAttribute.OSType = BuildIndex(DevicesBaseCols.AttribTable, DeviceAttribType.OSType);
                GlobalInstances.DeviceAttribute.StatusType = BuildIndex(DevicesBaseCols.AttribTable, DeviceAttribType.StatusType);
                GlobalInstances.SibiAttribute.StatusType = BuildIndex(SibiRequestCols.AttribTable, SibiAttribType.SibiStatusType);
                GlobalInstances.SibiAttribute.ItemStatusType = BuildIndex(SibiRequestCols.AttribTable, SibiAttribType.SibiItemStatusType);
                GlobalInstances.SibiAttribute.RequestType = BuildIndex(SibiRequestCols.AttribTable, SibiAttribType.SibiRequestType);
                GlobalInstances.SibiAttribute.AttachFolder = BuildIndex(SibiRequestCols.AttribTable, SibiAttribType.SibiAttachFolder);
            });
            BuildIdxs.Wait();
        }

        public static AttributeDataStruct[] BuildIndex(string codeType, string typeName)
        {
            try
            {
                var strQRY = "SELECT * FROM " + codeType + " LEFT OUTER JOIN munis_codes on " + codeType + ".db_value = munis_codes.asset_man_code WHERE type_name ='" + typeName + "' ORDER BY " + ComboCodesBaseCols.DisplayValue + "";
                using (DataTable results = AssetManager.DBFactory.GetDatabase().DataTableFromQueryString(strQRY))
                {
                    List<AttributeDataStruct> tmpArray = new List<AttributeDataStruct>();
                    foreach (DataRow r in results.Rows)
                    {
                        string DisplayValue = "";
                        if (r.Table.Columns.Contains("munis_code"))
                        {
                            if (!Information.IsDBNull(r["munis_code"]))
                            {
                                DisplayValue = r[ComboCodesBaseCols.DisplayValue].ToString() + " - " + r["munis_code"].ToString();
                            }
                            else
                            {
                                DisplayValue = r[ComboCodesBaseCols.DisplayValue].ToString();
                            }
                        }
                        else
                        {
                            DisplayValue = r[ComboCodesBaseCols.DisplayValue].ToString();
                        }
                        tmpArray.Add(new AttributeDataStruct(DisplayValue, r[ComboCodesBaseCols.CodeValue].ToString(), Convert.ToInt32(r[ComboCodesBaseCols.ID])));
                    }
                    return tmpArray.ToArray();
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return null;
            }
        }

    }
}
