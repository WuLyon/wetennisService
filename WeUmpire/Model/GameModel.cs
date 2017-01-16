using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeUmpire
{
    /// <summary>
    /// 局实体
    /// </summary>
    [Serializable]
    public partial class GameModel
    {
        public GameModel() { }

        private string _SYS;
        /// <summary>
        /// 局主键
        /// </summary>
        public string SYS
        {
            get { return _SYS; }
            set { _SYS = value; }
        }

        private string _SETSYS;
        /// <summary>
        /// 盘主键
        /// </summary>
        public string SETSYS
        {
            get { return _SETSYS; }
            set { _SETSYS = value; }
        }

        private int _MORDER;
        /// <summary>
        /// 局顺序
        /// </summary>
        public int MORDER
        {
            get { return _MORDER; }
            set { _MORDER = value; }
        }

        private string _MSERVER;
        /// <summary>
        /// 本局发球方
        /// </summary>
        public string MSERVER
        {
            get { return _MSERVER; }
            set { _MSERVER = value; }
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

        private int _STATE;
        /// <summary>
        /// 状态
        /// </summary>
        public int STATE
        {
            get { return _STATE; }
            set { _STATE = value; }
        }

        private int? _ISTIEBREAK;
        /// <summary>
        /// 是否是抢七局
        /// </summary>
        public int? ISTIEBREAK
        {
            get { return _ISTIEBREAK; }
            set { _ISTIEBREAK = value; }
        }

        private int? _IsDecidePoint;
        /// <summary>
        /// 
        /// </summary>
        public int? IsDecidePoint
        {
            get { return _IsDecidePoint; }
            set { _IsDecidePoint = value; }
        }

        //2015-5-14
        //新增属性
        /// <summary>
        /// 本局在裁判左手边球员
        /// </summary>
        public string LeftsidePlayer { get; set; }
        public string ext1 { get; set; }
        public string ext2 { get; set; }
        public string ext3 { get; set; }
        public string ext4 { get; set; }
        public string ext5 { get; set; }

    }
}

