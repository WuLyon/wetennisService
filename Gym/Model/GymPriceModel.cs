using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gym
{
    /// <summary>
    /// 场馆价格实体
    /// </summary>
    [Serializable]    
    public class GymPriceModel
    {
        public string ID { get; set; }//主键
        public string GYMID { get; set; }//场馆主键
        public string COURTTYPE { get; set; }//场地类型
        public string ISWEEKDAY { get; set; }//是否周末，1表示周一至周五；2表示周末
        public string TIME { get; set; }//时段：上午，下午，晚上
        public string PRICE { get; set; }//单价元/小时
    }
}