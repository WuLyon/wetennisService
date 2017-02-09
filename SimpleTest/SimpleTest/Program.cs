using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            double value = Convert.ToDouble(15) /4;
            Console.WriteLine("-> " + value);
            int result = -99999;
            int.TryParse(Math.Ceiling(value).ToString(), out result);
            Console.WriteLine("EEEEEEE ->" + result);
            Console.Read();
        
        }

        static long ComputeDateDiff(string _Start, string _End)
        {
            DateTime Start = Convert.ToDateTime(_Start);
            DateTime End = Convert.ToDateTime(_End);

            TimeSpan ts1 = new TimeSpan(Start.Ticks);
            TimeSpan ts2 = new TimeSpan(End.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();//时间差距的绝对值
            //jiangdsfef
            return ts.Days * 24 * 60 * 60 + ts.Hours * 60 * 60 + ts.Minutes * 60 + ts.Seconds;
        }
    }
}
