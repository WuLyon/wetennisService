using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace WeTennisService
{
    public class ReadTxt
    {
         public string ReadTxtF(string filePath, string Files)
        {
            string path, configstr;
            try
            {
                path = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + filePath + Files;
                path = @"D:\\Resource\jsapi_ticket.json.txt";
                StreamReader sr = new StreamReader(path);
                configstr = sr.ReadToEnd();
                sr.Close();
                return configstr;
            }
            catch
            {
                return "Read error!";
            }
        }

         void WriteLog(string _Desc, string value1)
         {
             string sql = "insert into sys_setting (Enum,Descript,value,value2) values ('1101','" + _Desc + "','" + DateTime.Now.ToString() + "','" + value1 + "')";
             int a = DbHelperSQL.ExecuteSql(sql);
         }


        public void WriteJsonText(string filePath, string Files, string JSON_text)
        {
            try
            {
                string path = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + filePath + Files;
                path = @"D:\\Resource\jsapi_ticket.json.txt";
                FileStream aFile = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(aFile);

                sw.Write(JSON_text);
                sw.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
    
}
