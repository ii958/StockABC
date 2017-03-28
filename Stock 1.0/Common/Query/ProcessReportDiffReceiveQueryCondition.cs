using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// ProcessReportDiffReceiveQueryCondition 的摘要说明。
	/// </summary>
	public class ProcessReportDiffReceiveQueryCondition:QueryCondition
	{
		/// <summary>
		/// 进度报告开始时间
		/// </summary>
		public string YearMonthFrom
		{
			get { return this.GetCondition("YearMonthFrom",""); }
			set { this.SetCondition("YearMonthFrom",value); } 
		}	
	
		/// <summary>
		/// 进度报告结束时间
		/// </summary>
		public string YearMonthTo
		{
			get { return this.GetCondition("YearMonthTo","");}
			set { this.SetCondition("YearMonthTo",value);}
		}
		/// <summary>
		/// 项目所属公司
		/// </summary>
		public string CompanyID
		{
			get { return this.GetCondition("CompanyID","");}
			set { this.SetCondition("CompanyID",value);}
		}

		/// <summary>
		/// 项目所属SBU
		/// </summary>
		public string SBUID
		{
			get { return this.GetCondition("SBUID","");}
			set { this.SetCondition("SBUID",value);}
		} 
         
		/// <summary>
		/// 项目5位码
		/// </summary>
		public string ProjectCodeShort
		{
			get { return this.GetCondition("ProjectCodeShort","");}
			set { this.SetCondition("ProjectCodeShort",value);}
		}

		/// <summary>
		/// 报告类型
		/// </summary>
		public string ReportType
		{
			get { return this.GetCondition("ReportType","");}
			set { this.SetCondition("ReportType",value);}
		}

		/// <summary>
		/// 项目经理编号
		/// </summary>
		public string ProjectManagerCode
		{
			get { return this.GetCondition("ProjectManagerCode","");}
			set { this.SetCondition("ProjectManagerCode",value);}
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
		
		public string CompanyName
		{
			get { return this.GetCondition("CompanyName","");}
			set { this.SetCondition("CompanyName",value);}
		}
		
		public string ProcessOrder
		{
			get { return this.GetCondition("ProcessOrder","");}
			set { this.SetCondition("ProcessOrder",value);}	
		}

	}
}
