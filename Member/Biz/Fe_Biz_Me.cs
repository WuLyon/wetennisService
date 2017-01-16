using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Member
{
    public class Fe_Biz_Me
    {
        public static Fe_Biz_Me instance = new Fe_Biz_Me();

        public Fe_Model_Me fetchMyData(string _userId)
        {
            Fe_Model_Me model = new Fe_Model_Me();
            WeMemberModel mem = WeMemberDll.instance.GetModel(_userId);
            model.name = mem.NAME;
            model.gender = mem.GENDER;
            model.birthday = mem.BIRTHDAY;
            try
            {
                DateTime birth = Convert.ToDateTime(mem.BIRTHDAY);
                model.Constellation = getAstro(birth.Month, birth.Day);
            }
            catch
            {
                model.Constellation = "";
            }
            model.friendsNum = WeMemberDll.instance.GetFollowerModellist(_userId).Count;
            model.attentionsNum = WeMemberDll.instance.GetFollowModellist(_userId).Count;
            model.score = 0;
            model.gamesNum = 0;
            model.guessNum = 0;
            model.backGroundImageUrl = mem.EXT7;

            return model;
        }

        private String getAstro(int month, int day)
        {
            String[] starArr = {"魔羯座","水瓶座", "双鱼座", "牡羊座",
        "金牛座", "双子座", "巨蟹座", "狮子座", "处女座", "天秤座", "天蝎座", "射手座" };
            int[] DayArr = { 22, 20, 19, 21, 21, 21, 22, 23, 23, 23, 23, 22 };  // 两个星座分割日
            int index = month;
            // 所查询日期在分割日之前，索引-1，否则不变
            if (day < DayArr[month - 1])
            {
                index = index - 1;
            }
            // 返回索引指向的星座string
            if (index == 12)
            {
                index = 0;
            }
            return starArr[index];
        }

        public Fe_Model_Comment eventComments(string _id)
        {
            Fe_Model_Comment model = new Fe_Model_Comment();
            model.total = 100;
            //加载comments

            return model;
        }

        /// <summary>
        /// 查找双打搭档
        /// </summary>
        /// <param name="_userid"></param>
        /// <returns></returns>
        public List<Fe_Model_fetchPartners> GetDoubleParterner(string _userid)
        {
            List<Fe_Model_fetchPartners> list = new List<Fe_Model_fetchPartners>();
            List<WeMemberModel> followers = WeMemberDll.instance.GetFollowerModellist(_userid);
            foreach (WeMemberModel model in followers)
            {
                Fe_Model_fetchPartners part = new Fe_Model_fetchPartners();
                part.id = model.SYSNO;
                part.name = model.USERNAME;
                part.username = model.NAME;
                part.phone = model.TELEPHONE;

                //根据身份证号读取年龄
                if (model.EXT2.Length == 18)
                {
                    //身份证位数有效
                    string year = model.EXT2.Substring(6, 4);
                    int age = DateTime.Now.Year - Convert.ToInt32(year);
                    part.age = age; 
                }
                else
                {
                    part.age = 100;//未维护身份证，默认20岁
                }

                list.Add(part);
            }
            return list;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMeSetting(Fe_Model_updateMySettings model)
        { 
            //修改个人信息
            WeMemberModel member = new WeMemberModel();
            member.SYSNO = model.userId;
            member.NAME = model.name;
            member.GENDER = model.gender;
            member.BIRTHDAY = model.birthday;
            member.EMAIL = model.email;
            member.TENNISSTARTYEAR = model.startYear.ToString();
            member.FOREHAND = GetForhandStr(model.hand);
            member.BACKHAND = GetBackHandStr(model.habit);
            member.HEIGHT = model.height.ToString();
            member.WEIGHT = model.weight.ToString();
            member.EXT4 = model.club;
            member.EXT5 = model.companyName;
            member.EXT6 = model.companyTitle;
            member.TELEPHONE = model.phone;
            member.EXT2 = model.cardId;

            if (WeMemberDll.instance.UpdateUser(member))
            {               
                //修改Rank
                 Model_Me_TechRank rank=new Model_Me_TechRank();
                 rank.memsys = model.userId;
                 rank.Rank = model.Rank;
                 rank.TechRank = model.TechRank.ToString();
                 rank.SelfTechRank = model.SelfTechRank.ToString();
                 rank.OtherTechRank = model.OtherTechRank.ToString();
                if (Biz_Me_TechRank.instance.IsMemExist(model.userId))
                {
                    //修改
                    Biz_Me_TechRank.instance.UpdateRank(rank);
                }
                else
                { 
                    //新增
                    Biz_Me_TechRank.instance.InsertNew(rank);
                }

                //修改地址
                Biz_Me_Address.intance.DeleteMyAddress(model.userId);
                for ( int i=0;i<model.address.Count;i++)
                {
                    Fe_Model_updateMySettings_address add = model.address[i];
                    Model_Me_Address myAdd = new Model_Me_Address();
                    myAdd.memsys = model.userId;
                    myAdd.name = add.name;
                    myAdd.phone = add.phone;
                    myAdd.district = add.district;
                    myAdd.detailAddress = add.detailAddress;
                    myAdd.post = add.post;
                    myAdd.isDefault = 0;
                    myAdd.is_Active = 1;
                    Biz_Me_Address.intance.AddNewString(myAdd);
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        #region ForHand&BackHand
        private string GetForhandStr(int _hand)
        {
            string _forHand = "";
            if (_hand == 1)
            {
                _forHand = "左手";
            }
            else
            {
                _forHand = "右手";
            }
            return _forHand;
        }

        private int GetForHandCode(string _forhand)
        {
            int _code = 0;
            if (_forhand == "左手")
            {
                _code = 1;
            }
            else
            {
                _code = 2;
            }
            return _code;
        }

        private string GetBackHandStr(int _habit)
        {
            string _backHand = "";
            switch (_habit)
            {
                case 0:
                    _backHand = "单手正拍";
                    break;
                case 1:
                    _backHand = "双手正拍";
                    break;
                case 2:
                    _backHand = "单手反拍";
                    break;
                case 3:
                    _backHand = "双手反拍";
                    break;
                default:
                    _backHand = "双手反拍";
                    break;
            }
            return _backHand;
        }

        private int GetBackHandCode(string _BackHand)
        {
            int _hanbit = 0;
            switch (_BackHand)
            {
                case "单手正拍":
                    _hanbit = 0;
                    break;
                case "双手正拍":
                    _hanbit = 2;
                    break;
                case "单手反拍":
                    _hanbit = 3;
                    break;
                case "双手反拍":
                    _hanbit = 4;
                    break;
                default:
                    _hanbit = 4;
                    break;
            }
            return _hanbit;
        }
        #endregion

        public Fe_Model_updateMySettings fetchMySetting(string _userId)
        {
            Fe_Model_updateMySettings model = new Fe_Model_updateMySettings();

            //获取我的信息
            WeMemberModel mem = WeMemberDll.instance.GetModel(_userId);
            model.name = mem.NAME;
            model.email = mem.EMAIL;
            model.gender = mem.GENDER;
            model.birthday = mem.BIRTHDAY;
            model.club = mem.EXT4;
            model.companyName = mem.EXT5;
            model.companyTitle = mem.EXT6;
            model.cardId = mem.EXT2;
            model.phone = mem.TELEPHONE;
            try
            {
                model.startYear = Convert.ToInt32(mem.TENNISSTARTYEAR);
            }
            catch (Exception e)
            {
                model.startYear = 1997;
            }
            
            model.hand = GetForHandCode(mem.FOREHAND);
            model.habit =GetBackHandCode(mem.BACKHAND);
            try
            {
                model.height = Convert.ToInt32(mem.HEIGHT);
            }
            catch (Exception)
            {
                model.height = 180;
            }
            try
            {
                model.weight = Convert.ToInt32(mem.WEIGHT);
            }
            catch (Exception)
            {
                model.weight = 60;
            }
            

            //获取我的排名
            Model_Me_TechRank rank = Biz_Me_TechRank.instance.GetTechRank(_userId);
            model.Rank = rank.Rank;
            try
            {
                model.TechRank = Convert.ToDecimal(rank.TechRank);
            }
            catch (Exception)
            {
                model.TechRank = 4;
            }

            try
            {
                model.SelfTechRank = Convert.ToDecimal(rank.SelfTechRank);
            }
            catch (Exception)
            {
                model.SelfTechRank = 3;
            }

            try
            {
                model.OtherTechRank = Convert.ToDecimal(rank.OtherTechRank);
            }
            catch (Exception)
            {
                model.OtherTechRank = 0;
            }
            

            //获取我的地址
            List<Model_Me_Address> addlist = Biz_Me_Address.intance.GetMyAddress(_userId);
            List<Fe_Model_updateMySettings_address> feAddlist = new List<Fe_Model_updateMySettings_address>();
            foreach (Model_Me_Address add in addlist)
            {
                Fe_Model_updateMySettings_address feadd = new Fe_Model_updateMySettings_address();
                feadd.name = add.name;
                feadd.phone = add.phone;
                feadd.district = add.district;
                feadd.detailAddress = add.detailAddress;
                feadd.post = add.post;
                feAddlist.Add(feadd);
            }
            model.address = feAddlist;
            return model;
        }

        /// <summary>
        /// 读取个人信息
        /// </summary>
        /// <param name="_userId"></param>
        /// <returns></returns>
        public Model_Fe_UserInfo Get_UserInfo(string _userId)
        {
            Model_Fe_UserInfo model = new Model_Fe_UserInfo();
            WeMemberModel mem = WeMemberDll.instance.GetModel(_userId);
            model.id = _userId;
            model.name = mem.USERNAME;
            model.username = mem.NAME;
            model.password = mem.PASSWORD;
            model.phone = mem.TELEPHONE;
            if (mem.GENDER == "男"||mem.GENDER=="male")
            {
                model.gender = "male";
            }
            else
            {
                model.gender = "female";
            }
            model.cardId = mem.EXT2;
            return model;
        }
    }
}
