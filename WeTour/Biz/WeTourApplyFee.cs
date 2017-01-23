using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class WeTourApplyFee
    {
        public static WeTourApplyFee instance = new WeTourApplyFee();

        /// <summary>
        /// Get tour apply fees
        /// 2015-11-28修改，因为删除tourapply之后，只会删除apply数据，不会删除order_main.
        /// </summary>
        /// <returns></returns>
        public string GetApplyFee(string _Toursys,string _Status)
        {
            double _ApplyFee = 0;
            WeTourModel tmodel = WeTourDll.instance.GetModelbySys(_Toursys);
            
            List<WeTourContModel> list = WeTourContentDll.instance.GetContentlist(_Toursys);
            if (list.Count > 0)
            {
                foreach (WeTourContModel model in list)
                {
                    double App = 0;
                    Double.TryParse(tmodel.EXT1,out App);//报名费单价
                    //确定该子项的报名费
                    if (model.ContentType.IndexOf("双") > 0)
                    { 
                        //双打，报名费翻倍
                        App *= 2;
                    }

                    //查询报名支付数量
                    string sql = "select * from wtf_tourapply where contentid='"+model.id+"' ";
                    if (_Status != "")
                    {
                        sql += "and status='" + _Status + "'";
                    }
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    _ApplyFee += dt.Rows.Count * App;
                }
            }

            return _ApplyFee.ToString();
        }

        /// <summary>
        /// Get Applicants Qty by contentid
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <param name="_Status"></param>
        /// <returns></returns>
        public string GetApplyQty(string _ContentId,string _Status)
        {
            string sql = "select COUNT(id) from wtf_TourApply where contentid='" + _ContentId + "'";
            if (_Status != "")
            {
                sql += " and status='"+_Status+"'";
            }
            return DbHelperSQL.Query(sql).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 获得比赛子项报名费
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public int GetApplyFee(string _ContentId)
        { 
            int ContentApplyFee=0;
            WeTourContModel cmodel = WeTourContentDll.instance.GetModelbyId(_ContentId);
            WeTourModel tmodel = WeTourDll.instance.GetModelbySys(cmodel.Toursys);
            //判断是否包含特殊价
            if (!string.IsNullOrEmpty(cmodel.ext3))
            {
                ContentApplyFee = Convert.ToInt32(cmodel.ext3);
            }
            else
            { 
                //报名费为赛事统一定价
                ContentApplyFee = Convert.ToInt32(tmodel.EXT1);
            }
            return ContentApplyFee;
        }
    }
}
