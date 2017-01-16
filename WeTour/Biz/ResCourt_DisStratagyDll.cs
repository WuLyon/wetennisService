using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class ResCourt_DisStratagyDll
    {
        public static ResCourt_DisStratagyDll instance = new ResCourt_DisStratagyDll();

        #region CRUD
        /// <summary>
        /// 插入新纪录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(ResCourt_DisStratagyModel model)
        {
            string sql = string.Format("insert into ResCourt_DistryStratagy (Toursys,Gymid,Courtids,ContentId,IsGroup,GroupId,MatchDate,Round,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')",model.TOURSYS,model.GYMID,model.COURTIDS,model.CONTENTID.Replace("'","r"),model.ISGROUP,model.GROUPID,model.MATCHDATE,model.ROUND,model.EXT1,model.EXT2,model.EXT3,model.EXT4,model.EXT5);
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
        /// 根据条件获得分配资源
        /// </summary>
        /// <param name="_Condition"></param>
        /// <returns></returns>
        public List<ResCourt_DisStratagyModel> GetCourtsStratagy(string _Condition)
        {
            List<ResCourt_DisStratagyModel> list = new List<ResCourt_DisStratagyModel>();
            string sql = "select * from ResCourt_DistryStratagy where 1=1 "+_Condition;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<ResCourt_DisStratagyModel>>(dt);
                foreach (ResCourt_DisStratagyModel model in list)
                {
                    model.CONTENTID = model.CONTENTID.Replace("r", "'");
                }
            }
            return list;
        }

        /// <summary>
        /// 删除已分配策略
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        public bool DeleteStratagebyTourSys(string _TourSys)
        {
            string sql = "delete ResCourt_DistryStratagy where toursys='" + _TourSys + "'";
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

        #endregion
    }
}
