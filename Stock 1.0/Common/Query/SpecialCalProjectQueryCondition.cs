using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// SpecialCalProjectQueryCondition 的摘要说明。
	/// </summary>
	public class SpecialCalProjectQueryCondition:QueryCondition
	{
		/// <summary>
		/// 有效期限--开始时间
		/// </summary>
		public string MonthFrom
		{
			get{ return this.GetCondition("PROCESS_MONTH_START", ""); }
			set{ this.SetCondition("PROCESS_MONTH_START", value); }
		}

		/// <summary>
		/// 有效期限--结束时间
		/// </summary> 
		public string MonthTo
		{
			get{ return this.GetCondition("PROCESS_MONTH_END", ""); }
			set{ this.SetCondition("PROCESS_MONTH_END", value); }
		}

		/// <summary>
		/// 类型
		/// </summary>
		public string SpecialType
		{
			get{ return this.GetCondition("SPECIAL_TYPE", ""); }
			set{ this.SetCondition("SPECIAL_TYPE", value); }
		}

		/// <summary>
		/// 项目编码5位
		/// </summary>
		public string ProjectCode
		{
			get{return this.GetCondition("PROJECT_CODE_SHORT", "");}
			set{this.SetCondition("PROJECT_CODE_SHORT", value);}
		}

		public string Company
		{
			get { return this.GetCondition("Company","");}
			set { this.SetCondition("Company",value);}
		}

		public string SBU
		{
			get { return this.GetCondition("SBU","");}
			set { this.SetCondition("SBU",value);}
		}

	}
}
