using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ChampGuess
{
    public class ChampGuessMainBiz
    {

        public static ChampGuessMainBiz instance = new ChampGuessMainBiz();

        /// <summary>
        /// add a new Champion Guess Main
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(ChampGuessMainModel model)
        {
            string sql = string.Format("insert into wtf_ChampGuessMain (Toursys,ContentId,PrizeDescript,Status,ext1,ext2,ext3) values ('{0}','{1}','{2}','{3}','{4}','{5}')",model.Toursys,model.ContentId,model.PrizeDescript,model.Status,model.ext1,model.ext2,model.ext3);
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
        /// get Champion Guess Main Model by id
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public ChampGuessMainModel GetModel(string _id)
        {
            ChampGuessMainModel model = new ChampGuessMainModel();
            string sql = "select * from wtf_ChampGuessMain where id='" + _id + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<ChampGuessMainModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据赛事逐渐查看组别
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<ChampGuessMainModel> GetModellistbyTour(string _Toursys)
        {
            List<ChampGuessMainModel> list = new List<ChampGuessMainModel>();
            string sql = "select * from wtf_ChampGuessMain where toursys='" + _Toursys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<ChampGuessMainModel>>(dt);
            }
            return list;
        }
    }
}
