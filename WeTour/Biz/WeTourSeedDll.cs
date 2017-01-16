using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Member;

namespace WeTour
{
    public class WeTourSeedDll
    {
        public static WeTourSeedDll instance = new WeTourSeedDll();

        #region Seed基本操作
        /// <summary>
        /// Add a new tour seed 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(WeTourSeedModel model)
        {
            string sql = string.Format("insert into wtf_tourseed (contentid,membersys,seed) values ('{0}','{1}','{2}')",model.CONTENTID,model.MEMBERSYS,model.SEED);
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
        /// 删除种子
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public bool DeleteSeed(string _id)
        {
            string sql = "Delete wtf_tourseed where id='"+_id+"'";
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
        /// 修改种子顺序
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_Order"></param>
        /// <returns></returns>
        public bool UpdateSeedOrder(string _id, string _Order)
        {
            string sql = "Update wtf_tourseed set seed='"+_Order+"' where id='" + _id + "'";
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
        /// Initiate a TourContetn Seed
        /// </summary>
        /// <param name="_ContentId"></param>
        public void InitiateTourSeed(string _ContentId)
        {
            WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_ContentId);
            int SignQty = Convert.ToInt32(model.SignQty);
            int SeedQty = SignQty / 4;
            if (SeedQty > 1)
            {
                for (int i = 0; i < SeedQty; i++)
                {
                    WeTourSeedModel smodel = new WeTourSeedModel();
                    smodel.CONTENTID = _ContentId;
                    smodel.SEED = (i + 1).ToString();
                    smodel.MEMBERSYS = GetApplySeed(_ContentId, i + 1);//get coresponding seed
                    InsertNew(smodel);
                }
            }
        }

        /// <summary>
        /// Get seed player from Apply list
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <param name="Order"></param>
        /// <returns></returns>
        public string GetApplySeed(string _ContentId, int Order)
        {
            string Memsys = "";
            WeTourContModel wmodel = WeTourContentDll.instance.GetModelbyId(_ContentId);
            if (wmodel.ContentType.IndexOf("双") > 0)
            {
                //get double ranking
                
            }
            else
            { 
                //get single ranking
                string sql = "select * from rank_rank where RankType='' and Status=1 and IsSingle='单打' and Memsys in (select memberid from wtf_TourApply where ContentId='" + _ContentId + "' order by Convert(int,rank))";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > Order)
                { 
                    //applied person has a rank
                    Memsys = dt.Rows[Order - 1]["Memsys"].ToString();
                }
            }
            return Memsys;
        }

        /// <summary>
        /// initiate tour content seed
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public List<WeTourSeedModel> GetContentSeed(string _ContentId)
        {
            List<WeTourSeedModel> list = new List<WeTourSeedModel>();
            WeTourContModel cmodel = WeTourContentDll.instance.GetModelbyId(_ContentId);

            string sql = "select * from wtf_tourseed where contentid='"+_ContentId+"' order by id";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourSeedModel>>(dt);

                //render player
                foreach (WeTourSeedModel model in list)
                {
                    if (model.MEMBERSYS != "")
                    {
                        if (model.MEMBERSYS.IndexOf(",") > 0)
                        {
                            //double
                            WeMemberModel mem1 = WeMemberDll.instance.GetModel(model.MEMBERSYS.Split(',')[0]);
                            WeMemberModel mem2 = WeMemberDll.instance.GetModel(model.MEMBERSYS.Split(',')[1]);
                            model.P1LNAME = mem1.USERNAME;
                            model.P1LIMGURL = mem1.EXT1;
                            model.P2LNAME = mem2.USERNAME;
                            model.P2LIMGURL = mem2.EXT1;
                        }
                        else
                        { 
                            //single
                            WeMemberModel mem = WeMemberDll.instance.GetModel(model.MEMBERSYS);
                            model.P1LNAME = mem.USERNAME;
                            model.P1LIMGURL = mem.EXT1;
                        }
                    }
                    else
                    {
                        model.P1LNAME = "未设定";
                        model.P1LIMGURL = "";
                    }
                }
            }            
            return list;
        }
        #endregion

        #region 生成种子
        /// <summary>
        /// 根据id生成初始化种子
        /// 
        /// </summary>
        /// <param name="_ContentId"></param>
        public void GenerateSeedbyCont(string _ContentId)
        {
            WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(_ContentId);
            //计算种子数量
            int SeedQty = Convert.ToInt32(cont.SignQty) / 4;
            if (SeedQty > 0)
            { 
                
            }
        }
        #endregion

