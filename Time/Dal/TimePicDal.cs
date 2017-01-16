using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Time
{
    public class TimePicDal
    {
        public static TimePicDal instance = new TimePicDal();

        /// <summary>
        /// 插入新的时光图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNewTimePic(TimePicsModel model)
        {
            string sql = string.Format("insert into wtf_TimePics(timesys,picurl,updatetime,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",model.TIMESYS,model.PICURL,DateTime.Now.ToString(),model.EXT1,model.EXT2,model.EXT3,model.EXT4,model.EXT5);
            int a = WeTour.DbHelperSQL.ExecuteSql(sql);
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
        /// 根据时光主键获得图片列表
        /// </summary>
        /// <param name="_TimeSys"></param>
        /// <returns></returns>
        public List<TimePicsModel> GetTimePics(string _TimeSys)
        {
            List<TimePicsModel> list = new List<TimePicsModel>();
            string sql = "select * from wtf_TimePics where timesys='" + _TimeSys + "'";
            DataTable dt = WeTour.DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = WeTour.JsonHelper.ParseDtModelList<List<TimePicsModel>>(dt);
                //render pic url
                foreach (TimePicsModel model in list)
                {
                    if (model.PICURL.IndexOf("http://") < 0)
                    {
                        model.PICURL = "http://wetennis.cn:86" + model.PICURL;
                    }
                }
            }
            return list;
        }
    }
}
