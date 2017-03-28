using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// ProjectPitchPointQueryCondition 的摘要说明。
	/// </summary>
	public class ProjectPitchPointQueryCondition: QueryCondition
	{				
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
		/// 项目8位编码
		/// </summary>
		public string ProjectCode
		{
			get { return this.GetCondition("ProjectCode","");}
			set { this.SetCondition("ProjectCode",value);}
		}

		/// <summary>
		/// 项目名称
		/// </summary>
		public string ProjectName
		{
			get { return this.GetCondition("ProjectName","");}
			set { this.SetCondition("ProjectName",value);}
		}
		
		/// <summary>
		/// 节点
		/// </summary>
		public string PitchPoint
		{
			get { return this.GetCondition("projectManagerNo","");}
			set { this.SetCondition("projectManagerNo",value);}
		}		
	}
}
