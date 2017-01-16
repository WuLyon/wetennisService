using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gym;

namespace WeTour
{
    /// <summary>
    /// 用于标记赛事所绑定的场馆与场地
    /// </summary>
   public class Biz_TourGyms
    {
       public static Biz_TourGyms instance = new Biz_TourGyms();

        #region CRUD
       /// <summary>
       /// 为赛事添加新的场馆主键
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddNewGym(Model_TourGym model)
       {
           string sql = string.Format("insert into Tour_Gyms (TourSys,GymSys) values ('{0}','{1}')",model.TourSys,model.GymSys);
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
       /// 为赛事场馆，选择场地
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddNewGymCourt(Model_TourGymCourts model)
       {
           string sql = string.Format("insert into Tour_GymCourts (TourSys,GymSys,CourtId) values ('{0}','{1}','{2}')", model.TourSys, model.GymSys,model.CourtId);
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


       private bool DeleteGymCourts(string _Toursys, string _Gymsys)
       {
           string sql = "Delete Tour_GymCourts where TourSys='" + _Toursys + "' and GymSys='" + _Gymsys + "'";
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

       public bool AddNewGymCont(Model_GymContents model)
       {
           string sql = string.Format("Insert into Tour_GymContent (TourSys,GymSys,ContId) values ('{0}','{1}','{2}')",model.TourSys,model.GymSys,model.ContId);
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

       public bool DeleteGymCont(string _Toursys, string _Gymsys,string _ContId)
       {
           string sql = "Delete Tour_GymContent where TourSys='" + _Toursys + "' and GymSys='" + _Gymsys + "' and ContId='"+_ContId+"'";
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

        #region 获取赛事场馆数据

       /// <summary>
       /// 获取赛事的场馆信息
       /// </summary>
       /// <param name="_Toursys"></param>
       /// <returns></returns>
       public List<TourGymList> GetTourGyms(string _Toursys)
       {
           List<TourGymList> list = new List<TourGymList>();
           string sql = "select * from Tour_Gyms where TourSys='"+_Toursys+"'";
           DataTable dt = DbHelperSQL.Query(sql).Tables[0];
           for (int i = 0; i < dt.Rows.Count; i++)
           {
               TourGymList model = new TourGymList();
               GymModel gmo = GymDll.instanc.GetmodelbySys(dt.Rows[i]["GymSys"].ToString());
               model.gymImgUrl = "";//gym还未添加照片功能。
               model.gymName = gmo.GYMNAME;
               model.gymSys = gmo.SYS;

               //添加赛事场地分配情况
               int cqty = GetTourGymcourtQty(_Toursys, gmo.SYS);
               model.courtInfo = "已选择" + cqty.ToString() + "片场地";
               list.Add(model);
           }
           return list;
       }

       /// <summary>
       /// 获取场地数量
       /// </summary>
       /// <param name="_Toursys"></param>
       /// <param name="_Gymsys"></param>
       /// <returns></returns>
       private int GetTourGymcourtQty(string _Toursys, string _Gymsys)
       {
           string sql = "select * from Tour_GymCourts where TourSys='" + _Toursys + "' and GymSys='" + _Gymsys + "'";
           DataTable dt = DbHelperSQL.Query(sql).Tables[0];
           return dt.Rows.Count;
       }

       //删除gym
       public void DeleteTourGym(string _Toursys, string _Gymsys)
       {
           string sql = string.Format("Delete Tour_Gyms where TourSys='{0}' and GymSys='{1}' Delete Tour_GymCourts where TourSys='{0}' and GymSys='{1}'",_Toursys,_Gymsys);
           int a = DbHelperSQL.ExecuteSql(sql);
       }

       /// <summary>
       /// 获取赛事场馆的场地信息
       /// </summary>
       /// <param name="_TourSys"></param>
       /// <param name="_GymSys"></param>
       /// <returns></returns>
       public List<Model_Dist_GymCourts> GetTourGymCourts(string _TourSys, string _GymSys)
       { 
           List<Model_Dist_GymCourts> list=new List<Model_Dist_GymCourts>();
           List<CourtModel> Coutl = CourtDll.Get_Instance().GetModellistbyGymsys(_GymSys);
           foreach (CourtModel court in Coutl)
           {
               Model_Dist_GymCourts model = new Model_Dist_GymCourts();
               model.courtId = court.ID;
               model.courtName = court.COURTNAME;
               model.courtType = court.CTYPE;
               if (IsCourtSelected(_TourSys, _GymSys, court.ID))
               {
                   model.isCheck = "1";
               }
               else
               {
                   model.isCheck = "0";
               }
               list.Add(model);
           }
           return list;
       }

       /// <summary>
       /// 判断场地是否选中
       /// </summary>
       /// <param name="_TourSys"></param>
       /// <param name="_GymSys"></param>
       /// <param name="_CourtId"></param>
       /// <returns></returns>
       private bool IsCourtSelected(string _TourSys, string _GymSys, string _CourtId)
       {
           string sql = "select * from Tour_GymCourts where TourSys='" + _TourSys + "' and GymSys='" + _GymSys + "' and CourtId='" + _CourtId + "'";
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

       public void UpdateTourGymCourts(string _TourSys, string _GymSys, List<Model_Dist_GymCourts> list)
       {
           DeleteGymCourts(_TourSys, _GymSys);

           foreach (Model_Dist_GymCourts model in list)
           {
               if (model.isCheck == "1")
               {
                   Model_TourGymCourts cout = new Model_TourGymCourts();
                   cout.TourSys = _TourSys;
                   cout.GymSys = _GymSys;
                   cout.CourtId = model.courtId;
                   AddNewGymCourt(cout);
               }
           }
       }


        #endregion

        #region 获取赛事项目
       public List<Model_Dist_GymCont> GetGymContent(string _Toursys, string _Gymsys)
       {
           List<Model_Dist_GymCont> list = new List<Model_Dist_GymCont>();
           string sql = "select * from Tour_GymContent where TourSys='"+_Toursys+"' and GymSys='"+_Gymsys+"'";
           DataTable dt = DbHelperSQL.Query(sql).Tables[0];
           if (dt.Rows.Count > 0)
           {
               foreach (DataRow dr in dt.Rows)
               {
                   Model_Dist_GymCont model = new Model_Dist_GymCont();
                   model.contId = dr["ContId"].ToString();
                   WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(model.contId);
                   model.contName = cont.TourDate + cont.ContentName;
                   list.Add(model);
               }
           }
           return list;
       }
        #endregion
    }
}
