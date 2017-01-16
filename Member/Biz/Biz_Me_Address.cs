using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Member
{
    public class Biz_Me_Address
    {
        public static Biz_Me_Address intance = new Biz_Me_Address();

        #region 地址操作
        public bool DeleteMyAddress(string _UserId)
        {
            string sql = "delete Me_Address where memsys='"+_UserId+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return true ;
            }
            else
            {
                return false;
            }
        }

        public string AddNewString(Model_Me_Address model)
        {
            model.sysno = Guid.NewGuid().ToString("N");
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into Me_Address values (");
            sb.Append("'"+model.sysno+"',");//
            sb.Append("'" + model.memsys + "',");//
            sb.Append("'" + model.name + "',");//
            sb.Append("'" + model.phone + "',");//
            sb.Append("'" + model.district + "',");//
            sb.Append("'" + model.detailAddress + "',");//
            sb.Append("'" + model.post + "',");//
            sb.Append("'" + model.isDefault + "',");//
            sb.Append("'" + model.is_Active + "')");//
            string sql = sb.ToString();
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return model.sysno;
            }
            else
            {
                return "";
            }
        }

        //获取地址列表
        public List<Model_Me_Address> GetMyAddress(string _userId)
        {
            List<Model_Me_Address> addlist = new List<Model_Me_Address>();
            string sql = "select * from Me_Address where memsys='"+_userId+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            addlist = JsonHelper.ParseDtModelList<List<Model_Me_Address>>(dt);
            return addlist;
        }
        #endregion
    }
}
