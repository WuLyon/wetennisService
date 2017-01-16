using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace OrderLib
{
    public class OrderMoneyBll
    {
        public static OrderMoneyBll instance = new OrderMoneyBll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertOrderMoney(MoneyModel model)
        {
            string sql = string.Format("insert into Order_MyMoney (Membersys,Money,UpdateDate,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", model.Membersys, model.Money, DateTime.Now.ToString(), model.ext1, model.ext2, model.ext3, model.ext4, model.ext5);
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
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateOrderMoney(MoneyModel model)
        {
            string sql = string.Format("Update Order_MyMoney set Membersys='{0}',Money='{1}',UpdateDate='{2}',ext1='{3}',ext2='{4}',ext3='{5}',ext4='{6}',ext5='{7}' where id='{8}'", model.Membersys, model.Money, DateTime.Now.ToString(), model.ext1, model.ext2, model.ext3, model.ext4, model.ext5, model.id);
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
        /// get MoneyModel by _memsys
        /// </summary>
        /// <param name="_memsys"></param>
        /// <returns></returns>
        public MoneyModel GetModelbyMem(string _memsys)
        {
            MoneyModel model = new MoneyModel();
            string sql = "select * from Order_MyMoney where Membersys='" + _memsys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<MoneyModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_memsys"></param>
        /// <param name="_money"></param>
        /// <returns></returns>
        public void ChargeMoney(string _memsys, string _money)
        {
            MoneyModel model = GetModelbyMem(_memsys);
            if (string.IsNullOrEmpty(model.id))
            {
                //create new
                model.Membersys = _memsys;
                model.Money = _money;
                InsertOrderMoney(model);
            }
            else
            {
                model.Money = (Convert.ToDecimal(model.Money) + Convert.ToDecimal(_money)).ToString();
                UpdateOrderMoney(model);
            }
        }

        

    }
}
