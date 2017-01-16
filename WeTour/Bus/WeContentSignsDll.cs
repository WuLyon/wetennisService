using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Member;

namespace WeTour
{
    public class WeContentSignsDll
    {
        public static WeContentSignsDll instance = new WeContentSignsDll();

        /// <summary>
        /// 根据赛事主键获得赛事签表
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<WeTourSignsModel> GetToursSign(string _Toursys)
        {
            List<WeTourSignsModel> list = new List<WeTourSignsModel>();
            List<WeTourContModel> contList = WeTourContentDll.instance.GetContentlist(_Toursys);
            if (contList.Count > 0)
            { 
                foreach(WeTourContModel contModel in contList)
                {
                    WeTourSignsModel model = new WeTourSignsModel();
                    model.ContentID = contModel.id;
                    model.ContentName = contModel.ContentName;
                    model.contSigns = GetContentSign(contModel.id);
                    list.Add(model);
                }
            }
            return list;
        }


        /// <summary>
        /// 根据项目编号获得子项的签表展示内容
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public List<ContSignModel> GetContentSign(string _ContentId)
        {
            List<ContSignModel> list = new List<ContSignModel>();
            List<WeMatchModel> listR = WeMatchDll.instance.GetContentRounds(_ContentId);
            WeTourContModel contModel=WeTourContentDll.instance.GetModelbyId(_ContentId);
            foreach (WeMatchModel rmodel in listR)
            {
                ContSignModel model = new ContSignModel();
                model.ContentID = _ContentId;
                model.ContentName = contModel.ContentName;
                model.Round = rmodel.ROUND.ToString();
                model.RoundName = rmodel.RoundName;

                //添加小组排名或淘汰赛比赛
                if (model.Round == "0")
                {
                    //添加小组赛
                    model.Groups = GetContentGroups(_ContentId);
                }
                else
                { 
                    //添加淘汰赛比赛
                    model.KnockOuts = WeMatchDll.instance.GetMatchlistbyContRound(_ContentId, model.Round);
                }
                list.Add(model);
            }
            return list;
        }

        public List<ContSignModel> GetSignKnockOut(string _ContentId)
        {
            List<ContSignModel> list = new List<ContSignModel>();
            List<WeMatchModel> listR = WeMatchDll.instance.GetContentRounds(_ContentId);
            WeTourContModel contModel = WeTourContentDll.instance.GetModelbyId(_ContentId);
            foreach (WeMatchModel rmodel in listR)
            {
                if (rmodel.ROUND != 0)
                {
                    ContSignModel model = new ContSignModel();
                    model.ContentID = _ContentId;
                    model.ContentName = contModel.ContentName;
                    model.Round = rmodel.ROUND.ToString();
                    model.RoundName = rmodel.RoundName;
                    model.KnockOuts = WeMatchDll.instance.GetMatchlistbyContRound(_ContentId, model.Round);
                    //List<WeMatchModel> mlist = WeMatchDll.instance.GetMatchlistbyContRound(_ContentId, model.Round);
                    //List<Fe_Model_eventDrawKO> getMatches = new List<Fe_Model_eventDrawKO>();
                    //foreach (WeMatchModel match in mlist)
                    //{
                    //    Fe_Model_eventDrawKO eventKO = new Fe_Model_eventDrawKO();
                        
                    //}
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 获得小组
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public List<ContGroupModel> GetContentGroups(string _ContentId)
        {
            List<ContGroupModel> list = new List<ContGroupModel>();
            string sql1 = "select distinct(GroupId) from wtf_TourSign where ContentId='" + _ContentId + "'";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    ContGroupModel model = new ContGroupModel();
                    model.GroupID = dt1.Rows[i][0].ToString();
                    model.GroupName = "第" + model.GroupID + "组";
                    model.GroupMembers = GetGroupMembers(_ContentId, model.GroupID);
                    list.Add(model);
                }
            }

            return list;
        }

        /// <summary>
        /// 获得小组成员
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <param name="_GroupId"></param>
        /// <returns></returns>
        public List<GroupMemberModel> GetGroupMembers(string _ContentId,string _GroupId)
        {
            List<GroupMemberModel> list = new List<GroupMemberModel>();
            DataTable dt = SortGroup(_ContentId, _GroupId);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                GroupMemberModel model = new GroupMemberModel();
                model.Order=(i+1).ToString();
                model.Name = dt.Rows[i]["MemName"].ToString();
                model.Memsys = dt.Rows[i]["membersys"].ToString();
                model.MatchQty = dt.Rows[i]["MatchQty"].ToString();
                model.WinLoseMatch = dt.Rows[i]["MatchWinLose"].ToString();
                model.WinLoseGame = dt.Rows[i]["GameWinLose"].ToString();
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 根据子项id和组id获得小组排名
        /// 暂未考虑净胜场和净胜局相同的情况
        /// </summary>
        /// <param name="ContentId"></param>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        public DataTable SortGroup(string ContentId, string GroupId)
        {
            string sql = "select * from wtf_TourSign where contentid='" + ContentId + "' and GroupId='" + GroupId + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            dt.Columns.Add("MemName");
            dt.Columns.Add("MatchQty");
            dt.Columns.Add("MatchWinLose");
            dt.Columns.Add("NetMatchWin", typeof(int));
            dt.Columns.Add("GameWinLose");
            dt.Columns.Add("NetGameWin", typeof(int));

            //补充小组成员信息
            WeTourContModel tcmodel = WeTourContentDll.instance.GetModelbyId(ContentId);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        //球员姓名
                        if (tcmodel.ContentType.IndexOf("双") > 0)
                        {
                            //双打
                            string[] Players = dr["membersys"].ToString().Split(',');
                            dr["MemName"] = WeMemberDll.instance.GetModel(Players[0]).USERNAME + "/" + WeMemberDll.instance.GetModel(Players[1]).USERNAME;
                        }
                        else
                        {
                            //单打
                            dr["MemName"] = WeMemberDll.instance.GetModel(dr["membersys"].ToString()).USERNAME;
                        }
                    }
                    catch
                    {
                    }

                    //计算胜负场次
                    int GroupMatchWin = GetGroupMatchWin(ContentId, GroupId, dr["membersys"].ToString());
                    int GroupMatchLose = GetGroupMatchloss(ContentId, GroupId, dr["membersys"].ToString());
                    dr["MatchWinLose"] = GroupMatchWin.ToString() + "-" + GroupMatchLose.ToString();

                    //计算净胜场次
                    dr["NetMatchWin"] = GroupMatchWin - GroupMatchLose;

                    //计算场次
                    dr["MatchQty"] = (GroupMatchWin + GroupMatchLose).ToString();

                    //计算胜负局数
                    int GroupGameWin = GetGroupGameWin(ContentId, GroupId, dr["membersys"].ToString());
                    int GroupGameLose = GetGroupGameLose(ContentId, GroupId, dr["membersys"].ToString());
                    dr["GameWinLose"] = GroupGameWin.ToString() + "-" + GroupGameLose.ToString();

                    //计算净胜局数
                    dr["NetGameWin"] = GroupGameWin - GroupGameLose;
                }
            }

