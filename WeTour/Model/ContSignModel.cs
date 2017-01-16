using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    /// <summary>
    /// 子项签表的实体
    /// </summary>
    public class ContSignModel
    {
        //子项id
       public  string ContentID { get; set; }
        //项目名称
       public string ContentName { get; set; }
        //比赛轮次
       public string Round { get; set; }
        //轮次名称
       public string RoundName { get; set; }
        //小组排名情况
       public List<ContGroupModel> Groups { get; set; }
        /// <summary>
        ///淘汰赛比赛
        /// </summary>
       public List<WeMatchModel> KnockOuts { get; set; }
    }

    //赛事签表
    public class WeTourSignsModel {
        //子项id
        public string ContentID { get; set; }

        //项目名称
        public string ContentName { get; set; }

        //项目签表
        public List<ContSignModel> contSigns { get; set; }
    }
}
