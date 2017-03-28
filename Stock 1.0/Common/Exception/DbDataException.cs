using System;
using AISRS.Common.Framework;

namespace AISRS.Common.Exception
{
	/// <summary>
	/// ���ݿ��д��ڷǷ������쳣
	/// 
	/// ʹ��������
	/// ����BussinessRule��
	/// ��ͨ��DA������ݿ��в�ѯ�������ݣ��������ݿ��е������Ƿ���Ϲ���
	/// ����ַǷ����ݣ���������ϵͳ�����ݵ�Լ�������������׳����쳣
	/// 
	/// ���磺
	/// Ӧ��ֻ���һ����¼��ȴ���������
	/// ĳ���ֶεĸ�ʽ���ԣ�
	/// Ӧ��������ȴû�в鵽��
	/// 
	/// ����˵���μ�CommonExeption
	/// </summary>
	public class DbDataException : CommonException
	{
		/// <summary>
		/// ȱʡ��FriendlyMessage
		/// </summary>
		public static string Default = Configuration.GetConfiguration("Exception.DbDataException.Default");

		/// <summary>
		/// ���û�д����κβ�����ʹ��Default��ΪFriendlyMessage
		/// </summary>
		public DbDataException():base(Default,string.Empty){}

		/// <summary>
		/// ʹ��Default��ΪFriendlyMessage
		/// </summary>
		/// <param name="message">��Ҫ��¼����־�е�������Ϣ</param>
		public DbDataException(string message):base(Default,message){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="friendlyMessage">��ʾ���û������Ѻ���Ϣ</param>
		/// <param name="message">��Ҫ��¼����־�е�������Ϣ</param>
		public DbDataException(string friendlyMessage,string message):base(friendlyMessage,message){}
	}
}
