using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Time
{
    public class TimePicsModel
    {
        /// <summary>
        /// 时光照片id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 时光主键
        /// </summary>
        public string TIMESYS { get; set; }
        /// <summary>
        /// 照片URL
        /// </summary>
        public string PICURL { get; set; }
        /// <summary>
        /// 更新日期
        /// </summary>
        public string UPDATETIME { get; set; }
        /// <summary>
        /// 备用字段
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
