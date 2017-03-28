using System;

namespace AISRS.Common.Query
{
	public enum SortType
	{
		Ascending = 0,
		Descending = 1
	}

	/// <summary>
	/// SortField ��ժҪ˵����
	/// </summary>
	public class SortField
	{
		#region ����
		private string _name;
		private SortType _sortType;
		#endregion

		#region ����
		public string Name
		{
			get { return _name; }
		}
		public SortType SortType
		{
			get { return _sortType; }
		}		
		#endregion
		
		#region ������������
		public SortField(string name, SortType sortType)
		{
			this._name = name;
			this._sortType = sortType;
		}
		#endregion
	}
}
