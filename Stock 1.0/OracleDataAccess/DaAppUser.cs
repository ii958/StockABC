using System;
using System.Data;
using System.Data.OracleClient;


using AISRS.Common.Data;
using AISRS.Common.Exception;
using AISRS.Common.Query;

namespace AISRS.DataAccess
{
	/// <summary>
	/// DaAppUser 的摘要说明。
	/// </summary>
	public class DaAppUser : DataAccessBase
	{
		/// <summary>
		/// 取出所有的用户信息，按用户名排序
		/// </summary>
		/// <param name="dataTable">用来保存用户信息的AppUserData.AppUserDataTable对象</param>
		public void LoadAllAppUser(AppUserData.AppUserDataTable dataTable)
		{
			string sql = "SELECT user_id as ID,Username,Password,FullName,EmailAddress,IsValid FROM book_user Order By Username";
			this.AutoFill(dataTable, sql);
		}

		/// <summary>
		/// 根据所给条件和限制查出用户数据
		/// </summary>
		/// <param name="dataTable">用来保存用户信息的AppUserData.AppUserDataTable对象</param>
		/// <param name="qc">查询条件</param>
		/// <param name="sortFields">排序字段信息</param>
		/// <param name="pagination">分页信息</param>
		/// <param name="loginUserID">当前登录用户ID</param>
		public void LoadAppUser(
			AppUserData.AppUserDataTable dataTable,
			AppUserQueryCondition qc,
			SortFieldArray sortFields,
			Pagination pagination,
			Guid loginUserID)
		{
			
			Guid roleID = qc.RoleID;
			string username = qc.Username;
			string fullName = qc.FullName;

			// 构造 Where 字句
			string whereClause = string.Empty;									
			if(roleID != Guid.Empty)
				whereClause += " AND user_id IN (SELECT user_id FROM book_user_role WHERE role_id = '" + roleID.ToString() + "')";
			
			if(username != string.Empty)
				whereClause += " AND Username like '%" + this.StringToSQL(username) + "%'";

			if(fullName != string.Empty)
				whereClause += " AND FullName like '%" + this.StringToSQL(fullName) + "%'";
			
			if(whereClause != string.Empty)
			{
				whereClause = whereClause.Substring(5);
			}
			
			// 取得符合条件的记录数
			string sql = "SELECT count(*) FROM book_User";
			if(whereClause != string.Empty)
				sql += " WHERE " + whereClause;
			pagination.TotalRecordCount = int.Parse(this.GetScalar(sql).ToString());
			
			// 如果所给PageNumber比实际的PageCount还大，PageNumber取值为PageCount
			if(pagination.PageNumber > pagination.PageCount)
				pagination.PageNumber = pagination.PageCount;	
		
			//			if(PageNumber == 0)
			//				return;
			
			// 取得分页数据
			string orderByClause = sortFields.ToSqlOrderClause();
			sql = "SELECT user_id as ID,Username,\"PASSWORD\",FullName,EmailAddress,IsValid FROM book_user ";
			if(whereClause != string.Empty)
				sql += " WHERE " + whereClause;
			if(orderByClause != string.Empty)
				sql += " ORDER BY " + orderByClause;

			OracleParameter[] paras = new OracleParameter[]
				{
					this.MakeInParam("p_sql", OracleType.NVarChar,2000,sql),
					this.MakeInParam("p_page_size",OracleType.Int32,4,pagination.PageSize),
					this.MakeInParam("p_page_number",OracleType.Int32,4,pagination.PageNumber),
					this.MakeSelectCursorParam()					
				};

			this.AutoFill(dataTable,"Book_Query_User",paras);	
			
		}

