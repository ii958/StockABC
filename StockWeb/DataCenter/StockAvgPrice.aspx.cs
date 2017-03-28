using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Net;

using AISRS.Common.Data;
using AISRS.BusinessFacade;
using AISRS.Common.Function;
using AISRS.Common.Exception;
using AISRS.Common.Framework;
using AISRS.Common.Entity;

namespace AISRS.WebUI.DataCenter
{
    /// <summary>
    /// StockAvgPrice 的摘要说明。
    /// </summary>
    public partial class StockAvgPrice : AISRS.WebUI.PageBaseNoPermission
    {        
        public void Page_Load(object sender, EventArgs e)
        {
            this.LinkButtonImport.JavascriptOnClick = "return SubmitClick();";
            this.LinkButtonImport.LinkButtonClicked += new AISRS.WebUI.Modules.LinkButton.LinkButtonClickedHandler(LinkButtonImport_LinkButtonClicked);
            this.labScript.Text = string.Empty;
            if (!IsPostBack)
            {
                InitImportData();
            }
        }

        private void InitImportData()
        {
            this.DatePickerImportDate.DateTime = DateTime.Now.ToShortDateString();
        }

        private void LinkButtonImport_LinkButtonClicked(object sender, EventArgs e)
        {
            DeleteExistsData();
            CalculateAvgPriceData();
            this.Refresh();
        }

        private void DeleteExistsData()
        {
            string date = DateTimeFunction.ConvertDate(this.DatePickerImportDate.DateTime);
            bool exists = new StockSystem().IsExistsData(date);
            if (exists)
            {
                new StockSystem().DeleteAvgData(date);
            }
        }

        private void CalculateAvgPriceData()
        {
            AISTOCK_STOCK_AVG_PRICE_DATA data = new AISTOCK_STOCK_AVG_PRICE_DATA();
            //List<Stock> stocks = new ParseStock().ParseStockXml();
            AISTOCK_STOCK_IMPORT_DATA stocks = new StockSystem().GetStockImport();
            string stockCode = string.Empty;
            string date = DateTimeFunction.ConvertDate(this.DatePickerImportDate.DateTime);
            decimal fiveAvg = 0;
            decimal tenAvg = 0;
            decimal twentyAvg = 0;
            decimal thirtyAvg = 0;
            decimal sixtyAvg = 0;
            foreach (var item in stocks.AISTOCK_STOCK_IMPORT)
            {
                AISTOCK_STOCK_AVG_PRICE_DATA.AISTOCK_STOCK_AVG_PRICERow row = data.AISTOCK_STOCK_AVG_PRICE.NewAISTOCK_STOCK_AVG_PRICERow();
                Guid stock_id = Guid.NewGuid();
                stockCode = item.STOCK_CODE;                
                fiveAvg = new StockSystem().GetAvgPrice(stockCode, date, 5);
                tenAvg = new StockSystem().GetAvgPrice(stockCode, date, 10);
                twentyAvg = new StockSystem().GetAvgPrice(stockCode, date, 20);
                thirtyAvg = new StockSystem().GetAvgPrice(stockCode, date, 30);
                sixtyAvg = new StockSystem().GetAvgPrice(stockCode, date, 60);

                row.STOCK_AVG_ID = stock_id.ToString();
                row.STOCK_CODE = stockCode;
                row.STOCK_NAME = item.STOCK_NAME;
                row.STOCK_DAY = date;
                row.FIVE_AVG = fiveAvg;
                row.TEN_AVG = tenAvg;
                row.TWENTY_AVG = twentyAvg;
                row.THIRTY_AVG = thirtyAvg;
                row.SIXTY_AVG = sixtyAvg;

                data.AISTOCK_STOCK_AVG_PRICE.Rows.Add(row);
            }
            new StockSystem().InsertAvgPrice(data);
        }

        private void Refresh()
        {
            string date = DateTimeFunction.ConvertDate(this.DatePickerImportDate.DateTime);           
            AISTOCK_STOCK_AVG_PRICE_DATA data = new StockSystem().GetStockAvgData(date);

            this.BmBlafTableStock.Clear();
            this.DrawBmTableHeader();
            this.DrawBmTableBody(data);
        }

        private void DrawBmTableHeader()
        {
            TableRow headerRow = this.BmBlafTableStock.AddHeadRow();

            headerRow.Height = 25;

            this.BmBlafTableStock.AddHeadCell(headerRow, "股票代码", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "股票名称", 100).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "5日均价", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "10日均价", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "20日均价", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "30日均价", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "60日均价", 100).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.Width = "100%";
        }

        private void DrawBmTableBody(AISTOCK_STOCK_AVG_PRICE_DATA data)
        {
            if (data == null)
            {
                return;
            }
            if (data.AISTOCK_STOCK_AVG_PRICE.Count <= 0)
            {
                return;
            }
            TableRow bodyRow;
            foreach (AISTOCK_STOCK_AVG_PRICE_DATA.AISTOCK_STOCK_AVG_PRICERow row in data.AISTOCK_STOCK_AVG_PRICE.Rows)
            {
                bodyRow = this.BmBlafTableStock.AddBodyRow();

                this.BmBlafTableStock.AddCell(bodyRow, row.STOCK_CODE, HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.STOCK_NAME, HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.IsFIVE_AVGNull() ? string.Empty : row.FIVE_AVG.ToString(), HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.IsTEN_AVGNull() ? string.Empty : row.TEN_AVG.ToString(), HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.IsTWENTY_AVGNull() ? string.Empty : row.TWENTY_AVG.ToString(), HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.IsTHIRTY_AVGNull() ? string.Empty : row.THIRTY_AVG.ToString(), HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.IsSIXTY_AVGNull() ? string.Empty : row.SIXTY_AVG.ToString(), HorizontalAlign.Left);
            }
        }
    }
}