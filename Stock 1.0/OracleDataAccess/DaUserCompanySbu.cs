using System;
using System.Data;
using System.Data.OracleClient ;

namespace AISRS.DataAccess
{
	/// <summary>
	/// DaUserCompanySbu 的摘要说明。
	/// </summary>
	public class DaUserCompanySbu : DataAccessBase
	{
		/// <summary>
        /// 删除指定用户对应的CompanySbu权限信息【非设置进度报告附件信息相关的Company、SBU权限】
		/// </summary>
		/// <param name="id">指定用户的PersonID</param>
		public void DeleteUserCompanySbuByUserID(decimal id)
		{
			string sql = "DELETE FROM AIAPC_USER_COMPANY_SBU"
                       + " WHERE PERMISSION_ID NOT IN ('" + Common.Const.PermissionConst.PERMISSION_REPORT_PROCESS_FULLCONTROL_ID + "','" + Common.Const.PermissionConst.PERMISSION_REPORT_PROCESS_DETAIL_READONLY_ID + "','" + Common.Const.PermissionConst.PERMISSION_REPORT_PROCESS_TOTAL_READONLY_ID + "')"
                       + " AND User_ID = " + id.ToString();
			this.ExecuteNonQuerySql(sql);
		}
        /// <summary>
        /// 删除指定用户对应的CompanySbu权限信息【设置进度报告附件信息相关的Company、SBU权限】
        /// </summary>
        /// <param name="id">指定用户的PersonID</param>
        public void DeleteUserProcessReportCompanySbuByUserID(decimal id)
        {
            string sql = "DELETE FROM AIAPC_USER_COMPANY_SBU"
                       + " WHERE PERMISSION_ID IN ('" + Common.Const.PermissionConst.PERMISSION_REPORT_PROCESS_FULLCONTROL_ID + "','" + Common.Const.PermissionConst.PERMISSION_REPORT_PROCESS_DETAIL_READONLY_ID + "','" + Common.Const.PermissionConst.PERMISSION_REPORT_PROCESS_TOTAL_READONLY_ID + "')"
                       + " AND User_ID = " + id.ToString();
            this.ExecuteNonQuerySql(sql);
        }
		/// <summary>
		/// 得到指定用户对应的CompanySbu权限信息
		/// </summary>
		/// <param name="id">指定用户的PersonID</param>
		/// <param name="dataTable"></param>
		public void LoadUserCompanySbuByID(decimal id,DataTable dataTable)
		{
			string sql = "SELECT * FROM AIAPC_USER_COMPANY_SBU WHERE User_ID = " + id.ToString();
			this.AutoFill(dataTable,sql);			
		}

		/// <summary>
		/// 调整指定用户不正常的CompanySbu权限
		/// </summary>
		/// <param name="id"></param>
		public void AdjustUserCompanySbu(decimal id)
		{
			OracleParameter[] parameters = 
				{
					this.MakeInParam("p_person_id",OracleType.VarChar,150, id.ToString()),
			};
			this.ExecuteProc("AIAPC_ADJUST_USER_COMPANY_SBU",parameters);
		}

		/// <summary>
		/// 保存用户对应的CompanySbu权限信息
		/// </summary>
		/// <param name="dataTable"></param>
		public void SaveUserCompanySbu(DataTable dataTable)
		{
			string tableName = "AIAPC_USER_COMPANY_SBU";
			this.AutoUpdate(dataTable,tableName) ;
		}

        public void SaveUserCompanySbus(string _data)
        {
            if (string.IsNullOrEmpty(_data) && _data.Length == 0)
            {
                return;
            }
            string[] rows = _data.Split('~');
            for (int i = 0; i < rows.Length; i++)
            {
                string[] row = rows[i].Split('|');
                string sql = "INSERT INTO AIAPC_USER_COMPANY_SBU(USER_ID, COMPANY_ID, SBU_ID, PERMISSION_ID) VALUES ('" + row[0] + "','" + row[1] + "','" + row[2] + "','" + row[3] + "')";
                this.ExecuteNonQuerySql(sql);
            }
        }

		/// <summary>
		/// 得到指定用户对应的报告类型信息
		/// </summary>
		/// <param name="id"></param>
		/// <param name="dataTable"></param>
		public void LoadReportTypeByUserID(decimal id,DataTable dataTable)
		{
			string sql = "SELECT * FROM AIAPC_USER_REPORT_TYPE WHERE User_ID = " + id.ToString();
			this.AutoFill(dataTable,sql);	
		}

		/// <summary>
		/// 保存用户对应的报告类型信息
		/// </summary>
		/// <param name="?"></param>
		/// <param name="?"></param>
		public void SaveUserReportTypeData(DataTable dataTable)
		{
			string tableName = "AIAPC_USER_REPORT_TYPE";
			this.AutoUpdate(dataTable,tableName) ;
		}

		/// <summary>
		/// 删除指定用户对应的报告类型信息
		/// </summary>
		/// <param name="id"></param>
		public void DeleteReportTypeByUserID(decimal id)
		{
			string sql = "DELETE AIAPC_USER_REPORT_TYPE WHERE User_ID = " + id.ToString();
			this.ExecuteNonQuerySql(sql);
		}
	}
}

