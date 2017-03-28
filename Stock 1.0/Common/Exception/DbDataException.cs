using System;
using AISRS.Common.Framework;

namespace AISRS.Common.Exception
{
	/// <summary>
	/// 数据库中存在非法数据异常
	/// 
	/// 使用条件：
	/// 用在BussinessRule层
	/// 在通过DA层从数据库中查询出的数据，检验数据库中的数据是否符合规则，
	/// 如出现非法数据（即不满足系统的数据的约束条件），则抛出此异常
	/// 
	/// 例如：
	/// 应该只查出一条记录，却查出多条；
	/// 某个字段的格式不对；
	/// 应该有数据却没有查到。
	/// 
	/// 其他说明参见CommonExeption
	/// </summary>
	public class DbDataException : CommonException
	{
		/// <summary>
		/// 缺省的FriendlyMessage
		/// </summary>
		public static string Default = Configuration.GetConfiguration("Exception.DbDataException.Default");

		/// <summary>
		/// 如果没有传入任何参数，使用Default作为FriendlyMessage
		/// </summary>
		public DbDataException():base(Default,string.Empty){}

		/// <summary>
		/// 使用Default作为FriendlyMessage
		/// </summary>
		/// <param name="message">需要记录在日志中的其他信息</param>
		public DbDataException(string message):base(Default,message){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="friendlyMessage">显示给用户看的友好信息</param>
		/// <param name="message">需要记录在日志中的其他信息</param>
		public DbDataException(string friendlyMessage,string message):base(friendlyMessage,message){}
	}
}
