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

using AISRS.Common.Data;
using AISRS.BusinessFacade;
using AISRS.Common.Function;
using AISRS.Common.Exception;
using AISRS.Common.Framework;
using AISRS.Common.Query;

namespace AISRS.WebUI.DataCenter
{
    /// <summary>
    /// StockHistoryExport 的摘要说明。
    /// </summary>
    public partial class StockHistoryExport : AISRS.WebUI.PageBaseNoPermission
    {
        private ArrayList _dateList;

		private void Page_Load(object sender, System.EventArgs e)
		{
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            this.ViewState["_StockQueryCondition"] = string.Empty;

            this.Refresh();
		}		

		private void Refresh()
		{
            string dateFrom = Request.QueryString["datefrom"] == null ? string.Empty : Request.QueryString["datefrom"];
            string dateTo = Request.QueryString["dateto"] == null ? string.Empty : Request.QueryString["dateto"];
            string market = Request.QueryString["market"] == null ? string.Empty : Request.QueryString["market"];
            string field = Request.QueryString["field"] == null ? string.Empty : Request.QueryString["field"];
            string chart = Request.QueryString["chart"] == null ? string.Empty : Request.QueryString["chart"];
            string province = Request.QueryString["province"] == null ? string.Empty : Request.QueryString["province"];
            string stockName = Request.QueryString["stockName"] == null ? string.Empty : Request.QueryString["stockName"].ToString().Trim();
            string stockcode = Request.QueryString["stockcode"] == null ? string.Empty : Request.QueryString["stockcode"];
            string MarketName = Request.QueryString["MarketName"] == "--请选择--" ? string.Empty : Request.QueryString["MarketName"];
            string FieldName = Request.QueryString["FieldName"] == "--请选择--" ? string.Empty : Request.QueryString["FieldName"].Trim();
            string ChartName = Request.QueryString["ChartName"] == "--请选择--" ? string.Empty : Request.QueryString["ChartName"];
            string ProvinceName = Request.QueryString["ProvinceName"] == "--请选择--" ? string.Empty : Request.QueryString["ProvinceName"].Trim();

            StockQueryCondition qc = new StockQueryCondition();
            qc.DatePickerFrom = dateFrom;
            qc.DatePickerTo = dateTo;
            qc.StockCode = stockcode;
            qc.StockName = stockName;
            qc.Market = MarketName;
            qc.Field = FieldName;
            qc.Chart = ChartName;
            qc.Province = ProvinceName;


            this.ViewState["_StockQueryCondition"] = qc.SerializeToString();
            AISTOCK_STOCK_DAILY_DATA_V_DATA data = new AISTOCK_STOCK_DAILY_DATA_V_DATA();
            data = new StockSystem().GetStockHistory(qc);
            AISTOCK_STOCK_AVG_PRICE_DATA avgData = new AISTOCK_STOCK_AVG_PRICE_DATA();
            avgData = new StockSystem().GetStockAvgHistory(qc);

            this.BmBlafTableHistory.Clear();
            this.BmBlafTableHistory.BorderColor = Color.Black;
            this.BmBlafTableHistory.BorderWidth = Unit.Point(1);
            this.DrawBlafTableHeader();
            this.DrawBlafTableBody(data, avgData);

            this.LabelTitle.Text = "名称：查询股票历史记录<br>" +
                "创建时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "<br>" +
                "时间：" + qc.DatePickerFrom + "至" + qc.DatePickerTo + "<br>" +
                "所属证券市场：" + (qc.Market == string.Empty ? "ALL" : qc.Market) + "<br>" +
                "所属证监会行业：" + (qc.Field == string.Empty ? "ALL" : qc.Field) + "<br>" +
                "所属省份：" + (qc.Province == string.Empty ? "ALL" : qc.Province) + "<br>" +
                "K线图：" + (qc.Chart == string.Empty ? "ALL" : qc.Chart) + "<br>" +
                "股票代码：" + (qc.StockCode == string.Empty ? "ALL" : qc.StockCode) + "<br>" +
                "股票名称：" + (qc.StockName == string.Empty ? "ALL" : qc.StockName);
		}

