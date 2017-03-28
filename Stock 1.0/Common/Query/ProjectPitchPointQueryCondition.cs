using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// ProjectPitchPointQueryCondition ��ժҪ˵����
	/// </summary>
	public class ProjectPitchPointQueryCondition: QueryCondition
	{				
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
		/// ��Ŀ8λ����
		/// </summary>
		public string ProjectCode
		{
			get { return this.GetCondition("ProjectCode","");}
			set { this.SetCondition("ProjectCode",value);}
		}

		/// <summary>
		/// ��Ŀ����
		/// </summary>
		public string ProjectName
		{
			get { return this.GetCondition("ProjectName","");}
			set { this.SetCondition("ProjectName",value);}
		}
		
		/// <summary>
		/// �ڵ�
		/// </summary>
		public string PitchPoint
		{
			get { return this.GetCondition("projectManagerNo","");}
			set { this.SetCondition("projectManagerNo",value);}
		}		
	}
}
