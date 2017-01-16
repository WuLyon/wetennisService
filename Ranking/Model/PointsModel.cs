using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ranking
{
    [Serializable]
    public class PointsModel
    {
        public string ID { get; set; }

        /// <summary>
        /// Player'sys
        /// </summary>
        public string MemSys { get; set; }

        /// <summary>
        /// type of point, wetennis' or club's
        /// </summary>
        public string PointType { get; set; }

        /// <summary>
        /// paired with pointtype, if point type is club's, the typesys is a club's sysno
        /// </summary>
        public string TypeSys { get; set; }

        /// <summary>
        /// absolute point
        /// </summary>
        public string Points { get; set; }

        /// <summary>
        /// to distinguish single or double
        /// </summary>
        public string IsSingle { get; set; }

        /// <summary>
        /// Gender, and get from the contentname
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// Tour sysno
        /// </summary>
        public string TourSys { get; set; }

        /// <summary>
        /// tour's content
        /// </summary>
        public string ContentId { get; set; }

        /// <summary>
        /// Remark of Each point record
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Modify date
        /// </summary>
        public string UpdateDate { get; set; }
    }
}
