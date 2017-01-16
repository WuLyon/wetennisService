using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WeTour
{
    /// <summary>
    /// 用于配置报名页面所需的内容
    /// </summary>
    public class WeTourApplyPage
    {
        public static WeTourApplyPage instance = new WeTourApplyPage();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<WeTourContModel> GetContentListForApply(string _Toursys,string _Memsys)
        {
            List<WeTourContModel> list = WeTourContentDll.instance.GetContentlist(_Toursys);
            //添加报名数据
            foreach (WeTourContModel model in list)
            {
                int a = WeTourApplyDll.instance.GetApplyQty(model.id);
                int b = WeTourApplyDll.instance.GetApplyCap(model.id);
                model.ApplyInfo += "(" + a.ToString() + "/" + b.ToString() + ")";
                if (a < b)
                {
                    model.ApplyInfo += "可报名";
                }
                else
                {
                    model.ApplyInfo += "已报满";
                }

                //判断是否已报名
                if (WeTourApplyDll.instance.IsAlreadyApply(_Memsys, model.id, _Memsys))
                {
                    model.ApplyInfo += "[已报名]";
                }
            }
            return list;
        }

        public void ApplyContent(string _ContentId, string Memsys, string _Parterner)
        { 
        
        }
    }
}
