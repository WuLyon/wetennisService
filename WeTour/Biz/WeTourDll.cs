using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Member;

namespace WeTour
{
    /// <summary>
    /// Methods of Wetennis Tours, from create to Finish,include every section
    /// </summary>
    public class WeTourDll
    {
        public static WeTourDll instance = new WeTourDll();

        /// <summary>
        /// Get TourModel by sysno
        /// </summary>
        /// <param name="sysno"></param>
        /// <returns></returns>
        public WeTourModel GetModelbySys(string sysno)
        {
            WeTourModel model = new WeTourModel();
            string strSql = "select * from wtf_CityTour where sysno='" + sysno + "'";
            DataTable dt = DbHelperSQL.Query(strSql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<WeTourModel>(dt);
            }
            //修改比赛状态
            model.StatusName = RenderTourStatus(model.STATUS);
            return model;
        }

        /// <summary>
        /// 获得微网球级别赛事
        /// </summary>
        /// <returns></returns>
        public List<WeTourModel> GetWeTennisTour()
        {
            List<WeTourModel> list = new List<WeTourModel>();
            string sql = "select * from wtf_CityTour where CityType='' and status>0 and status<10 order by id desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourModel>>(dt);
                //update Status
                foreach (WeTourModel model in list)
                {
                    model.StatusName = RenderTourStatus(model.STATUS);
                }
            }
            return list;
        }

        /// <summary>
        /// 获得举办赛事的城市
        /// </summary>
        /// <returns></returns>
        public List<CityInfo> GetTourCities()
        {
            List<CityInfo> list = new List<CityInfo>();
            string sql = "select * from hat_city where cityid in (select distinct(cityid) from wtf_CityTour where CityType='' and status>0 and status<10 and cityid!='510100')";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<CityInfo>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 获取赛事所在城市
        /// 刘涛,2016-08-19
        /// </summary>
        /// <returns></returns>
        public List<CityInfo> GetTourCity()
        {
            List<CityInfo> list = new List<CityInfo>();
            string sql = "select distinct(cityid) as cityid from wtf_CityTour";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<CityInfo>>(dt);
            }
            return list;
        }

