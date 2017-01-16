using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Member;
using Gym;
using Club;

namespace WeTour
{
    public class WeTourSignDll
    {
        public static WeTourSignDll instance = new WeTourSignDll();

        /// <summary>
        /// Insert New Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(WeTourSignModel model)
        {
            string sql = string.Format("insert into wtf_TourSign (signorder,membersys,Toursys,ContentId,Round,GroupId) values('{0}','{1}','{2}','{3}','{4}','{5}')",model.SIGNORDER,model.MEMBERSYS,model.TOURSYS,model.CONTENTID,model.ROUND,model.GROUPID);
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
        /// 修改签表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSignPlayer(WeTourSignModel model)
        {
            string sql = string.Format("update wtf_TourSign set signorder='{0}',membersys='{1}',Round='{2}',GroupId='{3}' where id='{4}'",model.SIGNORDER,model.MEMBERSYS,model.ROUND,model.GROUPID,model.id);
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
        /// Get Tour Sign Model
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public WeTourSignModel GetModel(string _id)
        {
            WeTourSignModel model = new WeTourSignModel();
            string sql = "select * from wtf_TourSign where id='"+_id+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<WeTourSignModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// Insert tour sign by tour sign qty
        /// </summary>
        /// <param name="_Contentid"></param>
        public void InitiateTourSign(string _Contentid)
        {
            WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_Contentid);
            int Qty = Convert.ToInt32(model.SignQty);
            for (int i = 0; i < Qty; i++)
            {
                WeTourSignModel nmoel = new WeTourSignModel();
                nmoel.SIGNORDER = (i + 1).ToString();
                nmoel.MEMBERSYS = "1bf973ea-7d4c-41d1-aa05-8bf68b567d77";//轮空
                nmoel.TOURSYS = model.Toursys;
                nmoel.CONTENTID = _Contentid;
                nmoel.ROUND = "1";

                InsertNew(nmoel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_contentId"></param>
        /// <returns></returns>
        public bool IsTourSignAssigned(string _contentId)
        {
            string sql = "select * from wtf_toursign where contentid='"+_contentId+"'";
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
        /// Get Sign by contentid
        /// </summary>
        /// <param name="_ContId"></param>
        /// <returns></returns>
        public List<WeTourSignModel> GetTourSignModelList(string _ContId)
        {
            List<WeTourSignModel> list = new List<WeTourSignModel>();
            WeTourContModel cmodel = WeTourContentDll.instance.GetModelbyId(_ContId);
            int SignQty = Convert.ToInt32(cmodel.SignQty);
           
            if (SignQty > 0)
            {
                for (int i = 0; i < SignQty; i++)
                {
                    int SignOrder = i + 1;
                    WeTourSignModel model = new WeTourSignModel();
                    model.SIGNORDER = SignOrder.ToString();
                    model.SEED = "";

                    string sql = "select * from wtf_toursign where contentid='" + _ContId + "' and  SignOrder ='" + SignOrder+ "'";
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count ==1)
                    {
                        
                        #region Round1
                        try
                        {
                            if (dt.Rows[0]["membersys"].ToString().IndexOf(",") > 0)
                            {
                                //double player
                                string[] players = dt.Rows[0]["membersys"].ToString().Split(',');
                                WeMemberModel mmodel = WeMemberDll.instance.GetModel(players[0]);
                                model.PLAYER1NAME = mmodel.USERNAME;
                                if (mmodel.EXT1 != "")
                                {
                                    model.PLAYER1IMG = mmodel.EXT1;
                                }
                                else
                                {
                                    model.PLAYER1IMG = "/images/touxiang.jpg";
                                }

                                WeMemberModel mmodel1 = WeMemberDll.instance.GetModel(players[1]);
                                model.PLAYER2NAME = mmodel1.USERNAME;
                                if (mmodel1.EXT1 != "")
                                {
                                    model.PLAYER2IMG = mmodel1.EXT1;
                                }
                                else
                                {
                                    model.PLAYER2IMG = "/images/touxiang.jpg";
                                }
                            }
                            else
                            {
                                //single player
                                WeMemberModel mmodel = WeMemberDll.instance.GetModel(dt.Rows[0]["membersys"].ToString());
                                model.PLAYER1NAME = mmodel.USERNAME;
                                if (mmodel.EXT1 != "")
                                {
                                    model.PLAYER1IMG = mmodel.EXT1;
                                }
                                else
                                {
                                    model.PLAYER1IMG = "/images/touxiang.jpg";
                                }
                            }
                        }
                        catch { }
                        #endregion 
                    }
                    else if (dt.Rows.Count > 1)
                    {
                        #region GroupMatch
                        model.PLAYER1NAME = dt.Rows.Count.ToString()+"位小组成员";
                        model.PLAYER1IMG = "/ico/Sign.png";
                        #endregion
                    }
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// get sign player by contentid and signorder
        /// </summary>
        /// <param name="_ContId"></param>
        /// <param name="_SignOrder"></param>
        /// <returns></returns>
        public List<WeTourSignModel> GetTourSignPlayer(string _ContId,string _SignOrder)
        {
            List<WeTourSignModel> list = new List<WeTourSignModel>();
            WeTourContModel cmodel = WeTourContentDll.instance.GetModelbyId(_ContId);
            string sql = "select * from wtf_toursign where contentid='" + _ContId + "' AND SignOrder='" + _SignOrder + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WeTourSignModel model = new WeTourSignModel();
                    model.SIGNORDER = dt.Rows[i]["SignOrder"].ToString();
                    model.SEED = "";
                    model.id = dt.Rows[i]["id"].ToString();
                    if (dt.Rows[i]["membersys"].ToString().IndexOf(",") > 0)
                    {
                        //double player
                        string[] players = dt.Rows[i]["membersys"].ToString().Split(',');
                        WeMemberModel mmodel = WeMemberDll.instance.GetModel(players[0]);
                        model.PLAYER1NAME = mmodel.USERNAME;
                        if (mmodel.EXT1 != "")
                        {
                            model.PLAYER1IMG = mmodel.EXT1;
                        }
                        else
                        {
                            model.PLAYER1IMG = "/images/touxiang.jpg";
                        }

                        WeMemberModel mmodel1 = WeMemberDll.instance.GetModel(players[1]);
                        model.PLAYER2NAME = mmodel1.USERNAME;
                        if (mmodel1.EXT1 != "")
                        {
                            model.PLAYER2IMG = mmodel1.EXT1;
                        }
                        else
                        {
                            model.PLAYER2IMG = "/images/touxiang.jpg";
                        }
                    }
                    else
                    {
                        //single player
                        WeMemberModel mmodel = WeMemberDll.instance.GetModel(dt.Rows[i]["membersys"].ToString());
                        model.PLAYER1NAME = mmodel.USERNAME;
                        if (mmodel.EXT1 != "")
                        {
                            model.PLAYER1IMG = mmodel.EXT1;
                        }
                        else
                        {
                            model.PLAYER1IMG = "/images/touxiang.jpg";
                        }
                    }
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// get select value for sign people 
        /// </summary>
        /// <param name="_Contentid"></param>
        /// <returns></returns>
        public List<WeTourApplyModel> GetSignAssign(string _Contentid)
        {
            WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_Contentid);
            List<WeTourApplyModel> list = WeTourApplyDll.instance.GetUnSignedApply(_Contentid);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (model.ContentType.IndexOf("双") > 0)
                    {
                        list[i].ApplySysno = list[i].MEMBERID + "," + list[i].PATERNER;
                        list[i].ApplyName = WeMemberDll.instance.GetModel(list[i].MEMBERID).USERNAME + "," + WeMemberDll.instance.GetModel(list[i].PATERNER).USERNAME;
                    }
                    else
                    { 
                        list[i].ApplySysno = list[i].MEMBERID;
                        list[i].ApplyName = WeMemberDll.instance.GetModel(list[i].MEMBERID).USERNAME;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Add a new Sign Person
        /// </summary>
        /// <param name="_Contid"></param>
        /// <param name="_SignOrder"></param>
        /// <param name="_MemSys"></param>
        public void AddSignPerSon(string _Contid, string _SignOrder, string _MemSys)
        {
            WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(_Contid);
            //whether sign has changed
            string sql = "select * from wtf_toursign where contentid='" + _Contid + "' and signorder='" + _SignOrder + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 1)
            {
                //group member more than one sign
                WeTourSignModel model = new WeTourSignModel();
                model.CONTENTID = _Contid;
                model.SIGNORDER = _SignOrder;
                model.GROUPID = _SignOrder;
                model.TOURSYS = cont.Toursys;
                model.ROUND = "0";
                model.MEMBERSYS = _MemSys;
                InsertNew(model);
            }
            else if(dt.Rows.Count == 1)
            { 
                //whether it has changed
                if (dt.Rows[0]["MEMBERSYS"].ToString() == "1bf973ea-7d4c-41d1-aa05-8bf68b567d77")
                {
                    //sign player haven't been changed
                    string sql2 = "update wtf_toursign set MEMBERSYS='" + _MemSys + "' where id='" + dt.Rows[0]["id"].ToString() + "'";
                    int a = DbHelperSQL.ExecuteSql(sql2);
                }
                else
                { 
                    //sign player already changed
                    string sql2 = "update wtf_toursign set Round='0' where id='" + dt.Rows[0]["id"].ToString() + "'";
                    int a = DbHelperSQL.ExecuteSql(sql2);
                    //add new sign
                    WeTourSignModel model = new WeTourSignModel();
                    model.CONTENTID = _Contid;
                    model.SIGNORDER = _SignOrder;
                    model.GROUPID = _SignOrder;
                    model.TOURSYS = cont.Toursys;
                    model.ROUND = "0";
                    model.MEMBERSYS = _MemSys;
                    InsertNew(model);
                }
            }
        }

        /// <summary>
        /// Delete Sign players
        /// </summary>
        /// <param name="_id"></param>
        public void DeleteSignPlayer(string _id)
        {
            WeTourSignModel model = GetModel(_id);
            //see how many players in this sign
            string sql = "select * from wtf_toursign where contentid='"+model.CONTENTID+"' and signorder='"+model.SIGNORDER+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            string sql2="";
            if (dt.Rows.Count > 2)
            {
                //there are more than two person in sign
                sql2 = "delete wtf_toursign where id='" + _id + "'";
            }
            else if (dt.Rows.Count == 2)
            {
                sql2 = "delete wtf_toursign where id='" + _id + "' update wtf_toursign set round='1' where contentid='" + model.CONTENTID + "' and signorder='" + model.SIGNORDER + "'";
            }
            else if (dt.Rows.Count == 1)
            {
                sql2 = "update wtf_toursign set membersys='1bf973ea-7d4c-41d1-aa05-8bf68b567d77' where id='" + _id + "'";
            }

            if (sql2 != "")
            {
                int a = DbHelperSQL.ExecuteSql(sql2);
            }
        }

        /// <summary>
        /// Get sign player
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <param name="_SignOrder"></param>
        /// <returns></returns>
        public string GetSignPlayer(string _ContentId, string _SignOrder)
        {
            string _Playersys = "";
            string sql = "select * from wtf_toursign where contentid='"+_ContentId+"' and signorder='"+_SignOrder+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count == 1)
            {
                _Playersys = dt.Rows[0]["membersys"].ToString();
            }
            return _Playersys;
        }

        /// <summary>
        /// 根据主键获取唯一的参赛者信息
        /// </summary>
        /// <param name="_eventId"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetDistinctPlayer(string _eventId)
        {
            WeTourModel tour = WeTourDll.instance.GetModelbySys(_eventId);
            Dictionary<string, object> players_list = new Dictionary<string, object>();
            //读取toursignt
            string sql = "select distinct(membersys) from wtf_toursign where toursys='"+_eventId+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> player = new Dictionary<string, object>();
                Dictionary<string, string> pname = new Dictionary<string, string>();
                string pn = "";
                if (tour.MATCHCONTENT == "group")
                {
                    pn = weTeamdll.instance.GetModel(dr[0].ToString()).TEAMNAME;
                }
                else
                {
                    if (dr[0].ToString().IndexOf(",") > 0)
                    {
                        //双打
                        WeMemberModel mem1 = WeMemberDll.instance.GetModel(dr[0].ToString().Split(',')[0]);
                        WeMemberModel mem2 = WeMemberDll.instance.GetModel(dr[0].ToString().Split(',')[1]);
                        pn = mem1.USERNAME + "/" + mem2.USERNAME;
                    }
                    else
                    {
                        //单打
                        WeMemberModel mem = WeMemberDll.instance.GetModel(dr[0].ToString());
                        pn = mem.USERNAME;
                    }
                }
                pname.Add("name",pn);
                players_list.Add(dr[0].ToString(), pname);
            }
            return players_list;
        }


        #region 根据contentid随机排布签表

        private void DeleteTourSign(string _contentid)
        {
            string sql = "delete wtf_toursign where contentid='"+_contentid+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// 根据contentid来随机排布签表
        /// 考虑排名，俱乐部，地区，
        /// 优先考虑种子，种子所在的签位，不安排小组赛：2015-12-20
        /// </summary>
        /// <param name="_ContentId"></param>
        public void RandomSign(string _ContentId)
        {
            //清空之前排布的签表
            //DeleteTourSign(_ContentId);

            WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(_ContentId);
            string SeedGroup = "";
            //首先排布种子的签位
            List<WeTourSeedModel> SeedList = WeTourSeedDll.instance.GetContentSeed(_ContentId); 
            //根据种子号获得所在签位
            if (SeedList.Count > 0)
            {
                foreach (WeTourSeedModel smodel in SeedList)
                {
                    string SignOrder = GetSignOrderForSeed(cont.SignQty, smodel.SEED);
                    SeedGroup += SignOrder + ",";
                    if (SignOrder != "")
                    {
                         AddPlayerToSign(smodel.MEMBERSYS, SignOrder, _ContentId);
                    }
                }
                SeedGroup = SeedGroup.TrimEnd(',');
            }

            //根据排名来分配报名的名单
            List<WeTourApplyModel> list = WeTourApplyDll.instance.GetUnSignedApply(_ContentId);
            if (list.Count > 0)
            { 
                    //已分配的种子小组
                    string[] seedG = SeedGroup.Split(',');
                    int Sgqty = seedG.Length;
                    int j = 0;
                    //按照报名顺序，随机排布签表
                    //为报名人员添加积分信息                                   
                    int signQ = Convert.ToInt32(cont.SignQty);//小组数量
                    int GroupQ = Convert.ToInt32(cont.GroupType);
                    if (list.Count > GroupQ * signQ)
                    {
                        GroupQ += 1;
                    }
                    for (int a = 0; a < GroupQ; a++)
                    {
                        for (int i = 0; i < signQ; i++)
                        {
                            //检查是否超过
                            if (j >= list.Count)
                            {
                                break;
                            }

                            //检查小组是否是种子小组
                            bool isSeedg = false;
                            if (seedG.Length > 0)
                            {
                               
                                for (int k = 0; k < seedG.Length; k++)
                                {
                                    if (seedG[k] == (i + 1).ToString())
                                    {
                                        isSeedg = true;
                                        break;
                                    }
                                }                                
                            }
                            if (!isSeedg)
                            {
                                string _Membersys = "";
                                if (cont.ContentType.IndexOf("双") > 0)
                                {
                                    _Membersys = list[j].MEMBERID + "," + list[j].PATERNER;
                                }
                                else
                                {
                                    _Membersys = list[j].MEMBERID;
                                }
                               
                                AddPlayerToSign(_Membersys, (i + 1).ToString(), _ContentId);
                                
                                j += 1;
                            }                           
                        }
                    }
                
            }
        }

        /// <summary>
        /// 根据种子序号获得种子所在签位
        /// </summary>
        /// <param name="_SignQty"></param>
        /// <param name="_SeedOrder"></param>
        /// <returns></returns>
        public string GetSignOrderForSeed(string _SignQty, string _SeedOrder)
        {
            string SignOrder = "";
            int So=Convert.ToInt32(_SeedOrder);
            int Sq=Convert.ToInt16(_SignQty);
            if (_SeedOrder == "1")
            {
                SignOrder = "1";
            }
            else if(So<=Sq/2)
            {
                switch (So)
                {
                    case 2:
                        SignOrder = _SignQty;
                        break;
                    case 3:
                        SignOrder = (Sq / 2 + 1).ToString();
                        break;
                    case 4:
                        SignOrder = (Sq / 2).ToString();
                        break;
                    case 5:
                        SignOrder = (Sq * 3 / 4).ToString();
                        break;
                    case 6:
                        SignOrder = (Sq / 4 + 1).ToString();
                        break;
                    case 7:
                        SignOrder = (Sq * 3 / 4 + 1).ToString();
                        break;
                    case 8:
                        SignOrder = (Sq / 4).ToString();
                        break;
                    default:
                        //若未在设定范围内，则随机获取一个签位
                        SignOrder = _SeedOrder;
                        break;
                }
            }
            return SignOrder;
        }

        /// <summary>
        /// 获取种子选手应该排布的签位
        /// </summary>
        /// <returns></returns>
        private string GetRandomSign(string _SeedOrder)
        {
            //先默认为2
            return "2";
        }

        /// <summary>
        /// 根据contid和签号获得签内容
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <param name="_SignOrder"></param>
        /// <returns></returns>
        private List<WeTourSignModel> GetSignList(string _ContentId, string _SignOrder)
        {
            List<WeTourSignModel> list = new List<WeTourSignModel>();
            string sql = "select * from wtf_toursign where contentid='"+_ContentId+"' and signorder='"+_SignOrder+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourSignModel>>(dt);
            }
            return list;
        }

        private void AddPlayerToSign(string _Player, string SignOrder,string _Contentid)
        {
            List<WeTourSignModel> list = GetSignList(_Contentid, SignOrder);
            if (list.Count > 0)
            {
                if (list.Count > 1)
                {
                    //小组资格赛
                    //修改此前的签位的Round
                    string sql = "update wtf_Toursign set round='0' where contentid='" + _Contentid + "' and Signorder='" + SignOrder + "'";
                    int a = DbHelperSQL.ExecuteSql(sql);
                    WeTourSignModel model = list[0];
                    model.MEMBERSYS = _Player;
                    model.GROUPID = SignOrder;
                    model.ROUND = "0";
                    InsertNew(model);    
                }
                else
                { 
                    
                    //单个签
                    if (list[0].MEMBERSYS == "1bf973ea-7d4c-41d1-aa05-8bf68b567d77")
                    {
                        //轮空签,替换
                        list[0].MEMBERSYS = _Player;
                        list[0].GROUPID=SignOrder;
                        UpdateSignPlayer(list[0]);
                    }
                    else
                    { 
                        //非轮空签，增加球员
                        //判断是否已作为种子添加到
                        WeTourSignModel model = list[0];
                        model.MEMBERSYS = _Player;
                        model.GROUPID = SignOrder;
                        model.ROUND = "1";
                        InsertNew(model);                        
                    }
                }
            }
        }

        /// <summary>
        /// 判断球员是否已作为种子添加到签表
        /// </summary>
        /// <param name="_ContentID"></param>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        private bool IsSignAssigned(string _ContentID, string _Memsys)
        {
            string sql = "select * from wtf_toursign where contentid='"+_ContentID+"' and membersys='"+_Memsys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count < 0)
            {
                return true;
            }
            else
            {
                return false; 
               }
        }

        #endregion

        #region Adjust Tour Sign
        /// <summary>
        /// put seeded player to 
        /// </summary>
        /// <param name="_Contentid"></param>
        public void AdjustSeedPlayer(string _Contentid)
        { 
            
        }

        /// <summary>
        /// consider club, to adjust tour sign
        /// </summary>
        /// <param name="_ContentId"></param>
        public void DistinguishClub(string _ContentId)
        { 
            //查询小组
            string sql = "select distinct(groupid) from wtf_TourSign where ContentId='"+_ContentId+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                { 
                    //判断
                    if (IsGroupsFromSameClub(_ContentId, dr[0].ToString()))
                    { 
                        
                    }
                }
            }
            //查询每个小组的成员，判断是否有来自同一个club的情况

            //如果来自同一个club，则需要调换

            //再检查

        }
        /// <summary>
        /// 判断小组成员的club情况
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <param name="_GroupId"></param>
        /// <returns></returns>
        private bool IsGroupsFromSameClub(string _ContentId, string _GroupId)
        {
            bool Res = false;
            string clubs = "";
            string sql = "select * from wtf_TourSign where ContentId='"+_ContentId+"' and GroupId='"+_GroupId+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string MemClub = "";
                    string Mem = dr["membersys"].ToString();
                    if (Mem.IndexOf(",") > 0)
                    {
                        //double play
                        string memclub1 = ClubMemBiz.instance.GetMemberClub(Mem.Split(',')[0]);
                        string memclub2 = ClubMemBiz.instance.GetMemberClub(Mem.Split(',')[1]);
                        if (memclub1 != "")
                        {
                            if (clubs.IndexOf(memclub1) > 0)
                            {
                                //已存在
                                Res = true;
                                break;
                            }
                        }

                        if (memclub2 != "")
                        {
                            if (clubs.IndexOf(memclub2) > 0)
                            {
                                //已存在
                                Res = true;
                                break;
                            }
                        }

                        if (memclub1 == memclub2 && memclub1 != ""&&memclub2 != "")
                        {
                            MemClub = memclub1;
                        }
                        else if (memclub1 != memclub2 && memclub1 != "" && memclub2 != "")
                        {
                            MemClub = memclub1 + "," + memclub2;
                        }
                        else if (memclub1 != "" && memclub2 == "")
                        {
                            MemClub = memclub1;
                        }
                        else if (memclub1 == "" && memclub2 != "")
                        {
                            MemClub = memclub2;
                        }
                    }
                    else
                    { 
                        //single play
                        MemClub = ClubMemBiz.instance.GetMemberClub(Mem);
                        if (clubs.IndexOf(MemClub) > 0)
                        {
                            //已存在
                            Res = true;
                            break;
                        }
                    }
                    clubs += "," + MemClub;//添加clubname
                }
            }
            return Res;
        }

        /// <summary>
        /// change member to another group
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <param name="Membersys"></param>
        private void ChangeGroup(string _ContentId, string Membersys)
        { 
        
        }
        
        #endregion


        #region 显示签表
        /// <summary>
        /// 构造打印版签表
        /// </summary>
        /// <param name="_Contentid"></param>
        /// <returns></returns>
        public string GetPersonalMatchesbyContentid(string _Contentid)
        {
            string Groups = "";
            WeTourContModel tmodel = WeTourContentDll.instance.GetModelbyId(_Contentid);

            #region 添加小组签表
            //count Round,and order round form low to high
            string sql = "select * from wtf_match where ContentID='" + _Contentid + "' and round='0'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string sql1 = "select distinct(GroupId) from wtf_TourSign where ContentId='" + _Contentid + "'";
                DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        //添加小组比赛
                        string _ContBuid = "";
                        string sql2 = "select * from wtf_TourSign where ContentId='" + _Contentid + "' and GroupId='" + dt1.Rows[i][0].ToString() + "' order by id";
                        DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
                        //添加小组表头
                        _ContBuid += "<br/><table class=\"table table-bordered\"><tr><th colspan=\"2\">" + tmodel.ContentName + "<br />第" + dt1.Rows[i][0].ToString() + "组</th>";
                        if (dt2.Rows.Count > 0)
                        {
                            foreach (DataRow dr2 in dt2.Rows)
                            {
                                if (dr2["membersys"].ToString().IndexOf(",") > 0)
                                {
                                    dr2["ext1"] = WeMemberDll.instance.GetModel(dr2["membersys"].ToString().Split(',')[0]).USERNAME + "/<br />" + WeMemberDll.instance.GetModel(dr2["membersys"].ToString().Split(',')[1]).USERNAME;
                                }
                                else
                                {
                                    dr2["ext1"] = WeMemberDll.instance.GetModel(dr2["membersys"].ToString()).USERNAME;
                                }
                                _ContBuid += "<th >" + dr2["ext1"].ToString() + "</th>";
                            }
                        }
                        _ContBuid += "<th >胜负场</th><th >胜负局</th><th >名次</th> </tr>      ";
                        //添加行
                        if (dt2.Rows.Count > 0)
                        {
                            for (int m = 0; m < dt2.Rows.Count; m++)
                            {
                                _ContBuid += "<tr><td>" + (m + 1) + "</td><td>" + dt2.Rows[m]["ext1"].ToString() + "</td>";
                                //添加比赛场地
                                #region 添加比赛场地
                                for (int n = 0; n < dt2.Rows.Count; n++)
                                {
                                    if (m == n)
                                    {
                                        //分界线
                                        _ContBuid += "<td style=\"background-color:#ccc\"></td>";
                                    }
                                    else if (m < n)
                                    {
                                        //court，get court and order by two players
                                        WeMatchModel pmat = WeMatchDll.instance.GetModelbyPlayer(_Contentid, dt2.Rows[m]["membersys"].ToString(), dt2.Rows[n]["membersys"].ToString());
                                        string _Court = "未指定";
                                        if (pmat.COURTID != "" && pmat.COURTID != null)
                                        {
                                            _Court = CourtDll.Get_Instance().GetCourtNoByID(pmat.COURTID);
                                            _Court += "(" + pmat.PLACE + ")";
                                        }
                                        _ContBuid += "<td>" + _Court + "</td>";
                                    }
                                    else
                                    {
                                        //score
                                        WeMatchModel pmat = WeMatchDll.instance.GetModelbyPlayer(_Contentid, dt2.Rows[m]["membersys"].ToString(), dt2.Rows[n]["membersys"].ToString());
                                        if (pmat.STATE == 2)
                                        {
                                            _ContBuid += "<td>" + pmat.SCORE + "</td>";
                                        }
                                        else
                                        {
                                            _ContBuid += "<td></td>";
                                        }
                                    }
                                }
                                #endregion

                                //
                                _ContBuid += "<td></td><td></td><td></td> </tr>";
                            }
                        }
                        Groups += _ContBuid;
                    }
                }
            }
            #endregion

            #region 添加淘汰赛签表
            Groups += GetKnockOutSign(_Contentid);
            #endregion

            return Groups;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        private string GetKnockOutSign(string _ContentId)
        {
            string html = "";
            WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_ContentId);
            int SignQ = Convert.ToInt32(model.SignQty);
            //只有当SignQ大于1时才显示淘汰赛签表
            if(SignQ>1)
            {
                //计算列数
                double Col = Math.Log(SignQ, 2);
                Col += 1;

                //添加表头
                html += "<div class=\"scores-draw-table-wrapper\"><table class=\"scores-draw-table\"><thead><tr>";
                string Colname = "";
                for (int i = (int)Col; i >0; i--)
                {
                    Colname += "<th style=\"width:150px\"><span >" + RenderHeader((i).ToString()) + "</span></th>";
                }
                html += Colname+"</tr></thead>";

                //添加表体
                html += "<tbody>";
                
                string sql = "select * from wtf_match where ContentID='"+_ContentId+"' and Round=1 order by matchorder";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    #region 添加第一列
                    string Player1 = "";
                    if (dt.Rows[j]["player1"].ToString() != "")
                    {
                        if (dt.Rows[j]["player1"].ToString().IndexOf(",") > 0)
                        {
                            //双打球员
                            Player1 = WeMemberDll.instance.GetModel(dt.Rows[j]["player1"].ToString().Split(',')[0]).USERNAME + "/" + WeMemberDll.instance.GetModel(dt.Rows[j]["player1"].ToString().Split(',')[1]).USERNAME;
                        }
                        else
                        {
                            Player1 = WeMemberDll.instance.GetModel(dt.Rows[j]["player1"].ToString()).USERNAME;
                        }
                    }
                    string Player2 = "";
                    if (dt.Rows[j]["player2"].ToString() != "")
                    {
                        if (dt.Rows[j]["player2"].ToString().IndexOf(",") > 0)
                        {
                            //双打球员
                            Player2 = WeMemberDll.instance.GetModel(dt.Rows[j]["player2"].ToString().Split(',')[0]).USERNAME + "/" + WeMemberDll.instance.GetModel(dt.Rows[j]["player2"].ToString().Split(',')[1]).USERNAME;
                        }
                        else
                        {
                            Player2 = WeMemberDll.instance.GetModel(dt.Rows[j]["player2"].ToString()).USERNAME;
                        }
                    }

                    html += "<tr><td><div class=\"scores-draw-entry-box\"><table class=\"scores-draw-entry-box-table\"><tbody><tr><td>" + (j * 2 + 1) + "</td><td></td><td style=\"border-bottom:1px dotted #c7c8c8\">" + Player1 + "</td></tr><tr><td>" + (j * 2 + 2) + "</td><td></td><td>" + Player2 + "</td></tr></tbody></table></div></td>";
                    #endregion

                    //添加第二列
                    html += "<td rowspan=\"1\" class=\"bye-bracket\"><div class=\"scores-draw-entry-box\"></div></td>";
                    
                    //判断是否还需要添加其他列
                    if (Col - 2 > 0)
                    {
                        //判断是否是奇数行
                        if (j % 2 == 0)
                        {
                            //添加其余列
                            //第一行添加剩余列
                            if (j == 0)
                            {
                                //第一行添加剩余列
                                for (int k = 0; k < Col - 2; k++)
                                {
                                    double RowSp = Math.Pow(2, (double)(k + 1));
                                    html += "<td rowspan=\"" + RowSp + "\" class=\"bye-bracket\"><div class=\"scores-draw-entry-box\"></div></td>";
                                }
                            }
                            else
                            {
                                int SpcCol = GetSpecCol(j);
                                //第一行添加剩余列
                                for (int k = 0; k < SpcCol; k++)
                                {
                                    double RowSp = Math.Pow(2, (double)(k + 1));
                                    html += "<td rowspan=\"" + RowSp + "\" class=\"bye-bracket\"><div class=\"scores-draw-entry-box\"></div></td>";
                                }
                            }
                        }

                    }
                    html += "</tr>";
                }
                html += "</tbody></table></div>";
            }

