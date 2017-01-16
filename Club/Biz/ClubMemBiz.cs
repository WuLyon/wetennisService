using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Member;
using SMS;

namespace Club
{
    public class ClubMemBiz
    {
        public static ClubMemBiz instance = new ClubMemBiz();

        /// <summary>
        /// Add New ClubMember
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddClubMember(ClubMemModel model)
        {
            //限制一个人只能加入一个俱乐部。2016年8月9日，取消此限制
            //if (!IsMemberAlreadyInClub( model.MEMSYS))
            if (1==1)
            {
                string sql = string.Format("Insert into wtf_clubmember (clubsys,memsys,job,ext1,ext2,ext3) values ('{0}','{1}','{2}','{3}','{4}','{5}')", model.CLUBSYS, model.MEMSYS, model.JOB, model.EXT1, model.EXT2, model.EXT3);
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
                return false;
            }
        }

        /// <summary>
        /// Delete a club memeber
        /// </summary>
        /// <param name="_Clubsys"></param>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public bool DeleteClubmem(string _Clubsys,string _Memsys)
        {
            string sql = "Delete wtf_clubmember where clubsys='" + _Clubsys + "' and memsys='" + _Memsys + "'";
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
        /// Is member already in club
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public bool IsMemberAlreadyInClub(string _Memsys)
        {
            string sql = "select * from wtf_clubmember where memsys='"+_Memsys+"'";
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
        /// Delete Club Member by id
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public bool DeleteClubMember(string _id)
        {
            string sql = "update wtf_clubmember set ext1='99' where id='"+_id+"'";
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
        /// Update Club Member Status
        /// </summary>
        /// <param name="_ClubSys"></param>
        /// <param name="_Memsys"></param>
        /// <param name="_Status"></param>
        /// <returns></returns>
        public bool UpdateMemStatus(string _ClubSys, string _Memsys, string _Status)
        {
            string sql = "update wtf_clubmember set ext1='" + _Status + "' where clubsys='"+_ClubSys+"' and memsys='"+_Memsys+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                ClubModel cmodel = ClubBiz.Get_Instance().GetModel(_ClubSys);
                WeMemberModel model=WeMemberDll.instance.GetModel(_Memsys);
                string Msg = "";
                if (_Status == "1")
                {
                    //accept
                    Msg = model.USERNAME+",您好！您已成功加入"+cmodel.CLUBNAME+"。登陆微网球，体验更多俱乐部功能。";
                }
                else
                { 
                    //refuse
                    Msg = model.USERNAME + ",您好！您申请加入" + cmodel.CLUBNAME + "的申请已被管理员拒绝。请选择其他的俱乐部！";
                }
                SMSdll.instance.BatchSendSMS(model.TELEPHONE, Msg);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get Club member relationship
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public ClubMemModel getModel(string _id)
        {
            ClubMemModel model = new ClubMemModel();
            string sql = "select * from wtf_clubmember where id='" + _id + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<ClubMemModel>(dt);
            }
            return model;
        }

       

        /// <summary>
        /// Get Member's Club
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public List<ClubModel> GetListbyMemsys(string _Memsys)
        {
            List<ClubModel> list = new List<ClubModel>();
            string sql = "select b.* from wtf_clubmember a left join wtf_club b on a.clubsys=b.sysno where a.memsys='" + _Memsys + "' and a.ext1='1' and a.job='管理员'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<ClubModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Get Club's Members 
        /// </summary>
        /// <param name="_Clubsys"></param>
        /// <returns></returns>
        public List<ClubMemModel> GetListByClub(string _Clubsys,string _Status)
        {
            List<ClubMemModel> list = new List<ClubMemModel>();
            string sql = "select * from wtf_clubmember where clubsys='"+_Clubsys+"' and ext1='"+_Status+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<ClubMemModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Get club member who haven't applied for a tourcontent
        /// </summary>
        /// <param name="_Clubsys"></param>
        /// <param name="_Contentid"></param>
        /// <returns></returns>
        public List<ClubMemModel> GetUnAppliedClubMem(string _Clubsys, string _Contentid)
        {
            List<ClubMemModel> list = new List<ClubMemModel>();
            string sql = "select * from wtf_clubmember where clubsys='" + _Clubsys + "' and ext1='1' and memsys not in (select memberid from wtf_tourapply where contentid='" + _Contentid + "' )and memsys not in (select paterner from wtf_tourapply where contentid='"+_Contentid+"' )";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<ClubMemModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Is Member already in a Club
        /// </summary>
        /// <param name="_ClubSys"></param>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public bool IsMemberInClub(string _ClubSys, string _Memsys)
        {
            string sql = "select * from wtf_clubmember where clubsys='" + _ClubSys + "' and memsys='" + _Memsys + "'";
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
        /// Send an apply message to club owner
        /// </summary>
        /// <param name="_Clubsys"></param>
        /// <param name="_Memsys"></param>
        public void SendClubApplyMsg(string _Clubsys, string _Memsys)
        {
            string Msg = "";
            WeMemberModel model = WeMemberDll.instance.GetModel(_Memsys);
            ClubModel cmodel = ClubBiz.Get_Instance().GetModel(_Clubsys);
            Msg = "您好!"+model.NAME+"("+model.USERNAME+")申请加入"+cmodel.CLUBNAME+"。请尽快处理！";
            List<ClubMemModel> list = getClubManager(_Clubsys);
            if (list.Count > 0)
            {
                foreach (ClubMemModel lmodel in list)
                {
                    WeMemberModel cmmodel = WeMemberDll.instance.GetModel(lmodel.MEMSYS);
                    SMSdll.instance.BatchSendSMS(cmmodel.TELEPHONE, Msg);
                }
            }           

        }

        /// <summary>
        /// Is Club Manager
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public bool IsMemberClubManager(string _Memsys)
        {
            string sql = "select * from wtf_clubmember where memsys='" + _Memsys + "' and job='管理员' and ext1='1'";
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
        /// <param name="_ClubSys"></param>
        /// <returns></returns>
        public string GetClubMemberQty(string _ClubSys)
        {
            string sql = "select count(*) from wtf_clubmember where ext1='1' and  clubsys='"+_ClubSys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows[0][0].ToString();
        }

        /// <summary>
        /// Set manager for club
        /// </summary>
        /// <param name="_ClubSys"></param>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public bool SetClubManager(string _ClubSys, string _Memsys)
        {
            try
            {
                if (!IsMemberInClub(_ClubSys, _Memsys))
                {
                    //Add Memsys first
                    ClubMemModel cmodel = new ClubMemModel();
                    cmodel.CLUBSYS = _ClubSys;
                    cmodel.MEMSYS = _Memsys;
                    AddClubMember(cmodel);
                }
            }
            catch
            { 
            }

            //update job
            string sql = "update wtf_clubmember set job='管理员',ext1=1 where clubsys='" + _ClubSys + "' and memsys='" + _Memsys + "'";
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
        /// GEt Club's Managers
        /// </summary>
        /// <param name="_ClubSys"></param>
        /// <returns></returns>
        public List<ClubMemModel> getClubManager(string _ClubSys)
        {
            List<ClubMemModel> list = new List<ClubMemModel>();
            string sql = "select * from wtf_clubmember where clubsys='" + _ClubSys + "' and job='管理员' and ext1=1";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<ClubMemModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Get Clubsys by Memsys
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public string GetClubsysbyMgr(string _Memsys)
        {
            string Clubsys = "";
            string sql = "select * from wtf_clubMember where Memsys='" + _Memsys + "' and Job='管理员' and ext1=1";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Clubsys = dt.Rows[0]["Clubsys"].ToString();
            }
            return Clubsys;
        }

        /// <summary>
        /// Get member's club name
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public string GetMemberClub(string _Memsys)
        {
            string _ClubName="";
            string sql = "select b.ClubName from wtf_clubMember a left join wtf_club b on a.Clubsys=b.sysno where a.Memsys='" + _Memsys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                _ClubName = dt.Rows[0][0].ToString();
            }
            return _ClubName;
        }

        /// <summary>
        /// 获取第一个管理员信息
        /// </summary>
        /// <param name="_Clubsys"></param>
        /// <returns></returns>
        public string GetClubManagerSys(string _Clubsys)
        {
            string _MgrSys="";
            string sql = string.Format("select * from wtf_clubMember where Clubsys='{0}' and job='管理员' and ext1='1'",_Clubsys);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            { 
                //默认获取第一个管理员
                _MgrSys = dt.Rows[0]["Memsys"].ToString();
            }
            return _MgrSys;
        }
    }
}
