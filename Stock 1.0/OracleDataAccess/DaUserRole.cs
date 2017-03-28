using System;
using System.Data;
using System.Data.SqlClient;

using AISRS.Common.Data;
using AISRS.Common.Exception;

namespace AISRS.DataAccess
{
	/// <summary>
	/// DaUserRole 的摘要说明。
	/// </summary>
	public class DaUserRole : DataAccessBase
	{
		/// <summary>
		/// 取出与指定的用户相关的角色
		/// </summary>
		/// <param name="userID">用户标识</param>
		/// <param name="dataTable">返回用户角色对应关系的UserRoleData.UserRoleDataTable对象</param>
		public void LoadUserRole(
			Guid userID,
			DataTable dataTable)
		{
			string sql = "SELECT user_id as UserID, role_id as RoleID FROM book_user_role WHERE user_id = '" + this.StringToSQL(userID.ToString()) + "'";
			this.AutoFill(dataTable, sql);
		}


		/// <summary>
		/// 得到指定用户当前的所有角色
		/// </summary>
		/// <param name="id">指定用户的PersonID</param>
		/// <param name="dataTable"></param>
		public void LoadUserRoleByUserID(decimal id,DataTable dataTable)
		{
			string sql = "SELECT * FROM AIAPC_USER_ROLE WHERE User_ID = " + id.ToString();
			this.AutoFill(dataTable,sql);			
		}

		/// <summary>
		/// 更新与用户相关的角色
		/// </summary>
		/// <param name="dataTable">保存着用户角色对应关系的UserRoleData.UserRoleDataTable对象</param>
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
//		/// 加入事务，执行此方法后，对象所有数据库的操作均在参与的事务中进行
//		/// </summary>
//		/// <param name="transaction">要参与的事务</param>
//		public void JoinTransaction(Transaction transaction)
//		{
//			if(this._transaction != null)
//			{
//				throw new DbOperationException("对象已经在另一个事务中");
//			}
//			else
//			{				
//				this._transaction = transaction.DataBaseTransaction;
//			}
//		}

		/// <summary>
		/// 删除指定用户当前的所有角色
		/// </summary>
		/// <param name="id">指定用户的PersonID</param>
		public void DeleteUserRoleByUserID(decimal id)
		{
			string sql = "DELETE FROM AIAPC_USER_ROLE WHERE User_ID = " + id.ToString();
			this.ExecuteNonQuerySql(sql);
		}

		/// <summary>
		/// 保存用户角色信息
		/// </summary>
		/// <param name="dataTable"></param>
		public void SaveUserRole(DataTable dataTable)
		{
			string tableName = "AIAPC_USER_ROLE";
			this.AutoUpdate(dataTable,tableName) ;
		}
	}
}
