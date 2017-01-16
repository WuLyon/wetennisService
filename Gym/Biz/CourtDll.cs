using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Gym
{
    public class CourtDll
    {
        public CourtDll() { }
        private static CourtDll _Instance;
        public static CourtDll Get_Instance()
        {
            if (_Instance == null)
            {
                _Instance = new CourtDll();
            }
            return _Instance;
        }

        /// <summary>
        /// 根据id获得实体
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public CourtModel GetModelbyId(string _id)
        {
            CourtModel model = new CourtModel();
            string sql = "select * from wtf_court where id='" + _id + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<CourtModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 获得场地的全称，包含场馆名
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public string GetCourtFullName(string _id)
        {
            string _courtFull = "";
            CourtModel court = GetModelbyId(_id);
            GymModel gym = GymDll.instanc.GetmodelbySys(court.GYMID);
            _courtFull = gym.GYMNAME + court.COURTNAME;
            return _courtFull;
        }

        public List<CourtModel> GetModellistbyGymsys(string _GymSys)
        {
            List<CourtModel> list = new List<CourtModel>();
            GymModel gmod = GymDll.instanc.GetmodelbySys(_GymSys);
            string sql = "select * from wtf_court where Gymid='" + gmod.ID + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<CourtModel>>(dt);
            }
            return list;
        }

        public string GetCourtNoByID(string _id)
        {
            string CName = "";
            string sql = "select * from wtf_court where id='"+_id+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                CName = dt.Rows[0]["courtname"].ToString();
            }
            return CName;
        }

        /// <summary>
        /// 根据球场数量为场馆添加比赛场地
        /// </summary>
        /// <param name="_Gymsys"></param>
        /// <param name="_CourtQty"></param>
        public void AddGymCourts(string _Gymsys, int _CourtQty)
        {
            for (int i = 0; i < _CourtQty; i++)
            {
                AddCourt(_Gymsys,(i + 1).ToString() + "号场");
            }
        }

        /// <summary>
        /// 添加场地
        /// </summary>
        /// <param name="_Gymsys"></param>
        /// <param name="_CourtQty"></param>
        private void AddCourt(string _Gymsys, string _CourtQty)
        {
            string sql = "insert into wtf_court (courtNo,Gymsys) values ('" + _CourtQty + "','" + _Gymsys + "')";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        public List<CourtModel> getCourtsbyGymsys(string _Gymsys)
        {
            List<CourtModel> list = new List<CourtModel>();
            string sql = "select a.* from wtf_court a left join wtf_gym b on a.gymid=b.id where b.sys='" + _Gymsys + "' and a.price='1' ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<CourtModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Get Court price
        /// </summary>
        /// <param name="_CourtId"></param>
        /// <param name="_Date"></param>
        /// <param name="_Start"></param>
        /// <returns></returns>
        public string CourtPrice(string _CourtId, DateTime _Date, int _Start)
        {
            string _CourtPrice = "活动";
            try
            {
                CourtModel model = GetModelbyId(_CourtId);
                string IsweekDay = IsWeekDay(_Date);
                string sql = "select * from Wtf_GymPrice where Gymid='" + model.GYMID + "' and CourtType='" + model.CTYPE + "' and Time='" + _Date.DayOfWeek + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int startH = Convert.ToInt32(dr["StartHour"].ToString());
                        int EndH = Convert.ToInt32(dr["EndHour"].ToString());
                        if (_Start >= startH && _Start < EndH)
                        {
                            _CourtPrice = dr["Price"].ToString();
                        }
                    }
                }
            }
            catch
            { 
            
            }
            return _CourtPrice;
        }

        public string  IsWeekDay(DateTime _Date)
        {
            if (_Date.DayOfWeek.ToString() == "saturday" || _Date.DayOfWeek.ToString() == "sunday")
            {
                return "2";
            }
            else
            {
                return "1";
            }
        }
    }
}