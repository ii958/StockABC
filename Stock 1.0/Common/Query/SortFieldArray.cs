using System;

using System.Collections;
using AISRS.Common.Query;

namespace AISRS.Common.Query
{
	/// <summary>
	/// SortFieldCollection ��ժҪ˵����
	/// </summary>
	public class SortFieldArray		
	{
		#region ����
		private ArrayList _sortFields;
		#endregion

		#region ����		
		public int Count
		{
			get { return _sortFields.Count; }
		}
		#endregion

		#region ����
		/// <summary>
		/// ���һ�������ֶ�
		/// </summary>
		/// <param name="sortField">����ӵ�SortField����</param>
		/// <returns>����ӵ�SortField����</returns>
		public void Add(SortField sortField)
		{
			_sortFields.Add(sortField);			
		}

		/// <summary>
		/// ��ȡIndex����Ԫ��
		/// </summary>
		/// <param name="index"></param>
		public SortField Get(int index)
		{
			return (SortField)_sortFields[index];
		}

		/// <summary>
		/// ��ָ��λ��ɾ��һ��Ԫ��
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
		/// �������Ԫ��
		/// </summary>
		public void Clear()
		{
			_sortFields.Clear();
		}

		/// <summary>
		/// ��������ֶ����ֺ����ݿ��е��ֶ�����һ�£�
		/// ���ô˷�������Order By ��������ַ���
		/// </summary>
		/// <returns>Sql ����� Order By ��������ַ���</returns>
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

		#region ������
		SortField this[int index]
		{
			get { return (SortField)_sortFields[index];}			
		}
		#endregion


		public SortFieldArray()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
			_sortFields = new ArrayList();			
		}
		
		

		
	}
}
