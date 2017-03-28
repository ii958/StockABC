using System;
using System.Text;
using System.Data;

namespace AISRS.Common.Function
{
	/// <summary>
	/// JavaScriptFunction 的摘要说明。
	/// </summary>
	public class JavaScriptFunction
	{
		public static string AddScript(string nakedScript)
		{
			StringBuilder script = new StringBuilder();
			script.Append("<SCRIPT language=\"JavaScript\">\r\n");
			script.Append(nakedScript);
			script.Append("</SCRIPT>");
			return script.ToString();
		}

		public static string AddHrefScript(string nakedScript)
		{
			StringBuilder script = new StringBuilder();
			script.Append("javascript:");
			script.Append(nakedScript);
			return script.ToString();
		}

		public static string Alert(string s)
		{
			StringBuilder script = new StringBuilder();
			script.Append("<SCRIPT language=\"JavaScript\">\r\n");
			script.Append("window.alert('" + s.Replace("\'","\\\'").Replace("\n","\\n") + "');\r\n");
			script.Append("</SCRIPT>");
			return script.ToString();
		}

		public static string ParentAlert(string s)
		{
			StringBuilder script = new StringBuilder();
			script.Append("<SCRIPT language=\"JavaScript\">\r\n");
			script.Append("window.opener.alert('" + s.Replace("\'","\\\'").Replace("\n","\\n") + "');\r\n");
			script.Append("</SCRIPT>");
			return script.ToString();			
		}


		public static string Back()
		{
			StringBuilder script = new StringBuilder();
			script.Append("<SCRIPT language=\"JavaScript\">\r\n");
			script.Append("window.history.back()");
			script.Append("</SCRIPT>");
			return script.ToString();
		}

		public static string Close()
		{
			StringBuilder script = new StringBuilder();
			script.Append("<SCRIPT language=\"JavaScript\">\r\n");
			script.Append("window.close()");
			script.Append("</SCRIPT>");
			return script.ToString();
		}

		public static string Refresh()
		{
			StringBuilder script = new StringBuilder();
			script.Append("<SCRIPT language=\"JavaScript\">\r\n");
			script.Append("window.refresh();");
			script.Append("</SCRIPT>");
			return script.ToString();
		}

		public static string RefreshOpener()
		{
			StringBuilder script = new StringBuilder();
			script.Append("<SCRIPT language=\"JavaScript\">\r\n");
			script.Append("window.opener.Refresh();");
			script.Append("</SCRIPT>");
			return script.ToString();
		}	
		
		public static string RefreshOpenerAndClose()
		{
			StringBuilder script = new StringBuilder();
			script.Append("<SCRIPT language=\"JavaScript\">\r\n");
			script.Append("window.opener.refresh();");
			script.Append("window.close();");
			script.Append("</SCRIPT>");
			return script.ToString();
		}


		public static string GetArgumentString(params string[] param)
		{
			StringBuilder argString = new StringBuilder("");
			
			for(int i=0;i<param.Length - 1;i++)
			{
				string paramArg = "'" + param[i].Replace("\'","\\\'") + "'," ;			
				argString.Append(paramArg);
			}

			if(param.Length>0)
			{
				string paramArg = "'" + param[param.Length-1].Replace("\'","\\\'") + "'" ;			
				argString.Append(paramArg);
			}
 									
			return argString.ToString();
		}

		#region SetTreeData
		/// <summary>
		/// 给树状结构赋值
		/// </summary>
		/// <param name="table">包含数据的DataTable对象，其中各行的数据是排好序的。</param>
		/// <param name="includeValue">为真表示DataTable对象中含有数值域；为假表示DataTable对象中不含有value Field</param>
		/// <param name="rootName">树型结构根节点的名称</param>
		/// <returns>一组Javascript的赋值语句</returns>
		public static string SetTreeData(DataTable table, bool includeValue, string rootName)
		{
			if(table.Rows.Count == 0)
				return "";

			StringBuilder setDataScript = new StringBuilder();
			setDataScript.Append("var newItem;\r\n"); 
			setDataScript.Append("newItem = new jsoSelItem(\"\",\"\",null);\r\n");
			setDataScript.Append("var " + rootName + " = newItem;\r\n");
			setDataScript.Append("var curItem = newItem;\r\n");

			int i;
			int associatedSelectCount = table.Columns.Count;
			if(includeValue)
				associatedSelectCount /= 2;

			string[] texts = new string[associatedSelectCount];
			string[] values = new string[associatedSelectCount];

			for(i=0;i<associatedSelectCount;i++)
			{
				texts[i] = "";
				values[i] = "";
			}

			int colIndex = 0;
			int j;

			if(includeValue)
			{
				foreach(DataRow row in table.Rows)
				{
					for(i=0;i<associatedSelectCount;i++)
					{
						string aaa = row[i*2].ToString();
						string bbb = values[i].ToString();
						if(row[i*2].ToString()!=values[i])
						{
							if (colIndex > i)
							{
								setDataScript.Append("curItem = curItem.getParent(" + (colIndex-i).ToString() + ");\r\n"); 
								colIndex = i;
								for(j=i+1;j < associatedSelectCount; j++)
									values[j] = "";	
							}
							if (colIndex == i - 1)
							{
								setDataScript.Append("curItem = newItem;\r\n");
								colIndex = i;
							}
							
							setDataScript.Append("newItem = new jsoSelItem(\"" + TrimEndCrLf(row[i*2].ToString().Replace("\"","\\\"")) + "\",\"" + TrimEndCrLf(row[i*2+1].ToString().Replace("\"","\\\"")) + "\",curItem);\r\n");
							setDataScript.Append("curItem.addChild(newItem);\r\n");
							
							values[i] = row[i*2].ToString();
						}
					}
				}
			}
			else
			{
				foreach(DataRow row in table.Rows)
				{
					for(i=0;i<associatedSelectCount;i++)
					{
						if(row[i].ToString()!=texts[i])
						{
							if (colIndex > i)
							{
								setDataScript.Append("curItem = curItem.getParent(" + (colIndex-i).ToString() + ");\r\n"); 
								colIndex = i;
								for(j=i+1;j < associatedSelectCount; j++)
									texts[j] = "";
							}
							else
							{
								if (colIndex == i - 1)
								{
									setDataScript.Append("curItem = newItem;\r\n");
									colIndex = i;						
								}
							}
							setDataScript.Append("newItem = new jsoSelItem(\"" + TrimEndCrLf(row[i].ToString().Replace("\"","\\\"")) + "\",\"" + TrimEndCrLf(row[i].ToString().Replace("\"","\\\"")) + "\",curItem);\r\n");
							setDataScript.Append("curItem.addChild(newItem);\r\n");
							
							texts[i] = row[i].ToString();
						} // if((string)...
					}// for(i=0...)
				} // for each
			}// if
			return "<Script language=\"JavaScript\">\r\n" +  setDataScript.ToString() + "</Script>";
			//return setDataScript.ToString();
		}// end of function		

