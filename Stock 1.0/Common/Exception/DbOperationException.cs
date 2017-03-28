using System;
using AISRS.Common.Framework;

namespace AISRS.Common.Exception
{

	/// <summary>
	/// ���ݿ��в����쳣
	/// 
	/// ʹ��������
	/// ����DataAccess�㣨ͨ��ֻ����base���Transation���У�
	/// ��ִ�����ݿ����ʱ����������⣬���׳����쳣
	/// ͨ�����Բ��ף���Ϊ.net���Զ���SqlException
	/// 
	/// ����˵���μ�CommonExeption
	/// </summary>	
	public class DbOperationException : CommonException
	{
		/// <summary>
		/// ȱʡ��FriendlyMessage
		/// </summary>
		public static string Default = Configuration.GetConfiguration("Exception.DbOperationException.Default");

		/// <summary>
		/// ���û�д����κβ�����ʹ��Default��ΪFriendlyMessage
		/// </summary>
		public DbOperationException():base(Default,string.Empty){}

		/// <summary>
		/// ʹ��Default��ΪFriendlyMessage
		/// </summary>
		/// <param name="message">��Ҫ��¼����־�е�������Ϣ</param>
		public DbOperationException(string message):base(Default,message){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="friendlyMessage">��ʾ���û������Ѻ���Ϣ</param>
		/// <param name="message">��Ҫ��¼����־�е�������Ϣ</param>
		public DbOperationException(string friendlyMessage,string message):base(friendlyMessage,message){}

	}	
}
