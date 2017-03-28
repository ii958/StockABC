using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// PercentHistoryQueryCondition 的摘要说明。
	/// </summary>
	public class PercentHistoryQueryCondition : QueryCondition
	{
		public string YearMonthFrom
		{
			get { return this.GetCondition("YearMonthFrom",""); }
			set { this.SetCondition("YearMonthFrom",value); } 
		}	
	
		public string YearMonthTo
		{
			get { return this.GetCondition("YearMonthTo","");}
			set { this.SetCondition("YearMonthTo",value);}
		}
		
		public string CompanyID
		{
			get { return this.GetCondition("CompanyID","");}
			set { this.SetCondition("CompanyID",value);}
		}

		public string SBUID
		{
			get { return this.GetCondition("SBUID","");}
			set { this.SetCondition("SBUID",value);}
		}

		public string ProjectCodeShort
		{
			get { return this.GetCondition("ProjectCodeShort","");}
			set { this.SetCondition("ProjectCodeShort",value);}
		}

		public string ProjectManagerNumber
		{
			get { return this.GetCondition("ProjectManagerNumber","");}
			set { this.SetCondition("ProjectManagerNumber",value);}
		}

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
		public string CompanyName
		{
			get { return this.GetCondition("CompanyName","");}
			set { this.SetCondition("CompanyName",value);}
		}
	}
}
