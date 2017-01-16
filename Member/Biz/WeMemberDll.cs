using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Member
{
    public class WeMemberDll
    {
        public static WeMemberDll instance = new WeMemberDll();
        string ImgSer = System.Configuration.ConfigurationManager.AppSettings["ImageServer"].ToString();
        /// <summary>
        /// 添加新会员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string CreateUser(WeMemberModel model)
        {
            //判断是否已存在
            string sysno=Guid.NewGuid().ToString();
            string sql = string.Format("insert into wtf_member (Sysno,Name,Provinceid,province,CityId,City,RegionId,Region,Address,BirthDay,RegisterProId,RegisterPro,RegisterCityId,RegisterCity,RegisterRegId,RegisterReg,RegisterDate,MemberType,Gender,Telephone,Email,TennisStartYear,Description,Forehand,Backhand,Height,Weight,job,status,username,password,ext3,ext1) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{2}','{3}','{4}','{5}','{6}','{7}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','1','{22}','{23}','{24}','{25}')", sysno, model.NAME, model.PROVINCEID, model.PROVINCE, model.CITYID, model.CITY, model.REGIONID, model.REGION, model.ADDRESS, model.BIRTHDAY, DateTime.Now.ToString("yyyy-MM-dd"), "1", model.GENDER, model.TELEPHONE, model.EMAIL, model.TENNISSTARTYEAR, model.DESCRIPTION, model.FOREHAND, model.BACKHAND, model.HEIGHT, model.WEIGHT, "会员", model.USERNAME, model.PASSWORD, model.EXT3,model.EXT1);

            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {               
                return sysno;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 修改会员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateUser(WeMemberModel model)
        {            
            StringBuilder sb = new StringBuilder();
            sb.Append("update wtf_member");
            sb.Append(" set Name='"+model.NAME+"',");
            sb.Append(" Height='"+model.HEIGHT+"',");
            sb.Append(" Weight='" + model.WEIGHT + "',");
            sb.Append(" province='"+model.PROVINCE+"',");
            sb.Append(" City='" + model.CITY + "',");
            sb.Append(" Region='" + model.REGION + "',");
            sb.Append(" Address='"+model.ADDRESS+"',");
            sb.Append(" BirthDay='"+model.BIRTHDAY+"',");
            sb.Append(" Gender='"+model.GENDER+"',");
            sb.Append(" Telephone='"+model.TELEPHONE+"',");
            sb.Append(" Email='"+model.EMAIL+"',");
            sb.Append(" TennisStartYear='"+model.TENNISSTARTYEAR+"',");
            sb.Append(" Forehand='"+model.FOREHAND+"',");
            sb.Append(" Backhand='"+model.BACKHAND+"',");
            sb.Append(" ext2='" + model.EXT2 + "',");
            sb.Append(" ext4='"+model.EXT4+"',");
            sb.Append(" ext5='" + model.EXT5 + "',");
            sb.Append(" ext6='" + model.EXT6 + "'");
            sb.Append(" where Sysno='"+model.SYSNO+"'");

            string sql = sb.ToString();
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        //判断用户名是否重复
        public bool IsUserNameUniqu(string _Name)
        {
            string sql = string.Format(" select * from wtf_member where  name='{0}'", _Name);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public WeMemberModel GetModel(string _Sys)
        {
            WeMemberModel model = new WeMemberModel();
            string sql = string.Format(" select * from wtf_member where  sysno='{0}'", _Sys);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<WeMemberModel>(dt);
            }
            else
            { 
                //未找到人员
                model.NAME = "未知";
                model.USERNAME = "未知";
            }

            //处理空头像
            if (model.EXT1 == null || model.EXT1 == "")
            {
                model.EXT1 = "/images/touxiang.jpg";
            }

            //为头像添加服务器地址
            if (model.EXT1.IndexOf("http") < 0)
            {
                model.EXT1 = ImgSer + model.EXT1;
            }

            //处理背景图
            if (model.EXT7 == null || model.EXT7 == "")
            {
                model.EXT7 = "/images/touxiang.jpg";
            }

            if (model.EXT7.IndexOf("http") < 0)
            {
                model.EXT7 = ImgSer + model.EXT7;
            }


            return model;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public WeMemberModel GetModelbyName(string _Name)
        {
            WeMemberModel model = new WeMemberModel();
            string sql = string.Format(" select * from wtf_member where  Name='{0}' or telephone='{0}'", _Name);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<WeMemberModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据电话号码获取实体
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public WeMemberModel GetModelbyTelephone(string _Tele)
        {
            WeMemberModel model = new WeMemberModel();
            string sql = string.Format(" select * from wtf_member where  Telephone='{0}'", _Tele);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<WeMemberModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据微信openid来获取用户实体
        /// </summary>
        /// <param name="_OpenId"></param>
        /// <returns></returns>
        public WeMemberModel GetModelbyOpenId(string _OpenId)
        {
            WeMemberModel model = new WeMemberModel();
            string sql = string.Format(" select * from wtf_member where  ext3='{0}'", _OpenId);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<WeMemberModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 判断openid是否已绑定用户
        /// </summary>
        /// <param name="_OpenId"></param>
        /// <returns></returns>
        public bool IsOpenIdExsit(string _OpenId)
        { 
            string sql = string.Format(" select * from wtf_member where  ext3='{0}'", _OpenId);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
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
        /// 验证用户信息
        /// </summary>
        /// <param name="_UserInfo"></param>
        /// <param name="_Pwd"></param>
        /// <returns></returns>
        public bool ValidateUser(string _UserInfo, string _Pwd)
        {
            if (string.IsNullOrEmpty(_UserInfo) || string.IsNullOrEmpty(_Pwd))
            {
                return false;
            }
            else
            {
                string sql = "select * from wtf_member where (name='" + _UserInfo + "' or Telephone='" + _UserInfo + "') and password='" + _Pwd + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 绑定用户信息
        /// </summary>
        /// <param name="_sysno"></param>
        /// <param name="_OpenId"></param>
        /// <returns></returns>
        public bool BindUser(string _sysno, string _OpenId)
        {
            string sql = "update wtf_member set ext3='" + _OpenId + "' where sysno='" + _sysno + "'";
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
        /// 修改电话号码
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <param name="_Telephone"></param>
        /// <returns></returns>
        private bool UpdateTelephone(string _Memsys, string _Telephone)
        {
            string sql = "update wtf_member set Telephone='"+_Telephone+"' where sysno='"+_Memsys+"'";
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
        /// <param name="_UserName"></param>
        /// <param name="_OpenId"></param>
        /// <param name="_Telephone"></param>
        /// <returns></returns>
        public bool UpdateUserTelephone(string _UserName, string _OpenId, string _Telephone)
        {
            WeMemberModel model = new WeMemberModel();
            if (!string.IsNullOrEmpty(_UserName))
            { 
                //根据用户名修改电话号码
                model = GetModelbyName(_UserName);
            }

            if (!string.IsNullOrEmpty(_OpenId))
            { 
                //根据openid修改电话号码
                model = GetModelbyOpenId(_OpenId);
            }

            if (!string.IsNullOrEmpty(model.SYSNO))
            {
                return UpdateTelephone(model.SYSNO, _Telephone.Trim());
            }
            else
            {
                return false;
            }
        }

        #region 验证用户电话和身份证是否正确
        /// <summary>
        /// 验证电话是否正确
        /// </summary>
        /// <param name="_Sysno"></param>
        /// <returns></returns>
        public bool IsTelephoneRight(string _Sysno)
        {
            WeMemberModel model = GetModel(_Sysno);
            if (model.TELEPHONE.Trim().Length == 11)
            {
                //验证电话是否重复
                string sql = "select * from wtf_member where telephone='"+_Sysno+"'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsphoneDuplicated(string _Phone)
        {
            if (_Phone.Length == 11)
            {
                //验证电话是否重复
                string sql = "select * from wtf_member where telephone='" + _Phone + "'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 验证身份证是否正确
        /// </summary>
        /// <param name="_Sysno"></param>
        /// <returns></returns>
        public bool IsIDcardRight(string _Sysno)
        {
            WeMemberModel model = GetModel(_Sysno);
            if (model.EXT2.Trim().Length == 18)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model"></param>
        public void UpdateMemberInfo(WeMemberModel model)
        {
            string sql = string.Format("update wtf_member set username='{0}',telephone='{1}',ext2='{2}',ext4='{4}',ext5='{5}',ext6='{6}' where sysno='{3}'",model.USERNAME,model.TELEPHONE,model.EXT2,model.SYSNO,model.EXT4,model.EXT5,model.EXT6);
            int a = DbHelperSQL.ExecuteSql(sql);
        }
        #endregion

#region 获得用户关注的人
        /// <summary>
        /// 获得关注的人,我关注的人
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public List<WeMemberModel> GetFollowerModellist(string _Memsys)
        {
            List<WeMemberModel> list = new List<WeMemberModel>();
            string sql = "select b.* from wtf_MemberFollow a left join wtf_member b on a.followsys=b.sysno where a.membersys='" + _Memsys + "' and b.sysno<>''";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["EXT1"].ToString() == "")
                    {
                        dr["EXT1"] = "/images/touxiang.jpg";
                    }
                }
                list = JsonHelper.ParseDtModelList<List<WeMemberModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 获得关注我的人
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public List<WeMemberModel> GetFollowModellist(string _Memsys)
        {
            List<WeMemberModel> list = new List<WeMemberModel>();
            string sql = "select b.* from wtf_MemberFollow a left join wtf_member b on a.followsys=b.sysno where a.followsys='" + _Memsys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["EXT1"].ToString() == "")
                    {
                        dr["EXT1"] = "/images/touxiang.jpg";
                    }
                }
                list = JsonHelper.ParseDtModelList<List<WeMemberModel>>(dt);
            }
            return list;
        }

#endregion

        /// <summary>
        /// 模糊查询用户信息
        /// </summary>
        /// <param name="_Cond"></param>
        /// <returns></returns>
        public List<WeMemberModel> GetMemberlistbyCond(string _Cond)
        {
            List<WeMemberModel> list = new List<WeMemberModel>();
            string sql = string.Format("select sysno,name,username from wtf_member where name+username+telephone like '%{0}%'",_Cond);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMemberModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 根据电话号码修改密码
        /// </summary>
        /// <param name="_Phone"></param>
        /// <param name="_password"></param>
        public void updatePasswordbyPhone(string _Phone, string _password)
        {
            string sql = "update wtf_member set password='"+_password+"' where telephone='"+_Phone+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// 修改图片url
        /// </summary>
        /// <param name="_userId"></param>
        /// <param name="_imagePath"></param>
        /// <returns></returns>
        public bool updateBGimage(string _userId, string _imagePath)
        {
            string sql = "update wtf_member set ext7='" + _imagePath + "' where sysno='" + _userId + "'";
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
