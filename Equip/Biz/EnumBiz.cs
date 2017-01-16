using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Equip
{
    public class EnumBiz
    {
        public static EnumBiz instance = new EnumBiz();

        /// <summary>
        /// 获得清单
        /// </summary>
        /// <param name="_Enum"></param>
        /// <returns></returns>
        public List<EnmuModel> GetEnumlist(string _Enum)
        {
            List<EnmuModel> list = new List<EnmuModel>();
            string sql = "select * from Equ_Enum where enum='"+_Enum+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<EnmuModel>>(dt);
            }
            return list;
        }
    }
}
