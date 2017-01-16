using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Basic
{
    public class Biz_Basic_Service
    {
        public static Biz_Basic_Service instance = new Biz_Basic_Service();

        #region 基础接口
        /// <summary>
        /// 添加新的角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(Model_Basic_Service model)
        {
            string sql = string.Format("insert into Basic_Service (ServiceSys,ServiceType,ServiceName,ServiceDesc,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", model.SERVICESYS, model.SERVICETYPE, model.SERVICENAME, model.SERVICEDESC, model.EXT1, model.EXT2, model.EXT3, model.EXT4, model.EXT5);
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
        public Model_Basic_Service GetModelSys(string _Sys)
        {
            Model_Basic_Service model = new Model_Basic_Service();
            string sql = "select * from Basic_Service where ServiceSys='" + _Sys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<Model_Basic_Service>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据接口名称
        /// </summary>
        /// <param name="_ServiceName"></param>
        /// <returns></returns>
        public string GetServiceSys(string _ServiceName)
        {
            string _ServiceSys = "";
            string sql = "select ServiceSys from Basic_Service where ServiceName='"+_ServiceName+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                _ServiceSys = dt.Rows[0][0].ToString();
            }
            return _ServiceSys;
        }

        #endregion
    }
}
