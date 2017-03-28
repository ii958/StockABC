using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// AppUserQueryCondition 的摘要说明。
	/// </summary>
	public class AppUserQueryCondition : QueryCondition
	{
		public string Username
		{
			get { return this.GetCondition("Username",""); }
			set { this.SetCondition("Username",value); } 
		}	
	
		public string FullName
		{
			get { return this.GetCondition("FullName","");}
			set { this.SetCondition("FullName",value);}
		}

		public Guid RoleID
		{
			get { return this.GetCondition("RoleID",Guid.Empty);}
			set { this.SetCondition("RoleID",value);}
		}
	}
}
