using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeTour;

namespace WeUmpire
{
    /// <summary>
    /// 裁判页面，比分显示实体
    /// </summary>
    public  class UmpScoreModel
    {
        /// <summary>
        /// p1比赛盘分
        /// </summary>
        public List<SetGameWon> SetScore1 { get; set; }

        /// <summary>
        /// p2比赛盘分
        /// </summary>
        public List<SetGameWon> SetScore2 { get; set; }

        /// <summary>
        /// p1分数
        /// </summary>
        public string GameScore1 { get; set; }
        /// <summary>
        /// p2分数
        /// </summary>
        public string GameScore2 { get; set; }
        /// <summary>
        /// p1局分
        /// </summary>
        public string GameWon1 { get; set; }
        /// <summary>
        /// p2局分
        /// </summary>
        public string GameWon2 { get; set; }
        /// <summary>
        /// 上一分的信息
        /// </summary>
        public string LastPtInfo { get; set; }
    }

    public class SetGameWon
    {
        /// <summary>
        /// 盘序号
        /// </summary>
        public string SetOrder { get; set; }

        /// <summary>
        /// 获胜局数
        /// </summary>
        public string WinGames { get; set; }
    }

    /// <summary>
    /// 局的发球球员和左手方球员
    /// </summary>
    public class GameServerLeftModel {
        /// <summary>
        /// 比赛状态，1：进行中，2：已完成
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 当前局的发球方
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// 当前局的左手方球员
        /// </summary>
        public string LeftSide { get; set; }

        /// <summary>
        /// 当前比赛的信息
        /// </summary>
        public WeMatchModel MatchInfo { get; set; }
    }

    /// <summary>
    /// 技术统计的实体
    /// </summary>
    public class UmpPointModel {
        /// <summary>
        /// 比赛主键
        /// </summary>
        public string MatchSys { get; set; }
        /// <summary>
        /// 发球方式
        /// </summary>
        public string ServeType { get; set; }
        /// <summary>
        /// 获胜方
        /// </summary>
        public string Winner { get; set; }
        /// <summary>
        /// 获胜方式
        /// </summary>
        public string WinType { get; set; }
    }
}
