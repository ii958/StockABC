using System;
using System.Data;
using System.Data.SqlClient;

using AISRS.Common.Data;
using AISRS.Common.Exception;

namespace AISRS.DataAccess
{
	/// <summary>
	/// DaUserRole ��ժҪ˵����
	/// </summary>
	public class DaUserRole : DataAccessBase
	{
		/// <summary>
		/// ȡ����ָ�����û���صĽ�ɫ
		/// </summary>
		/// <param name="userID">�û���ʶ</param>
		/// <param name="dataTable">�����û���ɫ��Ӧ��ϵ��UserRoleData.UserRoleDataTable����</param>
		public void LoadUserRole(
			Guid userID,
			DataTable dataTable)
		{
			string sql = "SELECT user_id as UserID, role_id as RoleID FROM book_user_role WHERE user_id = '" + this.StringToSQL(userID.ToString()) + "'";
			this.AutoFill(dataTable, sql);
		}


		/// <summary>
		/// �õ�ָ���û���ǰ�����н�ɫ
		/// </summary>
		/// <param name="id">ָ���û���PersonID</param>
		/// <param name="dataTable"></param>
		public void LoadUserRoleByUserID(decimal id,DataTable dataTable)
		{
			string sql = "SELECT * FROM AIAPC_USER_ROLE WHERE User_ID = " + id.ToString();
			this.AutoFill(dataTable,sql);			
		}

		/// <summary>
		/// �������û���صĽ�ɫ
		/// </summary>
		/// <param name="dataTable">�������û���ɫ��Ӧ��ϵ��UserRoleData.UserRoleDataTable����</param>
		public void UpdateUserRole(
			DataTable dataTable)
		{
			
			this.AutoUpdate(dataTable,"book_user_role", "user_id as UserID, role_id as RoleID");
		}

		public void LoadUserRoleViewByRoleID(Guid rleID,DataTable dataTable)
		{
			string sql = "SELECT * FROM AIAPC_USER_ROLE_V WHERE ROLE_ID = '" + StringToSQL(rleID.ToString()) + "'";
			this.AutoFill(dataTable,sql);		
		}

//		/// <summary>
//		/// ��������ִ�д˷����󣬶����������ݿ�Ĳ������ڲ���������н���
//		/// </summary>
//		/// <param name="transaction">Ҫ���������</param>
//		public void JoinTransaction(Transaction transaction)
//		{
//			if(this._transaction != null)
//			{
//				throw new DbOperationException("�����Ѿ�����һ��������");
//			}
//			else
//			{				
//				this._transaction = transaction.DataBaseTransaction;
//			}
//		}

		/// <summary>
		/// ɾ��ָ���û���ǰ�����н�ɫ
		/// </summary>
		/// <param name="id">ָ���û���PersonID</param>
		public void DeleteUserRoleByUserID(decimal id)
		{
			string sql = "DELETE FROM AIAPC_USER_ROLE WHERE User_ID = " + id.ToString();
			this.ExecuteNonQuerySql(sql);
		}

		/// <summary>
		/// �����û���ɫ��Ϣ
		/// </summary>
		/// <param name="dataTable"></param>
		public void SaveUserRole(DataTable dataTable)
		{
			string tableName = "AIAPC_USER_ROLE";
			this.AutoUpdate(dataTable,tableName) ;
		}
	}
}
