using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookModel
{
    public class DataFieldAttribute : Attribute
    {
        private string _FieldName;
        private string _FieldType;
        public DataFieldAttribute(string fieldname, string fieldtype)
        {
            this._FieldName = fieldname;
            this._FieldType = fieldtype;
        }
        public string FieldName
        {
            get { return this._FieldName; }
            set { this._FieldName = value; }
        }
        public string FieldType
        {
            get { return this._FieldType; }
            set { this._FieldType = value; }
        }
    }
}
