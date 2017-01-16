using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Basic
{
    public class Biz_Basic_RoleAuth
    {
        public static Biz_Basic_RoleAuth instance = new Biz_Basic_RoleAuth();

        #region 基础接口
        /// <summary>
        /// 添加新的角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(Model_Basic_RoleAuth model)
        {
            string sql = string.Format("insert into Basic_RoleAuth (ServiceSys,AuthType,ObjSys,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", model.SERVICESYS, model.AUTHTYPE, model.OBJSYS, model.EXT1, model.EXT2, model.EXT3, model.EXT4, model.EXT5);
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

        #region 业务方法
        /// <summary>
        /// 接口，角色是否存在
        /// </summary>
        /// <param name="_RoleSys"></param>
        /// <param name="_ServiceSys"></param>
        /// <returns></returns>
        public bool IsAuthExsit(string _RoleSys, string _ServiceSys)
        {
            string sql = "select * from Basic_RoleAuth where ServiceSys='" + _RoleSys + "' and ObjSys='" + _ServiceSys + "'";
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
        #endregion
    }
}
