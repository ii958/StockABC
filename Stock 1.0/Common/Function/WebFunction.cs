using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace AISRS.Common.Function
{
	/// <summary>
	/// WebFunction 的摘要说明。
	/// </summary>
	public class WebFunction
	{
		/// <summary>
		/// 使用正则表达式替换字符串中的特定项
		/// George 2003-6-18
		/// </summary>
		/// <param name="patrn">正则表达式</param>
		/// <param name="strng">原始字符串</param>
		/// <param name="rplptn">要替换成的形式（正则表达式）</param>
		/// <returns>转换完成的字符串</returns>
		protected static string RegExpReplace(string patrn,string strng,string rplptn)
		{
			Regex regEx = new Regex(patrn, RegexOptions.ECMAScript & RegexOptions.IgnoreCase);

			return regEx.Replace(strng, rplptn);
		}

		/// <summary>
		/// 把字符串中的email地址提取出来，打上HTML的mailto标记
		/// George 2003-6-18
		/// 
		/// 找到形如****@****.***的字符串变成<a href="mailto:****@****.***">****@****.***</a>的格式
		/// </summary>
		/// <param name="strContent">要提取email的字符串</param>
		/// <returns>转换完成的字符串</returns>
		public static string ParseEmail(string strContent)
		{
			string strPattern = @"(([a-zA-Z0-9_\-\.]+)@((([a-zA-Z0-9\-]+\.)+)([a-zA-Z]{2,4})))";

			string strReplacePattern = "<A HREF=\"mailto:$1\">$1</A>";

			return RegExpReplace(strPattern, strContent, strReplacePattern);
		}

		/// <summary>
		/// 把字符串中的链接提取出来，打上HTML的超链接标记
		/// George 2003-6-18
		/// 
		/// 找到字符串中的超链接变成<a href="[超链接]">[超链接]</a>的格式
		/// </summary>
		/// <param name="strContent">要提取超链接的字符串</param>
		/// <returns>转换完成的字符串</returns>
		public static string ParseURL(string strContent)
		{
			string strPattern = @"((ftp|http|https):\/\/((([a-zA-Z0-9/_\-]+\.)+)([a-zA-Z]{2,4}))[a-zA-Z0-9/_\-?=\%&;+\.#]*)";

			string strReplacePattern = "<A HREF=\"$1\">$1</A>";

			return RegExpReplace(strPattern, strContent, strReplacePattern);
		}

		/// <summary>
		/// 把字符串中的格式变成HTML的标记
		/// George 2003-6-18
		/// </summary>
		/// <param name="TheString">需要转换的字符串</param>
		/// <returns>带HTML标记格式的字符串</returns>
		public static string StringToHTML(string TheString)
		{
			string strHtml = TheString;
			strHtml = ParseURL(strHtml);
			strHtml = ParseEmail(strHtml);
			strHtml = strHtml.Replace("\r\n","<br>");
			return strHtml; 
		}
	}
}
