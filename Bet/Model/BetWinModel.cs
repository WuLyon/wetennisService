using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Member;

namespace Bet
{
    public class BetWinModel
    {
        /// <summary>
        /// 竞猜日期
        /// </summary>
        public string BetEndDate { get; set; }
        /// <summary>
        /// 竞猜主键
        /// </summary>
        public string BetRateID { get; set; }
        /// <summary>
        /// 竞猜描述
        /// </summary>
        public string BetDesc{get;set;}
        /// <summary>
        /// 竞猜正确人员名单
        /// </summary>
        public List<WeMemberModel> WinnerList { get; set; }
    }

    public class BetDateModel
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string BetEndDate { get; set; }

        /// <summary>
        /// 日期内的竞猜项
        /// </summary>
        public List<BetWinModel> BetDateList { get; set; }
    }
}
