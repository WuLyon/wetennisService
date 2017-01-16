using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SMS
{
    public class PraiseBll
    {
        public static PraiseBll instance = new PraiseBll();

        public void InsertPraise(PraiseModel model)
        {
            string sql = string.Format("insert into datadriven.wtf_ComPrise (dtype,typesysno,IsGood,memsys,UpdateTime) values ('{0}','{1}','{2}','{3}','{4}')",model.DTYPE,model.TYPESYSNO,model.ISGOOD,model.MEMSYS,DateTime.Now.ToString());
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        

        /// <summary>
        /// 对于一条sys只能点赞一次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddPraise(PraiseModel model)
        {
            if (!IsMemPraise(model.DTYPE, model.TYPESYSNO, model.MEMSYS))
            {
                InsertPraise(model);
                return true;
            }
            else
            {
                DeletePraise(model);
                return true;
            }
        }

        /// <summary>
        /// 取消点赞
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeletePraise(PraiseModel model)
        {
            string sql = string.Format("delete datadriven.wtf_ComPrise where dtype='" + model.DTYPE + "' and typesysno='" + model.TYPESYSNO + "' and memsys='" + model.MEMSYS + "'");
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
        /// 对于一条sys一天只能点赞一次
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddPicPraiseInday(PraiseModel model)
        {
            if (!IsMemPraiseInOneDay(model.DTYPE, model.TYPESYSNO, model.MEMSYS))
            {
                InsertPraise(model);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// 获取点赞数量，1：表示点赞，0：表示漏油
        /// </summary>
        /// <param name="_Type"></param>
        /// <param name="_Typesysno"></param>
        /// <param name="_IsGood"></param>
        /// <returns></returns>
        public int CountPraiseQty(string _Type, string _Typesysno, string _IsGood)
        {
            string sql = "select * from datadriven.wtf_ComPrise where dtype='" + _Type + "' and typesysno='" + _Typesysno + "' and Isgood='" + _IsGood + "'";
            return DbHelperSQL.Query(sql).Tables[0].Rows.Count;
        }

        /// <summary>
        /// 判断用户是否已点赞
        /// </summary>
        /// <param name="_Type"></param>
        /// <param name="_Typesysno"></param>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public bool IsMemPraise(string _Type, string _Typesysno, string _Memsys)
        {
            string sql = "select * from datadriven.wtf_ComPrise where dtype='" + _Type + "' and typesysno='" + _Typesysno + "' and memsys='" + _Memsys + "'";
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
        /// 在同一天内，是否点赞了多次
        /// </summary>
        /// <param name="_Type"></param>
        /// <param name="_Typesysno"></param>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        private bool IsMemPraiseInOneDay(string _Type, string _Typesysno, string _Memsys)
        {
            string startDay = DateTime.Now.ToString("yyyy-MM-dd");
            string endDay = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string sql = "select * from datadriven.wtf_ComPrise where dtype='" + _Type + "' and typesysno='" + _Typesysno + "' and memsys='" + _Memsys + "' and Convert(datetime,UpdateTime,120) between Convert(datetime,'" + startDay + "',120) and Convert(datetime,'" + endDay + "',120)";
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
