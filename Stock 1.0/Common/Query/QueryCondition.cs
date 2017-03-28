using System;
using System.Text;
using System.Collections.Specialized;

namespace AISRS.Common.Query 
{
	/// <summary>
	/// QueryConfition ��ժҪ˵����
	/// �����ѯ���������ֺ�ֵ����ʵ��һЩ������ʹ�õĲ���
	/// </summary>
	public class QueryCondition : IDisposable
	{		
		private const string _divStr = "|~-_*_-~|";
		private const string _nameTag = "_name:";
		private const string _valueTag = "_value:";
		private NameValueCollection _nameValueCollection;


		public QueryCondition()
		{
			this._nameValueCollection = new NameValueCollection();
		}
		/// <summary>
		/// �Ѳ�ѯ�������л����ַ���
		/// </summary>
		/// <returns></returns>
		public string SerializeToString()
		{
			StringBuilder sb = new StringBuilder();
			for(int i=0;i<this._nameValueCollection.Count;i++)
			{
				sb.Append(_nameTag + this._nameValueCollection.GetKey(i));
				sb.Append(_valueTag + this._nameValueCollection.GetValues(i)[0]);
				sb.Append(_divStr);
			}
			return sb.ToString();
		}

		/// <summary>
		/// ���ַ����з����л�����ѯ����
		/// </summary>
		/// <param name="conditionsString">�������л���Ϣ���ַ���</param>
		/// <returns></returns>
		public void DeserializeFromString(string conditionsString)
		{
			this._nameValueCollection.Clear();

			int posBegin = 0;
			int posEnd = 0;

			posEnd = conditionsString.IndexOf(_divStr,posBegin);
			while(posEnd > 0)
			{
				string strNameValue = conditionsString.Substring(posBegin,posEnd - posBegin);
				posBegin = posEnd + _divStr.Length;
				posEnd = conditionsString.IndexOf(_divStr,posBegin);

				int nameBegin = strNameValue.IndexOf(_nameTag) + _nameTag.Length;
				int nameEnd = strNameValue.IndexOf(_valueTag);

				int valueBegin = nameEnd + _valueTag.Length;

				string strName = "";
				string strValue = "";
				if(nameBegin <= nameEnd)
					strName = strNameValue.Substring(nameBegin,nameEnd - nameBegin);
				else
					continue;
				if(valueBegin > 0)
					strValue = strNameValue.Substring(valueBegin);
				else
					continue;
				
				this._nameValueCollection.Add(strName,strValue);
			}
		}

		/// <summary>
		/// �Ѳ�ѯ������� SQL Server ��ѯ����е�where �־䡣
		/// ǰ�᣺ ��ֲ�Լ����е����ֱ�����SQL ����е��ֶ�����
		/// ת����ԭ��
		///		1. ��������ʾ���ֶκ�ֵ����ȹ�ϵ
		///		2. ֵΪ�մ�ʱ������Ϊһ��Where����
		/// </summary>
		/// <returns></returns>
		protected string ToWhereClause()
		{
			StringBuilder sb = new StringBuilder();			
			for(int i=0;i<this._nameValueCollection.Count;i++)
			{
				if(this._nameValueCollection.GetValues(i)[0] != string.Empty)
				{
					sb.Append(" AND ");
					sb.Append(this._nameValueCollection.GetKey(i) + " = '");
					sb.Append(this._nameValueCollection.GetValues(i)[0].Replace("'","''") + "' ");
				}				
			}
			if(sb.Length > 0)
				sb.Remove(0,4);
			return sb.ToString();
		}	
	
		/// <summary>
		/// �����ַ���������ֵ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		protected string GetCondition(string name,string defaultValue)
		{
			string returnValue = this._nameValueCollection[name] as string;
			if( returnValue == null)	
				return defaultValue;
			else
				return returnValue;
		}

		/// <summary>
		/// �����ַ���������ֵ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected void SetCondition(string name,string value)
		{
			this._nameValueCollection[name] = value;
		}


		/// <summary>
		/// ����int������ֵ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		protected int GetCondition(string name, int defaultValue)
		{
			string returnValue = this._nameValueCollection[name] as string;
			if( returnValue == null)	
				return defaultValue;
			else
				return int.Parse(returnValue);
		}

		/// <summary>
		/// ����int������ֵ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected void SetCondition(string name,int value)
		{
			this._nameValueCollection[name] = value.ToString();
		}
	

		/// <summary>
		/// ����decimal������ֵ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		protected decimal GetCondition(string name, decimal defaultValue)
		{
			string returnValue = this._nameValueCollection[name] as string;
			if( returnValue == null)	
				return defaultValue;
			else
				return decimal.Parse(returnValue);
		}

		/// <summary>
		/// ����decimal������ֵ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected void SetCondition(string name,decimal value)
		{
			this._nameValueCollection[name] = value.ToString();
		}
		
		/// <summary>
		/// ����float������ֵ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		protected float GetCondition(string name, float defaultValue)
		{
			string returnValue = this._nameValueCollection[name] as string;
			if( returnValue == null)	
				return defaultValue;
			else
				return float.Parse(returnValue);
		}

		/// <summary>
		/// ���� float ������ֵ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected void SetCondition(string name,float value)
		{
			this._nameValueCollection[name] = value.ToString();
		}

		/// <summary>
		/// ����double������ֵ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		protected double GetCondition(string name, double defaultValue)
		{
			string returnValue = this._nameValueCollection[name] as string;
			if( returnValue == null)	
				return defaultValue;
			else
				return double.Parse(returnValue);
		}

		/// <summary>
		/// ���� double ������ֵ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected void SetCondition(string name,double value)
		{
			this._nameValueCollection[name] = value.ToString();
		}

		/// <summary>
		/// ����DateTime������ֵ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		protected DateTime GetCondition(string name, DateTime defaultValue)
		{
			string returnValue = this._nameValueCollection[name] as string;
			if( returnValue == null)	
				return defaultValue;
			else
				return DateTime.Parse(returnValue);
		}

		/// <summary>
		/// ���� DateTime ������ֵ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected void SetCondition(string name,DateTime value)
		{
			this._nameValueCollection[name] = value.ToString();
		}

		/// <summary>
		/// ����Guid������ֵ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		protected Guid GetCondition(string name, Guid defaultValue)
		{
			string returnValue = this._nameValueCollection[name] as string;
			if( returnValue == null)	
				return defaultValue;
			else
			{
				if(returnValue == string.Empty)
					return Guid.Empty;
				else
					return new Guid(returnValue);
			}
		}

		/// <summary>
		/// ���� Guid ������ֵ
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected void SetCondition(string name,Guid value)
		{
			if(value == Guid.Empty)
				this._nameValueCollection[name] = string.Empty;
			else
				this._nameValueCollection[name] = value.ToString();
		}
		
		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			if(this._nameValueCollection != null)
				this._nameValueCollection.Clear();
		}

	}
}
