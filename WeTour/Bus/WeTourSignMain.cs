using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Member;

namespace WeTour
{
    public class WeTourSignMain
    {
        public static WeTourSignMain instance = new WeTourSignMain();

        public List<WeMatchModel> GetPersonalKnockOutMatch(string _ContentId, string _Round)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            WeTourContModel tcmodel = WeTourContentDll.instance.GetModelbyId(_ContentId);
            //查找淘汰赛阶段的比赛
            string sql = "select * from wtf_match where ContentID='" + _ContentId + "' and Round='" + _Round + "' order by matchorder";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    WeMatchModel model = new WeMatchModel();
                    model.SYS = dr["SYS"].ToString();
                    //
                    if (tcmodel.ContentType.IndexOf("双") > 0)
                    {
                        try
                        {
                            //Double Games
                            if (dr["player1"].ToString() != "")
                            {
                                if (dr["player1"].ToString().IndexOf(",") > 0)
                                {
                                    model.PLAYER1NAME = WeMemberDll.instance.GetModel(dr["player1"].ToString().Split(',')[0]).USERNAME + "/" + WeMemberDll.instance.GetModel(dr["player1"].ToString().Split(',')[1]).USERNAME;
                                }
                                else
                                {
                                    model.PLAYER1NAME = WeMemberDll.instance.GetModel(dr["player1"].ToString()).USERNAME;
                                }
                            }
                            else
                            {
                                model.PLAYER1NAME = "未知";
                            }
                            if (dr["player2"].ToString() != "")
                            {
                                if (dr["player2"].ToString().IndexOf(",") > 0)
                                {
                                    model.PLAYER2NAME = WeMemberDll.instance.GetModel(dr["player2"].ToString().Split(',')[0]).USERNAME + "/" + WeMemberDll.instance.GetModel(dr["player2"].ToString().Split(',')[1]).USERNAME;
                                }
                                else
                                {
                                    model.PLAYER2NAME = WeMemberDll.instance.GetModel(dr["player2"].ToString()).USERNAME;
                                }
                            }
                            else
                            {
                                model.PLAYER2NAME = "未知";
                            }
                            if (dr["winner"].ToString() != "")
                            {
                                model.WINNER = WeMemberDll.instance.GetModel(dr["winner"].ToString().Split(',')[0]).USERNAME + "/" + WeMemberDll.instance.GetModel(dr["winner"].ToString().Split(',')[1]).USERNAME;
                            }
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        //Sigle Games
                        if (dr["player1"].ToString() != "")
                        {
                            model.PLAYER1NAME = WeMemberDll.instance.GetModel(dr["player1"].ToString()).USERNAME;
                        }
                        else
                        {
                            model.PLAYER1NAME = "未知";
                        }

                        if (dr["player2"].ToString() != "")
                        {
                            model.PLAYER2NAME = WeMemberDll.instance.GetModel(dr["player2"].ToString()).USERNAME;
                        }
                        else
                        {
                            model.PLAYER2NAME = "未知";
                        }

                        if (dr["winner"].ToString() != "")
                        {
                            model.WINNER = WeMemberDll.instance.GetModel(dr["winner"].ToString()).USERNAME;
                        }
                    }

                    model.SCORE = dr["score"].ToString();

                    list.Add(model);
                }
            }
            return list;
        }
    }
}
