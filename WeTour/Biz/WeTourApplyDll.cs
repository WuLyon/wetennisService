using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Member;
using SMS;
using OrderLib;

namespace WeTour
{
    public class WeTourApplyDll
    {
        public static WeTourApplyDll instance = new WeTourApplyDll();

        /// <summary>
        /// Add new tour apply
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNewApply(WeTourApplyModel model)
        {
            string sql = string.Format("insert into wtf_tourapply (toursys,ContentId,memtype,memberid,status,ApplyDate,TourType,Paterner,ext1,ext2,ext3) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",model.TOURSYS,model.CONTENTID,model.memtype,model.MEMBERID,model.STATUS,DateTime.Now.ToString(),model.TOURTYPE,model.PATERNER,model.EXT1,model.EXT2,model.EXT3);
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
        /// 根据主键号获得报名实体
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public WeTourApplyModel GetModelbyId(string _id)
        {
            WeTourApplyModel model = new WeTourApplyModel();
            string sql = "select * from wtf_tourapply where id="+_id;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<WeTourApplyModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据子项id和用户主键获得报名实体
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public WeTourApplyModel GetModelbyContMem(string _ContentId, string _Memsys)
        {
            WeTourApplyModel model = new WeTourApplyModel();
            string sql = "select * from wtf_tourapply where contentid='" + _ContentId + "' and memberid='"+_Memsys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            { 
                model=JsonHelper.ParseDtInfo<WeTourApplyModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// Get Tour apply
        /// </summary>
        /// <param name="_Clubsys"></param>
        /// <param name="_Contentid"></param>
        /// <returns></returns>
        public List<WeTourApplyModel> GetClubTourApply(string _Clubsys, string _Contentid)
        {
            List<WeTourApplyModel> list = new List<WeTourApplyModel>();
            string sql = "select * from wtf_tourapply where contentid='" + _Contentid + "' and ext1='" + _Clubsys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourApplyModel>>(dt);
            }
            return list;
        }

        public List<WeTourApplyModel> GetApplyListbyCond(string _Cond)
        {
            List<WeTourApplyModel> list = new List<WeTourApplyModel>();
            string sql = "select * from wtf_tourapply where 1=1 "+_Cond;
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourApplyModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 获得报名人数
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public int GetApplyQty(string _ContentId)
        {
            string sql = "select * from wtf_TourApply where ContentId='" + _ContentId + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获得报名人数限制
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public int GetApplyCap(string _ContentId)
        {
            int ApplyCap = 0;
            WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_ContentId);
            if (model.AllowGroup == "1")
            {
                ApplyCap = Convert.ToInt32(model.SignQty) * Convert.ToInt32(model.GroupType);
            }
            else
            { 
            ApplyCap=Convert.ToInt32(model.SignQty);
            }
            return ApplyCap;
        }

        /// <summary>
        /// to see whether the person has applied
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <param name="_memsys"></param>
        /// <returns></returns>
        public bool IsMemberApplyed(string _ContentId, string _memsys)
        {
            string sql = string.Format("select * from wtf_tourapply where contentid='{0}' and (memberid='{1}' or Paterner='{1}')",_ContentId,_memsys);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsMemApplyTour(string _TourSys, string _memsys)
        {
            string sql = string.Format("select * from wtf_tourapply where toursys='{0}' and (memberid='{1}' or Paterner='{1}')", _TourSys, _memsys);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据赛事主键号获取报名实体
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<WeTourApplyModel> GetApplyListbyTour(string _Toursys)
        {
            List<WeTourApplyModel> list = new List<WeTourApplyModel>();
            string sql = "select * from wtf_tourapply where toursys='"+_Toursys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourApplyModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Add Club Apply
        /// </summary>
        /// <param name="_Player1"></param>
        /// <param name="_Player2"></param>
        /// <param name="_Contentid"></param>
        /// <returns></returns>
        public bool AddClubApply(string _Player1, string _Player2, string _Contentid,string _Clubsys)
        {
            if (IsMemberApplyed(_Contentid, _Player1))
            {
                return false;
            }
            WeTourApplyModel model = new WeTourApplyModel();
            WeTourContModel tcmodel = WeTourContentDll.instance.GetModelbyId(_Contentid);
            model.TOURSYS = tcmodel.Toursys;
            model.MEMBERID = _Player1;
            model.STATUS = "1";
            model.CONTENTID = _Contentid;
            model.EXT1 = _Clubsys;
            if (tcmodel.ContentType.LastIndexOf("双") > 0)
            {
                if (IsMemberApplyed(_Contentid, _Player1))
                {
                    return false;
                }
                model.PATERNER = _Player2;
                
            }

            return InsertNewApply(model);
        }

        /// <summary>
        /// Get Club Apply list
        /// </summary>
        /// <param name="_ClubSys"></param>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public List<WeTourApplyModel> GetApplyListbyClub(string _ClubSys, string _ContentId)
        {
            List<WeTourApplyModel> list = new List<WeTourApplyModel>();
            string sql = "select * from wtf_TourApply where contentid='"+_ContentId+"' and ext1='"+_ClubSys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourApplyModel>>(dt); 
            }
            return list;
        }

        /// <summary>
        /// Detele an apply record
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public bool DeleteApply(string _id)
        {
            WeTourApplyModel model = GetModelbyId(_id);
            string sql = "delete wtf_tourapply where id='"+_id+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                //删除订单主键
                OrderBll.Instance("wtf").DeleteOrder(model.EXT2);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get content apply qty
        /// </summary>
        /// <param name="_contentid"></param>
        /// <param name="_clubsys"></param>
        /// <returns></returns>
        public int GetApplyQty(string _contentid, string _clubsys)
        {
            int ApplyQty=0;
            string sql = "select count(id) from wtf_tourapply where contentid='" + _contentid + "' and ext1='" + _clubsys + "' and status=1";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            ApplyQty = Convert.ToInt32(dt.Rows[0][0].ToString());
            return ApplyQty;
        }

        /// <summary>
        /// Compute Tour apply money
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_Clubsys"></param>
        /// <returns></returns>
        public string GetClubApplyMoney(string _Toursys, string _Clubsys)
        {
            decimal OrderMoney = 0;
            string sql = "select distinct(contentid) from wtf_tourapply where toursys='"+_Toursys+"' and ext1='"+_Clubsys+"' and status=1";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //get Unit Price
                    WeTourContModel tcmodel = WeTourContentDll.instance.GetModelbyId(dr[0].ToString());
                    decimal UnitPrice = 0;
                    if (tcmodel.ContentType.IndexOf("双") > 0)
                    {
                        UnitPrice = 2 * WeTourContentDll.instance.GetContentApplyFee(dr[0].ToString());
                    }
                    else
                    {
                        UnitPrice = WeTourContentDll.instance.GetContentApplyFee(dr[0].ToString());
                    }

                    //get unpaid qty
                    int OrderQty = GetApplyQty(dr[0].ToString(), _Clubsys);
                    OrderMoney += OrderQty * UnitPrice;
                }
            }
            return OrderMoney.ToString() ;
        }

        /// <summary>
        /// 更新报名明细
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_Clubsys"></param>
        /// <param name="_OrderSys"></param>
        public void UpdateGroupApplyDetails(string _Toursys, string _Clubsys, string _OrderSys)
        {
            string sql = "update wtf_tourapply set ext2='" + _OrderSys + "' where toursys='" + _Toursys + "' and ext1='" + _Clubsys + "' and status=1";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// Get Club Apply Member names 
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_Clubsys"></param>
        /// <returns></returns>
        public string GetClubApplyDescription(string _Toursys, string _Clubsys)
        {
            string _ApplyDescription = "";
            string sql = "select distinct(contentid) from wtf_tourapply where toursys='" + _Toursys + "' and ext1='" + _Clubsys + "' and status=1";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string sql2 = "select * from wtf_tourapply where contentid='" + dr[0].ToString() + "' and ext1='" + _Clubsys + "' and status=1";
                    DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
                    if (dt2.Rows.Count > 0)
                    {
                        WeTourContModel tcmodel = WeTourContentDll.instance.GetModelbyId(dr[0].ToString());
                        _ApplyDescription += tcmodel.ContentName + "(";
                        foreach (DataRow dr2 in dt2.Rows)
                        {
                            if (tcmodel.ContentType.IndexOf("双") > 0)
                            {
                                _ApplyDescription += WeMemberDll.instance.GetModel(dr2["memberid"].ToString()).NAME + "/" + WeMemberDll.instance.GetModel(dr2["Paterner"].ToString()).NAME + ",";
                            }
                            else
                            {
                                _ApplyDescription += WeMemberDll.instance.GetModel(dr2["memberid"].ToString()).NAME + ",";
                            }
                        }
                        _ApplyDescription = _ApplyDescription.TrimEnd(',');
                        _ApplyDescription += "),";
                    }
                }
                _ApplyDescription = _ApplyDescription.TrimEnd(',');
            }
            return _ApplyDescription;
        }
        #region 报名费处理

        /// <summary>
        /// Get those who applied but not signed
        /// </summary>
        /// <param name="_Contentid"></param>
        /// <returns></returns>
        public List<WeTourApplyModel> GetUnSignedApply(string _Contentid)
        {
            List<WeTourApplyModel> list = new List<WeTourApplyModel>();
            WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_Contentid);
            string sql = "";
            if (model.ContentType.IndexOf("双") > 0)
            {
                //double match
                sql = "select * from wtf_tourapply  where ContentId='" + _Contentid + "' and   memberid+','+paterner not in (select membersys from wtf_TourSign where ContentId='" + _Contentid + "')";
            }
            else
            {
                //single match
                sql = "select * from wtf_tourapply  where ContentId='" + _Contentid + "' and   memberid not in (select membersys from wtf_TourSign where ContentId='" + _Contentid + "')";
            }
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourApplyModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// get UnSigned Apply by Tour sysno
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        public int GetAllApplySigned(string _TourSys)
        {
            int UnSigned = 0;
            List<WeTourContModel> list = WeTourContentDll.instance.GetContentlist(_TourSys);
            if (list.Count > 0)
            {
                foreach (WeTourContModel model in list)
                {
                    string _Contentid = model.id;
                    string sql = "";
                    if (model.ContentType.IndexOf("双") > 0)
                    {
                        //double match
                        sql = "select * from wtf_tourapply where ContentId='" + _Contentid + "' and   memberid+','+paterner not in (select membersys from wtf_TourSign where ContentId='" + _Contentid + "')";
                    }
                    else
                    {
                        //single match
                        sql = "select * from wtf_tourapply where ContentId='" + _Contentid + "' and   memberid not in (select membersys from wtf_TourSign where ContentId='" + _Contentid + "')";
                    }
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    UnSigned += dt.Rows.Count;
                }
            }
            return UnSigned;
        }

        /// <summary>
        /// 获得赛事报名费信息
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        public string GetTourApplyFeeInfo(string _TourSys)
        {
            string _ApplyFee = "";
            //wetennisfee
            _ApplyFee += WeTourApplyFee.instance.GetApplyFee(_TourSys, "2")+"|" ;
            //UnpaidFee
            _ApplyFee += WeTourApplyFee.instance.GetApplyFee(_TourSys, "1") + "|";
            //TotalFee
            _ApplyFee += WeTourApplyFee.instance.GetApplyFee(_TourSys, "");
            return _ApplyFee;
        }

        

        /// <summary>
        /// Get Applicants statics by tour sysno
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<TourApplyCon> GetApplyCon(string _Toursys)
        {
            List<TourApplyCon> list = new List<TourApplyCon>();
            List<WeTourContModel> conlist = WeTourContentDll.instance.GetContentlist(_Toursys);
            if (conlist.Count > 0)
            {
                foreach (WeTourContModel model in conlist)
                {
                    TourApplyCon nmoel = new TourApplyCon();
                    nmoel.ContentName = model.ContentName;
                    nmoel.ApplyFee = WeTourContentDll.instance.GetContentApplyFee(model.id).ToString();
                    nmoel.PaidQty = WeTourApplyFee.instance.GetApplyQty(model.id, "2");
                    nmoel.UnPaidQty = WeTourApplyFee.instance.GetApplyQty(model.id,"1");
                    nmoel.TotalQty = WeTourApplyFee.instance.GetApplyQty(model.id, "");
                    list.Add(nmoel);
                }
            }
            return list;
        }

        /// <summary>
        /// 获得签到表
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        public string GetTourApplyMember(string _TourSys)
        {
            string html = "";
            List<WeTourContModel> list = WeTourContentDll.instance.GetContentlist(_TourSys);
            if (list.Count > 0)
            {
                foreach (WeTourContModel model in list)
                { 
                    //添加子项名称
                    html += "<h2>"+model.ContentName+"</h2>";
                    //添加报名数据表
                    //添加表头
                    html += " <table class=\"table table-bordered\"><tr><th>序号</th><th style=\"width:100px\">姓名</th><th style=\"width:100px\">电话</th> <th style=\"width:100px\">身份证号</th><th>报名状态</th><th>签到</th> </tr>";
                    //添加表内容
                    string sql = "select a.id,a.applydate,a.status,a.TourType,a.Paterner,b.name,b.username,b.Province,b.city,b.region,b.address,b.Telephone,b.ext2 from wtf_TourApply a left join wtf_member b on a.memberid=b.sysno where a.Contentid='" + model.id + "' order by Convert(datetime,a.applydate) desc";
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    int i=1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        string username = dr["username"].ToString();
                        string Tel = dr["Telephone"].ToString();
                        string IDcard = dr["ext2"].ToString();
                        if (dr["Paterner"].ToString() != "" && model.ContentType.IndexOf("双") > 0)
                        {
                            WeMemberModel mem = WeMemberDll.instance.GetModel(dr["Paterner"].ToString());

                            username += "/<br />" + mem.USERNAME;
                            Tel += "/<br />" + mem.TELEPHONE;
                            IDcard += "/<br />" + mem.EXT2;
                        }
                        string status = (dr["status"].ToString() == "2") ? "已支付" : (dr["status"].ToString() == "1") ? "未支付" : "未知";
                        html += "<tr><td>" + i + "</td><td>" + username + "</td><td>" + Tel + "</td><td>" + IDcard + "</td><td>" + status + "</td><td>□</td> </tr>";
                        i += 1;
                    }
                    //添加表结尾
                    html += "</table><div style=\"page-break-after:always\"></div>";
                }
            }
            return html;
        }

        #endregion

        #region 处理未支付
        /// <summary>
        /// send sms
        /// </summary>
        /// <param name="_Toursys"></param>
        public void SendUnpaidMsg(string _Toursys)
        {
            string sql = "select * from wtf_TourApply where status='1' and toursys='" + _Toursys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    WeMemberModel mem = WeMemberDll.instance.GetModel(dr["memberid"].ToString());
                    WeTourContModel tco = WeTourContentDll.instance.GetModelbyId(dr["contentid"].ToString());
                    string Sms = mem.NAME + ",您好！您报名的" + tco.ContentName + "还未付款，系统将在24小时后删除报名信息，请尽快支付报名费";
                    SMSdll.instance.BatchSendSMS(mem.TELEPHONE, Sms);
                }

                //添加发送记录
                WeTourDll.instance.UpdateTourUnpaidInform(_Toursys);
            }
        }

        /// <summary>
        /// 删除报名信息
        /// </summary>
        /// <param name="_Toursys"></param>
        public void DeleteUnpaids(string _Toursys)
        {
            string sql = "delete wtf_TourApply where status='1' and toursys='" + _Toursys + "'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }
        #endregion

        #region 报名
        public string ApplyTour(string _TourSys,string _TourType,string _Partner,string memsys,string _RankScore)
        {
            string msg = "";//报名结果
            try
            {
                //验证球员是否登陆
                if (string.IsNullOrEmpty(memsys))
                {
                    msg = "请先登录再进行报名！";
                    return msg;
                }


                //验证球员是否重复报名
                if (IsAlreadyApply(memsys, _TourType, _Partner))
                {
                    msg = "不能重复报名！";
                    return msg;
                }
                //验证双打是否选择搭档
                WeTourContModel tcmodel = WeTourContentDll.instance.GetModelbyId(_TourType);
                if (tcmodel.ContentType.IndexOf("双") > 0)
                {
                    if (_Partner == "" || _Partner == "-1")
                    {
                        msg = "请选择双打搭档，首先在会员中心-联系人中添加搭档！";
                        return msg;
                    }
                }

                string _Ordersys = "";
                //添加报名支付订单
                try
                {
                    _Ordersys = AddApplyOrder(_TourSys, _TourType, memsys, _Partner);
                }
                catch
                {

                }
                WeTourApplyModel model = new WeTourApplyModel();
                model.TOURSYS = _TourSys;
                model.MEMBERID = memsys;
                model.TOURTYPE = _TourType;
                model.CONTENTID = _TourType;
                model.PATERNER = _Partner;
                model.EXT2 = _Ordersys;
                model.STATUS = "1";
                //添加报名人员积分信息
                model.EXT3 = _RankScore;

                if (InsertNewApply(model))
                {
                    msg = "ok";
                    #region 判断报名者身份信息和是否需要支付报名费
                    //验证身份信息
                    bool IsIdentity = true;
                    if (WeMemberDll.instance.IsTelephoneRight(memsys) && WeMemberDll.instance.IsIDcardRight(memsys))
                    {
                        IsIdentity = true;
                        if (tcmodel.ContentType.IndexOf("双") > 0)
                        {
                            //验证搭档身份信息
                            if (WeMemberDll.instance.IsTelephoneRight(_Partner) && WeMemberDll.instance.IsIDcardRight(_Partner))
                            {
                                IsIdentity = true;
                            }
                            else
                            {
                                IsIdentity = false;
                            }
                        }
                    }
                    else
                    {
                        IsIdentity = false ;
                    }

                    //验证报名费是否大于0
                    bool ApplyFee;
                    int ContentFee = WeTourApplyFee.instance.GetApplyFee(_TourType);
                    if (ContentFee > 0)
                    {
                        ApplyFee = false;
                    }
                    else
                    {
                        ApplyFee = true;
                    }

                    //根据判断结果制定返回值

                    if (IsIdentity && ApplyFee)
                    {
                        //身份信息完整，报名费为0元--直接将报名状态改为2，将订单状态改为2.发送一条确认短信
                        OrderBll.Instance("wtf").UpdateBusinessOrder(_Ordersys);
                        msg = "1";
                    }
                    else if (IsIdentity && !ApplyFee)
                    {
                        //身份信息完整，报名费大于0——发送报名成功，请支付短信，跳转到收银台，付款成功后，修改报名及订单状态。
                        SendApplyConfirm(_TourType, memsys);
                        msg = "2";
                    }
                    else if (!IsIdentity && ApplyFee)
                    {
                        //身份信息不完成，报名费为0——跳转到身份信息确认页面，电话和身份证确认
                        msg = "3";
                    }
                    else if (!IsIdentity && !ApplyFee)
                    {
                        //身份信息不完整，报名费大于0——跳转到身份信息确认页面，确认后,再发送确认短信
                        msg = "4";
                    }
                    #endregion

                }
                else
                {
                    msg = "no";
                }
            }
            catch(Exception e)
            {
                msg = e.ToString();
            }
            return msg;
        }

        /// <summary>
        /// 发送报名短信
        /// </summary>
        /// <param name="_TourType"></param>
        /// <param name="_Mem"></param>
        public void SendApplyConfirm(string _TourType, string _Mem)
        {
            WeTourContModel cmodel = WeTourContentDll.instance.GetModelbyId(_TourType);
            WeTourModel tmodel = WeTourDll.instance.GetModelbySys(cmodel.Toursys);
            WeMemberModel mem = WeMemberDll.instance.GetModel(_Mem);
            string Msg = mem.USERNAME + ",您好!您已报名" + tmodel.NAME + cmodel.ContentName + ",请尽快付款";
            SMSdll.instance.BatchSendSMS(mem.TELEPHONE, Msg);                
        }

        /// <summary>
        /// 获得微网球积分
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public string GetSocialScort(string _Memsys)
        {
            string sql = "select sum(points) as points from rank_points where  IsSingle='单打' and Gender='男' and memsys='" + _Memsys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows[0][0].ToString();
        }

        /// <summary>
        /// 添加赛事报名支付订单
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_TourContent"></param>
        /// <returns></returns>
        public string AddApplyOrder(string _Toursys, string _TourContent, string _Memsys, string _Parterner)
        {
            WeTourModel tmodel =WeTourDll.instance.GetModelbySys(_Toursys);//获取实体
            WeTourContModel tcmodel = WeTourContentDll.instance.GetModelbyId(_TourContent);
            WeMemberModel mmodel = WeMemberDll.instance.GetModel(_Memsys);
            WeMemberModel smodel = WeMemberDll.instance.GetModel(tmodel.MGRSYS);

            //计算订单支付金额
            string _OrderMoney = tmodel.EXT1;
            if (!string.IsNullOrEmpty(tcmodel.ext3))
            {
                _OrderMoney = tcmodel.ext3;
            }
            if (tcmodel.ContentType.IndexOf("双")>0)
            {
                //双打支付双倍的钱
                _OrderMoney = (Convert.ToDecimal(_OrderMoney) * 2).ToString();
            }

            //添加订单主记录
            string Ordersys = OrderLib.OrderBll.Instance("Wtf").InsertOrderMain(tmodel.NAME + tcmodel.ContentName + "报名费", "apply", _Memsys, mmodel.NAME, mmodel.ADDRESS, mmodel.TELEPHONE, _OrderMoney, "", _OrderMoney, "", "1", tmodel.MGRSYS, tmodel.NAME, smodel.NAME, smodel.TELEPHONE, _TourContent, "", "", "", "");
            
            return Ordersys;
        }

        public string AddGroupApplyOrder(string _Toursys, string _TourContent, string _Memsys, string _groupName)
        {
            WeTourModel tmodel = WeTourDll.instance.GetModelbySys(_Toursys);//获取实体
            WeTourContModel tcmodel = WeTourContentDll.instance.GetModelbyId(_TourContent);
            WeMemberModel mmodel = WeMemberDll.instance.GetModel(_Memsys);

            //计算订单支付金额
            string _OrderMoney = tcmodel.ext3;
            
            //添加订单主记录
            string Ordersys = OrderLib.OrderBll.Instance("Wtf").InsertOrderMain(tmodel.NAME + tcmodel.ContentName + "报名费", "apply", _Memsys, mmodel.NAME, mmodel.ADDRESS, mmodel.TELEPHONE, _OrderMoney, "", _OrderMoney, "", "1", tmodel.MGRSYS, tmodel.NAME, "", "", _TourContent, _groupName, "", "", "");

            return Ordersys;
        }


        /// <summary>
        /// 验证是否已报名或者是做为双打搭档已报名
        /// 需增加性别验证，同时同性质比赛不能重复报名
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <param name="_Player"></param>
        /// <param name="_Type"></param>
        /// <returns></returns>
        public bool IsAlreadyApply(string _Player, string _Type, string _Partner)
        {
            WeTourContModel tcmodel =WeTourContentDll.instance.GetModelbyId(_Type);

            //验证是否为报名人
            string sql = "select * from wtf_tourapply where memberid='" + _Player + "' and Contentid = '" + _Type + "' and status<10";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count == 0)
            {
                //未报名该子项
                if (tcmodel.ContentType.IndexOf('双') > 0)
                {
                    //验证当前报名人是否为双打partner
                    string sql1 = "select * from wtf_tourapply where  Contentid = '" + _Type + "' and paterner='" + _Player + "' and status<10";
                    DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                    //验证partner是否已报名双打
                    string sql2 = "select * from wtf_tourapply where Contentid = '" + _Type + "' and memberid='" + _Partner + "' and status<10";
                    DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
                    //验证partner是否已作为其他人的partner
                    string sql3 = "select * from wtf_tourapply where  Contentid = '" + _Type + "' and paterner='" + _Partner + "' and status<10";
                    DataTable dt3 = DbHelperSQL.Query(sql3).Tables[0];
                    if (dt1.Rows.Count + dt2.Rows.Count + dt3.Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {                  
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 验证是否满足报名条件
        /// 0,不需要验证，1，验证是否已报名，2，验证性别，3，验证年龄段
        /// </summary>
        /// <param name="model"></param>
        /// <param name="_Level"></param>
        /// <returns></returns>
        public string ValidateContApply(WeTourApplyModel model,int _Level)
        {
            string _Res = "ok";
            if (_Level == 0)
            { 
                //验证级别最低，不需要验证
                if (string.IsNullOrEmpty(model.MEMBERID))
                {
                    _Res = "报名信息不完全";
                }
                return _Res;
            }
            WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(model.CONTENTID);
            WeMemberModel mem1 = WeMemberDll.instance.GetModel(model.MEMBERID);
            WeMemberModel mem2 = WeMemberDll.instance.GetModel(model.PATERNER);

            if (_Level >= 1)
            {
                //验证是否已报名
                if (IsAlreadyApply(model.MEMBERID, model.CONTENTID, model.PATERNER))
                {
                    _Res = "已经报名该项目";
                    return _Res;
                }
            }

            if (_Level >= 2)
            {
                //验证性别
                string Sex = cont.ContentType.Substring(0, 1);
                if (mem1.GENDER != Sex)
                {
                    _Res = "球员性别不匹配";
                    return _Res;
                }
                if (cont.ContentType.IndexOf("双") > 0)
                {
                    if (mem2.GENDER != Sex)
                    {
                        _Res = "搭档性别不匹配";
                        return _Res;
                    }
                }
            }

            if (_Level >= 3)
            {
                //验证年龄段

            }

            return _Res;

        }


        /// <summary>
        /// 根据id获得子项报名情况
        /// </summary>
        /// <param name="_ContId"></param>
        /// <returns></returns>
        public List<WeTourApplyModel> getContentApplyByid(string _ContId)
        {
            List<WeTourApplyModel> list = new List<WeTourApplyModel>();
            string sql = "select * from wtf_tourapply where ContentId='" + _ContId + "' and status<5 order by id desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WeTourApplyModel model = new WeTourApplyModel();
                    model.APPLYDATE = dt.Rows[i]["applydate"].ToString();
                    model.STATUS = (dt.Rows[i]["status"].ToString() == "1") ? "未支付" : (dt.Rows[i]["status"].ToString() == "2") ? "已支付" : "未知";
                    WeMemberModel member = WeMemberDll.instance.GetModel(dt.Rows[i]["MEMBERID"].ToString());
                    model.MemberName = member.USERNAME;
                    model.MemberImg = member.EXT1;
                    if (dt.Rows[i]["PATERNER"].ToString() != "")
                    {
                        WeMemberModel Patmem = WeMemberDll.instance.GetModel(dt.Rows[i]["PATERNER"].ToString());
                        model.ParName = Patmem.USERNAME;
                        model.ParImg = Patmem.EXT1;
                    }
                    list.Add(model);
                }
            }
            return list;
        }
        
        #endregion

        #region 报名种子
        public List<WeMemberModel> GetSeedApplyMember(string _Cond, string _Contentid)
        {
            List<WeMemberModel> list = new List<WeMemberModel>();
            WeTourContModel contModel = WeTourContentDll.instance.GetModelbyId(_Contentid);
            string sql = "";
            if (contModel.ContentType.IndexOf("双") > 0)
            {
                //双打
                sql = string.Format("select b.* from wtf_tourapply a left join wtf_member b on a.memberid=b.sysno where contentid='{0}' and b.Name+b.UserName like '%{1}%'", _Contentid, _Cond);
            }
            else
            {
                sql = string.Format("select b.* from wtf_tourapply a left join wtf_member b on a.memberid=b.sysno where contentid='{0}' and b.Name+b.UserName like '%{1}%'", _Contentid, _Cond);
            }
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMemberModel>>(dt);
            }
            return list;
        }
        #endregion

        #region 获取报名信息
        /// <summary>
        /// 获取各个项目报名的情况
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        public List<WeTourContModel> GetContApplicant(string _TourSys)
        {
            List<WeTourContModel> list = WeTourContentDll.instance.GetContentlist(_TourSys);
            foreach (WeTourContModel model in list)
            {
                model.ext1 = GetApplyQty(model.id).ToString();
            }
            return list;
        }
        /// <summary>
        /// 根据项目id获取报名名单，2016-5-15
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public List<WeTourApplyModel> GetContAppMemberInfo(string _ContentId)
        {
            List<WeTourApplyModel> list = new List<WeTourApplyModel>();
            string sql = "select * from wtf_tourapply where contentid='"+_ContentId+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeTourApplyModel>>(dt);
                foreach(WeTourApplyModel model in list)
                {
                    if (model.memtype == "group")
                    {
                        weTeamModel team = weTeamdll.instance.GetModel(model.MEMBERID);
                        model.ApplyName = team.TEAMNAME;
                    }
                    else
                    {
                        WeMemberModel mem = WeMemberDll.instance.GetModel(model.MEMBERID);
                        model.ApplyName = mem.USERNAME;
                        if (model.PATERNER != "")
                        {
                            WeMemberModel mem1 = WeMemberDll.instance.GetModel(model.PATERNER);
                            model.ApplyName += "," + mem1.USERNAME;
                        }
                    }
                }

            }
            return list;
        }
        #endregion


    #region EXCEL导入报名信息
        /// <summary>
        /// 将从EXCEL读取的到的DataTable内容导入报名信息
        /// </summary>
        /// <param name="dt"></param>
        public List<WeTourApplyModel> ExcelApplicants(DataTable dt,string _Toursys)
        {
            List<WeTourApplyModel> list = new List<WeTourApplyModel>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                { 
                    //验证数据有效性
                    string Name = dr[1].ToString();//姓名
                    string Tele = dr[2].ToString();//电话
                    string IDCard = dr[3].ToString();//身份证号
                    string PartName = dr[4].ToString();//搭档姓名
                    string PartTele = dr[5].ToString();//搭档电话
                    string PartCard = dr[6].ToString();//搭档身份证
                    string ContName = dr[7].ToString();//报名项目
                    DbHelperSQL.WriteLog("ExcelApplicants", Name + "-"+Tele +"-"+ _Toursys);
                    if (Name != "" && ContName != "")
                    {
                        #region 验证数据与报名

                        //数据齐全
                        WeTourApplyModel model = new WeTourApplyModel();
                        model.TOURSYS = _Toursys;
                        model.CONTENTNAME = ContName;
                        WeTourContModel cont = WeTourContentDll.instance.GetModelbyContName(ContName, _Toursys);
                        if (cont.id != null)
                        {
                            model.CONTENTID = cont.id;
                            model.CONTENTNAME = cont.ContentName;
                            int price = -999;
                            int.TryParse(model.EXT3, out price);
                            if (price == 0)
                            {
                                model.STATUS = "2";
                            }
                            else
                            { 
                                model.STATUS = "1"; 
                            }
                            //查找用户信息                            
                            WeMemberModel mem = WeMemberDll.instance.GetModelbyTelephone(Tele);
                            if (mem.SYSNO == null)
                            {
                                //TelePhone 不存在，新创建用户
                                mem.NAME = Name;
                                mem.USERNAME = Name;
                                mem.TELEPHONE = Tele;//根据电话创建用户
                                mem.EXT2 = IDCard;
                                mem.PASSWORD = "123";
                                mem.SYSNO = WeMemberDll.instance.CreateUser(mem);
                            }
                            model.MEMBERID = mem.SYSNO;

                            //验证是否双打
                            if (cont.ContentType.IndexOf("双") > 0)
                            {
                                //查找用户信息                            
                                WeMemberModel mem1 = WeMemberDll.instance.GetModelbyTelephone(PartTele);
                                if (mem1.SYSNO == null)
                                {
                                    //TelePhone 不存在，新创建用户
                                    mem1.NAME = PartName;
                                    mem1.USERNAME = PartName;
                                    mem1.TELEPHONE = PartTele;//根据电话创建用户
                                    mem1.EXT2 = PartCard;
                                    mem1.PASSWORD = "123";
                                    mem1.SYSNO = WeMemberDll.instance.CreateUser(mem1);
                                }
                                model.PATERNER = mem1.SYSNO;
                            }

                            //添加报名类型标记
                            model.EXT3 = "EXCEL";

                            //验证是否报名
                            if (ValidateContApply(model, 1) == "ok")
                            {
                                //通过验证，可以报名,则添加报名信息
                                InsertNewApply(model);
                                model.EXT1 = "已成功报名";
                            }
                            else
                            {
                                model.EXT1 = "已报名，不能重复报名";
                            }
                        }
                        else
                        {
                            model.EXT1 = "报名项目不正确" + _Toursys + ContName;
                        }
                        //将返回信息添加到list，准备返回
                        model.MemberName = Name + PartName;
                        list.Add(model);
                        #endregion
                    }//end of if 
                         
                }//end of foreach
                       
            }
            return list;
        }
    #endregion

        #region 已报名任务列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_toursys"></param>
        /// <param name="_userId"></param>
        /// <returns></returns>
        public List<WeTourContModel> Get_Applied_Content(string _toursys,string _userId)
        {
            List<WeTourContModel> cont_list = new List<WeTourContModel>();
            string sql = string.Format("select * from wtf_tourapply where (memberid='{0}' or paterner='{0}') and status in (1,2) and toursys='{1}'",_userId,_toursys);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(dt.Rows[i]["ContentId"].ToString());
                cont_list.Add(cont);
            }
                return cont_list;
        }
        #endregion

    }

    

    /// <summary>
    /// 报名费实体
    /// </summary>
    public class TourApplyCon
    {
        public string ContentName { get; set; }
        public string ApplyFee { get; set; }
        public string PaidQty { get; set; }
        public string UnPaidQty { get; set; }
        public string TotalQty { get; set; }
    }
}
