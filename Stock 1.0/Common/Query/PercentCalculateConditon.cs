using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// PercentCalculateConditon 的摘要说明。
	/// </summary>
	public class PercentCalculateConditon : QueryCondition
	{
		public string YearMonth
		{
			get { return this.GetCondition("YearMonthFrom",""); }
			set { this.SetCondition("YearMonthFrom",value); } 
		}	

		public string YearMonthTo
		{
			get { return this.GetCondition("YearMonthTo",""); }
			set { this.SetCondition("YearMonthTo",value); } 
		}

		public string ProjectCodeShort
		{
			get { return this.GetCondition("ProjectCodeShort","");}
			set { this.SetCondition("ProjectCodeShort",value);}
		}

        public string COMPANY
		{
            get { return this.GetCondition("COMPANY", ""); }
            set { this.SetCondition("COMPANY", value); }
		}

        public string SBU
        {
            get { return this.GetCondition("SBU", ""); }
            set { this.SetCondition("SBU", value); }
        }

		public string Task
		{
			get{return this.GetCondition("Task","");}
			set{this.SetCondition("Task",value);}
		}

		public decimal CurrentUserID
		{
			get { return this.GetCondition("CurrentUserID",0);}
			set { this.SetCondition("CurrentUserID",value);}
		}

		public int ReCalculateParameter
		{
			get { return this.GetCondition("ReCalculateParameter",0);}
			set { this.SetCondition("ReCalculateParameter",value);}
		}
		public string Company
		{
			get { return this.GetCondition("Company","");}
			set { this.SetCondition("Company",value);}
		}
	}
}
