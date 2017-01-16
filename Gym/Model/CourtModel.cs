using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gym
{
    /// <summary>
    /// 场地类
    /// </summary>
    [Serializable]
    public class CourtModel
    {
        public string ID { get; set; }//主键
        public string GYMID { get; set; }//场馆编号
        public string COURTNAME { get; set; }//球场名
        public string CTYPE { get; set; }//场馆类型
        //场馆主页的实体
        public string COURTQTY { get; set; }//球场数量
        public string WEEKPRICE { get; set; }//周一至周五价格
        public string ENDPRICE { get; set; }//周末价格
    }
}