using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class GameModel
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

        private string _SETSYS;
        /// <summary>
        /// 
        /// </summary>
        public string SETSYS
        {
            get { return _SETSYS; }
            set { _SETSYS = value; }
        }

        private int _MORDER;
        /// <summary>
        /// 
        /// </summary>
        public int MORDER
        {
            get { return _MORDER; }
            set { _MORDER = value; }
        }

        private string _MSERVER;
        /// <summary>
        /// 
        /// </summary>
        public string MSERVER
        {
            get { return _MSERVER; }
            set { _MSERVER = value; }
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

        private int? _ISTIEBREAK;
        /// <summary>
        /// 
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
        public string LeftsidePlayer { get; set; }
        public string ext1 { get; set; }
        public string ext2 { get; set; }
        public string ext3 { get; set; }
        public string ext4 { get; set; }
        public string ext5 { get; set; }
    }
}
