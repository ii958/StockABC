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
    public partial class StockBuyBrowse : AISRS.WebUI.PageBaseNoPermission
    {
        private string url = "<a href='http://finance.sina.com.cn/realstock/company/stockcode/nc.shtml' target='_blank'>stockcode</a>";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.labScript.Text = string.Empty;
            this.LinkButtonQuery.LinkButtonClicked += new AISRS.WebUI.Modules.LinkButton.LinkButtonClickedHandler(LinkButtonQuery_LinkButtonClicked);
            this.LinkButtonForecast.LinkButtonClicked += new AISRS.WebUI.Modules.LinkButton.LinkButtonClickedHandler(LinkButtonForecast_LinkButtonClicked);
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

            //买入类型
            AISTOCK_FIELD_DOMAIN_VALUE_DATA market = new StockSystem().GetDropDownList("buy");
            this.DropDownListType.Items.Clear();
            this.DropDownListType.Items.Add(new ListItem("--请选择--", ""));

            foreach (AISTOCK_FIELD_DOMAIN_VALUE_DATA.AISTOCK_FIELD_DOMAIN_VALUERow row in market.AISTOCK_FIELD_DOMAIN_VALUE.Rows)
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
            string datepickerfrom = this.DatePickerFrom.DateTime;
            string type = this.DropDownListType.SelectedValue;
            StockQueryCondition qc = new StockQueryCondition();

            qc.DatePickerFrom = datepickerfrom;
            qc.BuyPoint = type;
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

                this.DatePickerFrom.DateTime = qc.DatePickerFrom;

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

        private void Refresh()
        {
            StockQueryCondition qc = new StockQueryCondition();
            qc.DeserializeFromString(this.ViewState["_StockQueryCondition"].ToString());

            AISTOCK_STOCK_BASEINFO_DATA baseData = new StockSystem().GetStockBaseInfoWithoutCondition();

            int indexDay = 5;

            if (qc.BuyPoint.Equals("低档五连阳"))
            {
                indexDay = 4;
            }
            else if (qc.BuyPoint.Equals("双针探底") || qc.BuyPoint.Equals("锤子线探底"))
            {
                indexDay = 1;
            }
            else if (qc.BuyPoint.Equals("十字星探底"))
            {
                indexDay = 3;
            }
            else if (qc.BuyPoint.Equals("早晨之星"))
            {
                indexDay = 2;
            }

            string dateTo = DateTimeFunction.ConvertDate(qc.DatePickerFrom);
            string date = new StockSystem().GetIndexDay(qc.DatePickerFrom, indexDay);
            date = DateTimeFunction.ConvertDate(DateTime.Parse(date).ToShortDateString());

            if (qc.BuyPoint.Equals("低档五连阳"))
            {
                AISTOCK_STOCK_LOW_FIVE_V_DATA data = new StockSystem().GetLowFiveData(date, dateTo);
                this.BmBlafTable.Clear();
                DrawLowFiveTableHeader();
                DrawLowFiveTableBody(data, baseData);
            }
            else if (qc.BuyPoint.Equals("双针探底"))
            {
                AISTOCK_STOCK_TWO_PIN_V_DATA data = new StockSystem().GetTwoPinData(date, dateTo);
                this.BmBlafTable.Clear();
                DrawTwoPinTableHeader();
                DrawTwoPinTableBody(data);
            }
            else if (qc.BuyPoint.Equals("十字星探底"))
            {
                AISTOCK_STOCK_SINGLE_PIN_V_DATA data = new StockSystem().GetSinglePinData(date, dateTo);
                this.BmBlafTable.Clear();
                DrawSinglePinTableHeader();
                DrawSinglePinTableBody(data);
            }
            else if (qc.BuyPoint.Equals("锤子线探底"))
            {
                AISTOCK_STOCK_HAMMER_V_DATA data = new StockSystem().GetHammerData(date, dateTo);
                this.BmBlafTable.Clear();
                DrawHammerHeader();
                DrawHammerTableBody(data);
            }
            else if (qc.BuyPoint.Equals("早晨之星"))
            {
                string twoDate = new StockSystem().GetIndexDay(qc.DatePickerFrom, 1);
                twoDate = DateTimeFunction.ConvertDate(DateTime.Parse(twoDate).ToShortDateString());
                AISTOCK_STOCK_MORNING_STAR_V_DATA data = new StockSystem().GetMorningStarData(date, twoDate, dateTo);
                this.BmBlafTable.Clear();
                DrawMorningStarHeader();
                DrawMorningStarTableBody(data);
            }
        }

        private void DrawMorningStarHeader()
        {
            TableRow headerRow = this.BmBlafTable.AddHeadRow();

            headerRow.Height = 25;

            this.BmBlafTable.AddHeadCell(headerRow, "股票代码", 70).HorizontalAlign = HorizontalAlign.Center; //股票代码

            this.BmBlafTable.AddHeadCell(headerRow, "股票名称", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTable.AddHeadCell(headerRow, "前日开盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "前日收盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "前日最低价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "前日涨跌幅", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "昨日最高价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "昨日开盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "昨日收盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "昨日最低价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "今日开盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "今日收盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "今日涨跌幅", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.Width = "1000";
        }

        private void DrawMorningStarTableBody(AISTOCK_STOCK_MORNING_STAR_V_DATA data)
        {
            if (data == null)
            {
                return;
            }
            if (data.AISTOCK_STOCK_MORNING_STAR_V.Count <= 0)
            {
                return;
            }
            TableRow bodyRow;
            foreach (AISTOCK_STOCK_MORNING_STAR_V_DATA.AISTOCK_STOCK_MORNING_STAR_VRow row in data.AISTOCK_STOCK_MORNING_STAR_V.Rows)
            {
                bodyRow = this.BmBlafTable.AddBodyRow();
                this.BmBlafTable.AddCell(bodyRow, url.Replace("stockcode", row.STOCK_CODE), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.STOCK_NAME, HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.ONE_BEGIN.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.ONE_END.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.ONE_MIN_PRICE.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.ONE_PERCENT.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.TWO_MAX_PRICE.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.TWO_BEGIN.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.TWO_END.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.TWO_MIN_PRICE.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.THREE_BEGIN.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.THREE_END.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.THREE_PERCENT.ToString(), HorizontalAlign.Left);
            }
        }

        private void DrawHammerHeader()
        {
            TableRow headerRow = this.BmBlafTable.AddHeadRow();

            headerRow.Height = 25;

            this.BmBlafTable.AddHeadCell(headerRow, "股票代码", 70).HorizontalAlign = HorizontalAlign.Center; //股票代码

            this.BmBlafTable.AddHeadCell(headerRow, "股票名称", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTable.AddHeadCell(headerRow, "今日开盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "今日收盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "锤子线开盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "锤子线收盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "锤子线最低价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "今日收盘价与<br>锤子线最高价<br>的差价", 100).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "今日收盘价与<br>锤子线最高价<br>的差价率", 100).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.Width = "1000";
        }

        private void DrawHammerTableBody(AISTOCK_STOCK_HAMMER_V_DATA data)
        {
            if (data == null)
            {
                return;
            }
            if (data.AISTOCK_STOCK_HAMMER_V.Count <= 0)
            {
                return;
            }
            TableRow bodyRow;
            foreach (AISTOCK_STOCK_HAMMER_V_DATA.AISTOCK_STOCK_HAMMER_VRow row in data.AISTOCK_STOCK_HAMMER_V.Rows)
            {
                bodyRow = this.BmBlafTable.AddBodyRow();
                this.BmBlafTable.AddCell(bodyRow, url.Replace("stockcode", row.STOCK_CODE), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.STOCK_NAME, HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.TODAY_BEGIN.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.TODAY_END.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.HAMMER_BEGIN.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.HAMMER_END.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.HAMMER_MIN.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.DIFF.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.DIFF_PERCENT.ToString("0.0000"), HorizontalAlign.Left);
            }
        }

        private void DrawSinglePinTableHeader()
        {
            TableRow headerRow = this.BmBlafTable.AddHeadRow();

            headerRow.Height = 25;

            this.BmBlafTable.AddHeadCell(headerRow, "股票代码", 70).HorizontalAlign = HorizontalAlign.Center; //股票代码

            this.BmBlafTable.AddHeadCell(headerRow, "股票名称", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTable.AddHeadCell(headerRow, "今日开盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "今日收盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "十字星收盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "十字星最高价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "十字星最低价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "今日收盘价与<br>十字星最高价<br>的差价", 100).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "今日收盘价与<br>十字星最高价<br>的差价率", 100).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.Width = "1000";
        }

        private void DrawSinglePinTableBody(AISTOCK_STOCK_SINGLE_PIN_V_DATA data)
        {
            if (data == null)
            {
                return;
            }
            if (data.AISTOCK_STOCK_SINGLE_PIN_V.Count <= 0)
            {
                return;
            }
            TableRow bodyRow;
            foreach (AISTOCK_STOCK_SINGLE_PIN_V_DATA.AISTOCK_STOCK_SINGLE_PIN_VRow row in data.AISTOCK_STOCK_SINGLE_PIN_V.Rows)
            {
                bodyRow = this.BmBlafTable.AddBodyRow();
                this.BmBlafTable.AddCell(bodyRow, url.Replace("stockcode", row.STOCK_CODE), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.STOCK_NAME, HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.TODAY_BEGIN.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.TODAY_END.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.PIN_BEGIN.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.PIN_MAX.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.PIN_MIN.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.DIFF.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.DIFF_PERCENT.ToString("0.0000"), HorizontalAlign.Left);
            }
        }

        private void DrawTwoPinTableHeader()
        {
            TableRow headerRow = this.BmBlafTable.AddHeadRow();

            headerRow.Height = 25;

            this.BmBlafTable.AddHeadCell(headerRow, "股票代码", 70).HorizontalAlign = HorizontalAlign.Center; //股票代码

            this.BmBlafTable.AddHeadCell(headerRow, "股票名称", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTable.AddHeadCell(headerRow, "昨天日期", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "昨天开盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "昨天收盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "昨天最低价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "今天日期", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "今天开盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "今天收盘价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "今天最低价", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.Width = "1000";
        }

        private void DrawTwoPinTableBody(AISTOCK_STOCK_TWO_PIN_V_DATA data)
        {
            if (data == null)
            {
                return;
            }
            if (data.AISTOCK_STOCK_TWO_PIN_V.Count <= 0)
            {
                return;
            }
            TableRow bodyRow;
            foreach (AISTOCK_STOCK_TWO_PIN_V_DATA.AISTOCK_STOCK_TWO_PIN_VRow row in data.AISTOCK_STOCK_TWO_PIN_V.Rows)
            {
                bodyRow = this.BmBlafTable.AddBodyRow();
                this.BmBlafTable.AddCell(bodyRow, url.Replace("stockcode", row.STOCK_CODE), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.STOCK_NAME, HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.YESTERDAY, HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.YESTERDAY_BEGIN.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.YESTERDAY_END.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.YESTERDAY_MIN_PRICE.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.TODAY, HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.TODAY_BEGIN.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.TODAY_END.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.TODAY_MIN_PRICE.ToString(), HorizontalAlign.Left);
            }
        }

        private void DrawLowFiveTableHeader()
        {
            TableRow headerRow = this.BmBlafTable.AddHeadRow();

            headerRow.Height = 25;

            this.BmBlafTable.AddHeadCell(headerRow, "股票代码", 70).HorizontalAlign = HorizontalAlign.Center; //股票代码

            this.BmBlafTable.AddHeadCell(headerRow, "股票名称", 70).HorizontalAlign = HorizontalAlign.Center; //股票名称

            this.BmBlafTable.AddHeadCell(headerRow, "最低增长率", 70).HorizontalAlign = HorizontalAlign.Center; //RSI指标

            this.BmBlafTable.Width = "1000";
        }

        private void DrawLowFiveTableBody(AISTOCK_STOCK_LOW_FIVE_V_DATA data, AISTOCK_STOCK_BASEINFO_DATA baseData)
        {
            if (data == null)
            {
                return;
            }
            if (data.AISTOCK_STOCK_LOW_FIVE_V.Count <= 0)
            {
                return;
            }
            TableRow bodyRow;                 
            foreach (AISTOCK_STOCK_LOW_FIVE_V_DATA.AISTOCK_STOCK_LOW_FIVE_VRow row in data.AISTOCK_STOCK_LOW_FIVE_V.Rows)
            {
                DataRow[] stockInfo = baseData.AISTOCK_STOCK_BASEINFO.Select("STOCK_CODE = '" + row.STOCK_CODE + "'");
                bodyRow = this.BmBlafTable.AddBodyRow();
                this.BmBlafTable.AddCell(bodyRow, url.Replace("stockcode",row.STOCK_CODE), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, ((AISTOCK_STOCK_BASEINFO_DATA.AISTOCK_STOCK_BASEINFORow)stockInfo[0]).STOCK_NAME, HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.MIN_PERCENT.ToString(), HorizontalAlign.Left);
            }
        }

        private bool IsValidDate()
        {
            StockQueryCondition qc = new StockQueryCondition();
            qc.DeserializeFromString(this.ViewState["_StockQueryCondition"].ToString());
            string dateTo = DateTimeFunction.ConvertDate1(qc.DatePickerFrom);
            
            return new StockSystem().IsValidDate(dateTo);
        }

        private void LinkButtonQuery_LinkButtonClicked(object sender, EventArgs e)
        {
            this.RecordQueryCondition();
            if (IsValidDate())
            {
                this.Refresh();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "sa", "<script>alert('查询日为休市日');</script>");
            }
        }

        private void LinkButtonForecast_LinkButtonClicked(object sender, EventArgs e)
        {
            this.RecordQueryCondition();
            if (IsValidDate())
            {
                this.Stats();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "sa", "<script>alert('查询日为休市日');</script>");
            }
        }

        private void Stats()
        {
            StockQueryCondition qc = new StockQueryCondition();
            qc.DeserializeFromString(this.ViewState["_StockQueryCondition"].ToString());

            AISTOCK_FIELD_DOMAIN_VALUE_DATA buy = new StockSystem().GetDropDownList("buy");
            int indexDay = 5;

            string dateTo = DateTimeFunction.ConvertDate(qc.DatePickerFrom);
            string date = string.Empty;
            
            AISTOCK_STOCK_BUY_STATS_DATA stats = new AISTOCK_STOCK_BUY_STATS_DATA();
            AISTOCK_STOCK_BASEINFO_DATA baseData = new StockSystem().GetStockBaseInfoWithoutCondition();
            foreach (AISTOCK_FIELD_DOMAIN_VALUE_DATA.AISTOCK_FIELD_DOMAIN_VALUERow row in buy.AISTOCK_FIELD_DOMAIN_VALUE.Rows)
            {
                string value = row.FIELD_DOMAIN_VALUE;
                if (value.Equals("低档五连阳"))
                {
                    indexDay = 4;
                    date = new StockSystem().GetIndexDay(qc.DatePickerFrom, indexDay);
                    date = DateTimeFunction.ConvertDate(DateTime.Parse(date).ToShortDateString());
                    
                    AISTOCK_STOCK_LOW_FIVE_V_DATA data = new StockSystem().GetLowFiveData(date, dateTo);
                    foreach (AISTOCK_STOCK_LOW_FIVE_V_DATA.AISTOCK_STOCK_LOW_FIVE_VRow dataRow in data.AISTOCK_STOCK_LOW_FIVE_V.Rows)
                    {
                        AISTOCK_STOCK_BUY_STATS_DATA.AISTOCK_STOCK_BUY_STATSRow saveRow = stats.AISTOCK_STOCK_BUY_STATS.NewAISTOCK_STOCK_BUY_STATSRow();
                        Guid id = Guid.NewGuid();
                        saveRow.STOCK_BUY_ID = id.ToString().ToUpper();
                        saveRow.STOCK_CODE = dataRow.STOCK_CODE;
                        DataRow[] tmpRow = baseData.AISTOCK_STOCK_BASEINFO.Select("STOCK_CODE = '" + dataRow.STOCK_CODE + "'");
                        saveRow.STOCK_NAME = ((AISTOCK_STOCK_BASEINFO_DATA.AISTOCK_STOCK_BASEINFORow)tmpRow[0]).STOCK_NAME;
                        saveRow.STOCK_TYPE = "低档五连阳";
                        saveRow.STOCK_DAY = dateTo;
                        stats.AISTOCK_STOCK_BUY_STATS.AddAISTOCK_STOCK_BUY_STATSRow(saveRow);
                    }
                }
                else if (value.Equals("双针探底"))
                {
                    indexDay = 1;
                    date = new StockSystem().GetIndexDay(qc.DatePickerFrom, indexDay);
                    date = DateTimeFunction.ConvertDate(DateTime.Parse(date).ToShortDateString());

                    AISTOCK_STOCK_TWO_PIN_V_DATA data = new StockSystem().GetTwoPinData(date, dateTo);
                    foreach (AISTOCK_STOCK_TWO_PIN_V_DATA.AISTOCK_STOCK_TWO_PIN_VRow dataRow in data.AISTOCK_STOCK_TWO_PIN_V.Rows)
                    {
                        AISTOCK_STOCK_BUY_STATS_DATA.AISTOCK_STOCK_BUY_STATSRow saveRow = stats.AISTOCK_STOCK_BUY_STATS.NewAISTOCK_STOCK_BUY_STATSRow();
                        Guid id = Guid.NewGuid();
                        saveRow.STOCK_BUY_ID = id.ToString().ToUpper();
                        saveRow.STOCK_CODE = dataRow.STOCK_CODE;
                        saveRow.STOCK_NAME = dataRow.STOCK_NAME;
                        saveRow.STOCK_TYPE = "双针探底";
                        saveRow.STOCK_DAY = dateTo;
                        stats.AISTOCK_STOCK_BUY_STATS.AddAISTOCK_STOCK_BUY_STATSRow(saveRow);
                    }
                }
                else if (value.Equals("十字星探底"))
                {
                    indexDay = 3;
                    date = new StockSystem().GetIndexDay(qc.DatePickerFrom, indexDay);
                    date = DateTimeFunction.ConvertDate(DateTime.Parse(date).ToShortDateString());

                    AISTOCK_STOCK_SINGLE_PIN_V_DATA data = new StockSystem().GetSinglePinData(date, dateTo);
                    foreach (AISTOCK_STOCK_SINGLE_PIN_V_DATA.AISTOCK_STOCK_SINGLE_PIN_VRow dataRow in data.AISTOCK_STOCK_SINGLE_PIN_V.Rows)
                    {
                        AISTOCK_STOCK_BUY_STATS_DATA.AISTOCK_STOCK_BUY_STATSRow saveRow = stats.AISTOCK_STOCK_BUY_STATS.NewAISTOCK_STOCK_BUY_STATSRow();
                        Guid id = Guid.NewGuid();
                        saveRow.STOCK_BUY_ID = id.ToString().ToUpper();
                        saveRow.STOCK_CODE = dataRow.STOCK_CODE;
                        saveRow.STOCK_NAME = dataRow.STOCK_NAME;
                        saveRow.STOCK_TYPE = "十字星探底";
                        saveRow.STOCK_DAY = dateTo;
                        stats.AISTOCK_STOCK_BUY_STATS.AddAISTOCK_STOCK_BUY_STATSRow(saveRow);
                    }
                }
                else if (value.Equals("锤子线探底"))
                {
                    indexDay = 1;
                    date = new StockSystem().GetIndexDay(qc.DatePickerFrom, indexDay);
                    date = DateTimeFunction.ConvertDate(DateTime.Parse(date).ToShortDateString());

                    AISTOCK_STOCK_HAMMER_V_DATA data = new StockSystem().GetHammerData(date, dateTo);
                    foreach (AISTOCK_STOCK_HAMMER_V_DATA.AISTOCK_STOCK_HAMMER_VRow dataRow in data.AISTOCK_STOCK_HAMMER_V.Rows)
                    {
                        AISTOCK_STOCK_BUY_STATS_DATA.AISTOCK_STOCK_BUY_STATSRow saveRow = stats.AISTOCK_STOCK_BUY_STATS.NewAISTOCK_STOCK_BUY_STATSRow();
                        Guid id = Guid.NewGuid();
                        saveRow.STOCK_BUY_ID = id.ToString().ToUpper();
                        saveRow.STOCK_CODE = dataRow.STOCK_CODE;
                        saveRow.STOCK_NAME = dataRow.STOCK_NAME;
                        saveRow.STOCK_TYPE = "锤子线探底";
                        saveRow.STOCK_DAY = dateTo;
                        stats.AISTOCK_STOCK_BUY_STATS.AddAISTOCK_STOCK_BUY_STATSRow(saveRow);
                    }
                }
                else if (value.Equals("早晨之星"))
                {
                    indexDay = 2;
                    date = new StockSystem().GetIndexDay(qc.DatePickerFrom, indexDay);
                    date = DateTimeFunction.ConvertDate(DateTime.Parse(date).ToShortDateString());

                    string twoDate = new StockSystem().GetIndexDay(qc.DatePickerFrom, 1);
                    twoDate = DateTimeFunction.ConvertDate(DateTime.Parse(twoDate).ToShortDateString());
                    AISTOCK_STOCK_MORNING_STAR_V_DATA data = new StockSystem().GetMorningStarData(date, twoDate, dateTo);
                    foreach (AISTOCK_STOCK_MORNING_STAR_V_DATA.AISTOCK_STOCK_MORNING_STAR_VRow dataRow in data.AISTOCK_STOCK_MORNING_STAR_V.Rows)
                    {
                        AISTOCK_STOCK_BUY_STATS_DATA.AISTOCK_STOCK_BUY_STATSRow saveRow = stats.AISTOCK_STOCK_BUY_STATS.NewAISTOCK_STOCK_BUY_STATSRow();
                        Guid id = Guid.NewGuid();
                        saveRow.STOCK_BUY_ID = id.ToString().ToUpper();
                        saveRow.STOCK_CODE = dataRow.STOCK_CODE;
                        saveRow.STOCK_NAME = dataRow.STOCK_NAME;
                        saveRow.STOCK_TYPE = "早晨之星";
                        saveRow.STOCK_DAY = dateTo;
                        stats.AISTOCK_STOCK_BUY_STATS.AddAISTOCK_STOCK_BUY_STATSRow(saveRow);
                    }
                }
            }
            new StockSystem().SaveStockStatsData(stats);
        }
    }
}