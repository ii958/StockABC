using System;

namespace AISRS.Common.Framework
{
	/// <summary>
	/// User ��ժҪ˵��
	/// ����Ͳ����Ự�е��û���������Ϣ��Ȩ����Ϣ
	/// </summary>
	public class User : IDisposable
	{		
		#region ˽�б���
		private Decimal _userID;        //PersonID
		private string _employeeNumber; //EmployeeNumber
		private string _username;		//NT Account
		private string _fullName;		//��������
		private string _emailAddress;   //�ʼ���ַ
		private string _personRegion;	//�û������������������豸����Ա��Ϊ�մ�
		private string _personRegionID ; //�û����������ID

		private System.Collections.Hashtable _permissionTable;
		#endregion

		#region ����
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

		#region ������������
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
		/// ���һ��Ȩ��
		/// </summary>
		/// <param name="permission">Ҫ��ӵ�Ȩ��</param>
		public void AddPermission(string permission)
		{
			if(permission == null || permission == string.Empty)
				return;
			
			_permissionTable.Add(permission, permission);
		}

		/// <summary>
		/// �Ƴ�һ��Ȩ��
		/// </summary>
		/// <param name="permission"></param>
		public void RemovePermission(string permission)
		{
			this._permissionTable.Remove(permission);
		}

		/// <summary>
		/// �ж��û��Ƿ��и�����Ȩ��
		/// </summary>
		/// <param name="permission">Ҫ�жϵ�Ȩ��</param>
		public bool IsHavePermission(string permission)
		{
			if(this._permissionTable.Contains(permission))
				return true;
			else
				return false;
		}

		/// <summary>
		/// �ж��û��Ƿ��и�����Ȩ�����е�����һ��Ȩ��
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
		/// �ͷ���Դ
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
