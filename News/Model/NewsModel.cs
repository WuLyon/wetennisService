using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace News
{
    /// <summary>
    /// 实体
    /// </summary>
    public partial class NewsModel
    {
        public string ID { get; set; }
        public string TYPE { get; set; }
        public string TITLE { get; set; }
        public string ISSUETIME { get; set; }
        public string WRITER { get; set; }
        public string SMALLURL { get; set; }
        public string IMGURL { get; set; }
        public string STATUS { get; set; }
        public string FORURL { get; set; }
        public string EXT1 { get; set; }
        public string EXT2 { get; set; }
        public string EXT3 { get; set; }

        public string SYSNO { get; set; }
    }


}
