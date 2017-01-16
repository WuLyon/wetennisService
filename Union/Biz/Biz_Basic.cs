using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Union
{
    public class Biz_Basic
    {
        public static Biz_Basic instance = new Biz_Basic();

        #region CRUD
        /// <summary>
        /// add a new Union and return sys
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string InsertRtnSys(Model_Basic model)
        {
            string sys = Guid.NewGuid().ToString("N");
            string sql = string.Format("insert into Union_Basic (UnionSys,UnionName,UnionDesc,CreateBy,CreateClub,CreateDate,Status,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',)",sys,model.UnionName,model.UnionDesc,model.UnionDesc,model.CreateBy,model.CreateClub,model.CreateDate,model.Status,model.ext1,model.ext2,model.ext3,model.ext4,model.ext5);
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return sys;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Update field Info
        /// </summary>
        /// <param name="_UnionSys"></param>
        /// <param name="_FieldName"></param>
        /// <param name="_FieldValue"></param>
        /// <returns></returns>
        public bool UpdateInfo(string _UnionSys, string _FieldName, string _FieldValue)
        {
            string sql = string.Format("update Union_Basic set {0}='{1}' where UnionSys='{2}'",_FieldName,_FieldValue,_UnionSys);
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
        /// update a union state
        /// </summary>
        /// <param name="_UnionSys"></param>
        /// <param name="_Status"></param>
        /// <returns></returns>
        public bool UpdateUnionStatus(string _UnionSys, string _Status)
        {
            string sql = string.Format("Update Union_Basic set Status='{0}' where UnionSys='{1}'",_Status,_UnionSys);
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

        public Model_Basic GetUnionInfobySys(string _UnionSys)
        {
            Model_Basic model = new Model_Basic();
            string sql = "select * from Union_Basic where UnionSys='"+_UnionSys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<Model_Basic>(dt);
            }
            return model;
        }

        #endregion
    }
}
