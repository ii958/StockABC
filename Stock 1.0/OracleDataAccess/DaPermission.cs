using System;
using System.Data;
using System.Data.OracleClient;
using AISRS.Common.Data;

namespace AISRS.DataAccess
{
	/// <summary>
	/// DaPermission ��ժҪ˵����
	/// </summary>
	public class DaPermission :DataAccessBase
	{
		/// <summary>
		/// ȡ������Ȩ��
		/// </summary>
		/// <param name="dataTable">����Ȩ�����ݵ�PermissionData.PermissionDataTable����</param>
		public void LoadAllPermission(DataTable dataTable)
		{
			string sql = "SELECT permission_id as ID, InternalName, CategoryName, GroupName, Permission_Name as Name, Position, Description, IsValid FROM AIAPC_permission WHERE (IsValid = 1) ORDER BY CategoryName, GroupName, Position";
			this.AutoFill(dataTable, sql);
		}

		/// <summary>
		/// ȡ�������û���Ȩ��
		/// </summary>
		/// <param name="appUserID">�û�ID</param>
		/// <param name="dataTable">����Ȩ�����ݵ�PermissionData.PermissionDataTable����</param>
		public void LoadUserPermission(Guid appUserID,DataTable dataTable)
		{
			OracleParameter[] paras = new OracleParameter[]
				{this.MakeInParam("p_user_id",OracleType.Char,36,appUserID.ToString()),
				 this.MakeSelectCursorParam()
				};
			this.AutoFill(dataTable,"AIAPC_Get_User_Permission",paras);
		}

		/// <summary>
		/// �����û�IDȡ���û���ָ��ҳ���Ȩ��
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="userID"></param>
		/// <param name="internalPageName"></param>
		public void LoadUserPagePermission(DataTable dataTable,decimal userID,string internalPageName)
		{
			OracleParameter[] parameters = 
				{
					this.MakeInParam("p_personID",OracleType.VarChar,150,userID.ToString()),
					this.MakeInParam("p_pageInternalName",OracleType.VarChar,100,internalPageName),
					this.MakeSelectCursorParam()
				};
			this.AutoFill(dataTable,"AIAPC_GET_USER_PAGE_PERMISSION",parameters);
		}
		


		/// <summary>
		/// ȡ�����е�Ȩ��
		/// </summary>
		/// <param name="dataTable"></param>
		public void LoadPermission(
			DataTable dataTable)
		{
			string sql = "SELECT * FROM AIAPC_Permission WHERE (IsValid = 1) ORDER BY Category_Name DESC, Group_Name DESC,Name";

			this.AutoFill(dataTable, sql);
		}

		/// <summary>
		/// ȡ���û���Ҫ����Ȩ�޷�Χ��Ȩ������
		/// </summary>
		/// <param name="data"></param>
		/// <param name="userID"></param>
		public void LoadUserPermissionRangeData(DataTable data, decimal userID, decimal rangeType)
		{
			OracleParameter[] parameters = 
				{
					this.MakeInParam("P_PERSON_ID",OracleType.Number,10,userID),
					this.MakeInParam("P_RANGE_TYPE",OracleType.Number,1,rangeType),
					this.MakeSelectCursorParam()
				};
			this.AutoFill(data,"AIAPC_GET_USERPERMISSIONRANGE",parameters);
		}

		/// <summary>
		/// �����û�ID��Permission ID��ȡ�ø��û�����Company��SBU��Ȩ�޷�Χ����
		/// </summary>
		public void LoadUserRangeByCondition(decimal userID, string permissionID, DataTable dataTable)
		{
			string sql = "SELECT DISTINCT * FROM AIAPC_USER_COMPANY_SBU"
				+ " WHERE USER_ID = '" + userID.ToString().Trim() + "'"
				+ " AND PERMISSION_ID = '" + permissionID.Trim() + "'";
			this.AutoFill(dataTable,sql);
		}
	}
}
