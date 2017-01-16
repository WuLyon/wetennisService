using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeUmpire
{
    /// <summary>
    /// 盘-实体
    /// </summary>
    [Serializable]
    public partial class SetModel
    {
        public SetModel() { }

        private string _SYS;
        /// <summary>
        /// 主键
        /// </summary>
        public string SYS
        {
            get { return _SYS; }
            set { _SYS = value; }
        }

        private string _MATCHSYS;
        /// <summary>
        /// 比赛主键
        /// </summary>
        public string MATCHSYS
        {
            get { return _MATCHSYS; }
            set { _MATCHSYS = value; }
        }

        private int _SORDER;
        /// <summary>
        /// 盘顺序
        /// </summary>
        public int SORDER
        {
            get { return _SORDER; }
            set { _SORDER = value; }
        }

        private string _WINNER;
        /// <summary>
        /// 盘获胜方
        /// </summary>
        public string WINNER
        {
            get { return _WINNER; }
            set { _WINNER = value; }
        }

        private int _STATE;
        /// <summary>
        /// 盘状态
        /// </summary>
        public int STATE
        {
            get { return _STATE; }
            set { _STATE = value; }
        }

    }
}

