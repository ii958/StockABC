using System;
using System.Collections;

namespace AISRS.Common.Framework
{
	/// <summary>
	/// AccessingRoleAttribute 的摘要说明。
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class PermissionAttribute : System.Attribute
	{
		private string[] _permissions;

		public PermissionAttribute(params string[] permissions)
		{
			this._permissions = permissions;
		}

		public PermissionAttribute(string permissions)
		{
			this._permissions = permissions.Split(';');
		}

		public string[] Permissions
		{
			get
			{
				return this._permissions;
			}
		}
	}
}
