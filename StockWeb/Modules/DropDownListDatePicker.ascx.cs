namespace AISRS.WebUI.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		DropDownListDatePicker ��ժҪ˵����
	/// </summary>
	public class DropDownListDatePicker : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DropDownList yearList;
		protected System.Web.UI.WebControls.DropDownList dayList;
		protected System.Web.UI.WebControls.DropDownList monthList;

		#region ����
		// Date
		public DateTime Date
		{
			get 
			{
				return DateTime.Parse( yearList.SelectedValue + "-" 
					+ monthList.SelectedValue + "-" 
					+ dayList.SelectedValue);
			}
			set
			{				
				if(yearList.Items.Count == 0)
				{
					Initialize(DateTime.Now.Year - 10, DateTime.Now.Year + 10);
				}
				yearList.SelectedValue = value.Year.ToString();
				monthList.SelectedValue = value.Month.ToString();
				dayList.SelectedValue = value.Day.ToString();				
			}
		}

		// Enabled
		public bool Enabled
		{
			get { return yearList.Enabled;}
			set 
			{ 
				yearList.Enabled = value;
				monthList.Enabled = value;
				dayList.Enabled = value;				
			}
		}

		#endregion
		private void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(yearList.Items.Count == 0)
			{
				Initialize(DateTime.Now.Year - 10, DateTime.Now.Year + 10);
			}

			RegisterPublicScript();
			RegisterPrivateScript();
			yearList.Attributes.Add("onclick","javascript:change" + this.dayList.ClientID + "();");
			monthList.Attributes.Add("onclick","javascript:change" + this.dayList.ClientID + "();");
		}

		// ��ʼ�������б�
		public void Initialize(int beginYear,int endYear)
		{
			if(endYear < beginYear)
				return;

			yearList.Items.Clear();
			for(int i=beginYear;i<=endYear;i++ )
				yearList.Items.Add(new ListItem(i.ToString(),i.ToString()));
			
			monthList.Items.Clear();
			for(int i=1;i<=12;i++)
				monthList.Items.Add(new ListItem(i.ToString(),i.ToString()));

			dayList.Items.Clear();
			for(int i=1;i<=31;i++)
				dayList.Items.Add(new ListItem(i.ToString(),i.ToString()));
		}

		/// <summary>
		/// ע��ͬ��ؼ����õĽű�
		/// </summary>
		private void RegisterPublicScript()
		{
			string script = @"
				<script language=javascript>
					function saveDayOption(optionArray, objSel)
					{
						for(var day=29;day<=31;day++)
						{
							optionArray[day-29] = objSel.options[day-1];
						}	
					}
					
					function setMaxDay(objSel,maxDay,optionArray)
					{
						var oldMaxDay = parseInt(objSel.options[objSel.options.length-1].value);
						var selectedDay = objSel.value;
						if(maxDay == oldMaxDay)
							return;
						if(maxDay > oldMaxDay)
						{
							for(var day=oldMaxDay + 1; day<=maxDay; day++)
							{
								objSel.add(optionArray[day-29]);
							}
						}
						else
						{
							if(selectedDay > maxDay)
							{
								objSel.options[selectedDay - 1].selected = false;
								objSel.options[maxDay-1].selected = true;					
							}
							
							for(var day=oldMaxDay; day>=maxDay + 1; day--)
							{
								objSel.remove(day-1);
							}		
						}
						
					}


					//�õ�ĳ��ĳ������
					function getMaxDay(year,month)
					{
						switch (month)
						{
							case 2:
								if ((year%4==0)&&((year%100!=0)||(year%400==0)))
									return 29;
								else
									return 28;
							case 1:
							case 3:
							case 5:
							case 7:
							case 8:
							case 10:
							case 12:
								return 31;
							case 4:
							case 6:
							case 9:
							case 11:
								return 30;
						}
					}
				</script>
				";
				if(!this.Page.IsClientScriptBlockRegistered("DropDownListDataPickerScript"))
					this.Page.RegisterClientScriptBlock("DropDownListDataPickerScript",script);
		}

		/// <summary>
		/// ע��ֻ�б������õĽű�
		/// </summary>
		private void RegisterPrivateScript()
		{
			string script = "<script language=javascript>\r\n";
			
			script += "var " + this.ClientID + "_DayOptions = new Array();\r\n";
			script += "saveDayOption(" + this.ClientID + "_DayOptions, document.getElementById('" + this.dayList.ClientID + "')" + ");\r\n";
			script += "function change" + this.dayList.ClientID + "()\r\n";
			script += "{\r\n";
			script += "var yearList = document.getElementById('" + this.yearList.ClientID + "');\r\n";
			script += "var monthList = document.getElementById('" + this.monthList.ClientID + "');\r\n";
			script += "var dayList = document.getElementById('" + this.dayList.ClientID + "');\r\n";
			script += "var year = parseInt(yearList.value);\r\n";
			script += "var month = parseInt(monthList.value);\r\n";
			script += "var maxDay = getMaxDay(year,month);\r\n";
			script += "setMaxDay(dayList,maxDay," + this.ClientID + "_DayOptions)\r\n";
			script += "}\r\n";
			script += "change" + this.dayList.ClientID + "()\r\n";
			script += "</script>\r\n";

			this.Page.RegisterStartupScript(this.ClientID + "script",script);

		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
		///		�޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