		/// <summary>
		/// 查询所有的员工信息
		/// </summary>
		/// <param name="employeeNo">员工编号</param>
		/// <param name="employeeName">员工名称</param>
		/// <param name="nTAccount">NT帐号</param>
		/// <param name="organizationName">部门名称</param>
		/// <param name="pageNumber">分页信息，第几页</param>
		/// <param name="pageSize">分页信息，每页显示的最多记录数</param>
		/// <param name="dataTable">AIAPC_USER_V</param>
		/// <param name="totalRecordCount">符合查询条件的所有记录总数</param>
		public void LoadUserByCondition(
			string employeeNo,
			string employeeName,
			string nTAccount,
			string organizationName,
			int pageNumber,
			int pageSize,
			DataTable dataTable,
			out int totalRecordCount)
		{
			string sql = "SELECT DISTINCT PERSON_ID,EMPLOYEE_NUMBER FROM AISRS_USER_V WHERE 1=1 " ;

			if(employeeNo.Trim() != String.Empty)
			{
				if(employeeNo.IndexOf("%") == -1)
				{
					sql += " AND EMPLOYEE_NUMBER LIKE '%" + StringToSQL(employeeNo.Trim()) + "%'";
				}
				else
				{
					sql += " AND EMPLOYEE_NUMBER LIKE '" + StringToSQL(employeeNo.Trim()) + "'";
				}
			}

			if(employeeName.Trim() != String.Empty)
			{
				if(employeeName.IndexOf("%") == -1)
				{
					sql += " AND LAST_NAME LIKE '%" + StringToSQL(employeeName.Trim()) + "%'";
				}
				else
				{
					sql += " AND LAST_NAME LIKE '" + StringToSQL(employeeName.Trim()) + "'";
				}
				
			}
			if(nTAccount.Trim() != String.Empty)
			{
				if(nTAccount.IndexOf("%") == -1)
				{
					sql += " AND NT_ACCOUNT LIKE '%" + StringToSQL(nTAccount.Trim()) + "%'";
				}
				else
				{
					sql += " AND NT_ACCOUNT LIKE '" + StringToSQL(nTAccount.Trim()) + "'";
				}
				
			}

			if(organizationName.Trim() != String.Empty)
			{
				if(organizationName.IndexOf("%") == -1)
				{
					sql += " AND ORG_NAME LIKE '%" + StringToSQL(organizationName.Trim()) + "%'" ;
				}
				else
				{
					sql += " AND ORG_NAME LIKE '" + StringToSQL(organizationName.Trim()) + "'" ;
				}
				
			}

			sql += " ORDER BY EMPLOYEE_NUMBER" ;


			OracleParameter[] paras = new OracleParameter[]
				{
					this.MakeInParam("p_sql", OracleType.NVarChar,2000,sql),
					this.MakeInParam("p_page_size",OracleType.Int32,4,pageSize),
					this.MakeInParam("p_page_number",OracleType.Int32,4,pageNumber),
					this.MakeOutParam("p_count",OracleType.Number,10),
					this.MakeSelectCursorParam()					
				};

			this.AutoFill(dataTable,"AIAPC_QUERY_USER",paras);	
			totalRecordCount = Int32.Parse(paras[3].Value.ToString());

		}

		/// <summary>
		/// 取出指定的用户的信息
		/// </summary>
		/// <param name="appUserID">用户的ID</param>
		/// <param name="dataTable">用来保存用户信息的AppUserData.AppUserDataTable对象</param>
		public void LoadAppUser(
			Guid appUserID,
			AppUserData.AppUserDataTable dataTable)
		{
			string sql = "SELECT user_id as ID,Username,Password,FullName,EmailAddress,IsValid FROM book_user "
				+ " WHERE user_id = '" + this.StringToSQL(appUserID.ToString()) + "'";
			this.AutoFill(dataTable, sql);
		}


		/// <summary>
		/// 取出指定的用户的信息
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="dataTable">用来保存用户信息的AppUserData.AppUserDataTable对象</param>
		public void LoadAppUser(
			string username,
			AppUserData.AppUserDataTable dataTable)
		{
			string sql = "SELECT user_id as ID,Username,Password,FullName,EmailAddress,IsValid FROM book_user " 
				+ " WHERE Username = '" + this.StringToSQL(username) + "'";
			this.AutoFill(dataTable, sql);
		}

		/// <summary>
		/// 插入新用户的信息
		/// </summary>
		/// <param name="dataTable">AppUserData.AppUserDataTable对象,该对象中应该有且只有一条记录。</param>
		/// <returns>插入操作结果</returns>
		public InsertResult InsertAppUser(AppUserData.AppUserDataTable dataTable)
		{
			InsertResult insertResult = InsertResult.Success;

			// 检查参数有效性
			if(dataTable == null || dataTable.Count == 0)
				throw new ValidationException("插入用户时出错，传入参数为空或没有记录。");
			
			// 执行插入
			OracleParameter[] paras = new OracleParameter[6];
			paras[0] = this.MakeInParam("p_user_id",OracleType.Char,36,dataTable[0].ID.ToString());
			paras[1] = this.MakeInParam("p_username",OracleType.NVarChar,50,dataTable[0].Username);
			paras[2] = this.MakeInParam("p_password",OracleType.NVarChar,50,dataTable[0].Password);
			if(!dataTable[0].IsFullNameNull())
				paras[3] = this.MakeInParam("p_fullName",OracleType.NVarChar,200,dataTable[0].FullName);
			else
				paras[3] = this.MakeInParam("p_fullName",OracleType.NVarChar,200, System.DBNull.Value);
			
			if(!dataTable[0].IsEmailAddressNull())
				paras[4] = this.MakeInParam("p_emailAddress",OracleType.NVarChar,200,dataTable[0].EmailAddress);
			else
				paras[4] = this.MakeInParam("p_emailAddress",OracleType.NVarChar,200, System.DBNull.Value);
			
			paras[5] = MakeOutParam("p_result",OracleType.Int32,4);

			this.ExecuteProc("Book_Insert_User",paras);
			
			// 检查返回结果是否正常
			int outParam = int.Parse(paras[5].Value.ToString());
			if(outParam < 0 || outParam > 2)
				throw new DbOperationException("存储过程InsertAppUser返回参数值不可识别。");

			// 变换返回值
			switch(outParam)
			{
				case 0:
					insertResult = InsertResult.Success;
					break;
				case 1:
					insertResult = InsertResult.Fail;
					break;
				case 2:
					insertResult = InsertResult.ObeyUniqueConstraint;								
					break;
			}

			return insertResult;

		}

