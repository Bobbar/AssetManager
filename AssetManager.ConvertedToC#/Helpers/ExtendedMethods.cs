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
using System.Data.Common;
using System.Reflection;
namespace AssetManager
{

	static class ExtendedMethods
	{

		public static void DoubleBufferedDataGrid(DataGridView dgv, bool setting)
		{
			Type dgvType = dgv.GetType();
			PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
			pi.SetValue(dgv, setting, null);
		}

		public static void DoubleBufferedListView(ListView dgv, bool setting)
		{
			Type dgvType = dgv.GetType();
			PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
			pi.SetValue(dgv, setting, null);
		}

		public static void DoubleBufferedListBox(ListBox dgv, bool setting)
		{
			Type dgvType = dgv.GetType();
			PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
			pi.SetValue(dgv, setting, null);
		}

		public static void DoubleBufferedFlowLayout(FlowLayoutPanel dgv, bool setting)
		{
			Type dgvType = dgv.GetType();
			PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
			pi.SetValue(dgv, setting, null);
		}

		public static void DoubleBufferedTableLayout(TableLayoutPanel dgv, bool setting)
		{
			Type dgvType = dgv.GetType();
			PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
			pi.SetValue(dgv, setting, null);
		}

		public static void DoubleBufferedPanel(Panel dgv, bool setting)
		{
			Type dgvType = dgv.GetType();
			PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
			pi.SetValue(dgv, setting, null);
		}

		/// <summary>
		/// Adds a parameter to the command.
		/// </summary>
		/// <param name="command">
		/// The command.
		/// </param>
		/// <param name="parameterName">
		/// Name of the parameter.
		/// </param>
		/// <param name="parameterValue">
		/// The parameter value.
		/// </param>
		/// <remarks>
		/// </remarks>
		//[System.Runtime.CompilerServices.Extension()]
		public static void AddParameterWithValue(this DbCommand command, string parameterName, object parameterValue)
		{
			var parameter = command.CreateParameter();
			parameter.ParameterName = parameterName;
			parameter.Value = parameterValue;
			command.Parameters.Add(parameter);
		}

	}
}
