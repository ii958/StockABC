using System;
using System.Data;
using System.Data.OracleClient;

namespace AISRS.DataAccess
{
    /// <summary>
    /// DaLockedTime 的摘要说明。
    /// </summary>
    public class DaLockedTime : DataAccessBase
    {
        /// <summary>
        /// 取得所有的锁定月份数据
        /// </summary>
        /// <param name="data"></param>
        public void LoadAllLockedMonth(DataTable data)
        {
            string sql = "select * from aisrs_locked_time_v"
                + " where is_valid = 1"
                + " order by lock_month desc";
            this.AutoFill(data, sql);
        }

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="lockedMonthID"></param>
        /// <param name="userID"></param>
        public void UnLockingMonthByID(
            Guid lockedMonthID,
            decimal userID
            )
        {
            string sql = "update aisrs_locked_time"
                + " set is_valid = 0,"
                + " last_updated_by =" + userID + ","
                + " last_updation_date = sysdate"
                + " where lock_month_id ='"
                + lockedMonthID.ToString().ToUpper()
                + "'";
            this.ExecuteNonQuerySql(sql);
        }

        /// <summary>
        /// 保存锁定月份数据 
        /// </summary>
        /// <param name="data"></param>
        public void SaveLockedMonthData(DataTable data)
        {
            this.AutoUpdate(data, "AISRS_LOCKED_TIME");
        }

        /// <summary>
        /// 判断指定月份是否处于锁定状态
        /// </summary>
        /// <param name="monthString"></param>
        /// <returns></returns>
        public bool IsExistLockedMonth(string monthString)
        {
            string sql = "select count(*) from aisrs_locked_time where lock_month = '"
                + monthString
                + "' and is_valid = 1";
            int count = int.Parse(this.GetScalar(sql).ToString());
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取最新被锁定的月份
        /// </summary>
        /// <returns></returns>
        public string GetLastedLockedMonth()
        {
            string sql = "select lock_month" +
                         " from aisrs_locked_time" +
                         " where is_valid = 1" +
                         " order by lock_month desc";
            string month = this.GetScalar(sql).ToString();
            return month;
        }
    }
}
