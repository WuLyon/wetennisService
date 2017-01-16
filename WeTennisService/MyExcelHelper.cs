using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace WeTennisService
{
    public class MyExcelHelper
    {
        /// <summary> 
        /// 连接Excel 读取Excel数据 并返回DataSet数据集合 
        /// </summary> 
        /// <param name="filepath">Excel服务器路径</param> 
        /// <param name="tableName">Excel表名称</param> 
        /// <returns></returns> 
        public static System.Data.DataSet ExcelSqlConnection(string filepath, string tableName)
        {
            //string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1'"; 
            string strCon = "Provider=Microsoft.Ace.OleDb.12.0;Data Source=" + filepath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1'";
            OleDbConnection ExcelConn = new OleDbConnection(strCon);
            try
            {
                string strCom = string.Format("SELECT * FROM [Sheet1$]");
                ExcelConn.Open();
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, ExcelConn);
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "[" + tableName + "$]");
                ExcelConn.Close();
                return ds;
            }
            catch
            {
                ExcelConn.Close();
                return null;
            }
        } 
    }
}