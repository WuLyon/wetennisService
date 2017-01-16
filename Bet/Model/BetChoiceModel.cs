using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bet
{
    public class BetChoiceModel
    {
        public string ID { get; set; }
        /// <summary>
        /// 竞猜主记录id
        /// </summary>
        public string BETRATEID { get; set; }
        /// <summary>
        /// 竞猜项，业余比赛用来存放比赛选手的用户id
        /// </summary>
        public string BETCHOICE { get; set; }
        /// <summary>
        /// 竞猜项描述
        /// </summary>
        public string CHOICEDESC { get; set; }
        /// <summary>
        /// 竞猜赔率
        /// </summary>
        public string RATE { get; set; }
    }
}
