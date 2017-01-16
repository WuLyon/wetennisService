using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class SetModel
    {
        private string _SYS;
        /// <summary>
        /// 
        /// </summary>
        public string SYS
        {
            get { return _SYS; }
            set { _SYS = value; }
        }

        private string _MATCHSYS;
        /// <summary>
        /// 
        /// </summary>
        public string MATCHSYS
        {
            get { return _MATCHSYS; }
            set { _MATCHSYS = value; }
        }

        private int _SORDER;
        /// <summary>
        /// 
        /// </summary>
        public int SORDER
        {
            get { return _SORDER; }
            set { _SORDER = value; }
        }

        private string _WINNER;
        /// <summary>
        /// 
        /// </summary>
        public string WINNER
        {
            get { return _WINNER; }
            set { _WINNER = value; }
        }

        private int _STATE;
        /// <summary>
        /// 
        /// </summary>
        public int STATE
        {
            get { return _STATE; }
            set { _STATE = value; }
        }

    }
}
