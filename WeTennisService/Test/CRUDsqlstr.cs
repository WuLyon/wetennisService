using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace WeTennisService
{
    public class CRUDsqlstr
    {
        public static CRUDsqlstr instance = new CRUDsqlstr();

        /// <summary>
        /// 添加语句
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Fields"></param>
        /// <returns></returns>
        public string GetInsert(string TableName, string Fields)
        {
            string sql = "";
            string[] _Field = Fields.Split(',');
            sql += "string.Format(\"Insert into "+TableName;
            sql += " (" + Fields + ") " ;          

            sql += " values (";
            for (int j = 0; j < _Field.Length; j++)
            {
                sql += "'{"+j+"}',";
            }
            sql=sql.TrimEnd(',');
            sql += ")\",";
            for (int k = 0; k < _Field.Length; k++)
            {
                sql += "model." + _Field[k].ToLower() + ",";
            }
            sql = sql.TrimEnd(',');
            sql += ")";
            return sql;
        }

        public string GetEntity(string Fields)
        {
            StringBuilder sqlb = new StringBuilder(); 
            string sql = "";
            string[] _Field = Fields.Split(',');
            for (int j = 0; j < _Field.Length; j++)
            {
                sqlb.AppendLine("public string " + _Field[j].ToLower() + " {get;set;} ");
                //sql += "public string " + _Field[j] + " {get;set;}  /r/n ";
            }
            sql = sqlb.ToString();
            return sql;
        }
    }
}