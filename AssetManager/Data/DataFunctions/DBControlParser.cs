using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace AssetManager
{
    /// <summary>
    /// Determines how the parser handles the updating and filling of a control.
    /// </summary>
    public enum ParseType
    {
        /// <summary>
        /// The control is filled only.
        /// </summary>
        DisplayOnly,

        /// <summary>
        /// The control is filled and will also be added to Update and Insert tables.
        /// </summary>
        UpdateAndDisplay
    }
}

namespace AssetManager
{
    /// <summary>
    /// Instantiate and assign to <see cref="Control.Tag"/> property to enable DBParsing functions.
    /// </summary>
    public class DBControlInfo
    {
        #region "Fields"

        private AttributeDataStruct[] db_attrib_index;
        private string db_column;
        private ParseType db_parse_type;

        private bool db_required;

        #endregion "Fields"

        #region "Constructors"

        public DBControlInfo()
        {
            db_column = "";
            db_required = false;
            db_parse_type = ParseType.DisplayOnly;
            db_attrib_index = null;
        }

        public DBControlInfo(string dataColumn, ParseType parseType, bool required = false)
        {
            db_column = dataColumn;
            db_required = required;
            db_parse_type = parseType;
            db_attrib_index = null;
        }

        public DBControlInfo(string dataColumn, bool required = false)
        {
            db_column = dataColumn;
            db_required = required;
            db_parse_type = ParseType.UpdateAndDisplay;
            db_attrib_index = null;
        }

        public DBControlInfo(string dataColumn, AttributeDataStruct[] attribIndex, bool required = false)
        {
            db_column = dataColumn;
            db_required = required;
            db_parse_type = ParseType.UpdateAndDisplay;
            db_attrib_index = attribIndex;
        }

        public DBControlInfo(string dataColumn, AttributeDataStruct[] attribIndex, ParseType parseType, bool required = false)
        {
            db_column = dataColumn;
            db_required = required;
            db_parse_type = parseType;
            db_attrib_index = attribIndex;
        }

        #endregion "Constructors"

        #region "Properties"

        /// <summary>
        /// Gets or sets the <see cref="AttributeDataStruct"/> index for <see cref="ComboBox"/> controls.
        /// </summary>
        /// <returns></returns>
        public AttributeDataStruct[] AttribIndex
        {
            get { return db_attrib_index; }
            set { db_attrib_index = value; }
        }

        /// <summary>
        /// Gets or sets the Database Column used to update and/or populate the assigned control.
        /// </summary>
        /// <returns></returns>
        public string DataColumn
        {
            get { return db_column; }
            set { db_column = value; }
        }

        /// <summary>
        /// Gets or sets <seealso cref="ParseType"/>
        /// </summary>
        /// <returns></returns>
        public ParseType ParseType
        {
            get { return db_parse_type; }
            set { db_parse_type = value; }
        }

        /// <summary>
        /// Is the Control a required field?
        /// </summary>
        /// <returns></returns>
        public bool Required
        {
            get { return db_required; }
            set { db_required = value; }
        }

        #endregion "Properties"
    }
}

namespace AssetManager
{
    public struct DBRemappingInfo
    {
        public string FromColumnName { get; set; }
        public string ToColumnName { get; set; }

        public DBRemappingInfo(string fromColumn, string toColumn)
        {
            FromColumnName = fromColumn;
            ToColumnName = toColumn;
        }
    }
}

namespace AssetManager
{
    public class DBControlParser
    {
        #region "Fields"

        private Form ParentForm;

        #endregion "Fields"

        #region "Constructors"

        /// <summary>
        /// Instantiate new instance of <see cref="DBControlParser"/>
        /// </summary>
        /// <param name="parentForm">Form that contains controls initiated with <see cref="DBControlInfo"/> </param>
        public DBControlParser(Form parentForm)
        {
            this.ParentForm = parentForm;
        }

        #endregion "Constructors"

        #region "Methods"

