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
	/// DataPicker ��ժҪ˵����
	/// </summary>
	public class DataPicker : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Calendar CalendarDate;
		protected System.Web.UI.WebControls.Label LabelScript;

		protected System.Web.UI.WebControls.DropDownList DropDownListYear;
		protected System.Web.UI.WebControls.DropDownList DropDownListMonth;
		protected System.Web.UI.WebControls.ImageButton ImagebuttonPrevMonth;
		protected System.Web.UI.WebControls.ImageButton ImageButtonNextMonth;

		protected string _selctedDate; //ѡ�е�����
		protected string _sender;      //����������ID
		private int _minYear ;         //����Сֵ
		private int _maxYear ;         //�����ֵ

	
		private void Page_Load(object sender, System.EventArgs e)
		{
			LabelScript.Text ="";
			_sender =  Request["Sender"] == null?String.Empty:Request["Sender"];

			_minYear = 1988;  //�����б��С��ꡱ��СֵΪ1988
			_maxYear = DateTime.Now.AddYears(10).Year;  //�����б��С��ꡱ���ֵΪ��ǰ�꣫10

			if(!IsPostBack)
			{
				//�������ڿؼ���ʾ���·ݵ�����
				CalendarDate.VisibleDate = DateTime.Now;

				//��ʼ�ꡢ�������б�
				this.SetYear(_minYear,_maxYear);
				this.SetMonth();

				//���������б�λ����ǰ�ꡢ��
				this.SelectDate(
					DateTime.Now.Year.ToString(),
					DateTime.Now.Month.ToString());
			}


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
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
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
		/// ѡ������ڱ仯ʱ����javascript��������������е�����ֵ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CalendarDate_SelectionChanged(object sender, System.EventArgs e)
		{
			_selctedDate = CalendarDate.SelectedDate.ToString("yyyy-MM-dd");
			LabelScript.Text = "<script>DatePickerSetDate('"+_sender+"','"+_selctedDate+"');</script>";

		}

		/// <summary>
		/// ����һ�°�ťʱ�����¼���
		/// �������ڿؼ���ʾ�����£�
		/// ���Ҹ�����ʾ���ꡢ�¶�λ�ꡢ�������б��
		/// �����ʾ����ݳ�������������ķ�Χ�����ڵ�ǰ�꣫10��
		/// ������������б���е�ֵ��1988�����ڿؼ���ʾ���꣩
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
		/// ����һ�°�ťʱ�����¼���
		/// �������ڿؼ���ʾ�����£�
		/// ���Ҹ�����ʾ���ꡢ�¶�λ�ꡢ�������б��
		/// �����ʾ����ݳ�������������ķ�Χ��С��1988��
		/// ������������б���е�ֵ�����ڿؼ���ʾ���굽��ǰ�꣫10��
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
		/// ��ʼ�����ꡱ�����б�
		/// </summary>
		/// <param name="minYear">����Сֵ</param>
		/// <param name="maxYear">�����ֵ</param>
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
		/// ��ʼ�����¡������б�
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
		/// �������ڿؼ���ʾ���·�
		/// </summary>
		/// <param name="visibleDate">Ҫ��ʾ�·ݵ�����</param>
		private void SetVisibleDate(DateTime visibleDate)
		{
			CalendarDate.VisibleDate = visibleDate;
		}

		/// <summary>
		/// �����ꡢ�¶�λ�ꡢ�������б�
		/// </summary>
		/// <param name="year">Ҫ��λ����</param>
		/// <param name="month">Ҫ��λ����</param>
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
