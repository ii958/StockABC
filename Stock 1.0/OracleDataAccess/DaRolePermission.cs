using System;
using System.Data;
using System.Data.SqlClient;
using AISRS.Common.Data;
using AISRS.DataAccess;

namespace AISRS.DataAccess
{
    /// <summary>
    /// DaRolePermission 的摘要说明。
    /// </summary>
    public class DaRolePermission : DataAccessBase
    {
        /// <summary>
        /// 取出与指定的用户角色相关的权限
        /// </summary>
        /// <param name="roleID">角色标识</param>
        /// <param name="dataTable">返回角色对应权限的RolePermissionData.RolePermissionDataTable对象</param>
        public void LoadRolePermission(
            Guid roleID,
            DataTable dataTable)
        {
            string sql = "SELECT * FROM AIAPC_role_permission WHERE role_id = '" + this.StringToSQL(roleID.ToString()) + "'";
            this.AutoFill(dataTable, sql);
        }

        /// <summary>
        /// 更新与用户角色相关的权限
        /// </summary>
        /// <param name="dataTable">保存着角色权限对应关系的RolePermissionData.RolePermissionDataTable对象</param>
        public void UpdateRolePermission(
            DataTable dataTable)
        {
            this.AutoUpdate(dataTable, "AIAPC_role_permission", "*");
        }

        /// <summary>
        /// 根据角色ID获得角色下所有的需指定区域的权限
        /// </summary>
        /// <param name="strRole"></param>
        /// <param name="dataTable"></param>
        public void LoadPermissionByRoleID(
            string strRole,
            DataTable dataTable)
        {
            string sql = "SELECT * FROM AIAPC_PERMISSION WHERE  "
                + " PERMISSION_ID IN (SELECT PERMISSION_ID FROM AIAPC_ROLE_PERMISSION WHERE "
                + " ROLE_ID IN (" + strRole + ")) AND HAVE_CONTROL_RANGE = '1' ";

            this.AutoFill(dataTable, sql);
        }
    }
}