		/// <summary>
		/// 修改用户信息
		/// </summary>
		/// <param name="dataTable">AppUserData.AppUserDataTable对象,该对象中应该有且只有一条记录。</param>
		/// <returns>修改操作结果</returns>
		public UpdateResult UpdateAppUser(AppUserData.AppUserDataTable dataTable)
		{
			UpdateResult updateResult = UpdateResult.Success;

			// 检查参数有效性
			if(dataTable == null || dataTable.Count == 0)
				throw new ValidationException("更新用户时出错，传入参数为空或没有记录。");
			
			// 执行修改
			OracleParameter[] paras = new OracleParameter[6];
			paras[0] = this.MakeInParam("p_user_id",OracleType.Char,36,dataTable[0].ID.ToString());
			paras[1] = this.MakeInParam("p_username",OracleType.NVarChar,50,dataTable[0].Username);
			paras[2] = this.MakeInParam("p_password",OracleType.NVarChar,50,dataTable[0].Password);
			if(!dataTable[0].IsFullNameNull())
				paras[3] = this.MakeInParam("p_fullname",OracleType.NVarChar,200,dataTable[0].FullName);
			else
				paras[3] = this.MakeInParam("p_fullname",OracleType.NVarChar,200, System.DBNull.Value);
			
			if(!dataTable[0].IsEmailAddressNull())
				paras[4] = this.MakeInParam("p_emailaddress",OracleType.NVarChar,200,dataTable[0].EmailAddress);
			else
				paras[4] = this.MakeInParam("p_emailaddress",OracleType.NVarChar,200, System.DBNull.Value);
			
			paras[5] = MakeOutParam("p_result",OracleType.Int32,4);

			this.ExecuteProc("Book_Update_User",paras);
			
			// 检查返回结果是否正常
			int outParam = int.Parse(paras[5].Value.ToString());
			if(outParam < 0 || outParam > 2)
				throw new DbOperationException("存储过程UpdateAppUser返回参数值不可识别。");

			// 变换返回值
			switch(outParam)
			{
				case 0:
					updateResult = UpdateResult.Success;
					break;
				case 1:
					updateResult = UpdateResult.Fail;
					break;
				case 2:
					updateResult = UpdateResult.ObeyUniqueConstraint;								
					break;
			}

			return updateResult;
		}
		
		/// <summary>
		/// 删除指定的用户和其相关信息
		/// </summary>
		/// <param name="appUserID">被删除用户的标识</param>
		/// <returns>删除操作结果</returns>
		public DeleteResult DeleteAppUser(Guid appUserID)			
		{
			DeleteResult deleteResult = DeleteResult.Success;
			
			// 执行删除操作
			OracleParameter[] parameters = 
				{
					this.MakeInParam("p_user_id",OracleType.Char,36, appUserID.ToString()),
					MakeOutParam("p_result",OracleType.Int32,0)
				};

			this.ExecuteProc("Book_Delete_User", parameters);

			// 检查返回结果是否正常
			int outParam = int.Parse(parameters[1].Value.ToString());
			if(outParam < 0 || outParam > 2)
				throw new DbOperationException("存储过程DeleteAppUser返回参数值不可识别。");

			// 变换返回值
			switch(outParam)
			{
				case 0:
					deleteResult = DeleteResult.Success;
					break;
				case 1:
					deleteResult = DeleteResult.Fail;
					break;
				case 2:
					deleteResult = DeleteResult.Refered;								
					break;
			}

			return deleteResult;
		}

		/// <summary>
		/// 根据NT帐号获取员工信息
		/// </summary>
		/// <param name="ntAccount">NT帐号</param>
		/// <param name="dataTable"></param>
		public void LoadUserByNTAccount(string ntAccount,DataTable dataTable)
		{
			string sql = "SELECT * FROM AISRS_USER_V WHERE NT_ACCOUNT = '" + StringToSQL(ntAccount.Trim()) + "'";

			this.AutoFill(dataTable,sql);
		}

		/// <summary>
		/// 根据用户的用户ID获取用户权限
		/// </summary>
		/// <param name="person_ID">用户ID</param>
		/// <param name="dataTable"></param>
		public void LoadUserPermission(decimal person_ID,DataTable dataTable)
		{
			OracleParameter[] parameters = 
				{
					this.MakeInParam("p_personID",OracleType.VarChar,150,person_ID.ToString()),
					this.MakeSelectCursorParam()
				};
			this.AutoFill(dataTable,"AISRS_GET_USER_PERMISSION",parameters);
		}
		

		/// <summary>
		/// 根据用户ID得到需要设置角色权限范围
		/// </summary>
		/// <param name="id">指定用户的PersonID</param>
		/// <param name="dataTable"></param>
		public void LoadUserRolePermissionRange(decimal id,DataTable dataTable)
		{
			OracleParameter[] parameters = 
				{
					this.MakeInParam("p_person_id",OracleType.VarChar,150, id.ToString()),
					this.MakeSelectCursorParam()
				};
			this.AutoFill(dataTable,"AISRS_GET_USER_ROLE_RANGE",parameters);
		}
	}
}
