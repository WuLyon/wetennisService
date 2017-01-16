using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class CourtResourceDll
    {
        public static CourtResourceDll instance = new CourtResourceDll();

        #region 增删读的方法
        /// <summary>
        /// 添加新的分配方案
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(CourtResourceModel model)
        {
            string sql = string.Format("insert into Wtf_CourtDisForGroups (ResSys,Toursys,Courts,ContentId,Groups,ext1,ext2,ext3) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",model.ResSys,model.Toursys,model.Courts,model.ContentId,model.Groups,model.ext1,model.ext2,model.ext3);
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
        /// 删除赛事的资源分配方案
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public bool DeleteCourts(string _Toursys)
        {
            string sql = "delete Wtf_CourtDisForGroups where toursys='"+_Toursys+"'";
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
        /// 获得资源分配实体
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public CourtResourceModel GetModel(string _id)
        {
            CourtResourceModel model = new CourtResourceModel();
            string sql = "select * from Wtf_CourtDisForGroups where id='"+_id+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<CourtResourceModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据资源分配主键，获得资源分配实体
        /// </summary>
        /// <param name="_ResSys"></param>
        /// <returns></returns>
        public List<CourtResourceModel> GetModellistbyResSys(string _ResSys)
        {
            List<CourtResourceModel> list = new List<CourtResourceModel>();
            string sql = "select * from Wtf_CourtDisForGroups where ResSys='"+_ResSys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<CourtResourceModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 根据赛事主键，获得该赛事的资源分配方案
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<CourtResourceModel> GetResSysbyToursys(string _Toursys)
        {
            List<CourtResourceModel> list = new List<CourtResourceModel>();
            string sql = "select distinct(ResSys) from Wtf_CourtDisForGroups where Toursys='" + _Toursys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<CourtResourceModel>>(dt);
            }
            return list;
        }
        #endregion
    }
}
