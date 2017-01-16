using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.Script.Serialization;//添加引用
using System.Data.Common;
using System.Web;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Web;///记得引用这个命名空间
using System.IO;
using System.Reflection;
using System.ComponentModel;

namespace Ranking
{
    public class JsonHelper
    {
        /// <summary>
        /// 类对像转换成json格式
        /// </summary> 
        /// <returns></returns>
        public static string ToJson(object t)
        {
            return new JavaScriptSerializer().Serialize(t);
        }

        public static string ToJsonDs(DataSet ds)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {
                json.Append("{\"");
                json.Append(dt.TableName);
                json.Append("\":");
                json.Append(ToJsonDt(dt));
                json.Append("}");
            }
            return json.ToString();
        }

        public static string ToJsonDt(DataTable dt)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.MaxJsonLength = 1024000;
            ArrayList dic = new ArrayList();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> drow = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    drow.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                }
                dic.Add(drow);
            }
            return jss.Serialize(dic);
        }

        public static string ToJsonDtInfo(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                jsonBuilder.Append("\"");
                jsonBuilder.Append(dt.Columns[j].ColumnName);
                jsonBuilder.Append("\":\"");
                try
                {
                    jsonBuilder.Append(dt.Rows[0][j].ToString());
                }
                catch (Exception)
                {
                    jsonBuilder.Append("");
                }
                jsonBuilder.Append("\",");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("},");
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            return jsonBuilder.ToString();
        }


        /// <summary>
        /// 把JSON字符串还原为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="szJson">JSON字符串</param>
        /// <returns>对象实体</returns>
        public static T ParseFormJson<T>(string szJson)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                DataContractJsonSerializer dcj = new DataContractJsonSerializer(typeof(T));
                return (T)dcj.ReadObject(ms);
            }
        }

        public static T ParseInfo<T>(string _json)
        {
            T obj = Activator.CreateInstance<T>();
            return new JavaScriptSerializer().Deserialize<T>(_json);
        }

        /// <summary>
        /// 将只有一条数据的DT转化为MODEL实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T ParseDtInfo<T>(DataTable dt)
        {
            string _json = ToJsonDtInfo(dt);
            T obj = Activator.CreateInstance<T>();
            return new JavaScriptSerializer().Deserialize<T>(_json);
        }

        public static T ParseDtModelList<T>(DataTable dt)
        {
            string _json = ToJsonDt(dt);
            T obj = Activator.CreateInstance<T>();
            return new JavaScriptSerializer().Deserialize<T>(_json);
        }

        /// <summary>
        /// json格式转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T FromJson<T>(string strJson) where T : class
        {
            return new JavaScriptSerializer().Deserialize<T>(strJson);
        }



        ///// <summary>
        ///// 获取树格式对象的JSON
        ///// </summary>
        ///// <param name="commandText">commandText</param>
        ///// <param name="id">ID的字段名</param>
        ///// <param name="pid">PID的字段名</param>
        ///// <returns></returns>
        //public static string GetArrayJSON(string commandText, string id, string pid)
        //{
        //    var o = ArrayToTreeData(commandText, id, pid);
        //    return ToJson(o);
        //}
        /// <summary>
        /// 获取树格式对象的JSON
        /// </summary>
        /// <param name="command">command</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <returns></returns>
        //public static string GetArrayJSON(DbCommand command, string id, string pid)
        //{
        //    var o = ArrayToTreeData(command, id, pid);
        //    return ToJson(o);
        //}

        /// <summary>
        /// 获取树格式对象的JSON
        /// </summary>
        /// <param name="list">线性数据</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <returns></returns>
        public static string GetArrayJSON(IList<Hashtable> list, string id, string pid)
        {
            var o = ArrayToTreeData(list, id, pid);
            return ToJson(o);
        }

        ///// <summary>
        ///// 获取树格式对象
        ///// </summary>
        ///// <param name="command">command</param>
        ///// <param name="id">id的字段名</param>
        ///// <param name="pid">pid的字段名</param>
        ///// <returns></returns>
        //public static object ArrayToTreeData(DbCommand command, string id, string pid)
        //{
        //    var reader = DbHelper.Db.ExecuteReader(command);
        //    var list = DbReaderToHash(reader);
        //    return JSONHelper.ArrayToTreeData(list, id, pid);
        //}

        ///// <summary>
        ///// 获取树格式对象
        ///// </summary>
        ///// <param name="commandText">sql</param>
        ///// <param name="id">ID的字段名</param>
        ///// <param name="pid">PID的字段名</param> 
        ///// <returns></returns>
        //public static object ArrayToTreeData(string commandText, string id, string pid)
        //{
        //    var reader = DbHelper.Db.ExecuteReader(commandText);
        //    var list = DbReaderToHash(reader);
        //    return JSONHelper.ArrayToTreeData(list, id, pid);
        //}

        /// <summary>
        /// 获取树格式对象
        /// </summary>
        /// <param name="list">线性数据</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <returns></returns>
        public static object ArrayToTreeData(IList<Hashtable> list, string id, string pid)
        {
            var h = new Hashtable(); //数据索引 
            var r = new List<Hashtable>(); //数据池,要返回的 
            foreach (var item in list)
            {
                if (!item.ContainsKey(id)) continue;
                h[item[id].ToString()] = item;
            }
            foreach (var item in list)
            {
                if (!item.ContainsKey(id)) continue;
                if (!item.ContainsKey(pid) || item[pid] == null || !h.ContainsKey(item[pid].ToString()))
                {
                    r.Add(item);
                }
                else
                {
                    var pitem = h[item[pid].ToString()] as Hashtable;
                    if (!pitem.ContainsKey("children"))
                        pitem["children"] = new List<Hashtable>();
                    var children = pitem["children"] as List<Hashtable>;
                    children.Add(item);
                }
            }
            return r;
        }

        /// <summary>
        /// 行列转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string getProperties<T>(T t)
        {
            string tStr = string.Empty;
            if (t == null)
            {
                return tStr;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return tStr;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (value != null)
                    {
                        tStr += "{\"id\":\"" + name + "\",\"text\":\"" + value + "\"},";
                    }
                }
                else
                {
                    getProperties(value);
                }
            }
            return "[" + tStr.Substring(0, tStr.LastIndexOf(",")).ToString() + "]";
        }


        /// <summary>
        /// 执行SQL 返回json
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        //public static string ExecuteCommandToJSON(DbCommand command)
        //{
        //    var o = ExecuteReaderToHash(command);
        //    return ToJson(o);
        //}

        /// <summary>
        /// 执行SQL 返回json
        /// </summary>
        /// <param name="commandText"></param>
        ///// <returns></returns>
        //public static string ExecuteCommandToJSON(string commandText)
        //{
        //    var o = ExecuteReaderToHash(commandText);
        //    return ToJson(o);
        //}

        ///// <summary>
        ///// 将db reader转换为Hashtable列表
        ///// </summary>
        ///// <param name="commandText"></param>
        ///// <returns></returns>
        //public static List<Hashtable> ExecuteReaderToHash(string commandText)
        //{
        //    var reader = DbHelper.Db.ExecuteReader(commandText);
        //    return DbReaderToHash(reader);
        //}

        ///// <summary>
        ///// 将db reader转换为Hashtable列表
        ///// </summary>
        ///// <param name="command"></param>
        ///// <returns></returns>
        //public static List<Hashtable> ExecuteReaderToHash(DbCommand command)
        //{
        //    var reader = DbHelper.Db.ExecuteReader(command);
        //    return DbReaderToHash(reader);
        //}

        /// <summary>
        /// 将db reader转换为Hashtable列表
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static List<Hashtable> DbReaderToHash(IDataReader reader)
        {
            var list = new List<Hashtable>();
            while (reader.Read())
            {
                var item = new Hashtable();

                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    var value = reader[i];
                    item[name] = value;
                }
                list.Add(item);
            }
            return list;
        }
    }
}
