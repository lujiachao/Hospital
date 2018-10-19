using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{
    public class BillCatalogInfo
    {
        DalCatalogInfo dalCatalogInfo = new DalCatalogInfo();
        public DataTable GetMaterialTreeListInfo(decimal acc_ID, int levelState)
        {
            DataTable dt = dalCatalogInfo.GetMaterialTreeListInfo(acc_ID, levelState);
            return dt;
        }
        public DataTable GetMaterialList(decimal catalogID)
        {
            DataTable dt = dalCatalogInfo.GetMaterialList(catalogID);
            return dt;
        }
        public bool BillCheckCode(string CODE)
        {
            bool code = dalCatalogInfo.DalCheckCode(CODE);
            return code;
        }
        public bool insertCatalog(string code, string name, string Account, string Type, string Price_rule, string App_mode, string inputcode1, string inputcode2, string State, string Cansell)
        {
            bool bl = dalCatalogInfo.insertCatalog(code,name,Account,Type,Price_rule,App_mode,inputcode1,inputcode2,State,Cansell);
            return bl;
        }
        public bool updateCatalog(string code, string name, string Account, string Type, string Price_rule, string App_mode, string inputcode1, string inputcode2, string State, string Cansell)
        {
            bool bl = dalCatalogInfo.updateCatalog(code, name, Account, Type, Price_rule, App_mode, inputcode1, inputcode2, State, Cansell);
            return bl;
        }
        public bool deleteCatalog(string code)
        {
            bool bl = dalCatalogInfo.deleteCatalog(code);
            return bl;
        }
    }
}
