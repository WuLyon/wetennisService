using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ChampGuess
{
    public class ChampGuessResultBiz
    {
        public static ChampGuessResultBiz instance = new ChampGuessResultBiz();

        /// <summary>
        /// add new Champion Guess
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertResult(ChampGuessResultModel model)
        {
            string sql = string.Format("insert into wtf_ChampGuessResult (memsys,Guessid,UpdateDate,ext1,ext2,ext3) values ('{0}','{1}','{2}','{3}','{4}','{5}')", model.memsys, model.Guessid, DateTime.Now.ToString(), model.ext1, model.ext2, model.ext3);
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
        /// get Guess Result Model by id
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public ChampGuessResultModel GetModel(string _id)
        {
            ChampGuessResultModel model = new ChampGuessResultModel();
            string sql = "select * from wtf_ChampGuessResult where id='" + _id + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<ChampGuessResultModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 判断是否已选择竞猜
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <param name="_Gmid"></param>
        /// <returns></returns>
        public bool IsChamGuessed(string _Memsys, string _Gmid)
        {
            string sql = "select * from wtf_ChampGuessResult a left join wtf_ChamCad b on a.guessid=b.id where b.guessid='" + _Gmid + "' and a.memsys='"+_Memsys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
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
        /// <param name="_Memsys"></param>
        /// <param name="_Gmid"></param>
        /// <returns></returns>
        public ChamCadModel GetMyGuess(string _Memsys, string _Gmid)
        {
            ChamCadModel model = new ChamCadModel();
            string sql = "select b.* from wtf_ChampGuessResult a left join wtf_ChamCad b on a.guessid=b.id where b.guessid='" + _Gmid + "' and a.memsys='" + _Memsys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<ChamCadModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据候选人id获得支持的人的信息
        /// </summary>
        /// <param name="_GuessCadid"></param>
        /// <returns></returns>
        public List<ChampGuessResultModel> getCadidateFollows(string _GuessCadid)
        {
            List<ChampGuessResultModel> list = new List<ChampGuessResultModel>();
            string sql = "select * from wtf_ChampGuessResult where guessid='" + _GuessCadid + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<ChampGuessResultModel>>(dt); 
            }
            return list;
        }

        /// <summary>
        /// 获得候选人支持人数
        /// </summary>
        /// <param name="_GuessCadid"></param>
        /// <returns></returns>
        public int GetCandidateFQty(string _GuessCadid)
        {
            string sql = "select * from wtf_ChampGuessResult where guessid='" + _GuessCadid + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }
    }
}
