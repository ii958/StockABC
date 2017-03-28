namespace AISRS.WebUI.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Text;



	/// <summary>
	///		DatePicker 的摘要说明。
	/// </summary>
	public class DatePicker : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.TextBox textBoxDate;
		protected System.Web.UI.WebControls.ImageButton imageButtonSelectDate;
		protected System.Web.UI.WebControls.Label labelScript;
		private string _applicationPath;

		private void Page_Load(object sender, System.EventArgs e)
		{
			_applicationPath = Request.ApplicationPath == "/" ? "":Request.ApplicationPath;
			imageButtonSelectDate.ImageUrl = _applicationPath+"/Images/DatePicker.gif";
			
			this.RegisteScript();
			
			this.imageButtonSelectDate.Attributes["onClick"]= "javascript:ShowDatePicker('"+textBoxDate.ClientID+"');return false;";
			this.textBoxDate.Attributes["onblur"] = "javascript:CheckDate(this)";
		}

		/// <summary>
		/// 注册JavaScript脚本
		/// </summary>
		private void RegisteScript()
		{
			string script = @" 
					function StringTrim(theString)
						{
							var checkedAll = false;
							var checkString = theString;
							while ((!checkedAll) && (checkString.length != 0))
							{
								// cut off head space
								if (checkString.indexOf(' ') == 0)
								{
									checkString = checkString.substring(1);
									continue;
								}
								
								//  cut off tail space
								if (checkString.lastIndexOf(' ') == (checkString.length - 1))
								{
									checkString = checkString.substring(0,checkString.length - 1);
									continue;
								}
								
								checkedAll = true;
							}
							return checkString;
					}

					function DatePickerCheckDate(theObj)
					{
						if (theObj == null)
							return true;
						if(StringTrim(theObj.value) == '')
						{
							return true;
						}
						
						var inPutValue = StringTrim(theObj.value).split(' ');

						var theValue = inPutValue[0];

						var theData = theValue.split('-');	

						if (theData.length != 3)
						{
							return false;
						}
						
					
						// Check is number
						if (isNaN(theData[0]) || isNaN(theData[1]) || isNaN(theData[2]))
						{
							return false;
						}
						if(theData[0]=='' || theData[1]=='' || theData[2]=='')
						{
							return false;
						}
						if(theData[0]==null || theData[1]==null || theData[2]==null)
						{
							return false;
						}
						// Check Year
						var year = parseInt(theData[0], 10);
						if (year < 1900 || year > 9999)
						{
							return false;
						}

						// Check Month
						var month = parseInt(theData[1], 10);
						if (month < 1 || month > 12)
						{
							return false;
						}

						// Check day
						var dayLength = 0;		
						switch (month)
						{
							case 2:
								if ((year%4==0)&&((year%100!=0)||(year%400==0)))
								{
									dayLength = 29;
								}
								else
								{
									dayLength = 28;
								}
								break;
							case 1:
							case 3:
							case 5:
							case 7:
							case 8:
							case 10:
							case 12:
								dayLength = 31;
								break;
							case 4:
							case 6:
							case 9:
							case 11:
								dayLength = 30;
								break;
						}
						var day = parseInt(theData[2], 10);
						if (day < 1 || day > dayLength)
						{
							return false;
						}
					
						var theTime ;
					
						var inPutLength = inPutValue.length;
					
						if(inPutLength>1)	
						{
							theTime = inPutValue[inPutLength-1].split(':');
					
							if (isNaN(theTime[0]) || isNaN(theTime[1]) || isNaN(theTime[2]))
							{
								return false;
							}
							if(theTime[0]=='' || theTime[1]=='' || theTime[2]=='')
							{
								return false;
							}
							if(theTime[0]==null || theTime[1]==null || theTime[2]==null)
							{
						
								return false;
							}

							var h = parseInt(theTime[0], 10);
						
							if(h<0 || h>=24)
							{
								return false;
							}
						
							var m =  parseInt(theTime[1], 10);
							if(m<0 || m>=60)
							{
								return false;
							}
						
							var s =  parseInt(theTime[2], 10);
							if(s <0 || s>=60)
							{
								return false;
							}
						
						}
						return true;
					} " ;

			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<script language=\"javascript\">\r\n");
			stringBuilder.Append(script);
			stringBuilder.Append("var datePickerWindow;\r\n");
			stringBuilder.Append("function ShowDatePicker(sender)\r\n");
			stringBuilder.Append("{\r\n");
			stringBuilder.Append("var url = \""+_applicationPath+"/DatePicker/DatePicker.aspx?Sender=\"+sender;\r\n");
			stringBuilder.Append("if(datePickerWindow == null || datePickerWindow.closed)\r\n");
			stringBuilder.Append("{\r\n");
			stringBuilder.Append("datePickerWindow = window.open(url,\"SelectDate\",\"width=260,height=260,top=200,left=270,Status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no\");");
			stringBuilder.Append("datePickerWindow.focus();\r\n");
			stringBuilder.Append("}\r\n");
			stringBuilder.Append("else\r\n");
			stringBuilder.Append("{\r\n");
			stringBuilder.Append("datePickerWindow.focus();\r\n");
			stringBuilder.Append("}\r\n");
			stringBuilder.Append("}\r\n");
			//构造检查输入的日期格式是否正确的javascript函数
			stringBuilder.Append("function CheckDate(obj)\r\n");
			stringBuilder.Append("{\r\n");
			stringBuilder.Append("try\r\n");
			stringBuilder.Append("{\r\n");
			stringBuilder.Append("if(DatePickerCheckDate(obj) == false)\r\n");
			stringBuilder.Append("{\r\n");
			stringBuilder.Append("alert('日期格式错误,正确的格式为yyyy-MM-dd!');\r\n");

