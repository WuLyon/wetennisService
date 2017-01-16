using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Ranking
{
    public class WithDrawDll
    {
        public static WithDrawDll instance = new WithDrawDll();

        /// <summary>
        /// Insert a new withdraw record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Insert(WithDrawModel model)
        {
            string sql = string.Format("insert into wtf_matchWithdraw (Memsys,MatchSys,Contentid,TourSys,UpdateDate) values ('{0}','{1}','{2}','{3}','{4}')", model.Memsys, model.Matchsys, model.Contentid, model.Toursys,DateTime.Now.ToString());
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
        /// 根据赛事主键
        /// </summary>
        /// <param name="_Toursys"></param>
        public void CountWithdraw(string _Toursys)
        {
            string sql = "select * from wtf_match where toursys='" + _Toursys + "' and iswithdraw='1'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                //删除此前数据
                string sql1 = "Delete wtf_matchWithdraw where toursys='"+_Toursys+"'";
                DbHelperSQL.ExecuteSql(sql1);

                foreach (DataRow dr in dt.Rows)
                {
                    WithDrawModel model = new WithDrawModel();
                    model.Matchsys = dr["sys"].ToString();
                    model.Toursys = dr["toursys"].ToString();
                    model.Contentid = dr["Contentid"].ToString();
                    if (dr["loser"].ToString() != "")
                    {
                        //only one of the player withdraw the match
                        if (dr["loser"].ToString().IndexOf(",") > 0)
                        {
                            //double match
                            model.Memsys = dr["loser"].ToString().Split(',')[0];
                            Insert(model);

                            model.Memsys = dr["loser"].ToString().Split(',')[1];
                            Insert(model);
                        }
                        else
                        {
                            model.Memsys = dr["loser"].ToString();
                            Insert(model);
                        }
                    }
                    else
                    { 
                        //double withdraw
                        if (dr["player1"].ToString().IndexOf(",") > 0)
                        {
                            //double match
                            model.Memsys = dr["player1"].ToString().Split(',')[0];
                            Insert(model);

                            model.Memsys = dr["player1"].ToString().Split(',')[1];
                            Insert(model);
                        }
                        else
                        {
                            model.Memsys = dr["player1"].ToString();
                            Insert(model);
                        }

                        if (dr["player2"].ToString().IndexOf(",") > 0)
                        {
                            //double match
                            model.Memsys = dr["player2"].ToString().Split(',')[0];
                            Insert(model);

                            model.Memsys = dr["player2"].ToString().Split(',')[1];
                            Insert(model);
                        }
                        else
                        {
                            model.Memsys = dr["player2"].ToString();
                            Insert(model);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get WithDraw Line
        /// </summary>
        /// <returns></returns>
        public List<WithDrawModel> GetWithDrawQty()
        {
            List<WithDrawModel> list = new List<WithDrawModel>();
            string sql = "select Count(*) as withQty,Memsys from wtf_matchWithdraw where memsys!='' group by Memsys order by Count(*) desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WithDrawModel>>(dt);
            }
            return list;
        }
    }
}
