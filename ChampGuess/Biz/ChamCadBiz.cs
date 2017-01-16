using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ChampGuess
{
    public class ChamCadBiz
    {
        public static ChamCadBiz instance = new ChamCadBiz();

        /// <summary>
        /// 添加新记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(ChamCadModel model)
        {
            string sql = string.Format("insert into wtf_ChamCad (GuessId,Playersys,ext1,ext2,ext3) values ('{0}','{1}','{2}','{3}','{4}')",model.GuessId,model.Playersys,model.ext1,model.ext2,model.ext3);
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
        public ChamCadModel GetModel(string _id)
        {
            ChamCadModel model = new ChamCadModel();
            string sql = "select * from wtf_ChamCad where id='" + _id + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<ChamCadModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据主键获得候选人
        /// </summary>
        /// <param name="_GuessId"></param>
        /// <returns></returns>
        public List<ChamCadModel> GetCandisList(string _GuessId)
        {
            List<ChamCadModel> list = new List<ChamCadModel>();
            string sql = "select * from wtf_ChamCad where GuessId='" + _GuessId + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            dt.Columns.Add("FollowQty", typeof(int));
            if (dt.Rows.Count > 0)
            {
               
                foreach (DataRow dr in dt.Rows )
                {
                    dr["FollowQty"] = ChampGuessResultBiz.instance.GetCandidateFQty(dr["id"].ToString()); 
                }

                DataView dv = dt.DefaultView;
                dv.Sort = "FollowQty desc";

                list = JsonHelper.ParseDtModelList<List<ChamCadModel>>(dv.ToTable());
            }

            return list;
        }
    }
}
