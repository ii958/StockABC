namespace AISRS.WebUI.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		DropDownListYearMonthPicker 的摘要说明。
	/// </summary>
	public class DropDownListYearMonthPicker : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DropDownList yearList;
		protected System.Web.UI.WebControls.DropDownList monthList;

		#region 属性
		// Date
		public string YearMonth
		{
			get 
			{
				return yearList.SelectedValue + monthList.SelectedValue;
			}
			set
			{				
				if(yearList.Items.Count == 0)
				{
					Initialize(DateTime.Now.Year - 10, DateTime.Now.Year + 10);
				}

				if(value == string.Empty)
				{
					yearList.Items.Add(new ListItem("----",""));
					monthList.Items.Add(new ListItem("--",""));
					yearList.SelectedValue = "";
					monthList.SelectedValue = "";
				}
				else
				{
					yearList.SelectedValue = value.ToString().Substring(0,4);
					monthList.SelectedValue = value.ToString().Substring(4,2);
				}
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
			}
		}

		#endregion
		private void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if(yearList.Items.Count == 0)
			{
				Initialize(DateTime.Now.Year - 10, DateTime.Now.Year + 10);
			}

		}

		// 初始化下拉列表
		public void Initialize(int beginYear,int endYear)
		{
			if(endYear < beginYear)
				return;

			yearList.Items.Clear();
			for(int i=beginYear;i<=endYear;i++ )
				yearList.Items.Add(new ListItem(i.ToString(),i.ToString()));
			
			monthList.Items.Clear();
			for(int i=1;i<=12;i++)
				monthList.Items.Add(new ListItem(i.ToString("00"),i.ToString("00")));

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
