using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Basic
{
    public class Biz_Basic_ServiceHistory
    {
        public static Biz_Basic_ServiceHistory instance = new Biz_Basic_ServiceHistory();

        #region 基础接口
        /// <summary>
        /// 添加新的角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(Model_Basic_ServiceHistory model)
        {
            string sql = string.Format("insert into Basic_ServiceHistory (Userid,ServiceName,UpdateTime,Status,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", model.USERID, model.SERVICENAME, model.UPDATETIME,model.EXT1, model.EXT2, model.EXT3, model.EXT4, model.EXT5);
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
        /// 根据userid获得接口调用历史记录
        /// </summary>
        /// <param name="_Userid"></param>
        /// <param name="_Secret"></param>
        /// <returns></returns>
        public List<Model_Basic_ServiceHistory> GetUserOriginList(string _Userid)
        {
            List<Model_Basic_ServiceHistory> list = new List<Model_Basic_ServiceHistory>();
            string sql = "select * from Basic_ServiceHistory where Userid='" + _Userid + "' order by id desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<Model_Basic_ServiceHistory>>(dt);
            }
            return list;
        }

        #endregion
                
       
    }
}
