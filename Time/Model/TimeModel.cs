using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Time
{
    public class TimeModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 时光主键
        /// </summary>
        public string SYS { get; set; }

        /// <summary>
        /// 会员主键
        /// </summary>
        public string MEMSYS { get; set; }

        /// <summary>
        /// 时光类型：0:非比赛，1：友谊赛，2：赛事比赛
        /// </summary>
        public string TYPE { get; set; }

        /// <summary>
        /// 比赛主键
        /// </summary>
        public string MATCHSYS { get; set; }

        /// <summary>
        /// 时光描述
        /// </summary>
        public string DESCRIPTION { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string UPDATETIME { get; set; }

        /// <summary>
        /// 时光状态.1：公开，0：隐私
        /// </summary>
        public string EXT1 { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        public string EXT2 { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        public string EXT3 { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        public string EXT4 { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        public string EXT5 { get; set; }
    }
}
