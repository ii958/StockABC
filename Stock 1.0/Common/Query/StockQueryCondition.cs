using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISRS.Common.Query
{
    public class StockQueryCondition : QueryCondition
    {
        public string StockCode
        {
            get { return this.GetCondition("StockCode", ""); }
            set { this.SetCondition("StockCode", value); }
        }

        public string StockName
        {
            get { return this.GetCondition("StockName", ""); }
            set { this.SetCondition("StockName", value); }
        }

        public string Market
        {
            get { return this.GetCondition("Market", ""); }
            set { this.SetCondition("Market", value); }
        }

        public string Field
        {
            get { return this.GetCondition("Field", ""); }
            set { this.SetCondition("Field", value); }
        }

        public string FieldMain
        {
            get { return this.GetCondition("FieldMain", ""); }
            set { this.SetCondition("FieldMain", value); }
        }

        public string Province
        {
            get { return this.GetCondition("Province", ""); }
            set { this.SetCondition("Province", value); }
        }

        public string Chart
        {
            get { return this.GetCondition("Chart", ""); }
            set { this.SetCondition("Chart", value); }
        }

        public string Index
        {
            get { return this.GetCondition("Index", ""); }
            set { this.SetCondition("Index", value); }
        }

        public string BuyPoint
        {
            get { return this.GetCondition("BuyPoint", ""); }
            set { this.SetCondition("BuyPoint", value); }
        }

        public string DatePickerFrom
        {
            get { return this.GetCondition("DatePickerFrom", ""); }
            set { this.SetCondition("DatePickerFrom", value); }
        }

        public string DatePickerTo
        {
            get { return this.GetCondition("DatePickerTo", ""); }
            set { this.SetCondition("DatePickerTo", value); }
        }

        public string DatePickerSearch
        {
            get { return this.GetCondition("DatePickerSearch", ""); }
            set { this.SetCondition("DatePickerSearch", value); }
        }
    }
}
