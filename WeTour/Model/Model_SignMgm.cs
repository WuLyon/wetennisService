using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class Model_SignedApp
    {
        //签位号
        public string signorder { get; set; }

        //签位对应的人员
        public List<Model_SignMember> signmember { get; set; }
    }

    public class Model_SignMember
    {
        //会员主键
        public string memsys { get; set; }

        //会员姓名
        public string name { get; set; }
    }
}
