using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace AISRS.Common.Function
{
	/// <summary>
	/// WebFunction ��ժҪ˵����
	/// </summary>
	public class WebFunction
	{
		/// <summary>
		/// ʹ��������ʽ�滻�ַ����е��ض���
		/// George 2003-6-18
		/// </summary>
		/// <param name="patrn">������ʽ</param>
		/// <param name="strng">ԭʼ�ַ���</param>
		/// <param name="rplptn">Ҫ�滻�ɵ���ʽ��������ʽ��</param>
		/// <returns>ת����ɵ��ַ���</returns>
		protected static string RegExpReplace(string patrn,string strng,string rplptn)
		{
			Regex regEx = new Regex(patrn, RegexOptions.ECMAScript & RegexOptions.IgnoreCase);

			return regEx.Replace(strng, rplptn);
		}

		/// <summary>
		/// ���ַ����е�email��ַ��ȡ����������HTML��mailto���
		/// George 2003-6-18
		/// 
		/// �ҵ�����****@****.***���ַ������<a href="mailto:****@****.***">****@****.***</a>�ĸ�ʽ
		/// </summary>
		/// <param name="strContent">Ҫ��ȡemail���ַ���</param>
		/// <returns>ת����ɵ��ַ���</returns>
		public static string ParseEmail(string strContent)
		{
			string strPattern = @"(([a-zA-Z0-9_\-\.]+)@((([a-zA-Z0-9\-]+\.)+)([a-zA-Z]{2,4})))";

			string strReplacePattern = "<A HREF=\"mailto:$1\">$1</A>";

			return RegExpReplace(strPattern, strContent, strReplacePattern);
		}

		/// <summary>
		/// ���ַ����е�������ȡ����������HTML�ĳ����ӱ��
		/// George 2003-6-18
		/// 
		/// �ҵ��ַ����еĳ����ӱ��<a href="[������]">[������]</a>�ĸ�ʽ
		/// </summary>
		/// <param name="strContent">Ҫ��ȡ�����ӵ��ַ���</param>
		/// <returns>ת����ɵ��ַ���</returns>
		public static string ParseURL(string strContent)
		{
			string strPattern = @"((ftp|http|https):\/\/((([a-zA-Z0-9/_\-]+\.)+)([a-zA-Z]{2,4}))[a-zA-Z0-9/_\-?=\%&;+\.#]*)";

			string strReplacePattern = "<A HREF=\"$1\">$1</A>";

			return RegExpReplace(strPattern, strContent, strReplacePattern);
		}

		/// <summary>
		/// ���ַ����еĸ�ʽ���HTML�ı��
		/// George 2003-6-18
		/// </summary>
		/// <param name="TheString">��Ҫת�����ַ���</param>
		/// <returns>��HTML��Ǹ�ʽ���ַ���</returns>
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
