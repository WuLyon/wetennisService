using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    /// <summary>
    /// 记录赛事资源预分配的方案，分配到小组组合，项目，赛事
    /// </summary>
    public class CourtResourceModel
    {
        //主键
        public string id { get; set; }

        public string ResSys{get;set;}

        public string Toursys { get; set; }

        public string Courts { get; set; }

        public string ContentId { get; set; }

        public string Groups { get; set; }

        public string ext1 { get; set; }

        public string ext2 { get; set; }

        public string ext3 { get; set; }

    }
}
