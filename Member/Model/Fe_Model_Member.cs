using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Member
{
    public class Fe_Model_Me
    {
        public string name { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public string Constellation { get; set; }
        public int friendsNum { get; set; }
        public int attentionsNum { get; set; }
        public int score { get; set; }
        public int gamesNum { get; set; }
        public int guessNum { get; set; }

        public string backGroundImageUrl { get; set; }
        public object equipment { get; set; }
    }

    public class Fe_Model_equipment
    {
        public string id { get; set; }
        public string imgUrl { get; set; }
        public string logo { get; set; }
        public decimal size { get; set; }
        public decimal price { get; set; }
    }

    public class Fe_Model_Comment
    {
        public int total{get;set;}

        public object comments{get;set;}
    }

    public class Fe_comment
    {
        public string id { get; set; }
        public string username { get; set; }
        public string userimage { get; set; }
        public string time { get; set; }
        public string context { get; set; }
        public bool like { get; set; }
        public int likenumber { get; set; }

    }

    #region 用户
    public class Model_Fe_SignUp
    {
        public string id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
    }

    public class Model_Fe_Signin {
        public string id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string gender { get; set; }
        public string cardId { get; set; } 
    }

    public class Model_Fe_UserInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string gender { get; set; }
        public string cardId { get; set; }
    }

    public class Model_Fe_userNameDuplicated {
        public bool userNameDuplicated { get; set; }
    }

    public class Model_Fe_phoneDuplicated
    {
        public bool phoneDuplicated { get; set; }
    }

    #endregion


    #region 报名
    public class Fe_Model_fetchPartners
    {
        public string id { get; set; }

        public string name { get; set; }
        public string username { get; set; }
        public string phone { get; set; }
        public int age { get; set; }
    }   
    #endregion

    #region 个人信息
    public class Fe_Model_updateMySettings {
        public string method { get; set; }
        public string userId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public string Rank { get; set; }
        public decimal TechRank { get; set; }
        public decimal SelfTechRank { get; set; }
        public decimal OtherTechRank { get; set; }
        public int startYear { get; set; }
        public int hand { get; set; }
        public int habit { get; set; }
        public decimal height{get;set;}
        public decimal weight{get;set;}

        public string club { get; set; }
        public string company { get; set; }
        public string companyName { get; set; }
        public string companyTitle { get; set; }
        public List<Fe_Model_updateMySettings_address> address { get; set; }

        //2016-12-16，
        public string cardId { get; set; }
        public string phone { get; set; }
    }

    public class Fe_Model_updateMySettings_address {
        public string name { get; set; }
        public string phone { get; set; }
        public string district { get; set; }
        public string detailAddress { get; set; }
        public string post { get; set; }
    }

    public class Fe_Model_Req_resetPassword {
        public string method { get; set; }
        public string phone { get; set; }
        public string activationCode { get; set; }
        public string password { get; set; }
    }
    #endregion

}
