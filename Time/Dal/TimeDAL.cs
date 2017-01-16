using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Member;

namespace Time
{
    public class TimeDAL
    {
        public static TimeDAL instance = new TimeDAL();

        /// <summary>
        /// 添加新的时光,成功，返回Sys
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string InsertNew(TimeModel model)
        {
            string Sys = Guid.NewGuid().ToString("N").ToUpper();
            string sql = string.Format("insert into wtf_Time (sys,memsys,type,matchsys,Description,UpdateTime,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",Sys,model.MEMSYS,model.TYPE,model.MATCHSYS,model.DESCRIPTION,DateTime.Now.ToString(),model.EXT1,model.EXT2,model.EXT3,model.EXT4,model.EXT5);
            WriteLog("InsertNewTime", model.MEMSYS);
            int a = WeTour.DbHelperSQL.ExecuteSql(sql);
            return Sys;
        }

        void WriteLog(string _Desc, string value1)
        {
            string sql = "insert into sys_setting (Enum,Descript,value,value2) values ('1101','" + _Desc + "','" + DateTime.Now.ToString() + "','" + value1 + "')";
            int a = WeTour.DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// 先指定时光主键
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string InsertNewwithSys(TimeModel model)
        {
            //判断是否存在
            if (!IsTimeExist(model.SYS))
            {
                string sql = string.Format("insert into wtf_Time (sys,memsys,type,matchsys,Description,UpdateTime,ext1,ext2,ext3,ext4,ext5) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", model.SYS, model.MEMSYS, model.TYPE, model.MATCHSYS, model.DESCRIPTION, DateTime.Now.ToString(), model.EXT1, model.EXT2, model.EXT3, model.EXT4, model.EXT5);
                int a = WeTour.DbHelperSQL.ExecuteSql(sql);
            }
            return model.SYS;
        }

        public bool IsTimeExist(string _Sysno)
        {
            string sql = "select * from wtf_Time where sys='"+_Sysno+"'";
            DataTable dt = WeTour.DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据会员主键和类型，获得时光列表,按照添加顺序倒序排列,IsOpen为1的时候，表示获得公开的内容，否则获得全部的时光
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public List<TimeModel> GetListbyMemsys(string _Memsys,string _IsOpen)
        {
            List<TimeModel> list = new List<TimeModel>();
            string sql = "select * from wtf_Time where memsys='"+_Memsys+"' ";
            if (_IsOpen == "1")
            { 
                //仅显示公开的内容
                sql += " and ext1='1'";
            }
            sql+=" order by id desc";
            DataTable dt = WeTour.DbHelperSQL.Query(sql).Tables[0];
            list = WeTour.JsonHelper.ParseDtModelList<List<TimeModel>>(dt);
            return list;
        }

        /// <summary>
        /// 获得指定的信息
        /// </summary>
        /// <param name="_PageSize"></param>
        /// <param name="_Page"></param>
        /// <returns></returns>
        public List<TimeModel> GetPicContestList(int _PageSize,int _Page,string _OrderType)
        {
            List<TimeModel> list = new List<TimeModel>();
            int start = (_Page - 1) * _PageSize;
            int End = _Page*_PageSize;
            string sql = "";
            string datasql="";
            if (_OrderType == "Time")
            {
                //按照更新时间排序
                datasql = "select *,row_number() over(order by id desc) as rn  from wtf_Time where ext1='2'";
            }
            else
            {
                //按照热度排序
                datasql = "select *,row_number() over(order by qty desc) as rn  from view_HotPicContext";
            }
            sql = string.Format("select * from ({0})a where a.rn>{1} and a.rn<={2}",datasql,start, End);

            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<TimeModel>>(dt);
                foreach (TimeModel model in list)
                { 
                    //添加图片链接
                    TimePicsModel picmodel = TimePicDal.instance.GetTimePics(model.SYS)[0];
                    model.EXT3 = picmodel.PICURL;//
                    //添加like数量
                    model.EXT4 = SMS.PraiseBll.instance.CountPraiseQty("pic", model.SYS, "1").ToString();
                }
            }
            return list;
        }


        /// <summary>
        /// 根据主键获得时光实体
        /// </summary>
        /// <param name="_TimeSys"></param>
        /// <returns></returns>
        public TimeModel GetModlebySys(string _TimeSys)
        {
            TimeModel model = new TimeModel();
            string sql = "select * from wtf_Time where sys='"+_TimeSys+"'";
            DataTable dt = WeTour.DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = WeTour.JsonHelper.ParseDtInfo<TimeModel>(dt);
                //添加用户名
                WeMemberModel mem = WeMemberDll.instance.GetModel(model.MEMSYS);
                model.EXT4 = mem.NAME;
                model.EXT5 = mem.EXT1;

                //添加照片
                List<TimePicsModel> list = TimePicDal.instance.GetTimePics(model.SYS);
                model.EXT3 = list[0].PICURL;//将图片放在EXT3备用字段中                
            }
            return model;
        }

        /// <summary>
        /// 根据员工主键获得时光实体
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public TimeModel GetModelByMemsys(string _Memsys)
        {
            TimeModel model = new TimeModel();
            string sql = "select top 1 * from wtf_Time where memsys='" + _Memsys + "' and ext1='2' order by id desc";
            DataTable dt = WeTour.DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = WeTour.JsonHelper.ParseDtInfo<TimeModel>(dt);

                //添加用户名
                WeMemberModel mem = WeMemberDll.instance.GetModel(model.MEMSYS);
                model.EXT4 = mem.NAME;
                model.EXT5 = mem.EXT1;

                //添加照片
                List<TimePicsModel> list = TimePicDal.instance.GetTimePics(model.SYS);
                model.EXT3 = list[0].PICURL;//将图片放在EXT3备用字段中
            }
            return model;
        }

        /// <summary>
        /// 删除作品，将ext1的状态值变为99；
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        public bool DeletePicWork(string sys)
        {
            string sql = "update wtf_Time set ext1='99' where sys='"+sys+"'";
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
    }
}
