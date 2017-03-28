using System;

using System.Collections;
using AISRS.Common.Query;

namespace AISRS.Common.Query
{
	/// <summary>
	/// SortFieldCollection 的摘要说明。
	/// </summary>
	public class SortFieldArray		
	{
		#region 变量
		private ArrayList _sortFields;
		#endregion

		#region 属性		
		public int Count
		{
			get { return _sortFields.Count; }
		}
		#endregion

		#region 方法
		/// <summary>
		/// 添加一个排序字段
		/// </summary>
		/// <param name="sortField">被添加的SortField对象</param>
		/// <returns>被添加的SortField对象</returns>
		public void Add(SortField sortField)
		{
			_sortFields.Add(sortField);			
		}

		/// <summary>
		/// 获取Index处的元素
		/// </summary>
		/// <param name="index"></param>
		public SortField Get(int index)
		{
			return (SortField)_sortFields[index];
		}

		/// <summary>
		/// 从指定位置删除一个元素
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public SortField Remove(int index)
		{
			SortField sortField = (SortField)_sortFields[index];
			_sortFields.RemoveAt(index);
			return sortField;
		}

		/// <summary>
		/// 清空所有元素
		/// </summary>
		public void Clear()
		{
			_sortFields.Clear();
		}

		/// <summary>
		/// 如果排序字段名字和数据库中的字段名字一致，
		/// 可用此方法生成Order By 后的排序字符串
		/// </summary>
		/// <returns>Sql 语句中 Order By 后的排序字符串</returns>
		public string ToSqlOrderClause()
		{
			string orderClause = string.Empty;
			for(int i=0;i<_sortFields.Count;i++)
			{
				SortField sortField = (SortField)_sortFields[i];
				orderClause += sortField.Name + " ";
				if(sortField.SortType == SortType.Descending)
					orderClause += "DESC ";
				if(i < _sortFields.Count - 1)
					orderClause += ", ";
			}
			return orderClause;
		}
		#endregion

		#region 索引器
		SortField this[int index]
		{
			get { return (SortField)_sortFields[index];}			
		}
		#endregion


		public SortFieldArray()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
			_sortFields = new ArrayList();			
		}
		
		

		
	}
}
