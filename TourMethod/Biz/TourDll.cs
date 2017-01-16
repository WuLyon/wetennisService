using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrderLib;
using System.Data;

namespace TourMethod
{
    class TourDll
    {
        public TourDll instance = new TourDll();

        /// <summary>
        /// 根据订单主键来修改订单状态和报名状态
        /// </summary>
        /// <param name="sysno"></param>
        public void UpdatePaymentbySysno(string sysno)
        {
            OrderModel model = OrderBll.Instance("wtf").getOrderModelbySys(sysno);
            string _id = model.ID;
            string Description = "来自接口的修改";
            //修改订单状态
            string a = OrderBll.Instance("wtf").UpdateOrderStatus(_id, "2", Description);

            if (a != "0")
            {
                //修改赛事报名的状态
                OrderModel omodel = OrderBll.Instance("wtf").getOrderModelbySys(_id);

                TourApplyModel tamodel = GetTourApplybyinfo(omodel.EXT1, omodel.CUST_SYS);
                if (UpdateTourApplyState(tamodel.ID, "2"))
                {
                    //发送确认短信
                    MsgBll.instance.SendPaymentConfirm(omodel.EXT1, omodel.CUST_SYS);
                }
            }
        }

        /// <summary>
        /// 根据报名信息获得赛事报名实体
        /// </summary>
        /// <param name="_Contentid"></param>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public TourApplyModel GetTourApplybyinfo(string _Contentid, string _Memsys)
        {
            TourApplyModel model = new TourApplyModel();
            string sql = "select * from wtf_tourapply where  Contentid ='" + _Contentid + "' and memberid='" + _Memsys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<TourApplyModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 修改赛事报名的状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="_status"></param>
        /// <returns></returns>
        public bool UpdateTourApplyState(string id, string _status)
        {
            string sql = "update wtf_tourapply set status='" + _status + "' where id='" + id + "'";
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
