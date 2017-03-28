using System;
using System.Data;
using System.Data.OracleClient;
using AISRS.Common.Framework;
using AISRS.Common.Exception;

namespace AISRS.DataAccess
{
	/// <summary>
	/// ��װ������������������
	/// </summary>
	public class Transaction : IDisposable
	{
		#region protected variables

		/// <summary>
		/// ���Ӵ�
		/// </summary>
		protected string _connectionString;

		/// <summary>
		/// ���ݿ�����
		/// </summary>
		protected OracleConnection _connection;


		/// <summary>
		/// ����
		/// </summary>
		protected OracleTransaction _transaction;

		#endregion

		/// <summary>
		/// �����������
		/// </summary>
		public OracleTransaction DataBaseTransaction
		{
			get { return this._transaction;}
		}

		/// <summary>
		/// ���캯��
		/// </summary>
		public Transaction()
		{
			this._connectionString = Configuration.DataAccessConnectionString;
			this._connection = new OracleConnection(this._connectionString);
			this._connection.Open();
			this._transaction = _connection.BeginTransaction();
		}

		/// <summary>
		/// �ύ����
		/// </summary>
		public void Commit()
		{
			this._transaction.Commit();			
		}

		/// <summary>
		/// �ع�����
		/// </summary>
		public void RollBack()
		{
			this._transaction.Rollback();
		}

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
		/// ����ʱ�ͷ���Դ
		/// </summary>
		~Transaction()
		{
			Dispose(false);
		}

	}
}
