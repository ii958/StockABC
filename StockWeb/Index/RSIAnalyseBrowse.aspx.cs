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
    public partial class RSIAnalyseBrowse : AISRS.WebUI.PageBaseNoPermission
    {
        private string url = "<a href='http://finance.sina.com.cn/realstock/company/stockcode/nc.shtml' target='_blank'>stockcode</a>";
        private void Page_Load(object sender, EventArgs e)
        {
            this.labScript.Text = string.Empty;
            this.LinkButtonQuery.LinkButtonClicked += new AISRS.WebUI.Modules.LinkButton.LinkButtonClickedHandler(LinkButtonQuery_LinkButtonClicked);
            if (!IsPostBack)
            {
                InitPage();
                this.RecordQueryCondition();
            }
        }

        private void InitPage()
        {
            //初始化日期
            this.DatePickerFrom.DateTime = DateTime.Now.ToShortDateString();                        
        }

        /// <summary>
        /// 记录查询条件
        /// </summary>
        private void RecordQueryCondition()
        {
            string code = this.txtStockCode.Text.Trim();
            string datepickerfrom = this.DatePickerFrom.DateTime;
            StockQueryCondition qc = new StockQueryCondition();

            qc.StockCode = code;
            qc.DatePickerFrom = datepickerfrom;
            this.ViewState["_StockQueryCondition"] = qc.SerializeToString();
        }

        /// <summary>
        /// 还原界面查询条件
        /// </summary>
        private void RestoreQueryCondition()
        {
            if (this.ViewState["_StockQueryCondition"].ToString() != string.Empty)
            {
                StockQueryCondition qc = new StockQueryCondition();

                qc.DeserializeFromString(this.ViewState["_StockQueryCondition"].ToString());

                this.txtStockCode.Text = qc.StockCode;               
                this.DatePickerFrom.DateTime = qc.DatePickerFrom;
                qc.Dispose();
            }
        }

        private void Refresh()
        {
            StockQueryCondition qc = new StockQueryCondition();
            qc.DeserializeFromString(this.ViewState["_StockQueryCondition"].ToString());
            this.labPrompt.Text = "";
            //查询日前两天
            string dateTo2 = new StockSystem().GetIndexDay(qc.DatePickerFrom, 2);
            dateTo2 = DateTimeFunction.ConvertDate(DateTime.Parse(dateTo2).ToShortDateString());  
            string date2RSI6 = new StockSystem().GetIndexDay(qc.DatePickerFrom, 8);
            date2RSI6 = DateTimeFunction.ConvertDate(DateTime.Parse(date2RSI6).ToShortDateString());
            string date2RSI12 = new StockSystem().GetIndexDay(qc.DatePickerFrom, 14);
            date2RSI12 = DateTimeFunction.ConvertDate(DateTime.Parse(date2RSI12).ToShortDateString());
            //string date2RSI24 = new StockSystem().GetIndexDay(qc.DatePickerFrom, 26);
            //date2RSI24 = DateTimeFunction.ConvertDate(DateTime.Parse(date2RSI24).ToShortDateString());

            //查询日
            string dateTo = DateTimeFunction.ConvertDate(qc.DatePickerFrom);            
            string dateRSI6 = new StockSystem().GetIndexDay(qc.DatePickerFrom, 6);
            dateRSI6 = DateTimeFunction.ConvertDate(DateTime.Parse(dateRSI6).ToShortDateString());
            string dateRSI12 = new StockSystem().GetIndexDay(qc.DatePickerFrom, 12);
            dateRSI12 = DateTimeFunction.ConvertDate(DateTime.Parse(dateRSI12).ToShortDateString());
            //string dateRSI24 = new StockSystem().GetIndexDay(qc.DatePickerFrom, 24);
            //dateRSI24 = DateTimeFunction.ConvertDate(DateTime.Parse(dateRSI24).ToShortDateString());

            AISTOCK_STOCK_STATS_DATA stockData = new StockSystem().GetStockData(dateTo2, txtStockCode.Text.Trim());
            string today = DateTimeFunction.ConvertDate(DateTime.Now.ToShortDateString());
            today = new StockSystem().GetToday(today);
            bool flag = false;
            if (int.Parse(dateTo) < int.Parse(today))
            {
                flag = true;
            }

            AISTOCK_STOCK_INDEX_V_DATA data2RSI6 = new AISTOCK_STOCK_INDEX_V_DATA();
            data2RSI6 = new StockSystem().GetStockDataIndex(date2RSI6, dateTo2, txtStockCode.Text.Trim());

            this.BmBlafTable.Clear();
            DrawRsiTableHeader();
            DrawRsiTableBody(data2RSI6, stockData, txtStockCode.Text.Trim(), flag, today, dateTo2, date2RSI12, dateTo, dateRSI6, dateRSI12);              
        }

        #region RSI指标
        private void DrawRsiTableHeader()
        {
            TableRow headerRow = this.BmBlafTable.AddHeadRow();

            headerRow.Height = 25;

            this.BmBlafTable.AddHeadCell(headerRow, "股票代码", 70).HorizontalAlign = HorizontalAlign.Center; //股票代码

            this.BmBlafTable.AddHeadCell(headerRow, "股票名称", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称
            
            this.BmBlafTable.AddHeadCell(headerRow, "RSI指标", 70).HorizontalAlign = HorizontalAlign.Center; //RSI指标

            this.BmBlafTable.AddHeadCell(headerRow, "成交量比值", 70).HorizontalAlign = HorizontalAlign.Center; //RSI指标

            this.BmBlafTable.AddHeadCell(headerRow, "查询日开盘", 70).HorizontalAlign = HorizontalAlign.Center; //股票代码

            this.BmBlafTable.AddHeadCell(headerRow, "查询前一日收盘", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTable.AddHeadCell(headerRow, "查询日收盘", 70).HorizontalAlign = HorizontalAlign.Center; //威廉指数

            this.BmBlafTable.AddHeadCell(headerRow, "增长率", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTable.AddHeadCell(headerRow, "K线图", 100).HorizontalAlign = HorizontalAlign.Center; //威廉指数

            this.BmBlafTable.AddHeadCell(headerRow, "今日开盘", 70).HorizontalAlign = HorizontalAlign.Center; 

            this.BmBlafTable.AddHeadCell(headerRow, "今日收盘", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "今日增长率", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "相对增长率", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.Width = "1400";
        }

        private void DrawRsiTableBody(AISTOCK_STOCK_INDEX_V_DATA data2RSI, AISTOCK_STOCK_STATS_DATA stockData, string code, bool isToday, string today, string dateTo2, string date2RSI12, string dateTo, string dateRSI6, string dateRSI12)
        {
            if (data2RSI == null || data2RSI.AISTOCK_STOCK_INDEX_V.Count <= 0) return;           
            TableRow bodyRow;            
            for (int i = 0; i < stockData.AISTOCK_STOCK_INFORMATION.Rows.Count; i++)
            {
                decimal sumA = 0;
                decimal sumB = 0;
                decimal index = 0;
                AISTOCK_STOCK_STATS_DATA.AISTOCK_STOCK_INFORMATIONRow stockRow = ((AISTOCK_STOCK_STATS_DATA.AISTOCK_STOCK_INFORMATIONRow)stockData.AISTOCK_STOCK_INFORMATION.Rows[i]);
                string stockCode = stockRow.STOCK_CODE.ToString();
                DataRow[] rows = data2RSI.AISTOCK_STOCK_INDEX_V.Select("STOCK_CODE = '" + stockCode + "'");
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
                if (index > 40) continue;
                else
                {
                    //符合条件，计算12日RSI
                    decimal sumA12 = 0;
                    decimal sumB12 = 0;
                    decimal index12 = 0;
                    AISTOCK_STOCK_INDEX_V_DATA data2RSI12 = new AISTOCK_STOCK_INDEX_V_DATA();
                    data2RSI12 = new StockSystem().GetStockDataIndex(date2RSI12, dateTo2, stockCode);
                    foreach (var row2RSI12 in data2RSI12.AISTOCK_STOCK_INDEX_V)
                    {
                        if (row2RSI12.DIFF > 0) sumA12 += row2RSI12.DIFF;
                        else sumB12 += row2RSI12.DIFF;
                    }
                    if (sumA12 - sumB12 != 0)
                    {
                        index12 = sumA12 / (sumA12 - sumB12) * 100;
                        if (index12 > index)
                        {
                            //符合条件，计算查询日RSI6
                            decimal sumARSI6 = 0, sumBRSI6 = 0, indexRSI6 = 0;
                            AISTOCK_STOCK_INDEX_V_DATA dataRSI6 = new AISTOCK_STOCK_INDEX_V_DATA();
                            dataRSI6 = new StockSystem().GetStockDataIndex(dateRSI6, dateTo, stockCode);
                            foreach (var rowRSI6 in dataRSI6.AISTOCK_STOCK_INDEX_V)
                            {
                                if (rowRSI6.DIFF > 0) sumARSI6 += rowRSI6.DIFF;
                                else sumBRSI6 += rowRSI6.DIFF;
                            }
                            if (sumARSI6 - sumBRSI6 != 0)
                            {
                                indexRSI6 = sumARSI6 / (sumARSI6 - sumBRSI6) * 100;
                                decimal sumARSI12 = 0, sumBRSI12 = 0, indexRSI12 = 0;
                                AISTOCK_STOCK_INDEX_V_DATA dataRSI12 = new AISTOCK_STOCK_INDEX_V_DATA();
                                dataRSI12 = new StockSystem().GetStockDataIndex(dateRSI12, dateTo, stockCode);
                                foreach (var rowRSI12 in dataRSI12.AISTOCK_STOCK_INDEX_V)
                                {
                                    if (rowRSI12.DIFF > 0) sumARSI12 += rowRSI12.DIFF;
                                    else sumBRSI12 += rowRSI12.DIFF;
                                }
                                if (sumARSI12 - sumBRSI12 != 0)
                                {
                                    indexRSI12 = sumARSI12 / (sumARSI12 - sumBRSI12) * 100;
                                    if (indexRSI6 > indexRSI12)
                                    {
                                        //符合条件，输出
                                        //成交量指标
                                        int quantity = new StockSystem().GetQuantity(stockCode);
                                        decimal ratio;
                                        if (quantity == 0) ratio = -1;
                                        ratio = (decimal)stockRow.QUANTITY / (decimal)quantity;
                                        if (ratio < 2) continue;
                                        if (index == 0) continue;

                                        bodyRow = this.BmBlafTable.AddBodyRow();
                                        this.BmBlafTable.AddCell(bodyRow, url.Replace("stockcode", stockRow.STOCK_CODE), HorizontalAlign.Left);
                                        this.BmBlafTable.AddCell(bodyRow, stockRow.STOCK_NAME, HorizontalAlign.Left);
                                        this.BmBlafTable.AddCell(bodyRow, index.ToString("0.00"), HorizontalAlign.Left);
                                        this.BmBlafTable.AddCell(bodyRow, ratio.ToString("0.00"), HorizontalAlign.Left);

                                        this.BmBlafTable.AddCell(bodyRow, stockRow.IsTODAY_BEGINNull() ? string.Empty : stockRow.TODAY_BEGIN.ToString(), HorizontalAlign.Left);
                                        this.BmBlafTable.AddCell(bodyRow, stockRow.IsYESTERDAY_ENDNull() ? string.Empty : stockRow.YESTERDAY_END.ToString(), HorizontalAlign.Left);
                                        this.BmBlafTable.AddCell(bodyRow, stockRow.IsTODAY_ENDNull() ? string.Empty : stockRow.TODAY_END.ToString(), HorizontalAlign.Left);
                                        this.BmBlafTable.AddCell(bodyRow, stockRow.IsINCREASE_PERCENTNull() ? string.Empty : stockRow.INCREASE_PERCENT.ToString(), HorizontalAlign.Left);
                                        this.BmBlafTable.AddCell(bodyRow, stockRow.IsCHARTNull() ? string.Empty : stockRow.CHART.ToString(), HorizontalAlign.Left);

                                        #region [取当天数据]

                                        if (isToday)
                                        {
                                            AISTOCK_STOCK_STATS_DATA todayData = new StockSystem().GetStockDataJingQue(today, stockCode);
                                            AISTOCK_STOCK_STATS_DATA.AISTOCK_STOCK_INFORMATIONRow todayRow = ((AISTOCK_STOCK_STATS_DATA.AISTOCK_STOCK_INFORMATIONRow)todayData.AISTOCK_STOCK_INFORMATION.Rows[0]);
                                            this.BmBlafTable.AddCell(bodyRow, todayRow.IsTODAY_BEGINNull() ? string.Empty : todayRow.TODAY_BEGIN.ToString(), HorizontalAlign.Left);
                                            this.BmBlafTable.AddCell(bodyRow, todayRow.IsTODAY_ENDNull() ? string.Empty : todayRow.TODAY_END.ToString(), HorizontalAlign.Left);
                                            this.BmBlafTable.AddCell(bodyRow, todayRow.IsINCREASE_PERCENTNull() ? string.Empty : todayRow.INCREASE_PERCENT.ToString(), HorizontalAlign.Left);
                                            decimal relativePercent = 0;
                                            if (stockRow.TODAY_END > 0)
                                            {
                                                relativePercent = (todayRow.TODAY_END - stockRow.TODAY_END) / stockRow.TODAY_END * 100;
                                            }

                                            this.BmBlafTable.AddCell(bodyRow, relativePercent.ToString("0.0000"), HorizontalAlign.Left);
                                        }
                                        else
                                        {
                                            this.BmBlafTable.AddCell(bodyRow, "", HorizontalAlign.Left);
                                            this.BmBlafTable.AddCell(bodyRow, "", HorizontalAlign.Left);
                                            this.BmBlafTable.AddCell(bodyRow, "", HorizontalAlign.Left);
                                            this.BmBlafTable.AddCell(bodyRow, "", HorizontalAlign.Left);
                                        }
                                        #endregion
                                    }
                                }
                            }
                            else continue;                                      
                        }
                        else continue;
                    }
                }                
            }           
        }
        #endregion               
        private void LinkButtonQuery_LinkButtonClicked(object sender, EventArgs e)
        {
            this.RecordQueryCondition();
            this.Refresh();
        }
    }
}