            return html;
        }

        /// <summary>
        /// 根据总行数，以及行号确定要添加的td的rowspan 参数值
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="RowN"></param>
        /// <returns></returns>
        private int GetSpecCol(int RowN)
        {
            int SpcCol = 0;
            switch (RowN)
            {
                #region 枚举特殊列数,由于抓不出规律，只能用枚举值
                case 2:
                    SpcCol = 1;
                    break;
                case 4:
                    SpcCol = 2;
                    break;
                case 6:
                    SpcCol = 1;
                    break;
                case 8:
                    SpcCol = 3;
                    break;
                case 10:
                    SpcCol = 1;
                    break;
                case 12:
                    SpcCol = 2;
                    break;
                case 14:
                    SpcCol = 1;
                    break;
                case 16:
                    SpcCol = 4;
                    break;
                case 18:
                    SpcCol = 1;
                    break;
                case 20:
                    SpcCol = 2;
                    break;
                case 22:
                    SpcCol = 1;
                    break;
                case 24:
                    SpcCol = 3;
                    break;
                case 26:
                    SpcCol = 1;
                    break;
                case 28:
                    SpcCol = 2;
                    break;
                case 30:
                    SpcCol = 1;
                    break;
                case 32:
                    SpcCol = 5;
                    break;
                case 34:
                    SpcCol = 1;
                    break;
                case 36:
                    SpcCol = 2;
                    break;
                case 38:
                    SpcCol = 1;
                    break;
                case 40:
                    SpcCol = 3;
                    break;
                case 42:
                    SpcCol = 1;
                    break;
                case 44:
                    SpcCol = 2;
                    break;
                case 46:
                    SpcCol = 1;
                    break;
                case 48:
                    SpcCol = 4;
                    break;
                case 50:
                    SpcCol = 1;
                    break;
                case 52:
                    SpcCol = 2;
                    break;
                case 54:
                    SpcCol = 1;
                    break;
                case 56:
                    SpcCol = 3;
                    break;
                case 58:
                    SpcCol = 1;
                    break;
                case 60:
                    SpcCol = 2;
                    break;
                case 62:
                    SpcCol = 1;
                    break;
                #endregion
            }
            return SpcCol;
        }

