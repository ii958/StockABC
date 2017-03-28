using System;
using System.Text;
using System.Collections;
using System.Data;

namespace AISRS.Common.Data
{	
	/// <summary>
	/// DataSetPackage 的摘要说明。
	/// 如果多个DataSet需要一起作为参数或返回值传送，必须从此类继承，并实现Dispose方法。
	/// 示例参见SomeDataPackage.cs 
	/// </summary>	
	public class DataSetPackageBase : IDisposable
	{
	
		#region 变量
		private NameDataSetCollection _nameDataSetCollection;
		#endregion
	
		#region 构造析构
		public DataSetPackageBase()
		{
			_nameDataSetCollection = new NameDataSetCollection();
		}
		#endregion

		/// <summary>
		/// 添加一个新的DataSet类到集合中，并与key关联
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		protected void Add(string key, DataSet value)
		{
			_nameDataSetCollection.Add(key,value);
		}

		/// <summary>
		/// 获取和key相关的DataSet对象，如果集合中没有提供的key,返回null
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected DataSet Get(string key)
		{
			return _nameDataSetCollection.Get(key);
		}

		/// <summary>
		/// 释放DataSetArray占用的资源
		/// </summary>
		public void Dispose()
		{
			if(_nameDataSetCollection != null)		
			{
				for(int i=_nameDataSetCollection.Count - 1;i>=0; i--)
				{
					DataSet ds = _nameDataSetCollection.Get(i);					
					_nameDataSetCollection.Remove(i);
					if(ds != null)
						ds.Dispose();			
				}				
			}			
		}
	}
}
