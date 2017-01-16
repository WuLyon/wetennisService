using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    [Serializable]
    public partial class PlayerModel
    {
        public PlayerModel() { }

        private string _SYS;
        /// <summary>
        /// 
        /// </summary>
        public string SYS
        {
            get { return _SYS; }
            set { _SYS = value; }
        }

        private string _PNAME;
        /// <summary>
        /// 
        /// </summary>
        public string PNAME
        {
            get { return _PNAME; }
            set { _PNAME = value; }
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

        private string _TURNEDTENNIS;
        /// <summary>
        /// 
        /// </summary>
        public string TURNEDTENNIS
        {
            get { return _TURNEDTENNIS; }
            set { _TURNEDTENNIS = value; }
        }

    }
}

