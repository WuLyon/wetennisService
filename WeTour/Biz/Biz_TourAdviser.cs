using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class Biz_TourAdviser
    {
        public static Biz_TourAdviser instance = new Biz_TourAdviser();

        /// <summary>
        /// 添加新的赞助商信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(Model_WeTourAdvertiser model)
        {
            string sysno = Guid.NewGuid().ToString("N");
            string sql=string.Format("insert into wtf_TourAdvertiser (Toursys,Sysno,AdvertiserName,AdvertiserUrl,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",model.TourSys,sysno,model.AdvertiserName,model.AdvertiserUrl,model.ext1,model.ext2,model.ext3,model.ext4,model.ext5);
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
        /// 根据赛事主键获得赞助商信息
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<Model_WeTourAdvertiser> GetAdvertiserbyToursys(string _Toursys)
        {
            List<Model_WeTourAdvertiser> list = new List<Model_WeTourAdvertiser>();
            string sql = "select * from wtf_TourAdvertiser where toursys='"+_Toursys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<Model_WeTourAdvertiser>>(dt);
            }
            return list;
        }
    }
}