		/// <summary>
		/// 给树状结构赋值
		/// </summary>
		/// <param name="dataRows">包含数据的DataRow集合，数据在DataRow中是按列的先后排好序的</param>
		/// <param name="columnCount">每行的列数</param>
		/// <param name="includeValue">为真表示DataTable对象中含有数值域；为假表示DataTable对象中不含有value Field</param>
		/// <param name="rootName">树型结构根节点的名称</param>
		/// <returns>一组Javascript的赋值语句</returns>
		public static string SetTreeData(DataRow[] dataRows, int columnCount, bool includeValue, string rootName)
		{
			if(dataRows.Length == 0)
				return "";

			StringBuilder setDataScript = new StringBuilder();
			setDataScript.Append("var newItem;\r\n"); 
			setDataScript.Append("newItem = new jsoSelItem(\"\",\"\",null);\r\n");
			setDataScript.Append("var " + rootName + " = newItem;\r\n");
			setDataScript.Append("var curItem = newItem;\r\n");

			int i;
			int associatedSelectCount = columnCount;
			if(includeValue)
				associatedSelectCount /= 2;

			string[] texts = new string[associatedSelectCount];
			string[] values = new string[associatedSelectCount];

			for(i=0;i<associatedSelectCount;i++)
			{
				texts[i] = "";
				values[i] = "";
			}

			int colIndex = 0;
			int j;
			
			if(includeValue)
			{
				for(int rowIndex = 0;rowIndex < dataRows.Length; rowIndex++)
				{
					DataRow row = dataRows[rowIndex];

					for(i=0;i<associatedSelectCount;i++)					
					{
						if(row[i*2].ToString()!=values[i])
						{
							if (colIndex > i)
							{
								setDataScript.Append("curItem = curItem.getParent(" + (colIndex-i).ToString() + ");\r\n"); 
								colIndex = i;
								for(j=i+1;j < associatedSelectCount; j++)
									values[j] = "";	
							}
							if (colIndex == i - 1)
							{
								setDataScript.Append("curItem = newItem;\r\n");
								colIndex = i;
							}
							
							setDataScript.Append("newItem = new jsoSelItem(\"" + TrimEndCrLf(row[i*2].ToString().Replace("\"","\\\"")) + "\",\"" + TrimEndCrLf(row[i*2+1].ToString().Replace("\"","\\\"")) + "\",curItem);\r\n");
							setDataScript.Append("curItem.addChild(newItem);\r\n");
							
							values[i] = row[i*2].ToString();
						}
					}
				}
			}
			else
			{
				for(int rowIndex = 0; rowIndex < dataRows.Length; rowIndex++)
				{
					DataRow row = dataRows[rowIndex];
					for(i=0;i<associatedSelectCount;i++)
					{
						if(row[i].ToString()!=texts[i])
						{
							if (colIndex > i)
							{
								setDataScript.Append("curItem = curItem.getParent(" + (colIndex-i).ToString() + ");\r\n"); 
								colIndex = i;
								for(j=i+1;j < associatedSelectCount; j++)
									texts[j] = "";
							}
							else
							{
								if (colIndex == i - 1)
								{
									setDataScript.Append("curItem = newItem;\r\n");
									colIndex = i;						
								}
							}
							setDataScript.Append("newItem = new jsoSelItem(\"" + TrimEndCrLf(row[i].ToString().Replace("\"","\\\"")) + "\",\"" + TrimEndCrLf(row[i].ToString().Replace("\"","\\\"")) + "\",curItem);\r\n");
							setDataScript.Append("curItem.addChild(newItem);\r\n");
							
							texts[i] = row[i].ToString();
						} // if((string)...
					}// for(i=0...)
				} // for (int rowIndex = 0...)
			}// if
			return "<Script language=\"JavaScript\">\r\n" +  setDataScript.ToString() + "</Script>";
			//return setDataScript.ToString();
		}// end of function		

		/// <summary>
		/// 去掉字符串尾部的回车符和换行符
		/// </summary>
		/// <param name="s">需要处理的字符串</param>
		/// <returns>去掉尾部回车符和换行符后的字符串</returns>
		private static string TrimEndCrLf(string s)
		{
			char[] CrLf = {'\r','\n'};
			return s.TrimEnd(CrLf);			
		}
		#endregion

	}
}
