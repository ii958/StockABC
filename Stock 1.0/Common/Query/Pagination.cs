using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// Pagination 的摘要说明。
	/// 用来传递分页信息
	/// 一般PageNumber和PageSize作为查询的参数传入用
	/// TotalRecordCount作为查询结果传出用。
	/// </summary>
	public class Pagination
	{
		#region 变量
		private int _pageNumber = -1;
		private int _pageSize = -1;
		private int _totalRecordCount = -1;
		#endregion

		#region 属性
		public int PageNumber
		{
			get { return _pageNumber; }
			set { _pageNumber = value;}
		}

		public int PageSize
		{
			get { return _pageSize; }
			set { _pageSize = value;}
		}
		public int TotalRecordCount
		{
			get { return _totalRecordCount; }
			set {_totalRecordCount = value; }
		}

		public int PageCount
		{
			get
			{ 
				if(_totalRecordCount < 0)
					return -1;
				if(_pageSize <= 0)
					return -1;

				int pageCount = _totalRecordCount/_pageSize;
				if(_totalRecordCount % _pageSize != 0)
					pageCount += 1;

				return pageCount;
			}				
		}
		#endregion
		
		#region 构造析构函数
		public Pagination(int pageNumber,int pageSize)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
			TotalRecordCount = -1;
		}
		#endregion
		
	}
}
