using System;
using System.Data;
using System.Data.OracleClient;
using AISRS.Common.Data;
using AISRS.Common.Exception;

namespace AISRS.DataAccess
{
	/// <summary>
	/// DaRole ��ժҪ˵����
	/// </summary>
	public class DaRole : DataAccessBase
	{
		/// <summary>
		/// ȡ�����е��û���ɫ��Ϣ
		/// </summary>
		/// <param name="dataTable">���������ɫ��Ϣ��RoleData.RoleDataTable����</param>
		public void LoadAllRole(DataTable dataTable)
		{
			string sql = "SELECT * FROM AIAPC_role WHERE is_valid = 1 Order By name";//role_id as ID, name as Name, description,created_by,creation_date,last_updated_by,last_updation_date
			this.AutoFill(dataTable, sql);
		}

		/// <summary>
		/// ȡ��ָ�����û���ɫ����Ϣ
		/// </summary>
		/// <param name="roleID">�û���ɫ��ID</param>
		/// <param name="dataTable">���������ɫ��Ϣ��RoleData.RoleDataTable����</param>
		public void LoadRole(
			Guid roleID,
			DataTable dataTable)
		{
			string sql = "SELECT * FROM AIAPC_role WHERE role_id = '" + this.StringToSQL(roleID.ToString()) + "'";
			this.AutoFill(dataTable, sql);
		}

		/// <summary>
		/// ȡ��ָ�����û���ɫ����Ϣ
		/// </summary>
		/// <param name="roleName">�û���ɫ������</param>
		/// <param name="dataTable">���������ɫ��Ϣ��RoleData.RoleDataTable����</param>
		public void LoadRole(
			string roleName,
			DataTable dataTable)
		{
			string sql = "SELECT * FROM AIAPC_role WHERE name = '" + this.StringToSQL(roleName) + "' and is_valid = 1";//role_id as ID, name as Name, description,is_valid,created_by,creation_date,last_updated_by,last_updation_date
			this.AutoFill(dataTable, sql);
		}

		/// <summary>
		/// �����û���ɫ����Ϣ
		/// </summary>
		/// <param name="dataTable">RoleData</param>
		public void UpdateRole(	DataTable dataTable)
		{
			this.AutoUpdate(dataTable,"AIAPC_role","*");//AIAPC_role","role_id as ID, name as Name, Description, created_by, creation_date, last_updated_by, last_updation_date, is_valid
		}
		
		/// <summary>
		/// ɾ��ָ�����û���ɫ���������Ϣ
		/// </summary>
		/// <param name="roleID">��ɾ����ɫ�ı�ʶ</param>
		/// <param name="outParam">�����洢���̵�ִ�н��</param>
		public void DeleteRole(Guid roleID, string lastUpdateID)			//DeleteResult
		{
//			DeleteResult deleteResult = DeleteResult.Success;
			
			// ִ��ɾ������
			OracleParameter[] parameters = 
				{
					this.MakeInParam("p_role_id",OracleType.Char,36, roleID.ToString()),
					this.MakeInParam("p_last_update_by", OracleType.Number, 10, Convert.ToDecimal(lastUpdateID))
//					MakeOutParam("p_result",OracleType.Int32,4)
				};

			this.ExecuteProc("AIAPC_Delete_Role", parameters);

//			// ��鷵�ؽ���Ƿ�����
//			int outParam = int.Parse(parameters[1].Value.ToString());
//			if(outParam < 0 || outParam > 2)
//				throw new DbOperationException("�洢����DeleteRole���ز���ֵ����ʶ��");
//
//			// �任����ֵ
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
