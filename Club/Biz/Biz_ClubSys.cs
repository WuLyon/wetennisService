using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gym;

namespace Club
{
    public class Biz_ClubSys
    {
        public static Biz_ClubSys instance = new Biz_ClubSys();

        #region CRUD
        /// <summary>
        /// 添加新的对应关系
        /// </summary>
        /// <param name="model"></param>
        /// Status：0表示正常，99表示删除
        /// <returns></returns>
        public bool AddNew(Model_ClubGym model)
        {
            string sql = string.Format("insert into Club_Gyms (ClubSys,GymSys,Status) values ('{0}','{1}','{2}')",model.ClubSys,model.GymSys,"0");
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
        /// 根据俱乐部主键获取所对应的信息
        /// </summary>
        /// <param name="_ClubSys"></param>
        /// <returns></returns>
        public List<Model_ClubGym> GetClubGymbyClubsys(string _ClubSys)
        {
            List<Model_ClubGym> list = new List<Model_ClubGym>();
            string sql = "select * from Club_Gyms where ClubSys='" + _ClubSys + "' and status='0'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<Model_ClubGym>>(dt);
            }
            return list;
        }
        #endregion

        #region 赛事后台管理
        /// <summary>
        /// 获取添加赛事页面的列表
        /// </summary>
        /// <param name="_ClubSys"></param>
        /// <returns></returns>
        public List<Model_Dist_Gyms> GetGymsByClub(string _ClubSys)
        {
            List<Model_Dist_Gyms> list = new List<Model_Dist_Gyms>();
            List<Model_ClubGym> gymlist = GetClubGymbyClubsys(_ClubSys);
            foreach (Model_ClubGym model in gymlist)
            {
                Model_Dist_Gyms mod = new Model_Dist_Gyms();
                mod.gymSys = model.GymSys;
                mod.gymName = GymDll.instanc.GetmodelbySys(mod.gymSys).GYMNAME;
                list.Add(mod);
            }
            return list;
        }



        #endregion

    }
}
