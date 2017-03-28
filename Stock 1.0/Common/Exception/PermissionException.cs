using System;
using AISRS.Common.Framework;

namespace AISRS.Common.Exception
{
	/// <summary>
	/// ʹ��������
	/// ����WebUI���BussinessRules��
	/// �ڼ�⵽�û�û��Ȩ��ʱ���׳����쳣
	/// 
	/// ����˵���μ�CommonExeption
	/// </summary>
	public class PermissionException : CommonException
	{
		/// <summary>
		/// ȱʡ��FriendlyMessage
		/// </summary>
		public static string Default = Configuration.GetConfiguration("Exception.PermissionException.Default");

		/// <summary>
		/// �������κβ����Ļ���ʹ��Default��ΪFriendlyMessage
		/// </summary>
		public PermissionException():base(Default,string.Empty){}
		/// <summary>
		/// ʹ��Defaultֵ��ΪFriendlyMessage
		/// </summary>
		/// <param name="userName">�û���</param>
		/// <param name="authority">Ȩ����</param>
		public PermissionException(string userName, string authority)
			:base(Default,GetExceptionMessage(string.Empty,userName,authority)){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message">��Ҫ��¼����־�е�������Ϣ</param>
		/// <param name="userName">�û���</param>
		/// <param name="authority">Ȩ����</param>
		public PermissionException(string message,string userName, string authority)
			:base(Default ,GetExceptionMessage(message,userName,authority) ){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="friendlyMessage">��ʾ���û����Ѻ���Ϣ</param>
		/// <param name="message">��Ҫ��¼����־�е�������Ϣ</param>
		/// <param name="userName">�û���</param>
		/// <param name="authority">Ȩ����</param>
		public PermissionException(string friendlyMessage,string message,string userName, string authority)
			:base(friendlyMessage , GetExceptionMessage(message,userName,authority) ){}

		/// <summary>
		/// ����Exception��Message
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
