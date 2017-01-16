using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Basic
{
    public class Biz_Basic_UserOrigin
    {
        public static Biz_Basic_UserOrigin instance = new Biz_Basic_UserOrigin();

        #region 基础接口
        /// <summary>
        /// 添加新的角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(Model_Basic_UserOrigin model)
        {
            string sql = string.Format("insert into Basic_UserOrigin (Memsys,OriginName,Status,Userid,Secret,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", model.MEMSYS, model.ORIGINNAME, model.STATUS,model.USERID,model.SECRET, model.EXT1, model.EXT2, model.EXT3, model.EXT4, model.EXT5);
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
        /// 根据接口userid和secret
        /// </summary>
        /// <param name="_Userid"></param>
        /// <param name="_Secret"></param>
        /// <returns></returns>
        public Model_Basic_UserOrigin GetUserOrigin(string _Userid, string _Secret)
        {
            Model_Basic_UserOrigin model = new Model_Basic_UserOrigin();
            string sql = "select * from Basic_UserOrigin where Userid='"+_Userid+"' and Secret='"+_Secret+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<Model_Basic_UserOrigin>(dt);
            }
            return model;
        }

        #endregion

        #region 业务方法

        /// <summary>
        /// 根据用户信息来获取域名,返回值是空值不能
        /// </summary>
        /// <param name="_UserId"></param>
        /// <param name="_Secret"></param>
        /// <returns></returns>
        public string GetUserOriginbyCredential(string _UserId, string _Secret,string _ServiceName)
        {
            string _Origin = "";
            Model_Basic_UserOrigin model = GetUserOrigin(_UserId, _Secret);//根据uid和secret获取实体
            if (!string.IsNullOrEmpty(model.ORIGINNAME))
            {
                //认证用户信息是否准确
                //根据Memsys获取角色
                string _RoleSys = Biz_Basic_RoleMember.instance.GetRoleSys(model.MEMSYS);

                //判断是否具有接口的权限
                string _ServiceSys=Biz_Basic_Service.instance.GetServiceSys(_ServiceName);
                if (Biz_Basic_RoleAuth.instance.IsAuthExsit(_RoleSys, _ServiceSys))
                {
                    //存在接口权限
                    _Origin = model.ORIGINNAME;
                }
            }
            return _Origin;
        }
        #endregion
    }
}
