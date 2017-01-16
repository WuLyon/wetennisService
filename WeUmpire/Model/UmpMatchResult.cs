using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeUmpire
{
    public class UmpMatchResult
    {
        /// <summary>
        /// 比赛主键
        /// </summary>
        public string MatchSys { get; set; }
        /// <summary>
        /// 盘分
        /// </summary>
        public List<SetScores> SetScore { get; set; }
    }

    public class SetScores {
        public string SetOrder { get; set; }
        public string P1SetS { get; set; }
        public string P2SetS { get; set; }
    }
}
