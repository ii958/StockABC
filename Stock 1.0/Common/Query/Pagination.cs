using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// Pagination ��ժҪ˵����
	/// �������ݷ�ҳ��Ϣ
	/// һ��PageNumber��PageSize��Ϊ��ѯ�Ĳ���������
	/// TotalRecordCount��Ϊ��ѯ��������á�
	/// </summary>
	public class Pagination
	{
		#region ����
		private int _pageNumber = -1;
		private int _pageSize = -1;
		private int _totalRecordCount = -1;
		#endregion

		#region ����
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
		
		#region ������������
		public Pagination(int pageNumber,int pageSize)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
			TotalRecordCount = -1;
		}
		#endregion
		
	}
}
