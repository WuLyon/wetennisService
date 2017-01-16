using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SMS
{
    public class TeleCheckDll
    {
        public static TeleCheckDll instance = new TeleCheckDll();

        /// <summary>
        /// 插入新的一条记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Insert(TeleCheckModel model)
        {
            string sql = string.Format("insert into sys_TelCheck (Telephone,ValidateCode,Status,EXT1,EXT2,EXT3) values ('{0}','{1}','{2}','{3}','{4}','{5}')", model.TELEPHONE, model.VALIDATECODE, "0", DateTime.Now.ToString(), model.EXT2, model.EXT3);
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
        /// 根据电话获得待验证的code
        /// </summary>
        /// <param name="_Telephone"></param>
        /// <returns></returns>
        public TeleCheckModel GetModelbyTelephone(string _Telephone)
        {
            TeleCheckModel model = new TeleCheckModel();
            string sql = "select * from sys_TelCheck where Telephone='" + _Telephone + "' and status='0'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = Member.JsonHelper.ParseDtInfo<TeleCheckModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据验证码查询实体
        /// </summary>
        /// <param name="_ValCode"></param>
        /// <returns></returns>
        public TeleCheckModel GetModelbyCode(string _ValCode)
        {
            TeleCheckModel model = new TeleCheckModel();
            string sql = "select * from sys_TelCheck where ValidateCode='"+_ValCode+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = Member.JsonHelper.ParseDtInfo<TeleCheckModel>(dt);
            }
            return model;
        }

        #region 创建code
        /// <summary>
        /// 创建一个四位的随机数
        /// </summary>
        /// <returns></returns>
        private string GenerateRandom(int qty)
        {
            string Rand = "";
            Random ran = new Random();
            for (int i = 0; i < qty; i++)
            {
                Rand += ran.Next(0, 9).ToString();//每位数都随机产生；
            }
            return Rand;
        }

        /// <summary>
        /// 
        /// </summary>
        private string CreateNewCode()
        {
            string newcode = "";
            string code = GenerateRandom(4);//创建一个4位数code
            //
            TeleCheckModel codemodel = GetModelbyCode(code);
            if (string.IsNullOrEmpty(codemodel.STATUS))
            {
                //code不重复
                newcode = code;
            }
            else
            {
                if (codemodel.STATUS == "0")
                {
                    //重复，需要重新获取code
                    newcode = GenerateRandom(4);
                }
                else
                {
                    //code不重复
                    newcode = code;
                }
            }
            return newcode;
        }

        #endregion

        /// <summary>
        /// 修改code的状态
        /// </summary>
        /// <param name="_Telephone"></param>
        /// <param name="_Code"></param>
        /// <param name="_Status"></param>
        /// <returns></returns>
        private bool UpdateCodeStatus(string _Telephone,string _Code, string _Status)
        {
            string sql = string.Format("update sys_TelCheck set status='{0}' where ValidateCode='{1}' and Telephone='{2}'",_Status,_Code,_Telephone);
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
        /// 根据电话号码来获取code
        /// </summary>
        /// <param name="_Telephone"></param>
        /// <returns></returns>
        public string GreateCode(string _Telephone)
        {
            string _Code = "";
            //根据电话号码获取状态是0的code
            TeleCheckModel model = GetModelbyTelephone(_Telephone);
            if (string.IsNullOrEmpty(model.VALIDATECODE))
            {
                //该电话未产生code,获取新的code
                _Code = CreateNewCode();
            }
            else
            { 
                //已有code，验证时间是否超时
                if (!Checkdate(model.EXT1))
                {
                    //产生时间超过3分钟，让code失效
                    UpdateCodeStatus(_Telephone, model.VALIDATECODE, "9");
                    _Code=CreateNewCode();
                }
                else
                {
                    //产生时间在3分钟以内，拒绝产生code
                    _Code = "FALSE";
                }
            }

            if (_Code != "FALSE")
            {
                TeleCheckModel newmodel = new TeleCheckModel();
                newmodel.TELEPHONE = _Telephone;
                newmodel.VALIDATECODE = _Code;
                if (Insert(newmodel))
                {
                    string Message = _Code + "(微网球验证码，3分钟有效）";
                    SMSdll.instance.BatchSendSMS(_Telephone, Message);
                }
            }
            return _Code;
        }

        /// <summary>
        /// 验证时间有效性
        /// </summary>
        /// <param name="_DateTime"></param>
        /// <returns></returns>
        private bool Checkdate(string _DateTime)
        {
            DateTime createTime = Convert.ToDateTime(_DateTime);
            TimeSpan ts = DateTime.Now - createTime;
            double Sec = ts.TotalSeconds;
            if (Sec > 180)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 认证验证码的正确性
        /// </summary>
        /// <param name="_Telephone"></param>
        /// <param name="_Code"></param>
        /// <returns></returns>
        public bool ValidateCode(string _Telephone, string _Code)
        { 
            //根据_Telephone获取验证码
            TeleCheckModel model = GetModelbyTelephone(_Telephone);
            if (!string.IsNullOrEmpty(model.VALIDATECODE))
            {
                //验证有效时间
                if (Checkdate(model.EXT1))
                {                    
                    //时间有效,验证code
                    if (_Code.Trim() == model.VALIDATECODE.Trim())
                    {
                        UpdateCodeStatus(_Telephone, model.VALIDATECODE, "2");
                        return true;
                    }
                    else
                    {
                        UpdateCodeStatus(_Telephone, model.VALIDATECODE, "1");
                        return false;
                    }
                }
                else
                { 
                    //已过期，
                    UpdateCodeStatus(_Telephone, model.VALIDATECODE, "9");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
