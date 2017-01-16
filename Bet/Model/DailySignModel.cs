using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bet
{
    public class DailySignModel
    {
        public string ID { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string MEMSYS { get; set; }
        /// <summary>
        /// 签到日期
        /// </summary>
        public string SIGNDATE { get; set; }
        /// <summary>
        /// 签到时间
        /// </summary>
        public string UPDATETIME { get; set; }
    }
}
