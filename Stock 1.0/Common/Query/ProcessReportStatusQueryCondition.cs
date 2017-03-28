using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// ProcessReportStatusQueryCondition 的摘要说明。
	/// </summary>
	public class ProcessReportStatusQueryCondition: QueryCondition
	{
		/// <summary>
		/// 查询时间:年月
		/// </summary>
		public string YearMonth
		{
			get { return this.GetCondition("YearMonth",""); }
			set { this.SetCondition("YearMonth",value); } 
		}
		
		/// <summary>
		/// 公司ID
		/// </summary>
		public string CompanyID
		{
			get { return this.GetCondition("CompanyID","");}
			set { this.SetCondition("CompanyID",value);}
		}

		/// <summary>
		/// SBU_ID	
		/// </summary>
		public string SBUID
		{
			get { return this.GetCondition("SBUID","");}
			set { this.SetCondition("SBUID",value);}
		}

		/// <summary>
		/// 项目5位编码
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
		public string ProjectManagerNo
		{
			get { return this.GetCondition("projectManagerNo","");}
			set { this.SetCondition("projectManagerNo",value);}
		}

		/// <summary>
		/// 项目经理姓名
		/// </summary>
		public string ProjectManagerName
		{
			get { return this.GetCondition("projectManagerName","");}
			set { this.SetCondition("projectManagerName",value);}
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