            //根据净胜场次，净胜局数排序
            DataView dv = dt.DefaultView;
            dv.Sort = "NetMatchWin desc,NetGameWin desc";
            return dv.ToTable();
        }

        #region 小组赛RoundRobin
        /// <summary>
        /// 获得球员胜场
        /// </summary>
        /// <param name="ContentId"></param>
        /// <param name="GroupId"></param>
        /// <param name="Membersys"></param>
        /// <returns></returns>
        public int GetGroupMatchWin(string ContentId, string GroupId, string Membersys)
        {
            string sql = "select * from wtf_match where ContentId='" + ContentId + "' and Round=0 and winner='" + Membersys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获得球员负场
        /// </summary>
        /// <param name="ContentId"></param>
        /// <param name="GroupId"></param>
        /// <param name="Membersys"></param>
        /// <returns></returns>
        public int GetGroupMatchloss(string ContentId, string GroupId, string Membersys)
        {
            string sql = "select * from wtf_match where ContentId='" + ContentId + "' and Round=0 and loser='" + Membersys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获得胜局
        /// </summary>
        /// <param name="ContentId"></param>
        /// <param name="GroupId"></param>
        /// <param name="Membersys"></param>
        /// <returns></returns>
        public int GetGroupGameWin(string ContentId, string GroupId, string Membersys)
        {
            int GameWin = 0;
            string sql = "select * from wtf_match where ContentId='" + ContentId + "' and Round=0 and winner='" + Membersys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (Membersys == dr["player1"].ToString())
                    {
                        GameWin += Convert.ToInt32(dr["score"].ToString().Substring(0, 1));
                    }
                    else
                    {
                        GameWin += Convert.ToInt32(dr["score"].ToString().Substring(1, 1));
                    }
                }
            }

            string sql1 = "select * from wtf_match where ContentId='" + ContentId + "' and Round=0 and loser='" + Membersys + "'";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    if (Membersys == dr["player1"].ToString())
                    {
                        GameWin += Convert.ToInt32(dr["score"].ToString().Substring(0, 1));
                    }
                    else
                    {
                        GameWin += Convert.ToInt32(dr["score"].ToString().Substring(1, 1));
                    }
                }
            }
            return GameWin;
        }

        /// <summary>
        /// 获得负局
        /// </summary>
        /// <param name="ContentId"></param>
        /// <param name="GroupId"></param>
        /// <param name="Membersys"></param>
        /// <returns></returns>
        public int GetGroupGameLose(string ContentId, string GroupId, string Membersys)
        {
            int GameLose = 0;
            string sql = "select * from wtf_match where ContentId='" + ContentId + "' and Round=0 and winner='" + Membersys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (Membersys == dr["player1"].ToString())
                    {
                        GameLose += Convert.ToInt32(dr["score"].ToString().Substring(1, 1));
                    }
                    else
                    {
                        GameLose += Convert.ToInt32(dr["score"].ToString().Substring(0, 1));
                    }
                }
            }

            string sql1 = "select * from wtf_match where ContentId='" + ContentId + "' and Round=0 and loser='" + Membersys + "'";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    if (Membersys == dr["player1"].ToString())
                    {
                        GameLose += Convert.ToInt32(dr["score"].ToString().Substring(1, 1));
                    }
                    else
                    {
                        GameLose += Convert.ToInt32(dr["score"].ToString().Substring(0, 1));
                    }
                }
            }
            return GameLose;
        }
        #endregion
    }
}
