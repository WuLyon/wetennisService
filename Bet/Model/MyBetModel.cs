using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bet
{
    public class MyBetModel
    {
        public string ID { get; set; }
        /// <summary>
        /// 竞猜主键
        /// </summary>
        public string BETRATEID { get; set; }
        /// <summary>
        /// 选项id
        /// </summary>
        public string BETCHOICEID { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string MEMSYS { get; set; }
        /// <summary>
        /// 投注数量
        /// </summary>
        public string BETQTY { get; set; }
        /// <summary>
        /// 奖励数量
        /// </summary>
        public string RETURNQTY { get; set; }
        /// <summary>
        /// 投注日期
        /// </summary>
        public string CREATEDATE { get; set; }
        /// <summary>
        /// 状态(1:新购竞猜;2:竞猜成功;3:竞猜取消;4:竞猜失败
        /// 
        /// </summary>
        public string STATUS { get; set; }

    }
}
