using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Club
{
    public class ClubActsBiz
    {
        public static ClubActsBiz instance = new ClubActsBiz();

        /// <summary>
        /// insert new club activity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string InsertNew(ClubActsModel model)
        {
            string Rtr = "";
            string sysno = "";
            if (!string.IsNullOrEmpty(model.SYSNO))
            {
                sysno = model.SYSNO;
            }
            else
            {
                sysno = Guid.NewGuid().ToString("N").ToUpper();
            }
            string sql = string.Format("insert into Club_Activity (sysno,clubsys,Title,WriterName,WriterSys,UpdateDate,status) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",sysno,model.CLUBSYS,model.TITLE,model.WRITERNAME,model.WRITERSYS,DateTime.Now.ToString(),"1");
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                Rtr = sysno;
            }
            return Rtr;
        }

        /// <summary>
        /// get club activity model by activty sysno
        /// </summary>
        /// <param name="sysno"></param>
        /// <returns></returns>
        public ClubActsModel getModel(string sysno)
        {
            ClubActsModel model = new ClubActsModel();
            string sql = "select * from Club_Activity where sysno='"+sysno+"' and status='1'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<ClubActsModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// get activitis by club sysno 
        /// </summary>
        /// <param name="_ClubSys"></param>
        /// <returns></returns>
        public List<ClubActsModel> GetClubActs(string _ClubSys)
        {
            List<ClubActsModel> list = new List<ClubActsModel>();
            string sql = "select * from Club_Activity where clubsys='" + _ClubSys + "' and status='1' order by id desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<ClubActsModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Update Status
        /// </summary>
        /// <param name="_sysno"></param>
        /// <returns></returns>
        public bool DeleteClubActSys(string _sysno,string _Status)
        {
            string sql = "update Club_Activity set status='" + _Status + "' where sysno='" + _sysno + "'";
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




    }
}
