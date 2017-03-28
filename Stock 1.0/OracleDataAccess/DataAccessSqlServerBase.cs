using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using AISRS.Common.Framework;
using AISRS.Common.Exception;

namespace AISRS.DataAccess
{
    public class DataAccessSqlServerBase : IDisposable
    {
        #region 受保护的变量

        /// <summary>
        /// 链接串
        /// </summary>
        protected string _connectionString;

        // <summary>
        /// 同步数据的Adapter
        /// </summary>
        protected SqlDataAdapter _dataAdapter;

        /// <summary>
        /// 数据库链接
        /// </summary>
        protected SqlConnection _connection;

        /// <summary>
        /// 事务对象
        /// </summary>
        protected SqlTransaction _transaction;

        #endregion

        #region 释放资源

        /// <summary>
        /// 释放对象的资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // as a service to those who might inherit from us
        }

        /// <summary>
        ///	释放对象中的实例
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._transaction != null)
                {
                    this.QuitTransaction();
                    this._transaction = null;
                }

                if (this._dataAdapter != null)
                {
                    if (this._dataAdapter.SelectCommand != null)
                    {
                        if (this._dataAdapter.SelectCommand.Connection != null)
                            this._dataAdapter.SelectCommand.Connection.Dispose();
                        this._dataAdapter.SelectCommand.Dispose();
                    }

                    if (this._dataAdapter.InsertCommand != null)
                    {
                        if (this._dataAdapter.InsertCommand.Connection != null)
                            this._dataAdapter.InsertCommand.Connection.Dispose();
                        this._dataAdapter.InsertCommand.Dispose();
                    }

                    if (_dataAdapter.UpdateCommand != null)
                    {
                        if (this._dataAdapter.UpdateCommand.Connection != null)
                            this._dataAdapter.UpdateCommand.Connection.Dispose();
                        this._dataAdapter.UpdateCommand.Dispose();
                    }

                    if (_dataAdapter.DeleteCommand != null)
                    {
                        if (this._dataAdapter.DeleteCommand.Connection != null)
                            this._dataAdapter.DeleteCommand.Connection.Dispose();
                        this._dataAdapter.DeleteCommand.Dispose();
                    }

                    this._dataAdapter.Dispose();
                    this._dataAdapter = null;
                }

                if (this._connection != null)
                {
                    this._connection.Dispose();
                    this._connection = null;
                }

            }
        }

        #endregion

        #region 构造析构函数
		/// <summary>
		/// 析构函数
		/// </summary>
		~DataAccessSqlServerBase()
		{
			Dispose(false);
		}


		/// <summary>
		/// 构造函数
		/// </summary>		
        public DataAccessSqlServerBase()
		{
			this._connectionString = Configuration.DataAccessConnectionString;
			this._connection = new SqlConnection(this._connectionString);
			this._dataAdapter = new SqlDataAdapter();			
		}
		#endregion

        #region 受保护的方法
        protected void InsertTest()
        {
            string insertString = "INSERT INTO [AISTOCK].[dbo].[AISTOCK_STOCK_INFORMATION] (STOCK_ID, STOCK_CODE, STOCK_NAME, STOCK_DAY, TODAY_BEGIN, YESTERDAY_END, TODAY_END, MAX_PRICE, MIN_PRICE, BUY_PRICE, SELL_PRICE, QUANTITY, TOTAL_MONEY, BUY_ONE_PRICE, BUY_ONE_QUANTITY, BUY_TWO_PRICE, BUY_TWO_QUANTITY, BUY_THD_PRICE, BUY_THD_QUANTITY, BUY_FOU_PRICE, BUY_FOU_QUANTITY, BUY_FIF_PRICE, BUY_FIF_QUANTITY, SELL_ONE_PRICE, SELL_ONE_QUANTITY, SELL_TWO_PRICE, SELL_TWO_QUANTITY, SELL_THD_PRICE, SELL_THD_QUANTITY, SELL_FOU_PRICE, SELL_FOU_QUANTITY, SELL_FIF_PRICE, SELL_FIF_QUANTITY, INCREASE_PERCENT, CHART) VALUES ('FDA6715F-3A45-432B-B970-C4A0805B012A', 'sz000001', '平安银行', '20130108', '16.30', '16.30', '16.00', '16.37', '15.86', '15.99', '16.00', '31247912', '501360093.66', '15.99', '8700', '15.98', '9100', '15.97', '1200', '15.96', '8000', '15.95', '14000', '16.00', '14093', '16.01', '106558', '16.02', '14200', '16.03', '421287', '16.04', '69504', '-1.8404907975460122699386503100', '阴线')";
            this._connection.Open();
            SqlCommand cmd = new SqlCommand(insertString, this._connection);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// CommandBuilder根据指定的查询语句自动生成四个数据库操作的Command，
        /// 然后根据DataSet的DataTable中的记录状态在数据库中执行插入修改或者删除操作
        /// </summary>
        /// <param name="dataTable">包含更新数据的DataTable对象</param>
        /// <param name="tableName">数据库中将被修改数据的表的名称</param>		
        protected virtual void AutoUpdate(
            DataTable dataTable,
            string tableName
            )
        {
            this.AutoUpdate(dataTable, tableName, "*");
        }

        /// <summary>
        /// CommandBuilder根据指定的查询语句自动生成四个数据库操作的Command，
        /// 然后根据DataSet的DataTable中的记录状态在数据库中执行插入修改或者删除操作
        /// </summary>
        /// <param name="dataTable">包含要新数据的DataTable对象</param>
        /// <param name="tableName">数据库中将被修改数据的表的名称</param>
        /// <param name="fieldNameList">被修改的域的名称列表，形如："ID,Name,Description"</param>
        protected virtual void AutoUpdate(
            DataTable dataTable,
            string tableName,
            string fieldNameList
            )
        {
            try
            {
                string sqlQuery = "SELECT TOP 0 " + fieldNameList + " FROM " + tableName;
                if (this._transaction != null)
                {
                    this._dataAdapter.SelectCommand = new SqlCommand(sqlQuery, this._transaction.Connection);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(this._dataAdapter);

                    this._dataAdapter.SelectCommand.Transaction = this._transaction;

                    this._dataAdapter.Update(dataTable);
                }
                else
                {
                    this._dataAdapter.SelectCommand = new SqlCommand(sqlQuery, this._connection);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(this._dataAdapter);

                    this._dataAdapter.Update(dataTable);
                }
            }
            catch (Exception ex)
            {
                string message = "In AutoUpdate. Table name  is '" + tableName + "', Field name list is '" + fieldNameList + "'. " + ex.Message;
                throw new DbOperationException(message);
            }
            finally
            {
                if (this._dataAdapter != null)
                {
                    if (this._dataAdapter.SelectCommand != null)
                    {
                        this._dataAdapter.SelectCommand.Dispose();
                        this._dataAdapter.SelectCommand = null;
                    }

                    if (this._dataAdapter.InsertCommand != null)
                    {
                        this._dataAdapter.InsertCommand.Dispose();
                        this._dataAdapter.InsertCommand = null;
                    }

                    if (_dataAdapter.UpdateCommand != null)
                    {
                        this._dataAdapter.UpdateCommand.Dispose();
                        this._dataAdapter.UpdateCommand = null;
                    }

                    if (_dataAdapter.DeleteCommand != null)
                    {
                        this._dataAdapter.DeleteCommand.Dispose();
                        this._dataAdapter.DeleteCommand = null;
                    }
                }
            }
        }

        /// <summary>
        /// 忽略并发冲突，强行更新表数据
        /// </summary>
        /// <param name="dataTable">包含更新数据的DataTable对象</param>
        /// <param name="tableName">数据库中将被修改数据的表的名称</param>
        protected virtual void ForceUpdate(
            DataTable dataTable,
            string tableName)
        {
            // 创建sql语句
            string sql = string.Empty;
            foreach (DataRow row in dataTable.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        sql = CreateInsertSql(row, tableName);
                        this.ExecuteNonQuerySql(sql.ToString());
                        break;
                    case DataRowState.Deleted:
                        sql = CreateDeleteSql(row, tableName);
                        this.ExecuteNonQuerySql(sql.ToString());
                        break;
                    case DataRowState.Modified:
                        sql = CreateUpdateSql(row, tableName);
                        this.ExecuteNonQuerySql(sql.ToString());
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 根据指定的查询语句，填充指定的DataSet中的指定的表
        /// </summary>
        /// <param name="dataTable">被填充数据的DataTable对象</param>
        /// <param name="sqlQuery">SQL查询语句</param>		
        protected virtual void AutoFill(
            DataTable dataTable,
            string sqlQuery)
        {
            try
            {
                this._dataAdapter.SelectCommand = CreateCommand(sqlQuery);
                this._dataAdapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                string message = "In AutoFill. SQL sentence is '" + sqlQuery + "'. " + ex.Message;
                throw new DbOperationException(ex.Message);
            }
            finally
            {
                if (this._dataAdapter.SelectCommand != null)
                    this._dataAdapter.SelectCommand.Dispose();
            }
        }

        /// <summary>
        /// 执行用存储过程，填充指定的DataSet中的指定的表
        /// </summary>
        /// <param name="dataTable">被填充数据的DataTable对象</param>
        /// <param name="procName">存储过程名称</param>
        /// <param name="parameters">参数数组</param>	
        protected virtual void AutoFill(
            DataTable dataTable,
            string procName,
            SqlParameter[] parameters
            )
        {
            try
            {
                this._dataAdapter.SelectCommand = CreateCommand(procName, parameters);
                this._dataAdapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                string message = "In AutoFill. Store procedure name is '" + procName + "'. " + ex.Message;
                throw new DbOperationException(ex.Message);
            }
            finally
            {
                if (this._dataAdapter.SelectCommand != null)
                    this._dataAdapter.SelectCommand.Dispose();
            }
        }

        /// <summary>
        /// 执行一条非查询的sql语句
        /// </summary>
        /// <param name="sql">要执行的Oracle语句</param>		
        protected virtual void ExecuteNonQuerySql(string sql)
        {
            SqlCommand cmd = CreateCommand(sql);
            ExecuteNonQueryCommand(cmd);
        }


        /// <summary>
        /// 生成输入参数
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">参数值长度</param>
        /// <param name="paramValue">参数值</param>
        /// <returns>生成的参数对象</returns>
        protected virtual SqlParameter MakeInParam(string paramName, SqlDbType dbType, int size, object paramValue)
        {
            return MakeParam(paramName, dbType, size, ParameterDirection.Input, paramValue);
        }

        /// <summary>
        /// 生成输出参数
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">输出的参数值最大长度</param>		
        /// <returns>生成的参数对象</returns>
        protected virtual SqlParameter MakeOutParam(string paramName, SqlDbType dbType, int size)
        {
            return MakeParam(paramName, dbType, size, ParameterDirection.Output, null);
        }

        /// <summary>
        /// 生成存储过程参数
        /// </summary>
        /// <param name="paramName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">参数值长度</param>
        /// <param name="direction">参数传入或传出方向</param>
        /// <param name="paramValue">参数值</param>
        /// <returns>生成的参数对象</returns>
        protected virtual SqlParameter MakeParam(string paramName, SqlDbType dbType, int size, ParameterDirection direction, object paramValue)
        {
            SqlParameter param;

            if (size > 0)
                param = new SqlParameter(paramName, dbType, size);
            else
                param = new SqlParameter(paramName, dbType);

            param.Direction = direction;
            if (!(direction == ParameterDirection.Output && paramValue == null))
                param.Value = paramValue;

            return param;
        }

        /// <summary>
        /// 创建一个SELECT_CURSOR类型的参数，用来返回记录集
        /// </summary>
        /// <returns>SELECT_CURSOR类型的参数，用来返回记录集</returns>
        protected virtual SqlParameter MakeSelectCursorParam()
        {
            //return new SqlParameter("p_CURSOR", SqlDbType.Cursor, 0, ParameterDirection.Output, true, 0, 0, "", DataRowVersion.Default, Convert.DBNull);
            return null;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>		
        protected virtual void ExecuteProc(string procName)
        {
            SqlCommand cmd = CreateCommand(procName, null);
            this.ExecuteNonQueryCommand(cmd);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="prams">参数数组</param>				
        protected virtual void ExecuteProc(string procName, SqlParameter[] prams)
        {
            SqlCommand cmd = CreateCommand(procName, prams);
            this.ExecuteNonQueryCommand(cmd);
        }


        /// <summary>
        /// 执行存储过程，用传出的DataReader对象保存存储过程返回的数据
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="prams">参数数组</param>
        /// <param name="dataReader">保存数据的DataReader对象<</param>
        protected virtual void ExecuteProc(string procName, SqlParameter[] prams, out SqlDataReader dataReader)
        {
            SqlCommand cmd = CreateCommand(procName, prams);
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

        }

        /// <summary>
        /// 执行一条SQL语句，并取得查询结果第一行、第一列的值
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>查询结果第一行、第一列的值</returns>
        protected virtual object GetScalar(string sql)
        {
            SqlCommand cmd = CreateCommand(sql);
            return ExecuteScalarCommand(cmd);
        }

        /// <summary>
        /// 执行存储过程，并取得查询结果第一行、第一列的值
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="prams">参数数组</param>
        /// <returns>查询结果第一行、第一列的值</returns>
        protected virtual object GetScalar(string procName, SqlParameter[] prams)
        {
            SqlCommand cmd = CreateCommand(procName, prams);
            return ExecuteScalarCommand(cmd);
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 加入事务，执行此方法后，对象所有数据库的操作均在参与的事务中进行
        /// </summary>
        /// <param name="transaction">要参与的事务</param>
        public void JoinTransaction(TransactionSqlServer transaction)
        {
            if (this._transaction != null)
            {
                throw new DbOperationException("对象已经在另一个事务中");
            }
            else
            {
                this._transaction = transaction.DataBaseTransaction;
            }
        }

        /// <summary>
        /// 退出事务，执行此方法后，对象所有数据库的操作不再在任何事务中进行	
        /// </summary>
        public void QuitTransaction()
        {
            if (this._transaction != null)
            {
                // 确保_dataAdapter对象的各命令的连接不是事务的连接，以免Dispose _dataAdapter对象时，意外关闭事务的连接。
                if (this._dataAdapter != null)
                {
                    if (this._dataAdapter.SelectCommand != null)
                    {
                        if (this._dataAdapter.SelectCommand.Connection != null)
                            if (this._dataAdapter.SelectCommand.Connection.Equals(this._transaction.Connection))
                                this._dataAdapter.SelectCommand.Connection = null;
                    }

                    if (this._dataAdapter.InsertCommand != null)
                    {
                        if (this._dataAdapter.InsertCommand.Connection != null)
                            if (this._dataAdapter.InsertCommand.Connection.Equals(this._transaction.Connection))
                                this._dataAdapter.InsertCommand.Connection = null;
                    }

                    if (_dataAdapter.UpdateCommand != null)
                    {
                        if (this._dataAdapter.UpdateCommand.Connection != null)
                            if (this._dataAdapter.UpdateCommand.Connection.Equals(this._transaction.Connection))
                                this._dataAdapter.UpdateCommand.Connection = null;
                    }

                    if (_dataAdapter.DeleteCommand != null)
                    {
                        if (this._dataAdapter.DeleteCommand.Connection != null)
                            if (this._dataAdapter.DeleteCommand.Connection.Equals(this._transaction.Connection))
                                this._dataAdapter.DeleteCommand.Connection = null;
                    }
                }

                //确保本对象的连接不是事务的连接，以免本对象Dispose时，意外关闭事务的连接。
                if (this._connection != null)
                {
                    if (this._connection.Equals(this._transaction.Connection))
                        this._connection = null;
                }

                //
                this._transaction = null;
            }
        }

        /// <summary>
        /// 把字符串做适当的处理，返回在SQL语句中可用做字符串的新字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string StringToSQL(string input)
        {
            // TODO: 
            string output = input.Replace("'", "''");
            return output;
        }

        /// <summary>
        /// 返回数据库所在Server当前的时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            // TODO: Oracle的取得当前时间的函数
            string sql = "SELECT GETDATE()";
            object dt = this.GetScalar(sql);
            return DateTime.Parse(dt.ToString());
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 创建一个执行存储过程的Command对象
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="prams">传给存储过程的参数数组</param>		
        /// <returns>Command 对象</returns>
        private SqlCommand CreateCommand(string procName, SqlParameter[] prams)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = procName;
            //连接超时设置
            cmd.CommandTimeout = 180;
            if (this._transaction == null)
            {
                cmd.Connection = this._connection;
            }
            else
            {
                cmd.Connection = this._transaction.Connection;
                cmd.Transaction = this._transaction;
            }

            cmd.CommandType = CommandType.StoredProcedure;

            // add proc parameters
            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                    cmd.Parameters.Add(parameter);
            }

            // return param

            // Oracle的存储过程不返回任何值
            //			cmd.Parameters.Add(
            //				new SqlParameter("ReturnValue", SqlType.Int32, 4,
            //				ParameterDirection.ReturnValue, false, 0, 0,
            //				string.Empty, DataRowVersion.Default, null));

            return cmd;
        }


        /// <summary>
        /// 创建一个执行SQL语句的Command对象
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>			
        /// <returns>Command 对象</returns>
        private SqlCommand CreateCommand(string sql)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            //连接超时设置
            cmd.CommandTimeout = 180;
            if (this._transaction == null)
            {
                cmd.Connection = this._connection;
            }
            else
            {
                cmd.Connection = this._transaction.Connection;
                cmd.Transaction = this._transaction;
            }
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        /// <summary>
        /// 执行非查询操作命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private void ExecuteNonQueryCommand(SqlCommand cmd)
        {
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string message = "In ExecuteNonQueryCommand. Command text is '" + cmd.CommandText + "'. " + ex.Message;
                throw new DbOperationException(message);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        /// <summary>
        /// 执行取得第一行、第一列结果的查询命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private object ExecuteScalarCommand(SqlCommand cmd)
        {
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();

                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                string message = "In ExecuteScalarCommand. Command text is '" + cmd.CommandText + "'. " + ex.Message;
                throw new DbOperationException(message);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        /// <summary>
        /// 根据数据行的主键，创建Where语句
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <param name="row"></param>
        /// <returns>Where语句</returns>
        private string CreateWhereClause(DataRow row)
        {
            bool isDeleteRow = false;
            if (row.RowState == DataRowState.Deleted)
            {
                isDeleteRow = true;
                row.RejectChanges();
            }

            DataColumn[] primaryKey = row.Table.PrimaryKey;
            if (primaryKey == null || primaryKey.Length == 0)
            {
                if (isDeleteRow)
                    row.Delete();
                throw new ValidationException("Error in 'CreateWhereClause',No primary key in datarow object.");
            }

            StringBuilder whereClause = new StringBuilder();
            for (int i = 0; i < primaryKey.Length - 1; i++)
            {
                whereClause.Append(primaryKey[i].ColumnName + " = '" + this.StringToSQL(row[primaryKey[i].ColumnName].ToString()) + "' AND ");
            }
            whereClause.Append(primaryKey[primaryKey.Length - 1].ColumnName + " = '" + this.StringToSQL(row[primaryKey[primaryKey.Length - 1].ColumnName].ToString()) + "'");

            if (isDeleteRow)
                row.Delete();

            return whereClause.ToString();
        }

        /// <summary>
        /// 根据数据行，创建Insert语句
        /// </summary>
        /// <param name="row"></param>
        /// <param name="tableName"></param>
        /// <returns>Insert语句</returns>
        private string CreateInsertSql(DataRow row, string tableName)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO " + tableName + " (");

            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                sql.Append(row.Table.Columns[i].ColumnName + ", ");
            }

            // cut ", "
            sql.Remove(sql.Length - 2, 2);
            sql.Append(")");

            sql.Append(" VALUES (");

            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                if (row.IsNull(i))
                    sql.Append("NULL, ");
                else
                    sql.Append("'" + this.StringToSQL(row[i].ToString()) + "', ");
            }
            // cut ", "
            sql.Remove(sql.Length - 2, 2);
            sql.Append(")");

            return sql.ToString();
        }

        /// <summary>
        /// 根据数据行，创建Update语句
        /// </summary>
        /// <param name="row"></param>
        /// <param name="tableName"></param>
        /// <returns>Update语句</returns>
        private string CreateUpdateSql(DataRow row, string tableName)
        {
            if (row.Table.Columns.Count == row.Table.PrimaryKey.Length)
                return "";

            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE " + tableName + " SET ");
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                //判断是否为主键列
                bool isPrimayKeyColumn = false;
                foreach (DataColumn primaryKeyColumn in row.Table.PrimaryKey)
                {
                    if (row.Table.Columns[i].Equals(primaryKeyColumn))
                    {
                        isPrimayKeyColumn = true;
                        break;
                    }
                }


                if (!isPrimayKeyColumn)
                {
                    if (row.IsNull(i))
                        sql.Append(row.Table.Columns[i].ColumnName + " = NULL, ");
                    else
                        sql.Append(row.Table.Columns[i].ColumnName + " = '" + this.StringToSQL(row[i].ToString()) + "', ");
                }

            }

            // cut ", "
            sql.Remove(sql.Length - 2, 2);

            sql.Append(" WHERE " + CreateWhereClause(row));
            return sql.ToString();
        }

        /// <summary>
        /// 根据数据行，创建Delete语句
        /// </summary>
        /// <param name="row"></param>
        /// <param name="tableName"></param>
        /// <returns>Delete语句</returns>
        private string CreateDeleteSql(DataRow row, string tableName)
        {
            return "DELETE FROM " + tableName + " WHERE " + CreateWhereClause(row);
        }

        #endregion
    }
}
