namespace AISRS.WebUI.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		MonthModule 的摘要说明。
	/// </summary>
	public class MonthPicker : System.Web.UI.UserControl
	{
		// attributes
		
		// width
		public double Width
		{
			get { return monthList.Width.Value; }
			set 
			{ 
				monthList.Style.Remove("width");
				monthList.Style.Add("width",value.ToString());
			}
		}
		
		//
		public DateTime SelectedMonth
		{
			get { return DateTime.Parse(monthList.SelectedItem.Value); }
			set 
			{ 
				monthList.SelectedIndex = -1;
				string month = value.Year.ToString() + "-" + value.Month.ToString();
				for(int i=0;i<monthList.Items.Count;i++)
				{
					if(monthList.Items[i].Text == month)
					{
						monthList.SelectedIndex = i;
						break;
					}
				}
			}
		}
		
		//
		public bool AutoPostBack
		{
			get { return monthList.AutoPostBack; }
			set { monthList.AutoPostBack = value;}
		}

		//
		public bool Enabled
		{
			get { return monthList.Enabled; }
			set { monthList.Enabled = value;}
		}

		//Delegate 
		public delegate void MonthChangedHandler(Object sender,EventArgs e);
		public event MonthChangedHandler MonthChanged = null;

		protected System.Web.UI.WebControls.DropDownList monthList;

		private void Page_Load(object sender, System.EventArgs e)
		{			
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN：该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);			
		}
		
		///		设计器支持所需的方法 - 不要使用
		///		代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.monthList.SelectedIndexChanged += new System.EventHandler(this.monthList_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public void Initial(DateTime beginMonth, DateTime endMonth, DateTime selectedMonth)
		{
			if(beginMonth <= endMonth)
			{	
				monthList.Items.Clear();
				DateTime tempMonth = new DateTime(beginMonth.Year,beginMonth.Month,1);			
				while(tempMonth <= endMonth)
				{				
					ListItem li = new ListItem();					
					li.Text = tempMonth.Year.ToString() + "-" + tempMonth.Month.ToString();
					li.Value = tempMonth.Year.ToString() + "-" + tempMonth.Month.ToString() + "-1";
					if (tempMonth.Year == selectedMonth.Year && tempMonth.Month == selectedMonth.Month)
						li.Selected = true;
					
					monthList.Items.Add(li);
					tempMonth = tempMonth.AddMonths(1);
				}				
			}
		}

		private void monthList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(null != MonthChanged)
				MonthChanged(this,null);
		}		
	}
}
