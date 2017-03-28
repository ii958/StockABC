using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

namespace AISRS.Common.Data
{
	/// <summary>
	/// NameDataSetCollection 的摘要说明
	/// 与键值关联的DataSet集合
	/// </summary>
	public class NameDataSetCollection : NameObjectCollectionBase  
	{
		// Gets or sets the value associated with the specified key.
		public DataSet this[ String key ]  
		{
			get  
			{
				return((DataSet)this.BaseGet( key ) );
			}
			set  
			{
				this.BaseSet( key, value );
			}
		}

		// Gets a String array that contains all the keys in the collection.
		public String[] AllKeys  
		{
			get  
			{
				return( this.BaseGetAllKeys() );
			}
		}

		// Gets an DataSet array that contains all the values in the collection.
		public Array AllValues  
		{
			get  
			{
				return( this.BaseGetAllValues() );
			}
		}

		// Gets a String array that contains all the values in the collection.
		public DataSet[] AllDataSetValues  
		{
			get  
			{
				return((DataSet[]) this.BaseGetAllValues(typeof(DataSet)));
			}
		}

		// Gets a value indicating if the collection contains keys that are not null.
		public Boolean HasKeys  
		{
			get  
			{
				return( this.BaseHasKeys() );
			}
		}

		// get a value at position pointout by index
		public DataSet Get(int index)
		{
			return (DataSet)this.BaseGet(index);
		}

		public DataSet Get(string name)
		{
			return (DataSet)this.BaseGet(name);
		}

		// Adds an entry to the collection.
		public void Add( String key, DataSet value )  
		{
			this.BaseAdd( key, value );
		}

		// Removes an entry with the specified key from the collection.
		public void Remove( String key )  
		{
			this.BaseRemove( key );
		}

		// Removes an entry in the specified index from the collection.
		public void Remove( int index )  
		{
			this.BaseRemoveAt( index );			
		}

		// Clears all the elements in the collection.
		public void Clear()  
		{
			this.BaseClear();
		}

	}

}
