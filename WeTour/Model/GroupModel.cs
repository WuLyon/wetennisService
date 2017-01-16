using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class GroupMemberModel
    {        
        //名次
        public string Order { get; set; }

        //姓名
        public string Name { get; set; }

        public string Memsys { get; set; }

        //场次
        public string MatchQty { get; set; }

        //胜负场
        public string WinLoseMatch { get; set; }

        //胜负局
        public string WinLoseGame { get; set; }
    }

    public class ContGroupModel
    { 
        //小组id
        public string GroupID { get; set; }

        //小组名
        public string GroupName { get; set; }

        //小组成员
        public List<GroupMemberModel> GroupMembers
        { get; set; }
    }
}
