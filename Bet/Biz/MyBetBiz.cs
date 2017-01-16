using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Ranking;

namespace Bet
{
    public class MyBetBiz
    {
        /// <summary>
        /// 实例化
        /// </summary>
        public static MyBetBiz instance = new MyBetBiz();

        /// <summary>
        /// 添加新的竞猜记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(MyBetModel model)
        {
            string sql = string.Format("insert wtf_bet (betChoiceId,memsys,betQty,ReturnQty,CreateDate,Status,betRateid) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",model.BETCHOICEID,model.MEMSYS,model.BETQTY,model.RETURNQTY,DateTime.Now.ToString(),model.STATUS,model.BETRATEID);
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
        /// 根据竞猜选项id获得竞猜的统计项
        /// </summary>
        /// <param name="_betChoiceId"></param>
        /// <param name="_betQty"></param>
        /// <param name="_betTotal"></param>
        public void getBetQty(string _betChoiceId,out string _betQty,out string _betTotal)
        {
            _betQty = "";
            _betTotal = "";
            string sql = "select COUNT(id),SUM(betQty) from wtf_bet where betChoiceId='" + _betChoiceId + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            _betQty = dt.Rows[0][0].ToString();
            _betTotal = dt.Rows[0][1].ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memsys"></param>
        /// <param name="_choiceid"></param>
        /// <param name="_betQty"></param>
        public bool AddNewBet(string memsys, string _choiceid, string _betQty)
        {
            BetChoiceModel model = BetChoiceBiz.instance.GetModel(_choiceid);
            decimal rate = Convert.ToDecimal(model.RATE);
            int betQ = Convert.ToInt32(_betQty);
            decimal RetQ = rate * betQ;
            decimal _RetQ = Math.Ceiling(RetQ);//将返回积分数转换为整数
            MyBetModel mmodel = new MyBetModel();
            mmodel.BETCHOICEID = _choiceid;
            mmodel.BETQTY = _betQty;
            mmodel.MEMSYS = memsys;
            mmodel.RETURNQTY = _RetQ.ToString();
            mmodel.STATUS = "1";
            mmodel.BETRATEID = model.BETRATEID;
            return InsertNew(mmodel);
        }

        /// <summary>
        /// 获取竞猜结果清单
        /// </summary>
        /// <param name="_betRateId"></param>
        /// <returns></returns>
        public List<MyBetModel> GetmyBetList(string _betRateId)
        {
            List<MyBetModel> list = new List<MyBetModel>();
            string sql = "select * from wtf_bet where betrateid='"+_betRateId+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<MyBetModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 修改我的竞猜状态
        /// </summary>
        /// <param name="betid"></param>
        /// <param name="_status"></param>
        public void UpdateMybetStatus(string betid, string _status)
        {
            string sql = "update wtf_bet set status='"+_status+"' where id='"+betid+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// 根据员工编号获得我的竞猜记录
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public string GetMyBetlistHtml(string _Memsys)
        {
            string html = "";
            string sql = "select * from wtf_bet where memsys='"+_Memsys+"' and betRateid is not null order by id desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    
                        //获取竞猜项信息
                        BetRateModel model = BetRateBiz.instance.GetmodelbyId(dt.Rows[i]["betRateid"].ToString());
                        html += "<ul class=\"betMain\"><li class=\"betMainText\">" + model.BETDESCRIPTION + "</li>";
                    
                   
                    //添加竞猜时间
                    html += "<li> 竞猜时间:" + dt.Rows[i]["CreateDate"].ToString() + "</li>";

                    //添加投注情况                    
                    BetChoiceModel bmodel = BetChoiceBiz.instance.GetModel(dt.Rows[i]["betChoiceId"].ToString());
                    
                        html += "<li>竞猜情况:" + bmodel.CHOICEDESC + "(" + bmodel.RATE + ") </li>";
                    
                    

                    //添加投注情况
                    html += "<li>已压积分：" + dt.Rows[i]["betQty"].ToString() + ";预计收益:" + dt.Rows[i]["ReturnQty"].ToString() + "</li>";
                    //添加结果
                    string RndS = RenderStatus(dt.Rows[i]["status"].ToString());
                    html += "<li>" + RndS + "</li>";

                    //添加竞猜结果
                    if (dt.Rows[i]["status"].ToString() == "2")
                    {
                        if (dt.Rows[i]["betChoiceId"].ToString() == model.BETANSWER)
                        {
                            html += "<li class=\"betAnswer\">竞猜正确，获得" + dt.Rows[i]["ReturnQty"].ToString() + "个社区积分奖励</li>";
                        }
                        else
                        {
                            html += "<li class=\"betAnswer\">竞猜错误，未获得社区积分奖励</li>";
                        }
                    }

                    //添加末尾
                    html += "</ul>";

                }
            }
            else
            {
                html+="还没有竞猜历史记录";
            }
            return html;
        }

        /// <summary>
        /// 变换状态值
        /// </summary>
        /// <param name="_Status"></param>
        /// <returns></returns>
        private string RenderStatus(string _Status)
        {
            string Rend = "";
            switch (_Status)
            { 
                case "1":
                    Rend = "未结束";
                    break;
                case"2":
                    Rend = "已结束";
                    break;
                case"4":
                    Rend = "已取消";
                    break;
            }
            return Rend;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_BetRateid"></param>
        /// <param name="_ChoiceId"></param>
        public void ComputeResult(string _BetRateid, string _ChoiceId)
        {
            string sql = "select * from wtf_bet where betRateid='"+_BetRateid+"' ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //修改状态
                    UpdateMybetStatus(dt.Rows[i]["id"].ToString(), "2");
                    if (dt.Rows[i]["betChoiceId"].ToString() == _ChoiceId)
                    { 
                        //竞猜正确，返回比分
                        BetRateModel model=BetRateBiz.instance.GetmodelbyId(dt.Rows[i]["betRateId"].ToString());
                        RPointDll.instance.AddRankPoint("社区", "", dt.Rows[i]["ReturnQty"].ToString(), "", dt.Rows[i]["memsys"].ToString(), "竞猜正确:" + model.BETDESCRIPTION + "。积分奖励", "", "");
                    }
                }
            }
        }
    }
}
