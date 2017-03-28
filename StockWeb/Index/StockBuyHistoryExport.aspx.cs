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
    /// StockBuyHistoryExport 的摘要说明。
    /// </summary>
    public partial class StockBuyHistoryExport : AISRS.WebUI.PageBaseNoPermission
    {
        private ArrayList _dateList;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            this.ViewState["_StockQueryCondition"] = string.Empty;

            this.Refresh();
        }

        private void Refresh()
        {
            string dateFrom = Request.QueryString["datefrom"] == null ? string.Empty : Request.QueryString["datefrom"];
            string dateTo = Request.QueryString["dateto"] == null ? string.Empty : Request.QueryString["dateto"];
            string dateSearch = Request.QueryString["datesearch"] == null ? string.Empty : Request.QueryString["datesearch"];
            string type = Request.QueryString["type"] == null ? string.Empty : Request.QueryString["type"];
            string stockName = Request.QueryString["stockName"] == null ? string.Empty : Request.QueryString["stockName"].ToString().Trim();
            string stockcode = Request.QueryString["stockcode"] == null ? string.Empty : Request.QueryString["stockcode"];
            string TypeName = Request.QueryString["TypeName"] == "--请选择--" ? string.Empty : Request.QueryString["TypeName"];

            StockQueryCondition qc = new StockQueryCondition();
            qc.DatePickerFrom = dateFrom;
            qc.DatePickerTo = dateTo;
            qc.DatePickerSearch = dateSearch;
            qc.StockCode = stockcode;
            qc.StockName = stockName;
            qc.BuyPoint = TypeName;

            this.ViewState["_StockQueryCondition"] = qc.SerializeToString();
            AISTOCK_STOCK_BUY_HISTORY_DATA data = new StockSystem().GetBuyHistoryData(qc);
            this.BmBlafTableHistory.Clear();
            this.BmBlafTableHistory.BorderColor = Color.Black;
            this.BmBlafTableHistory.BorderWidth = Unit.Point(1);
            this.DrawBlafTableHeader();
            this.DrawBlafTableBody(data);

            this.LabelTitle.Text = "名称：查询买入股票历史记录<br>" +
                "创建时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "<br>" +
                "时间：" + qc.DatePickerFrom + "至" + qc.DatePickerTo + "<br>" +
                "买股类型：" + (qc.BuyPoint == string.Empty ? "ALL" : qc.BuyPoint) + "<br>" +
                "查询时间：" + qc.DatePickerFrom + "<br>" +
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

            this.BmBlafTableHistory.AddHeadCell(headerRow, "&nbsp;", 100).HorizontalAlign = HorizontalAlign.Center; //买入类型

            this.BmBlafTableHistory.AddHeadCell(headerRow, "&nbsp;", 70).HorizontalAlign = HorizontalAlign.Center; //查询日期

            StockQueryCondition qc = new StockQueryCondition();
            qc.DeserializeFromString(this.ViewState["_StockQueryCondition"].ToString());

            _dateList = this.SplitDate(qc.DatePickerFrom, qc.DatePickerTo);

            for (int i = _dateList.Count - 1; i >= 0; i--)
            {
                TableCell cell = this.BmBlafTableHistory.AddHeadCell(headerRow, _dateList[i].ToString(), 70 * 5);
                cell.ColumnSpan = 5;
                cell.HorizontalAlign = HorizontalAlign.Center;
            }

            //第二行
            TableRow secondRow = this.BmBlafTableHistory.AddHeadRow();
            secondRow.BorderColor = Color.Black;
            secondRow.BorderWidth = Unit.Point(1);

            this.BmBlafTableHistory.AddHeadCell(secondRow, "股票代码", 70).HorizontalAlign = HorizontalAlign.Center; //股票代码

            this.BmBlafTableHistory.AddHeadCell(secondRow, "股票名称", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTableHistory.AddHeadCell(secondRow, "买入类型", 100).HorizontalAlign = HorizontalAlign.Center; //买入类型

            this.BmBlafTableHistory.AddHeadCell(secondRow, "查询日期", 70).HorizontalAlign = HorizontalAlign.Center; //查询日期

            for (int i = _dateList.Count - 1; i >= 0; i--)
            {
                this.BmBlafTableHistory.AddHeadCell(secondRow, "开盘价", 70).HorizontalAlign = HorizontalAlign.Center;
                this.BmBlafTableHistory.AddHeadCell(secondRow, "收盘价", 70).HorizontalAlign = HorizontalAlign.Center;
                this.BmBlafTableHistory.AddHeadCell(secondRow, "最高价", 70).HorizontalAlign = HorizontalAlign.Center;
                this.BmBlafTableHistory.AddHeadCell(secondRow, "最低价", 70).HorizontalAlign = HorizontalAlign.Center;
                this.BmBlafTableHistory.AddHeadCell(secondRow, "增长率", 70).HorizontalAlign = HorizontalAlign.Center;
            }

            this.BmBlafTableHistory.Width = "100%";
            this.BmBlafTableHistory.Height = "400";
        }

        private void DrawBlafTableBody(AISTOCK_STOCK_BUY_HISTORY_DATA data)
        {
            if (data == null)
            {
                throw new CommonException("获取收入历史数据出错");
            }

            if (data.AISTOCK_STOCK_BUY_HISTORY.Count <= 0)
            {
                return;
            }
            string[,] stockData = new string[_dateList.Count, 5];

            bool flag = false;
            TableRow bodyRow;
            int count = 0;

            for (int i = 0; i < data.AISTOCK_STOCK_BUY_HISTORY.Count; i = count)
            {
                AISTOCK_STOCK_BUY_HISTORY_DATA.AISTOCK_STOCK_BUY_HISTORYRow row = data.AISTOCK_STOCK_BUY_HISTORY[i];
                flag = true;
                for (int j = _dateList.Count - 1; j >= 0; j--)
                {
                    string[] dateArr = _dateList[_dateList.Count - 1].ToString().Split('-');
                    string date = dateArr[0] + (int.Parse(dateArr[1]) < 10 ? "0" + dateArr[1] : dateArr[1]) + (int.Parse(dateArr[2]) < 10 ? "0" + dateArr[2] : dateArr[2]);
                    DataRow[] thisRow = data.AISTOCK_STOCK_BUY_HISTORY.Select("STOCK_CODE = '" + row.STOCK_CODE + "' AND STOCK_DAY = '" + date + "'");
                    if (thisRow.Length <= 0)
                    {
                        flag = false;
                    }
                    dateArr = _dateList[j].ToString().Split('-');
                    date = dateArr[0] + (int.Parse(dateArr[1]) < 10 ? "0" + dateArr[1] : dateArr[1]) + (int.Parse(dateArr[2]) < 10 ? "0" + dateArr[2] : dateArr[2]);
                    DataRow[] tmpRow = data.AISTOCK_STOCK_BUY_HISTORY.Select("STOCK_CODE = '" + row.STOCK_CODE + "' AND STOCK_DAY = '" + date + "'");
                    if (tmpRow.Length > 0)
                    {
                        count++;
                        stockData[j, 0] = ((AISTOCK_STOCK_BUY_HISTORY_DATA.AISTOCK_STOCK_BUY_HISTORYRow)tmpRow[0]).IsTODAY_BEGINNull() ? string.Empty : ((AISTOCK_STOCK_BUY_HISTORY_DATA.AISTOCK_STOCK_BUY_HISTORYRow)tmpRow[0]).TODAY_BEGIN.ToString();
                        stockData[j, 1] = ((AISTOCK_STOCK_BUY_HISTORY_DATA.AISTOCK_STOCK_BUY_HISTORYRow)tmpRow[0]).IsTODAY_ENDNull() ? string.Empty : ((AISTOCK_STOCK_BUY_HISTORY_DATA.AISTOCK_STOCK_BUY_HISTORYRow)tmpRow[0]).TODAY_END.ToString();
                        stockData[j, 2] = ((AISTOCK_STOCK_BUY_HISTORY_DATA.AISTOCK_STOCK_BUY_HISTORYRow)tmpRow[0]).IsMAX_PRICENull() ? string.Empty : ((AISTOCK_STOCK_BUY_HISTORY_DATA.AISTOCK_STOCK_BUY_HISTORYRow)tmpRow[0]).MAX_PRICE.ToString();
                        stockData[j, 3] = ((AISTOCK_STOCK_BUY_HISTORY_DATA.AISTOCK_STOCK_BUY_HISTORYRow)tmpRow[0]).IsMIN_PRICENull() ? string.Empty : ((AISTOCK_STOCK_BUY_HISTORY_DATA.AISTOCK_STOCK_BUY_HISTORYRow)tmpRow[0]).MIN_PRICE.ToString();
                        stockData[j, 4] = ((AISTOCK_STOCK_BUY_HISTORY_DATA.AISTOCK_STOCK_BUY_HISTORYRow)tmpRow[0]).IsINCREASE_PERCENTNull() ? string.Empty : ((AISTOCK_STOCK_BUY_HISTORY_DATA.AISTOCK_STOCK_BUY_HISTORYRow)tmpRow[0]).INCREASE_PERCENT.ToString();
                    }
                    else
                    {
                        for (int k = 0; k < 5; k++)
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
                    this.BmBlafTableHistory.AddCell(bodyRow, row.STOCK_TYPE, HorizontalAlign.Left);
                    this.BmBlafTableHistory.AddCell(bodyRow, row.SEARCH_DAY, HorizontalAlign.Left);
                    for (int m = _dateList.Count - 1; m >= 0; m--)
                    {
                        for (int n = 0; n < 5; n++)
                        {
                            this.BmBlafTableHistory.AddCell(bodyRow, stockData[m, n], HorizontalAlign.Left);
                        }
                    }
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