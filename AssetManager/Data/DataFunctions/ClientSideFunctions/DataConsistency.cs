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
using System.Text.RegularExpressions;
namespace AssetManager
{

	static class DataConsistency
	{

		public const string strDBDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
		public static string NoNull(object DBVal)
		{
			try {
				return (Information.IsDBNull(DBVal) ? "" : DBVal.ToString()).ToString();
			} catch (Exception ex) {
				ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
				return "";
			}
		}

		/// <summary>
		/// Trims, removes LF and CR chars and returns a DBNull if string is empty.
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		public static object CleanDBValue(string Value)
		{
			string CleanString = Regex.Replace(Strings.Trim(Value), "[/\\r?\\n|\\r]+", string.Empty);
            if (CleanString == string.Empty)
            {
                return DBNull.Value;
            }
            else
            {
                return CleanString;
            }

          //  return (CleanString == string.Empty ? DBNull.Value : CleanString);
		}

		public static bool ValidPhoneNumber(string PhoneNum)
		{
			if (!string.IsNullOrEmpty(Strings.Trim(PhoneNum))) {
				const int nDigits = 10;
				string fPhoneNum = "";
				char[] NumArray = PhoneNum.ToCharArray();
				foreach (char num in NumArray) {
					if (char.IsDigit(num))
						fPhoneNum += num.ToString();
				}
				if (Strings.Len(fPhoneNum) != nDigits) {
					return false;
				} else {
					return true;
				}
			} else {
				return true;
			}
		}

		public static string YearFromDate(System.DateTime dtDate)
		{
			return dtDate.Year.ToString();
		}

		public static string DeviceHostnameFormat(string Serial)
		{
			return "D" + Strings.Trim(Serial);
		}

		public static bool IsValidYear(string Year)
		{
			try {
				if (!string.IsNullOrEmpty(Strings.Trim(Year))) {
					if (Enumerable.Range(1900, 200).Contains(Convert.ToInt32(Year))) {
						return true;
					}
				}
				return false;
			} catch {
				return false;
			}
		}

	}
}
