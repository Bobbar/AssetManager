using System;
using System.Drawing;

namespace AssetManager
{
    public class DataGridColumn
    {
        public string ColumnName { get; set; }
        public string ColumnCaption { get; set; }
        public Type ColumnType { get; set; }
        public bool ColumnReadOnly { get; set; }
        public bool ColumnVisible { get; set; }
        public AttributeDataStruct[] AttributeIndex { get; set; }
        public ColumnFormatTypes ColumnFormatType { get; set; }

        public DataGridColumn(string colName, string caption, Type type)
        {
            ColumnName = colName;
            ColumnCaption = caption;
            ColumnType = type;
            ColumnReadOnly = false;
            ColumnVisible = true;
            AttributeIndex = null;
            ColumnFormatType = ColumnFormatTypes.DefaultFormat;
        }

        public DataGridColumn(string colName, string caption, Type type, ColumnFormatTypes displayMode)
        {
            ColumnName = colName;
            ColumnCaption = caption;
            ColumnType = type;
            ColumnReadOnly = false;
            ColumnVisible = true;
            AttributeIndex = null;
            ColumnFormatType = displayMode;
        }

        public DataGridColumn(string colName, string caption, AttributeDataStruct[] attribIndex)
        {
            ColumnName = colName;
            ColumnCaption = caption;
            ColumnType = typeof(string);
            ColumnReadOnly = false;
            ColumnVisible = true;
            this.AttributeIndex = attribIndex;
            ColumnFormatType = ColumnFormatTypes.AttributeCombo;
        }

        public DataGridColumn(string colName, string caption, AttributeDataStruct[] attribIndex, ColumnFormatTypes displayMode)
        {
            ColumnName = colName;
            ColumnCaption = caption;
            ColumnType = typeof(string);
            ColumnReadOnly = false;
            ColumnVisible = true;
            this.AttributeIndex = attribIndex;
            ColumnFormatType = displayMode;
        }

        public DataGridColumn(string colName, string caption, Type type, bool isReadOnly, bool visible)
        {
            ColumnName = colName;
            ColumnCaption = caption;
            ColumnType = type;
            ColumnReadOnly = isReadOnly;
            ColumnVisible = visible;
            AttributeIndex = null;
            ColumnFormatType = ColumnFormatTypes.DefaultFormat;
        }
    }
}

namespace AssetManager
{
    public enum ColumnFormatTypes
    {
        DefaultFormat,
        AttributeCombo,
        AttributeDisplayMemberOnly,
        NotePreview,
        Image,
        FileSize
    }
}

namespace AssetManager
{
    public struct StatusColumnColorStruct
    {
        public string StatusID;

        public Color StatusColor;

        public StatusColumnColorStruct(string id, Color color)
        {
            StatusID = id;
            StatusColor = color;
        }
    }
}