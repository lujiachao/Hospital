using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;

namespace BookDAL
{
    public class DalImageComBox
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        //会计人员
        public DataTable DalImageCombox()
        {
            string sql = @"SELECT CODE,NAME FROM PUB_EMP ";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        //账册
        public DataTable DalImageComboxAccount()
        {
            string sql = @"SELECT CODE,NAME FROM MMIS_ACCOUNT_TYPE ";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        //仓库
        public DataTable DalImageComboxStorage()
        {
            string sql = @"SELECT ID,CODE,NAME FROM MMIS_STORAGE ";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        //上下级关系仓库
        public DataTable DalImageComboxStorageClass(decimal sto_ID,decimal acc_ID)
        {
            string sql = @"SELECT B.TO_ID AS ID,B.TO_ID||','||(SELECT NAME FROM MMIS_STORAGE WHERE ID=B.TO_ID ) AS NAME FROM MMIS_STORAGE A ,MMIS_STORAGE_RELATION B WHERE A.ID = "+sto_ID+" AND A.ID = B.STO_ID AND B.ACC_id = "+acc_ID+"";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        //厂商
        public DataTable DalImageComboxFactory()
        {
            string sql = @"SELECT CODE,NAME FROM MMIS_FACTORY ";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        //通用目录
        public DataTable DalImageComboxCatalog()
        {
            string sql = @"SELECT CODE,NAME FROM MMIS_CATALOG ";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        //物资分类
        public DataTable DalImageComboxMatClass()
        {
            string sql = @"SELECT CODE,NAME FROM MMIS_MAT_CLASS ";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        //入库方式分类
        public DataTable DalImageComboxBinType(decimal acc_ID)
        {
            string sql = @"SELECT ID,NAME FROM MMIS_BIN_TYPE WHERE ACC_ID = "+acc_ID+"";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        //出库方式分类
        public DataTable DalImageComboxBoutType(decimal acc_ID)
        {
            string sql = @"SELECT ID,NAME FROM MMIS_BOUT_TYPE WHERE ACC_ID = " + acc_ID + "";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        //往来单位
        public DataTable DalImageComboxCompany()
        {
            string sql = @"SELECT ID,NAME FROM MMIS_COMPANY ";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        //往来单位
        public DataTable DalImageComboxCompany1()
        {
            string sql = @"SELECT ID,ID||','||NAME AS NAME FROM MMIS_COMPANY";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
    }
}
