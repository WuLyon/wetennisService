using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class Fe_Model_Filters
    {
        public object location { get; set; }

        public object status { get; set; }
    }

    public class Fe_Model_Key_Value
    {
        public string text { get; set; }

        public string value { get; set; }
    }

    /// <summary>
    /// 赛事列表
    /// </summary>
    public class Fe_Model_EventList {
        public string id { get; set; }
        public string name { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string startDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string endDate { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 地点
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// 标志图
        /// </summary>
        public string thumb { get; set; }

    }

    /// <summary>
    /// 赛事详情
    /// </summary>
    public class Fe_Model_EventInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string type { get; set; }
        public string location { get; set; }
        /// <summary>
        /// 赛事倒计时，以毫秒为单位的时间戳
        /// </summary>
        public long countdown { get; set; }
        public string banner { get; set; }
        public string thumb { get; set; }      
        public int friendRegisterCount { get; set; }
       
        public int drawCountdown { get; set; }
        /// <summary>
        /// 已报名的比赛
        /// </summary>
        public object registerList { get; set; }

        public string state { get; set; }

        public bool follow { get; set; }


        /// <summary>
        /// 判断是否是团体赛
        /// </summary>
        public bool isGroup { get; set; }
        /// <summary>
        /// 赛事说明链接
        /// </summary>
        public string eventDetailLink { get; set; }
    }

    public class rigisterCont {
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Fe_Model_eventSponsors {
        public string name { get; set; }
        public string thumb { get; set; }
    }

    public class Fe_Model_eventDrawKO {
        public string username { get; set; }
        public string userimage { get; set; }
        public bool win { get; set; }
        public int score { get; set; }
    }

    public class Fe_Model_DrawResult {
        public string name { get; set; }
        public List<Fe_Model_RoundMatches> round { get; set; }
    }

    public class Fe_Model_RoundMatches {
        public string roundName { get; set; }
        public List<Fe_Model_MatchMain> roundmatches { get; set; }
    }

    public class Fe_Model_MatchMain {
        public List<Fe_Model_member> player1 { get; set; }
        public int score1{get;set;}
        public List<Fe_Model_member> player2 { get; set; }
        public int score2 { get; set; }
    }

    public class Fe_Model_member {
        public string memberName { get; set; }
        public string memberEnglishName{get;set;}
        public string memberThumb { get; set; }
    }

    #region 报名
    public class Fe_Model_eventGroups {
        public object groups { get; set; }
    }

    public class Fe_Model_eventGroups_Group {
        public string id { get; set; }

        public string name { get; set; }

        public decimal price { get; set; }

        public object items { get; set; }
    }
    public class Fe_Model_eventGroups_items {
        public string id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public bool needPartner { get; set; }
        public object restriction { get; set; }
    }

    public class Fe_Model_eventGroups_item_restriction {
        public int minAmountAge { get; set; }
        public int maxAge { get; set; }
        public int minAge { get; set; }
        public string gender { get; set; }
        public bool isMixedPair { get; set; }
    }

    public class Fe_Model_registeredUsers {
        public string[] name { get; set; }
        public string[] thumbImgUrl { get; set; }
        public string registerDatestr { get; set; }
    }

    /// <summary>
    /// 赛事签表
    /// </summary>
    public class Fe_Model_DrawTable {
        public bool qualify { get; set; }
        public string gamename { get; set; }
        public object matchs { get; set; }
        public object details { get; set; }
    }

    public class Fe_Model_DrawTable_Qualify {
        public bool complete { get; set; }
        public string groupname { get; set; }
        public object games { get; set; }
    }

    public class Fe_Model_DrawTable_Qualify_games {
        public object users { get; set; }
        public bool qulified { get; set; }
        public int gameNumber { get; set; }
        public string criticalSpot { get; set; }
        public string criticalGame { get; set; }
    }

    public class Fe_Model_DrawTable_Qualify_games_member {
        public string username { get; set; }
        public string userimage { get; set; }
    }

    public class Fe_Model_DrawTable_knockOut_Player {
        public int score { get; set; }
        public bool win { get; set; }
        public object users { get; set; }
    }

    public class Fe_Model_DrawTable_knockOut_game {
        public List<Fe_Model_DrawTable_knockOut_Player> game { get; set; }
    }

    public class Fe_Model_DrawTable_knockOut_game_pair
    {
        public List<Fe_Model_DrawTable_knockOut_game> games { get; set; }
    }

    //赛程相关实体
    public class Fe_Model_eventScheduleFilter {
        public object location { get; set; }
        public object date { get; set; }
    }

    public class Fe_Model_eventScheduleFilter_item
    {
        public string text { get; set; }
        public string value { get; set; }
        public object locations { get; set; }
    }

    #endregion

    #region 比赛统计
    public class Fe_fetchMyMatch {
        public string single_ytdwl { get; set; }
        public string single_totalwl { get; set; }
        public string couple_ytdwl { get; set; }
        public string couple_totalwl { get; set; }
        public object singleMatch { get; set; }
        public object coupleMatch { get; set; }
    }

    /// <summary>
    /// 我的约球统计
    /// </summary>
    public class Fe_fetchMyPractice
    {
        public string ytdwl { get; set; }
        public string totalwl { get; set; }
        public object singlePractice { get; set; }
        public object couplePractice { get; set; }
    }

    public class Fe_fetchMyMatch_singleMatch {
        public string title { get; set; }
        public string subtitle { get; set; }
        public int pts { get; set; }
        public object games { get; set; }
    }

    public class Fe_fetchMyMatch_single_game {
        public string gameTime { get; set; }
        public string matches { get; set; }
        public object teams { get; set; }
    }
    public class Fe_fetchMyMatch_single_game_team
    {
        public bool win { get; set; }
        public int score { get; set; }
        public object users { get; set; }
    }
    public class Fe_fetchMyMatch_signle_game_team_users
    {
        public string username { get; set; }
        public string userimage { get; set; }
    }

    public class Fe_Model_Filter {
        public string text { get; set; }
        public string value { get; set; }
        public object children { get; set; }
    }

    public class Fe_Model_Filter_child {
        public string text { get; set; }
        public string value { get; set; }
    }
    #endregion

    #region 赛程详情
    public class Fe_Model_Schedule {
        public string matchId { get; set; }
        public string matches { get; set; }
        public string group { get; set; }
        public bool completed { get; set; }
        public string gameTime { get; set; }
        public string date { get; set; }
        public string location { get; set; }
        public object team { get; set; }

    }

    public class Fe_Model_Result
    {
        public string id { get; set; }
        public string matches { get; set; }
        public string group { get; set; }
        public bool completed { get; set; }
        public string gameTime { get; set; }
        public int type { get; set; }
        public int status { get; set; }
        public object team { get; set; }

    }

    public class Fe_Model_Schedule_team
    {
        public object users { get; set; }
        public bool win { get; set; }
        public int score { get; set; }
        public string winGameNumber { get; set; }
        public string currentScore { get; set; }
        public bool currentScoreWin { get; set; }
        public bool first { get; set; }
    } 
    
    #endregion

    #region 比赛详情
    /// <summary>
    /// 比赛详情实体
    /// </summary>
    public class Fe_Model_MatchInfo {
        public int status { get; set; }
        public string eventName{get;set;}
        public string matchName { get; set; }
        public string datetime { get; set; }
        public string location { get; set; }
        public string matchDuration { get; set; }
        public object teams { get; set; }
        public object sets { get; set; }
        public object scoreDetails { get; set; }
    }
    #endregion

    #region 团体报名
    public class Fe_Model_GroupApplicants {
        public string eventId { get; set; }
        public string name { get; set; }
        public string groupId { get; set; }
        public string coachName { get; set; }
        public List<Fe_Model_GroupApplicants_member> members { get; set; }
    }
    public class Fe_Model_GroupApplicants_member {
        public string name { get; set; }
        public string gender { get; set; }
        public string identityCard { get; set; }
        public string identity { get; set; }
        public bool isBench { get; set; }
    }

    public class Fe_Model_GroupMember
    {
        public string name { get; set; }
        public string gender { get; set; }
        public string identityCard { get; set; }
        public string passport { get; set; }
        public string id { get; set; }
        public bool isBench { get; set; }
    }

    public class Fe_Model_GroupInfo
    {
        public string name { get; set; }
        public string coachName { get; set; }
        public string registerDate { get; set; }
        public string groupName { get; set; }
    }

    public class Fe_Model_MatchTeamSequ {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> teamMembers { get; set; }
        public string restrictions { get; set; }
    }

    public class Fe_Model_MatchTeamSchedule {
        public string matchId { get; set; }

        public List<Fe_Model_MatchTeamSequ> sechedule { get; set; }
    }
    #endregion
}
