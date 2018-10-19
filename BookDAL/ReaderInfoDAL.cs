using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.SqlClient;


namespace BookDAL
{
    public class ReaderInfoDAL
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        public bool CheckInformation(string Code, string Password)
        {
            string sql = @"SELECT COUNT(*) FROM PUB_EMP WHERE ID = "+Code+" AND PASSWORD = "+Password+"";
            int? count = sqlDBHelper.GetCount(sql);
            if (count != null && count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
