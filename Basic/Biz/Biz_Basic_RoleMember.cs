using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Basic
{
    public class Biz_Basic_RoleMember
    {
        public static Biz_Basic_RoleMember instance = new Biz_Basic_RoleMember();

        #region 基础接口
        /// <summary>
        /// 添加新的角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(Model_Basic_RoleMember model)
        {
            string sql = string.Format("insert into Basic_RoleMember (RoleSys,MemberType,Memsys,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", model.ROLESYS, model.MEMBERTYPE, model.MEMSYS, model.EXT1, model.EXT2, model.EXT3, model.EXT4, model.EXT5);
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
        /// 根据用户主键获取角色主键
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public string GetRoleSys(string _Memsys)
        {
            string sql = "select a.RoleSys from Basic_RoleMember a left join Basic_Role b on a.RoleSys=b.RoleSys where a.Memsys='" + _Memsys + "' and b.RoleType='service'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows[0][0].ToString();
        }
        #endregion
    }
}
