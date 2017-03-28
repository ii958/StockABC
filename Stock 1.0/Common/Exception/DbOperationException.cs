using System;
using AISRS.Common.Framework;

namespace AISRS.Common.Exception
{

	/// <summary>
	/// 数据库中操作异常
	/// 
	/// 使用条件：
	/// 用在DataAccess层（通常只用于base类和Transation类中）
	/// 在执行数据库操作时如果出现问题，可抛出此异常
	/// 通常可以不抛，因为.net会自动抛SqlException
	/// 
	/// 其他说明参见CommonExeption
	/// </summary>	
	public class DbOperationException : CommonException
	{
		/// <summary>
		/// 缺省的FriendlyMessage
		/// </summary>
		public static string Default = Configuration.GetConfiguration("Exception.DbOperationException.Default");

		/// <summary>
		/// 如果没有传入任何参数，使用Default作为FriendlyMessage
		/// </summary>
		public DbOperationException():base(Default,string.Empty){}

		/// <summary>
		/// 使用Default作为FriendlyMessage
		/// </summary>
		/// <param name="message">需要记录在日志中的其他信息</param>
		public DbOperationException(string message):base(Default,message){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="friendlyMessage">显示给用户看的友好信息</param>
		/// <param name="message">需要记录在日志中的其他信息</param>
		public DbOperationException(string friendlyMessage,string message):base(friendlyMessage,message){}

	}	
}
