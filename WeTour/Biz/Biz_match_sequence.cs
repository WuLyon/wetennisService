using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class Biz_match_sequence
    {
        public static Biz_match_sequence instance = new Biz_match_sequence();

        #region CRUD
        public bool Insert(model_match_sequence model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Insert into wtf_match_teamSequence values (");
            sb.Append(" '" + Guid.NewGuid().ToString("N") + "',");
            sb.Append("'" + model.toursys + "',");
            sb.Append("'" + model.contentId + "',");
            sb.Append("'" + model.matchsys + "',");
            sb.Append("'" + model.teamsys + "',");
            sb.Append("" + model.sequence + ",");
            sb.Append("'" + model.sequ_name + "',");
            sb.Append("'" + model.member1_sys + "',");
            sb.Append("'" + model.member1_name + "',");
            sb.Append("'" + model.member2_sys + "',");
            sb.Append("'" + model.member2_name + "')");
            string sql = sb.ToString();
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

        public List<model_match_sequence> getList(string cond)
        {
            List<model_match_sequence> seq_list = new List<model_match_sequence>();
            string sql = "select * from wtf_match_teamSequence where 1=1 " + cond;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                
                seq_list = JsonHelper.ParseDtModelList<List<model_match_sequence>>(dt);
            }
            return seq_list;
        }

        public bool Is_match_seq_exist(string _matchId, string _TeamId)
        {
            string sql = "select * from wtf_match_teamSequence where matchsys='"+_matchId+"' and teamsys='"+_TeamId+"'";
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

        public List<model_match_sequence> Init_MatchTeamSeq(string _matchId, string _teamId)
        {
            List<model_match_sequence> team_seq = new List<model_match_sequence>();

            WeMatchModel match = WeMatchDll.instance.GetModel(_matchId);
            WeTourModel tour = WeTourDll.instance.GetModelbySys(match.TOURSYS);
            string match_contents = tour.EXT5;
            string[] contents = match_contents.Split(',');
            for (int i = 0; i < contents.Length; i++)
            {
                model_match_sequence seq = new model_match_sequence();
                seq.contentId = match.ContentID;
                seq.matchsys = _matchId;
                seq.sequ_name = contents[i];
                seq.sequence = i + 1;
                seq.teamsys = _teamId;
                seq.toursys = match.TOURSYS;
                Insert(seq);
            }

            team_seq = getList(" and matchsys='" + _matchId + "' and teamsys='" + _teamId + "'");

            return team_seq;
        }
        
    #endregion

    #region Business
        public string GetTeamSys(string _Toursys, string _UserId)
        {
            string _TeamId = "";
            List<WeTourApplyModel> app_list = WeTourApplyDll.instance.GetApplyListbyCond(" and toursys='" + _Toursys + "' and paterner='" + _UserId + "'");
            if (app_list.Count > 0)
            {
                _TeamId = app_list[0].TOURSYS;
            }
            return _TeamId;
        }

        public List<Fe_Model_MatchTeamSequ> fetchMatchTeamSeq(string _matchId, string _TeamId)
        {
            List<Fe_Model_MatchTeamSequ> seq_list = new List<Fe_Model_MatchTeamSequ>();
            List<model_match_sequence> team_seq = new List<model_match_sequence>();
            //判断是否已添加顺序
            if (Is_match_seq_exist(_matchId, _TeamId))
            {
                //已存在
                team_seq = getList(" and matchsys='" + _matchId + "' and teamsys='" + _TeamId + "'"); 
            }
            else
            {
                team_seq = Init_MatchTeamSeq(_matchId, _TeamId);
            }

            if (team_seq.Count > 0)
            {
                foreach (model_match_sequence seq in team_seq)
                {
                    Fe_Model_MatchTeamSequ sequ = new Fe_Model_MatchTeamSequ();
                    sequ.id = seq.sysno;
                    sequ.name = seq.sequ_name;
                    List<string> team_member = new List<string>();
                    if (!string.IsNullOrEmpty(seq.member1_sys))
                    {
                        team_member.Add(seq.member1_sys);
                    }

                    if (!string.IsNullOrEmpty(seq.member2_sys))
                    {
                        team_member.Add(seq.member2_sys);
                    }

                    sequ.teamMembers = team_member;

                    seq_list.Add(sequ);
                }
            }
            return seq_list;

        }

        public void UpdateTeamSchedule(Fe_Model_MatchTeamSchedule teamSch)
        {
            List<string> sql_list = new List<string>();
            List<Fe_Model_MatchTeamSequ> sequ_list = teamSch.sechedule;
            foreach (Fe_Model_MatchTeamSequ sequ in sequ_list)
            {
                string sql = "update wtf_match_teamSequence set ";
                List<string> mems = sequ.teamMembers;
                for (int i = 0; i < mems.Count; i++)
                {
                    if (mems[i] != "")
                    {
                        if (i == 0)
                        {
                            sql += " member1_sys='" + mems[i] + "'";
                        }
                        else
                        {
                            sql += " ,member2_sys='" + mems[i] + "'";
                        }                        
                    }
                }
                sql += " where sysno='"+sequ.id+"'";
                sql_list.Add(sql);                
            }
            int a = DbHelperSQL.ExecuteSqlTran(sql_list);
        }
	#endregion
    }
}
