using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChampGuess
{
    public class ChamCadModel
    {
        public string id { get; set; }
        public string GuessId { get; set; }
        public string Playersys { get; set; }
        public string ext1 { get; set; }
        public string ext2 { get; set; }
        public string ext3 { get; set; }

        public int FollowQty { get; set; }
    }
}
