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
	[AttributeUsage(AttributeTargets.Property)]
	public class DataColumnNameAttribute : Attribute
	{


		private string _columnName;
		public string ColumnName {
			get { return _columnName; }
			set { _columnName = value; }
		}

		public DataColumnNameAttribute()
		{
			_columnName = string.Empty;
		}

		public DataColumnNameAttribute(string columnName)
		{
			_columnName = columnName;
		}

	}
}
