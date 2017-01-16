using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeTour
{
    [Serializable]
    public partial class MemberModel
    {
        public MemberModel() { }

        private string _ID;
        /// <summary>
        /// 
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _SYSNO;
        /// <summary>
        /// 
        /// </summary>
        public string SYSNO
        {
            get { return _SYSNO; }
            set { _SYSNO = value; }
        }

        private string _NAME;
        /// <summary>
        /// 
        /// </summary>
        public string NAME
        {
            get { return _NAME; }
            set { _NAME = value; }
        }

        private string _USERNAME;
        /// <summary>
        /// 
        /// </summary>
        public string USERNAME
        {
            get { return _USERNAME; }
            set { _USERNAME = value; }
        }

        private string _PASSWORD;
        /// <summary>
        /// 
        /// </summary>
        public string PASSWORD
        {
            get { return _PASSWORD; }
            set { _PASSWORD = value; }
        }

        private string _PROVINCEID;
        /// <summary>
        /// 
        /// </summary>
        public string PROVINCEID
        {
            get { return _PROVINCEID; }
            set { _PROVINCEID = value; }
        }

        private string _PROVINCE;
        /// <summary>
        /// 
        /// </summary>
        public string PROVINCE
        {
            get { return _PROVINCE; }
            set { _PROVINCE = value; }
        }

        private string _CITYID;
        /// <summary>
        /// 
        /// </summary>
        public string CITYID
        {
            get { return _CITYID; }
            set { _CITYID = value; }
        }

        private string _CITY;
        /// <summary>
        /// 
        /// </summary>
        public string CITY
        {
            get { return _CITY; }
            set { _CITY = value; }
        }

        private string _REGIONID;
        /// <summary>
        /// 
        /// </summary>
        public string REGIONID
        {
            get { return _REGIONID; }
            set { _REGIONID = value; }
        }

        private string _REGION;
        /// <summary>
        /// 
        /// </summary>
        public string REGION
        {
            get { return _REGION; }
            set { _REGION = value; }
        }

        private string _ADDRESS;
        /// <summary>
        /// 
        /// </summary>
        public string ADDRESS
        {
            get { return _ADDRESS; }
            set { _ADDRESS = value; }
        }

        private string _BIRTHDAY;
        /// <summary>
        /// 
        /// </summary>
        public string BIRTHDAY
        {
            get { return _BIRTHDAY; }
            set { _BIRTHDAY = value; }
        }

        private string _REGISTERPROID;
        /// <summary>
        /// 
        /// </summary>
        public string REGISTERPROID
        {
            get { return _REGISTERPROID; }
            set { _REGISTERPROID = value; }
        }

        private string _REGISTERPRO;
        /// <summary>
        /// 
        /// </summary>
        public string REGISTERPRO
        {
            get { return _REGISTERPRO; }
            set { _REGISTERPRO = value; }
        }

        private string _REGISTERCITYID;
        /// <summary>
        /// 
        /// </summary>
        public string REGISTERCITYID
        {
            get { return _REGISTERCITYID; }
            set { _REGISTERCITYID = value; }
        }

        private string _REGISTERCITY;
        /// <summary>
        /// 
        /// </summary>
        public string REGISTERCITY
        {
            get { return _REGISTERCITY; }
            set { _REGISTERCITY = value; }
        }

        private string _REGISTERREGID;
        /// <summary>
        /// 
        /// </summary>
        public string REGISTERREGID
        {
            get { return _REGISTERREGID; }
            set { _REGISTERREGID = value; }
        }

        private string _REGISTERREG;
        /// <summary>
        /// 
        /// </summary>
        public string REGISTERREG
        {
            get { return _REGISTERREG; }
            set { _REGISTERREG = value; }
        }

        private string _REGISTERDATE;
        /// <summary>
        /// 
        /// </summary>
        public string REGISTERDATE
        {
            get { return _REGISTERDATE; }
            set { _REGISTERDATE = value; }
        }

        private string _MEMBERTYPE;
        /// <summary>
        /// 
        /// </summary>
        public string MEMBERTYPE
        {
            get { return _MEMBERTYPE; }
            set { _MEMBERTYPE = value; }
        }

        private string _GENDER;
        /// <summary>
        /// 
        /// </summary>
        public string GENDER
        {
            get { return _GENDER; }
            set { _GENDER = value; }
        }

        private string _TELEPHONE;
        /// <summary>
        /// 
        /// </summary>
        public string TELEPHONE
        {
            get { return _TELEPHONE; }
            set { _TELEPHONE = value; }
        }

        private string _EMAIL;
        /// <summary>
        /// 
        /// </summary>
        public string EMAIL
        {
            get { return _EMAIL; }
            set { _EMAIL = value; }
        }

        private string _TENNISSTARTYEAR;
        /// <summary>
        /// 
        /// </summary>
        public string TENNISSTARTYEAR
        {
            get { return _TENNISSTARTYEAR; }
            set { _TENNISSTARTYEAR = value; }
        }

        private string _DESCRIPTION;
        /// <summary>
        /// 
        /// </summary>
        public string DESCRIPTION
        {
            get { return _DESCRIPTION; }
            set { _DESCRIPTION = value; }
        }

        private string _FOREHAND;
        /// <summary>
        /// 
        /// </summary>
        public string FOREHAND
        {
            get { return _FOREHAND; }
            set { _FOREHAND = value; }
        }

        private string _BACKHAND;
        /// <summary>
        /// 
        /// </summary>
        public string BACKHAND
        {
            get { return _BACKHAND; }
            set { _BACKHAND = value; }
        }

        private string _HEIGHT;
        /// <summary>
        /// 
        /// </summary>
        public string HEIGHT
        {
            get { return _HEIGHT; }
            set { _HEIGHT = value; }
        }

        private string _WEIGHT;
        /// <summary>
        /// 
        /// </summary>
        public string WEIGHT
        {
            get { return _WEIGHT; }
            set { _WEIGHT = value; }
        }

        private string _SYS_FSTLOG;
        /// <summary>
        /// 
        /// </summary>
        public string SYS_FSTLOG
        {
            get { return _SYS_FSTLOG; }
            set { _SYS_FSTLOG = value; }
        }

        private string _JOB;
        /// <summary>
        /// 
        /// </summary>
        public string JOB
        {
            get { return _JOB; }
            set { _JOB = value; }
        }

        private string _MGRTYPE;
        /// <summary>
        /// 
        /// </summary>
        public string MGRTYPE
        {
            get { return _MGRTYPE; }
            set { _MGRTYPE = value; }
        }

        private string _STATUS;
        /// <summary>
        /// 
        /// </summary>
        public string STATUS
        {
            get { return _STATUS; }
            set { _STATUS = value; }
        }

        private string _EXT1;
        /// <summary>
        /// 
        /// </summary>
        public string EXT1
        {
            get { return _EXT1; }
            set { _EXT1 = value; }
        }

        private string _EXT2;
        /// <summary>
        /// 
        /// </summary>
        public string EXT2
        {
            get { return _EXT2; }
            set { _EXT2 = value; }
        }

        private string _EXT3;
        /// <summary>
        /// 
        /// </summary>
        public string EXT3
        {
            get { return _EXT3; }
            set { _EXT3 = value; }
        }

        private string _EXT4;
        /// <summary>
        /// 
        /// </summary>
        public string EXT4
        {
            get { return _EXT4; }
            set { _EXT4 = value; }
        }

        private string _EXT5;
        /// <summary>
        /// 
        /// </summary>
        public string EXT5
        {
            get { return _EXT5; }
            set { _EXT5 = value; }
        }

        private string _EXT6;
        /// <summary>
        /// 
        /// </summary>
        public string EXT6
        {
            get { return _EXT6; }
            set { _EXT6 = value; }
        }

        private string _EXT7;
        /// <summary>
        /// 
        /// </summary>
        public string EXT7
        {
            get { return _EXT7; }
            set { _EXT7 = value; }
        }

        private string _EXT8;
        /// <summary>
        /// 
        /// </summary>
        public string EXT8
        {
            get { return _EXT8; }
            set { _EXT8 = value; }
        }

        private string _EXT9;
        /// <summary>
        /// 
        /// </summary>
        public string EXT9
        {
            get { return _EXT9; }
            set { _EXT9 = value; }
        }

        private string _EXT10;
        /// <summary>
        /// 
        /// </summary>
        public string EXT10
        {
            get { return _EXT10; }
            set { _EXT10 = value; }
        }

        /// <summary>
        /// 单打排名
        /// </summary>
        public string SinglRk { get; set; }

        /// <summary>
        /// 双打排名
        /// </summary>
        public string DoubRk { get; set; }

        /// <summary>
        /// 社区积分
        /// </summary>
        public string SocialPts { get; set; }

        /// <summary>
        /// 消息数量
        /// </summary>
        public string MsgQty { get; set; }

    }

    public class TrendModel
    {
        public string TRID { get; set; }//动态id
        public string WTRIMG { get; set; }//头像
        public string WTRSYSNO { get; set; }//主键
        public string WTRNAME { get; set; }//姓名
        public string WTRDATE { get; set; }//日期
        public string CONTENT { get; set; }//内容
        public string APPLOUSE { get; set; }//点赞数
        public string COMMENTS { get; set; }//动态评论
    }
}
