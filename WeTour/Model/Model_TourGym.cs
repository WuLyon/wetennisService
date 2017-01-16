using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class Model_TourGym
    {
        public string ID { get; set; }
        public string TourSys { get; set; }
        public string GymSys { get; set; }
    }

    public class Model_TourGymCourts
    {
        public string ID { get; set; }
        public string TourSys { get; set; }
        public string GymSys { get; set; }

        public string CourtId { get; set; }
    }

    /// <summary>
    /// 页面5-5-1.html ,赛事的场馆列表
    /// </summary>
    public class TourGymList {
        public string gymSys { get; set; }

        public string gymImgUrl { get; set; }

        public string gymName { get; set; }

        public string courtInfo { get; set; }
    }

    /// <summary>
    /// 5-5-1-1页面使用的场馆场地信息。
    /// </summary>
    public class Model_Dist_GymCourts
    {
        public string courtId { get; set; }
        public string courtName { get; set; }
        public string courtType { get; set; }
        public string isCheck { get; set; }
    }

    /// <summary>
    /// 表实体，在一个场馆要举行的赛事项目
    /// </summary>
    public class Model_GymContents
    {
        public string Id { get; set; }
        public string TourSys { get; set; }
        public string GymSys { get; set; }
        public string ContId { get; set; }
    }

    /// <summary>
    /// 赛事管理页面5-5-1-2，前端实体
    /// </summary>
    public class Model_Dist_GymCont
    {
        public string contId { get; set; }
        public string contName { get; set; }
        
    }

}
