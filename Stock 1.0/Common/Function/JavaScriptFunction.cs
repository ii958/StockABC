using System;
using System.Text;
using System.Data;

namespace AISRS.Common.Function
{
	/// <summary>
	/// JavaScriptFunction ��ժҪ˵����
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
		/// ����״�ṹ��ֵ
		/// </summary>
		/// <param name="table">�������ݵ�DataTable�������и��е��������ź���ġ�</param>
		/// <param name="includeValue">Ϊ���ʾDataTable�����к�����ֵ��Ϊ�ٱ�ʾDataTable�����в�����value Field</param>
		/// <param name="rootName">���ͽṹ���ڵ������</param>
		/// <returns>һ��Javascript�ĸ�ֵ���</returns>
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
		/// ����״�ṹ��ֵ
		/// </summary>
		/// <param name="dataRows">�������ݵ�DataRow���ϣ�������DataRow���ǰ��е��Ⱥ��ź����</param>
		/// <param name="columnCount">ÿ�е�����</param>
		/// <param name="includeValue">Ϊ���ʾDataTable�����к�����ֵ��Ϊ�ٱ�ʾDataTable�����в�����value Field</param>
		/// <param name="rootName">���ͽṹ���ڵ������</param>
		/// <returns>һ��Javascript�ĸ�ֵ���</returns>
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
		/// ȥ���ַ���β���Ļس����ͻ��з�
		/// </summary>
		/// <param name="s">��Ҫ������ַ���</param>
		/// <returns>ȥ��β���س����ͻ��з�����ַ���</returns>
		private static string TrimEndCrLf(string s)
		{
			char[] CrLf = {'\r','\n'};
			return s.TrimEnd(CrLf);			
		}
		#endregion

	}
}
