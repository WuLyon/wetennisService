using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Ranking
{
    public class RPointDll
    {
        public static RPointDll instance = new RPointDll();

        /// <summary>
        /// Insert New Point Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNew(PointsModel model)
        {
            string sql = string.Format("insert into rank_points (Memsys,PointType,TypeSys,Points,IsSingle,Gender,TourSys,ContentId,Remark,UpdateDate) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}') ", model.MemSys, model.PointType, model.TypeSys, model.Points, model.IsSingle, model.Gender, model.TourSys, model.ContentId, model.Remark, DateTime.Now.ToString());
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 添加积分
        /// </summary>
        /// <param name="_PointType">积分类型，微网球级别积分默认为空，俱乐部赛事积分为Club或社区</param>
        /// <param name="_TypeSys">如果积分类型是CLub，则这里填写俱乐部的sysno</param>
        /// <param name="_Point"></param>
        /// <param name="_gender"></param>
        /// <param name="_Memberid"></param>
        /// <param name="_Remark"></param>
        /// <param name="_ContentId"></param>
        /// <param name="_TourSys"></param>
        public void AddRankPoint(string _PointType,string _TypeSys, string _Point,string _gender, string _Memberid, string _Remark, string _ContentId,string _TourSys)
        {
            
            //parseInt a Point
            if (_Point.IndexOf('.') > 0)
            {
                _Point = _Point.Substring(0, _Point.IndexOf('.'));
            }
            //
            if (_Memberid.IndexOf(',') > 0)
            {
                //双打比赛
                string[] Players = _Memberid.Split(',');
                for (int i = 0; i < Players.Length; i++)
                {
                    string sql = "select gender from wtf_member where sysno='" + Players[i] + "'";
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    _gender = dt.Rows[0][0].ToString();

                    PointsModel prModel = new PointsModel() ;
                    prModel.MemSys = Players[i];
                    prModel.PointType = _PointType;
                    prModel.TypeSys = _TypeSys;
                    prModel.Points = _Point;
                    prModel.IsSingle = "双打";
                    prModel.Gender = _gender;
                    prModel.TourSys = _TourSys;
                    prModel.ContentId = _ContentId;
                    prModel.Remark = _Remark;

                    InsertNew(prModel);
                }
            }
            else
            {
                //单打比赛
                PointsModel prModel = new PointsModel();
                prModel.MemSys = _Memberid;
                prModel.PointType = _PointType;
                prModel.TypeSys = _TypeSys;
                prModel.Points = _Point;
                prModel.IsSingle = "单打";
                prModel.Gender = _gender;
                prModel.TourSys = _TourSys;
                prModel.ContentId = _ContentId;
                prModel.Remark = _Remark;

                InsertNew(prModel);
            }
   
        }

        /// <summary>
        /// 获取社区积分
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public string GetSocialScort(string _Memsys)
        {
            string sql = "select sum(points) as points from rank_points where  pointType='社区' and memsys='" + _Memsys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows[0][0].ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public string GetMatchScor(string _Memsys,string _IsSigle)
        {
            string _Point = "0";
            int P=0;
            string sql = "select sum(points) as points from rank_points where  pointType='' and memsys='" + _Memsys + "' and IsSingle='" + _IsSigle + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];            
            _Point = dt.Rows[0][0].ToString();
            try
            {
                P = Convert.ToInt32(_Point);
            }
            catch {
                P = 0;
            }
            return P.ToString();
        }

        /// <summary>
        /// 获取一个赛事获取的积分
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <param name="_IsSingle"></param>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        public int GetSocre_byTour(string _Memsys, string _IsSingle, string _TourSys)
        {
            string _Point = "0";
            int P = 0;
            string sql = "select sum(points) as points from rank_points where  pointType='' and memsys='" + _Memsys + "' and IsSingle='" + _IsSingle + "' and TourSys='" + _TourSys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            _Point = dt.Rows[0][0].ToString();
            try
            {
                P = Convert.ToInt32(_Point);
            }
            catch
            {
                P = 0;
            }
            return P;
        }
    }
}
