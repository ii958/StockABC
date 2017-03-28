using System;
using System.Data;
using System.Data.OracleClient;

namespace AISRS.DataAccess
{
	/// <summary>
	/// DaSBU ��ժҪ˵����
	/// </summary>
	public class DaSBU : DataAccessBase
	{
		/// <summary>
		/// ȡ������SBU����
		/// </summary>
		/// <param name="dataTable"></param>
		public void LoadAllSBU(DataTable dataTable)
		{
			string sql = "SELECT FLEX_VALUE,FLEX_VALUE || ':' || DESCRIPTION AS DESCRIPTION FROM AII_SBU_V  " +
				" WHERE FLEX_VALUE != '07' AND FLEX_VALUE != '16' "+
				" AND FLEX_VALUE != '17' AND FLEX_VALUE != '18' ORDER BY FLEX_VALUE";
			this.AutoFill(dataTable,sql);
		}

		/// <summary>
		/// �����û�ID��Ȩ��IDȡ�õ�ǰ�û���Ȩ�޵�SBU����
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="permissionID"></param>
		/// <param name="userID"></param>
		public void LoadAllSBUByPermission(DataTable dataTable,Guid[] permissionIDs,decimal userID )
		{
			string permissionIDList = string.Empty;
			foreach(Guid permissionID in permissionIDs)
			{
				permissionIDList += "'" + permissionID.ToString().ToUpper()+"',";
			}
			permissionIDList = permissionIDList.Substring(0,permissionIDList.Length - 1);
			string sql = "select distinct asv.flex_value, asv.flex_value || ':' || asv.description AS DESCRIPTION"
				+ " from aii_sbu_v asv, AIAPC_user_sbu aucs"
				+ " where upper(aucs.permission_id) in ("
				+ permissionIDList
				+ ") and aucs.user_id = "
				+ userID.ToString()
				+ " and asv.flex_value = aucs.sbu_id " 
				+" and asv.flex_value != '07' "
				+" and asv.flex_value != '16' "
				+" and asv.flex_value != '17' "
				+" and asv.flex_value != '18' ";

			this.AutoFill(dataTable,sql);
		}
		
		/// <summary>
		/// ����SBUIDȡ��SBU����
		/// </summary>
		/// <param name="no"></param>
		/// <param name="dataTable"></param>
		public void LoadSBUByNo(string no,DataTable dataTable)
		{
			string sql = "SELECT FLEX_VALUE,FLEX_VALUE || ':' || DESCRIPTION AS DESCRIPTION FROM AII_SBU_V WHERE FLEX_VALUE = '"+this.StringToSQL(no)+"'";
			this.AutoFill(dataTable,sql);
		}

		/// <summary>
		/// ���ݵ�ǰ�趨��������˵�PersonID��ȡ�����������趨��Company
		/// </summary>
		/// <param name="dataTable"></param>
		public void LoadApplyerAgentCanSetSBUByPersonID(
			decimal currentPersonID,
			DataTable dataTable)
		{
			string sql = "SELECT DISTINCT AII_SBU_V.* FROM AII_SBU_V,AISSE_USER_COSTCENTER WHERE "
				+" AII_SBU_V.FLEX_VALUE = AISSE_USER_COSTCENTER.SBU_ID "
				+" AND AISSE_USER_COSTCENTER.USER_PERSON_ID = "+currentPersonID.ToString();

			this.AutoFill(dataTable,sql);
		}

		/// <summary>
		/// ȡ���û���Ȩ�޵�SBU����
		/// </summary>
		/// <param name="data"></param>
		/// <param name="userID"></param>
		public void LoadUserSBUData(DataTable data,decimal userID)
		{
			string sql = "select * from aiapc_user_sbu "
				+ " where user_id = "+userID.ToString();
			this.AutoFill(data,sql);
		}

		/// <summary>
		/// �����û�SBUȨ������
		/// </summary>
		/// <param name="data"></param>
		public void SaveUserSbuData(DataTable data)
		{
			this.AutoUpdate(data,"AIAPC_USER_SBU");
		}

		/// <summary>
		/// �����û�IDɾ���û���SBUȨ������
		/// </summary>
		/// <param name="userID"></param>
		public void DeleteUserSbuByUserID(decimal userID)
		{
			string sql  = "delete aiapc_user_sbu"
				+ " where user_id = "+userID.ToString();
			this.ExecuteNonQuerySql(sql);
		}

	}
}
