using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// ProjectCostQueryCondition 的摘要说明。
	/// </summary>
	public class ProjectCostQueryCondition: QueryCondition
	{
		public string ProjectCode // 项目编码（8位码）
		{
			get { return this.GetCondition("ProjectCode",""); }
			set { this.SetCondition("ProjectCode",value); } 
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

		public string ProjectManagerName
		{
			get { return this.GetCondition("ProjectManagerName","");}
			set { this.SetCondition("ProjectManagerName",value);}
		}
	}
}
