using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{
    public class BillMain
    {
        DalMain dalMain = new DalMain();
        public string GetName(string Code)
        {
            string name;
            name = dalMain.GetName(Code);
            return name;
        }
        public DataTable BillGetMissList()
        {
            DataTable dt = new DataTable();
            dt = dalMain.DalGetMissList();
            return dt;            
        }
        public DataTable BillGetMissList2()
        {
            DataTable dt = new DataTable();
            dt = dalMain.DalGetMissList2();
            return dt;
        }
    }
}
