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

namespace AISRS.WebUI.Index
{
    /// <summary>
    /// StockHistoryExport 的摘要说明。
    /// </summary>
    public partial class IndexAnalyseExport : AISRS.WebUI.PageBaseNoPermission
    {
        private void Page_Load(object sender, EventArgs e)
        {
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            this.ViewState["_StockQueryCondition"] = string.Empty;

            this.Refresh();
        }

        private void Refresh()
        {
            string dateFrom = Request.QueryString["datefrom"] == null ? string.Empty : Request.QueryString["datefrom"];
            string market = Request.QueryString["market"] == null ? string.Empty : Request.QueryString["market"];
            string field = Request.QueryString["field"] == null ? string.Empty : Request.QueryString["field"];
            string index = Request.QueryString["index"] == null ? string.Empty : Request.QueryString["index"];
            string province = Request.QueryString["province"] == null ? string.Empty : Request.QueryString["province"];
            string stockName = Request.QueryString["stockName"] == null ? string.Empty : Request.QueryString["stockName"].ToString().Trim();
            string stockcode = Request.QueryString["stockcode"] == null ? string.Empty : Request.QueryString["stockcode"];
            string MarketName = Request.QueryString["MarketName"] == "--请选择--" ? string.Empty : Request.QueryString["MarketName"];
            string FieldName = Request.QueryString["FieldName"] == "--请选择--" ? string.Empty : Request.QueryString["FieldName"].Trim();
            string IndexName = Request.QueryString["IndexName"].Trim();
            string ProvinceName = Request.QueryString["ProvinceName"] == "--请选择--" ? string.Empty : Request.QueryString["ProvinceName"].Trim();

            StockQueryCondition qc = new StockQueryCondition();
            qc.DatePickerFrom = dateFrom;
            qc.StockCode = stockcode;
            qc.StockName = stockName;
            qc.Market = MarketName;
            qc.Field = FieldName;
            qc.Index = IndexName;
            qc.Province = ProvinceName;

            this.ViewState["_StockQueryCondition"] = qc.SerializeToString();

            int indexDay = 14;

            if (qc.Index.Equals("RSI(5)"))
            {
                indexDay = 5;
            }
            else if (qc.Index.Equals("RSI(9)"))
            {
                indexDay = 9;
            }
            else if (qc.Index.Equals("RSI(14)"))
            {
                indexDay = 14;
            }
            else if (qc.Index.Equals("WMS(10)"))
            {
                indexDay = 10;
            }
            else if (qc.Index.Equals("WMS(20)"))
            {
                indexDay = 20;
            }

            string dateTo = DateTimeFunction.ConvertDate(qc.DatePickerFrom);
            string date = new StockSystem().GetIndexDay(qc.DatePickerFrom, indexDay);
            date = DateTimeFunction.ConvertDate(DateTime.Parse(date).ToShortDateString());

            AISTOCK_STOCK_STATS_DATA stockData = new StockSystem().GetStockData(dateTo, stockcode);

            if (qc.Index.Equals("RSI(5)") || qc.Index.Equals("RSI(9)") || qc.Index.Equals("RSI(14)"))
            {
                AISTOCK_STOCK_INDEX_V_DATA dataRSI = new AISTOCK_STOCK_INDEX_V_DATA();
                dataRSI = new StockSystem().GetStockDataIndex(date, dateTo, stockcode);

                this.BmBlafTable.Clear();
                this.BmBlafTable.BorderColor = Color.Black;
                this.BmBlafTable.BorderWidth = Unit.Point(1);
                DrawRsiTableHeader();
                DrawRsiTableBody(dataRSI, stockData);
            }
            else if (qc.Index.Equals("WMS(10)") || qc.Index.Equals("WMS(20)"))
            {
                AISTOCK_STOCK_WMS_V_DATA dataWms = new AISTOCK_STOCK_WMS_V_DATA();
                dataWms = new StockSystem().GetStockWmsData(date, dateTo, stockcode);

                this.BmBlafTable.Clear();
                this.BmBlafTable.BorderColor = Color.Black;
                this.BmBlafTable.BorderWidth = Unit.Point(1);
                DrawWmsTableHeader();
                DrawWmsTableBody(stockData, dataWms);
            }
            this.LabelTitle.Text = "名称：股票指标分析<br>" +
                "创建时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "<br>" +
                "时间：" + qc.DatePickerFrom + "<br>" +
                "所属证券市场：" + (qc.Market == string.Empty ? "ALL" : qc.Market) + "<br>" +
                "所属证监会行业：" + (qc.Field == string.Empty ? "ALL" : qc.Field) + "<br>" +
                "所属省份：" + (qc.Province == string.Empty ? "ALL" : qc.Province) + "<br>" +
                "指标类型：" + (qc.Index == string.Empty ? "ALL" : qc.Index) + "<br>" +
                "股票代码：" + (qc.StockCode == string.Empty ? "ALL" : qc.StockCode) +
                "股票名称：" + (qc.StockName == string.Empty ? "ALL" : qc.StockName);
        }

