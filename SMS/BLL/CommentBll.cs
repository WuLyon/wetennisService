using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Member;

namespace SMS
{
    public class CommentBll
    {
        public static CommentBll instance = new CommentBll();

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="model"></param>
        public void InsertComment(CommentModel model)
        {
            string sql = string.Format("insert into wtf_comments (Type,TypeSysno,MemSysno,Comments,UpdateDate) values ('{0}','{1}','{2}','{3}','{4}')", model.TYPE, model.TYPESYSNO, model.MEMSYSNO, model.COMMENTS, DateTime.Now.ToString());
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// 获得评论列表
        /// </summary>
        /// <param name="_TypeSysno"></param>
        /// <param name="_Type"></param>
        /// <returns></returns>
        public List<CommentModel> GetComList(string _TypeSysno, string _Type)
        {
            List<CommentModel> list = new List<CommentModel>();
            string sql = "select * from wtf_comments where type='"+_Type+"' and typesysno='"+_TypeSysno+"' order by id desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<CommentModel>>(dt);

                foreach(CommentModel model in list)
                {
                    //加载评论人姓名，头像
                    WeMemberModel mmodel = WeMemberDll.instance.GetModel(model.MEMSYSNO);
                    model.MEMNAME = mmodel.NAME;
                    model.MEMIMG = mmodel.EXT1;

                    //加载点赞数量
                    model.GoodQty = PraiseBll.instance.CountPraiseQty("Comment", model.ID, "1");
                    model.BadQty = PraiseBll.instance.CountPraiseQty("Comment", model.ID, "0");

                    //
                }
            }
            return list;
        }

        public List<CommentModel> GetHotComments(string _TypeSysno, string _Type)
        {
            List<CommentModel> list = new List<CommentModel>();
            List<CommentModel> Alllist = GetComList(_TypeSysno, _Type);
            //var listorder = Alllist.OrderBy(x >= x.GoodQty);
            var query = from p in Alllist
                        orderby p.GoodQty descending
                        select p;
            list = query.ToList<CommentModel>();
            List<CommentModel> listnew = new List<CommentModel>();
            if (list.Count > 3)
            {
                listnew.Add(list[0]);
                listnew.Add(list[1]);
                listnew.Add(list[2]);
            }
            return listnew;
        }
    }
}
