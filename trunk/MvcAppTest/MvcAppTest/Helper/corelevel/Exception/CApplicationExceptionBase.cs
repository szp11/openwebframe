using System;
using System.Data;
using System.Configuration;
using System.Linq;

namespace MvcAppTest.Helper.corelevel.Exception
{
    /// <summary>
    /// 应用程序异常基类；不赞成使用应用程序异常
    /// </summary>
    [Serializable]
    public class CApplicationExceptionBase_WuQi : System.ApplicationException
    {
        		/// <summary>
		/// 构造函数
		/// </summary>
		public CApplicationExceptionBase_WuQi():base() {
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="message">异常消息</param>
		/// <param name="innerException">内部异常</param>
		public CApplicationExceptionBase_WuQi(string message, System.Exception innerException) : base(message, innerException) {
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="message">异常消息</param>
		public CApplicationExceptionBase_WuQi(string message) : base(message) {
		}


		/// <summary>
		/// 查找原始的异常
		/// </summary>
		/// <param name="e">异常</param>
		/// <returns>原始的异常</returns>
        public static System.Exception FindSourceException(System.Exception e)
        {
            System.Exception e1 = e;
			while(e1 != null) {
				e = e1;
				e1 = e1.InnerException;
			}
			return e;
		}

		/// <summary>
		/// 从异常树种查找指定类型的异常
		/// </summary>
		/// <param name="e">异常</param>
		/// <param name="expectedExceptionType">期待的异常类型</param>
		/// <returns>所要求的异常，如果找不到，返回null</returns>
        public static System.Exception FindSourceException(System.Exception e, Type expectedExceptionType)
        {
			while(e != null) {
				if(e.GetType() == expectedExceptionType) {
					return e;
				}
				e = e.InnerException;
			}
			return null;
		}

    }
}
