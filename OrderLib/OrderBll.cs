using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Gym;
using SMS;


namespace OrderLib
{
    public class OrderBll
    {
        private static string connectionString = "";
        public static OrderBll Instance(string _Code)
        {
            OrderBll _instance = new OrderBll();
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[_Code].ToString();
            return _instance;
        }


        #region 数据处理基本方法
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        private static DataSet Query(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        private static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        #endregion

        #region  查询数据
        /// <summary>
        /// 根据主订单号获得订单详情
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public OrderModel getOrderModel(string _id)
        {
            OrderModel model = new OrderModel();
            string sql = "select * from Order_Main where id='"+_id+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<OrderModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据sysno获得实体
        /// </summary>
        /// <param name="_sysno"></param>
        /// <returns></returns>
        public OrderModel getOrderModelbySys(string _sysno)
        {
            OrderModel model = new OrderModel();
            string sql = "select * from Order_Main where sysno='" + _sysno + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<OrderModel>(dt);
            }
            return model;
        }


        /// <summary>
        /// 根据客户主键获取客户的订单
        /// </summary>
        /// <param name="CustNo"></param>
        /// <returns></returns>
        public DataTable GetOrdersbyCust(string CustNo,string _Status)
        {
            string sql = "select * from Order_Main where Cust_Sys='" + CustNo + "' and status='" + _Status + "' order by id desc";
            return Query(sql).Tables[0];
        }

        /// <summary>
        /// 获得用户的订单
        /// </summary>
        /// <param name="CustNo"></param>
        /// <param name="_Status"></param>
        /// <returns></returns>
        public List<OrderModel> GetOrderModelsByCust(string CustNo, string _Status)
        {
            List<OrderModel> list = new List<OrderModel>();
            DataTable dt = GetOrdersbyCust(CustNo, _Status);
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<OrderModel>>(dt);
            }

            
            return list;
        }


        /// <summary>
        /// 根据提供商查询订单详情
        /// </summary>
        /// <param name="SupplyNo"></param>
        /// <param name="_Status"></param>
        /// <returns></returns>
        public DataTable GetOrdersbySupplier(string SupplyNo, string _Status,string ext1)
        {
            string sql = "select * from Order_Main where Supply_Sys='" + SupplyNo + "' and status='" + _Status + "' order by id desc";
            if (ext1 != "")
            {
                sql = "select * from Order_Main where Supply_Sys='" + SupplyNo + "' and status='" + _Status + "' and ext1='" + ext1 + "' order by id desc";
            }
            return Query(sql).Tables[0];
        }

        /// <summary>
        /// 根据提供商查询订单详情
        /// </summary>
        /// <param name="SupplyNo"></param>
        /// <param name="_Status"></param>
        /// <returns></returns>
        public DataTable GetOrders(string Condition)
        {
            string sql = "select * from Order_Main where status in (1,2) " + Condition + " order by id desc";           
            return Query(sql).Tables[0];
        }

       

