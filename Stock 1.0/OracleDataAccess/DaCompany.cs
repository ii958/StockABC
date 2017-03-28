using System;
using System.Data;
using System.Data.OracleClient;

namespace AISRS.DataAccess
{
	/// <summary>
	/// DaCompany 的摘要说明。
	/// </summary>
	public class DaCompany : DataAccessBase
	{
		public void LoadAllCompany(DataTable dataTable)
		{
			string sql = "SELECT  FLEX_VALUE,FLEX_VALUE || ':' || DESCRIPTION AS DESCRIPTION  FROM AII_COMPANY_V  where FLEX_VALUE <= '65' ORDER BY FLEX_VALUE";
			this.AutoFill(dataTable,sql);
		}
		
		public void LoadCompanyByNo(string no,DataTable dataTable)
		{
			string sql = "SELECT  FLEX_VALUE,DESCRIPTION AS DESCRIPTION  FROM AII_COMPANY_V WHERE FLEX_VALUE = '"+this.StringToSQL(no)+"'";
			this.AutoFill(dataTable,sql);
		}

		/// <summary>
		/// 调整指定用户不正常的CompanySbu权限
		/// </summary>
		/// <param name="id"></param>
		public void AdjustUserRegionCompany(decimal id,
			int deleteAll)
		{
			OracleParameter[] parameters = 
				{
					this.MakeInParam("p_person_id",OracleType.VarChar,150, id.ToString()),
					this.MakeInParam("p_delete_bln",OracleType.Int16,1, deleteAll),
			};
//			this.ExecuteProc("AIAPC_ADJUST_USERREGIONCOMPANY",parameters);//存储过程中有一表还未建立，暂时放着
		}

	}
}
