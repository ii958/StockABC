using System;

using System.Data;
using System.Collections;

namespace AISRS.Common.Data
{
	/// <summary>
	/// DataSet的集合
	/// </summary>
	[Serializable()]
	public class DataSetPackage : CollectionBase
	{
		/// <summary>
		/// 索引器
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
		/// 通过DataSet的名称，从集合中获取DataSet
		/// </summary>
		/// <param name="dataSetName">要获取DataSet的名称</param>
		/// <returns>
		/// 集合中包含指定名称的DataSet		：DataSet的实例
		/// 集合中不包含指定名称的DataSet	：null
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
		/// 从集合中移除指定名称的DataSet
		/// </summary>
		/// <param name="dataSetName">被移除DataSet的名称</param>
		/// <returns>
		/// 集合中包含指定名称的DataSet		：DataSet的实例
		/// 集合中不包含指定名称的DataSet	：null
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
		/// 向集合中添加一个DataSet
		/// </summary>
		/// <param name="value">被添加的DataSet</param>
		/// <returns>新元素的插入位置</returns>
		public int Add( DataSet value )  
		{
			return( List.Add( value ) );
		}

		/// <summary>
		/// 获取指定DataSet的索引
		/// </summary>
		/// <param name="value">指定的DataSet</param>
		/// <returns>指定DataSet的索引</returns>
		public int IndexOf( DataSet value )  
		{
			return( List.IndexOf( value ) );
		}


		/// <summary>
		/// 向集合中指定的位置插入一个新的DataSet
		/// </summary>
		/// <param name="index">插入的位置</param>
		/// <param name="value">插入的DataSet</param>
		public void Insert( int index, DataSet value )  
		{
			List.Insert( index, value );
		}


		/// <summary>
		/// 从集合中移除一个DataSet
		/// </summary>
		/// <param name="value">被移除的DataSet</param>
		public void Remove( DataSet value )  
		{
			List.Remove( value );
		}


		/// <summary>
		/// 确定集合中是否包含特定的值
		/// </summary>
		/// <param name="value">指定的DataSet</param>
		/// <returns>是否包含</returns>
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

