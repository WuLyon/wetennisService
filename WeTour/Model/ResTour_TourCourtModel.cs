using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class ResTour_TourCourtModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 场馆编号
        /// </summary>
        public string GYMID { get; set; }
        /// <summary>
        /// 赛事主键
        /// </summary>
        public string TOURSYS { get; set; }
        /// <summary>
        /// 场地编号
        /// </summary>
        public string COURTID { get; set; }
        /// <summary>
        /// 是否中心场
        /// </summary>
        public string ISCENTER { get; set; }
        /// <summary>
        /// 备用1
        /// </summary>
        public string EXT1 { get; set; }
        /// <summary>
        /// 备用2
        /// </summary>
        public string EXT2 { get; set; }
        /// <summary>
        /// 备用3
        /// </summary>
        public string EXT3 { get; set; }
        /// <summary>
        /// 备用4
        /// </summary>
        public string EXT4 { get; set; }
        /// <summary>
        /// 备用5
        /// </summary>
        public string EXT5 { get; set; }

    }

    public class Model_Schedule_Adjust {
        /// <summary>
        /// 场地排布情况
        /// </summary>
        public object courts { get; set; }
        /// <summary>
        /// 所有比赛
        /// </summary>
        public object matches { get; set; }
        /// <summary>
        /// 所有人员
        /// </summary>
        public object players { get; set; }
    }

    public class Model_req_adjsutSchedule
    {
        public List<Model_req_adjsutSchedule_item> courts { get; set; }
    }

    public class Model_req_adjsutSchedule_item
    {
        public string id { get; set; }
        public List<string> matches { get; set; }
    }
}
