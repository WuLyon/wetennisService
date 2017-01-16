using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Bet
{
    public class DailySignBiz
    {
        public static DailySignBiz instance = new DailySignBiz();

        /// <summary>
        /// 添加每日签到信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNewSign(DailySignModel model)
        {
            string sql = string.Format("insert into wtf_dailySign (Memsys,SignDate,UpdateTime) values ('{0}','{1}','{2}')",model.MEMSYS,DateTime.Now.ToShortDateString(),DateTime.Now.ToString());
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否已签到
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public bool IsTodaySigned(string _Memsys)
        {
            string sql = "select * from wtf_dailySign where Memsys='"+_Memsys+"' and Signdate='"+DateTime.Now.ToShortDateString()+"'";
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
    }
}
