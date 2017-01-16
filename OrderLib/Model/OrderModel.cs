using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderLib
{
     [Serializable]
    public class OrderModel
    {
        public string ID { get; set; }
        public string SYSNO { get; set; }
        public string DESCRIPTION { get; set; }
        public string TYPE { get; set; }
        public string UPDATEDATE { get; set; }
        public string CUST_SYS { get; set; }
        public string CUSTNAME { get; set; }
        public string CUSTREALNAME { get; set; }
        public string CUSTIDCARD { get; set; }
        public string CUSTADDRESS { get; set; }
        public string CUSTTEL { get; set; }
        public string ORDERMONEY { get; set; }
        public string DISCOUNT { get; set; }
        public string TICKETMONEY { get; set; }
        public string TICKETSYS { get; set; }
        public string SHOULDPAY { get; set; }
        public string ACTUALPAY { get; set; }
        public string BALANCEPAY { get; set; }
        public string PAYWAY { get; set; }
        public string STATUS { get; set; }
        public string SUPPLY_SYS { get; set; }
        public string SUPPLY_NAME { get; set; }
        public string SUPPLY_CONTACTPERSON { get; set; }
        public string SUPPLY_TEL { get; set; }
        public string EXT1 { get; set; }
        public string EXT2 { get; set; }
        public string EXT3 { get; set; }
        public string EXT4 { get; set; }
        public string EXT5 { get; set; }
         //附加字段
        public string TOURAPPLYSYS { get; set; }
    }
}
