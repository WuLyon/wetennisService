using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS
{
    public class CommentModel
    {
        public string ID { get; set; }//编号
        public string TYPE { get; set; }//评论类型
        public string TYPESYSNO { get; set; }//主体主键
        public string MEMSYSNO { get; set; }//评论人员主键
        public string COMMENTS { get; set; }//评论内容
        public string UPDATEDATE { get; set; }//更新日期

        //附加属性
        public string MEMIMG { get; set; }//评论人员头像
        public string MEMNAME { get; set; }//评论人员姓名

        public int GoodQty { get; set; }
        public int BadQty { get; set; }
    }
}
