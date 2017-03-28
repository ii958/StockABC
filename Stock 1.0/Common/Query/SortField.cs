using System;

namespace AISRS.Common.Query
{
	public enum SortType
	{
		Ascending = 0,
		Descending = 1
	}

	/// <summary>
	/// SortField 的摘要说明。
	/// </summary>
	public class SortField
	{
		#region 变量
		private string _name;
		private SortType _sortType;
		#endregion

		#region 属性
		public string Name
		{
			get { return _name; }
		}
		public SortType SortType
		{
			get { return _sortType; }
		}		
		#endregion
		
		#region 构造析构函数
		public SortField(string name, SortType sortType)
		{
			this._name = name;
			this._sortType = sortType;
		}
		#endregion
	}
}
