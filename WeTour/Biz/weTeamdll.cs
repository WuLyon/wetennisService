using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class weTeamdll
    {
        public static weTeamdll instance = new weTeamdll();

        #region CRUD
        public bool Insert(weTeamModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into wtf_team (tid,teamname,status,description,headmember) values (");
            sb.Append("'"+model.TID+"',");
            sb.Append("'" + model.TEAMNAME + "',");
            sb.Append("'" + model.STATUS + "',");
            sb.Append("'" + model.DESCRIPTION + "',");
            sb.Append("'" + model.HEADMEMBER + "')");
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

        public weTeamModel GetModel(string tid)
        {
            weTeamModel model=new weTeamModel();
            string sql = "select * from wtf_team where tid='"+tid+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<weTeamModel>(dt);
            }
            return model;
        }
        #endregion

        #region 添加团体
        public string AddGroups(Fe_Model_GroupApplicants model)
        {
            string groupSys = Guid.NewGuid().ToString("N");
            weTeamModel team = new weTeamModel();
            team.TID = groupSys;
            team.TEAMNAME = model.name;
            team.HEADMEMBER = model.coachName;
            team.DESCRIPTION = model.coachName;
            if (Insert(team))
            { 
                //添加成员
                foreach (Fe_Model_GroupApplicants_member member in model.members)
                {
                    weTeamMemberModel mem = new weTeamMemberModel();
                    mem.teamid = groupSys;
                    mem.gender = member.gender;
                    mem.name = member.name;
                    mem.passport = member.identity;
                    mem.identityCard = member.identityCard;
                    if (member.isBench)
                    {
                        mem.isBench = "1";
                    }
                    else
                    {
                        mem.isBench = "0";
                    }
                    weTeamMemberdll.instance.Insert(mem);
                }
            }
            return groupSys;
        }

       #endregion
    }
}
