using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeTour;
using SMS;

namespace Time
{
    public class TimesBll
    {
        public static TimesBll instance = new TimesBll();

        /// <summary>
        /// 根据会员主键，获得时光,IsOpen=1,表示仅获得公开时光，否则获得全部时光。
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public List<MyTimeModel> GetMyTimeList(string _Memsys,string _IsOpen)
        {
            List<MyTimeModel> list = new List<MyTimeModel>();
            List<TimeModel> tlist = TimeDAL.instance.GetListbyMemsys(_Memsys,_IsOpen);
            foreach (TimeModel model in tlist)
            {
                MyTimeModel mmodel = new MyTimeModel();
                mmodel.TypeSys = model.SYS;
                mmodel.UpdateTime = model.UPDATETIME;
                switch (model.TYPE)
                { 
                    case "0":
                        mmodel.TypeName = "重要时刻";
                        break;
                    case "1":
                        mmodel.TypeName = "友谊赛";
                        mmodel.MatchModel = WeMatchDll.instance.RenderMatch(model.MATCHSYS);
                        break;
                    case "2":
                        mmodel.TypeName = "正式比赛";
                        mmodel.MatchModel = WeMatchDll.instance.RenderMatch(model.MATCHSYS);
                        break;
                }
                //添加描述
                mmodel.Description = model.DESCRIPTION;

                //加载时光图片
                mmodel.TimePics = TimePicDal.instance.GetTimePics(model.SYS);

                //加载点赞数量
                mmodel.LikeQty = PraiseBll.instance.CountPraiseQty("Times",model.SYS,"1").ToString();

                //加载评论内容
                mmodel.TimeComments = CommentBll.instance.GetComList(model.SYS, "Times");

                list.Add(mmodel);
            }
            return list;
        }
    }
}