        /// <summary>
        /// Populates all Controls in the ParentForm that have been initiated via <see cref="DBControlInfo"/> with their corresponding column names.
        /// </summary>
        /// <param name="data">DataTable that contains the rows and columns associated with the controls.</param>
        public void FillDBFields(DataTable data, List<DBRemappingInfo> remappingList = null)
        {
            DataRow Row = data.Rows[0];
            foreach (Control ctl in GetDBControls(ParentForm))
            {
                DBControlInfo DBInfo = (DBControlInfo)ctl.Tag;
                string DBColumn = null;

                if (remappingList != null)
                {
                    DBColumn = GetRemappedColumnName(DBInfo.DataColumn, remappingList);
                }
                else
                {
                    DBColumn = DBInfo.DataColumn;
                }

                if (Row.Table.Columns.Contains(DBColumn))
                {
                    #region "Type Finder - Dictionary method"

                    //var setDataMap = new Dictionary<Type, Delegate>
                    //{
                    //    {typeof(TextBox), new Action<Control>(c =>
                    //    {
                    //        TextBox dbTxt = (TextBox)c;
                    //        if (DBInfo.AttribIndex != null)
                    //        {
                    //          if (DBInfo.AttribIndex != null) {
                    //            dbTxt.Text = AttribIndexFunctions.GetDisplayValueFromCode(DBInfo.AttribIndex, Row[DBColumn].ToString());
                    //        } else {
                    //            dbTxt.Text = Row[DBColumn].ToString();
                    //        }
                    //        }
                    //    }) },
                    //    {typeof(MaskedTextBox), new Action<Control>(c =>
                    //    {
                    //        MaskedTextBox dbMaskTxt = (MaskedTextBox)c;
                    //        dbMaskTxt.Text = Row[DBColumn].ToString();
                    //    }) },
                    //    {typeof(DateTimePicker), new Action<Control>(c => {
                    //        DateTimePicker dbDtPick = (DateTimePicker)c;
                    //        dbDtPick.Value = DateTime.Parse(Row[DBColumn].ToString());
                    //    }) },
                    //    {typeof(ComboBox), new Action<Control>(c => {
                    //        ComboBox dbCmb = (ComboBox)c;
                    //        dbCmb.SelectedIndex = AttribIndexFunctions.GetComboIndexFromCode(DBInfo.AttribIndex, Row[DBColumn].ToString());
                    //     }) },
                    //    {typeof(Label), new Action<Label>(c => {
                    //       Label dbLbl = (Label)c;
                    //        dbLbl.Text = Row[DBColumn].ToString();
                    //     }) },
                    //    {typeof(CheckBox), new Action<CheckBox>(c => {
                    //      CheckBox dbChk = (CheckBox)c;
                    //        dbChk.Checked = Convert.ToBoolean(Row[DBColumn]);
                    //     }) },
                    //    {typeof(RichTextBox), new Action<RichTextBox>(c => {
                    //     RichTextBox dbRtb = (RichTextBox)c;
                    //     OtherFunctions.SetRichTextBox(ref dbRtb, Row[DBColumn].ToString());
                    //     }) }
                    //};

                    //var t = ctl.GetType();
                    //setDataMap[t].DynamicInvoke(ctl);

                    #endregion "Type Finder - Dictionary method"

                    #region "Type Finder - If ElseIf method"

                    Type ctlType = ctl.GetType();

                    if (ctlType == typeof(TextBox))
                    {
                        TextBox dbTxt = (TextBox)ctl;
                        if (DBInfo.AttribIndex != null)
                        {
                            dbTxt.Text = AttribIndexFunctions.GetDisplayValueFromCode(DBInfo.AttribIndex, Row[DBColumn].ToString());
                        }
                        else
                        {
                            dbTxt.Text = Row[DBColumn].ToString();
                        }
                    }
                    else if (ctlType == typeof(MaskedTextBox))
                    {
                        MaskedTextBox dbMaskTxt = (MaskedTextBox)ctl;
                        dbMaskTxt.Text = Row[DBColumn].ToString();
                    }
                    else if (ctlType == typeof(DateTimePicker))
                    {
                        DateTimePicker dbDtPick = (DateTimePicker)ctl;
                        dbDtPick.Value = DateTime.Parse(Row[DBColumn].ToString());
                    }
                    else if (ctlType == typeof(ComboBox))
                    {
                        ComboBox dbCmb = (ComboBox)ctl;
                        dbCmb.SelectedIndex = AttribIndexFunctions.GetComboIndexFromCode(DBInfo.AttribIndex, Row[DBColumn].ToString());
                    }
                    else if (ctlType == typeof(Label))
                    {
                        Label dbLbl = (Label)ctl;
                        dbLbl.Text = Row[DBColumn].ToString();
                    }
                    else if (ctlType == typeof(CheckBox))
                    {
                        CheckBox dbChk = (CheckBox)ctl;
                        dbChk.Checked = Convert.ToBoolean(Row[DBColumn]);
                    }
                    else if (ctlType == typeof(RichTextBox))
                    {
                        RichTextBox dbRtb = (RichTextBox)ctl;
                        OtherFunctions.SetRichTextBox(dbRtb, Row[DBColumn].ToString());
                    }
                    else
                    {
                        throw new Exception("Unexpected type.");
                    }

                    #endregion "Type Finder - If ElseIf method"

                    //               switch (true) {
                    //	case ctl is TextBox:
                    //		TextBox dbTxt = (TextBox)ctl;
                    //		if (DBInfo.AttribIndex != null) {
                    //			dbTxt.Text = AttribIndexFunctions.GetDisplayValueFromCode(DBInfo.AttribIndex, Row[DBColumn].ToString());
                    //		} else {
                    //			dbTxt.Text = Row[DBColumn].ToString();
                    //		}

                    //		break;
                    //	case ctl is MaskedTextBox:
                    //		MaskedTextBox dbMaskTxt = (MaskedTextBox)ctl;
                    //		dbMaskTxt.Text = Row[DBColumn].ToString();

                    //		break;
                    //	case ctl is DateTimePicker:
                    //		DateTimePicker dbDtPick = (DateTimePicker)ctl;
                    //		dbDtPick.Value = DateTime.Parse(Row[DBColumn].ToString());

                    //		break;
                    //	case ctl is ComboBox:
                    //		ComboBox dbCmb = (ComboBox)ctl;
                    //		dbCmb.SelectedIndex = AttribIndexFunctions.GetComboIndexFromCode(DBInfo.AttribIndex, Row[DBColumn].ToString());

                    //		break;
                    //	case ctl is Label:
                    //		Label dbLbl = (Label)ctl;
                    //		dbLbl.Text = Row[DBColumn].ToString();

                    //		break;
                    //	case ctl is CheckBox:
                    //		CheckBox dbChk = (CheckBox)ctl;
                    //		dbChk.Checked = Convert.ToBoolean(Row[DBColumn]);

                    //		break;
                    //	case ctl is RichTextBox:
                    //		RichTextBox dbRtb = (RichTextBox)ctl;
                    //		OtherFunctions.SetRichTextBox(ref dbRtb, Row[DBColumn].ToString());

                    //		break;
                    //	default:
                    //		throw new Exception("Unexpected type.");
                    //}
                }
            }
        }

