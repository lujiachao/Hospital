using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;

namespace BookDAL
{
    public class DalMain
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        public string GetName(string Code)
        {
            string sql = @"SELECT NAME FROM PUB_EMP WHERE Code = " + Code + "";
            string name = sqlDBHelper.GetScalar(sql).ToString();
            return name;
        }
        public DataTable DalGetMissList()
        {
            string sql = @"SELECT NAME,OBJECT,PARENTID FROM SYS_OBJECT WHERE MODULE_CODE <> 'SYS1' AND TYPE <> '11' order by parentid";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        public DataTable DalGetMissList2()
        {
            string sql = @"SELECT NAME,OBJECT,PARENTID FROM SYS_OBJECT WHERE MODULE_CODE <> 'SYS1' AND TYPE = '11' OR TYPE = '33'";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
    }
}
