using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bet
{
    public class BetRateModel
    {
        public string ID { get; set; }
        /// <summary>
        /// 针对业余赛事的比赛主键
        /// </summary>
        public string MATCHSYS { get; set; }
        /// <summary>
        /// 针对业余赛事，竞猜类型，一般为胜负
        /// </summary>
        public string BETNAME { get; set; }
        /// <summary>
        /// 竞猜状态，开始和完成，或者删除
        /// </summary>
        public string STATUS { get; set; }
        /// <summary>
        /// 竞猜的详细描述，可以兼容其它格式的竞猜
        /// </summary>
        public string BETDESCRIPTION { get; set; }
        /// <summary>
        /// 竞猜类型
        /// </summary>
        public string BETTYPE { get; set; }
        /// <summary>
        /// 竞猜标签
        /// </summary>
        public string BETTAG { get; set; }
        /// <summary>
        /// 奖励类型，默认为积分，也可以额外针对猜对的给与实体奖励
        /// </summary>
        public string PRIZETYPE { get; set; }
        /// <summary>
        /// 创建者，如果是业余赛事，就为业余赛事举办者
        /// </summary>
        public string CREATOR { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CREATETIME { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string ENDTIME { get; set; }

        /// <summary>
        /// 竞猜结果
        /// </summary>
        public string BETANSWER { get; set; }

        /// <summary>
        /// 竞猜主键
        /// </summary>
        public string SYSNO { get; set; }
    }
}
