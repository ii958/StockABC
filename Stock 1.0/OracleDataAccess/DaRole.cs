using System;
using System.Data;
using System.Data.OracleClient;
using AISRS.Common.Data;
using AISRS.Common.Exception;

namespace AISRS.DataAccess
{
	/// <summary>
	/// DaRole 的摘要说明。
	/// </summary>
	public class DaRole : DataAccessBase
	{
		/// <summary>
		/// 取出所有的用户角色信息
		/// </summary>
		/// <param name="dataTable">用来保存角色信息的RoleData.RoleDataTable对象</param>
		public void LoadAllRole(DataTable dataTable)
		{
			string sql = "SELECT * FROM AIAPC_role WHERE is_valid = 1 Order By name";//role_id as ID, name as Name, description,created_by,creation_date,last_updated_by,last_updation_date
			this.AutoFill(dataTable, sql);
		}

		/// <summary>
		/// 取出指定的用户角色的信息
		/// </summary>
		/// <param name="roleID">用户角色的ID</param>
		/// <param name="dataTable">用来保存角色信息的RoleData.RoleDataTable对象</param>
		public void LoadRole(
			Guid roleID,
			DataTable dataTable)
		{
			string sql = "SELECT * FROM AIAPC_role WHERE role_id = '" + this.StringToSQL(roleID.ToString()) + "'";
			this.AutoFill(dataTable, sql);
		}

		/// <summary>
		/// 取出指定的用户角色的信息
		/// </summary>
		/// <param name="roleName">用户角色的名称</param>
		/// <param name="dataTable">用来保存角色信息的RoleData.RoleDataTable对象</param>
		public void LoadRole(
			string roleName,
			DataTable dataTable)
		{
			string sql = "SELECT * FROM AIAPC_role WHERE name = '" + this.StringToSQL(roleName) + "' and is_valid = 1";//role_id as ID, name as Name, description,is_valid,created_by,creation_date,last_updated_by,last_updation_date
			this.AutoFill(dataTable, sql);
		}

		/// <summary>
		/// 更新用户角色的信息
		/// </summary>
		/// <param name="dataTable">RoleData</param>
		public void UpdateRole(	DataTable dataTable)
		{
			this.AutoUpdate(dataTable,"AIAPC_role","*");//AIAPC_role","role_id as ID, name as Name, Description, created_by, creation_date, last_updated_by, last_updation_date, is_valid
		}
		
		/// <summary>
		/// 删除指定的用户角色和其相关信息
		/// </summary>
		/// <param name="roleID">被删除角色的标识</param>
		/// <param name="outParam">传出存储过程的执行结果</param>
		public void DeleteRole(Guid roleID, string lastUpdateID)			//DeleteResult
		{
//			DeleteResult deleteResult = DeleteResult.Success;
			
			// 执行删除操作
			OracleParameter[] parameters = 
				{
					this.MakeInParam("p_role_id",OracleType.Char,36, roleID.ToString()),
					this.MakeInParam("p_last_update_by", OracleType.Number, 10, Convert.ToDecimal(lastUpdateID))
//					MakeOutParam("p_result",OracleType.Int32,4)
				};

			this.ExecuteProc("AIAPC_Delete_Role", parameters);

//			// 检查返回结果是否正常
//			int outParam = int.Parse(parameters[1].Value.ToString());
//			if(outParam < 0 || outParam > 2)
//				throw new DbOperationException("存储过程DeleteRole返回参数值不可识别。");
//
//			// 变换返回值
//			switch(outParam)
//			{
//				case 0:
//					deleteResult = DeleteResult.Success;
//					break;
//				case 1:
//					deleteResult = DeleteResult.Fail;
//					break;
//				case 2:
//					deleteResult = DeleteResult.Refered;								
//					break;
//			}
//
//			return deleteResult;
		}
	}
}
