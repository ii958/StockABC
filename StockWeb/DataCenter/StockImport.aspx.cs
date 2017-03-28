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
    /// StockImport 的摘要说明。
    /// </summary>
    public partial class StockImport : AISRS.WebUI.PageBaseNoPermission
    {
        protected System.Web.UI.WebControls.Label labScript;


		private void Page_Load(object sender, System.EventArgs e)
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

        private void ImportData()
        {
            string date = DateTimeFunction.ConvertDate(this.DatePickerImportDate.DateTime);
            AISTOCK_STOCK_STATS_DATA data = new AISTOCK_STOCK_STATS_DATA();
            //List<Stock> stocks = new ParseStock().ParseStockXml();
            AISTOCK_STOCK_IMPORT_DATA stocks = new StockSystem().GetStockImport();
            string stockCode = string.Empty;
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "text/html; charset=gb2312");
            Stream stockData;
            StreamReader reader;
            string returnData;
            int start;
            int end;
            string[] tradeData;
            string stockName = string.Empty;
            decimal todayBegin;
            decimal yesterdayEnd;
            decimal current;
            decimal max;
            decimal min;
            decimal buy;
            decimal sell;
            int quantity;
            decimal money;
            decimal[] buyer_price;
            int[] buyer_quantity;
            decimal[] seller_price;
            int[] seller_quanlity;

            foreach (var item in stocks.AISTOCK_STOCK_IMPORT)
            {
                AISTOCK_STOCK_STATS_DATA.AISTOCK_STOCK_INFORMATIONRow row = data.AISTOCK_STOCK_INFORMATION.NewAISTOCK_STOCK_INFORMATIONRow();
                stockCode = item.STOCK_CODE.ToString();
                Guid stock_id = Guid.NewGuid();
                buyer_price = new decimal[5];
                buyer_quantity = new int[5];
                seller_price = new decimal[5];
                seller_quanlity = new int[5];
                stockData = client.OpenRead("http://hq.sinajs.cn/list=" + stockCode);
                reader = new StreamReader(stockData, System.Text.Encoding.GetEncoding("gb2312"));
                returnData = reader.ReadToEnd();
                start = returnData.IndexOf('"');
                end = returnData.LastIndexOf('"');
                returnData = returnData.Substring(start + 1, end - start - 1);

                tradeData = returnData.Split(',');
                if (tradeData.Length == 33)
                {
                    stockName = tradeData[0].ToString();
                    todayBegin = decimal.Parse(tradeData[1].ToString());
                    yesterdayEnd = decimal.Parse(tradeData[2].ToString());
                    current = decimal.Parse(tradeData[3].ToString());
                    max = decimal.Parse(tradeData[4].ToString());
                    min = decimal.Parse(tradeData[5].ToString());
                    buy = decimal.Parse(tradeData[6].ToString());
                    sell = decimal.Parse(tradeData[7].ToString());
                    quantity = int.Parse(tradeData[8].ToString());
                    money = decimal.Parse(tradeData[9].ToString());
                    buyer_quantity[0] = int.Parse(tradeData[10].ToString());
                    buyer_price[0] = decimal.Parse(tradeData[11].ToString());
                    buyer_quantity[1] = int.Parse(tradeData[12].ToString());
                    buyer_price[1] = decimal.Parse(tradeData[13].ToString());
                    buyer_quantity[2] = int.Parse(tradeData[14].ToString());
                    buyer_price[2] = decimal.Parse(tradeData[15].ToString());
                    buyer_quantity[3] = int.Parse(tradeData[16].ToString());
                    buyer_price[3] = decimal.Parse(tradeData[17].ToString());
                    buyer_quantity[4] = int.Parse(tradeData[18].ToString());
                    buyer_price[4] = decimal.Parse(tradeData[19].ToString());
                    seller_quanlity[0] = int.Parse(tradeData[20].ToString());
                    seller_price[0] = decimal.Parse(tradeData[21].ToString());
                    seller_quanlity[1] = int.Parse(tradeData[22].ToString());
                    seller_price[1] = decimal.Parse(tradeData[23].ToString());
                    seller_quanlity[2] = int.Parse(tradeData[24].ToString());
                    seller_price[2] = decimal.Parse(tradeData[25].ToString());
                    seller_quanlity[3] = int.Parse(tradeData[26].ToString());
                    seller_price[3] = decimal.Parse(tradeData[27].ToString());
                    seller_quanlity[4] = int.Parse(tradeData[28].ToString());
                    seller_price[4] = decimal.Parse(tradeData[29].ToString());
                }
                else
                {
                    todayBegin = 0;
                    yesterdayEnd = 0;
                    current = 0;
                    max = 0;
                    min = 0;
                    buy = 0;
                    sell = 0;
                    quantity = 0;
                    money = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        buyer_quantity[i] = 0;
                        buyer_price[i] = 0;
                        seller_quanlity[i] = 0;
                        seller_price[i] = 0;
                    }
                }

                reader.Close();
                stockData.Close();

                decimal percent;
                string chart;
                if (yesterdayEnd > 0)
                {
                    percent = (current - yesterdayEnd) / yesterdayEnd * 100;
                }
                else
                {
                    percent = 0;
                }
                chart = this.GetChart(todayBegin, current, max, min);

                row.STOCK_ID = stock_id.ToString().ToUpper();
                row.STOCK_CODE = stockCode;
                row.STOCK_NAME = stockName;
                row.STOCK_DAY = date;
                row.TODAY_BEGIN = todayBegin;
                row.YESTERDAY_END = yesterdayEnd;
                row.TODAY_END = current;
                row.MAX_PRICE = max;
                row.MIN_PRICE = min;
                row.BUY_PRICE = buy;
                row.SELL_PRICE = sell;
                row.QUANTITY = quantity;
                row.TOTAL_MONEY = money;
                row.BUY_ONE_QUANTITY = buyer_quantity[0];
                row.BUY_ONE_PRICE = buyer_price[0];
                row.BUY_TWO_QUANTITY = buyer_quantity[1];
                row.BUY_TWO_PRICE = buyer_price[1];
                row.BUY_THD_QUANTITY = buyer_quantity[2];
                row.BUY_THD_PRICE = buyer_price[2];
                row.BUY_FOU_QUANTITY = buyer_quantity[3];
                row.BUY_FOU_PRICE = buyer_price[3];
                row.BUY_FIF_QUANTITY = buyer_quantity[4];
                row.BUY_FIF_PRICE = buyer_price[4];
                row.SELL_ONE_QUANTITY = seller_quanlity[0];
                row.SELL_ONE_PRICE = seller_price[0];
                row.SELL_TWO_QUANTITY = seller_quanlity[1];
                row.SELL_TWO_PRICE = seller_price[1];
                row.SELL_THD_QUANTITY = seller_quanlity[2];
                row.SELL_THD_PRICE = seller_price[2];
                row.SELL_FOU_QUANTITY = seller_quanlity[3];
                row.SELL_FOU_PRICE = seller_price[3];
                row.SELL_FIF_QUANTITY = seller_quanlity[4];
                row.SELL_FIF_PRICE = seller_price[4];
                row.INCREASE_PERCENT = percent;
                row.CHART = chart;

                data.AISTOCK_STOCK_INFORMATION.Rows.Add(row);
            }
            new StockSystem().ImportStockData(data);
        }

		private void Refresh()
		{
            string date = DateTimeFunction.ConvertDate(this.DatePickerImportDate.DateTime);
            AISTOCK_STOCK_STATS_DATA data = new StockSystem().GetStockData(date, null);

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

            this.BmBlafTableStock.AddHeadCell(headerRow, "昨日收盘价", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "今日开盘价", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "今日收盘价", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "增长率", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "成交量（股）", 100).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "成交额（元）", 100).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "K线图", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.Width = "100%";
		}

        private void DrawBmTableBody(AISTOCK_STOCK_STATS_DATA data)
		{
            if (data == null)
            {
                return;
            }
            if (data.AISTOCK_STOCK_INFORMATION.Count <= 0)
            {
                return;
            }
            TableRow bodyRow;

            foreach (AISTOCK_STOCK_STATS_DATA.AISTOCK_STOCK_INFORMATIONRow row in data.AISTOCK_STOCK_INFORMATION.Rows)
            {
                bodyRow = this.BmBlafTableStock.AddBodyRow();

                this.BmBlafTableStock.AddCell(bodyRow, row.STOCK_CODE, HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.STOCK_NAME, HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.YESTERDAY_END.ToString(), HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.TODAY_BEGIN.ToString(), HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.TODAY_END.ToString(), HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.INCREASE_PERCENT.ToString(), HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.QUANTITY.ToString(), HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.TOTAL_MONEY.ToString(), HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.CHART, HorizontalAlign.Left);
            }
		}		

		private void LinkButtonImport_LinkButtonClicked(object sender,EventArgs e)
		{
            ImportData();
			this.Refresh();
		}

        private string GetChart(decimal begin, decimal end, decimal max, decimal min)
        {
            string chart = string.Empty;
            decimal diff = begin - end;
            if (diff < 0)
            {
                chart = "阳线";
                if (begin == min && end == max)
                {
                    chart = "光头光脚大阳线";
                }
                else if (begin == min && end < max)
                {
                    chart = "光脚阳线";
                }
                else if (begin > min && end == max)
                {
                    chart = "光头阳线";
                }

            }
            else if (diff == 0)
            {
                chart = "十字星";
                if (begin == max && max == min)
                {
                    chart = "一字形";
                }
                else if (begin == max && max > min)
                {
                    chart = "T形";
                }
                else if (begin == min && max > min)
                {
                    chart = "塔形";
                }
            }
            else
            {
                chart = "阴线";
                if (begin == max && end == min)
                {
                    chart = "光头光脚大阴线";
                }
                else if (begin < max && end == min)
                {
                    chart = "光脚阴线";
                }
                else if (begin == max && end > min)
                {
                    chart = "光头阴线";
                }
            }
            return chart;
        }
    }
}