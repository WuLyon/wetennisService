using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS
{
    public class TeleCheckModel
    {
        public string ID { get; set; }
        //电话号码
        public string TELEPHONE { get; set; }
        //验证码
        public string VALIDATECODE { get; set; }
        //验证码状态0：代表待验证，9：代表失效，1：代表验证没有通过；2：代表验证通过
        public string STATUS { get; set; }
        /// <summary>
        /// 代表获取时间
        /// </summary>
        public string EXT1 { get; set; }
        public string EXT2 { get; set; }
        public string EXT3 { get; set; }
    }
}
