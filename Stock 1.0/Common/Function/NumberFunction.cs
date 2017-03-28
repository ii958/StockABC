using System;
using System.Text;

namespace AISRS.Common.Function
{
	/// <summary>
	/// NumberFunction ��ժҪ˵����
	/// </summary>
	public class NumberFunction
	{
		/// <summary>
		/// �ж��ַ����Ƿ��ʾ����
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static bool IsNumber(string s)
		{
			char firstChar = s[0];
			if(firstChar != '+' && firstChar != '-' && firstChar != '.' && (firstChar < '0' || firstChar > '9') )
				return false;

			for(int i=1;i<s.Length;i++)
			{
				if(s[i] != '.' && (s[i] < '0' || s[i] > '9'))
					return false;
			}

			return true;
		}
		/// <summary>
		/// �������뺯��
		/// </summary>
		/// <param name="value">Ҫ������������Ķ���</param>
		/// <param name="digit">������С��λ</param>
		/// <returns></returns>
		public static decimal Round(decimal value,int digit)
		{
			/*return Decimal.Parse(
				Convert.ToString(	Round(double.Parse(value.ToString()),
				digit)));*/
			return Decimal.Parse(
				Convert.ToString(IntRound(double.Parse(value.ToString()),
				digit)));
		}

		public static decimal IntRound(decimal d, int i)
		{
			return decimal.Parse(Convert.ToString(IntRound(double.Parse(d.ToString()),i)));
		}

		/// <summary>
		/// 20091210 chaidanlei ʹ��֮ǰ�ķ�������
		/// �������뺯����������С��λ
		/// </summary>
		/// <param name="d">Ҫ������������Ķ���</param>
		/// <param name="i">������С��λ</param>
		/// <returns></returns>
		public static double IntRound(double d, int i)
		{
			if(d >= 0)
			{
				d += 5 * Math.Pow(10, -(i + 1));
			}
			else
			{
				d += -5 * Math.Pow(10, -(i + 1));
			}
			string str = d.ToString();
			string[] strs = str.Split('.');
			int idot = str.IndexOf('.');
			if (idot >= 0)
			{
				string prestr = strs[0];
				string poststr = strs[1];
				if(poststr.Length > i)
				{
					poststr = str.Substring(idot + 1, i);
				}
				string strd = prestr + "." + poststr;
				d = Double.Parse(strd);
			}
			return d;
		}

		/// <summary>
		/// �������뺯��
		/// </summary>
		/// <param name="d">Ҫ������������Ķ���</param>
		/// <param name="i">������С��λ</param>
		/// <returns></returns>
		public static double Round(double d, int i)
		{
			if(d >=0)
			{
				d += 5 * Math.Pow(10, -(i + 1));
			}
			else
			{
				d += -5 * Math.Pow(10, -(i + 1));
			}
			string str = d.ToString();
			string[] strs = str.Split('.');
			int idot = str.IndexOf('.');
			string prestr = strs[0];
			string poststr = strs[1];
			if(poststr.Length > i)
			{
				poststr = str.Substring(idot + 1, i);
			}
			string strd = prestr + "." + poststr;
			d = Double.Parse(strd);
			return d;
		}

	}
}
