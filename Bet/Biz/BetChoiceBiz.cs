using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Bet
{
    public class BetChoiceBiz
    {
        public static BetChoiceBiz instance = new BetChoiceBiz();

        /// <summary>
        /// 添加新的竞猜选项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(BetChoiceModel model)
        {
            //根据sysno 获得id
            BetRateModel bmodel = BetRateBiz.instance.GetmodelbySys(model.BETRATEID);

            string sql = string.Format("insert into wtf_betChoice (betRateId,betChoice,choiceDesc,Rate,betRateSys) values ('{0}','{1}','{2}','{3}','{4}')", bmodel.ID, model.BETCHOICE, model.CHOICEDESC, model.RATE,model.BETRATEID);
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

        /// <summary>
        /// 根据id获得竞猜选项实体
        /// </summary>
        /// <param name="_Id"></param>
        /// <returns></returns>
        public BetChoiceModel GetModel(string _Id)
        {
            BetChoiceModel model = new BetChoiceModel();
            string sql = "select * from wtf_betChoice where id='"+_Id+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<BetChoiceModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据竞猜主键获得竞猜项清单
        /// </summary>
        /// <param name="_BetRate"></param>
        /// <returns></returns>
        public List<BetChoiceModel> GetBetChoicebyRate(string _BetRate)
        {
            List<BetChoiceModel> list = new List<BetChoiceModel>();
            string sql = "select * from wtf_betChoice where betRateId='" + _BetRate + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<BetChoiceModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 根据bet rate 主键来获得竞猜子项列表
        /// </summary>
        /// <param name="_BetRSys"></param>
        /// <returns></returns>
        public List<BetChoiceModel> GetBetChoicebySys(string _BetRSys)
        {
            List<BetChoiceModel> list = new List<BetChoiceModel>();
            string sql = "select * from wtf_betChoice where betRateSys='" + _BetRSys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<BetChoiceModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Delete bet choice
        /// </summary>
        /// <param name="_Id"></param>
        /// <returns></returns>
        public bool DeleteChoice(string _Id)
        {
            string sql = "Delete wtf_betChoice where id='"+_Id+"'";
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
