using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WeTour
{
    public class MemberDll
    {
        public MemberDll() { }
        private static MemberDll _Instance;
        public static MemberDll Get_Instance()
        {
            if (_Instance == null)
            {
                _Instance = new MemberDll();
            }
            return _Instance;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public MemberModel GetModel(string _Sys)
        {
            MemberModel model = new MemberModel();
            DataTable dt = MemberDac.SelectList(string.Format(" and sysno='{0}'", _Sys));
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<MemberModel>(dt);
            }
            return model;
        }
        /// <summary>
        /// 根据姓名获取实体
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public MemberModel GetModelbyName(string _Name)
        {
            MemberModel model = new MemberModel();
            DataTable dt = MemberDac.SelectList(string.Format(" and Name='{0}'", _Name));
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<MemberModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据微信号获取实体
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public MemberModel GetModelbyWeChatId(string _WeChatId)
        {
            MemberModel model = new MemberModel();
            DataTable dt = MemberDac.SelectList(string.Format(" and ext3='{0}'", _WeChatId));
            if (dt.Rows.Count == 1)
            {
                model = JsonHelper.ParseDtInfo<MemberModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据电话获取会员实体
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public MemberModel GetModelbyTelePhone(string _Sys)
        {
            MemberModel model = new MemberModel();
            DataTable dt = MemberDac.SelectList(string.Format(" and Telephone='{0}'", _Sys));
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<MemberModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据城市id获得该城市的的城市经理主键
        /// </summary>
        /// <param name="_CityId"></param>
        /// <returns></returns>
        public string GetMgrsysByCity(string _CityId)
        {
            string Mgrsys = "";
            string sql = "select * from wtf_member where cityid='" + _CityId + "' and job='城市经理' and status=1";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Mgrsys = dt.Rows[0]["sysno"].ToString();
            }
            return Mgrsys;
        }

     
        /// <summary>
        /// 判断是否已关注
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <param name="_Followsys"></param>
        /// <returns></returns>
        public bool IsMemberFollow(string _Memsys, string _Followsys)
        {
            string sql = string.Format("select * from wtf_MemberFollow where membersys='{0}' and followsys='{1}'", _Memsys, _Followsys);
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
        /// 获取指定会员关注的人
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public DataTable GetFollower(string _Memsys)
        {
            string sql = "select * from wtf_MemberFollow a left join wtf_member b on a.followsys=b.sysno where a.membersys='" + _Memsys + "'";
            return DbHelperSQL.Query(sql).Tables[0];
        }

        /// <summary>
        /// 获得关注的人
        /// 
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public List<MemberModel> GetFollowerModellist(string _Memsys)
        {
            List<MemberModel> list = new List<MemberModel>();
            string sql = "select b.* from wtf_MemberFollow a left join wtf_member b on a.followsys=b.sysno where a.membersys='" + _Memsys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["EXT1"].ToString() == "")
                    {
                        dr["EXT1"] = "/images/touxiang.jpg";
                    }
                }
                list = JsonHelper.ParseDtModelList<List<MemberModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 获取关注该会员的人
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public DataTable GetFollowMes(string _Memsys)
        {
            string sql = "select * from wtf_MemberFollow a left join wtf_member b on a.membersys=b.sysno where a.followsys='" + _Memsys + "'";
            return DbHelperSQL.Query(sql).Tables[0];
        }

        /// <summary>
        /// 获取作者列表中关于读者的未读消息数量
        /// </summary>
        /// <param name="_Writer"></param>
        /// <param name="_Reader"></param>
        /// <returns></returns>
        public string GetTendQty(string _Writer, string _Reader)
        {
            int Qty = 0;
            string sql = "select * from wtf_trend where creatememsys='" + _Writer + "' and status=1";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                if (!TrendDll.Get_Instance().IsTrendReaded(dr["id"].ToString(), _Reader))
                {
                    Qty += 1;
                }
            }
            return Qty.ToString();
        }

        /// <summary>
        /// 获取作者列表中关于读者的未读消息列表
        /// </summary>
        /// <param name="_Writer"></param>
        /// <param name="_Reader"></param>
        /// <returns></returns>
        public DataTable GetNewTrend(string _Writer, string _Reader)
        {
            string sql = "";
            if (_Reader == "")
            {
                //游客查看到的动态
                sql = "select top 2* from wtf_trend where CreateMemSys='" + _Writer + "' and status=1 order by id desc";
            }
            else if (IsFollower(_Reader, _Writer))
            {
                //会员查看到的动态
                sql = "select * from wtf_trend where CreateMemSys='" + _Writer + "' and status=1 order by id desc";
            }
            else
            {
                sql = "select top 2* from wtf_trend where CreateMemSys='" + _Writer + "' and status=1 order by id desc";
            }
            return DbHelperSQL.Query(sql).Tables[0];
        }

        /// <summary>
        /// 判断是否已关注了特定的人
        /// </summary>
        /// <param name="_Member">被关注的人</param>
        /// <param name="_Followsys">关注他的人</param>
        /// <returns></returns>
        public bool IsFollower(string _Member, string _Followsys)
        {
            string sql = "select * from wtf_memberfollow where membersys='" + _Member + "' and followsys='" + _Followsys + "'";
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
        /// 修改会员头像
        /// </summary>
        /// <param name="_memsys"></param>
        /// <param name="_imgurl"></param>
        public void UpdateHeadImg(string _memsys, string _imgurl)
        {
            string sql = "update wtf_member set ext1='" + _imgurl + "' where sysno='" + _memsys + "'";
            DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// 修改赛事经理的级别
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <param name="_Grade"></param>
        public void UpdateCityMgrGrade(string _Memsys, string _Grade)
        {
            string sql = "";
            string _grade = GetCityMgrGrade(_Memsys);
            if (_grade == "")
            {
                sql = string.Format("insert into wtf_CityMgr (Membersys,isAuthorized,StartDate) values ('{0}','{1}','{2}')", _Memsys, _Grade, DateTime.Now.ToString());
            }
            else
            {
                sql = string.Format("Update wtf_CityMgr set isAuthorized='{0}' where Membersys='{1}'", _Grade, _Memsys);
            }
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// 获取赛事经理的级别
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public string GetCityMgrGrade(string _Memsys)
        {
            string _Grade = "";
            string sql = "select * from wtf_CityMgr where Membersys='" + _Memsys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                _Grade = dt.Rows[0]["isAuthorized"].ToString();
            }
            return _Grade;
        }

        ///// <summary>
        ///// 根据会员sys获得首页资料
        ///// </summary>
        ///// <param name="_Memsys"></param>
        ///// <returns></returns>
        //public MemberModel GetMemberCenterInfo(string _Memsys)
        //{
        //    MemberModel mem = MemberDll.Get_Instance().GetModel(_Memsys);
        //    MemRankModel model = RankingDll.Get_Instance().getMemRankbySysno(_Memsys);

        //    mem.SinglRk = RankDll.instance.GetWetennisRankbyMem(_Memsys, "单打", mem.GENDER);
        //    mem.DoubRk = RankDll.instance.GetWetennisRankbyMem(_Memsys, "双打", mem.GENDER);

        //    mem.MsgQty = MsgBll.instance.GetUnreadQty(_Memsys).ToString();
        //    mem.SocialPts = RPointDll.instance.GetSocialScort(_Memsys);

        //    return mem;
        //}

        /// <summary>
        /// 获取所有会员
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllMember()
        {
            string sql = "select * from wtf_member";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt;
        }

        /// <summary>
        /// 根据会员主键获得基础信息
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public List<PlayerModel> GetPlayerBasicInfo(string _Memsys)
        {
            List<PlayerModel> list = new List<PlayerModel>();
            MemberModel mmodel = GetModel(_Memsys);

            //添加用户名
            PlayerModel pmode0 = new PlayerModel();
            pmode0.SYS = "用户名";
            pmode0.PNAME = mmodel.NAME;
            list.Add(pmode0);

            //添加姓名
            PlayerModel pmodel1 = new PlayerModel();
            pmodel1.SYS = "姓名";
            pmodel1.PNAME = mmodel.USERNAME;
            list.Add(pmodel1);

            //电话
            PlayerModel pmodel12 = new PlayerModel();
            pmodel12.SYS = "电话";
            pmodel12.PNAME = mmodel.TELEPHONE;
            //list.Add(pmodel12);

            //身份证
            PlayerModel pmodel120 = new PlayerModel();
            pmodel120.SYS = "身份证号";
            pmodel120.PNAME = mmodel.EXT2;
            //list.Add(pmodel120);

            //生日
            PlayerModel pmodel2 = new PlayerModel();
            pmodel2.SYS = "生日";
            pmodel2.PNAME = mmodel.BIRTHDAY;
            list.Add(pmodel2);

            //性别
            PlayerModel pmodel3 = new PlayerModel();
            pmodel3.SYS = "性别";
            pmodel3.PNAME = mmodel.GENDER;
            list.Add(pmodel3);

            //添加城市
            PlayerModel pmodel4 = new PlayerModel();
            pmodel4.SYS = "城市";
            pmodel4.PNAME = mmodel.PROVINCE + "-" + mmodel.CITY + "-" + mmodel.REGION;
            list.Add(pmodel4);

            //地址
            PlayerModel pmodel5 = new PlayerModel();
            pmodel5.SYS = "地址";
            pmodel5.PNAME = mmodel.ADDRESS;
            list.Add(pmodel5);

            //网球元年
            PlayerModel pmodel6 = new PlayerModel();
            pmodel6.SYS = "网球元年";
            pmodel6.PNAME = mmodel.TENNISSTARTYEAR;
            list.Add(pmodel6);

            //正拍
            PlayerModel pmodel7 = new PlayerModel();
            pmodel7.SYS = "正拍";
            pmodel7.PNAME = mmodel.FOREHAND;
            list.Add(pmodel7);

            //正拍
            PlayerModel pmodel8 = new PlayerModel();
            pmodel8.SYS = "反拍";
            pmodel8.PNAME = mmodel.BACKHAND;
            list.Add(pmodel8);

            //身高
            PlayerModel pmodel9 = new PlayerModel();
            pmodel9.SYS = "身高";
            pmodel9.PNAME = mmodel.HEIGHT;
            list.Add(pmodel9);

            //体重
            PlayerModel pmodel10 = new PlayerModel();
            pmodel10.SYS = "体重";
            pmodel10.PNAME = mmodel.WEIGHT;
            list.Add(pmodel10);

            return list;
        }

        /// <summary>
        /// 根据会员主键获得基础信息
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public List<PlayerModel> GetPlayerBasicInfoSelf(string _Memsys)
        {
            List<PlayerModel> list = new List<PlayerModel>();
            MemberModel mmodel = GetModel(_Memsys);

            //添加用户名
            PlayerModel pmode0 = new PlayerModel();
            pmode0.SYS = "用户名";
            pmode0.PNAME = mmodel.NAME;
            list.Add(pmode0);

            //添加姓名
            PlayerModel pmodel1 = new PlayerModel();
            pmodel1.SYS = "姓名";
            pmodel1.PNAME = mmodel.USERNAME;
            list.Add(pmodel1);

            //电话
            PlayerModel pmodel12 = new PlayerModel();
            pmodel12.SYS = "电话";
            pmodel12.PNAME = mmodel.TELEPHONE;
            list.Add(pmodel12);

            //身份证
            PlayerModel pmodel120 = new PlayerModel();
            pmodel120.SYS = "身份证号";
            pmodel120.PNAME = mmodel.EXT2;
            list.Add(pmodel120);

            //生日
            PlayerModel pmodel2 = new PlayerModel();
            pmodel2.SYS = "生日";
            pmodel2.PNAME = mmodel.BIRTHDAY;
            list.Add(pmodel2);

            //性别
            PlayerModel pmodel3 = new PlayerModel();
            pmodel3.SYS = "性别";
            pmodel3.PNAME = mmodel.GENDER;
            list.Add(pmodel3);

            //添加城市
            PlayerModel pmodel4 = new PlayerModel();
            pmodel4.SYS = "城市";
            pmodel4.PNAME = mmodel.PROVINCE + "-" + mmodel.CITY + "-" + mmodel.REGION;
            list.Add(pmodel4);

            //地址
            PlayerModel pmodel5 = new PlayerModel();
            pmodel5.SYS = "地址";
            pmodel5.PNAME = mmodel.ADDRESS;
            list.Add(pmodel5);

            //网球元年
            PlayerModel pmodel6 = new PlayerModel();
            pmodel6.SYS = "网球元年";
            pmodel6.PNAME = mmodel.TENNISSTARTYEAR;
            list.Add(pmodel6);

            //正拍
            PlayerModel pmodel7 = new PlayerModel();
            pmodel7.SYS = "正拍";
            pmodel7.PNAME = mmodel.FOREHAND;
            list.Add(pmodel7);

            //正拍
            PlayerModel pmodel8 = new PlayerModel();
            pmodel8.SYS = "反拍";
            pmodel8.PNAME = mmodel.BACKHAND;
            list.Add(pmodel8);

            //身高
            PlayerModel pmodel9 = new PlayerModel();
            pmodel9.SYS = "身高";
            pmodel9.PNAME = mmodel.HEIGHT;
            list.Add(pmodel9);

            //体重
            PlayerModel pmodel10 = new PlayerModel();
            pmodel10.SYS = "体重";
            pmodel10.PNAME = mmodel.WEIGHT;
            list.Add(pmodel10);

            return list;
        }

        /// <summary>
        /// 修改会员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdatePlayerInfo(MemberModel model)
        {
            string sql = string.Format("update wtf_Member set BIRTHDAY='{0}',GENDER='{1}',PROVINCEID='{2}',CITYID='{3}',REGIONID='{4}',PROVINCE='{5}',CITY='{6}',REGION='{7}',ADDRESS='{8}',TENNISSTARTYEAR='{9}',FOREHAND='{10}',BACKHAND='{11}',HEIGHT='{12}',WEIGHT='{13}',NAME='{15}',USERNAME='{16}',TELEPHONE='{17}',EXT2='{18}' where SYSNO='{14}'", model.BIRTHDAY, model.GENDER, model.PROVINCEID, model.CITYID, model.REGIONID, model.PROVINCE, model.CITY, model.REGION, model.ADDRESS, model.TENNISSTARTYEAR, model.FOREHAND, model.BACKHAND, model.HEIGHT, model.WEIGHT, model.SYSNO, model.NAME, model.USERNAME, model.TELEPHONE, model.EXT2);
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
        /// 获得系统中的裁判
        /// </summary>
        /// <returns></returns>
        public DataTable GetUmpires()
        {
            string sql = "select sysno,name from wtf_Member where job='裁判' ";
            return DbHelperSQL.Query(sql).Tables[0];
        }


        //#region 团体赛
        //public TeamModel GetTeamModel(string id)
        //{
        //    TeamModel model = new TeamModel();
        //    string sql = "select * from wtf_team where tid='" + id + "'";
        //    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        //    if (dt.Rows.Count > 0)
        //    {
        //        model = JsonHelper.ParseDtInfo<TeamModel>(dt);
        //    }
        //    return model;
        //}
        //#endregion

        /// <summary>
        /// 判断员工信息是否完整，是否已填写身份证号
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public bool IsMemberInfoFull(string _Memsys)
        {
            MemberModel model = GetModel(_Memsys);
            //验证身份证
            if (string.IsNullOrEmpty(model.EXT2))
            {
                return false;
            }

            //验证电话号码
            if (string.IsNullOrEmpty(model.TELEPHONE))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 修改会员报名时完善的信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateApplyInfo(MemberModel model)
        {
            string sql = string.Format("update wtf_member set USERNAME='{0}',EXT2='{1}',TELEPHONE='{2}' where sysno='{3}'", model.USERNAME, model.EXT2, model.TELEPHONE, model.SYSNO);
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
        /// 根据条件查看会员
        /// </summary>
        /// <param name="_Name"></param>
        /// <returns></returns>
        public List<MemberModel> SearchMember(string _Name)
        {
            List<MemberModel> list = new List<MemberModel>();
            string sql = "select * from wtf_member where Name like '%" + _Name + "%' or username like '%" + _Name + "%' or telephone like '%" + _Name + "%'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["EXT1"].ToString() == "")
                    {
                        dr["EXT1"] = "/images/touxiang.jpg";
                    }
                }
                list = JsonHelper.ParseDtModelList<List<MemberModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Get Member list by Real Name
        /// </summary>
        /// <param name="_Name"></param>
        /// <returns></returns>
        public List<MemberModel> GetMemberlistbyName(string _Name)
        {
            List<MemberModel> list = new List<MemberModel>();
            string sql = "select * from wtf_member where username='" + _Name + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<MemberModel>>(dt);
            }
            return list;
        }

    }
}