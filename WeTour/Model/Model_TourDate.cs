using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class Model_TourDate
    {
        public string Id { get; set; }

        public string TourSys { get; set; }
        public string TourDate { get; set; }

        //ext field
        public string TourMatchQty { get; set; }
    }

    public class Model_TourDateRound
    {
        public string Id { get; set; }
        public string TourSys { get; set; }
        //组别
        public string TourDate { get; set; }
        //项目名称
        public string ContentId { get; set; }
        //轮次
        public string Round { get; set; }

        //ext field
        //比赛进行的日期
        public string MatchDate { get; set; }

        public int TourMatchQty { get; set; }

        public string ContentType { get; set; }
        public string GroupType { get; set; }
        public string ContPriority { get; set; }
    }

    //用于排序的实体
    public class Model_PriorityCont
    {
        public int priority { get; set; }

        public List<Model_TourDateRound> contlist { get; set; }
    }

    public class Model_DistriTDR
    {
        public string contId { get; set; }
        public string contName { get; set; }
        public List<Model_DistricontRound> contRound { get; set; }

        /// <summary>
        /// 比赛数量
        /// </summary>
        public string MatchQty { get; set; }
    }

    public class Model_DistricontRound {
        public string roundNum { get; set; }
        public string roundName { get; set; }
        public string isCheck { get; set; }
        public string isEnable { get; set; }

        /// <summary>
        /// 比赛数量
        /// </summary>
        public string MatchQty { get; set; }
    }
}
