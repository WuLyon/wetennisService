using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TourMethod
{
    class MsgBll
    {
        public MsgBll instance = new MsgBll();

        /// <summary>
        /// 给用户发送支付确认通知的短信
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public bool SendPaymentConfirm(string _ContentId, string _Memsys)
        {
            try
            {
                MemberModel model = MemberDll.Get_Instance().GetModel(_Memsys);
                TourContentModel tcmodel = TourContentDll.Get_Instance().GetModelbyIdwithApplyPrice(_ContentId);
                TourModel tmodel = TourDll.Get_Instance().GetModel(tcmodel.Toursys);
                //加载赛事信息
                string ApplyFee = "";
                if (tcmodel.ContentType.IndexOf("双") > 0)
                {
                    //添加双打搭档
                    string PartSys = TourDll.Get_Instance().GetPartnerSys(_ContentId, _Memsys);
                    MemberModel ParModel = MemberDll.Get_Instance().GetModel(PartSys);
                    ApplyFee += "搭档：" + ParModel.NAME;
                }
                string TourInfo = tmodel.NAME + "-" + tcmodel.ContentName + "。" + ApplyFee;
                string Content = string.Format(MsgBll.instance.GetModulebyEnum("1002").MSGCONTENT, model.USERNAME, TourInfo);
                //判断是否已发送确认短信
                if (!SMSdll.instance.IsMsgSent(model.TELEPHONE, Content))
                {
                    //发送确认短信
                    SMSdll.instance.SendSMS(model.TELEPHONE, Content);

                    //添加系统消息
                    MsgModel mmodel = new MsgModel();
                    mmodel.MsgContent = Content;
                    mmodel.FromMsys = "city01";
                    mmodel.ToMsys = model.SYSNO;
                    mmodel.Status = "1";
                    MsgBll.instance.Insert(mmodel);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