        /// <summary>
        /// Get remapped column name for the specified column and remapping info. Returns new column name if a match is found in the map; otherwise, returns the original column name.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="mappingInfo"></param>
        /// <returns></returns>
        public string GetRemappedColumnName(string columnName, List<DBRemappingInfo> mappingInfo)
        {
            if (mappingInfo.Exists(m => m.ToColumnName == columnName))
            {
                return mappingInfo.Find(m => m.ToColumnName == columnName).FromColumnName;
            }
            else
            {
                return columnName;
            }
        }

        /// <summary>
        /// Recursively collects list of controls initiated with <see cref="DBControlInfo"/> tags within Parent control.
        /// </summary>
        /// <param name="parentForm">Parent control. Usually a Form to being.</param>
        /// <param name="controlList">Blank List of Control to be filled.</param>
        public List<Control> GetDBControls(Control parentForm, List<Control> controlList = null)
        {
            if (controlList == null)
                controlList = new List<Control>();
            foreach (Control ctl in parentForm.Controls)
            {
                if (ctl.Tag is DBControlInfo)
                {
                    controlList.Add(ctl);
                }

                //switch (true) {
                //	case ctl.Tag is DBControlInfo:
                //		controlList.Add(ctl);
                //		break;
                //}

                if (ctl.HasChildren)
                {
                    controlList.AddRange(GetDBControls(ctl));
                }
            }
            return controlList;
        }

