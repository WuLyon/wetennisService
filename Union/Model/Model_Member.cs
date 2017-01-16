using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Union
{
    public class Model_Member
    {
        public string ID { get;set;}
        public string UnionSys { get; set; }
        public string ClubSys { get; set; }
        public string JoinDate { get; set; }
        public string Status { get; set; }

        public string ext1 { get; set; }
        public string ext2 { get; set; }
        public string ext3 { get; set; }
        public string ext4 { get; set; }
        public string ext5 { get; set; }
    }

    public class Model_Union_ClubMember
    {
        public string ClubName { get; set; }
        public string ClubDesc { get; set; }
        public string ClubThumbImage { get; set; }
        public string ContactPerson { get; set; }
        public string ContacTel { get; set; }

        public string JoinDate { get; set; }
    }
}
