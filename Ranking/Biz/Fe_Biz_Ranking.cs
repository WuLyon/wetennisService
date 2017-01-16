using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Member;

namespace Ranking
{
    public class Fe_Biz_Ranking
    {
        public static Fe_Biz_Ranking instance = new Fe_Biz_Ranking();

        /// <summary>
        /// 获取排名筛选条件
        /// </summary>
        /// <returns></returns>
        public List<Fe_Model_rankingFilter> GetRankingFilters()
        {
            List<Fe_Model_rankingFilter> Fe_Ranks = new List<Fe_Model_rankingFilter>();
            string _Groups = "青少年组,青年组,中年组,老年组";
            string[] gps = _Groups.Split(',');
            for (int i = 0; i < gps.Length; i++)
            {
                Fe_Model_rankingFilter rank1 = new Fe_Model_rankingFilter();
                rank1.text = gps[i];
                rank1.value = (i+1).ToString();
                rank1.children = GetChild(rank1.value);
                Fe_Ranks.Add(rank1);
            }          
                            
            return Fe_Ranks;
        }

        public List<Dictionary<string, string>> GetRankTypes()
        {
            List<Dictionary<string, string>> rankIds = new List<Dictionary<string, string>>();
            //添加全网

            //添加四川高校网球排行榜
            Dictionary<string, string> rank2 = new Dictionary<string, string>();
            rank2.Add("text","成都市高校排行榜");
            rank2.Add("value","union1");
            rankIds.Add(rank2);
            return rankIds;
        }

        private List<Dictionary<string,string>> GetChild(string _Value)
        {
            List<Dictionary<string,string>> children=new List<Dictionary<string,string>>();
            string items = "男子单打,女子单打,男子双打,女子双打";
            string[] item = items.Split(',');
            for (int i = 0; i < item.Length; i++)
            { 
                Dictionary<string,string> cell=new Dictionary<string,string>();
                cell.Add("text",item[i]);
                string _val=_Value+"0"+(i+1).ToString();
                cell.Add("value",_val);
                children.Add(cell);
            }
            return children;

        }

        /// <summary>
        /// 获得排名详情
        /// </summary>
        /// <param name="_Typesys"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public List<Fe_Model_rankings> GetRankingDetail(string _Typesys, string _value,string _currentPage,string _limit)
        {
            List<Fe_Model_rankings> rankings = new List<Fe_Model_rankings>();
            Fe_Model_rankenum items = getenum(_value);
            //获取排名
            List<Model_MemRanking> itemRank = RankDll.instance.GetUnionMemRanking(_Typesys, items.groupname, items.issingle, items.gender);
            
            //筛选排名
            int StartIndex = Convert.ToInt32(_limit) * (Convert.ToInt32(_currentPage)-1);
            int EndIndex = Convert.ToInt32(_limit) * Convert.ToInt32(_currentPage);
            if (EndIndex > itemRank.Count)
            {
                EndIndex = itemRank.Count;
            }
            for (int i = StartIndex; i < EndIndex; i++)
            {
                Model_MemRanking memr = itemRank[i];
                Fe_Model_rankings rank = new Fe_Model_rankings();
                rank.type = Convert.ToInt32(_value);
                rank.ranking = Convert.ToInt32(memr.Rank);
                rank.username = memr.UserName;
                rank.userimage = memr.MemImg;
                rank.userid = memr.MemSys;
                rank.pts = Convert.ToInt32(memr.Points);
                rank.clubname = memr.ClubName;
                rankings.Add(rank);
            }
            return rankings;
        }

        public Fe_Model_rankenum getenum(string _value)
        {
            Fe_Model_rankenum enums = new Fe_Model_rankenum();
            string sql = "select * from Rank_rankenum where id='"+_value+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                enums = JsonHelper.ParseDtInfo<Fe_Model_rankenum>(dt);
            }
            return enums;
        }

        /// <summary>
        /// 获取排名详情
        /// </summary>
        /// <param name="_userId"></param>
        /// <param name="_rankId"></param>
        /// <returns></returns>
        public Fe_Model_rankingDetailsInfo getRankDetails(string _userId,string _rankId,string _currentUser)
        {
            Fe_Model_rankingDetailsInfo model = new Fe_Model_rankingDetailsInfo();
            //个人基础信息
            WeMemberModel mem = WeMemberDll.instance.GetModel(_userId);
            if (mem.GENDER == "男")
            {
                model.usersex = 0;
            }
            else
            {
                model.usersex = 1;
            }

            model.username = mem.USERNAME;
            model.userimage = mem.EXT1;
            model.birthday = mem.BIRTHDAY;
            model.constellation = "摩羯座";
            model.account = mem.NAME;
            model.year = mem.TENNISSTARTYEAR;
            model.city = mem.CITY;
            model.stature = mem.HEIGHT + "cm";
            model.weight = mem.WEIGHT + "kg";
            model.front = mem.FOREHAND;
            model.back = mem.BACKHAND;

            //是否关注该用户
            model.like = false;
            //排名信息
            model.singleNumber = 1;
            model.doubleNumber = 10;
            model.singleAccumulatePoints = 1000;
            model.doubleAccumulatePoints = 500;
            model.singleTabInfo = true;
            model.singleTabInfo = false;
            
            return model;
        }
    }
}
