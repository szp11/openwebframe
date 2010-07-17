using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace MvcAppTest.Helper.corelevel.Exception
{
    [Serializable]
    public class CUserException_WuQi:CApplicationExceptionBase_WuQi
    {
        		/// <summary>
		/// 构造函数
		/// </summary>
		public CUserException_WuQi() : this("", "user exception", new System.ApplicationException("user exception")) {
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="message">异常消息</param>
		/// <param name="innerException">内部异常</param>
		public CUserException_WuQi(string message, System.Exception innerException) : this("", message, innerException) {
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="message">异常消息</param>
		public CUserException_WuQi(string message) : this("", message) {
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="errorNo">异常编号</param>
		/// <param name="message">异常消息</param>
        public CUserException_WuQi(string errorUser, string message)
            : this(errorUser, message, new System.ApplicationException(message))
        {
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="errorNo">异常编号</param>
		/// <param name="message">异常消息</param>
		/// <param name="innerException">内部异常</param>
        public CUserException_WuQi(string errorUser, string message, System.Exception innerException)
            : base(message, innerException)
        {
			this.errorUser = errorUser;
		}

		/// <summary>
		/// 异常编号
		/// </summary>
		protected string errorUser;

		/// <summary>
		/// 异常编号
		/// </summary>
		public string ErrorUser {
			get { return this.errorUser; }
		}


    }
}
