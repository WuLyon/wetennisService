using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    /// <summary>
    /// 赛程调整
    /// 2016-10-28
    /// </summary>
    public class Biz_ScheduleAdjust
    {
        public static Biz_ScheduleAdjust instance = new Biz_ScheduleAdjust();

        /// <summary>
        /// 根据赛事主键和日期获取赛程详情
        /// </summary>
        /// <param name="_eventId"></param>
        /// <param name="_date"></param>
        /// <returns></returns>
        public Model_Schedule_Adjust Get_Scheduel_TourDate(string _eventId, string _date)
        {
            Model_Schedule_Adjust model = new Model_Schedule_Adjust();
            //添加courts
            Dictionary<string, object> courts_list = new Dictionary<string, object>();//定义courts
            List<string> courtId_list = WeMatchDll.instance.GetDistinctCourt(_eventId, _date);
            foreach (string courtId in courtId_list)
            {
               //单独添加每个场地的赛事主键
               Dictionary<string, object> matchsys= WeMatchDll.instance.GetMatchsys_byCourtDate(_eventId, _date, courtId);
               courts_list.Add(courtId, matchsys);
            }
            model.courts = courts_list;

            //添加matches
            List<WeMatchModel> match_list = WeMatchDll.instance.GetMachesByDate(_eventId, _date);
            Dictionary<string, object> maches_list = new Dictionary<string, object>();
            foreach (WeMatchModel wematch in match_list)
            {
                Dictionary<string, object> match = new Dictionary<string, object>();
                //添加比赛内容
                Dictionary<string, object>matchContent = new Dictionary<string, object>();
                string matchName=wematch.etc4+"-"+wematch.ContentName+"-"+wematch.RoundName;
                if (wematch.ROUND == 0)
                {
                    matchName += "-第" + wematch.etc1 + "组";
                }
                matchContent.Add("name", matchName);
                List<string> matchP = new List<string>();
                matchP.Add(wematch.PLAYER1);
                matchP.Add(wematch.PLAYER2);
                matchContent.Add("players",matchP);
                maches_list.Add(wematch.SYS, matchContent);
            }
            model.matches = maches_list;

            //添加players，去除重复的players
            model.players = WeTourSignDll.instance.GetDistinctPlayer(_eventId);
            return model;
        }

        /// <summary>
        /// 调整赛程结果
        /// </summary>
        /// <param name="model"></param>
        public void AdjustProgram(Model_req_adjsutSchedule model)
        { 
            //循环场地
            for (int i = 0; i < model.courts.Count; i++)
            { 
                //调整每一场比赛的顺序
                string courtId = model.courts[i].id;
                for (int j = 0; j < model.courts[i].matches.Count; j++)
                { 
                    //调整场地和场序
                    string matchsys = model.courts[i].matches[j];
                    string order = (j + 1).ToString();
                    WeMatchDll.instance.UpdateMatch_CourtId(matchsys, courtId);
                    WeMatchDll.instance.UpdateMatch_Place(matchsys, order);
                }
            }
        }

    }
}
