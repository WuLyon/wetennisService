using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WeTennisService
{
    public class LogHelper
    {
        public static LogHelper instance=new LogHelper();
        
        /// <summary>
        /// 根据LogType 将日志写入到对应的text文档中
        /// </summary>
        /// <param name="_UrlType"></param>
        /// <param name="_Content"></param>
        public void WriteLog(string _UrlType,string _Content)
        {
            string _Url = GetLogPath(_UrlType);
            FileStream fs = new FileStream(_Url, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(_Content+ "--\ttextlog:" + DateTime.Now.ToString() + "\n");            

            sw.Flush();
            sw.Close();
            fs.Close();
        }

        private string GetLogPath(string _Type)
        {
            string _LogUrl = "";
            switch (_Type)
            { 
                case "test":
                    _LogUrl = @"d:\MES.txt";
                    break;
                case "Service":
                    _LogUrl = @"d:\Log\Service_"+DateTime.Now.ToString("yyyyMMdd")+".txt";
                    break;
                case "req":
                    _LogUrl = @"d:\Log\Request_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    break; 
            }

            return _LogUrl;
        }
    }
}

