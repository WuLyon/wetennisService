using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS
{
    [Serializable]
    public class TrendModel
    {
        public string TRID { get; set; }//动态id
        public string WTRIMG { get; set; }//头像
        public string WTRSYSNO { get; set; }//主键
        public string WTRNAME { get; set; }//姓名
        public string WTRDATE { get; set; }//日期
        public string CONTENT { get; set; }//内容
        public string APPLOUSE { get; set; }//点赞数
        public string COMMENTS { get; set; }//动态评论
    }
}