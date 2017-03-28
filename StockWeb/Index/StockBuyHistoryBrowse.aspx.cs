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
    /// StockBuyHistoryBrowse 的摘要说明。
    /// </summary>
    public partial class StockBuyHistoryBrowse : AISRS.WebUI.PageBaseNoPermission
    {
        private ArrayList _dateList;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.labScript.Text = string.Empty;
            this.LinkButtonQuery.LinkButtonClicked += new AISRS.WebUI.Modules.LinkButton.LinkButtonClickedHandler(LinkButtonQuery_LinkButtonClicked);
            this.LinkButtonExport.JavascriptOnClick = "return ExportExcel();";
            if (!IsPostBack)
            {
                this.hiddenExportUrl.Value = Configuration.UrlRoot + "/Index/StockBuyHistoryExport.aspx";
                InitPage();
                this.RecordQueryCondition();
            }
        }

        private void InitPage()
        {
            //初始化日期
            this.DatePickerFrom.DateTime = DateTime.Now.ToShortDateString();
            this.DatePickerTo.DateTime = DateTime.Now.ToShortDateString();

            this.DatePickerSearch.DateTime = DateTime.Now.ToShortDateString();

            //买入类型
            AISTOCK_FIELD_DOMAIN_VALUE_DATA type = new StockSystem().GetDropDownList("buy");
            this.DropDownListType.Items.Clear();
            this.DropDownListType.Items.Add(new ListItem("--请选择--", ""));

            foreach (AISTOCK_FIELD_DOMAIN_VALUE_DATA.AISTOCK_FIELD_DOMAIN_VALUERow row in type.AISTOCK_FIELD_DOMAIN_VALUE.Rows)
            {
                ListItem item = new ListItem();
                item.Text = row.FIELD_DOMAIN_VALUE;
                item.Value = row.FIELD_DOMAIN_VALUE;
                this.DropDownListType.Items.Add(new ListItem(item.Text, item.Value));
            }            
        }

        /// <summary>
        /// 记录查询条件
        /// </summary>
        private void RecordQueryCondition()
        {
            string code = this.txtStockCode.Text.Trim();
            string name = this.txtStockName.Text.Trim();
            string datepickerfrom = this.DatePickerFrom.DateTime;
            string datepickerto = this.DatePickerTo.DateTime;
            string datepickersearch = this.DatePickerSearch.DateTime;
            string type = this.DropDownListType.SelectedValue;
            StockQueryCondition qc = new StockQueryCondition();

            qc.DatePickerFrom = datepickerfrom;
            qc.DatePickerTo = datepickerto;
            qc.DatePickerSearch = datepickersearch;
            qc.StockCode = code;
            qc.StockName = name;
            qc.BuyPoint = type;
            this.ViewState["_StockQueryCondition"] = qc.SerializeToString();
        }

        private void RestoreQueryCondition()
        {
            if (this.ViewState["_StockQueryCondition"].ToString() != string.Empty)
            {
                StockQueryCondition qc = new StockQueryCondition();

                qc.DeserializeFromString(this.ViewState["_StockQueryCondition"].ToString());

                this.txtStockCode.Text = qc.StockCode;
                this.txtStockName.Text = qc.StockName;
                this.DatePickerFrom.DateTime = qc.DatePickerFrom;
                this.DatePickerTo.DateTime = qc.DatePickerTo;
                this.DatePickerSearch.DateTime = qc.DatePickerSearch;

                for (int i = 0; i < this.DropDownListType.Items.Count; i++)
                {
                    if (this.DropDownListType.Items[i].Value == qc.BuyPoint)
                    {
                        this.DropDownListType.SelectedIndex = i;
                        break;
                    }
                }
                
                qc.Dispose();
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
            DateTime dtFrom = DateTime.Parse(this.DatePickerFrom.DateTime.ToString());
            DateTime dtTo = DateTime.Parse(this.DatePickerTo.DateTime.ToString());
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

        private void Refresh()
        {
            StockQueryCondition qc = new StockQueryCondition();
            qc.DeserializeFromString(this.ViewState["_StockQueryCondition"].ToString());

            DateTime dateFrom = DateTime.Parse(this.DatePickerFrom.DateTime.ToString());
            DateTime dateTo = DateTime.Parse(this.DatePickerTo.DateTime.ToString());

            if (dateFrom > dateTo)
            {
                this.RestoreQueryCondition();
                this.labScript.Text = JavaScriptFunction.Alert("选择的查询时间段不对，请重新选择。");
                return;
            }

            AISTOCK_STOCK_BUY_HISTORY_DATA data = new StockSystem().GetBuyHistoryData(qc);
            this.BmBlafTableHistory.Clear();
            this.BmBlafTableHistory.IsEnableScroll = true;
            this.BmBlafTableHistory.FreezeColumnCount = 4;

            if (data == null || data.AISTOCK_STOCK_BUY_HISTORY.Count <= 0)
            {
                //this.BmBlafTableHistory.TitleRowCount = 1;
                this.DrawBlafTableHeader();
            }
            else
            {
                //this.BmBlafTableHistory.TitleRowCount = 2;
                this.DrawBlafTableHeader();
                this.DrawBlafTableBody(data);
            }

            //恢复记录
            this.RestoreQueryCondition();
        }

        private void DrawBlafTableHeader()
        {
            //第一行
            TableRow headerRow = this.BmBlafTableHistory.AddHeadRow();

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

        private void LinkButtonQuery_LinkButtonClicked(object sender, EventArgs e)
        {
            this.RecordQueryCondition();
            this.Refresh();
        }
    }
}