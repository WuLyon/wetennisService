using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class Biz_Time
    {
        public static Biz_Time instance = new Biz_Time();

        /// <summary>
        /// 获取两个时间点之间的相隔秒数
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        public int ComputeDateDiff(string _Start, string _End)
        {
            DateTime Start = Convert.ToDateTime(_Start);
            DateTime End = Convert.ToDateTime(_End);

            TimeSpan ts1 = new TimeSpan(Start.Ticks);
            TimeSpan ts2 = new TimeSpan(End.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();//时间差距的绝对值
            //jiangdsfef
            return ts.Days*24*60*60+ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds;
        }
    }
}
