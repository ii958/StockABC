using System;
using AISRS.Common.Framework;

namespace AISRS.Common.Exception
{
	/// <summary>
	/// 
	/// 计算参数有效性检验异常
	/// 
	/// 使用条件：
	/// 用在WebUI层和BussinessRule层，
	/// 在函数的开始，需要在计算前对参与计算的各个参数进行有效性检验，如果数据不合法，会抛出此异常。
	/// 计算参数的范围包括:
	/// 1.本函数的传入参数
	/// 2.调用其他函数的返回值
	/// 3.用到的其他对象的属性(包括类成员属性)
	/// 4.页面的Request所带的参数
	/// 5.其他在计算中要用到的数据
	/// 
	/// 例如：
	/// Rule层在更新数据库时发现传入的某个字段的值超出字段的规定长度；
	/// UI层在调用后台的方法得到数据，发现得到的数据无效；
	/// 
	/// 其他说明参见CommonExeption
	/// </summary>
	public class ValidationException : CommonException
	{
		/// <summary>
		/// 缺省的FriendlyMessage
		/// </summary>
		public static string Default = Configuration.GetConfiguration("Exception.ValidationException.Default");

		/// <summary>
		/// 如果没有传入任何参数，使用Default作为FriendlyMessage
		/// </summary>
		public ValidationException():base(Default,string.Empty){}

		/// <summary>
		/// 使用Default作为FriendlyMessage
		/// </summary>
		/// <param name="message">需要记录在日志中的其他信息</param>
		public ValidationException(string message):base(Default,message){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="friendlyMessage">显示给用户看的友好信息</param>
		/// <param name="message">需要记录在日志中的其他信息</param>
		public ValidationException(string friendlyMessage,string message):base(friendlyMessage,message){}
	}
}
