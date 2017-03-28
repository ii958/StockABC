using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// PercentReportDiffAfterAdjustQueryCondition ��ժҪ˵����
	/// </summary>
	public class PercentReportDiffAfterAdjustQueryCondition: QueryCondition
	{
		/// <summary>
		/// ��ʼ�·�
		/// </summary>
		public string YearMonthFrom
		{
			get { return this.GetCondition("YearMonthFrom",""); }
			set { this.SetCondition("YearMonthFrom",value); } 
		}	
	
		/// <summary>
		/// �����·�
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

		//��Ŀ5λ����
		public string ProjectCodeShort
		{
			get { return this.GetCondition("ProjectCodeShort","");}
			set { this.SetCondition("ProjectCodeShort",value);}
		}

		/// <summary>
		/// ��Ŀ�������
		/// </summary>
		public string ProjectManagerNumber
		{
			get { return this.GetCondition("ProjectManagerNumber","");}
			set { this.SetCondition("ProjectManagerNumber",value);}
		}

		/// <summary>
		/// ��Ŀ��������
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
