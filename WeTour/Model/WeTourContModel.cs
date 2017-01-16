using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    [Serializable]
    public class WeTourContModel
    {
        public string id
        { get; set; }

        public string ContSys { get; set; }

        public string Toursys
        { get; set; }

        public string ContentName
        { get; set; }
        public string ContentType
        { get; set; }

        public string SignQty
        { get; set; }

        public string AllowGroup
        { get; set; }

        public string GroupType
        { get; set; }

        /// <summary>
        /// 组别，用来表示青少年组，青年组，中年组，老年组的区别
        /// </summary>
        public string TourDate
        { get; set; }

        //裁判的sysno
        public string ext1
        { get; set; }

        //若该项目是团体赛，就代表团体赛的内容，用/隔开
        public string ext2
        { get; set; }

        //报名费，如果此字段不为0，则读取赛事中的报名费
        public string ext3
        { get; set; }

        //计算赛事积分的系数
        public string ext4
        { get; set; }

        //子项报名情况
        public string ApplyInfo
        {
            get;
            set;
        }
        /// <summary>
        /// 备用字段5：报名年龄限制，合计最大年龄|最大年龄|最小年龄
        /// </summary>
        public string ext5 { get; set; }
        /// <summary>
        /// 备用字段6
        /// </summary>
        public string ext6 { get; set; }
        /// <summary>
        /// 备用字段7
        /// </summary>
        public string ext7 { get; set; }
        /// <summary>
        /// 备用字段8
        /// </summary>
        public string ext8 { get; set; }
        /// <summary>
        /// 备用字段9
        /// </summary>
        public string ext9 { get; set; }
        /// <summary>
        /// 备用字段10
        /// </summary>
        public string ext10 { get; set; }
    }
}
