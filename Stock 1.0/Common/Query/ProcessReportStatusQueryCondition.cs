using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// ProcessReportStatusQueryCondition ��ժҪ˵����
	/// </summary>
	public class ProcessReportStatusQueryCondition: QueryCondition
	{
		/// <summary>
		/// ��ѯʱ��:����
		/// </summary>
		public string YearMonth
		{
			get { return this.GetCondition("YearMonth",""); }
			set { this.SetCondition("YearMonth",value); } 
		}
		
		/// <summary>
		/// ��˾ID
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
		/// ��Ŀ5λ����
		/// </summary>
		public string ProjectCodeShort
		{
			get { return this.GetCondition("ProjectCodeShort","");}
			set { this.SetCondition("ProjectCodeShort",value);}
		}

		/// <summary>
		/// ��������
		/// </summary>
		public string ReportType
		{
			get { return this.GetCondition("ReportType","");}
			set { this.SetCondition("ReportType",value);}
		}

		/// <summary>
		/// ��Ŀ������
		/// </summary>
		public string ProjectManagerNo
		{
			get { return this.GetCondition("projectManagerNo","");}
			set { this.SetCondition("projectManagerNo",value);}
		}

		/// <summary>
		/// ��Ŀ��������
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
