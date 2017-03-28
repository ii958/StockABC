using System;
using System.Text;

using AISRS.Common.Exception;
using AISRS.Common.Framework;

namespace AISRS.Common.Exception
{
	/// <summary>
	/// 此类封装了对异常类的处理
	/// </summary>
	public class ExceptionHandler
	{

		public static string DEFAULT_FRENDLY_MESSAGE = "系统出现错误，请与管理员联系！";
		//public static string DEFAULT_FRENDLY_MESSAGE = "Undefined error found, please contact administrator.";

		/// <summary>
		/// 将异常记录到日志中
		/// </summary>
		/// <param name="exception">捕获的异常</param>
		/// <param name="catchInformation">捕获异常时记录的信息</param>
		public static void WriteLog(
			System.Exception e,
			string catchInformation)
		{

			//生成错误信息
			StringBuilder strBuilder = new StringBuilder();
//			strBuilder.Append("<LogItem>\r\n");

			strBuilder.Append("<Exception>" + e.GetType().ToString() + "</Exception>\r\n");

			strBuilder.Append("<FriendlyMessage>\r\n");
			strBuilder.Append( GetFriendlyMessage(e) + "\r\n");
			strBuilder.Append("</FriendlyMessage>\r\n");

			strBuilder.Append("<ThrowInformation>\r\n");
			strBuilder.Append( e.ToString() + "\r\n");
			strBuilder.Append("</ThrowInformation>\r\n");

			strBuilder.Append("<CatchInformation>\r\n");
			strBuilder.Append( catchInformation + "\r\n");
			strBuilder.Append("</CatchInformation>\r\n");

//			strBuilder.Append("<StackTrace>\r\n");
//			strBuilder.Append( e.StackTrace + "\r\n");
//			strBuilder.Append("</StackTrace>\r\n");

//			strBuilder.Append("</LogItem>\r\n");
			string errorXML = strBuilder.ToString();

			Log.WriteError(errorXML);

		}


		/// <summary>
		/// 在页面捕获异常时记录日志，日志中将页面的信息作为CatchInformation。
		/// </summary>
		/// <param name="exception">捕获的异常</param>
		/// <param name="page">捕获异常的页面</param>
		public static void WriteLogWithPageInformation(
			System.Exception e,
			System.Web.UI.Page page)
		{
			string catchInformation = "";
			catchInformation = "URL : " + page.Request.RawUrl + "\r\n" 
				+ "FORM :" + page.Request.Form ;
			WriteLog(e,catchInformation);
		}


		/// <summary>
		/// 获取用于显示给用户看的友好错误信息，对于非自定义的错误，显示缺省的友好错误信息
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public static string GetFriendlyMessage(System.Exception e)
		{
			if ( e is CommonException)
			{
				return ((CommonException)e).FriendMessage;
			}
			else
			{
				if (CommonException.CommonDefaultFriendlyMessage == "")
					return DEFAULT_FRENDLY_MESSAGE;
				else
					return CommonException.CommonDefaultFriendlyMessage;
			}
		}

	}
}