        public object GetDBControlValue(Control dbControl)
        {
            var DBInfo = (DBControlInfo)dbControl.Tag;

            if (dbControl is TextBox)
            {
                TextBox dbTxt = (TextBox)dbControl;
                return DataConsistency.CleanDBValue(dbTxt.Text);
            }
            else if (dbControl is MaskedTextBox)
            {
                MaskedTextBox dbMaskTxt = (MaskedTextBox)dbControl;
                return DataConsistency.CleanDBValue(dbMaskTxt.Text);
            }
            else if (dbControl is DateTimePicker)
            {
                DateTimePicker dbDtPick = (DateTimePicker)dbControl;
                return dbDtPick.Value;
            }
            else if (dbControl is ComboBox)
            {
                ComboBox dbCmb = (ComboBox)dbControl;
                return AttribIndexFunctions.GetDBValue(DBInfo.AttribIndex, dbCmb.SelectedIndex);
            }
            else if (dbControl is CheckBox)
            {
                CheckBox dbChk = (CheckBox)dbControl;
                return dbChk.Checked;
            }
            else
            {
                throw new Exception("Unexpected type.");
                //return null;
            }

            //switch (true)
            //{
            //    case dbControl is TextBox:
            //        TextBox dbTxt = (TextBox)dbControl;

            //        return DataConsistency.CleanDBValue(dbTxt.Text);
            //    case dbControl is MaskedTextBox:
            //        MaskedTextBox dbMaskTxt = (MaskedTextBox)dbControl;

            //        return DataConsistency.CleanDBValue(dbMaskTxt.Text);
            //    case dbControl is DateTimePicker:
            //        DateTimePicker dbDtPick = (DateTimePicker)dbControl;

            //        return dbDtPick.Value;
            //    case dbControl is ComboBox:
            //        ComboBox dbCmb = (ComboBox)dbControl;

            //        return AttribIndexFunctions.GetDBValue(DBInfo.AttribIndex, dbCmb.SelectedIndex);
            //    case dbControl is CheckBox:
            //        CheckBox dbChk = (CheckBox)dbControl;
            //        return dbChk.Checked;
            //    default:
            //        throw new Exception("Unexpected type.");
            //}
            //return null;
        }

        /// <summary>
        /// Collects an EMPTY DataTable via a SQL Select statement and adds a new row for SQL Insertion.
        /// </summary>
        /// <remarks>
        /// The SQL SELECT statement should return an EMPTY table. A new row will be added to this table via <see cref="UpdateDBControlRow(ByRef DataRow)"/>
        /// </remarks>
        /// <param name="selectQry">A SQL Select query string that will return an EMPTY table. Ex: "SELECT * FROM table LIMIT 0"</param>
        /// <returns>
        /// Returns a DataTable with a new row added via <see cref="UpdateDBControlRow(ByRef DataRow)"/>
        /// </returns>
        public DataTable ReturnInsertTable(string selectQry)
        {
            DataTable tmpTable = null;
            tmpTable = AssetManager.DBFactory.GetDatabase().DataTableFromQueryString(selectQry);
            tmpTable.Rows.Add();
            //UpdateDBControlRow(ref tmpTable.Rows[0]);
            UpdateDBControlRow(tmpTable.Rows[0]);
            return tmpTable;
        }

