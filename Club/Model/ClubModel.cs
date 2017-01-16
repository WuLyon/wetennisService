using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Club
{
    [Serializable]
    public partial class ClubModel
    {
        public ClubModel() { }

        public string ID { get; set; }

        private string _SYSNO;
        /// <summary>
        /// 
        /// </summary>
        public string SYSNO
        {
            get { return _SYSNO; }
            set { _SYSNO = value; }
        }

        private string _CLUBNAME;
        /// <summary>
        /// 
        /// </summary>
        public string CLUBNAME
        {
            get { return _CLUBNAME; }
            set { _CLUBNAME = value; }
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

        private string _CITY;
        /// <summary>
        /// 
        /// </summary>
        public string CITY
        {
            get { return _CITY; }
            set { _CITY = value; }
        }

        public string REGION { get; set; }
        public string PROID { get; set; }
        public string CITYID { get; set; }
        public string REGID { get; set; }

        private string _CONTACTPERSON;
        /// <summary>
        /// 
        /// </summary>
        public string CONTACTPERSON
        {
            get { return _CONTACTPERSON; }
            set { _CONTACTPERSON = value; }
        }

        private string _CONTACTTEL;
        /// <summary>
        /// 
        /// </summary>
        public string CONTACTTEL
        {
            get { return _CONTACTTEL; }
            set { _CONTACTTEL = value; }
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

        public string STATUS { get; set; }

        public string CLEVEL { get; set; }

        public string UPDATEDATE { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string EXT1 { get; set; }

        /// <summary>
        /// 俱乐部logo
        /// </summary>
        public string EXT2 { get; set; }
        public string EXT3 { get; set; }

        public string MemberQty { get; set; }

    }
}
