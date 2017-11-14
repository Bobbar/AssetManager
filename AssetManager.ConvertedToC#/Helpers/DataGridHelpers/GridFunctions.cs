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
using AssetManager.UserInterface.Forms;

namespace AssetManager
{
    public static class GridFunctions
    {

        public static void PopulateGrid(DataGridView grid, DataTable data, List<DataGridColumn> columns, bool forceRawData = false)
        {
            SetupGrid(grid, columns);
            using (data)
            {
                grid.DataSource = null;
                grid.DataSource = BuildDataSource(data, columns, forceRawData);
            }
        }

        private static DataTable BuildDataSource(DataTable data, List<DataGridColumn> columns, bool forceRawData)
        {
            var NeedsRebuilt = ColumnsRequireRebuild(columns);
            if (NeedsRebuilt & !forceRawData)
            {
                DataTable NewTable = new DataTable();
                foreach (DataGridColumn col in columns)
                {
                    NewTable.Columns.Add(col.ColumnName, col.ColumnType);
                }
                foreach (DataRow row in data.Rows)
                {
                    DataRow NewRow = null;
                    NewRow = NewTable.NewRow();

                    foreach (DataGridColumn col in columns)
                    {

                        switch (col.ColumnFormatType)
                        {
                            case ColumnFormatTypes.DefaultFormat:
                            case ColumnFormatTypes.AttributeCombo:
                                NewRow[col.ColumnName] = row[col.ColumnName];

                                break;
                            case ColumnFormatTypes.AttributeDisplayMemberOnly:
                                NewRow[col.ColumnName] = AttribIndexFunctions.GetDisplayValueFromCode(col.AttributeIndex, row[col.ColumnName].ToString());

                                break;
                            case ColumnFormatTypes.NotePreview:
                                var NoteText = OtherFunctions.RTFToPlainText(row[col.ColumnName].ToString());
                                NewRow[col.ColumnName] = OtherFunctions.NotePreview(NoteText);

                                break;
                            case ColumnFormatTypes.FileSize:
                                string HumanFileSize = Math.Round((Convert.ToInt32(row[col.ColumnName]) / 1024d), 1) + " KB";
                                NewRow[col.ColumnName] = HumanFileSize;

                                break;
                            case ColumnFormatTypes.Image:
                                NewRow[col.ColumnName] = FileIcon.GetFileIcon(row[col.ColumnName].ToString());

                                break;
                        }

                    }
                    NewTable.Rows.Add(NewRow);
                }
                return NewTable;
            }
            else
            {
                return data;
            }
        }

        private static bool ColumnsRequireRebuild(List<DataGridColumn> columns)
        {
            bool RebuildRequired = false;
            foreach (DataGridColumn col in columns)
            {
                switch (col.ColumnFormatType)
                {
                    case ColumnFormatTypes.AttributeDisplayMemberOnly:
                    case ColumnFormatTypes.NotePreview:
                    case ColumnFormatTypes.FileSize:
                    case ColumnFormatTypes.Image:
                        RebuildRequired = true;
                        break;
                }
            }
            return RebuildRequired;
        }

        private static void SetupGrid(DataGridView grid, List<DataGridColumn> columns)
        {
            grid.DataSource = null;
            grid.Rows.Clear();
            grid.Columns.Clear();
            grid.AutoGenerateColumns = false;
            foreach (DataGridColumn col in columns)
            {
                grid.Columns.Add(GetColumn(col));
            }
        }

        private static DataGridViewColumn GetColumn(DataGridColumn column)
        {
            switch (column.ColumnFormatType)
            {
                case ColumnFormatTypes.DefaultFormat:
                case ColumnFormatTypes.AttributeDisplayMemberOnly:
                case ColumnFormatTypes.NotePreview:
                case ColumnFormatTypes.FileSize:
                    return GenericColumn(column);
                case ColumnFormatTypes.AttributeCombo:
                    return DataGridComboColumn(column.AttributeIndex, column.ColumnCaption, column.ColumnName);
                case ColumnFormatTypes.Image:
                    return DataGridImageColumn(column);
            }
            return null;
        }

        private static DataGridViewColumn DataGridImageColumn(DataGridColumn column)
        {
            DataGridViewImageColumn NewCol = new DataGridViewImageColumn();
            NewCol.Name = column.ColumnName;
            NewCol.DataPropertyName = column.ColumnName;
            NewCol.HeaderText = column.ColumnCaption;
            NewCol.ValueType = column.ColumnType;
            NewCol.CellTemplate = new DataGridViewImageCell();
            NewCol.SortMode = DataGridViewColumnSortMode.Automatic;
            NewCol.ReadOnly = column.ColumnReadOnly;
            NewCol.Visible = column.ColumnVisible;
            NewCol.Width = 40;
            return NewCol;
        }

        private static DataGridViewColumn GenericColumn(DataGridColumn column)
        {
            DataGridViewColumn NewCol = new DataGridViewColumn();
            NewCol.Name = column.ColumnName;
            NewCol.DataPropertyName = column.ColumnName;
            NewCol.HeaderText = column.ColumnCaption;
            NewCol.ValueType = column.ColumnType;
            NewCol.CellTemplate = new DataGridViewTextBoxCell();
            NewCol.SortMode = DataGridViewColumnSortMode.Automatic;
            NewCol.ReadOnly = column.ColumnReadOnly;
            NewCol.Visible = column.ColumnVisible;
            return NewCol;
        }

        private static DataGridViewComboBoxColumn DataGridComboColumn(AttributeDataStruct[] indexType, string headerText, string name)
        {
            DataGridViewComboBoxColumn NewCombo = new DataGridViewComboBoxColumn();
            NewCombo.Items.Clear();
            NewCombo.HeaderText = headerText;
            NewCombo.DataPropertyName = name;
            NewCombo.Name = name;
            NewCombo.Width = 200;
            NewCombo.SortMode = DataGridViewColumnSortMode.Automatic;
            NewCombo.DisplayMember = nameof(AttributeDataStruct.DisplayValue);
            NewCombo.ValueMember = nameof(AttributeDataStruct.Code);
            NewCombo.DataSource = indexType;
            return NewCombo;
        }

        /// <summary>
        /// Returns a comma separated string containing the DB columns within a List(Of ColumnStruct). For use in queries.
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static string ColumnsString(List<DataGridColumn> columns)
        {
            string ColString = "";
            foreach (DataGridColumn col in columns)
            {
                ColString += col.ColumnName;
                if (columns.IndexOf(col) != columns.Count - 1)
                    ColString += ",";
            }
            return ColString;
        }

        public static int GetColIndex(DataGridView grid, string columnName)
        {
            try
            {
                return grid.Columns[columnName].Index;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static string GetCurrentCellValue(DataGridView grid, string columnName)
        {
            return DataConsistency.NoNull(grid[GetColIndex(grid, columnName), grid.CurrentRow.Index].Value.ToString());
        }

        public static void CopyToGridForm(DataGridView grid, ExtendedForm parentForm)
        {
            if (grid != null)
            {
                GridForm NewGridForm = new GridForm(parentForm, grid.Name + " Copy");
                NewGridForm.AddGrid(grid.Name, grid.Name, ((DataTable)grid.DataSource).Copy());
                NewGridForm.Show();
            }
        }

        public static void CopySelectedGridData(DataGridView grid)
        {
            grid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            Clipboard.SetDataObject(grid.GetClipboardContent());
            grid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText;
        }

    }
}
