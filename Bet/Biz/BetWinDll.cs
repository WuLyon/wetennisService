using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Member;
using System.Data;

namespace Bet
{
    public class BetWinDll
    {
        public static BetWinDll instance = new BetWinDll();

        /// <summary>
        /// 根据竞猜日期获得竞猜正确的名单,当未指定日期的时候，返回最近一个月的竞猜内容
        /// </summary>
        /// <param name="_BetEndDate"></param>
        /// <returns></returns>
        public List<BetDateModel> GetBetWinlist(string _BetEndDate)
        {
            List<BetDateModel> list = new List<BetDateModel>();

            //先获取对应的比赛主键,按照id倒序排列
            string sql = "";
            if (string.IsNullOrEmpty(_BetEndDate))
            {
                //未指定具体的竞猜日期
                sql = string.Format("select * from wtf_betrate where status=2 and convert(datetime,endtime) between dateadd(dd,-30,getdate()) and  getdate() order by id desc");
                DataTable dtd = DbHelperSQL.Query(sql).Tables[0];
                if (dtd.Rows.Count > 0)
                {
                    foreach (DataRow drd in dtd.Rows)
                    {
                        BetDateModel dmodel = new BetDateModel();
                        dmodel.BetEndDate = Convert.ToDateTime(drd["EndTime"].ToString()).ToString("yyyy-MM-dd");
                        dmodel.BetDateList = GetlistbyDate(dmodel.BetEndDate);
                        list.Add(dmodel);
                    }
                }
            }
            else
            {
                BetDateModel dmodel = new BetDateModel();
                dmodel.BetEndDate = _BetEndDate;
                dmodel.BetDateList = GetlistbyDate(dmodel.BetEndDate);
                list.Add(dmodel);
            }
            return list;
        }

        public List<BetWinModel> GetlistbyDate(string _BetEndDate)
        {
            List<BetWinModel> list = new List<BetWinModel>();
             //指定了具体的竞猜截止日期
             string sql = string.Format("select * from wtf_betrate where status=2 and convert(datetime,endtime) between convert(datetime,'{0}') and dateadd(dd,1,convert(datetime,'{0}')) order by id desc", _BetEndDate);
            
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                //构造竞猜获胜实体并添加到列表
                foreach (DataRow dr in dt.Rows)
                {
                    BetWinModel model = new BetWinModel();
                    model.BetEndDate = dr["EndTime"].ToString();
                    model.BetRateID = dr["id"].ToString();
                    model.BetDesc = dr["betDescription"].ToString();
                    //获取中奖人数
                    List<WeMemberModel> memlist = new List<WeMemberModel>();
                    string sql1 = string.Format("select distinct(memsys) from wtf_bet  where betrateid='{0}' and betChoiceId='{1}'", model.BetRateID, dr["betAnswer"].ToString());
                    DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in dt1.Rows)
                        {
                            WeMemberModel model1 = WeMemberDll.instance.GetModel(dr1[0].ToString());
                            memlist.Add(model1);
                            model.WinnerList = memlist;
                        }
                    }
                    //将构造完成的竞猜获胜实体添加到列表
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取最近一个月的日期
        /// </summary>
        /// <returns></returns>
        public List<BetWinModel> GetRencentEnddates()
        {
            List<BetWinModel> list = new List<BetWinModel>();
            string sql = string.Format("select * from wtf_betrate where status=2 and convert(datetime,endtime) between dateadd(dd,-30,getdate()) and  getdate() order by id desc");
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BetWinModel model = new BetWinModel();
                    model.BetEndDate = Convert.ToDateTime(dr["EndTime"].ToString()).ToString("yyyy-MM-dd");
                    list.Add(model);
                }
            }//end of cheack dates
            return list;
        }
    }
}
    