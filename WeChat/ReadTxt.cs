using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace WeChat
{
    /// <summary>
    /// 读取文本文件的文件名
    /// </summary>
    public class ReadTxt
    {
        /// <summary>
        ///读取text文档中写入内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Files"></param>
        /// <returns></returns>
        public string ReadTxtF(string filePath, string Files)
        {
            string path, configstr;
            try
            {
                path = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + filePath + Files;
                StreamReader sr = new StreamReader(path);
                configstr = sr.ReadToEnd();
                sr.Close();
                return configstr;
            }
            catch(Exception e)
            {
                return "Read error!";
            }
        }

        /// <summary>
        /// 向text 文档中写入内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Files"></param>
        /// <param name="JSON_text"></param>
        public void WriteJsonText(string filePath, string Files, string JSON_text)
        {
            try
            {
                string path = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + filePath + Files;
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