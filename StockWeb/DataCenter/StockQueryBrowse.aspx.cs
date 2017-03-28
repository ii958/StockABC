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
using AISRS.Common.Const;
using AISRS.Common.Query;

namespace AISRS.WebUI.DataCenter
{
    /// <summary>
    /// StockQueryBrowse 的摘要说明。
    /// </summary>
    public partial class StockQueryBrowse : AISRS.WebUI.PageBaseNoPermission, IDataService
    {
        protected System.Web.UI.WebControls.Label labScript;

        private StockInfo PrevInfo;

		private void Page_Load(object sender, System.EventArgs e)
		{
            this.LinkButtonQuery.LinkButtonClicked += new AISRS.WebUI.Modules.LinkButton.LinkButtonClickedHandler(LinkButtonQuery_LinkButtonClicked);
			this.labScript.Text = string.Empty;
            
			if (!IsPostBack)
			{
                InitDropDownList();
				Refresh();				
			}            
		}

        private void InitDropDownList()
        { 
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
        }

		private void Refresh()
		{
            if (this.ViewState["_StockQueryCondition"] == null)
            {
                this.RecordQueryCondition();
            }
            StockQueryCondition qc = new StockQueryCondition();
            qc.DeserializeFromString(this.ViewState["_StockQueryCondition"].ToString());

            AISTOCK_STOCK_BASEINFO_DATA data = new StockSystem().GetStockBaseInfo(qc);

            this.BmBlafTableStock.Clear();
            this.DrawBmTableHeader();
            this.DrawBmTableBody(data);  
		}

        public StockInfo GetCurrent(string stockCode)
        {
            return PrevInfo;
        }

		private void DrawBmTableHeader()
		{			
			TableRow headerRow = this.BmBlafTableStock.AddHeadRow();
			
			headerRow.Height = 25;

            this.BmBlafTableStock.AddHeadCell(headerRow, "股票代码", 60).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "股票名称", 80).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "大股东", 120).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "行业领域", 100).HorizontalAlign = HorizontalAlign.Left;
            
            this.BmBlafTableStock.AddHeadCell(headerRow, "证券市场", 60).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "省份", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "总股本数（亿股）", 100).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "收入（亿元）", 100).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "利润（亿元）", 100).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTableStock.AddHeadCell(headerRow, "净利润率", 100).HorizontalAlign = HorizontalAlign.Left;            

            this.BmBlafTableStock.Width = "100%";
		}

        private void DrawBmTableBody(AISTOCK_STOCK_BASEINFO_DATA data)
		{
            if (data == null)
            {
                return;
            }
            if (data.AISTOCK_STOCK_BASEINFO.Count <= 0)
            {
                return;
            }

            TableRow bodyRow;

            foreach (AISTOCK_STOCK_BASEINFO_DATA.AISTOCK_STOCK_BASEINFORow row in data.AISTOCK_STOCK_BASEINFO.Rows)
            {
                bodyRow = this.BmBlafTableStock.AddBodyRow();

                this.BmBlafTableStock.AddCell(bodyRow, row.STOCK_CODE, HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.STOCK_NAME, HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.STOCKER, HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.FIELD, HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.STOCK_ADDR, HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.PROVINCE, HorizontalAlign.Left);

                this.BmBlafTableStock.AddCell(bodyRow, row.TOTAL_STOCK.ToString(), HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.IsREVENUENull() ? string.Empty : row.REVENUE.ToString(), HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.IsPROFITNull() ? string.Empty : row.PROFIT.ToString(), HorizontalAlign.Left);
                this.BmBlafTableStock.AddCell(bodyRow, row.IsPROFIT_PERCENTNull() ? string.Empty : row.PROFIT_PERCENT.ToString(), HorizontalAlign.Left);
            }
		}

        /// <summary>
        /// 记录查询条件
        /// </summary>
        private void RecordQueryCondition()
        {
            string code = this.txtStockCode.Text.Trim();
            string market = this.DropDownListCategory.SelectedValue;
            string field = this.DropDownListField.SelectedValue;
            string province = this.DropDownListProvince.SelectedValue;
            StockQueryCondition qc = new StockQueryCondition();

            qc.StockCode = code;
            qc.Market = market;
            qc.Field = field;
            qc.Province = province;
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

                qc.Dispose();
            }
        }		

		private void LinkButtonQuery_LinkButtonClicked(object sender,EventArgs e)
		{
            this.RecordQueryCondition();
            this.Refresh();
            //恢复记录
            this.RestoreQueryCondition();
		}

        private AISTOCK_STOCK_INFOMATION_DATA ConvertListToDataTable(List<Stock> stock)
        {
            //AISTOCK_STOCK_STATS_DATA stockInfo = new StockSystem().GetStockData();

            AISTOCK_STOCK_INFOMATION_DATA data = new AISTOCK_STOCK_INFOMATION_DATA();

            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "text/html; charset=gb2312");
            Stream stockData;
            StreamReader reader;
            string returnData;
            int start;
            int end;
            string[] tradeData;
            decimal todayBegin;
            decimal yesterdayEnd;
            decimal current;
            decimal max;
            decimal min;
            decimal buy;
            decimal sell;
            int quantity;
            decimal money;

            foreach (var item in stock)
            {
                AISTOCK_STOCK_INFOMATION_DATA.AISTOCK_STOCK_INFORMATIONRow row = (AISTOCK_STOCK_INFOMATION_DATA.AISTOCK_STOCK_INFORMATIONRow)data.AISTOCK_STOCK_INFORMATION.NewRow();
                row.STOCK_CODE = item.StockCode;
                row.STOCK_NAME = item.StockName;
                row.STOCKER = item.Stocker;
                row.STOCK_FIELD = item.StockField;
                row.STOCK_CATEGORY = item.StockCategory;
                row.STOCK_MARKET = item.StockMarket;
                row.PROVINCE = item.Province;
                stockData = client.OpenRead("http://hq.sinajs.cn/list=" + row.STOCK_CODE);
                reader = new StreamReader(stockData, System.Text.Encoding.GetEncoding("gb2312"));
                returnData = reader.ReadToEnd();
                start = returnData.IndexOf('"');
                end = returnData.LastIndexOf('"');
                returnData = returnData.Substring(start + 1, end - start - 1);

                tradeData = returnData.Split(',');

                if (tradeData.Length == 33)
                {
                    todayBegin = decimal.Parse(tradeData[1].ToString());
                    yesterdayEnd = decimal.Parse(tradeData[2].ToString());
                    current = decimal.Parse(tradeData[3].ToString());
                    max = decimal.Parse(tradeData[4].ToString());
                    min = decimal.Parse(tradeData[5].ToString());
                    buy = decimal.Parse(tradeData[6].ToString());
                    sell = decimal.Parse(tradeData[7].ToString());
                    quantity = int.Parse(tradeData[8].ToString());
                    money = decimal.Parse(tradeData[9].ToString());
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
                }

                reader.Close();
                stockData.Close();

                row.TODAY_BEGIN = todayBegin;
                row.YESTERDAY_END = yesterdayEnd;
                row.CURRENT = current;
                row.MAX_PRICE = max;
                row.MIN_PRICE = min;
                row.BUYER = buy;
                row.SELLER = sell;
                row.QUANTITY = quantity;
                row.MONEY = money;

                data.AISTOCK_STOCK_INFORMATION.Rows.Add(row);
            }
            return data;
        }
    }
}