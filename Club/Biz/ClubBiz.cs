using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Club
{
    public class ClubBiz
    {
        private ClubBiz() { }
        private static ClubBiz _Instance;
        public static ClubBiz Get_Instance()
        {
            if (_Instance == null)
            {
                _Instance = new ClubBiz();
            }
            return _Instance;
        }

        /// <summary>
        /// 根据主键获得实体
        /// </summary>
        /// <returns></returns>
        public ClubModel GetModel(string _Sysno)
        {
            ClubModel model = new ClubModel();
            DataTable dt = ClubDac.SelectList(string.Format(" and sysno='{0}'", _Sysno));
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<ClubModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 获取俱乐部清单
        ///
        /// </summary>
        /// <returns></returns>
        public List<ClubModel> GetList()
        {
            List<ClubModel> modellist = new List<ClubModel>();
            DataTable dt = ClubDac.SelectList(" and status=2");
            dt.Columns.Add("MemberQty", typeof(int));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr["MemberQty"] = Convert.ToInt32(ClubMemBiz.instance.GetClubMemberQty(dr["SYSNO"].ToString()));
                    if (dr["ext2"].ToString() == "" || dr["ext2"].ToString()==null)
                    {
                        dr["ext2"] = "/images/club/club1.jpg";
                    }
                }
                DataView dv = dt.DefaultView;
                dv.Sort = "MemberQty desc";
                modellist = JsonHelper.ParseDtModelList<List<ClubModel>>(dv.ToTable());
            }

            
            return modellist;
        }

        /// <summary>
        /// 获得随机俱乐部
        /// </summary>
        /// <returns></returns>
        public List<ClubModel> GetModellist()
        {
            List<ClubModel> modellist = new List<ClubModel>();
            string sql = "select top 3 * from wtf_club where status=2 ORDER BY NEWID()";
            DataTable dt= DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                modellist = JsonHelper.ParseDtModelList<List<ClubModel>>(dt);
            }
            return modellist;
        }

        /// <summary>
        /// 获得随机俱乐部
        /// </summary>
        /// <returns></returns>
        public List<ClubModel> GetApplyClublist()
        {
            List<ClubModel> modellist = new List<ClubModel>();
            string sql = "select * from wtf_club where status=1 ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                modellist = JsonHelper.ParseDtModelList<List<ClubModel>>(dt);
            }
            return modellist;
        }

        /// <summary>
        /// 获得所有俱乐部
        /// </summary>
        /// <returns></returns>
        public List<ClubModel> GetAllClublist()
        {
            List<ClubModel> modellist = new List<ClubModel>();
            string sql = "select * from wtf_club where status=2 order by id desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                modellist = JsonHelper.ParseDtModelList<List<ClubModel>>(dt);
               
            }
            return modellist;
        }

        /// <summary>
        /// 添加新的俱乐部
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(ClubModel model)
        {
            model.SYSNO = Guid.NewGuid().ToString("N").ToUpper();
            model.STATUS = "1";
            if (ClubDac.Insert(model))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string AddNewClub(ClubModel model)
        {
            model.SYSNO = Guid.NewGuid().ToString("N").ToUpper();
            model.STATUS = "1";
            if (ClubDac.Insert(model))
            {
                return model.SYSNO;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 判断俱乐部名称是否重复
        /// </summary>
        /// <param name="_ClubName"></param>
        /// <returns></returns>
        public bool IsClubNameDuplicated(string _ClubName)
        {
            DataTable dt = ClubDac.SelectList(" and ClubName='" + _ClubName + "'");
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
        /// Get Manage Clubs
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public List<ClubModel> GetManageClubs(string _Memsys)
        {
            List<ClubModel> list = new List<ClubModel>();
            string sql = "select * from wtf_clubmember where job='管理员' and memsys='"+_Memsys+"' and ext1='1'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ClubModel model = GetModel(dr["clubsys"].ToString());
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 更新申请俱乐部状态
        /// </summary>
        /// <param name="_ClubSys"></param>
        /// <param name="_Status"></param>
        /// <returns></returns>
        public bool UpdateClubStatus(string _ClubSys, string _Status)
        {
            string sql = "update wtf_club set status='"+_Status+"' where sysno='"+_ClubSys+"'";
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
        /// update club logo
        /// </summary>
        /// <param name="_Clubsys"></param>
        /// <param name="_Imgurl"></param>
        public int UpdateClubLogo(string _Clubsys, string _Imgurl)
        {
            string sql = "update wtf_club set ext2='" + _Imgurl + "' where sysno='" + _Clubsys + "'";
            int a = DbHelperSQL.ExecuteSql(sql);
            return a;
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="_FieldName"></param>
        /// <param name="_Value"></param>
        /// <param name="_ClubSys"></param>
        /// <returns></returns>
        public bool UpdateClubField(string _FieldName, string _Value, string _ClubSys)
        {
            string sql = string.Format("Update wtf_club set {0}='{1}' where sysno='{2}'",_FieldName,_Value,_ClubSys);
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
    }
}
