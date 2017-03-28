using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// ProjectAppendInfoQueryCondition 的摘要说明。
	/// </summary>
	public class ProjectAppendInfoQueryCondition: QueryCondition
	{
		/// <summary>
		/// 项目5位编码
		/// </summary>
		public string ProjectCodeShort
		{
			get { return this.GetCondition("ProjectCodeShort",""); }
			set { this.SetCondition("ProjectCodeShort",value); } 
		}
		/// <summary>
		/// 项目附加信息类型代码;0:软件出库;1:风险转移;2:后续项目
		/// </summary>
		public int AppendInfoID
		{
			get { return this.GetCondition("AppendInfoID",0); }
			set { this.SetCondition("AppendInfoID",value); } 
		}
		
		/// <summary>
		/// 项目附加信息状态;0:是;1否
		/// </summary>
		public int AppendInfoStatus
		{
			get { return this.GetCondition("AppendInfoStatus",0);}
			set { this.SetCondition("AppendInfoStatus",value);}
		}

		/// <summary>
		/// 起始时间
		/// </summary>
		public string BeginTime
		{
			get { return this.GetCondition("BeginTime","");}
			set { this.SetCondition("BeginTime",value);}
		}

		/// <summary>
		/// 结束时间
		/// </summary>
		public string EndTime
		{
			get { return this.GetCondition("EndTime","");}
			set { this.SetCondition("EndTime",value);}
		}
	}
}