        public List<WeTourModel> GetCityTours(string _Status,string _Location)
        {
            List<WeTourModel> list = new List<WeTourModel>();
            string sql = "select * from wtf_CityTour where 1=1";
            //添加赛事筛选条件
            if (_Status == "-1")
            {
                sql += " and  status>0 and status<10 ";
            }
            else
            {
                sql += " and status='"+_Status+"'";
            }

            if (_Location != "0")
            {
                sql += " and cityid='"+_Location+"'";
            }

            sql += " order by id desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourModel>>(dt);
                //update Status
                foreach (WeTourModel model in list)
                {
                    model.StatusName = RenderTourStatus(model.STATUS);
                }
            }
            return list;
        }

        public List<CityInfo> GetTourCitiesNew()
        {
            List<CityInfo> list = new List<CityInfo>();
            string sql = "select distinct(cityid) from wtf_CityTour where status>0 and status<10 ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<CityInfo>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 根据比赛主键获得赛事信息，2016-1-5，liutao
        /// </summary>
        /// <param name="_MatchSys"></param>
        /// <returns></returns>
        public WeTourModel GetTourModelbyMatchsys(string _MatchSys)
        {
            WeMatchModel model = WeMatchDll.instance.GetModel(_MatchSys);
            return GetModelbySys(model.TOURSYS);
        }

        /// <summary>
        /// 根据城市,页数来加载赛事内容
        /// </summary>
        /// <param name="_CityId"></param>
        /// <param name="_PageSize"></param>
        /// <param name="_Page"></param>
        /// <returns></returns>
        public List<WeTourModel> GetWeTennisTourPage(string _CityId,string _PageSize, string _Page)
        {
            int Ps = Convert.ToInt32(_PageSize);
            int pg = Convert.ToInt32(_Page);
            int startr = (pg - 1) * Ps + 1;
            int endr = pg * Ps+1;

            List<WeTourModel> list = new List<WeTourModel>();
            string sql = string.Format("select * from (select *,row_number() over (order by id desc) rn from wtf_CityTour where CityType='' and Cityid='{0}' and status>0 and status<10 ) a where a.rn<{1} and a.rn>={2}",_CityId,endr.ToString(),startr.ToString());
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourModel>>(dt);
                //update Status
                foreach (WeTourModel model in list)
                {
                    model.StatusName = RenderTourStatus(model.STATUS);
                }
            }
            return list;
        }


        public string RenderTourLevel(string _TourLevel)
        {
            string LevelImg = "";
            switch (_TourLevel)
            {
                case "125":
                    LevelImg = "/Lib/img/tour/125A.png";
                    break;
                case "250":
                    LevelImg = "/Lib/img/tour/250A.png";
                    break;
                case "500":
                    LevelImg = "/Lib/img/tour/500A.png";
                    break;
                case "1000":
                    LevelImg = "/Lib/img/tour/1000A.png";
                    break;
                case "2000":
                    LevelImg = "/Lib/img/tour/2000A.png";
                    break;
            }
            return LevelImg;
        }

        /// <summary>
        /// Rend Tour Status
        /// </summary>
        /// <param name="_Status"></param>
        /// <returns></returns>
        public  string RenderTourStatus(string _Status)
        {
            string _statusStr = "";
            switch (_Status)
            {
                case "0":
                    _statusStr = "筹备中";
                    break;
                case "1":
                    _statusStr = "报名中";
                    break;
                case "2":
                    _statusStr = "报名结束";
                    break;
                case "3":
                    _statusStr = "已分配签表";
                    break;
                case "4":
                    _statusStr = "进行中";
                    break;
                case "5":
                    _statusStr = "已完成";
                    break;
            }
            return _statusStr;
        }

        /// <summary>
        /// Get TourModel by id
        /// </summary>
        /// <param name="sysno"></param>
        /// <returns></returns>
        public WeTourModel GetModelbyId(string _Id)
        {
            WeTourModel model = new WeTourModel();
            string strSql = "select * from wtf_CityTour where id='" + _Id + "'";
            DataTable dt = DbHelperSQL.Query(strSql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<WeTourModel>(dt);
            }
            return model;
        }

        #region 增删改
        /// <summary>
        /// Add a New CityTour
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string AddNewTour(WeTourModel model)
        {
            model.SYSNO = Guid.NewGuid().ToString("N").ToUpper();
            model.STATUS = "0";
            string sql = string.Format("insert into wtf_CityTour (sysno,Status,Name,TourSys,MgrSys,StartDate,EndDate,Capacity,SetType,GameType,Address,CityId,CityType,Description,TourImg,StartHour,ext1,ext2,ext3,ext4,ext5,UnionSys,Host,Asso_Host,TourBackImg,ext6,ext7,ext8,ext9,ext10) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}')", model.SYSNO, model.STATUS, model.NAME, model.TOURSYS, model.MGRSYS, model.STARTDATE, model.ENDDATE, model.CAPACITY, model.SETTYPE, model.GAMETYPE, model.ADDRESS, model.CITYID, model.CITYTYPE, model.DESCRIPTION, model.TOURIMG, model.STARTHOUR, model.EXT1, model.EXT2, model.EXT3, model.EXT4, model.EXT5, model.UnionSys, model.Host, model.Asso_Host,model.TourBackImg,model.ext6,model.ext7,model.ext8,model.ext9,model.ext10);
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return model.SYSNO;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 修改赛事信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateTourInfo(WeTourModel model)
        {
            string sql = string.Format("update wtf_CityTour set Name='{0}',TourSys='{1}',StartDate='{2}',EndDate='{3}',Capacity='{4}',SetType='{5}',GameType='{6}',Address='{7}',CityId='{8}',CityType='{9}',Description='{10}',TourImg='{11}',StartHour='{12}',ext1='{13}',ext2='{14}',ext3='{15}',ext4='{16}',ext5='{17}',UnionSys='{18}',Host='{19}',Asso_Host='{20}',TourBackImg='{21}',ext6='{22}',ext7='{23}',ext8='{24}',ext9='{25}',ext10='{26}' where sysno='{27}'",model.NAME,model.TOURSYS,model.STARTDATE,model.ENDDATE,model.CAPACITY,model.SETTYPE,model.GAMETYPE,model.ADDRESS,model.CITYID,model.CITYTYPE,model.DESCRIPTION,model.TOURIMG,model.STARTHOUR,model.EXT1,model.EXT2,model.EXT3,model.EXT4,model.EXT5,model.UnionSys,model.Host,model.Asso_Host,model.TourBackImg,model.ext6,model.ext7,model.ext8,model.ext9,model.ext10,model.SYSNO);
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

        /// <summary>
        /// Get Wetennis Tours
        /// 添加联盟赛事状态
        /// </summary>
        /// <param name="_MgrSys"></param>
        /// <param name="_CityType">""表示公开赛赛，“Club”表示俱乐部赛，“Union”表示联盟赛</param>
        /// <returns></returns>
        public List<WeTourModel> GetWeTourList(string _MgrSys, string _CityType)
        {
            List<WeTourModel> list = new List<WeTourModel>();
            string sql = "";
            if (_CityType == "Club")
            {
                //查询所有俱乐部发起的比赛，包含公开赛，俱乐部赛，联盟赛
                sql = "select * from wtf_CityTour where mgrsys='" + _MgrSys + "' order by id desc";
            }
            else
            {
                //查询所有的赛事
                sql = "select * from wtf_CityTour where mgrsys='" + _MgrSys + "' and citytype='" + _CityType + "' order by id desc";
            }
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 根据状态来获取赛事列表
        /// </summary>
        /// <param name="_MgrSys"></param>
        /// <param name="_CityType"></param>
        /// <returns></returns>
        public List<WeTourModel> GetWeTourListbyStatus(string _MgrSys, string _CityType,string _Status)
        {
            List<WeTourModel> list = new List<WeTourModel>();
            string sql = "";
            if (_CityType == "Club")
            {
                if (_Status != "")
                {
                    //查询所有俱乐部发起的比赛，包含公开赛，俱乐部赛，联盟赛
                    sql = "select * from wtf_CityTour where mgrsys='" + _MgrSys + "' and Status='" + _Status + "' order by id desc";
                }
                else
                {
                    sql = "select * from wtf_CityTour where mgrsys='" + _MgrSys + "' and Status!='99' order by id desc";
                }
            }
            else
            {
                //查询所有的赛事
                if (_Status != "")
                {
                    sql = "select * from wtf_CityTour where mgrsys='" + _MgrSys + "' and citytype='" + _CityType + "' and Status='" + _Status + "' order by id desc";
                }
                else
                {
                    sql = "select * from wtf_CityTour where mgrsys='" + _MgrSys + "' and citytype='" + _CityType + "'  and Status!='99' order by id desc";
                }
            
            }
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 根据联盟主键获得联盟的比赛列表
        /// </summary>
        /// <param name="_UnionSys"></param>
        /// <returns></returns>
        public List<WeTourModel> GetUnionTourList(string _UnionSys)
        {
            List<WeTourModel> list = new List<WeTourModel>();
            string sql = "select * from wtf_CityTour where unionsys='" + _UnionSys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Update tour's status
        /// </summary>
        /// <param name="_Clubsys"></param>
        /// <param name="_Status"></param>
        /// <returns></returns>
        public bool UpdateTourStatus(string _Clubsys, string _Status)
        {
            string sql = "update  wtf_CityTour set status='" + _Status + "' where sysno='" + _Clubsys + "'";//向上递增一个状态
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
        /// 验证当前赛事是否满足修改状态的要求。2016-5-30
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_Status"></param>
        public void ValidateTourStatusChange(string _Toursys, string _Status)
        {
            switch (_Status)
            { 
                case "1":
                    //赛事变为报名装态
                    break;
                case "2":
                    //赛事变为报名完成状态
                    break;
                case"3":
                    //赛事变为签表分配完成的状态
                    break;
                case "4":
                    //赛事变为进行中的状态
                    break;
                case "5":
                    //赛事变为已完成的状态
                    break;


            }
        }

        /// <summary>
        /// Delete a Tour Data
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public bool DeleteTour(string _Toursys)
        {
            string sql = "Delete wtf_CityTour where sysno='" + _Toursys + "'";
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
        /// 修改tourSign中的小组
        /// liutao,2015-10-22
        /// </summary>
        /// <param name="_Toursys"></param>
        public void UpdateSignGroups(string _Toursys)
        {
            List<WeTourContModel> list = WeTourContentDll.instance.GetContentlist(_Toursys);
            if (list.Count > 0)
            {
                foreach (WeTourContModel model in list)
                {
                    string sql = "select * from wtf_toursign where contentid='"+model.id+"' and round='0'";
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string upsql = "update wtf_toursign set GroupId='"+dr["signorder"].ToString()+"' where id='"+dr["id"].ToString()+"'";
                            int a = DbHelperSQL.ExecuteSql(upsql);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 分配赛事资源
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_CourtN"></param>
        /// <param name="_CourtS"></param>
        /// <param name="_Matchdate"></param>
        public void updateTourResource(string _Toursys, string _CourtN, string _CourtS, string _Matchdate)
        {
            string sql = string.Format("update wtf_CityTour set tourcourt='{0}',Courtsys='{1}',StartDate='{2}' where sysno='{3}'",_CourtN,_CourtS,_Matchdate,_Toursys);
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        #region UpdateTour Match Logic 
        /// <summary>
        /// Update tour logic
        /// </summary>
        /// <param name="_Toursys"></param>
        public void UpdateTourLogic(string _Toursys)
        {
            List<WeTourContModel> list = WeTourContentDll.instance.GetContentlist(_Toursys);
            if (list.Count > 0)
            {
                foreach (WeTourContModel model in list)
                {
                    UpdateWinto(model.id);
                }
            }
        }

        public void UpdateWinto(string _ContentId)
        {
            //get round
            string sql = "select distinct(round) from wtf_match where  contentid='" + _ContentId + "' and round>0";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string sql2 = "select matchorder from wtf_match where  contentid='" + _ContentId + "' and Round='"+dr[0].ToString()+"' order by matchorder";
                    DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];

                    int _RoundBefore = Convert.ToInt32(dr[0].ToString()) - 1;
                    if (_RoundBefore == 0)
                    {
                        //update group match
                        #region GroupMatch
                        string sql3 = "select distinct(convert(int,etc1)) from wtf_match where contentid='180' and round=0 order by convert(int,etc1)";
                        DataTable dt3 = DbHelperSQL.Query(sql3).Tables[0];
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            try
                            {
                                WeMatchDll.instance.UpdateGroupMatchWinto(_ContentId, dt3.Rows[2 * i][0].ToString(), dt2.Rows[i][0].ToString(), "1");
                                WeMatchDll.instance.UpdateGroupMatchWinto(_ContentId, dt3.Rows[2 * i + 1][0].ToString(), dt2.Rows[i][0].ToString(), "2");
                            }
                            catch
                            { }
                        }
                        #endregion

                    }
                    else
                    {
                        #region knockout
                        
                        List<WeMatchModel> list=WeMatchDll.instance.GetMatchlistbyContRound(_ContentId,_RoundBefore.ToString());
                        if (list.Count > 0)
                        {
                            //update knock out
                            if (dt2.Rows.Count > 0)
                            {
                                try
                                {
                                    if (dt2.Rows.Count == 1)
                                    {
                                        //target match is the final                                
                                        WeMatchDll.instance.UpdateMatchWinto(list[0].SYS, dt2.Rows[0][0].ToString(), "1");
                                        WeMatchDll.instance.UpdateMatchWinto(list[1].SYS, dt2.Rows[0][0].ToString(), "2");
                                    }
                                    else
                                    {
                                        for (int i = 0; i < dt2.Rows.Count; i++)
                                        {
                                            WeMatchDll.instance.UpdateMatchWinto(list[2 * i].SYS, dt2.Rows[i][0].ToString(), "1");
                                            WeMatchDll.instance.UpdateMatchWinto(list[2 * i + 1].SYS, dt2.Rows[i][0].ToString(), "2");
                                        }
                                    }
                                }
                                catch
                                { }
                            }
                        }
                        #endregion
                    }
                }
            }
        }               
        #endregion

        #region TourMatches
        /// <summary>
        /// Add tour matches by tour sysno, initiate matches
        /// </summary>
        /// <param name="_TourSys"></param>
        public void AddTourMatches(string _TourSys) 
        {
            
            List<WeTourContModel> list = WeTourContentDll.instance.GetContentlist(_TourSys);
            if (list.Count > 0)
            {
                foreach (WeTourContModel model in list)
                {
                    AddTourMatchbyContent(model.id);
                }
            }
        }

        public void AddTourMatchbyContent(string _Contentid)
        {
            //delete pre tour match
            string sql1 = "delete wtf_match where contentid='" + _Contentid + "'";
            int a = DbHelperSQL.ExecuteSql(sql1);

            //Add Group Matches
            string sql = "select distinct(signorder) from wtf_toursign where contentid='" + _Contentid + "' and round=0";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //Add Group match by match order
                    AddGroupMatches(_Contentid, dt.Rows[i][0].ToString());
                }
            }

            //add Knockout Matches
            AddKnockoutMatches(_Contentid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_contentid"></param>
        private void AddKnockoutMatches(string _Contentid)
        {
            WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_Contentid);
            string _Toursys = model.Toursys;
            int SignQty = Convert.ToInt32(model.SignQty);
            if (SignQty > 1)
            {
                //add match
                int _GroupMaxRound = Convert.ToInt32(Math.Log(SignQty,2));
                //按轮次添加比赛
                for (int i = 0; i < _GroupMaxRound; i++)
                {
                    if (i == 0)
                    {
                        //计算首轮签数
                        int RoundM = SignQty / ((i + 1) * 2);

                        for (int j = 0; j < RoundM; j++)
                        {
                            string Player1 = WeTourSignDll.instance.GetSignPlayer(_Contentid,(j*2+1).ToString());
                            string Player2 = WeTourSignDll.instance.GetSignPlayer(_Contentid, (j*2 + 2).ToString());
                            AddTourMatch(Player1, Player2, _Toursys, _Contentid, (i + 1).ToString(), "", "");
                        }
                    }
                    else
                    {
                        //获取本轮比赛数量
                        int RoundM = SignQty / ((i + 1) * 2);
                        for (int j = 0; j < RoundM; j++)
                        {
                            AddTourMatch("", "", _Toursys, _Contentid, (i + 1).ToString(), "", "");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Content"></param>
        /// <param name="_SignOrder"></param>
        private void AddGroupMatches(string _ContentId, string _SignOrder)
        {
            WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_ContentId);
            string TourSys = model.Toursys;
            string sql = "select * from wtf_TourSign where ContentId='" + _ContentId + "' and signorder='" + _SignOrder + "' order by signorder";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                switch (dt.Rows.Count)
                { 
                    case 2:
                        AddTourMatch(dt.Rows[0]["membersys"].ToString(), dt.Rows[1]["membersys"].ToString(), model.Toursys, _ContentId, "0", _SignOrder, "1");
                        break;
                    case 3:                       
                        AddTourMatch(dt.Rows[0]["membersys"].ToString(), dt.Rows[2]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "1");
                        AddTourMatch(dt.Rows[1]["membersys"].ToString(), dt.Rows[2]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "2");
                        AddTourMatch(dt.Rows[0]["membersys"].ToString(), dt.Rows[1]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "3");
                        break;
                    case 4:
                        AddTourMatch(dt.Rows[0]["membersys"].ToString(), dt.Rows[1]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "3");
                        AddTourMatch(dt.Rows[2]["membersys"].ToString(), dt.Rows[3]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "3");

                        AddTourMatch(dt.Rows[0]["membersys"].ToString(), dt.Rows[2]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "2");
                        AddTourMatch(dt.Rows[1]["membersys"].ToString(), dt.Rows[3]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "2");

                        AddTourMatch(dt.Rows[0]["membersys"].ToString(), dt.Rows[3]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "1");
                        AddTourMatch(dt.Rows[1]["membersys"].ToString(), dt.Rows[2]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "1");
                        break;

                    case 5:
                        AddTourMatch(dt.Rows[0]["membersys"].ToString(), dt.Rows[4]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "1");
                        AddTourMatch(dt.Rows[2]["membersys"].ToString(), dt.Rows[3]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "1");

                        AddTourMatch(dt.Rows[1]["membersys"].ToString(), dt.Rows[4]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "2");
                        AddTourMatch(dt.Rows[0]["membersys"].ToString(), dt.Rows[3]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "2");

                        AddTourMatch(dt.Rows[2]["membersys"].ToString(), dt.Rows[4]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "3");
                        AddTourMatch(dt.Rows[1]["membersys"].ToString(), dt.Rows[3]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "3");

                        AddTourMatch(dt.Rows[0]["membersys"].ToString(), dt.Rows[2]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "4");
                        AddTourMatch(dt.Rows[4]["membersys"].ToString(), dt.Rows[3]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "4");

                        AddTourMatch(dt.Rows[0]["membersys"].ToString(), dt.Rows[1]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "5");
                        AddTourMatch(dt.Rows[1]["membersys"].ToString(), dt.Rows[2]["membersys"].ToString(), TourSys, _ContentId, "0", _SignOrder, "5");
                        break;
                }
            }
        }


        /// <summary>
        /// 根据条件添加比赛
        /// </summary>
        /// <param name="_P1"></param>
        /// <param name="_P2"></param>
        /// <param name="_TourSys"></param>
        /// <param name="_ContentID"></param>
        /// <param name="_Round"></param>
        private void AddTourMatch(string _P1, string _P2, string _TourSys, string _ContentID, string _Round, string _GroupId, string _GroupRound)
        {
            WeTourModel tmodel = WeTourDll.instance.GetModelbySys(_TourSys);
            WeMatchModel mmodel = new WeMatchModel();
            mmodel.PLAYER1 = _P1;
            mmodel.PLAYER2 = _P2;
            switch (tmodel.SETTYPE)
            {
                case "一盘":
                    mmodel.MATCHTYPE = 0;//盘数，0表示1盘制比赛，1表示3盘制比赛，2表示5盘制比赛
                    break;
                case "三盘":
                    mmodel.MATCHTYPE = 1;//盘数，0表示1盘制比赛，1表示3盘制比赛，2表示5盘制比赛
                    break;
                case "五盘":
                    mmodel.MATCHTYPE = 2;//盘数，0表示1盘制比赛，1表示3盘制比赛，2表示5盘制比赛
                    break;
            }
            mmodel.GRADETYPE = 1;
            if (tmodel.GAMETYPE == "金球决胜制")
            {
                mmodel.ISDECIDE = 1;
            }
            else
            {
                mmodel.ISDECIDE = 0;
            }
            mmodel.STATE = 0;
            mmodel.TOURSYS = tmodel.SYSNO;
            mmodel.ROUND = Convert.ToInt32(_Round);
            mmodel.ContentID = _ContentID;
            mmodel.matchorder = GetNewMatchOrder(_ContentID);//为比赛添加顺序号
            mmodel.etc1 = _GroupId;//添加小组编号
            mmodel.etc2 = _GroupRound;//添加小组赛序号
            //未添加比赛时间和地点，在完成创建比赛以后统一来修改   
            WeMatchDll.instance.Insert(mmodel);
        }

        /// <summary>
        /// 获得比赛顺序号
        /// </summary>
        /// <param name="_Contentid"></param>
        /// <returns></returns>
        private string GetNewMatchOrder(string _Contentid)
        {
            string sql = "select * from wtf_match where contentid='" + _Contentid + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return (dt.Rows.Count + 1).ToString();
        }
        #endregion 


        /// <summary>
        /// 显示轮数汉字
        /// </summary>
        /// <param name="_Round"></param>
        /// <param name="_Capacity"></param>
        /// <returns></returns>
        public string RenderRound(int _Round, int _Capacity)
        {
            string RoundInfo = "";
            double Total = Math.Log(Convert.ToDouble(_Capacity), 2);
            int _Tot = Convert.ToInt32(Math.Round(Total));
            if (_Round == _Tot&&_Round!=0)
            {
                RoundInfo = "决赛";
            }
            else
            {
                if ((_Round == _Tot - 1) && _Round != 0)
                {
                    RoundInfo = "半决赛";
                }
                else
                {
                    switch (_Round)
                    {
                        case 0:
                            RoundInfo = "小组赛/资格赛";
                            break;
                        case 1:
                            RoundInfo = "第一轮";
                            break;
                        case 2:
                            RoundInfo = "第二轮";
                            break;
                        case 3:
                            RoundInfo = "第三轮";
                            break;
                        case 4:
                            RoundInfo = "第四轮";
                            break;
                        case 5:
                            RoundInfo = "第五轮";
                            break;
                    }
                }
            }

            return RoundInfo;
        }

        #region 赛事资源分配
        /// <summary>
        /// get match qty
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public string GetTourMatchQty(string _Toursys)
        {
            string MatchQty = "";
            //total matches
           int Total=WeMatchDll.instance.CountMatchQtybyTour(_Toursys);
           if (Total > 0)
            {
                MatchQty += "<ul><li>全部比赛-" + Total + "</li>";
                List<WeTourContModel> list = WeTourContentDll.instance.GetContentlist(_Toursys);
                if (list.Count > 0)
                {
                    foreach (WeTourContModel model in list)
                    {
                        int ContToal = WeMatchDll.instance.CountMatchQtybyCont(model.id);
                        MatchQty += "<li>" + model.ContentName + "-" + ContToal + "</li>";
                    }
                }
                //add suggest method
                MatchQty += "<li>" + WeMatchDll.instance.GetSuggestResource(_Toursys)+ "</li></ul>";
            }
            else
            {
                MatchQty = "还未生成比赛";
            }
            return MatchQty;
        }

        /// <summary>
        /// Close Empty matches
        /// </summary>
        /// <param name="_Toursys"></param>
        public void CloaseEmptyMatch(string _Toursys)
        { 
            //get empty matches
            string sql = "select * from wtf_match where toursys='" + _Toursys + "' and (player1='1bf973ea-7d4c-41d1-aa05-8bf68b567d77' or player2='1bf973ea-7d4c-41d1-aa05-8bf68b567d77') and state=0";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                { 
                    //close match
                    string winner = "";
                    if (dr["player1"].ToString() == "1bf973ea-7d4c-41d1-aa05-8bf68b567d77")
                    {
                        winner = dr["player2"].ToString();
                    }
                    else
                    {
                        winner = dr["player1"].ToString();
                    }
                    if (WeMatchDll.instance.UpdatematchState(dr["sys"].ToString(), "2"))
                    { 
                        //update result
                        WeMatchDll.instance.UpdateMatchResult(dr["sys"].ToString(), winner, "", "");
                        //update new match
                        WeMatchModel model = WeMatchDll.instance.GetMatchbyMatchOrder(dr["contentid"].ToString(), dr["winto"].ToString());
                        if (dr["etc3"].ToString() == "1")
                        {
                            WeMatchDll.instance.UpdatePlayer1(model.SYS, winner);
                        }
                        else
                        {
                            WeMatchDll.instance.UpdatePlayer2(model.SYS, winner);
                        }

                    }
                }
            }

        }

        /// <summary>
        /// Distribut Court for Tour Matches
        /// Courtid:the corresponding court
        /// place:the play order of each court
        /// 
        /// </summary>
        /// <param name="_TourSys"></param>
        public void DistributeCourt(string _TourSys)
        { 
            //Get Tour Court Resource
            WeTourModel model = GetModelbySys(_TourSys);            
            string[] CourtsRes;
            if (model.COURTSYS.IndexOf(",") > 0)
            {
                //more than one court
                int courtIndex = 0;
                CourtsRes = model.COURTSYS.Split(',');//将资源通过分割字符串的办法分开

                //distribute group round matches
                int Mground = WeMatchDll.instance.GetGroupMaxRound(_TourSys);
                for (int i = 1; i <= Mground; i++)
                {
                    //Distribute Group match by round 
                    List<WeMatchModel> list = WeMatchDll.instance.GetGroupMatchbyRound(_TourSys, i.ToString());
                    if (list.Count > 0)
                    {
                        foreach (WeMatchModel mmodel in list)
                        {
                            WeMatchDll.instance.UpdateMatch_CourtId(mmodel.SYS, CourtsRes[courtIndex]);//update a match's courtid
                            if (courtIndex < CourtsRes.Length - 1)
                            {
                                courtIndex += 1;//adjust court index
                            }
                            else
                            {
                                courtIndex = 0;
                            }
                        }
                    }
                }

                //distribute knockout matches by round
                int Mkround = WeMatchDll.instance.GetMaxKnockRound(_TourSys);
                for (int i = 1; i <= Mkround; i++)
                {
                    //Distribute Group match by round 
                    List<WeMatchModel> list = WeMatchDll.instance.getKnockOutMatch(_TourSys, i.ToString());
                    if (list.Count > 0)
                    {
                        foreach (WeMatchModel mmodel in list)
                        {
                            WeMatchDll.instance.UpdateMatch_CourtId(mmodel.SYS, CourtsRes[courtIndex]);//update a match's courtid
                            if (courtIndex < CourtsRes.Length - 1)
                            {
                                courtIndex += 1;//adjust court index
                            }
                            else
                            {
                                courtIndex = 0;
                            }
                        }
                    }
                }

            }
            else
            { 
                //there's only one court 
                string sql = "update wtf_match set courtid='" + model.COURTSYS + "' where toursys='" + _TourSys + "' and player1!='1bf973ea-7d4c-41d1-aa05-8bf68b567d77' and player2!='1bf973ea-7d4c-41d1-aa05-8bf68b567d77'";
                int a = DbHelperSQL.ExecuteSql(sql);
            }

            //update the court play order
            DataTable courtDestri = WeMatchDll.instance.GetCourtQty(_TourSys);
            if (courtDestri.Rows.Count > 0)
            {
                for (int i = 0; i < courtDestri.Rows.Count; i++)
                {
                    List<WeMatchModel> list=WeMatchDll.instance.GetMatchbyCourtid(_TourSys,courtDestri.Rows[i][0].ToString());
                    if (list.Count > 0)
                    {
                        for (int j=0;j<list.Count;j++)
                        {
                            WeMatchDll.instance.UpdateMatch_Place(list[j].SYS, (j + 1).ToString());//update match place
                        }
                    }
                }
            }
        }

        #region 2015-10-23,分配场地

        /// <summary>
        /// Distribut Court for Tour Matches
        /// Courtid:the corresponding court,place:the play order of each court
        /// 2015-10-23，修改逻辑，增加约束条件，同一小组的比赛尽量安排在同一场地
        /// 分配逻辑：先根据小组比赛数量，将场地按照比赛项目分开。
        /// </summary>
        /// <param name="_TourSys"></param>
        public void DistributeCourt2(string _TourSys)
        { 
            //Get Tour Court Resource
            WeTourModel model = GetModelbySys(_TourSys);            
            string[] CourtsRes;
            if (model.COURTSYS.IndexOf(",") > 0)
            {
                //将资源通过分割字符串的办法分开
                CourtsRes = model.COURTSYS.Split(',');

                //获得子项小组赛之和,正签数和小组数一致
                //当赛事出现一个小组两个人晋级的时候，这个方法就不行
                //string TotalGroupMatchesSql = "select SUM(signqty) from wtf_TourContent where toursys='"+_TourSys+"'";
                //DataTable dtT = DbHelperSQL.Query(TotalGroupMatchesSql).Tables[0];
                //int TotalGroupMQty = Convert.ToInt32(dtT.Rows[0][0].ToString());
                string totalGroup = "select distinct(GroupId),contentid from wtf_toursign where toursys='"+_TourSys+"' and round='0'";
                DataTable dtT = DbHelperSQL.Query(totalGroup).Tables[0];
                int TotalGroupMQty = dtT.Rows.Count;

                #region 使用子项比赛数量
                string totalMatch = "select * from wtf_match where toursys='" + _TourSys + "' and round='0'";
                int TatalMatchQty = DbHelperSQL.Query(totalMatch).Tables[0].Rows.Count;
                #endregion

                #region 分配小组赛场地
                //获取子项小组赛数量,并分配小组赛场地
                string sql = "select count(distinct(etc1)),contentid from wtf_match where toursys='"+_TourSys+"' and round=0 and state=0 group by ContentID";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    //已分配场地数量
                    int CourtsDistried = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    { 
                        //对应子项分配的场地数量
                        int ContCourtQ=0;
                        string SplitCourts = "";
                        //判断是否是最后一种   
                        if (i == dt.Rows.Count - 1)
                        {
                            ContCourtQ = CourtsRes.Length - CourtsDistried;
                        }
                        else
                        {
                            //计算该子项可分配场地数量（按照子项小组数量和总小组的数量的对比来计算）
                            #region 根据子项小组数量
                            //int contGameQty=Convert.ToInt32(dt.Rows[i][0].ToString());
                            //double q = (Convert.ToDouble(contGameQty) * Convert.ToDouble(CourtsRes.Length)) / Convert.ToDouble(TotalGroupMQty);
                            //ContCourtQ = Convert.ToInt32(Math.Round(q)); 
                            //if (ContCourtQ == 0)
                            //{
                            //    ContCourtQ = 1;
                            //}
                            #endregion

                            #region 根据小组比赛数量
                            //计算子项的比赛数量
                            string contMatchsql = "select * from wtf_match where contentid='" + dt.Rows[i][1].ToString() + "' and round='0'";
                            int contMatchQty = DbHelperSQL.Query(contMatchsql).Tables[0].Rows.Count;
                            double o = (contMatchQty * Convert.ToDouble(CourtsRes.Length)) / TatalMatchQty;
                            ContCourtQ = Convert.ToInt32(Math.Round(o));
                            if (ContCourtQ == 0)
                            {
                                ContCourtQ = 1;
                            }
                            #endregion


                        }
                        
                        //拼凑重新分配的场地
                        for (int j = CourtsDistried; j < CourtsDistried+ContCourtQ; j++)
                        {
                            SplitCourts += CourtsRes[j] + ",";
                        }
                        CourtsDistried += ContCourtQ;  
                        SplitCourts = SplitCourts.TrimEnd(',');
                       //WriteLog("1101", "子项分配的具体场地资源", SplitCourts, dt.Rows[i][1].ToString(), CourtsDistried.ToString());
                        
                        //首先将资源分配到子项，然后再在子项中分配场地（遵循规则一个小组的比赛尽量安排在一个场地）
                        //分配小组赛场地
                        GroupDistributCourtbyCont(dt.Rows[i][1].ToString(), SplitCourts);
                    }
                }
                #endregion

                #region 分配淘汰赛场地
                //淘汰赛阶段场地分配，约束条件：不同轮次的比赛的place(即场序)不能相同，相同轮次的比赛可以有不同的场序。淘汰赛阶段的比赛的特点是比赛一轮比一轮少。因此，按一轮议论来排。如果一轮比赛数量>场地数量，一轮就至少会有两个场序。如果议论比赛数量<场地数量。则比较简单，一轮比赛一片场地即可。
                int Mkround = WeMatchDll.instance.GetMaxKnockRound(_TourSys);
                int CourtQty = 0;//指分配到第几片场地了
                int MaxQty = CourtsRes.Length;//默认最多的场地是所有场地数量
                for (int i = 1; i <= Mkround; i++)
                {
                    //Distribute Group match by round 
                    List<WeMatchModel> list = WeMatchDll.instance.getKnockOutMatch(_TourSys, i.ToString());
                    if (list.Count > 0)
                    {
                        //对比轮次比赛数量和场地数量
                        if (list.Count < CourtsRes.Length)
                        {
                            MaxQty = list.Count;
                        }

                        foreach (WeMatchModel mmodel in list)
                        {
                            if (CourtQty + 1 >= MaxQty)
                            {
                                CourtQty = 0;
                            }

                            AssignMatchToCourt(mmodel.SYS, CourtsRes[CourtQty]);//update a match's courtid

                            CourtQty += 1;
                        }
                    }
                }
                #endregion
            }
        }

        private void WriteLog(string _Enum, string _Description, string _v1, string _v2, string _v3)
        {
            string sql = string.Format("insert into sys_visit (ContenEnum,Description,value1,value2,value3,visitDate,ipaddress,browser) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", _Enum, _Description, _v1, _v2, _v3, DateTime.Now.ToString(), "", "");
            int a = DbHelperSQL.ExecuteSql(sql); 
        }

        /// <summary>
        /// 按照比赛项目分配场地资源
        /// 考虑1个小组的比赛尽量在一片场地。
        /// 考虑当小组数量和场地数量不一致的时候应该怎么办，2015-12-20
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <param name="_Courts"></param>
        private void GroupDistributCourtbyCont(string _ContentId, string _Courts)
        {
            string sql = "select distinct(convert(int,etc1)) from wtf_match where contentid='" + _ContentId + "' and state=0 and round=0 ";//按照小组来排列
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            
            if (dt.Rows.Count > 0)
            {
                string[] courts = _Courts.Split(',');
                double q = Convert.ToDouble(dt.Rows.Count) / Convert.ToDouble(courts.Length);
                int a = Convert.ToInt32(Math.Round(q));
                
                int k=0; 
                for (int i = 0; i < courts.Length; i++)
                { 
                    //拼凑小组和场地
                    string Groups = "";
                    
                    try
                    {
                        for (int j = 0; j < a; j++)
                        {
                           
                            Groups += dt.Rows[k][0].ToString() + ",";
                            
                            k += 1;
                        }
                        if (Groups.IndexOf(",") > 0)
                        {
                            Groups = Groups.TrimEnd(',');
                        }
                        GroupDistributCourtbyContGroup(_ContentId, courts[i], Groups);
                    }
                    catch(Exception e) {
                        
                    }                   
                   
                    
                }
            }
        }

        private void GroupDistributCourtbyContGroup(string _ContentId, string _Courts,string _Groups)
        {
            string sql = "select * from wtf_match where contentid='" + _ContentId + "' and state=0 and round=0 and etc1 in ("+_Groups+") order by convert(int,etc2)";//按照小组轮次来排列
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
              
                foreach (DataRow dr in dt.Rows)
                {                   
                    //给比赛分配场地
                    AssignMatchToCourt(dr["sys"].ToString(), _Courts);
                }
            }
        }

        /// <summary>
        /// 指定比赛场地
        /// </summary>
        /// <param name="_MatchSys"></param>
        /// <param name="_Court"></param>
        public void AssignMatchToCourt(string _MatchSys, string _Court)
        { 
            //获取场地已分配数量
            WeMatchModel model = WeMatchDll.instance.GetModel(_MatchSys);
            int MatQ = GetCourtMatchQty(model.TOURSYS, _Court);
            string sql = "update wtf_match set courtId='" + _Court + "',place='" + (MatQ+1)+ "' where sys='" + _MatchSys + "'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        public void UpdateMatchCourtInfo(string _MatchSys, string _Court, string _Place)
        {
            string sql = "update wtf_match set courtId='" + _Court + "',place='"+_Place+"' where sys='" + _MatchSys + "'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// 获得指定场地比赛数量
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_CourtId"></param>
        /// <returns></returns>
        private int GetCourtMatchQty(string _Toursys,string _CourtId)
        {
            string sql = "select * from wtf_match where toursys='" + _Toursys + "' and courtId='" + _CourtId + "'";
            return DbHelperSQL.Query(sql).Tables[0].Rows.Count;
        }

        #endregion
        /// <summary>
        /// distribute Date
        /// </summary>
        /// <param name="_Toursys"></param>
        public void DistributeDate(string _Toursys)
        {
            WeTourModel model = GetModelbySys(_Toursys);
            if (model.STARTDATE.IndexOf("#") > 0)
            {
                //more than one day
                string[] matchdate = model.STARTDATE.Split('#');
                List<WeTourContModel> list = WeTourContentDll.instance.GetContentlist(_Toursys);
                int MatchQty = WeMatchDll.instance.CountMatchQtybyTour(_Toursys);
                if (list.Count == MatchQty)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        string sql = "update wtf_match set matchdate='" + matchdate[i] + "' where contentid='" + list[i].id + "'";
                        int a = DbHelperSQL.ExecuteSql(sql);
                    }
                }
            }
            else
            { 
                //only one day
                string sql = "update wtf_match set matchdate='"+model.STARTDATE+"' where toursys='"+_Toursys+"'";
                int a = DbHelperSQL.ExecuteSql(sql);
            }
        }
        #endregion

        #region UmpireMatches
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Tourys"></param>
        /// <returns></returns>
        public List<WeTourModel> GetUmpireTours(string _Tourys)
        {
            List<WeTourModel> list = new List<WeTourModel>();
            string sql = "select a.id,a.ContentName,b.Name,b.TourImg from wtf_TourContent a left join wtf_CityTour b on a.TourSys=b.sysno where b.Status=4 and b.sysno='" + _Tourys + "' order by a.id desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    WeTourModel model = new WeTourModel();
                    model.ID = dr["id"].ToString();
                    model.NAME = dr["Name"].ToString();
                    model.MATCHCONTENT = dr["ContentName"].ToString();
                    model.TOURIMG = dr["TourImg"].ToString();

                    //获得比赛数量
                    model.EXT1 = GetStateMatchbyCont(dr["id"].ToString(), "0").ToString();//未完成的比赛数量

                    model.EXT2 = GetStateMatchbyCont(dr["id"].ToString(), "2").ToString();//完成的比赛数量
                    if (model.EXT1 != "0")
                    {
                        list.Add(model);
                    }
                }
            }
            return list;
        }

            /// <summary>
        /// 根据状态，查看contentid对应的比赛数量
        ///
        /// </summary>
        /// <param name="_Contentid"></param>
        /// <param name="_State"></param>
        /// <returns></returns>
        public int GetStateMatchbyCont(string _Contentid,string _State)
        {
            WeTourContModel tmodel = WeTourContentDll.instance.GetModelbyId(_Contentid);            
            string sql = "";
            if (tmodel.ContentType == "团体")
            {
                //是团体赛
                sql = "select * from wtf_groupmatch where Contentid='" + _Contentid + "' and status";
            }
            else
            { 
                //非团体赛
                sql = "select * from wtf_match where ContentID='" + _Contentid + "' and state";
            }

            if (_State == "2")
            {
                //查询完成
                sql += "=2";
            }
            else
            { 
                //查询未完成
                sql += "!=2";
            }

            return DbHelperSQL.Query(sql).Tables[0].Rows.Count;
        }

        /// <summary>
        /// update match's game qty by toursys
        /// </summary>
        /// <param name="_Toursys"></param>
        public void UpdateGameQty(string _Toursys)
        {
            WeTourModel model = WeTourDll.instance.GetModelbySys(_Toursys);
            string sql = "update wtf_match set gameqty='"+model.EXT2+"' where toursys='"+_Toursys+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }


           
        #endregion

        #region 未支付通知
        public void UpdateTourUnpaidInform(string _Toursys)
        {
            string sql = "update wtf_CityTour set ext3='" + DateTime.Now.ToString() + "' where sysno='" + _Toursys + "'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        public bool IsUnpaidSent(string _Toursys)
        {
            string sql = "select * from wtf_CityTour where sysno='"+_Toursys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string SendTime = dt.Rows[0]["ext3"].ToString();
                if (SendTime != "")
                {
                    DateTime sentT = Convert.ToDateTime(SendTime);
                    if (sentT.AddDays(1) > DateTime.Now)
                    {
                        //上次发送已达24小时
                        return true;
                    }
                    else
                    {
                        //距上次发送未达24小时
                        return false;
                    }
                }
                else
                {
                    //未发送
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 赛事紧急通知
        /// <summary>
        /// 依据赛事主键，判断紧急通知短信是否可发送
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public bool IsEmgMsgSent(string _Toursys)
        {
            bool AllowSend = false;
            WeTourModel model = GetModelbySys(_Toursys);
            if (string.IsNullOrEmpty(model.EXT4))
            { 
                //为空，代表未发送
                string DateN = DateTime.Now.ToString("yyyy-MM-dd");
                if (DateN == model.STARTDATE)
                { 
                    //在比赛当天，可发送比赛紧急短信
                    AllowSend= true;
                }                
            }
            return AllowSend;
        }

        /// <summary>
        /// 批量发送短信
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <param name="_Emeg"></param>
        public void SendTourEmgSmg(string _TourSys, string _Emeg,string _Status)
        {
            string Result = "";
            if (_Status == "1")
            {
                //获得报名人员
                List<WeTourApplyModel> list = WeTourApplyDll.instance.GetApplyListbyTour(_TourSys);
                if (list.Count > 0)
                {
                    foreach (WeTourApplyModel model in list)
                    {
                        try
                        {
                            WeMemberModel mem = WeMemberDll.instance.GetModel(model.MEMBERID);
                            SMS.SMSdll.instance.BatchSendSMS(mem.TELEPHONE, _Emeg);

                        }
                        catch
                        {

                        }
                    }
                    //
                    Result = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            else
            { 
                //
                Result = "正常开始";
            }

            //修改赛事状态
            string sql = "update wtf_CityTour set ext4='"+Result+"' where sysno='"+_TourSys+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        #endregion

        #region 赛事状态回滚
        public string TourStatusRollBack(string _Toursys)
        {
            string _Res = "";
            WeTourModel model = GetModelbySys(_Toursys);
            string sql = "";

            switch (model.STATUS)
            {
                case "1":
                    //从正在报名，变回赛事筹备。删除已报名的信息。如果有已支付的，则还涉及到报名费的退款。tourapply
                    sql = "delete wtf_tourapply where toursys='" + _Toursys + "'";
                    break;
                case "2":
                    //从报名完成，变回正在报名，删除已分配的签表。toursign
                    sql = "delete wtf_toursign where toursys='" + _Toursys + "'";
                    break;
                case "3":
                    //从资源分配变回分配签表，删除已生成的比赛，match
                    sql = "delete wtf_match where toursys='" + _Toursys + "'";
                    break;
                case "4":
                    //从正在比赛变回资源分配，删除签到信息。signin
                    sql = "delete wtf_toursignin where toursys='" + _Toursys + "'";
                    break;
                case "5":
                    //从已完成，变成正在比赛，删除已添加的积分，rank_points,rank_rank
                    sql = "delete rank_points where toursys='" + _Toursys + "'";
                    //更新排名
                    //……
                    break;
            }
            if (sql != "")
            {
                int a = DbHelperSQL.ExecuteSql(sql);
            }
            //更新新的状态
            int _Status = Convert.ToInt32(model.STATUS) - 1;
            UpdateTourStatus(_Toursys, _Status.ToString());
            _Res = "已更新状态";

            return _Res;
        }
        #endregion

    }
}
