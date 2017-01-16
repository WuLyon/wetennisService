using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Basic
{
    public class Biz_Basic_Role
    {
        public static Biz_Basic_Role instance = new Biz_Basic_Role();

        #region 基础接口
        /// <summary>
        /// 添加新的角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string InsertNew(Model_Basic_Role model)
        {
            model.ROLESYS = Guid.NewGuid().ToString("N");
            string sql = string.Format("insert into Basic_Role (RoleSys,RoleName,RoleType,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",model.ROLESYS,model.ROLENAME,model.ROLETYPE,model.EXT1,model.EXT2,model.EXT3,model.EXT4,model.EXT5);
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return model.ROLESYS;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 根据主键获得角色
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public Model_Basic_Role GetModelbySys(string _Sys)
        {
            Model_Basic_Role model = new Model_Basic_Role();
            string sql = "select * from Basic_Role where RoleSys='"+_Sys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<Model_Basic_Role>(dt);
            }
            return model;
        }
        #endregion
    }
}
