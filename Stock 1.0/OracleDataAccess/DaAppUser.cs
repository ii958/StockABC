using System;
using System.Data;
using System.Data.OracleClient;


using AISRS.Common.Data;
using AISRS.Common.Exception;
using AISRS.Common.Query;

namespace AISRS.DataAccess
{
	/// <summary>
	/// DaAppUser ��ժҪ˵����
	/// </summary>
	public class DaAppUser : DataAccessBase
	{
		/// <summary>
		/// ȡ�����е��û���Ϣ�����û�������
		/// </summary>
		/// <param name="dataTable">���������û���Ϣ��AppUserData.AppUserDataTable����</param>
		public void LoadAllAppUser(AppUserData.AppUserDataTable dataTable)
		{
			string sql = "SELECT user_id as ID,Username,Password,FullName,EmailAddress,IsValid FROM book_user Order By Username";
			this.AutoFill(dataTable, sql);
		}

		/// <summary>
		/// �����������������Ʋ���û�����
		/// </summary>
		/// <param name="dataTable">���������û���Ϣ��AppUserData.AppUserDataTable����</param>
		/// <param name="qc">��ѯ����</param>
		/// <param name="sortFields">�����ֶ���Ϣ</param>
		/// <param name="pagination">��ҳ��Ϣ</param>
		/// <param name="loginUserID">��ǰ��¼�û�ID</param>
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

			// ���� Where �־�
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
			
			// ȡ�÷��������ļ�¼��
			string sql = "SELECT count(*) FROM book_User";
			if(whereClause != string.Empty)
				sql += " WHERE " + whereClause;
			pagination.TotalRecordCount = int.Parse(this.GetScalar(sql).ToString());
			
			// �������PageNumber��ʵ�ʵ�PageCount����PageNumberȡֵΪPageCount
			if(pagination.PageNumber > pagination.PageCount)
				pagination.PageNumber = pagination.PageCount;	
		
			//			if(PageNumber == 0)
			//				return;
			
			// ȡ�÷�ҳ����
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
		/// ��ѯ���е�Ա����Ϣ
		/// </summary>
		/// <param name="employeeNo">Ա�����</param>
		/// <param name="employeeName">Ա������</param>
		/// <param name="nTAccount">NT�ʺ�</param>
		/// <param name="organizationName">��������</param>
		/// <param name="pageNumber">��ҳ��Ϣ���ڼ�ҳ</param>
		/// <param name="pageSize">��ҳ��Ϣ��ÿҳ��ʾ������¼��</param>
		/// <param name="dataTable">AIAPC_USER_V</param>
		/// <param name="totalRecordCount">���ϲ�ѯ���������м�¼����</param>
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
		/// ȡ��ָ�����û�����Ϣ
		/// </summary>
		/// <param name="appUserID">�û���ID</param>
		/// <param name="dataTable">���������û���Ϣ��AppUserData.AppUserDataTable����</param>
		public void LoadAppUser(
			Guid appUserID,
			AppUserData.AppUserDataTable dataTable)
		{
			string sql = "SELECT user_id as ID,Username,Password,FullName,EmailAddress,IsValid FROM book_user "
				+ " WHERE user_id = '" + this.StringToSQL(appUserID.ToString()) + "'";
			this.AutoFill(dataTable, sql);
		}


		/// <summary>
		/// ȡ��ָ�����û�����Ϣ
		/// </summary>
		/// <param name="username">�û���</param>
		/// <param name="dataTable">���������û���Ϣ��AppUserData.AppUserDataTable����</param>
		public void LoadAppUser(
			string username,
			AppUserData.AppUserDataTable dataTable)
		{
			string sql = "SELECT user_id as ID,Username,Password,FullName,EmailAddress,IsValid FROM book_user " 
				+ " WHERE Username = '" + this.StringToSQL(username) + "'";
			this.AutoFill(dataTable, sql);
		}

		/// <summary>
		/// �������û�����Ϣ
		/// </summary>
		/// <param name="dataTable">AppUserData.AppUserDataTable����,�ö�����Ӧ������ֻ��һ����¼��</param>
		/// <returns>����������</returns>
		public InsertResult InsertAppUser(AppUserData.AppUserDataTable dataTable)
		{
			InsertResult insertResult = InsertResult.Success;

			// ��������Ч��
			if(dataTable == null || dataTable.Count == 0)
				throw new ValidationException("�����û�ʱ�����������Ϊ�ջ�û�м�¼��");
			
			// ִ�в���
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
			
			// ��鷵�ؽ���Ƿ�����
			int outParam = int.Parse(paras[5].Value.ToString());
			if(outParam < 0 || outParam > 2)
				throw new DbOperationException("�洢����InsertAppUser���ز���ֵ����ʶ��");

			// �任����ֵ
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
		/// �޸��û���Ϣ
		/// </summary>
		/// <param name="dataTable">AppUserData.AppUserDataTable����,�ö�����Ӧ������ֻ��һ����¼��</param>
		/// <returns>�޸Ĳ������</returns>
		public UpdateResult UpdateAppUser(AppUserData.AppUserDataTable dataTable)
		{
			UpdateResult updateResult = UpdateResult.Success;

			// ��������Ч��
			if(dataTable == null || dataTable.Count == 0)
				throw new ValidationException("�����û�ʱ�����������Ϊ�ջ�û�м�¼��");
			
			// ִ���޸�
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
			
			// ��鷵�ؽ���Ƿ�����
			int outParam = int.Parse(paras[5].Value.ToString());
			if(outParam < 0 || outParam > 2)
				throw new DbOperationException("�洢����UpdateAppUser���ز���ֵ����ʶ��");

			// �任����ֵ
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
		/// ɾ��ָ�����û����������Ϣ
		/// </summary>
		/// <param name="appUserID">��ɾ���û��ı�ʶ</param>
		/// <returns>ɾ���������</returns>
		public DeleteResult DeleteAppUser(Guid appUserID)			
		{
			DeleteResult deleteResult = DeleteResult.Success;
			
			// ִ��ɾ������
			OracleParameter[] parameters = 
				{
					this.MakeInParam("p_user_id",OracleType.Char,36, appUserID.ToString()),
					MakeOutParam("p_result",OracleType.Int32,0)
				};

			this.ExecuteProc("Book_Delete_User", parameters);

			// ��鷵�ؽ���Ƿ�����
			int outParam = int.Parse(parameters[1].Value.ToString());
			if(outParam < 0 || outParam > 2)
				throw new DbOperationException("�洢����DeleteAppUser���ز���ֵ����ʶ��");

			// �任����ֵ
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
		/// ����NT�ʺŻ�ȡԱ����Ϣ
		/// </summary>
		/// <param name="ntAccount">NT�ʺ�</param>
		/// <param name="dataTable"></param>
		public void LoadUserByNTAccount(string ntAccount,DataTable dataTable)
		{
			string sql = "SELECT * FROM AISRS_USER_V WHERE NT_ACCOUNT = '" + StringToSQL(ntAccount.Trim()) + "'";

			this.AutoFill(dataTable,sql);
		}

		/// <summary>
		/// �����û����û�ID��ȡ�û�Ȩ��
		/// </summary>
		/// <param name="person_ID">�û�ID</param>
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
		/// �����û�ID�õ���Ҫ���ý�ɫȨ�޷�Χ
		/// </summary>
		/// <param name="id">ָ���û���PersonID</param>
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
