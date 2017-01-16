using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace News
{
    public class NewsBiz
    {
        public static NewsBiz instance = new NewsBiz();

        /// <summary>
        /// GetModel by Id
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public NewsModel GetModel(string _id)
        {
            NewsModel model = new NewsModel();
            string sql = "select type,title,issuetime,writer,smallurl,imgurl,status,forurl,ext1,ext2,ext3 from wtf_news where id='" + _id + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<NewsModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据主键获得新闻实体
        /// </summary>
        /// <param name="_Sysno"></param>
        /// <returns></returns>
        public NewsModel GetModelbySys( string _Sysno)
        {
            NewsModel model = new NewsModel();
            string sql = "select * from wtf_news where  sysno='"+_Sysno+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<NewsModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据状态获得新闻列表
        /// 2015-12-18 添加分页信息
        /// </summary>
        /// <param name="_Type"></param>
        /// <returns></returns>
        public List<NewsModel> GetNewsList(string _Type,string _PageSize,string _PageN,string _Status)
        {
            List<NewsModel> list = new List<NewsModel>();
            int ps=Convert.ToInt32(_PageSize);
            int pn=Convert.ToInt32(_PageN);
            int start=(pn-1)*ps;
            int end=ps*pn;
            string _Statusstr = "";
            if (_Status == "")
            {
                _Statusstr = "'0','1'";
            }
            else
            {
                _Statusstr = "'"+_Status+"'"; 
            }

            string sql = string.Format("select * from (select id,type,title,issuetime,writer,smallurl,imgurl,status,forurl,ext1,ext2,ext3,sysno,row_number() over(order by id desc) as rn  from wtf_news where type='{0}' and status in (" + _Statusstr + ") )a where a.rn>{1} and a.rn<={2}", _Type, start.ToString(), end.ToString());
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            { 
                list=JsonHelper.ParseDtModelList<List<NewsModel>>(dt);                
            }
            return list;
        }

        /// <summary>
        /// 获得新闻清单
        /// </summary>
        /// <param name="_Type"></param>
        /// <param name="_Status"></param>
        /// <returns></returns>
        public int GetNewsQty(string _Type,string _Status)
        {
            string sql = "select * from wtf_news where type='"+_Type+"'";
            if (_Status == "")
            {
                sql += " and status in ('0','1')";
            }
            else
            {
                sql += " and status='1'";
            }
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

        /// <summary>
        /// 删除新闻
        /// </summary>
        /// <param name="_Id"></param>
        /// <returns></returns>
        public bool DeleteNews(string _Id)
        {
            string sql = "Update wtf_news set status='99' where id='"+_Id+"'";
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
        /// 创建新闻
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string InsertNews(NewsModel model)
        {
            string sysno = Guid.NewGuid().ToString("N").ToUpper();
            string sql = string.Format("insert into wtf_news (type, title, issuetime, writer, smallurl, imgurl, status, forurl, ext1, ext2, ext3,sysno) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",model.TYPE,model.TITLE,DateTime.Now.ToString(),model.WRITER,model.SMALLURL,model.IMGURL,model.STATUS,model.FORURL,model.EXT1,model.EXT2,model.EXT3,sysno);
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return sysno;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 更新新闻内容
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void UpdateNews(NewsModel model)
        {
            string sql = "";
            if (string.IsNullOrEmpty(model.IMGURL))
            {
                //传来的图片地址为空，不修改图片信息
                sql = string.Format("update wtf_news set type='{0}',title='{1}',issuetime='{2}',writer='{3}' where SYSNO='{4}'", model.TYPE, model.TITLE, DateTime.Now.ToString(), model.WRITER,model.SYSNO);
            }
            else
            { 
                //上传新的图片，需修改图片地址
                sql = string.Format("update wtf_news set type='{0}',title='{1}',issuetime='{2}',writer='{3}',smallurl='{4}',imgurl='{5}' where sysno='{6}'", model.TYPE, model.TITLE, DateTime.Now.ToString(), model.WRITER,model.SMALLURL, model.IMGURL, model.SYSNO);
            }
            int a = DbHelperSQL.ExecuteSql(sql);
        }
    }
}
