using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data;

namespace Basic
{
    public class Biz_Basic_PageURL
    {
        public static Biz_Basic_PageURL instance = new Biz_Basic_PageURL();

        #region 基础接口
        /// <summary>
        /// 添加新的角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(Model_Basic_PageURL model)
        {
            string sql = string.Format("insert into Basic_PageURL (RageSys,PageType,PageURL,PageDesc,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", model.PAGESYS, model.PAGETYPE, model.PAGEURL,model.PAGEDESC, model.EXT1, model.EXT2, model.EXT3, model.EXT4, model.EXT5);
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
        /// 根据sys的实体
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public Model_Basic_PageURL GetModelSys(string _Sys)
        {
            Model_Basic_PageURL model = new Model_Basic_PageURL();
            string sql = "select * from Basic_PageURL where PageSys='"+_Sys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<Model_Basic_PageURL>(dt);
            }
            return model;
        }



        #endregion
    }
}
