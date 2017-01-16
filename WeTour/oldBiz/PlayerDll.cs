using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class PlayerDll
    {
        public PlayerDll() { }
        private static PlayerDll _Instance;
        public static PlayerDll Get_Instance()
        {
            if (_Instance == null)
            {
                _Instance = new PlayerDll();
            }
            return _Instance;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public PlayerModel GetModel(string _Sys)
        {
            PlayerModel model = new PlayerModel();
            DataTable dt = PlayerDac.SelectList(string.Format(" and sys='{0}'", _Sys));
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<PlayerModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 获取所有的比赛选手
        /// </summary>
        /// <returns></returns>
        public List<PlayerModel> GetModelList(string _Name, string _Address)
        {
            List<PlayerModel> modelist = new List<PlayerModel>();
            DataTable dt = PlayerDac.SelectList(string.Format(" and pname like '%{0}%' and address like '%{1}%'", _Name, _Address));
            if (dt.Rows.Count > 0)
            {
                modelist = JsonHelper.ParseDtModelList<List<PlayerModel>>(dt);
            }
            return modelist;
        }

        public DataTable GetPlayerList()
        {
            DataTable dt = PlayerDac.SelectList(string.Format(""));
            return dt;
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MessageInfo Insert(PlayerModel model)
        {
            MessageInfo mess = new MessageInfo();
            model.SYS = Guid.NewGuid().ToString("N").ToUpper();
            if (PlayerDac.Insert(model))
            {
                mess.IsSucceed = true;
                mess.Message = "保存成功！";
            }
            else
            {
                mess.IsSucceed = false;
                mess.Message = "保存不成功！";
            }
            return mess;
        }
    }
}
