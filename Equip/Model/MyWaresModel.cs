using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Equip
{
    public class MyWaresModel
    {
        /// <summary>
        /// 标识id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 个人主键
        /// </summary>
        public string Memsysno { get; set; }
        public string WareId { get; set; }
        public string StartDate { get; set; }
        /// <summary>
        /// 标识，9，表示为前端创建的个人装备
        /// </summary>
        public string IsUsual { get; set; }
        public string IsActive { get; set; }
        /// <summary>
        /// 装备图片地址
        /// </summary>
        public string CustPro1 { get; set; }
        /// <summary>
        /// logo，品牌名
        /// </summary>
        public string CustPro2 { get; set; }
        /// <summary>
        /// size,尺码
        /// </summary>
        public string CustPro3 { get; set; }
        /// <summary>
        /// prize,价格
        /// </summary>
        public string CustPro4 { get; set; }
    }
}
