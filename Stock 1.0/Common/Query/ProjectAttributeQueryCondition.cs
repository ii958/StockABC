using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// ProjectAttributeQueryCondition 的摘要说明。
	/// </summary>
	public class ProjectAttributeQueryCondition :QueryCondition
	{
		public string ProjectCode // 8位码
		{
			get { return this.GetCondition("ProjectCode",""); }
			set { this.SetCondition("ProjectCode",value); } 
		}	
		public string ProjectCodeShort // 5位码
		{
			get { return this.GetCondition("ProjectCodeShort",""); }
			set { this.SetCondition("ProjectCodeShort",value); } 
		}
	
		public string SBUName
		{
			get { return this.GetCondition("SBUName","");}
			set { this.SetCondition("SBUName",value);}
		}
		public string SBU_ID 
		{
			get { return this.GetCondition("SBU_ID","");}
			set { this.SetCondition("SBU_ID",value);}
		}
		public string ReportType 
		{
			get { return this.GetCondition("REPORT_TYPE","");}
			set { this.SetCondition("REPORT_TYPE",value);}
		}
		
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

		public string IsFont //是否签字
		{
			get { return this.GetCondition("ATTRIBUTE1","");}
			set { this.SetCondition("ATTRIBUTE1",value);}
		}
		public string IsImpress //是否盖章
		{
			get { return this.GetCondition("ATTRIBUTE2","");}
			set { this.SetCondition("ATTRIBUTE2",value);}
		}
		public string IsOriginal //是否为原件
		{
			get { return this.GetCondition("ATTRIBUTE3","");}
			set { this.SetCondition("ATTRIBUTE3",value);}
		}
		public string Year //年度
		{
			get { return this.GetCondition("Year","");}
			set { this.SetCondition("Year",value);}
		}
		public string CompanyName
		{
			get { return this.GetCondition("CompanyName","");}
			set { this.SetCondition("CompanyName",value);}
		}
		public string CompanyID 
		{
			get { return this.GetCondition("CompanyID","");}
			set { this.SetCondition("CompanyID",value);}
		}
	}
}
