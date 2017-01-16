using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class WeTourResultModel
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public string ContentId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ContentName { get; set; }
        /// <summary>
        /// 项目比赛结果
        /// </summary>
        public List<WeMatchModel> WeTourResult { get; set; }
    }
}
