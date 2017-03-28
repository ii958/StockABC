using System;
using System.Text;

using AISRS.Common.Exception;
using AISRS.Common.Framework;

namespace AISRS.Common.Exception
{
	/// <summary>
	/// �����װ�˶��쳣��Ĵ���
	/// </summary>
	public class ExceptionHandler
	{

		public static string DEFAULT_FRENDLY_MESSAGE = "ϵͳ���ִ����������Ա��ϵ��";
		//public static string DEFAULT_FRENDLY_MESSAGE = "Undefined error found, please contact administrator.";

		/// <summary>
		/// ���쳣��¼����־��
		/// </summary>
		/// <param name="exception">������쳣</param>
		/// <param name="catchInformation">�����쳣ʱ��¼����Ϣ</param>
		public static void WriteLog(
			System.Exception e,
			string catchInformation)
		{

			//���ɴ�����Ϣ
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
		/// ��ҳ�沶���쳣ʱ��¼��־����־�н�ҳ�����Ϣ��ΪCatchInformation��
		/// </summary>
		/// <param name="exception">������쳣</param>
		/// <param name="page">�����쳣��ҳ��</param>
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
		/// ��ȡ������ʾ���û������Ѻô�����Ϣ�����ڷ��Զ���Ĵ�����ʾȱʡ���Ѻô�����Ϣ
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
