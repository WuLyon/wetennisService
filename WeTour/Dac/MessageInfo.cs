using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace WeTour
{
    [Serializable]
    public class MessageInfo
    {
        private bool _isSucceed;
        private string _message;
        private string _sysNo;
        private string _tag;

        public MessageInfo() { }
        public MessageInfo(bool isSucceed)
        {
            this._isSucceed = isSucceed;
        }
        public MessageInfo(bool isSucceed, string message)
        {
            this._isSucceed = isSucceed;
            this._message = message;
        }
        public MessageInfo(bool isSucceed, string message, string sysNo)
        {
            this._isSucceed = isSucceed;
            this._message = message;
            this._sysNo = sysNo;
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSucceed
        {
            get
            {
                return _isSucceed;
            }
            set
            {
                _isSucceed = value;
            }
        }


        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        /// <summary>
        /// 返回SysNo
        /// </summary>
        public string SysNo
        {
            get
            {
                return _sysNo;
            }
            set
            {
                _sysNo = value;
            }
        }

        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        private decimal _Money;

        public decimal Money
        {
            get { return _Money; }
            set { _Money = value; }
        }
    }
}

