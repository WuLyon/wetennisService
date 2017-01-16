using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourMethod
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class TourApplyModel
    {
        public string ID { get; set; }
        public string TOURSYS { get; set; }
        public string CONTENTID { get; set; }
        public string MEMBERID { get; set; }
        public string STATUS { get; set; }
        public string APPLYDATE { get; set; }
        public string TOURTYPE { get; set; }
        public string PATERNER { get; set; }
        public string EXT1 { get; set; }
        public string EXT2 { get; set; }
        public string EXT3 { get; set; }

        //附加属性
        public string CONTENTNAME { get; set; }
    }
}