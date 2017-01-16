using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Equip
{
    public class MyWaresBiz
    {
        public static MyWaresBiz instance = new MyWaresBiz();

        /// <summary>
        /// Insert Member's new ware
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(MyWaresModel model)
        {
            string sql = string.Format("insert into Equ_Mywares (Memsysno,WareId,StartDate,IsUsual,IsActive,CustPro1,CustPro2,CustPro3,CustPro4) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}') ",model.Memsysno,model.WareId,model.StartDate,model.IsUsual,model.IsActive,model.CustPro1,model.CustPro2,model.CustPro3,model.CustPro4);
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteMyWare(string id)
        {
            string sql = "delete Equ_Mywares where id='"+id+"'";
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
        /// Get model by id
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public MyWaresModel GetModel(string _id)
        {
            MyWaresModel model = new MyWaresModel();
            string sql = "select * from Equ_Mywares where id='"+_id+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<MyWaresModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// count my equip qty by type
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public string GetmyEquiQty(string _type,string _Memsys)
        {
            string sql = "select count(a.id) from equ_mywares a left join Equ_WareMain b on a.wareid=b.id where b.type='" + _type + "' and Memsysno='" + _Memsys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows[0][0].ToString();
        }

        /// <summary>
        /// Get Gear list by type and memsys
        /// </summary>
        /// <param name="_Type"></param>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public List<WareMainModel> GetMyGearsbyType(string _Type, string _Memsys)
        {
            List<WareMainModel> list = new List<WareMainModel>();
            string sql = "select b.* from equ_mywares a left join Equ_WareMain b on a.wareid=b.id where b.type='" + _Type + "' and Memsysno='" + _Memsys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WareMainModel>>(dt);
            }
            return list;
        }

        #region 根据前端需要

        /// <summary>
        /// 修改前端传回的个人装备信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateMyware(MyWaresModel model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("update Equ_Mywares set ");
            sb.Append("CustPro1='" + model.CustPro1 + "',");
            sb.Append("CustPro2='" + model.CustPro2 + "',");
            sb.Append("CustPro3='" + model.CustPro3 + "',");
            sb.Append("CustPro4='" + model.CustPro4 + "'");
            sb.Append(" where id='" + model.id + "'");
            int a = DbHelperSQL.ExecuteSql(sb.ToString());
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
        /// 获取我的所有装备
        /// </summary>
        /// <param name="_memsys"></param>
        /// <returns></returns>
        public List<MyWaresModel> getMywareList(string _memsys)
        {
            List<MyWaresModel> equip_list = new List<MyWaresModel>();
            string sql = "select * from Equ_Mywares where Memsysno='"+_memsys+"'";
            DataTable dt= DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                equip_list = JsonHelper.ParseDtModelList<List<MyWaresModel>>(dt);
            }
            return equip_list;
        }
        #endregion
    }
}
