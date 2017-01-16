using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Club;
using Member;

namespace Union
{
    public class Biz_Member
    {
        public static Biz_Member instance = new Biz_Member();

        #region CRUD
        /// <summary>
        /// Add a new Union Member
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Insert(Model_Member model)
        {
            string sql = string.Format("insert into Union_Member (UnionSys,ClubSys,JoinDate,Status,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",model.UnionSys,model.ClubSys,DateTime.Now.ToString(),"0",model.ext1,model.ext2,model.ext3,model.ext4,model.ext5);
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return true;
            }
            else
            {
                return false; ;
            }
        }

        public bool UpdateStatus(string _Id, string _Status)
        {
            string sql = string.Format("update Union_Member set status='{0}' where id='{1}'",_Status,_Id);
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

        #region Business
        /// <summary>
        /// 根据俱乐部主键获取联盟清单
        /// </summary>
        /// <param name="_Clubsys"></param>
        /// <returns></returns>
        public List<Model_Basic> GetUnionListbyClubsys(string _Clubsys)
        {
            List<Model_Basic> list = new List<Model_Basic>();
            string sql = string.Format("select b.* from Union_Member a left join Union_Basic b on a.UnionSys=b.UnionSys where a.ClubSys='" + _Clubsys + "'");
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<Model_Basic>>(dt);
            }
            return list;
        }

        public List<Model_Union_ClubMember> GetUnionMemberbySys(string _UnionSys)
        {
            List<Model_Union_ClubMember> list = new List<Model_Union_ClubMember>();
            string sql = string.Format("select * from Union_Member where UnionSys='"+_UnionSys+"' and status='1'");
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Model_Union_ClubMember model = new Model_Union_ClubMember();
                    ClubModel cmodel = ClubBiz.Get_Instance().GetModel(dr["ClubSys"].ToString());
                    model.ClubName = cmodel.CLUBNAME;
                    model.ClubDesc = cmodel.DESCRIPTION;
                    model.ClubThumbImage = cmodel.EXT2;

                    string _Mgrsys = ClubMemBiz.instance.GetClubManagerSys(dr["ClubSys"].ToString());
                    WeMemberModel mmodel = WeMemberDll.instance.GetModel(_Mgrsys);
                    model.ContactPerson = mmodel.USERNAME;
                    model.ContacTel = mmodel.TELEPHONE;
                    model.JoinDate = dr["JoinDate"].ToString();
                    list.Add(model);
                }
            }
            return list;
        }
        #endregion 
    }
}
