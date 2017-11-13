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
	/// <summary>
	/// Data mapper for classes tagged with <see cref="DataColumnNameAttribute"/>
	/// </summary>
	public class DataMapping
	{

		public void MapClassProperties(object obj, object data)
		{
			if (data is DataTable) {
				var row = ((DataTable)data).Rows[0];
				MapProperty(obj, row);
			} else if (data is DataRow) {
				MapProperty(obj, (DataRow)data);
			} else {
				throw new Exception("Invalid data object type.");
			}
		}

		public void MapClassProperties(object obj, DataRow row)
		{
			MapProperty(obj, row);
		}

		public void MapClassProperties(object obj, DataTable data)
		{
			var row = data.Rows[0];
			MapProperty(obj, row);
		}

		/// <summary>
		/// Uses reflection to recursively populate/map class properties that are marked with a <see cref="DataColumnNameAttribute"/>.
		/// </summary>
		/// <param name="obj">Object to be populated.</param>
		/// <param name="row">DataRow with columns matching the <see cref="DataColumnNameAttribute"/> in the objects properties.</param>
		private void MapProperty(object obj, DataRow row)
		{
			//Collect list of all properties in the object class.
			List<System.Reflection.PropertyInfo> Props = (obj.GetType().GetProperties().ToList());

			//Iterate through the properties.

			foreach (System.Reflection.PropertyInfo prop in Props) {
				
				//Check if the property contains a target attribute.

				if (prop.GetCustomAttributes(typeof(DataColumnNameAttribute), true).Length > 0) {
					//Get the column name attached to the property.
					var propColumn = ((DataColumnNameAttribute)prop.GetCustomAttributes(false)[0]).ColumnName;

					//Make sure the DataTable contains a matching column name.

					if (row.Table.Columns.Contains(propColumn))
                    {
                        //Check the type of the propery and set its value accordingly.

                        if (prop.PropertyType == typeof(string))
                        {
                            prop.SetValue(obj, row[propColumn].ToString(), null);
                        }
                        else if (prop.PropertyType == typeof(DateTime))
                        {
                            DateTime pDate = default(DateTime);
                            if (DateTime.TryParse(DataConsistency.NoNull(row[propColumn].ToString()), out pDate))
                            {
                                prop.SetValue(obj, pDate);
                            }
                            else
                            {
                                prop.SetValue(obj, null);
                            }
                        }
                        else if (prop.PropertyType == typeof(bool))
                        {
                            prop.SetValue(obj, Convert.ToBoolean(row[propColumn]));
                        }
                        else if (prop.PropertyType == typeof(int))
                        {
                            prop.SetValue(obj, Convert.ToInt32(row[propColumn]));
                        }
                        else
                        {
                            //Throw an error if type is unexpected.
                            Debug.Print(prop.PropertyType.ToString());
                            throw new Exception("Unexpected property type.");
                        }
                    }
				//If the property does not contain a target attribute, check to see if it is a nested class inheriting the DataMapping class.
				} else {

					if (typeof(DataMapping).IsAssignableFrom(prop.PropertyType)) {
						//Recurse with nested DataMapping properties.
						MapProperty(prop.GetValue(obj, null), row);
					}
				}
			}
		}

	}
}
