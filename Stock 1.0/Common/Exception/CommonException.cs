using System;
using AISRS.Common.Framework;

namespace AISRS.Common.Exception
{
	/// <summary>
	/// 通用的自定义异常，作为其他自定义异常的基类
	/// 
	/// 使用条件：
	/// 不满足其他自定义异常的条件时，抛此异常
	/// 
	/// 
	/// 关于异常的捕获：
	/// 
	/// 如果页面不捕获异常，PageBase会自动捕获，转到ErrorPage页，并记录日志。
	/// 在需要捕获异常的地方（通常是WebUI层），如果不想影响用户使用，可以使用try catch捕获，在页面中进行提示。
	/// 但捕获的时候需要指定自定义异常的种类（通常捕获CommonException即可），千万不要捕获Exception基类，否则
	/// 在程序中出现Bug的时候不会记录日志，无法进行诊断。
	/// 在页面中捕获异常的时候，需要判断是否要记日志，如果需要记日志，可直接调用ExceptionHandler.WriteLog或
	/// ExceptionHandler.WriteLogWithPageInformation。
	/// 
	/// 
	/// 关于配置预定义的FriendlyMessage的说明：
	/// 
	/// 对于一些用户可能需要修改的FriendlyMessage，可以放在web.config中定义
	/// 然后在自定义异常的基类中声明静态变量，取Config中相应的值。
	/// 在编程时，传入FriendlyMessage时，使用定义的静态变量作为FriendlyMessage值。
	/// 
	/// 
	/// 如何在.net自带的Exception上附加信息：
	/// 
	/// 使用Exception.innerException，具体方法是构造一个新的Exception，将原来的Exeption包进来。
	/// 例子代码如下：
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
	/// 抛出的新Exception，在记日志的时候会自动将附加的信息和原有的信息合并在一起记录到日志中
	/// 
	/// </summary>
	public class CommonException : ApplicationException
	{
		/// <summary>
		/// 缺省的FriendlyMessage
		/// </summary>
		public static string CommonDefaultFriendlyMessage = Configuration.GetConfiguration("Exception.CommonException.Default");

		/// <summary>
		/// 显示给用户看的友好信息
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
		/// 如果没有传入参数，使用Default值作为FriendlyMessage
		/// </summary>
		public CommonException():this(CommonDefaultFriendlyMessage,string.Empty){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message">需要记录在日志中的其他信息</param>
		public CommonException(string message):this(CommonDefaultFriendlyMessage,message){}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="friendlyMessage">显示给用户看的友好信息</param>
		/// <param name="message">需要记录在日志中的其他信息</param>
		public CommonException(string friendlyMessage,string message):base(message)
		{
			this._friendlyMessage = friendlyMessage;
		}

	}

}