        /// <summary>
        /// Collects a DataTable via a SQL Select statement and modifies the topmost row with data parsed from <seealso cref="UpdateDBControlRow(ByRef DataRow)"/>
        /// </summary>
        /// <remarks>
        /// The SQL SELECT statement should return a single row only. This is will be the table row that you wish to update.
        /// </remarks>
        /// <param name="selectQry">A SQL Select query string that will return the table row that is to be updated.</param>
        /// <returns>
        /// Returns a DataTable modified by the controls identified by <see cref="GetDBControls(Control, List(Of Control))"/>
        /// </returns>
        public DataTable ReturnUpdateTable(string selectQry)
        {
            DataTable tmpTable = new DataTable();
            tmpTable = AssetManager.DBFactory.GetDatabase().DataTableFromQueryString(selectQry);
            tmpTable.TableName = "UpdateTable";
            //UpdateDBControlRow(ref tmpTable.Rows[0]);
            UpdateDBControlRow(tmpTable.Rows[0]);
            return tmpTable;
        }

        /// <summary>
        /// Modifies a DataRow with data parsed from controls collected by <see cref="GetDBControls(Control, List(Of Control))"/>
        /// </summary>
        /// <param name="DBRow">DataRow to be modified.</param>
        //private void UpdateDBControlRow(ref DataRow DBRow)
        private void UpdateDBControlRow(DataRow DBRow)
        {
            foreach (Control ctl in GetDBControls(ParentForm))
            {
                DBControlInfo DBInfo = (DBControlInfo)ctl.Tag;
                if (DBInfo.ParseType != ParseType.DisplayOnly)
                {
                    if (ctl is TextBox)
                    {
                        TextBox dbTxt = (TextBox)ctl;
                        DBRow[DBInfo.DataColumn] = DataConsistency.CleanDBValue(dbTxt.Text);
                    }
                    else if (ctl is MaskedTextBox)
                    {
                        MaskedTextBox dbMaskTxt = (MaskedTextBox)ctl;
                        DBRow[DBInfo.DataColumn] = DataConsistency.CleanDBValue(dbMaskTxt.Text);
                    }
                    else if (ctl is DateTimePicker)
                    {
                        DateTimePicker dbDtPick = (DateTimePicker)ctl;
                        DBRow[DBInfo.DataColumn] = dbDtPick.Value;
                    }
                    else if (ctl is ComboBox)
                    {
                        ComboBox dbCmb = (ComboBox)ctl;
                        DBRow[DBInfo.DataColumn] = AttribIndexFunctions.GetDBValue(DBInfo.AttribIndex, dbCmb.SelectedIndex);
                    }
                    else if (ctl is CheckBox)
                    {
                        CheckBox dbChk = (CheckBox)ctl;
                        DBRow[DBInfo.DataColumn] = dbChk.Checked;
                    }
                    else
                    {
                        throw new Exception("Unexpected type.");
                        //return null;
                    }

                    //switch (true)
                    //{
                    //    case ctl is TextBox:
                    //        TextBox dbTxt = (TextBox)ctl;
                    //        DBRow[DBInfo.DataColumn] = DataConsistency.CleanDBValue(dbTxt.Text);

                    //        break;
                    //    case ctl is MaskedTextBox:
                    //        MaskedTextBox dbMaskTxt = (MaskedTextBox)ctl;
                    //        DBRow[DBInfo.DataColumn] = DataConsistency.CleanDBValue(dbMaskTxt.Text);

                    //        break;
                    //    case ctl is DateTimePicker:
                    //        DateTimePicker dbDtPick = (DateTimePicker)ctl;
                    //        DBRow[DBInfo.DataColumn] = dbDtPick.Value;

                    //        break;
                    //    case ctl is ComboBox:
                    //        ComboBox dbCmb = (ComboBox)ctl;
                    //        DBRow[DBInfo.DataColumn] = AttribIndexFunctions.GetDBValue(DBInfo.AttribIndex, dbCmb.SelectedIndex);

                    //        break;
                    //    case ctl is CheckBox:
                    //        CheckBox dbChk = (CheckBox)ctl;
                    //        DBRow[DBInfo.DataColumn] = dbChk.Checked;
                    //        break;
                    //    default:
                    //        throw new Exception("Unexpected type.");
                    //}
                }
            }
        }

        #endregion "Methods"
    }
}