using System;
using System.Text;
using System.Collections.Specialized;

namespace AISRS.Common.Query 
{
	/// <summary>
	/// QueryConfition 的摘要说明。
	/// 保存查询条件的名字和值，并实现一些简化条件使用的操作
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
		/// 把查询条件序列化到字符串
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
		/// 从字符串中反序列化出查询条件
		/// </summary>
		/// <param name="conditionsString">包含序列化信息的字符串</param>
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
		/// 把查询条件变成 SQL Server 查询语句中的where 字句。
		/// 前提： 名植对集合中的名字必须是SQL 语句中的字段名。
		/// 转换的原则：
		///		1. 名字所表示的字段和值是相等关系
		///		2. 值为空串时，不作为一个Where条件
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
		/// 返回字符串型条件值
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
		/// 设置字符串型条件值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected void SetCondition(string name,string value)
		{
			this._nameValueCollection[name] = value;
		}


		/// <summary>
		/// 返回int型条件值
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
		/// 设置int型条件值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected void SetCondition(string name,int value)
		{
			this._nameValueCollection[name] = value.ToString();
		}
	

		/// <summary>
		/// 返回decimal型条件值
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
		/// 设置decimal型条件值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected void SetCondition(string name,decimal value)
		{
			this._nameValueCollection[name] = value.ToString();
		}
		
		/// <summary>
		/// 返回float型条件值
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
		/// 设置 float 型条件值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected void SetCondition(string name,float value)
		{
			this._nameValueCollection[name] = value.ToString();
		}

		/// <summary>
		/// 返回double型条件值
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
		/// 设置 double 型条件值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected void SetCondition(string name,double value)
		{
			this._nameValueCollection[name] = value.ToString();
		}

		/// <summary>
		/// 返回DateTime型条件值
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
		/// 设置 DateTime 型条件值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected void SetCondition(string name,DateTime value)
		{
			this._nameValueCollection[name] = value.ToString();
		}

		/// <summary>
		/// 返回Guid型条件值
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
		/// 设置 Guid 型条件值
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
