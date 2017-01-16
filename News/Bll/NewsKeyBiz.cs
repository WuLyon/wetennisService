using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace News
{
    public class NewsKeyBiz
    {
        public static NewsKeyBiz instance = new NewsKeyBiz();

        /// <summary>
        /// 插入新的关键字
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string InsertNew(NewsKeyModel model)
        {
            string sysno = Guid.NewGuid().ToString("N").ToUpper();
            string sql = string.Format("insert into wtf_NewsKeyW (sysno,newsid,keyword) values ('{0}','{1}','{2}')",model.SYSNO,model.NEWSID,model.KEYWORD);
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return sysno;
            }
            else
            {
                return "";
            }            
        }
    }
}
