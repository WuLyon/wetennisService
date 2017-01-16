using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class WeTourContentDll
    {
        public static WeTourContentDll instance = new WeTourContentDll();

        /// <summary>
        /// Get Tour Contents
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<WeTourContModel> GetContentlist(string _Toursys)
        {
            List<WeTourContModel> list = new List<WeTourContModel>();
            string sql = "select * from wtf_tourcontent where toursys='"+_Toursys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourContModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 根据id获得实体
        /// </summary>
        /// <param name="_ContId"></param>
        /// <returns></returns>
        public WeTourContModel GetModelbyId(string _ContId)
        {
            WeTourContModel model = new WeTourContModel();
            string sql = "select * from wtf_tourcontent where id='"+_ContId+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<WeTourContModel>(dt);

                //10月10日，针对老赛事，无组别的，默认添加一个组别。使得老赛事也可以在新的
                //会员首页中显示，但是可能会影响到排名。
                model.TourDate = "青年组";
            }
            return model;
        }

        /// <summary>
        /// 根据项目名称获取项目实体
        /// </summary>
        /// <param name="_ContName"></param>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public WeTourContModel GetModelbyContName(string _ContName, string _Toursys)
        {
            WeTourContModel model = new WeTourContModel();
            string sql = "select * from wtf_tourcontent where contentName='" + _ContName + "' and toursys='" + _Toursys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<WeTourContModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// Get tourcontent apply fee
        /// </summary>
        /// <param name="_Contid"></param>
        /// <returns></returns>
        public decimal GetContentApplyFee(string _Contid)
        {
            decimal applyFee = 0;
            WeTourContModel tcmodel = GetModelbyId(_Contid);
            if (tcmodel.ext3 == "" || tcmodel.ext3 == null)
            {
                WeTourModel tmodel = WeTourDll.instance.GetModelbySys(tcmodel.Toursys);
                applyFee = Convert.ToDecimal(tmodel.EXT1);
            }
            else
            {
                applyFee = Convert.ToDecimal(tcmodel.ext3);
            }
            return applyFee;
        }

        #region 基础操作
        /// <summary>
        /// 添加新的Content
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNewContent(WeTourContModel model)
        {
            string sql = string.Format("insert into wtf_Tourcontent (Toursys,ContentName,ContentType,SignQty,AllowGroup,GroupType,tourDate,ext1,ext2,ext3,ext4,id) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",model.Toursys,model.ContentName,model.ContentType,model.SignQty,model.AllowGroup,model.GroupType,model.TourDate,model.ext1,model.ext2,model.ext3,model.ext4,Guid.NewGuid().ToString("N") );
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
        /// 修改项目内容
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateContent(WeTourContModel model)
        {
            string sql = string.Format("Update wtf_Tourcontent set ContentName='{0}',ContentType='{1}',SignQty='{2}',AllowGroup='{3}',GroupType='{4}',tourDate='{5}',ext1='{6}',ext2='{7}',ext3='{8}',ext4='{9}' where id='{10}'", model.ContentName, model.ContentType, model.SignQty, model.AllowGroup, model.GroupType, model.TourDate, model.ext1, model.ext2, model.ext3, model.ext4,model.id);
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
        /// 删除项目
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public bool DeleteContent(string _ContentId)
        {
            string sql = "delete wtf_Tourcontent where id='"+_ContentId+"'";
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
        #endregion

        #region 组及项目
        /// <summary>
        /// 根据赛事id，获取所有组别
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        public List<WeTourContModel> GetGroups(string _TourSys)
        {
            List<WeTourContModel> list = new List<WeTourContModel>();
            string sql = "select distinct(TourDate) from wtf_tourcontent where toursys='"+_TourSys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            list = JsonHelper.ParseDtModelList<List<WeTourContModel>>(dt);
            return list;
        }

        /// <summary>
        /// 根据组获取项目
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <param name="_Group"></param>
        /// <returns></returns>
        public List<WeTourContModel> GetcontentbyGroup(string _TourSys, string _Group)
        {
            List<WeTourContModel> list = new List<WeTourContModel>();
            string sql = "select * from wtf_tourcontent where toursys='"+_TourSys+"' and TourDate='"+_Group+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            list = JsonHelper.ParseDtModelList<List<WeTourContModel>>(dt);
            return list;
        }
        #endregion

        #region 赛事签表及轮次
        public string GetLatestRound(string _ItemId)
        {
            string _LatestRound = "";
            string sql = "select distinct(round) from wtf_match where contentid='"+_ItemId+"' and state<>2";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                _LatestRound = dt.Rows[0][0].ToString();
            }
            else
            { 
                //默认展示最大轮次
                string sql1 = "select max(round) from wtf_match where contentid='" + _ItemId + "'";
                DataTable dt2 = DbHelperSQL.Query(sql1).Tables[0];
                _LatestRound = dt2.Rows[0][0].ToString();
            }
            return _LatestRound;
        }

        /// <summary>
        /// 获取项目的轮次信息
        /// </summary>
        /// <param name="_Contid"></param>
        /// <returns></returns>
        public List<Dictionary<string, string>> GetcontentRounds(string _Contid)
        {
            List<Dictionary<string, string>> round_list = new List<Dictionary<string, string>>();
            List<WeMatchModel> match_list = WeMatchDll.instance.GetContentRoundsAsc(_Contid);
            foreach(WeMatchModel match in match_list)
            {
                Dictionary<string, string> item = new Dictionary<string, string>();
                item.Add("text", match.RoundName);
                item.Add("value",match.ROUND.ToString());
                round_list.Add(item);
            }
            return round_list;
        }

        public bool IfRoundLatest(string _ItemId, string _Round)
        {
            string _LatestRound = GetLatestRound(_ItemId);
            if (_LatestRound == _Round)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
