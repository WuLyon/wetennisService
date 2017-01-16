using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeTennisService
{
    public class Model_Return
    {
        public int code { get; set; }
        public string errorMsg { get; set; }
        /// <summary>
        /// 返回的数据
        /// </summary>
        public object data { get; set; }
    }


    public class Model_req_signup
    {
        public string method { get; set; }
        public string password { get; set; }
        public string userName { get; set; }
        public string username { get; set; }
        public string phone { get; set; }
        public string activationCode { get; set; }
    }

    public class Model_req_event
    {
        public string status { get; set; }
        public string eventFilter { get; set; }
        public string location { get; set; }
        public string currentPage { get; set; }
        public string limit { get; set; }
        public string eventId { get; set; }
    }

    public class Model_req_schedule
    {
        public string eventId { get; set; }
        public string date { get; set; }
        public string location { get; set; }
    }

   





    public class Model_req_eventDetail {
        public string id { get; set; }
        public string userId { get; set; }
    }

    public class Model_req_comment
    {
        public string id { get; set; }
        public string userId { get; set; }
        public string type { get; set; }
        public string content { get; set; }
    }

    public class Model_req_me
    {
        public string userId { get; set; }
        public string ImageUrl { get; set; }
    }

    public class Model_req_Register
    {
        public string eventId { get; set; }

        public string itemId { get; set; }
        public string userid { get; set; }
        public string id { get; set; }
        public string gender { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string personCard { get; set; }
        public string partnerId { get; set; }

        //2016-10-12添加club,company,title
        public string club { get; set; }
        public string company { get; set; }
        public string title { get; set; }
    }

    public class Model_req_ranking {
        public string rankId { get; set; }
        public string value { get; set; }
        public string currentPage { get; set; }
        public string limit { get; set; }
        public string userId { get; set; }
    }

    public class Model_req_draw {
        public string itemId { get; set; }
        public string round { get; set; }
    }

    public class Model_req_result
    {
        public string itemId { get; set; }
        public string status { get; set; }
    }

    public class Model_req_Image
    {
        public string imgstr { get; set; }
        public string name { get; set; }
    }

    public class Model_req_cascadeFilter
    {
        public string id { get; set; }
        public string type { get; set; }
    }

    public class Model_req_MatchScore
    {
        public string sys { get; set; }
        public string p1s { get; set; }
        public string p2s { get; set; }
        
    }

    public class Model_req_Match
    {
        public string matchId { get; set; }
    }

    /// <summary>
    /// 个人装备
    /// </summary>
    public class Model_req_Equipment {
        public string id { get; set; }
        public string imgUrl { get; set; }
        public string logo { get; set; }
        public string size { get; set; }
        public string price { get; set; }
    }
}