        #region 获取种子与非种子报名人员

        /// <summary>
        /// 获取非种子报名人员
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public List<WeTourApplyModel> GetUnseedApplicant(string _ContentId)
        {
            List<WeTourApplyModel> list = new List<WeTourApplyModel>();
            WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_ContentId);
            string sql = "";
            if (model.ContentType.IndexOf("双") > 0)
            {
                sql = string.Format("select * from wtf_tourapply where contentid='{0}' and memberid+','+paterner not in (select Membersys from wtf_tourseed where Contentid='{0}')",_ContentId);
            }
            else
            {
                sql = string.Format("select * from wtf_tourapply where contentid='{0}' and memberid not in (select Membersys from wtf_tourseed where Contentid='{0}')",_ContentId);
            }
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourApplyModel>>(dt);
                foreach(WeTourApplyModel app in list)
                {
                    if (app.memtype == "group")
                    {
                        //加载团体名称
                        weTeamModel team = weTeamdll.instance.GetModel(app.MEMBERID);
                        app.MemberName = team.TEAMNAME;
                    }
                    else
                    {
                        if (model.ContentType.IndexOf("双") > 0)
                        {
                            //添加搭档
                            WeMemberModel mem = WeMemberDll.instance.GetModel(app.PATERNER);
                            app.ParName = mem.USERNAME;
                            app.ParImg = mem.EXT1;
                        }
                        else
                        {
                            //添加姓名
                            WeMemberModel mem = WeMemberDll.instance.GetModel(app.MEMBERID);
                            app.MemberName = mem.USERNAME;
                            app.MemberImg = mem.EXT1;
                        }
                    }
                }

            }
            return list;
        }

        /// <summary>
        /// 获取赛事种子报名者信息
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public List<WeTourApplyModel> GetTourSeedApp(string _ContentId)
        {
            List<WeTourApplyModel> list = new List<WeTourApplyModel>();
            WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_ContentId);
            string sql = "";
            if (model.ContentType.IndexOf("双") > 0)
            {
                sql = string.Format("select * from wtf_tourapply where contentid='{0}' and memberid+','+paterner in (select Membersys from wtf_tourseed where Contentid='{0}')", _ContentId);
            }
            else
            {
                sql = string.Format("select * from wtf_tourapply where contentid='{0}' and memberid in (select Membersys from wtf_tourseed where Contentid='{0}')", _ContentId);
            }
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourApplyModel>>(dt);
                foreach (WeTourApplyModel app in list)
                {
                    if (app.memtype == "group")
                    {
                        //加载团体名称
                        weTeamModel team = weTeamdll.instance.GetModel(app.MEMBERID);
                        app.MemberName = team.TEAMNAME;
                    }
                    else
                    {
                        if (model.ContentType.IndexOf("双") > 0)
                        {
                            //添加搭档
                            WeMemberModel mem = WeMemberDll.instance.GetModel(app.PATERNER);
                            app.ParName = mem.USERNAME;
                            app.ParImg = mem.EXT1;
                        }
                        else
                        {
                            //添加姓名
                            WeMemberModel mem = WeMemberDll.instance.GetModel(app.MEMBERID);
                            app.MemberName = mem.USERNAME;
                            app.MemberImg = mem.EXT1;
                        }
                    }
                }

            }
            return list;

        }
        #endregion

        #region 批量添加种子
        /// <summary>
        /// 批量添加种子
        /// </summary>
        /// <param name="list"></param>
        /// <param name="_Id"></param>
        public void BatchAddSeed(List<WeTourApplyModel> list, string _Id)
        { 
            //首先删除之前的种子
            CleanContentSeed(_Id);

            WeTourContModel cont=WeTourContentDll.instance.GetModelbyId(_Id);
            //
            for(int i=0;i<list.Count;i++)
            {
                WeTourSeedModel seed = new WeTourSeedModel();
                seed.CONTENTID = _Id;
                if (cont.ContentType.IndexOf("双") > 0)
                {
                    //双打
                    seed.MEMBERSYS = list[i].MEMBERID + "," + list[i].PATERNER;
                }
                else
                { 
                    //单打
                    seed.MEMBERSYS = list[i].MEMBERID;
                }
                seed.SEED = (i + 1).ToString();

                InsertNew(seed);
            }
        }

        /// <summary>
        /// 清空种子
        /// </summary>
        /// <param name="_id"></param>
        private void CleanContentSeed(string _id)
        {
            string sql = "delete wtf_tourseed where contentid='"+_id+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }
        #endregion
    }
}
