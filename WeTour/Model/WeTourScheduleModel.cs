using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    /// <summary>
    /// 比赛签表
    /// </summary>
    public class WeTourScheduleModel
    {
        /// <summary>
        /// 比赛日期
        /// </summary>
        public string MatchDate { get; set; }

        /// <summary>
        /// 场地比赛
        /// </summary>
        public List<CourtMatchModel> CourtMatches { get; set; }
    }

    /// <summary>
    /// 场地比赛
    /// </summary>
    public class CourtMatchModel
    {
        /// <summary>
        /// 场地编号
        /// </summary>
        public string CourtId{get;set;}

        /// <summary>
        /// 场地名称
        /// </summary>
        public string CourtName{get;set;}

        /// <summary>
        /// 场馆id
        /// </summary>
        public string Gymid { get; set; }

        /// <summary>
        /// 场馆名称
        /// </summary>
        public string GymName { get; set; }
        /// <summary>
        /// 比赛实体
        /// </summary>
        public List<WeMatchModel> CourtMatches {get;set;}
    }
}
