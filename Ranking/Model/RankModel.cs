using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ranking
{
    [Serializable]
    public class RankModel
    {
        public string ID { get; set; }

        /// <summary>
        /// Player'sys
        /// </summary>
        public string MemSys { get; set; }

        /// <summary>
        /// type of rank, wetennis' or club's
        /// </summary>
        public string RankType { get; set; }

        /// <summary>
        /// paired with pointtype, if point type is club's, the typesys is a club's sysno
        /// </summary>
        public string TypeSys { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Rank { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string RankPoint { get; set; }

        /// <summary>
        /// single or doublr
        /// </summary>
        public string IsSingle { get; set; }

        /// <summary>
        /// Men or Women
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// boy,younghth,midle, old
        /// </summary>
        public string GroupType { get; set; }

        /// <summary>
        /// Ranking Count Year
        /// </summary>
        public string Ryear { get; set; }

        /// <summary>
        /// Ranking Count week
        /// </summary>
        public string Rweek { get; set; }

        /// <summary>
        /// Update Date
        /// </summary>
        public string Updatedate { get; set; }

        /// <summary>
        /// Status of Rank:1 current,0 past;
        /// </summary>
        public string Status { get; set; }
    }
}
