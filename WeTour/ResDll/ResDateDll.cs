using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class ResDateDll
    {
        public static ResDateDll instance = new ResDateDll();

        ///2016-10-15，修改赛事日期规则
        ///
        #region CRUD
        /// <summary>
        /// 添加赛事日期
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddTourDate(Model_TourDate model)
        {
            string sql = string.Format("Insert into Tour_Dates (TourSys,TourDate) values ('{0}','{1}')", model.TourSys, model.TourDate);
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
        /// 删除日期
        /// </summary>
        /// <param name="_Id"></param>
        /// <returns></returns>
        public bool DeleteTourDate(string _Id)
        {
            string sql = "delete Tour_Dates where id='" + _Id + "'";
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
        /// 添加赛事轮次信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddTourDateRound(Model_TourDateRound model)
        {
            string sql = string.Format("Insert into Tour_DateRounds (TourSys,TourDate,ContentId,Round) values ('{0}','{1}','{2}','{3}')", model.TourSys, model.TourDate, model.ContentId, model.Round);
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

        private bool DeleteTourDateRound(string _TourSys, string _TourDate)
        {
            string sql = "delete Tour_DateRounds where TourSys='" + _TourSys + "' and TourDate='" + _TourDate + "'";
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
        #endregion
        #region 获取赛事日期数据
        /// <summary>
        /// 根据赛事主键获得赛事日期清单
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<ResDate_TourDateModel> GetTourDatelist(string _Toursys)
        {
            List<ResDate_TourDateModel> list = new List<ResDate_TourDateModel>();
            string sql = "select * from ResDate_TourDate where toursys='" + _Toursys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<ResDate_TourDateModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 根据赛事日期id获得赛事日期所对应的项目及轮次信息
        /// </summary>
        /// <param name="_Tourdateid"></param>
        /// <returns></returns>
        public List<ResDate_ContentRoundModel> GetContentRoundlist(string _Tourdateid)
        {
            List<ResDate_ContentRoundModel> list = new List<ResDate_ContentRoundModel>();
            string sql = "select * from ResDate_ContentRound where tourdateid='" + _Tourdateid + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<ResDate_ContentRoundModel>>(dt);
            }
            return list;
        }
        #endregion
       
    }
}
