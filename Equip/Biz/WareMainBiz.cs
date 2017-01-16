using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Equip
{
    public class WareMainBiz
    {
        public static WareMainBiz instance = new WareMainBiz();

        /// <summary>
        /// Insert new gear
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(WareMainModel model)
        {
            string sql = string.Format("insert into Equ_WareMain (type,brand,model,pro1,pro2,pro3,pro4,pro5,pro6,pro7,pro8,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')",model.type,model.brand,model.model,model.pro1,model.pro2,model.pro3,model.pro4,model.pro5,model.pro6,model.pro7,model.pro8,model.ext1,model.ext2,model.ext3,model.ext4,model.ext5);
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
        /// get ware model by id
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public WareMainModel GetModel(string _id)
        {
            WareMainModel model = new WareMainModel();
            string sql = "select * from Equ_WareMain where id='" + _id + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<WareMainModel>(dt);
            }
            return model;
        }


        /// <summary>
        /// Get Gears list by Type
        /// </summary>
        /// <param name="_Type"></param>
        /// <returns></returns>
        public List<WareMainModel> GetModellistbyType(string _Type)
        {
            List<WareMainModel> list = new List<WareMainModel>();
            string sql = "select * from Equ_WareMain where type='" + _Type + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WareMainModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Get Equipment Type
        /// </summary>
        /// <returns></returns>
        public List<WareMainModel> GetdistinctType()
        {
            List<WareMainModel> list = new List<WareMainModel>();
            string sql = "select distinct(type) from Equ_WareMain";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WareMainModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Get distinct Brands 
        /// </summary>
        /// <param name="_Type"></param>
        /// <returns></returns>
        public List<WareMainModel> GetBrandsByType(string _Type)
        {
            List<WareMainModel> list = new List<WareMainModel>();
            string sql = "select distinct(brand) from Equ_WareMain where type='" + _Type + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WareMainModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Get distinct Models by brand 
        /// </summary>
        /// <param name="_Type"></param>
        /// <returns></returns>
        public List<WareMainModel> GetModelsByBrand(string _Brand)
        {
            List<WareMainModel> list = new List<WareMainModel>();
            string sql = "select distinct(model) from Equ_WareMain where brand='" + _Brand + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WareMainModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetWareIdbyCond(WareMainModel model)
        {
            string id = "";
            string sql = "select * from Equ_WareMain where type='"+model.type+"' and brand='"+model.brand+"' and model='"+model.model+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                id = dt.Rows[0]["id"].ToString();
            }
            return id;
        }

        /// <summary>
        /// Add to my wars form add ware page
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddTomyEqu(WareMainModel model)
        {
            string id = GetWareIdbyCond(model);
            MyWaresModel model1 = new MyWaresModel();
            model1.Memsysno = model.Memsysno;
            model1.WareId = id;
            model1.StartDate = model.ext1;
            return MyWaresBiz.instance.InsertNew(model1);            
        }


    }
}
