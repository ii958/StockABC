using System;
using System.Text;


namespace AISRS.Common.Function
{
	/// <summary>
	/// DateFunction 的摘要说明。
	/// </summary>
	public class DateTimeFunction
	{
		/// <summary>
		/// 根据输入参数年月，返回当月最大日期数
		/// </summary>
		/// <param name="year">年份</param>
		/// <param name="month">月份</param>
		/// <returns>参数所确定月份的天数</returns>
		public static int GetMaxDay(int year,int month)
		{
			if(month < 1 || month > 12)
				return 31;
			switch(month)
			{
				case 1:
				case 3:
				case 5:
				case 7:
				case 8:
				case 10:
				case 12:
					return 31;
				case 4:
				case 6:
				case 9:
				case 11:
					return 30;
				case 2:
					if(DateTime.IsLeapYear(year))
						return 29;
					else
						return 28;
				default:
					return 31;
			}
		}

		/// <summary>
		/// 返回传入参数月份的字符串，格式为 “YYYY-MM”
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string GetMonthString(DateTime date)
		{
			return date.Year.ToString() + "-" + date.Month.ToString();
		}
				
		/// <summary>
		/// 返回表示当前时间的字符串，格式为 HH:MM:SS
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		private static string GetTimeString(DateTime time)
		{
			return time.Hour.ToString() + ":" + time.Minute.ToString().PadLeft(2,'0') + ":" + time.Second.ToString().PadLeft(2,'0');
		}

		/// <summary>
		/// 返回表示当前时间的字符串，格式为 HH:MM
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string GetShortTimeString(DateTime time)
		{
			return time.Hour.ToString() + ":" + time.Minute.ToString().PadLeft(2,'0');
		}
		
		/// <summary>
		/// 返回表示当前日期的字符串，格式为 YYYY-MM-DD
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string GetDateString(DateTime date)
		{
			return date.Year.ToString() + "-" + date.Month.ToString().PadLeft(2,'0') + "-" + date.Day.ToString().PadLeft(2,'0');
		}
	
		/// <summary>
		/// 返回表示当前时间的字符串，格式为 YYYY-MM-DD HH:MM:SS
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static string GetFullTimeString(DateTime time)
		{
			return GetDateString(time) + " " + GetTimeString(time);
		}

        public static string ConvertDate(string date)
        {
            string[] dateArr;
            if (date.IndexOf('-') == -1)
            {
                dateArr = date.Split('/');
            }
            else
            {
                dateArr = date.Split('-');
            }
            return date = dateArr[0] + (int.Parse(dateArr[1]) < 10 ? "0" + int.Parse(dateArr[1]).ToString() : dateArr[1]) + (int.Parse(dateArr[2]) < 10 ? "0" + int.Parse(dateArr[2]).ToString() : dateArr[2]);            
        }

        public static string ConvertDate1(string date)
        {
            string[] dateArr;
            if (date.IndexOf('-') == -1)
            {
                dateArr = date.Split('/');
            }
            else
            {
                dateArr = date.Split('-');
            }
            return date = dateArr[0] +"-"+ (int.Parse(dateArr[1]) < 10 ? "0" + int.Parse(dateArr[1]).ToString() : dateArr[1]) +"-"+ (int.Parse(dateArr[2]) < 10 ? "0" + int.Parse(dateArr[2]).ToString() : dateArr[2]);
        }
	}
}
