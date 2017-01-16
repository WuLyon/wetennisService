using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeUmpire
{
    /// <summary>
    /// 分-实体
    /// </summary>
    [Serializable]
    public partial class PointsModel
    {
        public PointsModel() { }

        private string _SYS;
        /// <summary>
        /// 主键 
        /// </summary>
        public string SYS
        {
            get { return _SYS; }
            set { _SYS = value; }
        }

        private string _GAMESYS;
        /// <summary>
        /// 局主键
        /// </summary>
        public string GAMESYS
        {
            get { return _GAMESYS; }
            set { _GAMESYS = value; }
        }

        private int _GORDER;
        /// <summary>
        /// 分数序号
        /// </summary>
        public int GORDER
        {
            get { return _GORDER; }
            set { _GORDER = value; }
        }

        private int _SERVETYPE;
        /// <summary>
        /// 发球方式：0一发，1，二发 ，2：双误
        /// </summary>
        public int SERVETYPE
        {
            get { return _SERVETYPE; }
            set { _SERVETYPE = value; }
        }

        private int _WINTYPE;
        /// <summary>
        /// 获胜方式：0：ACE,1.制胜分，2，失误
        /// </summary>
        public int WINTYPE
        {
            get { return _WINTYPE; }
            set { _WINTYPE = value; }
        }

        private string _WINNER;
        /// <summary>
        /// 获胜方
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
        /// 是否破发点
        /// </summary>
        public int ISBREAKPOINT
        {
            get { return _ISBREAKPOINT; }
            set { _ISBREAKPOINT = value; }
        }

    }
}