        private void DrawBlafTableHeader()
        {
            //第一行
            TableRow headerRow = this.BmBlafTableHistory.AddHeadRow();
            headerRow.BorderColor = Color.Black;
            headerRow.BorderWidth = Unit.Point(1);
            headerRow.Height = 25;

            this.BmBlafTableHistory.AddHeadCell(headerRow, "&nbsp;", 70).HorizontalAlign = HorizontalAlign.Center; //股票代码

            this.BmBlafTableHistory.AddHeadCell(headerRow, "&nbsp;", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTableHistory.AddHeadCell(headerRow, "&nbsp;", 70).HorizontalAlign = HorizontalAlign.Center; //证券市场

            this.BmBlafTableHistory.AddHeadCell(headerRow, "&nbsp;", 120).HorizontalAlign = HorizontalAlign.Center; //证监会行业

            this.BmBlafTableHistory.AddHeadCell(headerRow, "&nbsp;", 120).HorizontalAlign = HorizontalAlign.Center; //行业领域

            this.BmBlafTableHistory.AddHeadCell(headerRow, "&nbsp;", 100).HorizontalAlign = HorizontalAlign.Center; //省份

            StockQueryCondition qc = new StockQueryCondition();
            qc.DeserializeFromString(this.ViewState["_StockQueryCondition"].ToString());

            _dateList = this.SplitDate(qc.DatePickerFrom, qc.DatePickerTo);

            for (int i = _dateList.Count - 1; i >= 0; i--)
            {
                TableCell cell = this.BmBlafTableHistory.AddHeadCell(headerRow, _dateList[i].ToString(), 70 * 12);
                cell.ColumnSpan = 12;
                cell.HorizontalAlign = HorizontalAlign.Center;
            }

            if (_dateList.Count > 1)
            {
                this.BmBlafTableHistory.AddHeadCell(headerRow, "&nbsp;", 70).HorizontalAlign = HorizontalAlign.Center;
                for (int i = _dateList.Count - 1; i > 0; i--)
                {
                    this.BmBlafTableHistory.AddHeadCell(headerRow, "&nbsp;", 70).HorizontalAlign = HorizontalAlign.Center;
                }
            }

            this.BmBlafTableHistory.AddHeadCell(headerRow, "&nbsp;", 70).HorizontalAlign = HorizontalAlign.Center; //总股本数

            this.BmBlafTableHistory.AddHeadCell(headerRow, "&nbsp;", 100).HorizontalAlign = HorizontalAlign.Center; //大股东

            this.BmBlafTableHistory.AddHeadCell(headerRow, "&nbsp;", 70).HorizontalAlign = HorizontalAlign.Center; //收入

            this.BmBlafTableHistory.AddHeadCell(headerRow, "&nbsp;", 70).HorizontalAlign = HorizontalAlign.Center; //利润

            this.BmBlafTableHistory.AddHeadCell(headerRow, "&nbsp;", 70).HorizontalAlign = HorizontalAlign.Center; //净利润率

            //第二行
            TableRow secondRow = this.BmBlafTableHistory.AddHeadRow();
            secondRow.BorderColor = Color.Black;
            secondRow.BorderWidth = Unit.Point(1);
            this.BmBlafTableHistory.AddHeadCell(secondRow, "股票代码", 70).HorizontalAlign = HorizontalAlign.Center; //股票代码

            this.BmBlafTableHistory.AddHeadCell(secondRow, "股票名称", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTableHistory.AddHeadCell(secondRow, "证券市场", 70).HorizontalAlign = HorizontalAlign.Center; //证券市场

            this.BmBlafTableHistory.AddHeadCell(secondRow, "证监会行业", 120).HorizontalAlign = HorizontalAlign.Center; //证监会行业

            this.BmBlafTableHistory.AddHeadCell(secondRow, "行业领域", 120).HorizontalAlign = HorizontalAlign.Center; //行业领域

            this.BmBlafTableHistory.AddHeadCell(secondRow, "省份", 100).HorizontalAlign = HorizontalAlign.Center; //省份

            for (int i = _dateList.Count - 1; i >= 0; i--)
            {
                this.BmBlafTableHistory.AddHeadCell(secondRow, "今日开盘价", 70).HorizontalAlign = HorizontalAlign.Center;
                this.BmBlafTableHistory.AddHeadCell(secondRow, "昨日收盘价", 70).HorizontalAlign = HorizontalAlign.Center;
                this.BmBlafTableHistory.AddHeadCell(secondRow, "今日收盘价", 70).HorizontalAlign = HorizontalAlign.Center;
                this.BmBlafTableHistory.AddHeadCell(secondRow, "5日均价", 70).HorizontalAlign = HorizontalAlign.Center;
                this.BmBlafTableHistory.AddHeadCell(secondRow, "10日均价", 70).HorizontalAlign = HorizontalAlign.Center;
                this.BmBlafTableHistory.AddHeadCell(secondRow, "20日均价", 70).HorizontalAlign = HorizontalAlign.Center;
                this.BmBlafTableHistory.AddHeadCell(secondRow, "最高价", 70).HorizontalAlign = HorizontalAlign.Center;
                this.BmBlafTableHistory.AddHeadCell(secondRow, "最低价", 70).HorizontalAlign = HorizontalAlign.Center;
                this.BmBlafTableHistory.AddHeadCell(secondRow, "成交量（股）", 70).HorizontalAlign = HorizontalAlign.Center;
                this.BmBlafTableHistory.AddHeadCell(secondRow, "成交额（元）", 70).HorizontalAlign = HorizontalAlign.Center;
                this.BmBlafTableHistory.AddHeadCell(secondRow, "增长率", 70).HorizontalAlign = HorizontalAlign.Center;
                this.BmBlafTableHistory.AddHeadCell(secondRow, "K线图", 70).HorizontalAlign = HorizontalAlign.Center;
            }

            if (_dateList.Count > 1)
            {
                this.BmBlafTableHistory.AddHeadCell(secondRow, "累计增长率", 70).HorizontalAlign = HorizontalAlign.Center;
                for (int i = _dateList.Count - 1; i > 0; i--)
                {
                    this.BmBlafTableHistory.AddHeadCell(secondRow, _dateList[i] + "与" + _dateList[i - 1] + "的成交量比率", 70).HorizontalAlign = HorizontalAlign.Center;
                }
            }

            this.BmBlafTableHistory.AddHeadCell(secondRow, "总股本数", 70).HorizontalAlign = HorizontalAlign.Center; //总股本数

            this.BmBlafTableHistory.AddHeadCell(secondRow, "大股东", 100).HorizontalAlign = HorizontalAlign.Center; //大股东

            this.BmBlafTableHistory.AddHeadCell(secondRow, "收入", 70).HorizontalAlign = HorizontalAlign.Center; //收入

            this.BmBlafTableHistory.AddHeadCell(secondRow, "利润", 70).HorizontalAlign = HorizontalAlign.Center; //利润

            this.BmBlafTableHistory.AddHeadCell(secondRow, "净利润率", 70).HorizontalAlign = HorizontalAlign.Center; //净利润率

            this.BmBlafTableHistory.Width = "100%";
            this.BmBlafTableHistory.Height = "400";
        }

        private void DrawBlafTableBody(AISTOCK_STOCK_DAILY_DATA_V_DATA data, AISTOCK_STOCK_AVG_PRICE_DATA avgData)
        {
            if (data == null)
            {
                throw new CommonException("获取收入历史数据出错");
            }

            if (data.AISTOCK_STOCK_DAILY_DATA_V.Count <= 0)
            {
                return;
            }
            string[,] stockData = new string[_dateList.Count, 12];
            bool flag = false;
            TableRow bodyRow;
            int count = 0;
            for (int i = 0; i < data.AISTOCK_STOCK_DAILY_DATA_V.Count; i=count)
            {
                AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow row = data.AISTOCK_STOCK_DAILY_DATA_V[i];
                flag = true;
                for (int j = _dateList.Count - 1; j >= 0; j--)
                {
                    string[] dateArr = _dateList[_dateList.Count - 1].ToString().Split('-');
                    string date = dateArr[0] + (int.Parse(dateArr[1]) < 10 ? "0" + dateArr[1] : dateArr[1]) + (int.Parse(dateArr[2]) < 10 ? "0" + dateArr[2] : dateArr[2]);
                    DataRow[] thisRow = data.AISTOCK_STOCK_DAILY_DATA_V.Select("STOCK_CODE = '" + row.STOCK_CODE + "' AND STOCK_DAY = '" + date + "'");
                    if (thisRow.Length <= 0)
                    {
                        flag = false;
                    }
                    dateArr = _dateList[j].ToString().Split('-');
                    date = dateArr[0] + (int.Parse(dateArr[1]) < 10 ? "0" + dateArr[1] : dateArr[1]) + (int.Parse(dateArr[2]) < 10 ? "0" + dateArr[2] : dateArr[2]);
                    DataRow[] tmpRow = data.AISTOCK_STOCK_DAILY_DATA_V.Select("STOCK_CODE = '" + row.STOCK_CODE + "' AND STOCK_DAY = '" + date + "'");
                    DataRow[] tmpAvgRow = avgData.AISTOCK_STOCK_AVG_PRICE.Select("STOCK_CODE = '" + row.STOCK_CODE + "' AND STOCK_DAY = '" + date + "'");
                    if (tmpRow.Length > 0)
                    {
                        count++;
                        stockData[j, 0] = ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).IsTODAY_BEGINNull() ? string.Empty : ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).TODAY_BEGIN.ToString();
                        stockData[j, 1] = ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).IsYESTERDAY_ENDNull() ? string.Empty : ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).YESTERDAY_END.ToString();
                        stockData[j, 2] = ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).IsTODAY_ENDNull() ? string.Empty : ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).TODAY_END.ToString();
                        stockData[j, 3] = ((AISTOCK_STOCK_AVG_PRICE_DATA.AISTOCK_STOCK_AVG_PRICERow)tmpAvgRow[0]).IsFIVE_AVGNull() ? string.Empty : ((AISTOCK_STOCK_AVG_PRICE_DATA.AISTOCK_STOCK_AVG_PRICERow)tmpAvgRow[0]).FIVE_AVG.ToString();
                        stockData[j, 4] = ((AISTOCK_STOCK_AVG_PRICE_DATA.AISTOCK_STOCK_AVG_PRICERow)tmpAvgRow[0]).IsTEN_AVGNull() ? string.Empty : ((AISTOCK_STOCK_AVG_PRICE_DATA.AISTOCK_STOCK_AVG_PRICERow)tmpAvgRow[0]).TEN_AVG.ToString();
                        stockData[j, 5] = ((AISTOCK_STOCK_AVG_PRICE_DATA.AISTOCK_STOCK_AVG_PRICERow)tmpAvgRow[0]).IsTWENTY_AVGNull() ? string.Empty : ((AISTOCK_STOCK_AVG_PRICE_DATA.AISTOCK_STOCK_AVG_PRICERow)tmpAvgRow[0]).TWENTY_AVG.ToString();
                        stockData[j, 6] = ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).IsMAX_PRICENull() ? string.Empty : ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).MAX_PRICE.ToString();
                        stockData[j, 7] = ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).IsMIN_PRICENull() ? string.Empty : ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).MIN_PRICE.ToString();
                        stockData[j, 8] = ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).IsQUANTITYNull() ? string.Empty : ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).QUANTITY.ToString();
                        stockData[j, 9] = ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).IsTOTAL_MONEYNull() ? string.Empty : ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).TOTAL_MONEY.ToString();
                        stockData[j, 10] = ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).IsINCREASE_PERCENTNull() ? string.Empty : ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).INCREASE_PERCENT.ToString();
                        stockData[j, 11] = ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).IsCHARTNull() ? string.Empty : ((AISTOCK_STOCK_DAILY_DATA_V_DATA.AISTOCK_STOCK_DAILY_DATA_VRow)tmpRow[0]).CHART.ToString();
                    }
                    else
                    {
                        for (int k = 0; k < 12; k++)
                        {
                            stockData[j, k] = string.Empty;
                        }
                    }
                }
                if (flag)//画一行
                {
                    bodyRow = this.BmBlafTableHistory.AddBodyRow();
                    bodyRow.BorderColor = Color.Black;
                    bodyRow.BorderWidth = Unit.Point(1);
                    this.BmBlafTableHistory.AddCell(bodyRow, row.STOCK_CODE, HorizontalAlign.Left);
                    this.BmBlafTableHistory.AddCell(bodyRow, row.STOCK_NAME, HorizontalAlign.Left);
                    this.BmBlafTableHistory.AddCell(bodyRow, row.STOCK_ADDR, HorizontalAlign.Left);
                    this.BmBlafTableHistory.AddCell(bodyRow, row.FIELD_MAIN, HorizontalAlign.Left);
                    this.BmBlafTableHistory.AddCell(bodyRow, row.FIELD, HorizontalAlign.Left);
                    this.BmBlafTableHistory.AddCell(bodyRow, row.PROVINCE, HorizontalAlign.Left);
                    for (int m = _dateList.Count - 1; m >= 0; m--)
                    {
                        for (int n = 0; n < 12; n++)
                        {
                            this.BmBlafTableHistory.AddCell(bodyRow, stockData[m, n], HorizontalAlign.Left);
                        }
                    }
                    decimal accu = 0;
                    if (decimal.Parse(stockData[0, 1]) != 0)
                    {
                        accu = (decimal.Parse(stockData[_dateList.Count - 1, 2]) - decimal.Parse(stockData[0, 1])) / decimal.Parse(stockData[0, 1]) * 100;
                    }
                    if (_dateList.Count > 1)
                    {
                        this.BmBlafTableHistory.AddCell(bodyRow, accu.ToString(), HorizontalAlign.Left);
                        for (int p = _dateList.Count - 1; p > 0; p--)
                        {
                            if (decimal.Parse(stockData[p - 1, 8]) != 0 && decimal.Parse(stockData[p, 8]) != 0)
                            {
                                this.BmBlafTableHistory.AddCell(bodyRow, (decimal.Parse(stockData[p, 8]) / decimal.Parse(stockData[p - 1, 8])).ToString(), HorizontalAlign.Left);
                            }
                            else
                            {
                                this.BmBlafTableHistory.AddCell(bodyRow, string.Empty, HorizontalAlign.Left);
                            }
                        }
                    }
                    this.BmBlafTableHistory.AddCell(bodyRow, row.TOTAL_STOCK.ToString(), HorizontalAlign.Left);
                    this.BmBlafTableHistory.AddCell(bodyRow, row.STOCKER.ToString(), HorizontalAlign.Left);
                    this.BmBlafTableHistory.AddCell(bodyRow, row.IsREVENUENull() ? string.Empty : row.REVENUE.ToString(), HorizontalAlign.Left);
                    this.BmBlafTableHistory.AddCell(bodyRow, row.IsPROFITNull() ? string.Empty : row.PROFIT.ToString(), HorizontalAlign.Left);
                    this.BmBlafTableHistory.AddCell(bodyRow, row.IsPROFIT_PERCENTNull() ? string.Empty : row.PROFIT_PERCENT.ToString(), HorizontalAlign.Left);
                }
            }
        }

        /// <summary>
        ///传入一个起始日期和一个结束日期,返回一个这个时间段的数组.
        /// </summary>
        /// <param name="yearMonthFrom"></param>
        /// <param name="yearMonthTo"></param>
        /// <returns></returns>
        private ArrayList SplitDate(string dateFrom, string dateTo)
        {
            ArrayList arrayListDate = new ArrayList();
            DateTime dtFrom = DateTime.Parse(dateFrom);
            DateTime dtTo = DateTime.Parse(dateTo);
            DataTable data = new StockSystem().GetDates(dtFrom, dtTo);
            if (data.Rows.Count == 1)
            {
                arrayListDate.Add(dateFrom);
            }
            else
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    arrayListDate.Add(((DateTime)data.Rows[i][0]).ToShortDateString());
                }
            }

            return arrayListDate;
        }
    }
}