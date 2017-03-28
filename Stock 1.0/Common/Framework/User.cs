using System;

namespace AISRS.Common.Framework
{
	/// <summary>
	/// User 的摘要说明
	/// 保存和操作会话中的用户的名称信息和权限信息
	/// </summary>
	public class User : IDisposable
	{		
		#region 私有变量
		private Decimal _userID;        //PersonID
		private string _employeeNumber; //EmployeeNumber
		private string _username;		//NT Account
		private string _fullName;		//中文名称
		private string _emailAddress;   //邮件地址
		private string _personRegion;	//用户管理的区域，如果不是设备管理员则为空串
		private string _personRegionID ; //用户管理区域的ID

		private System.Collections.Hashtable _permissionTable;
		#endregion

		#region 属性
		public Decimal UserID
		{
			get { return this._userID; } 
		}
		
		public string EmployeeNumber
		{
			get { return this._employeeNumber; }
		}

		public string Username
		{	
			get { return _username; }
		}
		
		public string FullName
		{
			get { return _fullName; }
		}

		public string EmailAddress
		{
			get { return _emailAddress; }
			set { _emailAddress = value;}
		}
		public string PersonRegion
		{
			get{ return _personRegion ;}
		}

		public string PersonRegionID
		{
			get
			{
				return _personRegionID;
			}
		}

		public System.Collections.Hashtable PermissionTable
		{
			get {return _permissionTable;}
		}

		
		#endregion

		#region 构造析构函数
		public User(Decimal userID, string employeeNumber,string username, string fullName)
		{
			this._userID = userID;
			this._employeeNumber = employeeNumber;
			this._username = username;
			this._fullName = fullName;
			this._permissionTable = new System.Collections.Hashtable();
		}

		public User(Decimal userID,string employeeNumber, string username, string fullName, string emailAddress)
		{
			this._userID = userID;
			this._employeeNumber = employeeNumber;
			this._username = username;
			this._fullName = fullName;
			this._emailAddress = emailAddress;
			this._permissionTable = new System.Collections.Hashtable();
		}


		public User(Decimal userID, string employeeNumber,string username, string fullName, string emailAddress,string personRegion,string personRegionID)
		{
			this._userID = userID;
			this._employeeNumber = employeeNumber;
			this._username = username;
			this._fullName = fullName;
			this._emailAddress = emailAddress;
			this._personRegion = personRegion;
			this._personRegionID = personRegionID;
			this._permissionTable = new System.Collections.Hashtable();
		}



		#endregion
		
		/// <summary>
		/// 添加一条权限
		/// </summary>
		/// <param name="permission">要添加的权限</param>
		public void AddPermission(string permission)
		{
			if(permission == null || permission == string.Empty)
				return;
			
			_permissionTable.Add(permission, permission);
		}

		/// <summary>
		/// 移除一条权限
		/// </summary>
		/// <param name="permission"></param>
		public void RemovePermission(string permission)
		{
			this._permissionTable.Remove(permission);
		}

		/// <summary>
		/// 判断用户是否有给出的权限
		/// </summary>
		/// <param name="permission">要判断的权限</param>
		public bool IsHavePermission(string permission)
		{
			if(this._permissionTable.Contains(permission))
				return true;
			else
				return false;
		}

		/// <summary>
		/// 判断用户是否有给出的权限组中的任意一个权限
		/// </summary>
		/// <param name="permissions"></param>
		/// <returns></returns>
		public bool IsHaveAnyoneOfPermissions(string[] permissions)
		{
			if(permissions == null || permissions.Length == 0)
				return false;

			foreach(string permission in permissions)
			{
				if(this._permissionTable.Contains(permission))
					return true;
			}

			return false;
		}

		/// <summary>
		/// 释放资源
		/// </summary>
		public void Dispose()
		{
			if(this._permissionTable != null)
			{
				this._permissionTable.Clear();				
			}
		}
	}
}
