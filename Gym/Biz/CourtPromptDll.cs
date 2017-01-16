using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Gym
{
    public class CourtPromptDll
    {
        /// <summary>
        /// 
        /// </summary>
        public static CourtPromptDll instance = new CourtPromptDll();

        #region Basic Methods
        /// <summary>
        /// Get PromptMain Model
        /// </summary>
        /// <param name="_Id"></param>
        /// <returns></returns>
        public CourtPromptModel GetPromtModel(string _Id)
        {
            CourtPromptModel model = new CourtPromptModel();
            string sql = "select * from wtf_courtpromt where id='"+_Id+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<CourtPromptModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// get promotion detail model
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public CourtPromptDetailModel GetPromtDetailModel(string _id)
        {
            CourtPromptDetailModel model = new CourtPromptDetailModel();
            string sql = "select * from wtf_courtPromDetail where id='" + _id + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<CourtPromptDetailModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_promid"></param>
        /// <param name="_Courtid"></param>
        /// <returns></returns>
        public CourtPromptDetailModel GetPromtDetailbyPromCourt(string _promtid, string _Courtid)
        {
            CourtPromptDetailModel model = new CourtPromptDetailModel();
            string sql = "select * from wtf_courtPromDetail where promtid='" + _promtid + "' and courtid='" + _Courtid + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<CourtPromptDetailModel>(dt);
            }
            return model;
        }

        #endregion

        #region deal with court Promotion
        /// <summary>
        /// to check whether the court is in promotion
        /// </summary>
        /// <param name="_courtid"></param>
        /// <param name="_date"></param>
        /// <param name="_startHour"></param>
        /// <returns></returns>
        public bool IsCourtHasPromotion(string _Courtid, DateTime _Date, int _Start)
        {
            CourtModel model = CourtDll.Get_Instance().GetModelbyId(_Courtid);
            //whether gym has promotion during the time span
            if (IsGymHasPromotion(model.GYMID, _Date.ToString("yyyy-MM-dd"), _Start))
            {
                //to check whether court is in promotion 
                if (IsCourtInPromotion(model.GYMID, _Date.ToString("yyyy-MM-dd"), _Start, _Courtid))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Decide whether Gym has Promotion
        /// </summary>
        /// <param name="_Gymid"></param>
        /// <param name="_Date"></param>
        /// <param name="_Start"></param>
        /// <returns></returns>
        public bool IsGymHasPromotion(string _Gymid, string _Date, int _Start)
        {
            string sql = string.Format("select * from wtf_courtpromt where Gymid='{0}' and status=1 and Convert(datetime,startdate)<='{1}' and Convert(datetime,endDate)>='{1}' and Convert(int,StartHour)<='{2}' and Convert(int,Endhour)>'{2}'", _Gymid, _Date, _Start);
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
        /// to check if the court is in promotion 
        /// </summary>
        /// <param name="_Gymid"></param>
        /// <param name="_Date"></param>
        /// <param name="_Start"></param>
        /// <param name="_CourtId"></param>
        /// <returns></returns>
        public bool IsCourtInPromotion(string _Gymid, string _Date, int _Start, string _CourtId)
        {
            bool IsCourtIn = false;
            List<CourtPromptModel> list = GetGymPromotions(_Gymid, _Date, _Start);
            if (list.Count > 0)
            {
                foreach (CourtPromptModel model in list)
                {
                    List<CourtPromptDetailModel> dlist = GetPromtDetail(model.ID);
                    if (dlist.Count > 0)
                    {
                        foreach (CourtPromptDetailModel dmodel in dlist)
                        {
                            if (dmodel.COURTID == _CourtId)
                            {
                                IsCourtIn = true;
                                break;
                            }
                        }
                    }
                }
            }
            return IsCourtIn;
        }

        /// <summary>
        /// Get gym's Promotions
        /// </summary>
        /// <param name="_Gymid"></param>
        /// <param name="_Date"></param>
        /// <param name="_Start"></param>
        /// <returns></returns>
        public List<CourtPromptModel> GetGymPromotions(string _Gymid, string _Date, int _Start)
        {
            List<CourtPromptModel> list = new List<CourtPromptModel>();
            string sql = string.Format("select * from wtf_courtpromt where Gymid='{0}' and status=1 and Convert(datetime,startdate)<='{1}' and Convert(datetime,endDate)>='{1}' and Convert(int,StartHour)<='{2}' and Convert(int,Endhour)>'{2}'", _Gymid, _Date, _Start);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<CourtPromptModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Get Promotion details
        /// </summary>
        /// <param name="_Promtid"></param>
        /// <returns></returns>
        public List<CourtPromptDetailModel> GetPromtDetail(string _Promtid)
        {
            List<CourtPromptDetailModel> list = new List<CourtPromptDetailModel>();
            string sql = "select * from wtf_courtPromDetail where Promtid='"+_Promtid+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<CourtPromptDetailModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// get promotion main info by conditions
        /// </summary>
        /// <param name="_Date"></param>
        /// <param name="_Hour"></param>
        /// <param name="_Courtid"></param>
        /// <returns></returns>
        public CourtPromptModel GetPromtModelbyCondition(string _Date, string _Hour, string _Courtid)
        {
            CourtPromptModel model = new CourtPromptModel();
            string sql = string.Format("select b.* from wtf_courtPromDetail a left join wtf_courtpromt b on a.PromtId=b.id where a.courtid='{0}' and b.status=1 and Convert(datetime,b.startdate)<='{1}' and Convert(datetime,b.endDate)>='{1}' and Convert(int,b.StartHour)<='{2}' and Convert(int,b.Endhour)>'{2}'", _Courtid, _Date, _Hour);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<CourtPromptModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// If he or she already order the promotion
        /// </summary>
        /// <param name="_Promtid"></param>
        /// <param name="_Memsys"></param>
        /// <param name="_Date"></param>
        /// <returns></returns>
        public bool IfAlreadyOrdered(string _Promtid, string _Memsys, string _Date)
        {
            string sql = "select * from wtf_CourtOrder WHERE remark='COURTPROM' and ext2='" + _Promtid + "' and Membersys='" + _Memsys + "' and orderDate='"+_Date+"' and status in ('1','2')";
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
        /// 
        /// </summary>
        /// <param name="_Promtid"></param>
        /// <param name="_courtid"></param>
        /// <param name="_Date"></param>
        /// <returns></returns>
        public bool IsPrompCourtApplyFull(string _Promtid, string _courtid, string _Date)
        {
            CourtPromptDetailModel model = GetPromtDetailbyPromCourt(_Promtid, _courtid); 
            int MaxQty = Convert.ToInt32(model.MAXQTY);
            int ApplyQty = CountPrompCourtApplyQty(_Promtid, _courtid, _Date);
            if (ApplyQty == MaxQty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get Promotion Apply Qty by court
        /// </summary>
        /// <param name="_Promtid"></param>
        /// <param name="_courtid"></param>
        /// <param name="_Date"></param>
        /// <returns></returns>
        public int CountPrompCourtApplyQty(string _Promtid, string _courtid,string _Date)
        {
            string sql = "select * from wtf_CourtOrder WHERE remark='COURTPROM' and ext2='" + _Promtid + "' and CourtSys='" + _courtid + "' and orderDate='" + _Date + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

        #endregion
    }
}
