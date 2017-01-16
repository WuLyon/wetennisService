using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeTour;
using SMS;

namespace Time
{
    public class MyTimeModel
    {
        public string TypeSys { get; set; }

        /// <summary>
        /// 时光发布日期
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 时光类型
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 比赛实体
        /// </summary>
        public WeMatchModel MatchModel { get; set; }

        /// <summary>
        /// 时光描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 时光图片列表
        /// </summary>
        public List<TimePicsModel> TimePics { get; set; }

        /// <summary>
        /// 时光点赞数量
        /// </summary>
        public string LikeQty { get; set; }

        /// <summary>
        /// 时光评论数量
        /// </summary>
        public List<CommentModel> TimeComments { get; set; }

    }
}
