using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BookDAL;

namespace BookBLL
{
    public class BillImageComBox
    {
        DalImageComBox dalImageComBox = new DalImageComBox();
        public DataTable BillImageCombox()
        {
            DataTable dt = dalImageComBox.DalImageCombox();
            return dt;
        }
        public DataTable BillImageComboxAccount()
        {
            DataTable dt = dalImageComBox.DalImageComboxAccount();
            return dt;
        }
        public DataTable BillImageComboxStorage()
        {
            DataTable dt = dalImageComBox.DalImageComboxStorage();
            return dt;
        }
        public DataTable BillImageComboxFactory()
        {
            DataTable dt = dalImageComBox.DalImageComboxFactory();
            return dt;
        }
        public DataTable BillImageComboxCatalog()
        {
            DataTable dt = dalImageComBox.DalImageComboxCatalog();
            return dt;
        }
        public DataTable BillImageComboxMatClass()
        {
            DataTable dt = dalImageComBox.DalImageComboxMatClass();
            return dt;
        }
        public DataTable DalImageComboxBinType(decimal acc_ID)
        {
            DataTable dt = dalImageComBox.DalImageComboxBinType(acc_ID);
            return dt;
        }
        public DataTable DalImageComboxBoutType(decimal acc_ID)
        {
            DataTable dt = dalImageComBox.DalImageComboxBoutType(acc_ID);
            return dt;
        }
        public DataTable BillImageComboxCompany()
        {
            DataTable dt = dalImageComBox.DalImageComboxCompany();
            return dt;
        }
        public DataTable BillImageComboxCompany1()
        {
            DataTable dt = dalImageComBox.DalImageComboxCompany1();
            return dt;
        }
        public DataTable DalImageComboxStorageClass(decimal sto_ID,decimal acc_ID)
        {
            DataTable dt = dalImageComBox.DalImageComboxStorageClass(sto_ID,acc_ID);
            return dt;
        }
    }
}
