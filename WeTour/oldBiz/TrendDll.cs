using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WeTour
{
    public class TrendDll
    {
        public TrendDll() { }
        private static TrendDll _Instance;
        public static TrendDll Get_Instance()
        {
            if (_Instance == null)
            {
                _Instance = new TrendDll();
            }
            return _Instance;
        }

        /// <summary>
        /// 添加新的动态
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <param name="_iContent"></param>
        /// <returns></returns>
        public bool InsertTrend(string _Memsys, string _iContent)
        {
            string sql = string.Format("insert into wtf_trend (CreateMemSys,CreateDate,iContent,status) values ('{0}','{1}','{2}','1')", _Memsys, DateTime.Now.ToString(), _iContent);
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
        /// 删除动态
        /// </summary>
        /// <param name="_Id"></param>
        /// <returns></returns>
        public bool DeleteTrendbyId(string _Id)
        {
            string sql = string.Format("Delete wtf_trend where id='" + _Id + "'");
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
        /// 添加阅读动态信息
        /// </summary>
        /// <param name="_TrendId"></param>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public bool InsertTendRead(string _TrendId, string _Memsys)
        {
            string sqld = "select * from wtf_trendread where TrendId='" + _TrendId + "' and ReadMemsys='" + _Memsys + "'";
            DataTable dt = DbHelperSQL.Query(sqld).Tables[0];
            if (dt.Rows.Count == 0)
            {
                string sql = string.Format("insert into wtf_trendread (TrendId,ReadMemsys,ReadDate) values ('{0}','{1}','{2}')", _TrendId, _Memsys, DateTime.Now.ToString());
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
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 添加动态评论
        /// </summary>
        /// <param name="_TrendId"></param>
        /// <param name="_Memsys"></param>
        /// <param name="_Comment"></param>
        /// <returns></returns>
        public bool InsertTrendComment(string _TrendId, string _Memsys, string _Comment)
        {
            string sql = string.Format("insert into wtf_TrendComments (Trendid,ComMemsys,Comments,ComDate) values ('{0}','{1}','{2}','{3}')", _TrendId, _Memsys, _Comment, DateTime.Now.ToString());
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
        /// 判定是否已经阅读
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public bool IsTrendReaded(string _id, string _Memsys)
        {
            string sql = string.Format("select * from wtf_trendread where trendid='{0}' and Readmemsys='{1}'", _id, _Memsys);
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
        /// 根据动态/赛事/比赛编号，获得评论数量
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public int GetTrendComQty(string _id)
        {
            string sql = "select * from wtf_trendcomments where trendid='" + _id + "'";
            return DbHelperSQL.Query(sql).Tables[0].Rows.Count;
        }

        /// <summary>
        /// 获取联系人动态
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public List<TrendModel> GetTend(string _Memsys)
        {
            List<TrendModel> list = new List<TrendModel>();
            //根据会员SYSNO获得所有关注的人SYSNO
            string AvoidCond = "'" + _Memsys + "'";//动态应该包含查看自己的动态
            List<MemberModel> FollowList = MemberDll.Get_Instance().GetFollowerModellist(_Memsys);
            if (FollowList.Count > 0)
            {
                for (int i = 0; i < FollowList.Count; i++)
                {
                    AvoidCond += ",'" + FollowList[i].SYSNO + "'";
                }
                AvoidCond = AvoidCond.TrimStart(',');
            }

            //查询所关注的人的动态，按时间先后顺序排列
            string sql = "";
            if (!string.IsNullOrEmpty(AvoidCond))
            {
                sql = " select * from wtf_Trend where CreateMemSys in (" + AvoidCond + ") order by id desc";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        TrendModel model = new TrendModel();
                        MemberModel meminfo = MemberDll.Get_Instance().GetModel(dr["CreateMemSys"].ToString());
                        if (meminfo.EXT1 != "")
                        {
                            model.WTRIMG = meminfo.EXT1;//会员头像
                        }
                        else
                        {
                            model.WTRIMG = "/images/touxiang.jpg";//默认头像
                        }
                        model.WTRNAME = meminfo.NAME;
                        model.WTRDATE = dr["CreateDate"].ToString();
                        model.WTRSYSNO = meminfo.SYSNO;
                        model.CONTENT = dr["iContent"].ToString();
                        model.APPLOUSE = "0";//点赞数量
                        model.TRID = dr["id"].ToString();//动态id
                        //评论内容
                        string Comments = "";
                        List<TrendModel> comlist = GetTendComments(dr["id"].ToString());
                        if (comlist.Count > 0)
                        {
                            //有评论
                            Comments = "<ul>";
                            for (int i = 0; i < comlist.Count; i++)
                            {
                                Comments += "<li style=\"padding-bottom:5px\"><span class=\"trendReplayName\" onclick=\"location.href='/Login/PlayerMain.htm?memsys=" + comlist[i].WTRSYSNO + "'\">" + comlist[i].WTRNAME + "</span>：" + comlist[i].CONTENT + "</li>";
                            }
                            Comments += "</ul>";
                        }
                        model.COMMENTS = Comments;
                        list.Add(model);
                    }
                }
            }

            return list;
        }


        /// <summary>
        /// 获得动态评论
        /// </summary>
        /// <param name="_TrendSys"></param>
        /// <returns></returns>
        public List<TrendModel> GetTendComments(string _Trendid)
        {
            List<TrendModel> list = new List<TrendModel>();
            string sql = "select * from wtf_TrendComments where Trendid='" + _Trendid + "' order by Comdate";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TrendModel model = new TrendModel();
                    MemberModel meminfo = MemberDll.Get_Instance().GetModel(dr["ComMemsys"].ToString());
                    model.WTRNAME = meminfo.NAME;
                    model.WTRSYSNO = meminfo.SYSNO;
                    model.CONTENT = dr["Comments"].ToString();
                    model.WTRDATE = dr["Comdate"].ToString();

                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Trendid"></param>
        /// <returns></returns>
        public TrendModel getLatestTrendCom(string _Trendid)
        {
            TrendModel model = new TrendModel();
            string sql = "select top 1 * from wtf_TrendComments where Trendid='" + _Trendid + "' order by id desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<TrendModel>(dt);
            }
            return model;
        }

    }
}