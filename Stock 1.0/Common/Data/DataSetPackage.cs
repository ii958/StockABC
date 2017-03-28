using System;

using System.Data;
using System.Collections;

namespace AISRS.Common.Data
{
	/// <summary>
	/// DataSet�ļ���
	/// </summary>
	[Serializable()]
	public class DataSetPackage : CollectionBase
	{
		/// <summary>
		/// ������
		/// </summary>
		public DataSet this[ int index ]  
		{
			get  
			{
				return( (DataSet) List[index] );
			}
			set  
			{
				List[index] = value;
			}
		}
	

		/// <summary>
		/// ͨ��DataSet�����ƣ��Ӽ����л�ȡDataSet
		/// </summary>
		/// <param name="dataSetName">Ҫ��ȡDataSet������</param>
		/// <returns>
		/// �����а���ָ�����Ƶ�DataSet		��DataSet��ʵ��
		/// �����в�����ָ�����Ƶ�DataSet	��null
		/// </returns>
		public DataSet GetDataSet(string dataSetName)
		{
			foreach(DataSet ds in List)
			{
				if(ds.DataSetName == dataSetName)
					return ds;
			}
			return null;
		}


		/// <summary>
		/// �Ӽ������Ƴ�ָ�����Ƶ�DataSet
		/// </summary>
		/// <param name="dataSetName">���Ƴ�DataSet������</param>
		/// <returns>
		/// �����а���ָ�����Ƶ�DataSet		��DataSet��ʵ��
		/// �����в�����ָ�����Ƶ�DataSet	��null
		/// </returns>
		public DataSet RemoveDataSet(string dataSetName)
		{

			foreach(DataSet ds in List)
			{
				if(ds.DataSetName == dataSetName)
				{
					this.Remove(ds);
					return ds;
				}
			}
			return null;
		}


		/// <summary>
		/// �򼯺������һ��DataSet
		/// </summary>
		/// <param name="value">����ӵ�DataSet</param>
		/// <returns>��Ԫ�صĲ���λ��</returns>
		public int Add( DataSet value )  
		{
			return( List.Add( value ) );
		}

		/// <summary>
		/// ��ȡָ��DataSet������
		/// </summary>
		/// <param name="value">ָ����DataSet</param>
		/// <returns>ָ��DataSet������</returns>
		public int IndexOf( DataSet value )  
		{
			return( List.IndexOf( value ) );
		}


		/// <summary>
		/// �򼯺���ָ����λ�ò���һ���µ�DataSet
		/// </summary>
		/// <param name="index">�����λ��</param>
		/// <param name="value">�����DataSet</param>
		public void Insert( int index, DataSet value )  
		{
			List.Insert( index, value );
		}


		/// <summary>
		/// �Ӽ������Ƴ�һ��DataSet
		/// </summary>
		/// <param name="value">���Ƴ���DataSet</param>
		public void Remove( DataSet value )  
		{
			List.Remove( value );
		}


		/// <summary>
		/// ȷ���������Ƿ�����ض���ֵ
		/// </summary>
		/// <param name="value">ָ����DataSet</param>
		/// <returns>�Ƿ����</returns>
		public bool Contains( DataSet value )  
		{
			// If value is not of type DataSet, this will return false.
			return( List.Contains( value ) );
		}


		protected override void OnInsert( int index, Object value )  
		{
			if ( value.GetType().BaseType != typeof(System.Data.DataSet) )
				throw new ArgumentException( "value must be of type DataSet.", "value" );
		}


		protected override void OnRemove( int index, Object value )  
		{
			if ( value.GetType().BaseType != typeof(System.Data.DataSet) )
				throw new ArgumentException( "value must be of type DataSet.", "value" );
		}


		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
			if ( newValue.GetType().BaseType != typeof(System.Data.DataSet) )
				throw new ArgumentException( "newValue must be of type DataSet.", "newValue" );
		}


		protected override void OnValidate( Object value )  
		{
			if ( value.GetType().BaseType != typeof(System.Data.DataSet) )
				throw new ArgumentException( "value must be of type DataSet." );
		}

	}
}

