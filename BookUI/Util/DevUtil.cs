using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KingT.MMIS.Common
{
    public class DevUtil
    {
        /// <summary>
        /// 通过传入的gridView获取界面设置好的DataTable，包括绑定的字段类型
        /// </summary>
        /// <param name="gridView"></param>
        /// <returns></returns>
        public static DataTable GetGridControlBindingDataTable(DevExpress.XtraGrid.Views.Grid.GridView gridView)
        {
            DataTable dt = new DataTable();
            foreach (DevExpress.XtraGrid.Columns.GridColumn gridColumn in gridView.Columns)
            {
                if (gridColumn.UnboundType.ToString() == DevExpress.Data.UnboundColumnType.Boolean.ToString())
                    dt.Columns.Add(gridColumn.FieldName, typeof(Boolean));
                else if (gridColumn.UnboundType.ToString() == DevExpress.Data.UnboundColumnType.Bound.ToString())
                    dt.Columns.Add(gridColumn.FieldName);
                else if (gridColumn.UnboundType.ToString() == DevExpress.Data.UnboundColumnType.DateTime.ToString())
                    dt.Columns.Add(gridColumn.FieldName, typeof(DateTime));
                else if (gridColumn.UnboundType.ToString() == DevExpress.Data.UnboundColumnType.Decimal.ToString())
                    dt.Columns.Add(gridColumn.FieldName, typeof(Decimal));
                else if (gridColumn.UnboundType.ToString() == DevExpress.Data.UnboundColumnType.Integer.ToString())
                    dt.Columns.Add(gridColumn.FieldName, typeof(Int32));
                else if (gridColumn.UnboundType.ToString() == DevExpress.Data.UnboundColumnType.String.ToString())
                    dt.Columns.Add(gridColumn.FieldName, typeof(String));
            }
            return dt;
        }
    }
}
