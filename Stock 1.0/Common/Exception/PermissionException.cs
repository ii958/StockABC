using System;
using AISRS.Common.Framework;

namespace AISRS.Common.Exception
{
	/// <summary>
	/// 使用条件：
	/// 用在WebUI层和BussinessRules层
	/// 在检测到用户没有权限时，抛出此异常
	/// 
	/// 其他说明参见CommonExeption
	/// </summary>
	public class PermissionException : CommonException
	{
		/// <summary>
		/// 缺省的FriendlyMessage
		/// </summary>
		public static string Default = Configuration.GetConfiguration("Exception.PermissionException.Default");

		/// <summary>
		/// 不传入任何参数的话，使用Default作为FriendlyMessage
		/// </summary>
		public PermissionException():base(Default,string.Empty){}
		/// <summary>
		/// 使用Default值作为FriendlyMessage
		/// </summary>
		/// <param name="userName">用户名</param>
		/// <param name="authority">权限名</param>
		public PermissionException(string userName, string authority)
			:base(Default,GetExceptionMessage(string.Empty,userName,authority)){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message">需要记录在日志中的其他信息</param>
		/// <param name="userName">用户名</param>
		/// <param name="authority">权限名</param>
		public PermissionException(string message,string userName, string authority)
			:base(Default ,GetExceptionMessage(message,userName,authority) ){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="friendlyMessage">显示给用户的友好信息</param>
		/// <param name="message">需要记录在日志中的其他信息</param>
		/// <param name="userName">用户名</param>
		/// <param name="authority">权限名</param>
		public PermissionException(string friendlyMessage,string message,string userName, string authority)
			:base(friendlyMessage , GetExceptionMessage(message,userName,authority) ){}

		/// <summary>
		/// 生成Exception的Message
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="authority"></param>
		/// <returns></returns>
		private static string GetExceptionMessage(string message,string userName, string authority)
		{
			string exceptionMessage = "UserName: " + userName + "\r\n"
				+ "Authority: " + authority + "\r\n"
				+ message;
			return exceptionMessage;
		}

	}
}
