using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Gym
{
    public class CourtOrderBll
    {
        public static CourtOrderBll instance = new CourtOrderBll();

        /// <summary>
        /// Add a new court order
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Insert(CourtOrderModel model)
        {
            string sql = string.Format("insert into wtf_CourtOrder (CourtSys,Membersys,OrderNo,OrderDate,StartHour,EndHour,TotalHour,UnitPrice,TotalMoney,Status,UpdateDate,Remark,ext1,ext2) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')", model.COURTSYS, model.MEMBERSYS, model.ORDERNO, model.ORDERDATE, model.STARTHOUR, model.ENDHOUR, model.TOTALHOUR, model.UNITPRICE, model.TOTALMONEY, model.STATUS, DateTime.Now.ToString(),model.REMARK,model.EXT1,model.EXT2);
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
        /// 
        /// </summary>
        /// <param name="_OrderNo"></param>
        /// <returns></returns>
        public CourtOrderModel GetModelbyOrderNo(string _OrderNo)
        {
            CourtOrderModel model = new CourtOrderModel();
            string sql = "select * from wtf_CourtOrder where orderNo='" + _OrderNo + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<CourtOrderModel>(dt);
            }
            return model;
        }



        /// <summary>
        /// Update Court Order Status
        /// </summary>
        /// <param name="_OrderNo"></param>
        /// <param name="_Status"></param>
        /// <returns></returns>
        public bool UpdateOrderStatus(string _OrderNo, string _Status)
        {
            string sql = "update wtf_CourtOrder set status='" + _Status + "' where OrderNo='" + _OrderNo + "'";
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
        /// 
        /// </summary>
        /// <param name="_CourtSys"></param>
        /// <param name="_Date"></param>
        /// <returns></returns>
        public List<CourtOrderModel> getCourtOrderBydate(string _CourtSys,DateTime _Date)
        {
            List<CourtOrderModel> list = new List<CourtOrderModel>();
            string sql = "select * from wtf_CourtOrder where status in (1,2) and  CourtSys='" + _CourtSys + "' and OrderDate='"+_Date.ToString("yyyy-MM-dd")+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<CourtOrderModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Initiate Court Resources
        /// </summary>
        /// <param name="_GymSys"></param>
        /// <param name="_Date"></param>
        /// <returns></returns>
        public string  LoadCourtRent(string _GymSys, string _Date)
        {
            string html = "";
            List<CourtOrderModel> list = new List<CourtOrderModel>();
            DateTime theDate = DateTime.Now;
            switch (_Date)
            { 
                case "d1":
                    theDate = DateTime.Now;
                    break;
                case "d2":
                    theDate = DateTime.Now.AddDays(1);
                    break;
                case "d3":
                    theDate = DateTime.Now.AddDays(2);
                    break;
            }

            List<CourtModel> Courtlist = CourtDll.Get_Instance().getCourtsbyGymsys(_GymSys);
            if (Courtlist.Count > 0)
            {
                foreach (CourtModel model in Courtlist) {
                    string _CourtType = (model.CTYPE == "indoor") ? "室内" : "室外";
                    html += "<li><ul class=\"rentc\"><li id=\"" + model.ID + "\" style=\"background-color:#fff\">" + model.COURTNAME + "<br/><span style=\"font-size:10px;\">" + _CourtType + "</span></li>";
                    //遍历资源，查看资源使用情况
                    html += GetCourtStatus(model.ID, theDate);
                    html += "</ul></li>";
                }
            }
            return html;
        }

        /// <summary>
        /// Get Court status
        /// </summary>
        /// <param name="_Courtid"></param>
        /// <param name="_Date"></param>
        /// <returns></returns>
        public string GetCourtStatus(string _Courtid, DateTime _Date)
        {
            string html = "";
            int _starth = 9;
            for (int i = 0; i < 14; i++)
            {
                int _Start = _starth + i;
                string id = _Courtid + "-" + _Start.ToString();
                string _Cstatus=GetCourtHourState(_Courtid,_Date,_Start);
                string _color = "";
                if (_Cstatus == "已过期")
                {
                    _color = "#ccc";
                }
                else if (_Cstatus == "预定")
                {
                    _color = "#FF4500";
                }
                else if (_Cstatus == "网预定")
                {
                    _color = "#00FFFF";
                }
                else if (_Cstatus == "活动")
                {
                    _color = "#FFF8DC";
                }
                else if (_Cstatus == "促销")
                {
                    _color = "#FFFF00";
                }
                else
                {
                    _color = "#fff";
                }

                html += "<li id=\"" + id + "\" style=\"background-color:" + _color + "\" onclick=\"selectThis('" + id + "')\">" + _Cstatus + "</li>";
            }
            return html;
        }

        public string GetCourtHourState(string _Courtid, DateTime _Date, int _Start)
        {
            string _CourtSatatus = "";
            //is Time OverDue
            if (IsTimeOverDue(_Date, _Start))
            {
                _CourtSatatus = "已过期";
            }
            else if (IsCourtOrdered(_Courtid, _Date, _Start) != "未预定")
            {
                _CourtSatatus = IsCourtOrdered(_Courtid, _Date, _Start);
            }
            else if (CourtPromptDll.instance.IsCourtHasPromotion(_Courtid, _Date, _Start))
            {
                //to check whether it is during promotion time
                _CourtSatatus = "促销";
            }
            else
            {
                //the court is available, return the price
                _CourtSatatus = CourtDll.Get_Instance().CourtPrice(_Courtid, _Date, _Start);
            }
            
            return _CourtSatatus;            
        }

        

        public bool IsTimeOverDue(DateTime _Date, int _Start)
        {
            if (_Date.DayOfYear == DateTime.Now.DayOfYear && _Start <= DateTime.Now.Hour)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check Court
        /// </summary>
        /// <param name="_CourtId"></param>
        /// <param name="_Date"></param>
        /// <param name="_Start"></param>
        /// <returns></returns>
        public string IsCourtOrdered(string _CourtId, DateTime _Date, int _Start)
        {
            List<CourtOrderModel> list = getCourtOrderBydate(_CourtId, _Date);
           
            string  IsCourtORder = "";
            try
            {
                if (list.Count > 0)
                {
                    foreach (CourtOrderModel model in list)
                    {
                        int StartH = Convert.ToInt32(model.STARTHOUR);
                        int EndH = Convert.ToInt32(model.ENDHOUR);
                        if (_Start >= StartH && _Start < EndH)
                        {
                            if (model.REMARK == "WEBORDER")
                            {
                                IsCourtORder = "网预定";
                                break;
                            }
                            else
                            {
                                IsCourtORder = "预定";
                                break;
                            }
                        }
                        else
                        {
                            IsCourtORder = "未预定";
                        }
                    }
                }
                else
                {
                    IsCourtORder = "未预定";
                }
            }
            catch {
                IsCourtORder = "未预定";
            }
            return IsCourtORder;
        }

        /// <summary>
        /// SubmitOrder
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SubmitOrder(CourtOrderModel model)
        {
            switch (model.ORDERDATE)
            { 
                case "d1":
                    model.ORDERDATE = DateTime.Now.ToString("yyyy-MM-dd");
                    break;
                case "d2":
                    model.ORDERDATE = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    break;
                case "d3":
                    model.ORDERDATE = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");
                    break;
            }

            if (Insert(model))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        /// <summary>
        /// get court order list by gym sysno
        /// </summary>
        /// <param name="_GymSys"></param>
        /// <returns></returns>
        public List<CourtOrderModel> GetCourtOrderList(string _GymSys,string _OrderDate,string _Type)
        {
            List<CourtOrderModel> list = new List<CourtOrderModel>();
            if (_OrderDate == "")
            {
                _OrderDate = DateTime.Now.ToString("yyyy-MM-dd");
            }

            string sql = "select a.*,d.UserName,d.Telephone,b.CourtName from wtf_CourtOrder a left join wtf_court b on a.CourtSys=b.id left join wtf_gym c on b.Gymid=c.id left join wtf_Member d on a.Membersys=d.sysno where c.sys='" + _GymSys + "' and OrderDate='" + _OrderDate + "' and a.status in (1,2)";
            if (_Type != "")
            {
                sql += " and Remark='" + _Type + "'";
            }
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<CourtOrderModel>>(dt);
            }
            return list;
        }
    }
}
