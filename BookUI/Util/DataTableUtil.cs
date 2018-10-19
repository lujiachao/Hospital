using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KingT.MMIS.Common
{
    public class DataTableUtil
    {
        /// <summary>
        /// 去除DataTable的DBNull约束
        /// </summary>
        /// <param name="dt">需要去除约束的DataTable</param>
        public static void RemoveDBNull(ref DataTable dt)
        {
            if (dt != null && dt.Columns != null)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    dc.AllowDBNull = true;
                }
            }
        }
    }
}
