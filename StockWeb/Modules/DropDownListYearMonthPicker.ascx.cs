namespace AISRS.WebUI.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		DropDownListYearMonthPicker ��ժҪ˵����
	/// </summary>
	public class DropDownListYearMonthPicker : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DropDownList yearList;
		protected System.Web.UI.WebControls.DropDownList monthList;

		#region ����
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
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(yearList.Items.Count == 0)
			{
				Initialize(DateTime.Now.Year - 10, DateTime.Now.Year + 10);
			}

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
				monthList.Items.Add(new ListItem(i.ToString("00"),i.ToString("00")));

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
