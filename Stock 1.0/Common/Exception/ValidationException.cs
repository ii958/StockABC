using System;
using AISRS.Common.Framework;

namespace AISRS.Common.Exception
{
	/// <summary>
	/// 
	/// ���������Ч�Լ����쳣
	/// 
	/// ʹ��������
	/// ����WebUI���BussinessRule�㣬
	/// �ں����Ŀ�ʼ����Ҫ�ڼ���ǰ�Բ������ĸ�������������Ч�Լ��飬������ݲ��Ϸ������׳����쳣��
	/// ��������ķ�Χ����:
	/// 1.�������Ĵ������
	/// 2.�������������ķ���ֵ
	/// 3.�õ����������������(�������Ա����)
	/// 4.ҳ���Request�����Ĳ���
	/// 5.�����ڼ�����Ҫ�õ�������
	/// 
	/// ���磺
	/// Rule���ڸ������ݿ�ʱ���ִ����ĳ���ֶε�ֵ�����ֶεĹ涨���ȣ�
	/// UI���ڵ��ú�̨�ķ����õ����ݣ����ֵõ���������Ч��
	/// 
	/// ����˵���μ�CommonExeption
	/// </summary>
	public class ValidationException : CommonException
	{
		/// <summary>
		/// ȱʡ��FriendlyMessage
		/// </summary>
		public static string Default = Configuration.GetConfiguration("Exception.ValidationException.Default");

		/// <summary>
		/// ���û�д����κβ�����ʹ��Default��ΪFriendlyMessage
		/// </summary>
		public ValidationException():base(Default,string.Empty){}

		/// <summary>
		/// ʹ��Default��ΪFriendlyMessage
		/// </summary>
		/// <param name="message">��Ҫ��¼����־�е�������Ϣ</param>
		public ValidationException(string message):base(Default,message){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="friendlyMessage">��ʾ���û������Ѻ���Ϣ</param>
		/// <param name="message">��Ҫ��¼����־�е�������Ϣ</param>
		public ValidationException(string friendlyMessage,string message):base(friendlyMessage,message){}
	}
}
