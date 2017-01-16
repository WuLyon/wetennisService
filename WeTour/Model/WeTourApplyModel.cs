using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class WeTourApplyModel
    {
        public string ID { get; set; }
        public string TOURSYS { get; set; }
        public string CONTENTID { get; set; }
        /// <summary>
        /// 报名类型，单打报名，双打报名，团体赛报名
        /// </summary>
        public string memtype { get; set; }
        /// <summary>
        /// 报名主键
        /// </summary>
        public string MEMBERID { get; set; }
        public string STATUS { get; set; }
        public string APPLYDATE { get; set; }
        public string TOURTYPE { get; set; }
        public string PATERNER { get; set; }
        public string EXT1 { get; set; }
        //对应订单主键
        public string EXT2 { get; set; }
        //报名方的积分情况
        public string EXT3 { get; set; }

        //附加属性
        public string CONTENTNAME { get; set; }
        public string ApplyName { get; set; }
        public string ApplySysno { get; set; }
        public int ApplyScore { get; set; }//报名双方的积分
        public string MemberName { get; set; }
        public string MemberImg { get; set; }
        public string ParName { get; set; }
        public string ParImg { get; set; }
    }
}
