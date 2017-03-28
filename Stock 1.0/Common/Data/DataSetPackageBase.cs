using System;
using System.Text;
using System.Collections;
using System.Data;

namespace AISRS.Common.Data
{	
	/// <summary>
	/// DataSetPackage ��ժҪ˵����
	/// ������DataSet��Ҫһ����Ϊ�����򷵻�ֵ���ͣ�����Ӵ���̳У���ʵ��Dispose������
	/// ʾ���μ�SomeDataPackage.cs 
	/// </summary>	
	public class DataSetPackageBase : IDisposable
	{
	
		#region ����
		private NameDataSetCollection _nameDataSetCollection;
		#endregion
	
		#region ��������
		public DataSetPackageBase()
		{
			_nameDataSetCollection = new NameDataSetCollection();
		}
		#endregion

		/// <summary>
		/// ���һ���µ�DataSet�ൽ�����У�����key����
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		protected void Add(string key, DataSet value)
		{
			_nameDataSetCollection.Add(key,value);
		}

		/// <summary>
		/// ��ȡ��key��ص�DataSet�������������û���ṩ��key,����null
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected DataSet Get(string key)
		{
			return _nameDataSetCollection.Get(key);
		}

		/// <summary>
		/// �ͷ�DataSetArrayռ�õ���Դ
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
