using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// PercentReportDiffAfterAdjustQueryCondition 的摘要说明。
	/// </summary>
	public class PercentReportDiffAfterAdjustQueryCondition: QueryCondition
	{
		/// <summary>
		/// 起始月份
		/// </summary>
		public string YearMonthFrom
		{
			get { return this.GetCondition("YearMonthFrom",""); }
			set { this.SetCondition("YearMonthFrom",value); } 
		}	
	
		/// <summary>
		/// 结束月份
		/// </summary>
		public string YearMonthTo
		{
			get { return this.GetCondition("YearMonthTo","");}
			set { this.SetCondition("YearMonthTo",value);}
		}	
	
		/// <summary>
		/// CompanyID
		/// </summary>
		public string CompanyID
		{
			get { return this.GetCondition("CompanyID","");}
			set { this.SetCondition("CompanyID",value);}
		}

		/// <summary>
		/// SBUID
		/// </summary>
		public string SBUID
		{
			get { return this.GetCondition("SBUID","");}
			set { this.SetCondition("SBUID",value);}
		}

		//项目5位编码
		public string ProjectCodeShort
		{
			get { return this.GetCondition("ProjectCodeShort","");}
			set { this.SetCondition("ProjectCodeShort",value);}
		}

		/// <summary>
		/// 项目经理编码
		/// </summary>
		public string ProjectManagerNumber
		{
			get { return this.GetCondition("ProjectManagerNumber","");}
			set { this.SetCondition("ProjectManagerNumber",value);}
		}

		/// <summary>
		/// 项目经理姓名
		/// </summary>
		public string ProjectManagerName
		{
			get { return this.GetCondition("ProjectManagerName","");}
			set { this.SetCondition("ProjectManagerName",value);}
		}

		public string SBUName
		{
			get { return this.GetCondition("SBUName","");}
			set { this.SetCondition("SBUName",value);}
		}
		/// <summary>
		/// CompanyID
		/// </summary>
		public string CompanyName
		{
			get { return this.GetCondition("CompanyName","");}
			set { this.SetCondition("CompanyName",value);}
		}
	}
}
