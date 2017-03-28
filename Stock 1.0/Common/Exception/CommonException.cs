using System;
using AISRS.Common.Framework;

namespace AISRS.Common.Exception
{
	/// <summary>
	/// ͨ�õ��Զ����쳣����Ϊ�����Զ����쳣�Ļ���
	/// 
	/// ʹ��������
	/// �����������Զ����쳣������ʱ���״��쳣
	/// 
	/// 
	/// �����쳣�Ĳ���
	/// 
	/// ���ҳ�治�����쳣��PageBase���Զ�����ת��ErrorPageҳ������¼��־��
	/// ����Ҫ�����쳣�ĵط���ͨ����WebUI�㣩���������Ӱ���û�ʹ�ã�����ʹ��try catch������ҳ���н�����ʾ��
	/// �������ʱ����Ҫָ���Զ����쳣�����ࣨͨ������CommonException���ɣ���ǧ��Ҫ����Exception���࣬����
	/// �ڳ����г���Bug��ʱ�򲻻��¼��־���޷�������ϡ�
	/// ��ҳ���в����쳣��ʱ����Ҫ�ж��Ƿ�Ҫ����־�������Ҫ����־����ֱ�ӵ���ExceptionHandler.WriteLog��
	/// ExceptionHandler.WriteLogWithPageInformation��
	/// 
	/// 
	/// ��������Ԥ�����FriendlyMessage��˵����
	/// 
	/// ����һЩ�û�������Ҫ�޸ĵ�FriendlyMessage�����Է���web.config�ж���
	/// Ȼ�����Զ����쳣�Ļ�����������̬������ȡConfig����Ӧ��ֵ��
	/// �ڱ��ʱ������FriendlyMessageʱ��ʹ�ö���ľ�̬������ΪFriendlyMessageֵ��
	/// 
	/// 
	/// �����.net�Դ���Exception�ϸ�����Ϣ��
	/// 
	/// ʹ��Exception.innerException�����巽���ǹ���һ���µ�Exception����ԭ����Exeption��������
	/// ���Ӵ������£�
	///	try
	///	{
	///		this.ExecuteNonQuerySql (sql);
	///	}
	///	catch (Exception ex)
	///	{
	///		Exception exWithInformation = new Exception(sql,ex);
	///		throw exWithInformation;
	///	}
	/// 
	/// �׳�����Exception���ڼ���־��ʱ����Զ������ӵ���Ϣ��ԭ�е���Ϣ�ϲ���һ���¼����־��
	/// 
	/// </summary>
	public class CommonException : ApplicationException
	{
		/// <summary>
		/// ȱʡ��FriendlyMessage
		/// </summary>
		public static string CommonDefaultFriendlyMessage = Configuration.GetConfiguration("Exception.CommonException.Default");

		/// <summary>
		/// ��ʾ���û������Ѻ���Ϣ
		/// </summary>
		protected string _friendlyMessage;
		public string FriendMessage
		{
			get
			{
				return this._friendlyMessage;
			}
		}

		/// <summary>
		/// ���û�д��������ʹ��Defaultֵ��ΪFriendlyMessage
		/// </summary>
		public CommonException():this(CommonDefaultFriendlyMessage,string.Empty){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message">��Ҫ��¼����־�е�������Ϣ</param>
		public CommonException(string message):this(CommonDefaultFriendlyMessage,message){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="friendlyMessage">��ʾ���û������Ѻ���Ϣ</param>
		/// <param name="message">��Ҫ��¼����־�е�������Ϣ</param>
		public CommonException(string friendlyMessage,string message):base(message)
		{
			this._friendlyMessage = friendlyMessage;
		}

	}

}
