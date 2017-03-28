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
    public partial class IndexAnalyseBrowse : AISRS.WebUI.PageBaseNoPermission
    {
        private string url = "<a href='http://finance.sina.com.cn/realstock/company/stockcode/nc.shtml' target='_blank'>stockcode</a>";
        private void Page_Load(object sender, EventArgs e)
        {
            this.labScript.Text = string.Empty;
            this.LinkButtonQuery.LinkButtonClicked += new AISRS.WebUI.Modules.LinkButton.LinkButtonClickedHandler(LinkButtonQuery_LinkButtonClicked);
            this.LinkButtonExport.JavascriptOnClick = "return ExportExcel();";
            if (!IsPostBack)
            {
                this.hiddenExportUrl.Value = Configuration.UrlRoot + "/Index/IndexAnalyseExport.aspx";
                InitPage();
                this.RecordQueryCondition();
            }
        }

        private void InitPage()
        {
            //初始化日期
            this.DatePickerFrom.DateTime = DateTime.Now.ToShortDateString();            
            //市场
            AISTOCK_FIELD_DOMAIN_VALUE_DATA market = new StockSystem().GetDropDownList("market");
            this.DropDownListCategory.Items.Clear();
            this.DropDownListCategory.Items.Add(new ListItem("--请选择--", ""));

            foreach (AISTOCK_FIELD_DOMAIN_VALUE_DATA.AISTOCK_FIELD_DOMAIN_VALUERow row in market.AISTOCK_FIELD_DOMAIN_VALUE.Rows)
            {
                ListItem item = new ListItem();
                item.Text = row.FIELD_DOMAIN_VALUE;
                item.Value = row.FIELD_DOMAIN_VALUE;
                this.DropDownListCategory.Items.Add(new ListItem(item.Text, item.Value));
            }
            //行业
            AISTOCK_FIELD_DOMAIN_VALUE_DATA field = new StockSystem().GetDropDownList("field");
            this.DropDownListField.Items.Clear();
            this.DropDownListField.Items.Add(new ListItem("--请选择--", ""));
            foreach (AISTOCK_FIELD_DOMAIN_VALUE_DATA.AISTOCK_FIELD_DOMAIN_VALUERow row in field.AISTOCK_FIELD_DOMAIN_VALUE.Rows)
            {
                ListItem item = new ListItem();
                item.Text = row.FIELD_DOMAIN_VALUE;
                item.Value = row.FIELD_DOMAIN_VALUE;
                this.DropDownListField.Items.Add(new ListItem(item.Text, item.Value));
            }
            //省份
            AISTOCK_FIELD_DOMAIN_VALUE_DATA province = new StockSystem().GetDropDownList("province");
            this.DropDownListProvince.Items.Clear();
            this.DropDownListProvince.Items.Add(new ListItem("--请选择--", ""));
            foreach (AISTOCK_FIELD_DOMAIN_VALUE_DATA.AISTOCK_FIELD_DOMAIN_VALUERow row in province.AISTOCK_FIELD_DOMAIN_VALUE.Rows)
            {
                ListItem item = new ListItem();
                item.Text = row.FIELD_DOMAIN_VALUE;
                item.Value = row.FIELD_DOMAIN_VALUE;
                this.DropDownListProvince.Items.Add(new ListItem(item.Text, item.Value));
            }
            //指标类型
            AISTOCK_FIELD_DOMAIN_VALUE_DATA index = new StockSystem().GetDropDownList("index");
            this.DropDownListIndex.Items.Clear();
            foreach (AISTOCK_FIELD_DOMAIN_VALUE_DATA.AISTOCK_FIELD_DOMAIN_VALUERow row in index.AISTOCK_FIELD_DOMAIN_VALUE.Rows)
            {
                ListItem item = new ListItem();
                item.Text = row.FIELD_DOMAIN_VALUE;
                item.Value = row.FIELD_DOMAIN_VALUE;
                this.DropDownListIndex.Items.Add(new ListItem(item.Text, item.Value));
            }
        }

        /// <summary>
        /// 记录查询条件
        /// </summary>
        private void RecordQueryCondition()
        {
            string code = this.txtStockCode.Text.Trim();
            string name = this.txtStockName.Text.Trim();
            string market = this.DropDownListCategory.SelectedValue;
            string field = this.DropDownListField.SelectedValue;
            string province = this.DropDownListProvince.SelectedValue;
            string index = this.DropDownListIndex.SelectedValue;
            string datepickerfrom = this.DatePickerFrom.DateTime;
            StockQueryCondition qc = new StockQueryCondition();

            qc.StockCode = code;
            qc.Index = index;
            qc.StockName = name;
            qc.Market = market;
            qc.Field = field;
            qc.Province = province;
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
                this.txtStockName.Text = qc.StockName;
                this.DatePickerFrom.DateTime = qc.DatePickerFrom;

                for (int i = 0; i < this.DropDownListCategory.Items.Count; i++)
                {
                    if (this.DropDownListCategory.Items[i].Value == qc.Market)
                    {
                        this.DropDownListCategory.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < this.DropDownListField.Items.Count; i++)
                {
                    if (this.DropDownListField.Items[i].Value == qc.Field)
                    {
                        this.DropDownListField.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < this.DropDownListProvince.Items.Count; i++)
                {
                    if (this.DropDownListProvince.Items[i].Value == qc.Province)
                    {
                        this.DropDownListProvince.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < this.DropDownListIndex.Items.Count; i++)
                {
                    if (this.DropDownListIndex.Items[i].Value == qc.Index)
                    {
                        this.DropDownListIndex.SelectedIndex = i;
                        break;
                    }
                }

                qc.Dispose();
            }
        }

        private void Refresh()
        {
            StockQueryCondition qc = new StockQueryCondition();
            qc.DeserializeFromString(this.ViewState["_StockQueryCondition"].ToString());
            this.labPrompt.Text = "";

            int indexDay = 14;

            if (qc.Index.Equals("RSI(5)"))
            {
                indexDay = 5;
            }
            else if (qc.Index.Equals("RSI(9)"))
            {
                indexDay = 9;
            }
            else if (qc.Index.Equals("RSI(14)") || qc.Index.Equals("ADR(14)"))
            {
                indexDay = 11;
            }
            else if (qc.Index.Equals("WMS(10)") || qc.Index.Equals("ADR(10)"))
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

            AISTOCK_STOCK_STATS_DATA stockData = new StockSystem().GetStockData(dateTo, txtStockCode.Text.Trim());
            if (qc.Index.Equals("RSI(5)") || qc.Index.Equals("RSI(9)") || qc.Index.Equals("RSI(14)"))
            {
                string today = DateTimeFunction.ConvertDate(DateTime.Now.ToShortDateString());
                today = new StockSystem().GetToday(today);
                bool flag = false;
                if (int.Parse(dateTo) < int.Parse(today))
                {
                    flag = true;
                }
                AISTOCK_STOCK_INDEX_V_DATA dataRSI = new AISTOCK_STOCK_INDEX_V_DATA();
                dataRSI = new StockSystem().GetStockDataIndex(date, dateTo, txtStockCode.Text.Trim());

                this.BmBlafTable.Clear();
                DrawRsiTableHeader();
                DrawRsiTableBody(dataRSI, stockData, txtStockCode.Text.Trim(), flag, today, qc.DatePickerFrom, indexDay);
            }
            else if (qc.Index.Equals("WMS(10)") || qc.Index.Equals("WMS(20)"))
            {
                AISTOCK_STOCK_WMS_V_DATA dataWms = new AISTOCK_STOCK_WMS_V_DATA();
                dataWms = new StockSystem().GetStockWmsData(date, dateTo, txtStockCode.Text.Trim());
                
                this.BmBlafTable.Clear();
                DrawWmsTableHeader();
                DrawWmsTableBody(stockData, dataWms);
            }
            else if (qc.Index.Equals("ADR(10)") || qc.Index.Equals("ADR(14)"))
            {
                AISTOCK_STOCK_ADR_INDEX_V_DATA dataAdr = new AISTOCK_STOCK_ADR_INDEX_V_DATA();
                dataAdr = new StockSystem().GetStockAdrData(date, dateTo, txtStockCode.Text.Trim());

                this.BmBlafTable.Clear();
                DrawAdrTableHeader();
                DrawAdrTableBody(dataAdr);
            }
        }

        #region RSI指标
        private void DrawRsiTableHeader()
        {
            TableRow headerRow = this.BmBlafTable.AddHeadRow();

            headerRow.Height = 25;

            this.BmBlafTable.AddHeadCell(headerRow, "股票代码", 70).HorizontalAlign = HorizontalAlign.Center; //股票代码

            this.BmBlafTable.AddHeadCell(headerRow, "股票名称", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称
            
            this.BmBlafTable.AddHeadCell(headerRow, "RSI指标", 70).HorizontalAlign = HorizontalAlign.Center; //RSI指标

            this.BmBlafTable.AddHeadCell(headerRow, "昨日RSI指标", 70).HorizontalAlign = HorizontalAlign.Center; //RSI指标

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

        private void DrawRsiTableBody(AISTOCK_STOCK_INDEX_V_DATA dataRSI, AISTOCK_STOCK_STATS_DATA stockData, string code, bool isToday, string today, string date, int indexDay)
        {
            int sh = 0, sz = 0, zx = 0, bj = 0;
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
                decimal sumAB = 0;
                decimal sumBB = 0;
                decimal indexB = 0;
                AISTOCK_STOCK_STATS_DATA.AISTOCK_STOCK_INFORMATIONRow stockRow = ((AISTOCK_STOCK_STATS_DATA.AISTOCK_STOCK_INFORMATIONRow)stockData.AISTOCK_STOCK_INFORMATION.Rows[i]);
                string stockCode = stockRow.STOCK_CODE.ToString();
                if (stockCode.ToLower().StartsWith("sz300") && string.IsNullOrEmpty(txtStockCode.Text)) continue;
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
                if ((index > 80 || index < 75) && string.IsNullOrEmpty(txtStockCode.Text)) continue;
                //计算昨天的指标是否也在75-80之间，不是则抛弃
                #region [开始计算]
                string dateTo = new StockSystem().GetIndexDay(date, 1);
                dateTo = DateTimeFunction.ConvertDate(DateTime.Parse(dateTo).ToShortDateString());
                string dateFrom = new StockSystem().GetIndexDay(date, indexDay + 1);
                dateFrom = DateTimeFunction.ConvertDate(DateTime.Parse(dateFrom).ToShortDateString());

                AISTOCK_STOCK_INDEX_V_DATA dataRSIB = new StockSystem().GetStockDataIndex(dateFrom, dateTo, stockCode);
                DataRow[] rowRSIB = dataRSIB.AISTOCK_STOCK_INDEX_V.Select("STOCK_CODE = '" + stockCode + "'");
                if (rowRSIB.Length <= 0)
                {
                    continue;
                }
                for (int j = 0; j < rowRSIB.Length; j++)
                {
                    AISTOCK_STOCK_INDEX_V_DATA.AISTOCK_STOCK_INDEX_VRow row = (AISTOCK_STOCK_INDEX_V_DATA.AISTOCK_STOCK_INDEX_VRow)rowRSIB[j];
                    if (row.DIFF > 0)
                    {
                        sumAB += row.DIFF;
                    }
                    else
                    {
                        sumBB += row.DIFF;
                    }
                }
                if (sumAB - sumBB != 0)
                {
                    indexB = sumAB / (sumAB - sumBB) * 100;
                }
                if ((indexB > 80 || indexB < 75) && string.IsNullOrEmpty(txtStockCode.Text)) continue;
                #endregion[结束计算]

                bodyRow = this.BmBlafTable.AddBodyRow();
                if (stockRow.STOCK_CODE.Contains("sh600")) sh++;
                else if (stockRow.STOCK_CODE.Contains("sz000")) sz++;
                else if (stockRow.STOCK_CODE.Contains("sz002")) zx++;
                else if (stockRow.STOCK_CODE.Contains("sh601") || stockRow.STOCK_CODE.Contains("sh603")) bj++;
               
                
                this.BmBlafTable.AddCell(bodyRow, url.Replace("stockcode", stockRow.STOCK_CODE), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, stockRow.STOCK_NAME, HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, index.ToString("0.00"), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, indexB.ToString("0.00"), HorizontalAlign.Left);
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
                    if (stockRow.TODAY_END > 0) { 
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
            int sum = sh + sz +zx + bj;
            this.labPrompt.Text = "渐强状态：共" + sum + "支，其中上市" + sh + "支，深市" + sz + "支，中小板" + zx + "，北京" + bj + "支<br/>";
        }
        #endregion

        #region 威廉指数
        private void DrawWmsTableHeader()
        {
            TableRow headerRow = this.BmBlafTable.AddHeadRow();

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

        #region 涨跌比率
        private void DrawAdrTableHeader()
        {
            TableRow headerRow = this.BmBlafTable.AddHeadRow();

            headerRow.Height = 25;

            this.BmBlafTable.AddHeadCell(headerRow, "增长家数", 100).HorizontalAlign = HorizontalAlign.Center; 

            this.BmBlafTable.AddHeadCell(headerRow, "下跌家数", 100).HorizontalAlign = HorizontalAlign.Center; 

            this.BmBlafTable.AddHeadCell(headerRow, "涨跌比率", 100).HorizontalAlign = HorizontalAlign.Center; 

            this.BmBlafTable.Width = "300";
        }

        private void DrawAdrTableBody(AISTOCK_STOCK_ADR_INDEX_V_DATA data)
        {
            if (data == null)
            {
                return;
            }
            if (data.AISTOCK_STOCK_ADR_INDEX.Count <= 0)
            {
                return;
            }
            TableRow bodyRow;
            bodyRow = this.BmBlafTable.AddBodyRow();
            DataRow[] row = data.AISTOCK_STOCK_ADR_INDEX.Select("DSC = 'INCREASE'");
            AISTOCK_STOCK_ADR_INDEX_V_DATA.AISTOCK_STOCK_ADR_INDEXRow increase = (AISTOCK_STOCK_ADR_INDEX_V_DATA.AISTOCK_STOCK_ADR_INDEXRow)row[0];
            row = data.AISTOCK_STOCK_ADR_INDEX.Select("DSC = 'DECREASE'");
            AISTOCK_STOCK_ADR_INDEX_V_DATA.AISTOCK_STOCK_ADR_INDEXRow decrease = (AISTOCK_STOCK_ADR_INDEX_V_DATA.AISTOCK_STOCK_ADR_INDEXRow)row[0];
            decimal totalIncrease = increase.TOTAL;
            decimal totalDecrease = decrease.TOTAL;
            decimal percent = 0;
            if (totalDecrease != 0)
            {
                percent = totalIncrease / totalDecrease;
            }
            this.BmBlafTable.AddCell(bodyRow, totalIncrease.ToString(), HorizontalAlign.Left);
            this.BmBlafTable.AddCell(bodyRow, totalDecrease.ToString(), HorizontalAlign.Left);
            this.BmBlafTable.AddCell(bodyRow, percent.ToString(), HorizontalAlign.Left);
        }
        #endregion

        private void LinkButtonQuery_LinkButtonClicked(object sender, EventArgs e)
        {
            this.RecordQueryCondition();
            this.Refresh();
        }
    }
}