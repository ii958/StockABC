using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// ProcessHistoryQueryCondition ��ժҪ˵����
	/// </summary>
	public class ProcessHistoryQueryCondition : QueryCondition
	{
		/// <summary>
		/// ���ȱ��濪ʼʱ��
		/// </summary>
		public string YearMonthFrom
		{
			get { return this.GetCondition("YearMonthFrom",""); }
			set { this.SetCondition("YearMonthFrom",value); } 
		}	
	
		/// <summary>
		/// ���ȱ������ʱ��
		/// </summary>
		public string YearMonthTo
		{
			get { return this.GetCondition("YearMonthTo","");}
			set { this.SetCondition("YearMonthTo",value);}
		}
		/// <summary>
		/// ��Ŀ������˾
		/// </summary>
		public string CompanyID
		{
			get { return this.GetCondition("CompanyID","");}
			set { this.SetCondition("CompanyID",value);}
		}

		/// <summary>
		/// ��Ŀ����SBU
		/// </summary>
		public string SBUID
		{
			get { return this.GetCondition("SBUID","");}
			set { this.SetCondition("SBUID",value);}
		} 
         
		/// <summary>
		/// ��Ŀ5λ��
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
		/// ��Ŀ��ʼʱ���
		/// </summary>
		public DateTime ProjectBeginDateFrom
		{
			get { return this.GetCondition("ProjectBeginDateFrom",DateTime.Now.AddYears(-50));}
			set { this.SetCondition("ProjectBeginDateFrom",value);}
		}


		/// <summary>
		/// ��Ŀ��ʼʱ����
		/// </summary>
		public DateTime ProjectBeginDateTo
		{
			get { return this.GetCondition("ProjectBeginDateTo",DateTime.Now.AddYears(50));}
			set { this.SetCondition("ProjectBeginDateTo",value);}
		}

		/// <summary>
		/// ��Ŀ������
		/// </summary>
		public string ProjectManagerCode
		{
			get { return this.GetCondition("ProjectManagerCode","");}
			set { this.SetCondition("ProjectManagerCode",value);}
		}

		/// <summary>
		/// ��Ŀ��������
		/// </summary>
		public string ProjectManagerName
		{
			get { return this.GetCondition("ProjectManagerName","");}
			set { this.SetCondition("ProjectManagerName",value);}
		}
		/// <summary>
		/// ��ʾ��ʽ
		/// </summary>
		public int ShowMethod
		{
			get { return this.GetCondition("ShowMethod",0); }
			set { this.SetCondition("ShowMethod",value); }
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
