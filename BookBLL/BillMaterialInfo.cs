using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookDAL;
using System.Data;

namespace BookBLL
{
    public class BillMaterialInfo
    {
        DalMaterialInfo dalMaterialInfo = new DalMaterialInfo();
        public DataTable BillGetMaterial()
        {
            DataTable dt = dalMaterialInfo.DalGetMaterial();
            return dt;
        }
        public bool BillCheckCode(string CODE)
        {
            bool code = dalMaterialInfo.DalCheckCode(CODE);
            return code;
        }
        public bool insertMaterialInfo(string cat_id, string code, string name, string spec, string factory, string grade, string permitno, string unit, string ratio, string price_rule, string retail_price, string trade_price, string max_price, string buying_price, string price_level, string high_level, string low_level, string state, string mid_unit)
        {
            bool Bool = dalMaterialInfo.insertMaterialInfo(cat_id,code, name, spec, factory, grade, permitno, unit, ratio, price_rule, retail_price, trade_price, max_price, buying_price, price_level, high_level, low_level, state, mid_unit);
            return Bool;
        }
        public bool updateMaterialInfo(string cat_id, string code, string name, string spec, string factory, string grade, string permitno, string unit, string ratio, string price_rule, string retail_price, string trade_price, string max_price, string buying_price, string price_level, string high_level, string low_level, string state, string mid_unit)
        {
            bool Bool = dalMaterialInfo.updateMaterialInfo(cat_id, code, name, spec, factory, grade, permitno, unit, ratio, price_rule, retail_price, trade_price, max_price, buying_price, price_level, high_level, low_level, state, mid_unit);
            return Bool;
        }
        public bool deleteMaterial(string CODE)
        {
            bool code = dalMaterialInfo.deleteMaterial(CODE);
            return code;
        }
    }
}
