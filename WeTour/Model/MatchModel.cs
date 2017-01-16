using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    [Serializable]
    public partial class MatchModel
    {
        public MatchModel() { }

        private string _SYS;
        /// <summary>
        /// 
        /// </summary>
        public string SYS
        {
            get { return _SYS; }
            set { _SYS = value; }
        }

        private string _PLAYER1;
        /// <summary>
        /// 
        /// </summary>
        public string PLAYER1
        {
            get { return _PLAYER1; }
            set { _PLAYER1 = value; }
        }

        private string _PLAYTYPE;
        /// <summary>
        /// 比赛类型(单打，双打)
        /// </summary>
        public string PLAYTYPE
        {
            get { return _PLAYTYPE; }
            set { _PLAYTYPE = value; }
        }

        private string _PLAYER2;
        /// <summary>
        /// 
        /// </summary>
        public string PLAYER2
        {
            get { return _PLAYER2; }
            set { _PLAYER2 = value; }
        }

        private string _MATCHDATE;
        /// <summary>
        /// 
        /// </summary>
        public string MATCHDATE
        {
            get { return _MATCHDATE; }
            set { _MATCHDATE = value; }
        }

        private string _PLACE;
        /// <summary>
        /// 
        /// </summary>
        public string PLACE
        {
            get { return _PLACE; }
            set { _PLACE = value; }
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

        private string _LOSER;
        /// <summary>
        /// 
        /// </summary>
        public string LOSER
        {
            get { return _LOSER; }
            set { _LOSER = value; }
        }

        private string _SCORE;
        /// <summary>
        /// 
        /// </summary>
        public string SCORE
        {
            get { return _SCORE; }
            set { _SCORE = value; }
        }

        private int? _MATCHTYPE;
        /// <summary>
        /// 盘数，0表示1盘制比赛，1表示3盘制比赛，2表示5盘制比赛
        /// </summary>
        public int? MATCHTYPE
        {
            get { return _MATCHTYPE; }
            set { _MATCHTYPE = value; }
        }

        private int? _GRADETYPE;
        /// <summary>
        /// 
        /// </summary>
        public int? GRADETYPE
        {
            get { return _GRADETYPE; }
            set { _GRADETYPE = value; }
        }

        private string _MATCHTIME;
        /// <summary>
        /// 
        /// </summary>
        public string MATCHTIME
        {
            get { return _MATCHTIME; }
            set { _MATCHTIME = value; }
        }

        private int? _STATE;
        /// <summary>
        /// 
        /// </summary>
        public int? STATE
        {
            get { return _STATE; }
            set { _STATE = value; }
        }

        private int? _ROUND;
        /// <summary>
        /// 
        /// </summary>
        public int? ROUND
        {
            get { return _ROUND; }
            set { _ROUND = value; }
        }

        private int? _ISDECIDE;
        /// <summary>
        /// 是否是金球制胜
        /// </summary>
        public int? ISDECIDE
        {
            get { return _ISDECIDE; }
            set { _ISDECIDE = value; }
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

        //-----附加字段

        private string _PLAYER1NAME;
        /// <summary>
        /// 
        /// </summary>
        public string PLAYER1NAME
        {
            get { return _PLAYER1NAME; }
            set { _PLAYER1NAME = value; }
        }

        private string _PLAYER2NAME;
        /// <summary>
        /// 
        /// </summary>
        public string PLAYER2NAME
        {
            get { return _PLAYER2NAME; }
            set { _PLAYER2NAME = value; }
        }

        private string _OPERATION;
        /// <summary>
        /// 
        /// </summary>
        public string OPERATION
        {
            get { return _OPERATION; }
            set { _OPERATION = value; }
        }

        private string _URL;
        /// <summary>
        /// 
        /// </summary>
        public string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }

        private string _PREDITTIME;
        /// <summary>
        /// 
        /// </summary>
        public string PREDITTIME
        {
            get { return _PREDITTIME; }
            set { _PREDITTIME = value; }
        }

        private string _ACTUALTIME;
        /// <summary>
        /// 
        /// </summary>
        public string ACTUALTIME
        {
            get { return _ACTUALTIME; }
            set { _ACTUALTIME = value; }
        }

        private string _COURTID;
        /// <summary>
        /// 
        /// </summary>
        public string COURTID
        {
            get { return _COURTID; }
            set { _COURTID = value; }
        }

        private string _NEEDUMPIRE;
        /// <summary>
        /// 是否需要裁判
        /// </summary>
        public string NEEDUMPIRE
        {
            get { return _NEEDUMPIRE; }
            set { _NEEDUMPIRE = value; }
        }

        private string _UMPIRE;
        /// <summary>
        /// 裁判
        /// </summary>
        public string UMPIRE
        {
            get { return _UMPIRE; }
            set { _UMPIRE = value; }
        }

        /// <summary>
        /// 赛事子项编码
        /// </summary>
        public string ContentID { get; set; }
        /// <summary>
        /// 比赛顺序
        /// </summary>
        public string matchorder { get; set; }
        /// <summary>
        /// 比赛胜者将成为第几场比赛的成员
        /// </summary>
        public string winto { get; set; }
        public string loseto { get; set; }
        public string iswithdraw { get; set; }
        /// <summary>
        /// 小组号
        /// </summary>
        public string etc1 { get; set; }
        /// <summary>
        /// 小组赛轮数
        /// </summary>
        public string etc2 { get; set; }
        public string etc3 { get; set; }
        public string etc4 { get; set; }
        public string etc5 { get; set; }


        //extra field
        public string RoundName { get; set; }
        public string CourtName { get; set; }
        public string ContentName { get; set; }

        public string GameQty { get; set; }
    }
}

