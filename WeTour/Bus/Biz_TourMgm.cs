using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class Biz_TourMgm
    {
        public static Biz_TourMgm instance = new Biz_TourMgm();

        /// <summary>
        /// 获取外部展示所需要的赛事列表
        /// </summary>
        /// <param name="_Clubsys"></param>
        /// <param name="_Status"></param>
        /// <returns></returns>
        public List<Model_ApiTourList> GetTourList(string _Clubsys, string _Status,string _TourType)
        {
            List<Model_ApiTourList> list = new List<Model_ApiTourList>();
            List<WeTourModel> tourlist = WeTourDll.instance.GetWeTourListbyStatus(_Clubsys, _TourType, _Status);
            if (tourlist.Count > 0)
            {
                foreach (WeTourModel tmodel in tourlist)
                { 
                    //逐个赛事整理，并添加到输出list中
                    Model_ApiTourList model = new Model_ApiTourList();
                    model.TourSys = tmodel.SYSNO;
                    model.TourName = tmodel.NAME;
                    model.TourDate = tmodel.STARTDATE;
                    model.TourAddress = tmodel.ADDRESS;
                    model.Status = tmodel.STATUS;
                    model.StatusDesc = RenderTourStatus(tmodel.STATUS);
                    model.host = tmodel.Host;
                    model.asso_host = tmodel.Asso_Host;

                    //添加赛事图片
                    if (tmodel.TOURIMG.IndexOf("wetennis.cn") < 0)
                    {
                        model.TourImg = "http://wetennis.cn" + tmodel.TOURIMG;
                    }
                    else
                    {
                        model.TourImg = tmodel.TOURIMG;
                    }

                    //添加赛事类型及图片
                    switch (tmodel.CITYTYPE)
                    { 
                        case "":
                            model.TourType = "公开赛";
                            model.TourTypeImg = "http://wetennis.cn:86/Common/TourType_gks.png";
                            break;
                        case "Club":
                            model.TourType = "俱乐部赛";
                            model.TourTypeImg = "http://wetennis.cn:86/Common/TourType_jlb.png";
                            break;
                        case "Union":
                            model.TourType = "联盟赛";
                            model.TourTypeImg = "http://wetennis.cn:86/Common/TourType_lms.png";
                            break;
                    }

                    //添加赛事控制内容
                    model.Tour_Controls = GetTourControl(tmodel.STATUS);

                    //获得赞助商信息
                    model.advertise = Biz_TourAdviser.instance.GetAdvertiserbyToursys(tmodel.SYSNO);

                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 后台管理获取赛事列表，增加了会员抽签状态值
        /// 1：正在报名，2：报名结束，3：会员抽签；4：排秩序册，5：正在比赛，6：比赛完成
        /// 2016-9-19，刘涛
        /// </summary>
        /// <param name="_Clubsys"></param>
        /// <param name="_Status"></param>
        /// <param name="_TourType"></param>
        /// <returns></returns>
        public List<Model_ApiTourList> GetTourList_Sep(string _Clubsys, string _Status, string _TourType)
        {
            List<Model_ApiTourList> list = new List<Model_ApiTourList>();
            List<WeTourModel> tourlist = WeTourDll.instance.GetWeTourListbyStatus(_Clubsys, _TourType, _Status);
            if (tourlist.Count > 0)
            {
                foreach (WeTourModel tmodel in tourlist)
                {
                    //逐个赛事整理，并添加到输出list中
                    Model_ApiTourList model = new Model_ApiTourList();
                    model.TourSys = tmodel.SYSNO;
                    model.TourName = tmodel.NAME;
                    model.TourDate = tmodel.STARTDATE;
                    model.TourAddress = tmodel.ADDRESS;
                    model.Status = tmodel.STATUS;
                    model.StatusDesc = RenderTourStatus_Sep(tmodel.STATUS);
                    model.host = tmodel.Host;
                    model.asso_host = tmodel.Asso_Host;

                    //添加赛事图片
                    if (tmodel.TOURIMG.IndexOf("wetennis.cn") < 0)
                    {
                        model.TourImg = "http://wetennis.cn" + tmodel.TOURIMG;
                    }
                    else
                    {
                        model.TourImg = tmodel.TOURIMG;
                    }

                    //添加赛事类型及图片
                    switch (tmodel.CITYTYPE)
                    {
                        case "":
                            model.TourType = "公开赛";
                            model.TourTypeImg = "http://wetennis.cn:86/Common/TourType_gks.png";
                            break;
                        case "Club":
                            model.TourType = "俱乐部赛";
                            model.TourTypeImg = "http://wetennis.cn:86/Common/TourType_jlb.png";
                            break;
                        case "Union":
                            model.TourType = "联盟赛";
                            model.TourTypeImg = "http://wetennis.cn:86/Common/TourType_lms.png";
                            break;
                    }

                    //添加赛事控制内容
                    model.Tour_Controls = GetTourControl(tmodel.STATUS);

                    //获得赞助商信息
                    model.advertise = Biz_TourAdviser.instance.GetAdvertiserbyToursys(tmodel.SYSNO);

                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 根据赛事状态，获得赛事可以具有的点击动作
        /// </summary>
        /// <param name="_Status"></param>
        /// <returns></returns>
        private List<Model_TourControl> GetTourControl(string _Status)
        {
            List<Model_TourControl> list = new List<Model_TourControl>();
            Model_TourControl cont = new Model_TourControl();
            switch (_Status)
            {
                case "0":
                    //筹备中    
                    cont = new Model_TourControl();
                    cont.ControlName = "开始报名";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "StartApply";
                    list.Add(cont);
                    
                    //修改赛事信息
                    cont = new Model_TourControl();
                    cont.ControlName = "修改赛事信息";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "ModifyTourInfo";
                    list.Add(cont);                    
                    break;
                case "1":
                    //正在报名
                    cont = new Model_TourControl();
                    cont.ControlName = "直通报名";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "DirectApply";
                    list.Add(cont);

                     cont = new Model_TourControl();
                    cont.ControlName = "查看报名";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "CheckApply";
                    list.Add(cont);

                    cont = new Model_TourControl();
                    cont.ControlName = "结束报名";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "EndApply";
                    list.Add(cont);

                    //修改赛事信息
                    cont = new Model_TourControl();
                    cont.ControlName = "修改赛事信息";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "ModifyTourInfo";
                    list.Add(cont);
                    break;
                case "2":
                    //结束报名
                    cont = new Model_TourControl();
                    cont.ControlName = "查看报名";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "CheckApply";
                    list.Add(cont);

                    cont = new Model_TourControl();
                    cont.ControlName = "设置签表";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "SetSign";
                    list.Add(cont);

                    cont = new Model_TourControl();
                    cont.ControlName = "开始抽签";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "StartSign";
                    list.Add(cont);

                    //修改赛事信息
                    cont = new Model_TourControl();
                    cont.ControlName = "修改赛事信息";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "ModifyTourInfo";
                    list.Add(cont);
                    break;

                case "3":
                    //赛事签表
                    cont = new Model_TourControl();
                    cont.ControlName = "管理签表";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "EditSign";
                    list.Add(cont);
                    
                    cont = new Model_TourControl();
                    cont.ControlName = "完成签表分配";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "EndSign";
                    list.Add(cont);

                    //修改赛事信息
                    cont = new Model_TourControl();
                    cont.ControlName = "修改赛事信息";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "ModifyTourInfo";
                    list.Add(cont);
                    break;
                case "4":
                    //赛事资源分布
                    cont = new Model_TourControl();
                    cont.ControlName = "分配资源";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "DistributeRes";
                    list.Add(cont);
                    //赛事积分设置
                    cont = new Model_TourControl();
                    cont.ControlName = "积分设置";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "ScoreSetting";
                    list.Add(cont);
                    //裁判设置
                    cont = new Model_TourControl();
                    cont.ControlName = "裁判分配";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "UmpireSetting";
                    list.Add(cont);

                    //开始比赛
                    cont = new Model_TourControl();
                    cont.ControlName = "开始比赛";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "StartEvent";
                    list.Add(cont);

                    //修改赛事信息
                    cont = new Model_TourControl();
                    cont.ControlName = "修改赛事信息";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "ModifyTourInfo";
                    list.Add(cont);
                    break;

                case "5":
                    //比赛结果维护
                    cont = new Model_TourControl();
                    cont.ControlName = "结果维护";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "MatchResult";
                    list.Add(cont);

                    //结束比赛
                    cont = new Model_TourControl();
                    cont.ControlName = "结束比赛";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "EndTour";
                    list.Add(cont);

                    //修改赛事信息
                    cont = new Model_TourControl();
                    cont.ControlName = "修改赛事信息";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "ModifyTourInfo";
                    list.Add(cont);
                    break;
          
                case "6":
                    //赛事积分查看
                     cont = new Model_TourControl();
                    cont.ControlName = "赛事积分查看";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "SeeTourScore";
                    list.Add(cont);

                    //修改赛事信息
                    cont = new Model_TourControl();
                    cont.ControlName = "修改赛事信息";
                    cont.ControlUrl = "javascript:void";
                    cont.ControlFun = "ModifyTourInfo";
                    list.Add(cont);
                    break;
                default:
                    if (Convert.ToInt32(_Status) < 0)
                    { 
                        //被禁用的赛事                      
                        cont.ControlName = "启用";
                        cont.ControlUrl = "javascript:void";
                        cont.ControlFun = "Enable";
                        list.Add(cont);

                        //修改赛事信息
                        cont = new Model_TourControl();
                        cont.ControlName = "修改赛事信息";
                        cont.ControlUrl = "javascript:void";
                        cont.ControlFun = "ModifyTourInfo";
                        list.Add(cont);
                    }
                    break;
            }
            return list;
        }

        private string RenderTourStatus(string _Status)
        {
            string StatusName = "";
            switch (_Status)
            { 
                case "-1":
                    StatusName = "禁用";
                    break;
                case "0":
                    StatusName= "筹备中";
                    break;
                case "1":
                    StatusName= "正在报名";
                    break;
                case "2":
                    StatusName = "分配签表";
                    break;
                case "3":
                    StatusName = "分配赛程";
                    break;
                case "4":
                    StatusName = "进行中";
                    break;
                case "5":
                    StatusName = "已完成";
                    break;
            }
            return StatusName;
        }

        /// <summary>
        /// 获取赛事状态值，
        /// 2016-9-19，刘涛
        /// </summary>
        /// <param name="_Status"></param>
        /// <returns></returns>
        private string RenderTourStatus_Sep(string _Status)
        {
            string StatusName = "";
            switch (_Status)
            {
                case "-1":
                    StatusName = "禁用";
                    break;
                case "0":
                    StatusName = "筹备中";
                    break;
                case "1":
                    StatusName = "正在报名";
                    break;
                case "2":
                    StatusName = "结束报名";
                    break;
                case "3":
                    StatusName = "赛事签表";
                    break;
                case "4":
                    StatusName = "资源分配";
                    break;
                case "5":
                    StatusName = "正在进行";
                    break;
                case "6":
                    StatusName = "已完成";
                    break;
            }
            return StatusName;
        }
    }
}