        private string RenderHeader(string _Round)
        {
            string _Header = "";
            switch (_Round)
            { 
                case "1":
                    _Header = "冠军";
                    break;
                case "2":
                    _Header = "决赛";
                    break;
                case "3":
                    _Header = "半决赛";
                    break;
                case "4":
                    _Header = "1/4决赛";
                    break;
                case "5":
                    _Header = "R16";
                    break;
                case "6":
                    _Header = "R32";
                    break;
                case "7":
                    _Header = "R64";
                    break;
                case "8":
                    _Header = "R128";
                    break;
            }
            return _Header;
        }

        public string PrintTourSign(string _Toursys)
        {
            string html = "";
            List<WeTourContModel> list = WeTourContentDll.instance.GetContentlist(_Toursys);
            if (list.Count > 0)
            {
                foreach (WeTourContModel model in list)
                {
                    //加载项目名称
                    html += "<h2 >"+model.ContentName+"</h2>";
                    //加载各小组的签位情况
                    html += GetPersonalMatchesbyContentid(model.id);
                   
                    //添加分页符
                    html += "<div style=\"page-break-after:always\"></div>";
                }
            }
            return html;
        }

        /// <summary>
        /// 获得赛事比赛秩序册
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public string GetToursMatchOrder(string _Toursys)
        {
            string html = "";
            //计算赛事日期
            string sqld = "select distinct(matchdate) from wtf_match where TourSys='" + _Toursys + "'";
            DataTable dtd = DbHelperSQL.Query(sqld).Tables[0];
            if (dtd.Rows.Count > 0)
            {
                foreach (DataRow drd in dtd.Rows)
                {
                    //添加比赛日期
                    html += "<h2>" + drd["matchdate"].ToString() + "竞赛秩序表</h2>";
                    //计算赛事的场地数量
                    string sqlc = "select distinct(courtid) from wtf_match where TourSys='" + _Toursys + "' and matchdate='"+drd["matchdate"].ToString()+"'";
                    DataTable dtc = DbHelperSQL.Query(sqlc).Tables[0];
                    if (dtc.Rows.Count > 0)
                    {
                        //分组处理
                        if (dtc.Rows.Count > 4)
                        {
                            string Courts = "";
                            for (int i = 0; i < dtc.Rows.Count; i++)
                            {
                                Courts += dtc.Rows[i][0].ToString() + ",";
                                if (Courts.TrimEnd(',').Split(',').Length == 4)
                                {
                                    html += GenerateMO(_Toursys, Courts.TrimEnd(','), drd["matchdate"].ToString());
                                    Courts = "";
                                }
                            }
                            if (Courts != "")
                            {
                                html += GenerateMO(_Toursys, Courts.TrimEnd(','), drd["matchdate"].ToString());
                            }
                        }
                        else
                        {
                            string Courts = "";
                            //未超过四片场地,只需一个表即可显示
                            for (int i = 0; i < dtc.Rows.Count; i++)
                            {
                                Courts += dtc.Rows[i][0].ToString() + ",";
                            }
                            Courts=Courts.TrimEnd(',');
                            html += GenerateMO(_Toursys, Courts, drd["matchdate"].ToString());
                        }
                    }
                }
            }
            return html;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Courts">格式“1,2,3”</param>
        /// <param name="_Matchdate"></param>
        /// <returns></returns>
        private string GenerateMO(string _Toursys,string _Courts,string _Matchdate)
        {
            string html = "";
            //添加表头
            html += "<br /><table class=\"table table-bordered\" style=\"text-align:center;width:100%\"><tr><th ></th>";
            //添加标题
            string _courtSql = "";
            if (_Courts.IndexOf(',') > 0)
            {
                //有多个球场
                string[] courts = _Courts.Split(',');
                for (int i = 0; i < courts.Length; i++)
                {
                    html += "<th >" + CourtDll.Get_Instance().GetCourtNoByID(courts[i]) + "</th>";
                    _courtSql += "'" + courts[i]+"',";
                }
                _courtSql = _courtSql.TrimEnd(',');
            }
            else
            { 
                //只有一个球场
                html += "<th >"+CourtDll.Get_Instance().GetCourtNoByID(_Courts)+"</th>";
                _courtSql += "'"+_Courts+"'";
            }
            html += "</tr>";

            //添加比赛信息
            //根据场地确定单个场地比赛数量
            string sqlp = "select * from (select distinct(Convert(int,place)) as place from wtf_match where TourSys='" + _Toursys + "' and courtid in (" + _courtSql + ") and matchdate='" + _Matchdate + "') a order by  a.place";
            DataTable dtp = DbHelperSQL.Query(sqlp).Tables[0];
            if (dtp.Rows.Count > 0)
            {
                for (int i = 0; i < dtp.Rows.Count; i++)
                { 
                    //逐行添加比赛信息
                    html += "<tr><td>" + dtp.Rows[i][0].ToString() + "</td>";
                    if (_Courts.IndexOf(',') > 0)
                    {
                        string [] courts=_Courts.Split(',');
                        for (int j = 0; j < courts.Length; j++)
                        {
                            //根据场地和序号确定一场比赛
                            string sqlm = "select * from wtf_match where TourSys='" + _Toursys + "' and matchdate='" + _Matchdate + "' and place='" + dtp.Rows[i][0].ToString() + "' and CourtId='" + courts[j] + "'";
                            DataTable dtm = DbHelperSQL.Query(sqlm).Tables[0];
                            if (dtm.Rows.Count > 0)
                            {
                                //添加内容
                                //确定比赛子项名称
                                WeTourContModel conmodel = WeTourContentDll.instance.GetModelbyId(dtm.Rows[0]["contentid"].ToString());
                                string MatchName = conmodel.ContentName;

                                //确定比赛轮次
                                string RoundName = "";
                                int SignQ = Convert.ToInt32(conmodel.SignQty);
                                int Round = Convert.ToInt32(dtm.Rows[0]["round"].ToString());
                                RoundName = WeTourDll.instance.RenderRound(Round, SignQ);
                                if (dtm.Rows[0]["round"].ToString() == "0")
                                {
                                    //小组赛
                                    RoundName += "第" + dtm.Rows[0]["etc1"].ToString() + "组";
                                }
                                else
                                { 
                                    //淘汰赛,添加比赛序号
                                    RoundName += "(第" + dtm.Rows[0]["matchorder"].ToString() + "场)";
                                }
                                //确定比赛双方
                                string Player1 = "";
                                if (dtm.Rows[0]["player1"].ToString() != "")
                                {
                                    if (dtm.Rows[0]["player1"].ToString().IndexOf(",") > 0)
                                    {
                                        //双打
                                        Player1 = WeMemberDll.instance.GetModel(dtm.Rows[0]["player1"].ToString().Split(',')[0]).USERNAME + "/" + WeMemberDll.instance.GetModel(dtm.Rows[0]["player1"].ToString().Split(',')[1]).USERNAME;
                                    }
                                    else
                                    {
                                        //单打
                                        Player1 = WeMemberDll.instance.GetModel(dtm.Rows[0]["player1"].ToString()).USERNAME;
                                    }
                                }
                                else
                                { 
                                    //获得哪个小组的胜者
                                    Player1 = GetNullPlayer(dtm.Rows[0]["contentid"].ToString(), dtm.Rows[0]["matchorder"].ToString(), "1");
                                }
                                string Player2 = "";
                                if (dtm.Rows[0]["player2"].ToString() != "")
                                {
                                    if (dtm.Rows[0]["player2"].ToString().IndexOf(",") > 0)
                                    {
                                        //双打
                                        Player2 = WeMemberDll.instance.GetModel(dtm.Rows[0]["player2"].ToString().Split(',')[0]).USERNAME + "/" + WeMemberDll.instance.GetModel(dtm.Rows[0]["player2"].ToString().Split(',')[1]).USERNAME;
                                    }
                                    else
                                    {
                                        //单打
                                        Player2 = WeMemberDll.instance.GetModel(dtm.Rows[0]["player2"].ToString()).USERNAME;
                                    }
                                }
                                else
                                {
                                    //获得哪个小组的胜者
                                    Player2 = GetNullPlayer(dtm.Rows[0]["contentid"].ToString(), dtm.Rows[0]["matchorder"].ToString(), "2");
                                }
                                html += "<td>" + MatchName + "<br />" + RoundName + "<br />" + Player1 + "<br />vs<br />" + Player2 + "</td>";
                            }
                            else
                            {
                                html += "<td></td>";
                            }
                        }
                    }
                    else
                    { 
                        //根据场地和序号确定一场比赛
                        string sqlm = "select * from wtf_match where TourSys='" + _Toursys + "' and matchdate='" + _Matchdate + "' and place='" + dtp.Rows[i][0].ToString() + "' and CourtId='" + _Courts + "'";
                        DataTable dtm = DbHelperSQL.Query(sqlm).Tables[0];
                        if (dtm.Rows.Count > 0)
                        {
                            //添加内容
                            //确定比赛子项名称
                            WeTourContModel conmodel = WeTourContentDll.instance.GetModelbyId(dtm.Rows[0]["contentid"].ToString());
                            string MatchName = conmodel.ContentName;

                            //确定比赛轮次
                            string RoundName = "";
                            int SignQ=Convert.ToInt32(conmodel.SignQty);
                            int Round=Convert.ToInt32(dtm.Rows[0]["round"].ToString());
                            RoundName=WeTourDll.instance.RenderRound(Round,SignQ);
                            if (dtm.Rows[0]["round"].ToString() == "0")
                            {
                                //小组赛
                                RoundName+="第"+dtm.Rows[0]["etc1"].ToString()+"组";
                            }
                            //确定比赛双方
                            string Player1 = "";
                            if (Player1 != "")
                            {
                                if (dtm.Rows[0]["player1"].ToString().IndexOf(",") > 0)
                                {
                                    //双打
                                    Player1 = WeMemberDll.instance.GetModel(dtm.Rows[0]["player1"].ToString().Split(',')[0]).USERNAME + "/" + WeMemberDll.instance.GetModel(dtm.Rows[0]["player1"].ToString().Split(',')[1]).USERNAME;
                                }
                                else
                                {
                                    //单打
                                    Player1 = WeMemberDll.instance.GetModel(dtm.Rows[0]["player1"].ToString()).USERNAME;
                                }
                            }
                            else
                            {
                                //获得哪个小组的胜者
                                Player1 = GetNullPlayer(dtm.Rows[0]["contentid"].ToString(), dtm.Rows[0]["matchorder"].ToString(), "1");
                            }
                            string Player2 = "";
                            if (dtm.Rows[0]["player2"].ToString() != "")
                            {
                                if (dtm.Rows[0]["player2"].ToString().IndexOf(",") > 0)
                                {
                                    //双打
                                    Player2 = WeMemberDll.instance.GetModel(dtm.Rows[0]["player2"].ToString().Split(',')[0]).USERNAME + "/" + WeMemberDll.instance.GetModel(dtm.Rows[0]["player2"].ToString().Split(',')[1]).USERNAME;
                                }
                                else
                                {
                                    //获得哪个小组的胜者
                                    Player2 = GetNullPlayer(dtm.Rows[0]["contentid"].ToString(), dtm.Rows[0]["matchorder"].ToString(), "2");
                                }
                            }
                            else
                            {
                                //单打
                                Player2 = WeMemberDll.instance.GetModel(dtm.Rows[0]["player2"].ToString()).USERNAME;
                            }
                            html += "<td>"+MatchName+"<br />"+RoundName+"<br />"+Player1+"<br />vs<br />"+Player2+"</td>";
                        }
                        else
                        {
                            html += "<td>未知<br />未知<br />vs<br />未知</td>";
                        }                       
                    }
                    html += "</tr>";
                }
            }
            return html;
        }

        private string GetNullPlayer(string _ContentID, string _MatchOrder, string _Player)
        {
            string _NullPlayer;
            string sql = "select * from wtf_match where ContentID='" + _ContentID + "' and winto='" + _MatchOrder + "' and etc3='"+_Player+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count == 1)
                {
                    //淘汰赛
                    _NullPlayer = "第" + dt.Rows[0]["matchorder"].ToString() + "场比赛的胜者";
                }
                else
                {
                    //小组赛
                    _NullPlayer = "第" + dt.Rows[0]["etc1"].ToString() + "小组的第一名";
                }
            }
            else
            {
                _NullPlayer = "未知";
            }
            return _NullPlayer;
        }

        #endregion

        #region 后台管理签表
        /// <summary>
        /// 获取展示用的toursign
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public List<Model_SignedApp> Tgm_GetSignApp(string _ContentId)
        {
            List<Model_SignedApp> list = new List<Model_SignedApp>();
            //先获取所有的签号，再依次获取签位上对应的人员
            WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(_ContentId);
            int SignQty=Convert.ToInt32(cont.SignQty);
            for (int i = 0; i < SignQty; i++)
            {
                Model_SignedApp model = new Model_SignedApp();
                model.signorder = (i + 1).ToString();
                //根据SignOrder获取人员
                List<WeTourSignModel> memlist = GetTourSignPlayer(_ContentId, model.signorder);
                List<Model_SignMember> signMemlist = new List<Model_SignMember>();
                foreach (WeTourSignModel smodel in memlist)
                {
                    Model_SignMember SignMem = new Model_SignMember();
                    SignMem.memsys = smodel.MEMBERSYS;
                    SignMem.name = smodel.PLAYER1NAME;
                    if (!string.IsNullOrEmpty(smodel.PLAYER2NAME))
                    {
                        SignMem.name += "/" + smodel.PLAYER2NAME;
                    }
                    signMemlist.Add(SignMem);
                }
                model.signmember = signMemlist;

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 获取未分配至签表的报名人员信息
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public List<Model_SignMember> GetUnSignedApp(string _ContentId)
        {
            List<Model_SignMember> applist = new List<Model_SignMember>();
            WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_ContentId);
            string sql = "";
            if (model.ContentType.IndexOf("双") > 0)
            {
                sql = string.Format("select * from wtf_tourapply where contentid='{0}' and memberid+','+paterner not in (select Membersys from wtf_toursign where Contentid='{0}')", _ContentId);
            }
            else
            {
                sql = string.Format("select * from wtf_tourapply where contentid='{0}' and memberid not in (select Membersys from wtf_toursign where Contentid='{0}')", _ContentId);
            }
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                List<WeTourApplyModel> list = JsonHelper.ParseDtModelList<List<WeTourApplyModel>>(dt);
                foreach (WeTourApplyModel app in list)
                {
                    Model_SignMember memSign = new Model_SignMember();

                    if (app.memtype == "group")
                    {
                        //加载团体名称
                        weTeamModel team = weTeamdll.instance.GetModel(app.MEMBERID);
                        memSign.name = team.TEAMNAME;
                        memSign.memsys = app.MEMBERID;
                    }
                    else
                    {
                        //添加姓名
                        WeMemberModel mem = WeMemberDll.instance.GetModel(app.MEMBERID);
                        memSign.name = mem.USERNAME;
                        memSign.memsys = mem.SYSNO;

                        if (model.ContentType.IndexOf("双") > 0)
                        {
                            //添加搭档
                            WeMemberModel mem1 = WeMemberDll.instance.GetModel(app.PATERNER);
                            memSign.name += "," + mem1.USERNAME;
                            memSign.memsys += "," + mem1.SYSNO;
                        }
                    }
                    
                    applist.Add(memSign); 
                }
            }
            return applist;
        }

        /// <summary>
        /// 更新签位
        /// </summary>
        /// <param name="list"></param>
        /// <param name="_Contid"></param>
        public void Tgm_UpdateContSign(List<Model_SignedApp> list,string _Contid)
        { 
            //先删除此前的签表
            DeleteTourSign(_Contid);

            //再添加新的签位
            WeTourContModel cont=WeTourContentDll.instance.GetModelbyId(_Contid);
            foreach (Model_SignedApp model in list)
            {
                if (model.signmember.Count > 0)
                {
                    //已添加了人员
                    foreach (Model_SignMember mem in model.signmember)
                    {
                        WeTourSignModel Smod = new WeTourSignModel();
                        Smod.CONTENTID = _Contid;
                        Smod.TOURSYS = cont.Toursys;
                        Smod.SIGNORDER = model.signorder;
                        Smod.MEMBERSYS = mem.memsys;
                        Smod.ROUND = "0";
                        Smod.GROUPID = model.signorder;
                        InsertNew(Smod);
                    }
                }
                else
                { 
                    //未添加人员,添加一个轮空签
                    WeTourSignModel Smod = new WeTourSignModel();
                    Smod.CONTENTID = _Contid;
                    Smod.TOURSYS = cont.Toursys;
                    Smod.SIGNORDER = model.signorder;
                    Smod.MEMBERSYS = "1bf973ea-7d4c-41d1-aa05-8bf68b567d77";//轮空
                    Smod.ROUND = "0";
                    Smod.GROUPID = model.signorder;
                    InsertNew(Smod);
                }
            }
        }

        #endregion
    }
}