        #region RSI指标
        private void DrawRsiTableHeader()
        {
            TableRow headerRow = this.BmBlafTable.AddHeadRow();
            headerRow.BorderColor = Color.Black;
            headerRow.BorderWidth = Unit.Point(1);
            headerRow.Height = 25;

            this.BmBlafTable.AddHeadCell(headerRow, "股票代码", 70).HorizontalAlign = HorizontalAlign.Center; //股票代码

            this.BmBlafTable.AddHeadCell(headerRow, "股票名称", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTable.AddHeadCell(headerRow, "RSI指标", 70).HorizontalAlign = HorizontalAlign.Center; //RSI指标

            this.BmBlafTable.AddHeadCell(headerRow, "今日开盘", 70).HorizontalAlign = HorizontalAlign.Center; //股票代码

            this.BmBlafTable.AddHeadCell(headerRow, "昨日收盘", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTable.AddHeadCell(headerRow, "今日收盘", 70).HorizontalAlign = HorizontalAlign.Center; //威廉指数

            this.BmBlafTable.AddHeadCell(headerRow, "增长率", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTable.AddHeadCell(headerRow, "K线图", 100).HorizontalAlign = HorizontalAlign.Center; //威廉指数

            this.BmBlafTable.Width = "1000";
        }

        private void DrawRsiTableBody(AISTOCK_STOCK_INDEX_V_DATA dataRSI, AISTOCK_STOCK_STATS_DATA stockData)
        {
            if (dataRSI == null)
            {
                return;
            }
            if (dataRSI.AISTOCK_STOCK_INDEX_V.Count <= 0)
            {
                return;
            }
            TableRow bodyRow;
            for (int i = 0; i < stockData.AISTOCK_STOCK_INFORMATION.Rows.Count; i++)
            {
                decimal sumA = 0;
                decimal sumB = 0;
                decimal index = 0;
                AISTOCK_STOCK_STATS_DATA.AISTOCK_STOCK_INFORMATIONRow stockRow = ((AISTOCK_STOCK_STATS_DATA.AISTOCK_STOCK_INFORMATIONRow)stockData.AISTOCK_STOCK_INFORMATION.Rows[i]);
                string stockCode = stockRow.STOCK_CODE.ToString();
                DataRow[] rows = dataRSI.AISTOCK_STOCK_INDEX_V.Select("STOCK_CODE = '" + stockCode + "'");
                if (rows.Length <= 0)
                {
                    continue;
                }
                for (int j = 0; j < rows.Length; j++)
                {
                    AISTOCK_STOCK_INDEX_V_DATA.AISTOCK_STOCK_INDEX_VRow row = (AISTOCK_STOCK_INDEX_V_DATA.AISTOCK_STOCK_INDEX_VRow)rows[j];
                    if (row.DIFF > 0)
                    {
                        sumA += row.DIFF;
                    }
                    else
                    {
                        sumB += row.DIFF;
                    }
                }
                if (sumA - sumB != 0)
                {
                    index = sumA / (sumA - sumB) * 100;
                }
                if (index < 75 || index > 80) continue;
                bodyRow = this.BmBlafTable.AddBodyRow();
                bodyRow.BorderColor = Color.Black;
                bodyRow.BorderWidth = Unit.Point(1);
                this.BmBlafTable.AddCell(bodyRow, stockRow.STOCK_CODE, HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, stockRow.STOCK_NAME, HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, index.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, stockRow.IsTODAY_BEGINNull() ? string.Empty : stockRow.TODAY_BEGIN.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, stockRow.IsYESTERDAY_ENDNull() ? string.Empty : stockRow.YESTERDAY_END.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, stockRow.IsTODAY_ENDNull() ? string.Empty : stockRow.TODAY_END.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, stockRow.IsINCREASE_PERCENTNull() ? string.Empty : stockRow.INCREASE_PERCENT.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, stockRow.IsCHARTNull() ? string.Empty : stockRow.CHART.ToString(), HorizontalAlign.Left);
            }
        }
        #endregion

        #region 威廉指数
        private void DrawWmsTableHeader()
        {
            TableRow headerRow = this.BmBlafTable.AddHeadRow();
            headerRow.BorderColor = Color.Black;
            headerRow.BorderWidth = Unit.Point(1);
            headerRow.Height = 25;

            this.BmBlafTable.AddHeadCell(headerRow, "股票代码", 70).HorizontalAlign = HorizontalAlign.Center; //股票代码

            this.BmBlafTable.AddHeadCell(headerRow, "股票名称", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTable.AddHeadCell(headerRow, "威廉指数", 70).HorizontalAlign = HorizontalAlign.Center; //威廉指数

            this.BmBlafTable.AddHeadCell(headerRow, "今日开盘", 70).HorizontalAlign = HorizontalAlign.Center; //股票代码

            this.BmBlafTable.AddHeadCell(headerRow, "昨日收盘", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTable.AddHeadCell(headerRow, "今日收盘", 70).HorizontalAlign = HorizontalAlign.Center; //威廉指数

            this.BmBlafTable.AddHeadCell(headerRow, "增长率", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTable.AddHeadCell(headerRow, "K线图", 100).HorizontalAlign = HorizontalAlign.Center; //威廉指数

            this.BmBlafTable.Width = "1000";
        }

        private void DrawWmsTableBody(AISTOCK_STOCK_STATS_DATA stockData, AISTOCK_STOCK_WMS_V_DATA data)
        {
            if (stockData == null)
            {
                return;
            }
            if (stockData.AISTOCK_STOCK_INFORMATION.Count <= 0)
            {
                return;
            }
            decimal max_price = 0;
            decimal min_price = 0;
            decimal end_price = 0;
            decimal index = 0;
            TableRow bodyRow;
            for (int i = 0; i < stockData.AISTOCK_STOCK_INFORMATION.Rows.Count; i++)
            {
                AISTOCK_STOCK_STATS_DATA.AISTOCK_STOCK_INFORMATIONRow stockRow = (AISTOCK_STOCK_STATS_DATA.AISTOCK_STOCK_INFORMATIONRow)stockData.AISTOCK_STOCK_INFORMATION.Rows[i];
                DataRow[] row = data.AISTOCK_STOCK_WMS_V.Select("STOCK_CODE = '" + stockRow.STOCK_CODE + "' AND STOCK_NAME = '" + stockRow.STOCK_NAME + "'");
                max_price = ((AISTOCK_STOCK_WMS_V_DATA.AISTOCK_STOCK_WMS_VRow)row[0]).MAX_PRICE;
                min_price = ((AISTOCK_STOCK_WMS_V_DATA.AISTOCK_STOCK_WMS_VRow)row[0]).MIN_PRICE;
                end_price = stockRow.TODAY_END;
                if (max_price - min_price != 0)
                {
                    index = (max_price - end_price) / (max_price - min_price) * 100;
                }
                bodyRow = this.BmBlafTable.AddBodyRow();
                bodyRow.BorderColor = Color.Black;
                bodyRow.BorderWidth = Unit.Point(1);
                this.BmBlafTable.AddCell(bodyRow, stockRow.STOCK_CODE, HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, stockRow.STOCK_NAME, HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, index.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, stockRow.IsTODAY_BEGINNull() ? string.Empty : stockRow.TODAY_BEGIN.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, stockRow.IsYESTERDAY_ENDNull() ? string.Empty : stockRow.YESTERDAY_END.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, stockRow.IsTODAY_ENDNull() ? string.Empty : stockRow.TODAY_END.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, stockRow.IsINCREASE_PERCENTNull() ? string.Empty : stockRow.INCREASE_PERCENT.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, stockRow.IsCHARTNull() ? string.Empty : stockRow.CHART.ToString(), HorizontalAlign.Left);
            }
        }
        #endregion
    }
}