//			stringBuilder.Append("document.all."+textBoxDate.ClientID+".value='';\r\n");
//			stringBuilder.Append("document.all."+textBoxDate.ClientID+".focus();\r\n");

			stringBuilder.Append("obj.value='';\r\n");
			stringBuilder.Append("obj.focus();\r\n");

			stringBuilder.Append("}\r\n");
			stringBuilder.Append("}\r\n");
			stringBuilder.Append("catch(e)\r\n");
			stringBuilder.Append("{\r\n");
			stringBuilder.Append("}\r\n");
			stringBuilder.Append("}\r\n");
			stringBuilder.Append("</script>");

			if(!this.Page.IsClientScriptBlockRegistered("DatePickerScript"))
			this.Page.RegisterClientScriptBlock("DatePickerScript",stringBuilder.ToString());
		}

		/// <summary>
		/// 日期文本框的长度
		/// </summary>
		public Unit Width
		{
			get
			{
				return this.textBoxDate.Width;
			}
			set
			{
				this.textBoxDate.Width = value;
			}
		}

		/// <summary>
		/// 日期是否只读
		/// </summary>
		public bool ReadOnly
		{
			get
			{
				return this.textBoxDate.ReadOnly;
			}
			set
			{
				this.textBoxDate.ReadOnly = value;
			}
		}

		/// <summary>
		/// 可输入的最大字符数
		/// </summary>
		public int MaxLength
		{
			get
			{
				return this.textBoxDate.MaxLength;
			}
			set
			{
				this.textBoxDate.MaxLength = value;
			}
		}

		/// <summary>
		/// 选择的日期
		/// </summary>
		public string DateTime
		{
			get
			{
				return this.textBoxDate.Text;
			}
			set
			{
				this.textBoxDate.Text = value;
			}

		}
		
		public int TextBoxWidth
		{
			set
			{
				this.textBoxDate.Width = value;
			}
		}

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		设计器支持所需的方法 - 不要使用代码编辑器
		///		修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
