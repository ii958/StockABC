using System;
using System.Data;
using System.Data.OracleClient;
using System.Text;

using AISRS.Common.Framework;
using AISRS.Common.Exception;

namespace AISRS.DataAccess
{
	/// <summary>
	/// DataAccessBase ��DataAccess��������DA��ĸ��࣬��װ�˲������ݿ��һЩ��������
	/// </summary>
	public abstract class DataAccessBase : IDisposable
	{	
		#region �ܱ����ı���

		/// <summary>
		/// ���Ӵ�
		/// </summary>
		protected string _connectionString;
		
		// <summary>
		/// ͬ�����ݵ�Adapter
		/// </summary>
		protected OracleDataAdapter _dataAdapter;

		/// <summary>
		/// ���ݿ�����
		/// </summary>
		protected OracleConnection _connection;

		/// <summary>
		/// �������
		/// </summary>
		protected OracleTransaction _transaction;
	
		#endregion

		#region �ͷ���Դ

		/// <summary>
		/// �ͷŶ������Դ
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); // as a service to those who might inherit from us
		}

		/// <summary>
		///	�ͷŶ����е�ʵ��
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if(this._transaction != null)
				{
					this.QuitTransaction();
					this._transaction = null;
				}

				if (this._dataAdapter != null)
				{
					if(this._dataAdapter.SelectCommand != null)
					{    
						if(this._dataAdapter.SelectCommand.Connection != null )
							this._dataAdapter.SelectCommand.Connection.Dispose();
						this._dataAdapter.SelectCommand.Dispose();
					}    
				
					if(this._dataAdapter.InsertCommand != null)
					{    
						if(this._dataAdapter.InsertCommand.Connection != null )
							this._dataAdapter.InsertCommand.Connection.Dispose();
						this._dataAdapter.InsertCommand.Dispose();
					}    

					if(_dataAdapter.UpdateCommand != null)
					{    
						if (this._dataAdapter.UpdateCommand.Connection != null )
							this._dataAdapter.UpdateCommand.Connection.Dispose();
						this._dataAdapter.UpdateCommand.Dispose();
					}    

					if(_dataAdapter.DeleteCommand != null)
					{    
						if (this._dataAdapter.DeleteCommand.Connection != null )
							this._dataAdapter.DeleteCommand.Connection.Dispose();
						this._dataAdapter.DeleteCommand.Dispose();
					}    

					this._dataAdapter.Dispose();
					this._dataAdapter = null;
				}

				if ( this._connection != null) 
				{
					this._connection.Dispose();
					this._connection = null;
				}

			}
		}

		#endregion

		#region ������������
		/// <summary>
		/// ��������
		/// </summary>
		~DataAccessBase()
		{
			Dispose(false);
		}


		/// <summary>
		/// ���캯��
		/// </summary>		
		public DataAccessBase()
		{
			this._connectionString = Configuration.DataAccessConnectionString;
			this._connection = new OracleConnection(this._connectionString);
			this._dataAdapter = new OracleDataAdapter();			
		}
		#endregion

		#region �ܱ����ķ���

		/// <summary>
		/// CommandBuilder����ָ���Ĳ�ѯ����Զ������ĸ����ݿ������Command��
		/// Ȼ�����DataSet��DataTable�еļ�¼״̬�����ݿ���ִ�в����޸Ļ���ɾ������
		/// </summary>
		/// <param name="dataTable">�����������ݵ�DataTable����</param>
		/// <param name="tableName">���ݿ��н����޸����ݵı������</param>		
		protected virtual void AutoUpdate(
			DataTable dataTable,
			string tableName		
			)
		{
			this.AutoUpdate(dataTable,tableName,"*");
		}

		/// <summary>
		/// CommandBuilder����ָ���Ĳ�ѯ����Զ������ĸ����ݿ������Command��
		/// Ȼ�����DataSet��DataTable�еļ�¼״̬�����ݿ���ִ�в����޸Ļ���ɾ������
		/// </summary>
		/// <param name="dataTable">����Ҫ�����ݵ�DataTable����</param>
		/// <param name="tableName">���ݿ��н����޸����ݵı������</param>
		/// <param name="fieldNameList">���޸ĵ���������б����磺"ID,Name,Description"</param>
		protected virtual void AutoUpdate(
			DataTable dataTable,
			string tableName,
			string fieldNameList
			)
		{
			try
			{
				string sqlQuery = "SELECT " + fieldNameList + " FROM " + tableName + " WHERE ROWNUM <= 0";
				if(this._transaction != null)
				{
					this._dataAdapter.SelectCommand = new OracleCommand( sqlQuery,this._transaction.Connection);
					OracleCommandBuilder commandBuilder = new OracleCommandBuilder(this._dataAdapter);
					
					this._dataAdapter.SelectCommand.Transaction = this._transaction;

					this._dataAdapter.Update(dataTable);
				}
				else
				{
					this._dataAdapter.SelectCommand = new OracleCommand( sqlQuery,this._connection);
					OracleCommandBuilder commandBuilder = new OracleCommandBuilder(this._dataAdapter);

					this._dataAdapter.Update(dataTable);
				}				
			}
			catch(Exception ex)
			{
				string message = "In AutoUpdate. Table name  is '" + tableName + "', Field name list is '" + fieldNameList + "'. " + ex.Message;
				throw new DbOperationException(message);
			}
			finally
			{
				if (this._dataAdapter != null)
				{
					if(this._dataAdapter.SelectCommand != null)
					{
						this._dataAdapter.SelectCommand.Dispose();
						this._dataAdapter.SelectCommand = null;
					}
				
					if(this._dataAdapter.InsertCommand != null)
					{
						this._dataAdapter.InsertCommand.Dispose();
						this._dataAdapter.InsertCommand = null;
					}

					if(_dataAdapter.UpdateCommand != null)
					{
						this._dataAdapter.UpdateCommand.Dispose();
						this._dataAdapter.UpdateCommand = null;
					}

					if(_dataAdapter.DeleteCommand != null)
					{
						this._dataAdapter.DeleteCommand.Dispose();
						this._dataAdapter.DeleteCommand = null;
					}
				}
			}
		}

		/// <summary>
		/// ���Բ�����ͻ��ǿ�и��±�����
		/// </summary>
		/// <param name="dataTable">�����������ݵ�DataTable����</param>
		/// <param name="tableName">���ݿ��н����޸����ݵı������</param>
		protected virtual void ForceUpdate(
			DataTable dataTable,
			string tableName)
		{
			// ����sql���
			string sql = string.Empty;			
			foreach(DataRow row in dataTable.Rows)
			{
				switch(row.RowState)
				{
					case DataRowState.Added:
						sql = CreateInsertSql(row,tableName);
						this.ExecuteNonQuerySql(sql.ToString());
						break;
					case DataRowState.Deleted:
						sql = CreateDeleteSql(row,tableName);
						this.ExecuteNonQuerySql(sql.ToString());
						break;
					case DataRowState.Modified:
						sql = CreateUpdateSql(row,tableName);
						this.ExecuteNonQuerySql(sql.ToString());
						break;
					default:
						break;
				}
			}			
		}
		
		/// <summary>
		/// ����ָ���Ĳ�ѯ��䣬���ָ����DataSet�е�ָ���ı�
		/// </summary>
		/// <param name="dataTable">��������ݵ�DataTable����</param>
		/// <param name="sqlQuery">SQL��ѯ���</param>		
		protected virtual void AutoFill( 
			DataTable dataTable,
			string sqlQuery)
		{
			try
			{
				this._dataAdapter.SelectCommand = CreateCommand(sqlQuery);
				this._dataAdapter.Fill(dataTable);				
			}
			catch(Exception ex)
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
		/// ִ���ô洢���̣����ָ����DataSet�е�ָ���ı�
		/// </summary>
		/// <param name="dataTable">��������ݵ�DataTable����</param>
		/// <param name="procName">�洢��������</param>
		/// <param name="parameters">��������</param>	
		protected virtual void AutoFill( 
			DataTable dataTable,
			string procName,
			OracleParameter[] parameters
			)
		{
			try
			{
				this._dataAdapter.SelectCommand = CreateCommand(procName,parameters);
				this._dataAdapter.Fill(dataTable);			
			}
			catch(Exception ex)
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
		/// ִ��һ���ǲ�ѯ��sql���
		/// </summary>
		/// <param name="sql">Ҫִ�е�Oracle���</param>		
		protected virtual void ExecuteNonQuerySql(string sql)
		{
			OracleCommand cmd = CreateCommand(sql);
			ExecuteNonQueryCommand(cmd);
		}


		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="paramName">��������</param>
		/// <param name="dbType">��������</param>
		/// <param name="size">����ֵ����</param>
		/// <param name="paramValue">����ֵ</param>
		/// <returns>���ɵĲ�������</returns>
		protected virtual OracleParameter MakeInParam(string paramName, OracleType dbType, int size, object paramValue) 
		{
			return MakeParam(paramName, dbType, size, ParameterDirection.Input, paramValue);
		}		

		/// <summary>
		/// �����������
		/// </summary>
		/// <param name="paramName">��������</param>
		/// <param name="dbType">��������</param>
		/// <param name="size">����Ĳ���ֵ��󳤶�</param>		
		/// <returns>���ɵĲ�������</returns>
		protected virtual OracleParameter MakeOutParam(string paramName, OracleType dbType, int size) 
		{
			return MakeParam(paramName, dbType, size, ParameterDirection.Output, null);
		}		

		/// <summary>
		/// ���ɴ洢���̲���
		/// </summary>
		/// <param name="paramName">��������</param>
		/// <param name="dbType">��������</param>
		/// <param name="size">����ֵ����</param>
		/// <param name="direction">��������򴫳�����</param>
		/// <param name="paramValue">����ֵ</param>
		/// <returns>���ɵĲ�������</returns>
		protected virtual OracleParameter MakeParam(string paramName, OracleType dbType, int size, ParameterDirection direction, object paramValue) 
		{
			OracleParameter param;

			if(size > 0)
				param = new OracleParameter(paramName, dbType, size);
			else
				param = new OracleParameter(paramName, dbType);

			param.Direction = direction;
			if (!(direction == ParameterDirection.Output && paramValue == null))
				param.Value = paramValue;

			return param;
		}

		/// <summary>
		/// ����һ��SELECT_CURSOR���͵Ĳ������������ؼ�¼��
		/// </summary>
		/// <returns>SELECT_CURSOR���͵Ĳ������������ؼ�¼��</returns>
		protected virtual OracleParameter MakeSelectCursorParam()
		{
			return new OracleParameter("p_CURSOR",OracleType.Cursor,0,ParameterDirection.Output,true,0,0,"",DataRowVersion.Default, Convert.DBNull);
		}
		/// <summary>
		/// ִ�д洢����
		/// </summary>
		/// <param name="procName">�洢��������</param>		
		protected virtual void ExecuteProc(string procName)
		{
			OracleCommand cmd = CreateCommand(procName, null);
			this.ExecuteNonQueryCommand(cmd);
		}

		/// <summary>
		/// ִ�д洢����
		/// </summary>
		/// <param name="procName">�洢��������</param>
		/// <param name="prams">��������</param>				
		protected virtual void ExecuteProc(string procName, OracleParameter[] prams)
		{
			OracleCommand cmd = CreateCommand(procName, prams);
			this.ExecuteNonQueryCommand(cmd);
		}


		/// <summary>
		/// ִ�д洢���̣��ô�����DataReader���󱣴�洢���̷��ص�����
		/// </summary>
		/// <param name="procName">�洢��������</param>
		/// <param name="prams">��������</param>
		/// <param name="dataReader">�������ݵ�DataReader����<</param>
		protected virtual void ExecuteProc(string procName, OracleParameter[] prams, out OracleDataReader dataReader) 
		{
			OracleCommand cmd = CreateCommand(procName, prams);
			if(cmd.Connection.State == ConnectionState.Closed)
				cmd.Connection.Open();
			dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);			
				
		}

		/// <summary>
		/// ִ��һ��SQL��䣬��ȡ�ò�ѯ�����һ�С���һ�е�ֵ
		/// </summary>
		/// <param name="sql">SQL���</param>
		/// <returns>��ѯ�����һ�С���һ�е�ֵ</returns>
		protected virtual object GetScalar(string sql)
		{
			OracleCommand cmd = CreateCommand(sql);
			return ExecuteScalarCommand(cmd);
		}
		
		/// <summary>
		/// ִ�д洢���̣���ȡ�ò�ѯ�����һ�С���һ�е�ֵ
		/// </summary>
		/// <param name="procName">�洢��������</param>
		/// <param name="prams">��������</param>
		/// <returns>��ѯ�����һ�С���һ�е�ֵ</returns>
		protected virtual object GetScalar(string procName,OracleParameter[] prams)
		{
			OracleCommand cmd = CreateCommand(procName,prams);
			return ExecuteScalarCommand(cmd);
		}

		#endregion

		#region ���з���

		/// <summary>
		/// ��������ִ�д˷����󣬶����������ݿ�Ĳ������ڲ���������н���
		/// </summary>
		/// <param name="transaction">Ҫ���������</param>
		public void JoinTransaction(Transaction transaction)
		{
			if(this._transaction != null)
			{
				throw new  DbOperationException("�����Ѿ�����һ��������");
			}
			else
			{				
				this._transaction = transaction.DataBaseTransaction;
			}
		}

		/// <summary>
		/// �˳�����ִ�д˷����󣬶����������ݿ�Ĳ����������κ������н���	
		/// </summary>
		public void QuitTransaction()
		{
			if(this._transaction != null)
			{
				// ȷ��_dataAdapter����ĸ���������Ӳ�����������ӣ�����Dispose _dataAdapter����ʱ������ر���������ӡ�
				if (this._dataAdapter != null)
				{
					if(this._dataAdapter.SelectCommand != null)
					{    
						if(this._dataAdapter.SelectCommand.Connection != null )
							if(this._dataAdapter.SelectCommand.Connection.Equals(this._transaction.Connection))
								this._dataAdapter.SelectCommand.Connection = null;
					}    
				
					if(this._dataAdapter.InsertCommand != null)
					{    
						if(this._dataAdapter.InsertCommand.Connection != null )
							if(this._dataAdapter.InsertCommand.Connection.Equals(this._transaction.Connection))
								this._dataAdapter.InsertCommand.Connection = null;						
					}    

					if(_dataAdapter.UpdateCommand != null)
					{    
						if (this._dataAdapter.UpdateCommand.Connection != null )
							if(this._dataAdapter.UpdateCommand.Connection.Equals(this._transaction.Connection))
								this._dataAdapter.UpdateCommand.Connection = null;
					}    

					if(_dataAdapter.DeleteCommand != null)
					{    
						if (this._dataAdapter.DeleteCommand.Connection != null )
							if(this._dataAdapter.DeleteCommand.Connection.Equals(this._transaction.Connection))
								this._dataAdapter.DeleteCommand.Connection = null;								
					}    					
				}

				//ȷ������������Ӳ�����������ӣ����Ȿ����Disposeʱ������ر���������ӡ�
				if(this._connection != null)
				{
					if(this._connection.Equals(this._transaction.Connection))
						this._connection = null;
				}

				//
				this._transaction = null;
			}
		}

		/// <summary>
		/// ���ַ������ʵ��Ĵ���������SQL����п������ַ��������ַ���
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public string StringToSQL(string input)
		{
			// TODO: 
			string output = input.Replace("'","''");
			return output;
		}

		/// <summary>
		/// �������ݿ�����Server��ǰ��ʱ��
		/// </summary>
		/// <returns></returns>
		public DateTime GetServerTime()
		{
			// TODO: Oracle��ȡ�õ�ǰʱ��ĺ���
			string sql = "SELECT SYSDATE FROM DUAL";
			object dt = this.GetScalar(sql);
			return DateTime.Parse(dt.ToString());
		}

		#endregion

		#region ˽�з���

		/// <summary>
		/// ����һ��ִ�д洢���̵�Command����
		/// </summary>
		/// <param name="procName">�洢��������</param>
		/// <param name="prams">�����洢���̵Ĳ�������</param>		
		/// <returns>Command ����</returns>
		private OracleCommand CreateCommand(string procName, OracleParameter[] prams)
		{
			
			OracleCommand cmd = new OracleCommand();
			cmd.CommandText = procName;
			if(this._transaction == null)
			{
				cmd.Connection= this._connection;
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
				foreach (OracleParameter parameter in prams)
					cmd.Parameters.Add(parameter);
			}
			
			// return param

			// Oracle�Ĵ洢���̲������κ�ֵ
			//			cmd.Parameters.Add(
			//				new OracleParameter("ReturnValue", OracleType.Int32, 4,
			//				ParameterDirection.ReturnValue, false, 0, 0,
			//				string.Empty, DataRowVersion.Default, null));

			return cmd;
		}


		/// <summary>
		/// ����һ��ִ��SQL����Command����
		/// </summary>
		/// <param name="sql">Ҫִ�е�SQL���</param>			
		/// <returns>Command ����</returns>
		private OracleCommand CreateCommand(string sql)
		{
			
			OracleCommand cmd = new OracleCommand();
			cmd.CommandText = sql;
			if(this._transaction == null)
			{
				cmd.Connection= this._connection;
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
		/// ִ�зǲ�ѯ��������
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		private void ExecuteNonQueryCommand(OracleCommand cmd)
		{
			try
			{
				if(cmd.Connection.State == ConnectionState.Closed)
					cmd.Connection.Open();
				
				cmd.ExecuteNonQuery();					
			}
			catch(Exception ex)
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
		/// ִ��ȡ�õ�һ�С���һ�н���Ĳ�ѯ����
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		private object ExecuteScalarCommand(OracleCommand cmd)
		{
			try
			{
				if(cmd.Connection.State == ConnectionState.Closed)
					cmd.Connection.Open();
				
				return cmd.ExecuteScalar();	
			}
			catch(Exception ex)
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
		/// ���������е�����������Where���
		/// </summary>
		/// <param name="primaryKey"></param>
		/// <param name="row"></param>
		/// <returns>Where���</returns>
		private string CreateWhereClause(DataRow row)
		{
			bool isDeleteRow = false;
			if(row.RowState == DataRowState.Deleted)
			{
				isDeleteRow = true;
				row.RejectChanges();
			}

			DataColumn[] primaryKey = row.Table.PrimaryKey;
			if(primaryKey == null || primaryKey.Length == 0)
			{
				if(isDeleteRow)
					row.Delete();
				throw new ValidationException("Error in 'CreateWhereClause',No primary key in datarow object.");				
			}
			
			StringBuilder whereClause = new StringBuilder();
			for(int i=0; i<primaryKey.Length-1;i++)
			{
				whereClause.Append(primaryKey[i].ColumnName + " = '" + this.StringToSQL(row[primaryKey[i].ColumnName].ToString()) + "' AND "); 
			}
			whereClause.Append(primaryKey[primaryKey.Length-1].ColumnName + " = '" + this.StringToSQL(row[primaryKey[primaryKey.Length-1].ColumnName].ToString()) + "'"); 
						
			if(isDeleteRow)
				row.Delete();

			return whereClause.ToString();
		}

		/// <summary>
		/// ���������У�����Insert���
		/// </summary>
		/// <param name="row"></param>
		/// <param name="tableName"></param>
		/// <returns>Insert���</returns>
		private string CreateInsertSql(DataRow row,string tableName)
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("INSERT INTO " + tableName + " (" );

			for(int i=0;i<row.Table.Columns.Count;i++)
			{
				sql.Append(row.Table.Columns[i].ColumnName + ", ");
			}

			// cut ", "
			sql.Remove(sql.Length - 2, 2);
			sql.Append(")");

			sql.Append(" VALUES (");

			for(int i=0;i<row.Table.Columns.Count;i++)
			{
				if(row.IsNull(i))
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
		/// ���������У�����Update���
		/// </summary>
		/// <param name="row"></param>
		/// <param name="tableName"></param>
		/// <returns>Update���</returns>
		private string CreateUpdateSql(DataRow row,string tableName)
		{
			if(row.Table.Columns.Count == row.Table.PrimaryKey.Length)
				return "";

			StringBuilder sql = new StringBuilder();
			sql.Append("UPDATE " + tableName + " SET " );
			for(int i=0;i<row.Table.Columns.Count;i++)
			{
				//�ж��Ƿ�Ϊ������
				bool isPrimayKeyColumn = false;
				foreach(DataColumn primaryKeyColumn in row.Table.PrimaryKey)
				{
					if(row.Table.Columns[i].Equals(primaryKeyColumn))
					{
						isPrimayKeyColumn = true;
						break;
					}
				}


				if(!isPrimayKeyColumn)
				{
					if(row.IsNull(i))
						sql.Append(row.Table.Columns[i].ColumnName + " = NULL, ");									
					else
						sql.Append(row.Table.Columns[i].ColumnName + " = '" + this.StringToSQL(row[i].ToString()) + "', ");									
				}
				
			}
			
			// cut ", "
			sql.Remove(sql.Length - 2,2);

			sql.Append(" WHERE " + CreateWhereClause(row));
			return sql.ToString();
		}

		/// <summary>
		/// ���������У�����Delete���
		/// </summary>
		/// <param name="row"></param>
		/// <param name="tableName"></param>
		/// <returns>Delete���</returns>
		private string CreateDeleteSql(DataRow row,string tableName)
		{		
			return "DELETE FROM " + tableName + " WHERE " + CreateWhereClause(row);	
		}

		#endregion

	}
}
