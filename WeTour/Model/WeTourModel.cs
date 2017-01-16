using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class WeTourModel
    {
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

        private string _STATUS;
        /// <summary>
        /// 
        /// </summary>
        public string STATUS
        {
            get { return _STATUS; }
            set { _STATUS = value; }
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

        private string _TOURSYS;
        /// <summary>
        /// 
        /// </summary>
        public string TOURSYS
        {
            get { return _TOURSYS; }
            set { _TOURSYS = value; }
        }

        private string _MGRSYS;
        /// <summary>
        /// Tour holder 
        /// </summary>
        public string MGRSYS
        {
            get { return _MGRSYS; }
            set { _MGRSYS = value; }
        }

        private string _STARTDATE;
        /// <summary>
        /// 赛事开始日期
        /// </summary>
        public string STARTDATE
        {
            get { return _STARTDATE; }
            set { _STARTDATE = value; }
        }

        private string _ENDDATE;
        /// <summary>
        /// 赛事报名截止日期
        /// </summary>
        public string ENDDATE
        {
            get { return _ENDDATE; }
            set { _ENDDATE = value; }
        }

        private string _CAPACITY;
        /// <summary>
        /// 
        /// </summary>
        public string CAPACITY
        {
            get { return _CAPACITY; }
            set { _CAPACITY = value; }
        }

        private string _SETTYPE;
        /// <summary>
        /// 盘类型
        /// </summary>
        public string SETTYPE
        {
            get { return _SETTYPE; }
            set { _SETTYPE = value; }
        }

        private string _GAMETYPE;
        /// <summary>
        /// 
        /// </summary>
        public string GAMETYPE
        {
            get { return _GAMETYPE; }
            set { _GAMETYPE = value; }
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

        private string _MATCHCONTENT;
        /// <summary>
        /// 
        /// </summary>
        public string MATCHCONTENT
        {
            get { return _MATCHCONTENT; }
            set { _MATCHCONTENT = value; }
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

        private string _CITYTYPE;
        /// <summary>
        /// Denote a wetennis tour or a club tour
        /// </summary>
        public string CITYTYPE
        {
            get { return _CITYTYPE; }
            set { _CITYTYPE = value; }
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

        private string _TOURIMG;
        /// <summary>
        /// 
        /// </summary>
        public string TOURIMG
        {
            get { return _TOURIMG; }
            set { _TOURIMG = value; }
        }

        private string _EXT1;
        /// <summary>
        /// 统一报名费，如项目未特殊指定报名费，就按此报名费执行
        /// </summary>
        public string EXT1
        {
            get { return _EXT1; }
            set { _EXT1 = value; }
        }

        private string _EXT2;
        /// <summary>
        /// 统一盘数
        /// </summary>
        public string EXT2
        {
            get { return _EXT2; }
            set { _EXT2 = value; }
        }

        private string _EXT3;
        /// <summary>
        /// 时间
        /// </summary>
        public string EXT3
        {
            get { return _EXT3; }
            set { _EXT3 = value; }
        }

        private string _EXT4;
        /// <summary>
        /// 赛事管理方主动发送短信通知，
        /// 判断：不为空，则表示已经发送短信通知
        /// 后期短信通知可考虑收费
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

        private string _STARTHOUR;
        /// <summary>
        /// 
        /// </summary>
        public string STARTHOUR
        {
            get { return _STARTHOUR; }
            set { _STARTHOUR = value; }
        }

        private string _COURTSYS;
        /// <summary>
        /// 
        /// </summary>
        public string COURTSYS
        {
            get { return _COURTSYS; }
            set { _COURTSYS = value; }
        }

        private string _TOURCOURT;
        /// <summary>
        /// 
        /// </summary>
        public string TOURCOURT
        {
            get { return _TOURCOURT; }
            set { _TOURCOURT = value; }
        }

        //状态值
        public string StatusName { get; set; }
        //级别图片
        public string LevelImg { get; set; }

        /// <summary>
        /// 未裁数量
        /// </summary>
        public string UnUmpQty { get; set; }
        /// <summary>
        /// 已裁数量
        /// </summary>
        public string UmpedQty { get; set; }
        //联盟主键，新增字段，有值，则表示是联盟赛事
        public string UnionSys { get; set; }
        //
        public string Host { get; set; }
        public string Asso_Host { get; set; }
        /// <summary>
        /// 赛事主页显示的赛事背景图
        /// </summary>
        public string TourBackImg { get; set; }
        /// <summary>
        /// 报名截止时间
        /// </summary>
        public string ext6 { get; set; }
        /// <summary>
        /// 抽签开始时间
        /// </summary>
        public string ext7 { get; set; }
        public string ext8 { get; set; }
        public string ext9 { get; set; }
        public string ext10 { get; set; }
    }
}
