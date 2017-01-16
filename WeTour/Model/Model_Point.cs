using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class PointsModel
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

        private string _GAMESYS;
        /// <summary>
        /// 
        /// </summary>
        public string GAMESYS
        {
            get { return _GAMESYS; }
            set { _GAMESYS = value; }
        }

        private int _GORDER;
        /// <summary>
        /// 
        /// </summary>
        public int GORDER
        {
            get { return _GORDER; }
            set { _GORDER = value; }
        }

        private int _SERVETYPE;
        /// <summary>
        /// 
        /// </summary>
        public int SERVETYPE
        {
            get { return _SERVETYPE; }
            set { _SERVETYPE = value; }
        }

        private int _WINTYPE;
        /// <summary>
        /// 
        /// </summary>
        public int WINTYPE
        {
            get { return _WINTYPE; }
            set { _WINTYPE = value; }
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

        private int _SCORE;
        /// <summary>
        /// 
        /// </summary>
        public int SCORE
        {
            get { return _SCORE; }
            set { _SCORE = value; }
        }

        private int _ISBREAKPOINT;
        /// <summary>
        /// 
        /// </summary>
        public int ISBREAKPOINT
        {
            get { return _ISBREAKPOINT; }
            set { _ISBREAKPOINT = value; }
        }
    }
}
