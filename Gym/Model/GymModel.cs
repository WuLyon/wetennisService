using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gym
{
    [Serializable]
    public class GymModel
    {
        public string ID { get; set; }//主键
        public string SYS { get; set; }//主键
        public string GYMNAME { get; set; }//场馆名
        public string PROID { get; set; }//省编号
        public string PROVINCE { get; set; }//省
        public string CITYID { get; set; }//城市编号
        public string CITY { get; set; }//城市
        public string REGIONID { get; set; }//区编号
        public string REGION { get; set; }//区
        public string ADDRESS { get; set; }//地址
        public string CONTACT { get; set; }//联系人
        public string CONTACTEL { get; set; }//联系人电话
        public string MEMBERSYS { get; set; }//发现人主键
        public string PRICES { get; set; }
        public string OWNERSYS { get; set; }
        public string ISOWNED { get; set; }
    }
}