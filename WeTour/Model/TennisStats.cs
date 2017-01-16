using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    [Serializable]
    public partial class TennisStats
    {
        public TennisStats() { }

        private string _INDEX;
        /// <summary>
        /// 
        /// </summary>
        public string INDEX
        {
            get { return _INDEX; }
            set { _INDEX = value; }
        }

        private string _P1;
        /// <summary>
        /// 
        /// </summary>
        public string P1
        {
            get { return _P1; }
            set { _P1 = value; }
        }

        private string _P2;
        /// <summary>
        /// 
        /// </summary>
        public string P2
        {
            get { return _P2; }
            set { _P2 = value; }
        }

    }
}

