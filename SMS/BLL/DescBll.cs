using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SMS
{
    public class DescBll
    {
        public static DescBll instance = new DescBll();

        public void InsertNew(string _type, string _typesys, string _Content)
        {
            string sql = "";
            if (!IsDescExist(_type, _typesys))
            {
                sql = string.Format("insert into datadriven.Desc_DescHtml (dtype,typesysno,contents) values ('{0}','{1}','{2}')", _type, _typesys, _Content);
            }
            else
            {
                sql = string.Format("update datadriven.Desc_DescHtml set contents='{0}' where dtype='{1}' and typesysno='{2}'", _Content, _type, _typesys);
            }
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        public bool IsDescExist(string _type, string _typesys)
        {
            string sql = "select contents from datadriven.Desc_DescHtml where dtype='" + _type + "' and typesysno='" + _typesys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得html的描述
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_typesys"></param>
        /// <returns></returns>
        public string GetContent(string _type, string _typesys)
        {
            string html = "";
            string sql = "select contents from datadriven.Desc_DescHtml where dtype='" + _type + "' and typesysno='" + _typesys + "'";
            //string sql = "select contents from Desc_DescHtml where dtype='" + _type + "' and typesysno='" + _typesys + "'";

            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                html = dt.Rows[0][0].ToString();
            }
            return html;
        }
    }
}
