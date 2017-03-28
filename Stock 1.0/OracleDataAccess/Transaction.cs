using System;
using System.Data;
using System.Data.OracleClient;
using AISRS.Common.Framework;
using AISRS.Common.Exception;

namespace AISRS.DataAccess
{
	/// <summary>
	/// 封装事务和运行事务的连接
	/// </summary>
	public class Transaction : IDisposable
	{
		#region protected variables

		/// <summary>
		/// 链接串
		/// </summary>
		protected string _connectionString;

		/// <summary>
		/// 数据库链接
		/// </summary>
		protected OracleConnection _connection;


		/// <summary>
		/// 事务
		/// </summary>
		protected OracleTransaction _transaction;

		#endregion

		/// <summary>
		/// 返回事务对象
		/// </summary>
		public OracleTransaction DataBaseTransaction
		{
			get { return this._transaction;}
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		public Transaction()
		{
			this._connectionString = Configuration.DataAccessConnectionString;
			this._connection = new OracleConnection(this._connectionString);
			this._connection.Open();
			this._transaction = _connection.BeginTransaction();
		}

		/// <summary>
		/// 提交事务
		/// </summary>
		public void Commit()
		{
			this._transaction.Commit();			
		}

		/// <summary>
		/// 回滚事务
		/// </summary>
		public void RollBack()
		{
			this._transaction.Rollback();
		}

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
				if(_transaction != null)
					this._transaction.Dispose();

				if(_connection != null && _connection.State == ConnectionState.Open)
				{
					this._connection.Close();
					this._connection.Dispose();
				}
			}
		}

		/// <summary>
		/// 析构时释放资源
		/// </summary>
		~Transaction()
		{
			Dispose(false);
		}

	}
}
