using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class Biz_TourContentScore
    {
        public static Biz_TourContentScore instance = new Biz_TourContentScore();

        /// <summary>
        /// 添加的新的纪录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddNew(Model_TourContScore model)
        {
            string sql = string.Format("Insert into Tour_ContentScore (toursys,contentid,round,score)  values ('{0}','{1}','{2}','{3}')", model.toursys, model.contentid, model.round, model.score);
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// 获取指定轮次的分数
        /// </summary>
        /// <param name="_Contentid"></param>
        /// <param name="_Round"></param>
        /// <returns></returns>
        public int GetContRoundScore(string _Contentid, string _Round)
        {
            int Score = 0;
            string sql = "select * from Tour_ContentScore where contentid='"+_Contentid+"' and round='"+_Round+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Score = Convert.ToInt32(dt.Rows[0]["score"].ToString());
            }
            return Score;
        }

        private List<Model_TourContScore> GetContRoundsScore(string _Toursys)
        {
            List<Model_TourContScore> list = new List<Model_TourContScore>();
            string sql = "select * from Tour_ContentScore where toursys='"+_Toursys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<Model_TourContScore>>(dt);
            }
            return list;
        }

        private void DeleteContentScores(string _TourSys)
        {
            string sql = "Delete Tour_ContentScore where toursys='"+_TourSys+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        #region 页面处理
        public List<PageS_ScoreSetting> GetRoundScore(string _TourSys)
        {
            List<PageS_ScoreSetting> list = new List<PageS_ScoreSetting>();
            //get rounds
            List<WeTourContModel> contlist = WeTourContentDll.instance.GetContentlist(_TourSys);
            string sql = "select distinct(contentId) from Tour_ContentScore where toursys='"+_TourSys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            for (int i = 0; i < contlist.Count; i++)
            {
                PageS_ScoreSetting model = new PageS_ScoreSetting();
                WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(contlist[i].id);
                model.contentId = cont.id;
                model.contentName = cont.ContentName;
                //添加项目的轮次
                List<PageS_contentRounds> roundlist = new List<PageS_contentRounds>();
                string sql2 = "select distinct(round) from wtf_match where contentid='" + cont.id + "'";
                DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    PageS_contentRounds round = new PageS_contentRounds();
                    round.roundNum = dt2.Rows[j]["round"].ToString();
                    round.score = GetContRoundScore(cont.id,round.roundNum).ToString();
                    int RounI = Convert.ToInt32(round.roundNum);
                    int Cap = Convert.ToInt32(cont.SignQty);
                    string RounName = WeTourDll.instance.RenderRound(RounI, Cap);
                    round.roundName = RounName;
                    roundlist.Add(round);
                }
                model.contentRounds = roundlist;
                list.Add(model);
            }
                return list;
        }

        /// <summary>
        /// 更新轮次积分奖励
        /// </summary>
        /// <param name="list"></param>
        public void UpdateContentScore(List<PageS_ScoreSetting> list,string _Toursys)
        { 
            //删除此前的分配
            DeleteContentScores(_Toursys);

            //添加分配
            foreach (PageS_ScoreSetting model in list)
            {
                foreach (PageS_contentRounds round in model.contentRounds)
                {
                    Model_TourContScore score = new Model_TourContScore();
                    score.toursys = _Toursys;
                    score.contentid = model.contentId;
                    score.round = round.roundNum;
                    score.score = round.score;

                    AddNew(score);
                }
            }
        }
        #endregion
    }
}
