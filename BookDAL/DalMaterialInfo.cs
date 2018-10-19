using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tool;
using System.Data;
using System.Data.OleDb;

namespace BookDAL
{
    public class DalMaterialInfo
    {
        SqlDBHelper sqlDBHelper = new SqlDBHelper();
        public DataTable DalGetMaterial()
        {
            string sql = @"SELECT * FROM MMIS_MATERIAL ORDER BY CODE";
            DataTable dt = sqlDBHelper.GetTable(sql);
            return dt;
        }
        public bool DalCheckCode(string CODE)
        {
            string sql = @"SELECT COUNT(*) FROM MMIS_MATERIAL WHERE CODE = '" + CODE + "'";
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
        public bool insertMaterialInfo(string cat_id,string code, string name, string spec,string factory,string grade,string permitno,string unit,string ratio,string price_rule,string retail_price,string trade_price,string max_price,string buying_price,string price_level,string high_level,string low_level,string state,string mid_unit)
        {
            StringBuilder strSql = new StringBuilder();
            int? id = Convert.ToInt32(code);
            int? Cat_id = Convert.ToInt32(cat_id);
            int? Factory = factory == "" ? 0:Convert.ToInt32(factory);
            char Grade = Convert.ToChar(grade);
            int? Ratio = Convert.ToInt32(ratio);
            int? Price_rule = Convert.ToInt32(price_rule);
            double? Retail_price = Convert.ToDouble(retail_price);
            double? Trade_price = Convert.ToDouble(trade_price);
            double? Max_price = Convert.ToDouble(max_price);
            double? Buying_price = Convert.ToDouble(buying_price);
            char Price_level = Convert.ToChar(price_level);
            int? High_level = high_level == "" ? 0:Convert.ToInt32(high_level);
            int? Low_level = low_level == "" ? 0 : Convert.ToInt32(low_level);
            char State = Convert.ToChar(state);
            int? Next_seq = id + 1;
            strSql.Append("INSERT INTO MMIS_MATERIAL(");
            strSql.Append("ID,CAT_ID,CODE,NAME,SPEC,FACTORY,GRADE,PERMITNO,UNIT,RATIO,PRICE_RULE,RETAIL_PRICE,TRADE_PRICE,MAX_PRICE,BUYING_PRICE,PRICE_LEVEL,HIGH_LEVEL,LOW_LEVEL,STATE,NEXT_SEQ,MID_UNIT)");
            strSql.Append(" values (");
            strSql.Append(":id,:cat_id,:code,:name,:spec,:factory,:grade,:permitno,:unit,:ratio,:price_rule,:retail_price,:trade_price,:max_price,:buying_price,:price_level,:high_level,:low_level,:state,:next_seq,:mid_unit)");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":id", OleDbType.Numeric,8),
                    new OleDbParameter(":cat_id", OleDbType.Numeric,5),
                    new OleDbParameter(":code", OleDbType.VarChar,16),
					new OleDbParameter(":name", OleDbType.VarChar,64),
                    new OleDbParameter(":spec", OleDbType.VarChar,32),
                    new OleDbParameter(":factory", OleDbType.Numeric,5),
					new OleDbParameter(":grade", OleDbType.Char,2),
					new OleDbParameter(":permitno", OleDbType.VarChar,32),
					new OleDbParameter(":unit", OleDbType.VarChar,16),
					new OleDbParameter(":ratio", OleDbType.Numeric,8),
					new OleDbParameter(":price_rule", OleDbType.Numeric,2),
					new OleDbParameter(":retail_price", OleDbType.Numeric,16),
                    new OleDbParameter(":trade_price", OleDbType.Numeric,16),
                    new OleDbParameter(":max_price", OleDbType.Numeric,16),
                    new OleDbParameter(":buying_price", OleDbType.Numeric,16),
                    new OleDbParameter(":price_level", OleDbType.Char,2),
                    new OleDbParameter(":high_level", OleDbType.Numeric,5),
                    new OleDbParameter(":low_level", OleDbType.Numeric,5),
                    new OleDbParameter(":state", OleDbType.Char,2),
                    new OleDbParameter(":next_seq", OleDbType.Numeric,5),
                    new OleDbParameter(":mid_unit", OleDbType.VarChar,20)};
            parameters[0].Value = id;
            parameters[1].Value = Cat_id;
            parameters[2].Value = code;
            parameters[3].Value = name;
            parameters[4].Value = spec;
            parameters[5].Value = Factory;
            parameters[6].Value = Grade;
            parameters[7].Value = permitno;
            parameters[8].Value = unit;
            parameters[9].Value = Ratio;
            parameters[10].Value = Price_rule;
            parameters[11].Value = Retail_price;
            parameters[12].Value = Trade_price;
            parameters[13].Value = Max_price;
            parameters[14].Value = Buying_price;
            parameters[15].Value = Price_level;
            parameters[16].Value = High_level;
            parameters[17].Value = Low_level;
            parameters[18].Value = State;
            parameters[19].Value = Next_seq;
            parameters[20].Value = mid_unit;
            int rows = SqlDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool updateMaterialInfo(string cat_id, string code, string name, string spec, string factory, string grade, string permitno, string unit, string ratio, string price_rule, string retail_price, string trade_price, string max_price, string buying_price, string price_level, string high_level, string low_level, string state, string mid_unit)
        {
            StringBuilder strSql = new StringBuilder();
            int? id = Convert.ToInt32(code);
            int? Cat_id = Convert.ToInt32(cat_id);
            int? Factory = factory == "" ? 0 : Convert.ToInt32(factory);
            char Grade = Convert.ToChar(grade);
            int? Ratio = Convert.ToInt32(ratio);
            int? Price_rule = Convert.ToInt32(price_rule);
            double? Retail_price = Convert.ToDouble(retail_price);
            double? Trade_price = Convert.ToDouble(trade_price);
            double? Max_price = Convert.ToDouble(max_price);
            double? Buying_price = Convert.ToDouble(buying_price);
            char Price_level = Convert.ToChar(price_level);
            int? High_level = high_level == "" ? 0 : Convert.ToInt32(high_level);
            int? Low_level = low_level == "" ? 0 : Convert.ToInt32(low_level);
            char State = Convert.ToChar(state);
            int? Next_seq = id + 1;
            strSql.Append("UPDATE MMIS_MATERIAL SET ");
            strSql.Append("CAT_ID = :cat_id,");
            strSql.Append("NAME = :name,");
            strSql.Append("SPEC = :spec,");
            strSql.Append("FACTORY = :factory,");
            strSql.Append("GRADE = :grade,");
            strSql.Append("PERMITNO = :permitno,");
            strSql.Append("UNIT = :unit,");
            strSql.Append("RATIO = :ratio,");
            strSql.Append("PRICE_RULE = :price_rule,");
            strSql.Append("RETAIL_PRICE = :retail_price,");
            strSql.Append("TRADE_PRICE = :trade_price,");
            strSql.Append("MAX_PRICE = :max_price,");
            strSql.Append("BUYING_PRICE = :buying_price,");
            strSql.Append("PRICE_LEVEL = :price_level,");
            strSql.Append("HIGH_LEVEL = :high_level,");
            strSql.Append("LOW_LEVEL = :low_level,");
            strSql.Append("STATE = :state,");
            strSql.Append("MID_UNIT = :mid_unit");
            strSql.Append(" WHERE CODE = :code");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":cat_id", OleDbType.Numeric,5),                  
					new OleDbParameter(":name", OleDbType.VarChar,64),
                    new OleDbParameter(":spec", OleDbType.VarChar,32),
                    new OleDbParameter(":factory", OleDbType.Numeric,5),
					new OleDbParameter(":grade", OleDbType.Char,2),
					new OleDbParameter(":permitno", OleDbType.VarChar,32),
					new OleDbParameter(":unit", OleDbType.VarChar,16),
					new OleDbParameter(":ratio", OleDbType.Numeric,8),
					new OleDbParameter(":price_rule", OleDbType.Numeric,2),
					new OleDbParameter(":retail_price", OleDbType.Numeric,16),
                    new OleDbParameter(":trade_price", OleDbType.Numeric,16),
                    new OleDbParameter(":max_price", OleDbType.Numeric,16),
                    new OleDbParameter(":buying_price", OleDbType.Numeric,16),
                    new OleDbParameter(":price_level", OleDbType.Char,2),
                    new OleDbParameter(":high_level", OleDbType.Numeric,5),
                    new OleDbParameter(":low_level", OleDbType.Numeric,5),
                    new OleDbParameter(":state", OleDbType.Char,2),
                    new OleDbParameter(":mid_unit", OleDbType.VarChar,20),
                    new OleDbParameter(":code", OleDbType.VarChar,16)};
            parameters[0].Value = Cat_id;
            parameters[1].Value = name;
            parameters[2].Value = spec;
            parameters[3].Value = Factory;
            parameters[4].Value = Grade;
            parameters[5].Value = permitno;
            parameters[6].Value = unit;
            parameters[7].Value = Ratio;
            parameters[8].Value = Price_rule;
            parameters[9].Value = Retail_price;
            parameters[10].Value = Trade_price;
            parameters[11].Value = Max_price;
            parameters[12].Value = Buying_price;
            parameters[13].Value = Price_level;
            parameters[14].Value = High_level;
            parameters[15].Value = Low_level;
            parameters[16].Value = State;
            parameters[17].Value = mid_unit;
            parameters[18].Value = code;
            int rows = SqlDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool deleteMaterial(string code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM MMIS_MATERIAL");
            strSql.Append(" WHERE CODE = :code");
            OleDbParameter[] parameters = {
                    new OleDbParameter(":code", OleDbType.VarChar,16)};
            parameters[0].Value = code;
            int rows = SqlDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
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
