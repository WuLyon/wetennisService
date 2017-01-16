using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ranking
{
    /// <summary>
    /// 联盟排名概况
    /// </summary>
    public class Model_GroupType
    {
        /// <summary>
        /// 组别名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 组别所包含的项目
        /// </summary>
        public string[] ContType { get; set; }
    }

    /// <summary>
    /// 联盟排名用户排名
    /// </summary>
    public class Model_GroupDetail
    {
        public string GroupName { get; set; }
        public string ContType { get; set; }
        public List<Model_MemRanking> MemRanking { get; set; }
    }

    public class Model_MemRanking {
        /// <summary>
        /// 排名
        /// </summary>
        public string Rank { get; set; }
        /// <summary>
        /// 用户主键
        /// </summary>
        public string MemSys { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        public string UserName { get; set; }
        public string MemImg { get; set; }

        public string Points { get; set; }

        public string ClubName { get; set; }
    }
}
