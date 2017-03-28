using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace APC.WebUI.DatePicker
{
	/// <summary>
	/// DataPicker 的摘要说明。
	/// </summary>
	public class DataPicker : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Calendar CalendarDate;
		protected System.Web.UI.WebControls.Label LabelScript;

		protected System.Web.UI.WebControls.DropDownList DropDownListYear;
		protected System.Web.UI.WebControls.DropDownList DropDownListMonth;
		protected System.Web.UI.WebControls.ImageButton ImagebuttonPrevMonth;
		protected System.Web.UI.WebControls.ImageButton ImageButtonNextMonth;

		protected string _selctedDate; //选中的日期
		protected string _sender;      //日期输入框的ID
		private int _minYear ;         //年最小值
		private int _maxYear ;         //年最大值

	
		private void Page_Load(object sender, System.EventArgs e)
		{
			LabelScript.Text ="";
			_sender =  Request["Sender"] == null?String.Empty:Request["Sender"];

			_minYear = 1988;  //下拉列表中“年”最小值为1988
			_maxYear = DateTime.Now.AddYears(10).Year;  //下拉列表中“年”最大值为当前年＋10

			if(!IsPostBack)
			{
				//设置日期控件显示的月份的日期
				CalendarDate.VisibleDate = DateTime.Now;

				//初始年、月下拉列表
				this.SetYear(_minYear,_maxYear);
				this.SetMonth();

				//年月下拉列表定位到当前年、月
				this.SelectDate(
					DateTime.Now.Year.ToString(),
					DateTime.Now.Month.ToString());
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
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    
			this.ImagebuttonPrevMonth.Click += new System.Web.UI.ImageClickEventHandler(this.ImagebuttonPrevMonth_Click);
			this.DropDownListYear.SelectedIndexChanged += new System.EventHandler(this.DropDownListYear_SelectedIndexChanged);
			this.DropDownListMonth.SelectedIndexChanged += new System.EventHandler(this.DropDownListMonth_SelectedIndexChanged);
			this.ImageButtonNextMonth.Click += new System.Web.UI.ImageClickEventHandler(this.ImageButtonNextMonth_Click);
			this.CalendarDate.SelectionChanged += new System.EventHandler(this.CalendarDate_SelectionChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// 选择的日期变化时调用javascript更新日期输入框中的日期值
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CalendarDate_SelectionChanged(object sender, System.EventArgs e)
		{
			_selctedDate = CalendarDate.SelectedDate.ToString("yyyy-MM-dd");
			LabelScript.Text = "<script>DatePickerSetDate('"+_sender+"','"+_selctedDate+"');</script>";

		}

		/// <summary>
		/// 点下一月按钮时触发事件，
		/// 设置日期控件显示的年月，
		/// 并且根据显示的年、月定位年、月下拉列表框
		/// 如果显示的年份超出了年下拉框的范围（大于当前年＋10）
		/// 则更新年下拉列表框中的值（1988到日期控件显示的年）
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImageButtonNextMonth_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			SetVisibleDate(CalendarDate.VisibleDate.AddMonths(1));

			if(CalendarDate.VisibleDate.Year >_maxYear)
			{
				this.SetYear(_minYear,CalendarDate.VisibleDate.Year);
			}

			this.SelectDate(
				CalendarDate.VisibleDate.Year.ToString(),
				CalendarDate.VisibleDate.Month.ToString());


		}

		/// <summary>
		/// 点上一月按钮时触发事件，
		/// 设置日期控件显示的年月，
		/// 并且根据显示的年、月定位年、月下拉列表框
		/// 如果显示的年份超出了年下拉框的范围（小于1988）
		/// 则更新年下拉列表框中的值（日期控件显示的年到当前年＋10）
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ImagebuttonPrevMonth_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			SetVisibleDate(CalendarDate.VisibleDate.AddMonths(-1));

			if(CalendarDate.VisibleDate.Year <_minYear)
			{
				this.SetYear(CalendarDate.VisibleDate.Year,_maxYear);
			}

			this.SelectDate(
				CalendarDate.VisibleDate.Year.ToString(),
				CalendarDate.VisibleDate.Month.ToString());


		}

		private void DropDownListYear_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int year = int.Parse(this.DropDownListYear.SelectedItem.Value);
			int month = int.Parse(this.DropDownListMonth.SelectedItem.Value);
			DateTime visibleDate = new DateTime(year,month,1);
			this.SetVisibleDate(visibleDate);

		}

		private void DropDownListMonth_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int year = int.Parse(this.DropDownListYear.SelectedItem.Value);
			int month = int.Parse(this.DropDownListMonth.SelectedItem.Value);
			DateTime visibleDate = new DateTime(year,month,1);
			this.SetVisibleDate(visibleDate);

		}

		/// <summary>
		/// 初始化“年”下拉列表
		/// </summary>
		/// <param name="minYear">年最小值</param>
		/// <param name="maxYear">年最大值</param>
		private void SetYear(int minYear,int maxYear)
		{
			this.DropDownListYear.Items.Clear();

			for(int i=minYear;i<=maxYear;i++)
			{
				this.DropDownListYear.Items.Add(
					new ListItem(i.ToString(),i.ToString()));
			}

		}

		/// <summary>
		/// 初始化“月”下拉列表
		/// </summary>
		private void SetMonth()
		{
			this.DropDownListMonth.Items.Clear();

			for(int i=1;i<=12;i++)
			{
				this.DropDownListMonth.Items.Add(
					new ListItem(i.ToString(),i.ToString()));
			}
			
		}

		/// <summary>
		/// 设置日期控件显示的月份
		/// </summary>
		/// <param name="visibleDate">要显示月份的日期</param>
		private void SetVisibleDate(DateTime visibleDate)
		{
			CalendarDate.VisibleDate = visibleDate;
		}

		/// <summary>
		/// 根据年、月定位年、月下拉列表
		/// </summary>
		/// <param name="year">要定位的年</param>
		/// <param name="month">要定位的月</param>
		private void SelectDate(string year,string month)
		{
			if(DropDownListYear.Items.Count ==0 || DropDownListMonth.Items.Count == 0)
			{
				return;
			}

			this.DropDownListYear.SelectedIndex = 
				this.DropDownListYear.Items.IndexOf(
				this.DropDownListYear.Items.FindByValue(year));

			this.DropDownListMonth.SelectedIndex = 
				this.DropDownListMonth.Items.IndexOf(
				this.DropDownListMonth.Items.FindByValue(month));

		}

		
	}
}
