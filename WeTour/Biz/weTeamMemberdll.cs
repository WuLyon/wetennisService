using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class weTeamMemberdll
    {
        public static weTeamMemberdll instance = new weTeamMemberdll();

        #region CRUD
        public bool Insert(weTeamMemberModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into wtf_TeamMember (teamid,membersys,status,name,gender,identityCard,passport,isbench,ext1,ext2,ext3,ext4) values (");
            sb.Append(" '"+model.teamid+"',");
            sb.Append(" '" + model.membersys + "',");
            sb.Append(" '" + model.status + "',");
            sb.Append(" '" + model.name + "',");
            sb.Append(" '" + model.gender + "',");
            sb.Append(" '" + model.identityCard + "',");
            sb.Append(" '" + model.passport + "',");
            sb.Append(" '" + model.isBench + "',");
            sb.Append(" '" + model.ext1 + "',");
            sb.Append(" '" + model.ext2 + "',");
            sb.Append(" '" + model.ext3 + "',");
            sb.Append(" '" + model.ext4 + "')");

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

        public List<weTeamMemberModel> GetList(string _TeamId)
        {
            List<weTeamMemberModel> list = new List<weTeamMemberModel>();
            string sql = "select * from wtf_TeamMember where teamid='"+_TeamId+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<weTeamMemberModel>>(dt);
            }
            return list;
        }

        public List<Fe_Model_GroupMember> GetMemberList(string _TeamId)
        {
            List<Fe_Model_GroupMember> Member_list = new List<Fe_Model_GroupMember>();
            List<weTeamMemberModel> list = weTeamMemberdll.instance.GetList(_TeamId);
            foreach (weTeamMemberModel member in list)
            {
                Fe_Model_GroupMember groupM = new Fe_Model_GroupMember();
                groupM.id = member.id;
                groupM.identityCard = member.identityCard;
                groupM.gender = member.gender;
                if (member.isBench == "0")
                {
                    groupM.isBench = false;
                }
                else
                {
                    groupM.isBench = true;
                }
                groupM.passport = member.passport;
                groupM.name = member.name;               

                Member_list.Add(groupM);
            }
            return Member_list;

        }
        #endregion
    }
}