        /// <summary>
        /// 根据供应商获得订单实体清单
        /// </summary>
        /// <param name="SupplyNo"></param>
        /// <param name="_Status"></param>
        /// <returns></returns>
        public List<OrderModel> GetOrdersModelbySupply(string SupplyNo, string _Status,string ext1)
        {
            List<OrderModel> list = new List<OrderModel>();
            DataTable dt = GetOrdersbySupplier(SupplyNo, _Status, ext1);
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<OrderModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 获得未支付的类别
        /// </summary>
        /// <param name="SupplyNo"></param>
        /// <param name="_Status"></param>
        /// <returns></returns>
        public List<OrderModel> GetOrdersTypebySupply(string SupplyNo, string _Status)
        {
            List<OrderModel> list = new List<OrderModel>();
            //string sql = "select distinct(Description),ext1 from Order_Main where Status="+_Status+" and Supply_Sys='"+SupplyNo+"'";
            string sql = "select distinct(Description),ext1 from Order_Main where Status=" + _Status + "";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        OrderModel model = new OrderModel();
                        model.EXT1 = dr["ext1"].ToString();
                        model.DESCRIPTION = dr["Description"].ToString();
                        list.Add(model);
                    }
                    catch
                    { 
                    
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// get order model from member sysno and content id
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <param name="_Contentid"></param>
        /// <returns></returns>
        public OrderModel GetOrderModelbyCond(string _Memsys, string _Contentid)
        {
            OrderModel model = new OrderModel();
            string sql = string.Format("select * from Order_Main where Cust_sys='{0}' and ext1='{1}' order by id desc",_Memsys,_Contentid);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<OrderModel>(dt);
            }
            return model;
        }
        #endregion




        #region 添加订单信息
        /// <summary>
        /// 向订单主表中添加一条记录
        /// </summary>
        /// <param name="_Desc">订单描述</param>
        /// <param name="_Type">订单类型，产品/服务</param>
        /// <param name="_CustSys">客户主键</param>
        /// <param name="_CustName">客户姓名</param>
        /// <param name="_CustAddress">客户地址</param>
        /// <param name="_CustTel">客户电话</param>
        /// <param name="_OrderMoney">订单总金额</param>
        /// <param name="_Discount">折扣</param>
        /// <param name="_ShouldPay">应付金额</param>
        /// <param name="_ActualPay">实付金额</param>
        /// <param name="_Status">状态</param>
        /// <param name="_SupplySys">供应商主键</param>
        /// <param name="_SuppyName">供应商名称</param>
        /// <param name="_SupContPerson">供应商联系人</param>
        /// <param name="_SupTel">供应商电话</param>
        /// <returns></returns>
        public string InsertOrderMain(string _Desc,string _Type,string _CustSys,string _CustName,string _CustAddress,string _CustTel,string _OrderMoney,string _Discount,string _ShouldPay,string _ActualPay,string _Status,string _SupplySys,string _SuppyName,string _SupContPerson,string _SupTel,string _ext1,string _ext2,string _ext3,string _ext4,string _ext5)
        {
            //string OrderSys=Guid.NewGuid().ToString("N").ToUpper();//
            string OrderSys = Makesysno();
            string sql = string.Format("insert into Order_Main (sysno,Description,Type,Cust_Sys,CustName,CustAddress,CustTel,OrderMoney,Discount,ShouldPay,ActualPay,Status,Supply_Sys,Supply_Name,Supply_ContactPerson,Supply_Tel,ext1,ext2,ext3,ext4,ext5,UpdateDate) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}')", OrderSys, _Desc, _Type, _CustSys, _CustName, _CustAddress, _CustTel, _OrderMoney, _Discount, _ShouldPay, _ActualPay, _Status, _SupplySys, _SuppyName, _SupContPerson, _SupTel,_ext1,_ext2,_ext3,_ext4,_ext5,DateTime.Now.ToString());
            int a = ExecuteSql(sql);
            if (a > 0)
            {
                //插入订单记录
                InsertOrderLog(OrderSys, _Status, "创建订单");
                return OrderSys;
            }
            else
            {
                return a.ToString();
            }
        }
        /// <summary>
        /// Add Order Main Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string AddOrderMain(OrderModel model)
        {
            string OrderSys = Makesysno();
            string sql = string.Format("insert into Order_Main (sysno,Description,Type,Cust_Sys,CustName,CustAddress,CustTel,OrderMoney,Discount,ShouldPay,ActualPay,Status,Supply_Sys,Supply_Name,Supply_ContactPerson,Supply_Tel,ext1,ext2,ext3,ext4,ext5,BalancePay,PayWay,UpdateDate) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')", OrderSys, model.DESCRIPTION, model.TYPE, model.CUST_SYS, model.CUSTNAME, model.CUSTADDRESS, model.CUSTTEL, model.ORDERMONEY, model.DISCOUNT, model.SHOULDPAY, model.ACTUALPAY, model.STATUS, model.SUPPLY_SYS, model.SUPPLY_NAME, model.SUPPLY_CONTACTPERSON, model.SUPPLY_TEL, model.EXT1, model.EXT2, model.EXT3, model.EXT4, model.EXT5,model.BALANCEPAY,model.PAYWAY,DateTime.Now.ToString());
            int a = ExecuteSql(sql);
            if (a > 0)
            {
                //插入订单记录
                InsertOrderLog(OrderSys, model.STATUS, "创建订单");
                return OrderSys;
            }
            else
            {
                return a.ToString();
            }
        
        }

        /// <summary>
        /// 构造日期+随机数的主键
        /// </summary>
        /// <returns></returns>
        public string Makesysno()
        {
            string NumPart = "";
            string DatePart = DateTime.Now.ToString("yyyyMMddHHmmss");
            //get sametime order
            string sql = "select * from Order_Main where sysno like '" + DatePart + "%'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];            
            NumPart = string.Format("{0:00}",dt.Rows.Count+1);
            return DatePart + NumPart;            
        }

        /// <summary>
        /// 添加订单行
        /// </summary>
        /// <param name="_Ordersys">主订单主键</param>
        /// <param name="_DetailType">订单行类型</param>
        /// <param name="_ItemNo">订单行号</param>
        /// <param name="_ItemName">订单名称</param>
        /// <param name="_ItemUnit">单位</param>
        /// <param name="_ItemSpecify">规格</param>
        /// <param name="_Qty">数量</param>
        /// <param name="_UnitPrice">单价</param>
        /// <param name="_DetailMoney">订单行金额</param>
        /// <param name="_Remark">订单行备注</param>
        /// <returns></returns>
        public string InsertOrderDetail(string _Ordersys,string _DetailType,string _ItemNo,string _ItemName,string _ItemUnit,string _ItemSpecify,string _Qty,string _UnitPrice,string _DetailMoney,string _Remark)
        {
            string DetailSys = Guid.NewGuid().ToString().ToUpper();//订单行主键
            string sql = string.Format("insert into Order_Detail (Sysno,OrderSysno,DetailType,Item_No,Item_Name,Item_Unit,Item_Specify,Qty,UnitPrice,DetailMoney,Remark) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",DetailSys,_Ordersys,_DetailType,_ItemNo,_ItemName,_ItemUnit,_ItemSpecify,_Qty,_UnitPrice,_DetailMoney,_Remark);
            int a = ExecuteSql(sql);
            if (a > 0)
            {
                return DetailSys;
            }
            else
            {
                return a.ToString();
            }
        }

        /// <summary>
        /// 插入订单记录
        /// </summary>
        /// <param name="_OrderSys"></param>
        /// <param name="_Status"></param>
        /// <param name="_Descript"></param>
        /// <returns></returns>
        private string InsertOrderLog(string _OrderSys,string _Status,string _Descript)
        {
            string sql = string.Format("insert into Order_Log (Ordersysno,Status,Description,UpdateDate) values ('{0}','{1}','{2}','{3}')",_OrderSys,_Status,_Descript,DateTime.Now.ToString());
            int a = ExecuteSql(sql);
            return a.ToString();
        }

        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="_OrderSys"></param>
        /// <param name="_Status"></param>
        /// <param name="_Description">修改状态描述</param>
        /// <returns></returns>
        public string UpdateOrderStatus(string _OrderSys, string _Status,string _Description)
        {
            string sql = string.Format("Update Order_Main set status='{0}' where sysno='{1}'",_Status,_OrderSys);
            int a = ExecuteSql(sql);
            if (a > 0)
            { 
                //添加订单记录
                InsertOrderLog(_OrderSys, _Status, _Description);

                //发送消费确认消费信息
                if (_Status == "3")
                {
                    //发送短信
                    OrderModel model = getOrderModelbySys(_OrderSys);
                    string Desc = model.DESCRIPTION;
                    if (Desc.Length > 16) 
                    {
                        Desc = model.DESCRIPTION.Substring(0, 16);
                    }

                    string MsgToCon = "您好！您的订单已确认消费成功!（" + Desc + "），感谢您对微网球的信赖！";
                    string MsgToSup = "您好！订单（" + Desc + ",客户：" + model.CUSTNAME + model.CUSTTEL + "）已确认消费成功！";

                    //send message to customer
                    //SMSdll.instance.BatchSendSMS(model.CUSTTEL, MsgToCon);
                    //send message to supplier
                    //SMSdll.instance.BatchSendSMS(model.SUPPLY_TEL, MsgToSup);
                }
            }
            return a.ToString();
        }

        /// <summary>
        /// 修改订单支付信息
        /// </summary>
        /// <param name="_OrderSys"></param>
        /// <param name="_Status"></param>
        /// <param name="_Discount"></param>
        /// <param name="_ShouldPay"></param>
        /// <param name="_ActualPay"></param>
        /// <param name="_TicketMoney"></param>
        /// <param name="_TicketSys"></param>
        /// <returns></returns>
        public string UpdateOrderPay(string _OrderSys, string _Status, string _Discount, string _ShouldPay, string _ActualPay, string _TicketMoney, string _TicketSys, string _Descript, string _BalancePay, string _PayWay)
        {
            string sql = string.Format("Update Order_Main set status='{0}',Discount='{2}',TicketMoney='{3}',TicketSys='{4}',ShouldPay='{5}',ActualPay='{6}',BalancePay='{7}',PayWay='{8}' where sysno='{1}'", _Status, _OrderSys, _Discount, _TicketMoney, _TicketSys, _ShouldPay,_ActualPay,_BalancePay, _PayWay);
            int a = ExecuteSql(sql);
            if (a > 0)
            {
                InsertOrderLog(_OrderSys, _Status, _Descript);
            }
            return a.ToString();
        }

        /// <summary>
        /// Update balance pay part
        /// </summary>
        /// <param name="_OrderSys"></param>
        /// <param name="_BalancePay"></param>
        /// <returns></returns>
        public bool UpdateBalancePay(string _OrderSys, string _BalancePay)
        {
            string sql = string.Format("Update Order_Main set BalancePay='{0}' where sysno='{1}'", _BalancePay, _OrderSys);
            int a = ExecuteSql(sql);
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
        /// Use Balance to pay
        /// </summary>
        /// <param name="_OrderSys"></param>
        /// <returns></returns>
        public bool BalancePay(string _OrderSys,string _OrderMoney)
        {
            OrderModel model=getOrderModelbySys(_OrderSys);
            string sql = string.Format("Update Order_Main set BalancePay='{0}',PayWay='{2}',status='{3}' where sysno='{1}'", _OrderMoney, _OrderSys,"balance","2");
            int a = ExecuteSql(sql);
            if (a > 0)
            {
                OrderMoneyBll.instance.ChargeMoney(model.CUST_SYS, "-" + _OrderMoney);//修改我的余额
                UpdateBusinessOrder(_OrderSys);//修改业务订单状态
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 修改业务订单
        /// <summary>
        /// 修改业务订单
        /// </summary>
        /// <param name="_OrderSys"></param>
        public void UpdateBusinessOrder(string _OrderSys)
        {
            OrderModel model = getOrderModelbySys(_OrderSys);
            string sql = "";
            string Desc = model.DESCRIPTION;
            if (Desc.Length > 16)
            {
                Desc = model.DESCRIPTION.Substring(0, 16);
            }
            string _MsgContent = "您已成功支付" + model.ORDERMONEY + "元。订单详情(" + Desc + "...)。请尽快到场消费";
            switch (model.TYPE)
            { 
                case "court":
                    //修改订场订单
                    sql = "update wtf_CourtOrder set Status='2' where OrderNo='"+_OrderSys+"'";
                    break;
                case "apply":
                    //修改报名订单
                    sql = "update wtf_TourApply set status='2' where ext2='"+_OrderSys+"'";
                    break;
                case "报名":
                    //修改报名订单
                    sql = "update wtf_TourApply set status='2' where ext2='" + _OrderSys + "'";
                    break;
                case "groupapply":
                    //修改报名订单
                    sql = "update wtf_TourApply set status='2' where ext2='" + _OrderSys + "'";
                    break;
                case "charge":
                    //charge money
                    OrderMoneyBll.instance.ChargeMoney(model.CUST_SYS, model.ORDERMONEY);
                    break;             
            }
            if (sql != "")
            {
                int a = DbHelperSQL.ExecuteSql(sql);
                if (a > 0)
                { 
                    //send message
                    SMSdll.instance.BatchSendSMS(model.CUSTTEL, _MsgContent);

                    //Send To Supplier
                    string supMsg = "客户成功付款" + model.ORDERMONEY + "元，订单详情(" + Desc + "...)。请登陆微网球查看详情";
                    //SMSdll.instance.BatchSendSMS(model.SUPPLY_TEL, supMsg);
                }
            }
        }

        /// <summary>
        /// cancel custormer order
        /// </summary>
        /// <param name="_OrderSys"></param>
        public string CancelOrder(string _OrderSys)
        {
            string _canRes = "";
            OrderModel model = getOrderModelbySys(_OrderSys);
            switch (model.TYPE)
            { 
                case "court":
                    //if return in correct time
                    if (IsCourtOrderReturnable(_OrderSys))
                    {
                        //return order
                        UpdateOrderStatus(_OrderSys, "99", "取消订单");//update order
                        CourtOrderBll.instance.UpdateOrderStatus(_OrderSys, "99");//update business order
                        OrderMoneyBll.instance.ChargeMoney(model.CUST_SYS, model.ORDERMONEY);//return money
                        _canRes = "ok";
                    }
                    else
                    {
                        _canRes = "no";
                    }
                    break;
            }
            return _canRes;
        }

        /// <summary>
        /// cancel custormer order
        /// </summary>
        /// <param name="_OrderSys"></param>
        public string DeleteOrder(string _OrderSys)
        {
            string _canRes = "";
            OrderModel model = getOrderModelbySys(_OrderSys);
            switch (model.TYPE)
            {
                case "court":                   
                    //return order
                    UpdateOrderStatus(_OrderSys, "97", "客户删除订单");//update order
                    CourtOrderBll.instance.UpdateOrderStatus(_OrderSys, "97");//update business order
                    _canRes = "ok";
                 
                    break;
                case "charge":
                    UpdateOrderStatus(_OrderSys, "97", "客户删除订单");//update order
                    _canRes = "ok";
                    break;
                default:
                    _canRes = "no";
                    break;
            }
            return _canRes;
        }

        /// <summary>
        /// 验证订场订单是否能退回，24小时以前订的订单可以退回
        /// </summary>
        /// <param name="_OrderSys"></param>
        /// <returns></returns>
        public bool IsCourtOrderReturnable(string _OrderSys)
        {
            CourtOrderModel comodel = CourtOrderBll.instance.GetModelbyOrderNo(_OrderSys);
            if (Convert.ToDateTime(comodel.ORDERDATE) < DateTime.Now.AddDays(-1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
    