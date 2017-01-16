using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Ranking;

namespace Bet
{
    public class BetRateBiz
    {
        public static BetRateBiz instance = new BetRateBiz();

        /// <summary>
        /// 添加新的记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string InsertNew(BetRateModel model)
        {
            string sysno = Guid.NewGuid().ToString("N").ToUpper();
            string sql = string.Format("insert into wtf_betRate ( matchsys,betName,Status,betDescription,betType,betTag,PrizeType,Creator,CreateTime,Sysno,EndTime) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",model.MATCHSYS,model.BETNAME,model.STATUS,model.BETDESCRIPTION,model.BETTYPE,model.BETTAG,model.PRIZETYPE,model.CREATOR,DateTime.Now.ToString(),sysno,model.ENDTIME);
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a <= 0)
            {
                sysno = "";
            }
            return sysno;
        }
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateStatus(string id,string status)
        {
            string sql = "update wtf_betRate set status='"+status+"' where id='"+id+"'";
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
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteBetRate(string id)
        {
            string sql = "delete wtf_betRate where id='" + id + "'";
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
        /// Get betRate model by id
        /// </summary>
        /// <param name="sysno"></param>
        /// <returns></returns>
        public BetRateModel GetmodelbyId(string id)
        {
            BetRateModel model = new BetRateModel();
            string sql = "select * from wtf_betRate where id='" + id + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<BetRateModel>(dt);
            }
            return model;
        }


        /// <summary>
        /// Get betRate model by sys
        /// </summary>
        /// <param name="sysno"></param>
        /// <returns></returns>
        public BetRateModel GetmodelbySys(string sysno)
        {
            BetRateModel model = new BetRateModel();
            string sql = "select * from wtf_betRate where Sysno='"+sysno+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<BetRateModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// Get Bet List by bet Type
        /// </summary>
        /// <param name="_BetType"></param>
        /// <returns></returns>
        public List<BetRateModel> getBetList(string _BetType)
        {
            List<BetRateModel> list = new List<BetRateModel>();
            string sql = "select * from wtf_betrate where Status='1' and convert(datetime,endtime)>'"+DateTime.Now.ToString()+"' ";
            if (_BetType != "")
            {
                sql += " and betType='" + _BetType + "' ";
            }

            sql += " order by endtime";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            list = JsonHelper.ParseDtModelList<List<BetRateModel>>(dt);
            return list;
        }

        /// <summary>
        /// get bet list html text
        /// </summary>
        /// <param name="_BetType"></param>
        /// <returns></returns>
        public string GetBetListHtml(string _BetType)
        {
            string html = "";
            List<BetRateModel> list = getBetList(_BetType);
            if (list.Count > 0)
            {
                foreach (BetRateModel model in list)
                { 
                    //add head
                    html += "<ul class=\"betMain\">";
                    //add main bet 
                    html += "<li class=\"betMainText\">"+model.BETDESCRIPTION+"</li>";
                    //add end time
                    html += "<li class=\"betendtime\">竞猜截至时间:"+model.ENDTIME+"</li>";
                    //add bet choices
                    //add choice head
                    html += "<li><ul class=\"betChoices\">";
                    List<BetChoiceModel> clist = BetChoiceBiz.instance.GetBetChoicebyRate(model.ID);
                    if (clist.Count > 0)
                    {
                        foreach (BetChoiceModel cmodel in clist)
                        {
                            //竞猜投注情况
                            string betQty = "";
                            string betTotal = "";
                            MyBetBiz.instance.getBetQty(cmodel.ID,out betQty,out betTotal);//为统计项赋值
                            html += "<li><ul class=\"betChoice\" onclick=\"Chose('" + cmodel.ID + "')\"><li class=\"bcTotal\">共投注" + betQty + "次 总积分：" + betTotal + " </li>";
                            //竞猜选项
                            html += "<li class=\"bcDesc\">"+cmodel.CHOICEDESC+"("+cmodel.RATE+")</li></ul></li>";
                        }
                    }
                    //add choice footer
                    html += "</ul></li>";
            
                    //add bet end
                    html += "</ul>";        
                }
            }
            return html;
        }

        /// <summary>
        /// 更新竞猜结果
        /// </summary>
        /// <param name="betRateid"></param>
        /// <param name="answerChoice"></param>
        public void UpdateBetAnswer(string betRateid, string answerChoice)
        { 
            //update betrate
            string sql = "update wtf_betRate set betAnswer='"+answerChoice+"' where id='"+betRateid+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            { 
                //update my bet result
                List<MyBetModel> list = MyBetBiz.instance.GetmyBetList(betRateid);
                foreach (MyBetModel model in list)
                {
                    if (model.BETCHOICEID == answerChoice)
                    {
                        //竞猜正确
                        MyBetBiz.instance.UpdateMybetStatus(model.ID, "2");
                        //为竞猜者添加相应的积分
                        RPointDll.instance.AddRankPoint("社区", "", model.RETURNQTY, "", model.MEMSYS, "竞猜正确，获得奖励积分", "", ""); 
                    }
                    else
                    { 
                        //竞猜错误
                        MyBetBiz.instance.UpdateMybetStatus(model.ID, "4");
                    }
                }
            }
        }

        /// <summary>
        /// 获得我管理的比赛竞猜
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public string GetMyCreatedBetList(string _Memsys)
        {
            string html = "";
            string sql = "select * from wtf_betRate where creator='" + _Memsys + "' order by id desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    //添加竞猜项描述
                    html += "<ul class=\"betMain\"><li class=\"betMainText\">" + dt.Rows[i]["betDescription"].ToString() + "</li>";
                    //添加竞猜选项
                    string betChoices = "";
                    List<BetChoiceModel> list = BetChoiceBiz.instance.GetBetChoicebyRate(dt.Rows[i]["id"].ToString());
                    if (list.Count > 0)
                    {
                        foreach (BetChoiceModel model in list)
                        {
                            betChoices += model.CHOICEDESC+"("+model.RATE+") /  ";
                        }
                        betChoices=betChoices.Trim().TrimEnd('/');
                    }
                    html += betChoices ;
                    //添加截止日期
                    html += " <li>截止日期:"+dt.Rows[i]["endtime"].ToString()+"</li>";
                    //添加状态
                    string Rstatus = dt.Rows[i]["status"].ToString() == "0" ? "未启用" : dt.Rows[i]["status"].ToString() == "1" ? "已启用" : "已结束";
                    html += "<li>状态：" + Rstatus + "</li>";
                    //添加操作按钮
                    switch (dt.Rows[i]["status"].ToString())
                    { 
                        case "0":                            
                            html += "<li><input type=\"button\" value=\"启用\" onclick=\"enablethis('" + dt.Rows[i]["id"].ToString() + "')\" /> <input type=\"button\" value=\"删除\" onclick=\"Deletethis('" + dt.Rows[i]["id"].ToString() + "')\" /></li></ul>";
                            break;
                        case "1":
                            html += "<li><input type=\"button\" value=\"完成\" onclick=\"Finishthis('" + dt.Rows[i]["id"].ToString() + "')\" /></li></ul>";
                            break;

                        case "2":
                            html += "</ul>";
                            break;

                    }
                    
                }
            }
            else
            {
                html += "暂无竞猜项，快点击新建，开始添加竞猜吧！";
            }
            return html;
        }

        /// <summary>
        /// 确认竞猜结果
        /// </summary>
        /// <param name="_Rateid"></param>
        /// <param name="_ChoiceId"></param>
        /// <returns></returns>
        public bool ConfirmBet(string _Rateid, string _ChoiceId)
        {
            string sql = "update wtf_betRate set betAnswer='"+_ChoiceId+"' ,Status='2' where id='"+_Rateid+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                //处理积分
                MyBetBiz.instance.ComputeResult(_Rateid, _ChoiceId);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
