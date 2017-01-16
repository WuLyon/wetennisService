using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Member;
using Gym;

namespace WeTour
{
    public class WeTourUmpireDll
    {
        public static WeTourUmpireDll instance = new WeTourUmpireDll();
        /// <summary>
        /// 根据裁判主键，获得裁判主页的欢迎语
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public UmpireGreetsModel GetUmpModel(string _Memsys)
        {
            UmpireGreetsModel model=new UmpireGreetsModel();
            WeMemberModel mem=WeMemberDll.instance.GetModel(_Memsys);
            model.UmpName=mem.USERNAME;
            model.UnUmpQty=GetUmpireMatchesbyState("",_Memsys,"0").Count.ToString();
            return model;
        }

        public WeTourModel GetTourInfo(string _Toursys,string _Memsys)
        {
            WeTourModel model = WeTourDll.instance.GetModelbySys(_Toursys);
            //获得未裁数量
            model.UnUmpQty = GetUmpireMatchesbyState(_Toursys, _Memsys, "0").Count.ToString();
            //获得已裁数量
            model.UmpedQty = GetUmpireMatchesbyState(_Toursys, _Memsys, "2").Count.ToString();            
            return model; 
        }

        /// <summary>
        /// 根据裁判主键获得未裁的赛事列表
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public List<WeTourModel> GetUnFinishedMatchbyUmpire(string _Memsys)
        {
            List<WeTourModel> list = new List<WeTourModel>();
            string sql = "select distinct(toursys) from wtf_match where state=0 and Umpire='"+_Memsys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    WeTourModel model = WeTourDll.instance.GetModelbySys(dr[0].ToString());
                    //获得未裁数量
                    model.UnUmpQty = GetUmpireMatchesbyState(dr[0].ToString(), _Memsys, "0").Count.ToString();
                    //获得已裁数量
                    model.UmpedQty = GetUmpireMatchesbyState(dr[0].ToString(), _Memsys, "2").Count.ToString();
                    list.Add(model);
                }                
            }
            return list;
        }

        /// <summary>
        /// 根据状态获得裁判员要裁决的比赛
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_Memsys"></param>
        /// <param name="_State"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetUmpireMatchesbyState(string _Toursys, string _Memsys, string _State)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select * from wtf_match where Umpire='" + _Memsys + "'";

            if(_Toursys!="")
            {
                sql+=" and toursys='" + _Toursys + "'";
            }

            if (_State == "2")
            {
                //已裁比赛
                sql += " and state=2";
            }
            else
            {
                //未裁比赛
                sql += " and state in ('0','1')";
            }
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
            return list;
        }
        /// <summary>
        /// 根据赛事主键和裁判主键获得未裁赛事信息
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public List<CourtMatchModel> GetUmpireMatchbyCourt(string _Toursys, string _Memsys)
        {
            List<CourtMatchModel> list = new List<CourtMatchModel>();
            string sql = "select distinct(courtid) from wtf_match where toursys='"+_Toursys+"' and state=0 and Umpire='"+_Memsys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CourtMatchModel model = new CourtMatchModel();
                    model.CourtId = dr[0].ToString();
                    CourtModel cmodel=CourtDll.Get_Instance().GetModelbyId(model.CourtId);
                    model.CourtName = cmodel.COURTNAME;
                    model.Gymid = cmodel.GYMID;
                    GymModel gmodel = GymDll.instanc.GetmodelbyId(cmodel.GYMID);
                    model.GymName = gmodel.GYMNAME;
                    model.CourtMatches = GetUmpMatchesbyCourt(_Toursys, _Memsys, model.CourtId);
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取裁判分场地的未裁比赛
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_Memsys"></param>
        /// <param name="_CourtId"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetUmpMatchesbyCourt(string _Toursys, string _Memsys, string _CourtId)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select * from wtf_match where toursys='" + _Toursys + "' and state=0 and Umpire='" + _Memsys + "' and courtid='" + _CourtId + "' order by Convert(int,place)";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    WeMatchModel nmodel = WeMatchDll.instance.RenderMatch(dr["sys"].ToString());
                    list.Add(nmodel);
                }
            }
            return list;
        }
    }

    public class UmpireGreetsModel
    {
        /// <summary>
        /// 裁判姓名
        /// </summary>
        public string UmpName { get; set; }
        /// <summary>
        /// 未裁比赛数量
        /// </summary>
        public string UnUmpQty { get; set; }
        /// <summary>
        /// 已裁比赛数量
        /// </summary>
        public string UmpedQty { get; set; }
    }
}
