namespace AISRS.WebUI.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using AISRS.Common;
	using AISRS.Common.Function;

	/// <summary>
	///		FNPeriod 的摘要说明。
	/// </summary>
	public abstract class HpFnPeriodPicker : System.Web.UI.UserControl
	{
		// attributes
		
		// width
		public double Width
		{
			get { return periodList.Width.Value; }
			set 
			{ 
				periodList.Style.Remove("width");
				periodList.Style.Add("width",value.ToString());
			}
		}
		
		//
		public DateTime PeriodBeginDate
		{
			get 
			{ 
				string period = periodList.SelectedItem.Value;
				char [] parsChar = {'|'};
				string[] dates = period.Split(parsChar);
				return DateTime.Parse(dates[0]); 
			}			
		}

		//
		public DateTime PeriodEndDate
		{
			get 
			{ 
				string period = periodList.SelectedItem.Value;
				char [] parsChar = {'|'};
				string[] dates = period.Split(parsChar);
				return DateTime.Parse(dates[1]); 
			}			
		}
		
		// value format {PeriodBeginDate}|{PeriodEndDate} example: 2001-1-1|2002-1-1
		public String Period
		{
			get { return periodList.SelectedItem.Value; }
			set 
			{ 
				periodList.SelectedValue = value;
				periodList_SelectedIndexChanged(periodList,null);
			}
		}
		
		//
		public bool AutoPostBack
		{
			get { return periodList.AutoPostBack; }
			set { periodList.AutoPostBack = value;}
		}

		//
		public bool Enabled
		{
			get { return periodList.Enabled; }
			set { periodList.Enabled = value;}
		}

		//Delegate 
		public delegate void PeriodChangedHandler(Object sender,EventArgs e);
		public event PeriodChangedHandler PeriodChanged = null;
		
		protected System.Web.UI.WebControls.DropDownList periodList;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
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
			this.periodList.SelectedIndexChanged += new System.EventHandler(this.periodList_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		// FY: Finance Year
		public void Initialize(int beginFY, int fyBeginMonth)
		{
			DateTime now = DateTime.Now;
			
			// calculate current finance year
			int currentFY;
			if(fyBeginMonth<=6)
			{
				if(now.Month < fyBeginMonth)
					currentFY = now.Year - 1;
				else
					currentFY = now.Year;
			}
			else
			{
				if(now.Month < fyBeginMonth)
					currentFY = now.Year;
				else
					currentFY = now.Year + 1;
				
			}

			// adjust begin FY
			if((currentFY - beginFY) < 3)
				beginFY = currentFY - 3;
					

			periodList.Items.Clear();
			int fy;	// finance year						
			int fq; // finance quarter
			int fm; // finance month

			// finance year only
			for(fy=beginFY;fy<=currentFY - 3;fy++)
			{
				AddFyItem(fy,fyBeginMonth);
			}

			//
			fy = currentFY - 2;			
			AddFyItem(fy,fyBeginMonth);									
            AddFhyItem(fy,fyBeginMonth,1);			
			for(fq=1;fq<=2;fq++)
				AddFqItem(fy,fyBeginMonth,fq);
					
			AddFhyItem(fy,fyBeginMonth,2);
			for(fq=3;fq<=4;fq++)
				AddFqItem(fy,fyBeginMonth,fq);

			
			// 
			fy = currentFY - 1;				
			AddFyItem(fy,fyBeginMonth);			
			AddFhyItem(fy,fyBeginMonth,1);
			AddFqItem(fy,fyBeginMonth,1);
			for(fm=1;fm<=3;fm++)
				AddFmItem(fy,fyBeginMonth,fm);

			AddFqItem(fy,fyBeginMonth,2);
			for(fm=4;fm<=6;fm++)
				AddFmItem(fy,fyBeginMonth,fm);
			
			AddFhyItem(fy,fyBeginMonth,2);
			AddFqItem(fy,fyBeginMonth,3);
			for(fm=7;fm<=9;fm++)
				AddFmItem(fy,fyBeginMonth,fm);

			AddFqItem(fy,fyBeginMonth,4);
			for(fm=10;fm<=12;fm++)
				AddFmItem(fy,fyBeginMonth,fm);

			//
			fy = currentFY;					
			int currentFhy = GetFhy(fyBeginMonth,now);
			int currentFq = GetFq(fyBeginMonth,now);
			int currentFm = GetFm(fyBeginMonth,now);			
			AddFyItem(fy,fyBeginMonth);			
			AddFhyItem(fy,fyBeginMonth,1);
			AddFqItem(fy,fyBeginMonth,1);			
			for(fm=1;fm<=Math.Min(3,currentFm);fm++)
				AddFmItem(fy,fyBeginMonth,fm);

			if(currentFq < 2)
				return;
			AddFqItem(fy,fyBeginMonth,2);			
			for(fm=4;fm<=Math.Min(6,currentFm);fm++)
				AddFmItem(fy,fyBeginMonth,fm);

			if(currentFhy < 2)
				return;
			AddFhyItem(fy,fyBeginMonth,2);
			AddFqItem(fy,fyBeginMonth,3);			
			for(fm=7;fm<=Math.Min(9,currentFm);fm++)
				AddFmItem(fy,fyBeginMonth,fm);

			if(currentFq<4)
				return;

			AddFqItem(fy,fyBeginMonth,4);			
			for(fm=10;fm<=Math.Min(12,currentFm);fm++)
				AddFmItem(fy,fyBeginMonth,fm);

		}

		// ----------------------------------------------------------------------------------
		private void periodList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(null != PeriodChanged)
				PeriodChanged(this,null);
		}

		// ----------------------------------------------------------------------------------
		private int GetFYBeginYear(int fy, int fyBeginMonth)
		{
			if(fyBeginMonth<=6)
				return fy;
			else
				return fy - 1;
		}

		// ----------------------------------------------------------------------------------
		private int GetFYEndYear(int fy, int fyBeginMonth)
		{
			if(fyBeginMonth<=6)
				return fy + 1 ;
			else
				return fy;
		}

		// ----------------------------------------------------------------------------------
		private int GetFhy(int fyBeginMonth, DateTime date)
		{
			int dateMonth = date.Month;

			if(dateMonth < fyBeginMonth)
				dateMonth += 12;

			return (int)((dateMonth - fyBeginMonth) / 6 + 1);			
		}

		// ----------------------------------------------------------------------------------
		private int GetFq(int fyBeginMonth, DateTime date)
		{
			int dateMonth = date.Month;

			if(dateMonth < fyBeginMonth)
				dateMonth += 12;

			return (int)((dateMonth - fyBeginMonth) / 3 + 1);	
		}

		// ----------------------------------------------------------------------------------
		private int GetFm(int fyBeginMonth, DateTime date)
		{
			int dateMonth = date.Month;

			if(dateMonth < fyBeginMonth)
				dateMonth += 12;

			return dateMonth - fyBeginMonth + 1;	
		}

		// ----------------------------------------------------------------------------------
		// add finance year item
		private void AddFyItem(int fy, int fyBeginMonth)
		{
			DateTime periodBeginDate = new DateTime(GetFYBeginYear(fy,fyBeginMonth),fyBeginMonth,1);
			DateTime periodEndDate = periodBeginDate.AddYears(1).AddDays(-1);

			ListItem li = new ListItem();					
			li.Text = "FY" + fy.ToString();
			li.Value = DateTimeFunction.GetDateString(periodBeginDate) + "|" + DateTimeFunction.GetDateString(periodEndDate);
			periodList.Items.Add(li);
		}

		// ----------------------------------------------------------------------------------
		// add finance half year item
		// fhy=1: 1st half year
		// fhy=2: 2nd half year
		private void AddFhyItem(int fy, int fyBeginMonth, int fhy)
		{
			DateTime periodBeginDate = new DateTime(GetFYBeginYear(fy,fyBeginMonth),fyBeginMonth,1);
			
			if(fhy==2)
                periodBeginDate = periodBeginDate.AddMonths(6);
			

            DateTime periodEndDate = periodBeginDate.AddMonths(6).AddDays(-1);

			ListItem li = new ListItem();					
			if(fhy==1)
				li.Text = "-1st Half Year";
			else
				li.Text = "-2nd Half Year";
			li.Text += " / FY" + fy.ToString();
			
			li.Value = DateTimeFunction.GetDateString(periodBeginDate) + "|" + DateTimeFunction.GetDateString(periodEndDate);
			periodList.Items.Add(li);
		}

		// ----------------------------------------------------------------------------------
		//add finance quarter item
		//fq value: 1 - 4
		private void AddFqItem(int fy, int fyBeginMonth, int fq)
		{
			DateTime periodBeginDate = (new DateTime(GetFYBeginYear(fy,fyBeginMonth),fyBeginMonth,1)).AddMonths((fq-1)*3);						
			DateTime periodEndDate = periodBeginDate.AddMonths(3).AddDays(-1);

			ListItem li = new ListItem();								
			li.Text = "--Q" + fq.ToString();
			li.Text += " / FY" + fy.ToString();
			
			li.Value = DateTimeFunction.GetDateString(periodBeginDate) + "|" + DateTimeFunction.GetDateString(periodEndDate);
			periodList.Items.Add(li);
		}

		// ----------------------------------------------------------------------------------
		//add finance month item
		// fm value: 1-12
		private void AddFmItem(int fy, int fyBeginMonth, int fm)
		{
			DateTime periodBeginDate = (new DateTime(GetFYBeginYear(fy,fyBeginMonth),fyBeginMonth,1)).AddMonths(fm-1);						
			DateTime periodEndDate = periodBeginDate.AddMonths(1).AddDays(-1);

			ListItem li = new ListItem();								
			li.Text = "---" + periodBeginDate.Year.ToString() + " / " + periodBeginDate.Month.ToString();
			
			li.Value = DateTimeFunction.GetDateString(periodBeginDate) + "|" + DateTimeFunction.GetDateString(periodEndDate);
			periodList.Items.Add(li);
		}
	}
}
