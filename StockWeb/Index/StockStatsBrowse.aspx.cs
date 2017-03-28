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
    public partial class StockStatsBrowse : AISRS.WebUI.PageBaseNoPermission
    {
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
            string datepickerfrom = this.DatePickerFrom.DateTime;
            StockQueryCondition qc = new StockQueryCondition();

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

                this.DatePickerFrom.DateTime = qc.DatePickerFrom;

                qc.Dispose();
            }
        }

        private void Refresh()
        {
            StockQueryCondition qc = new StockQueryCondition();
            qc.DeserializeFromString(this.ViewState["_StockQueryCondition"].ToString());

            string date = DateTimeFunction.ConvertDate(qc.DatePickerFrom);

            AISTOCK_STATS_PERCENT_V_DATA data = new StockSystem().GetStockStatsVData(date);
            DataTable leader = new StockSystem().GetLeaderStock(date);

            this.BmBlafTable.Clear();
            DrawTableHeader();
            DrawTableBody(data, leader);
            this.RestoreQueryCondition();
        }

        private void DrawTableHeader()
        {
            TableRow headerRow = this.BmBlafTable.AddHeadRow();

            headerRow.Height = 25;

            this.BmBlafTable.AddHeadCell(headerRow, "证监会行业", 120).HorizontalAlign = HorizontalAlign.Center; 

            this.BmBlafTable.AddHeadCell(headerRow, "总上市数", 70).HorizontalAlign = HorizontalAlign.Center; 

            this.BmBlafTable.AddHeadCell(headerRow, "涨（家）", 70).HorizontalAlign = HorizontalAlign.Center; 

            this.BmBlafTable.AddHeadCell(headerRow, "跌（家）", 70).HorizontalAlign = HorizontalAlign.Center; 

            this.BmBlafTable.AddHeadCell(headerRow, "增长率", 70).HorizontalAlign = HorizontalAlign.Center; 

            this.BmBlafTable.AddHeadCell(headerRow, "领涨股", 80).HorizontalAlign = HorizontalAlign.Center; 

            this.BmBlafTable.AddHeadCell(headerRow, "领涨股名称", 100).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "涨幅", 70).HorizontalAlign = HorizontalAlign.Center;

            this.BmBlafTable.AddHeadCell(headerRow, "行业", 100).HorizontalAlign = HorizontalAlign.Center; 

            this.BmBlafTable.AddHeadCell(headerRow, "省份", 100).HorizontalAlign = HorizontalAlign.Center; 

            this.BmBlafTable.AddHeadCell(headerRow, "K线图", 100).HorizontalAlign = HorizontalAlign.Center; 

            this.BmBlafTable.Width = "1000";
        }

        private void DrawTableBody(AISTOCK_STATS_PERCENT_V_DATA data, DataTable leader)
        {
            string field = string.Empty;
            string increasePercent = string.Empty;
            TableRow bodyRow; 
            for (int i = 0; i < leader.Rows.Count; i++)
            {
                field = leader.Rows[i][0].ToString();
                DataRow[] row = data.AISTOCK_STATS_PERCENT_V.Select("FIELD_MAIN = '" + field + "'");
                AISTOCK_STATS_PERCENT_V_DATA.AISTOCK_STATS_PERCENT_VRow thisRow = (AISTOCK_STATS_PERCENT_V_DATA.AISTOCK_STATS_PERCENT_VRow)row[0];
                bodyRow = this.BmBlafTable.AddBodyRow();
                this.BmBlafTable.AddCell(bodyRow, thisRow.FIELD_MAIN.ToString(), HorizontalAlign.Left);//证监会行业
                this.BmBlafTable.AddCell(bodyRow, thisRow.IsTOTALNull() ? string.Empty :thisRow.TOTAL.ToString(), HorizontalAlign.Left);//
                this.BmBlafTable.AddCell(bodyRow, thisRow.IsTOTAL_INCREASENull() ? string.Empty : thisRow.TOTAL_INCREASE.ToString(), HorizontalAlign.Left);//
                this.BmBlafTable.AddCell(bodyRow, thisRow.IsTOTAL_DECREASENull() ? string.Empty : thisRow.TOTAL_DECREASE.ToString(), HorizontalAlign.Left);//
                if (!thisRow.IsTOTALNull() && !thisRow.IsTOTAL_INCREASENull())
                {
                    this.BmBlafTable.AddCell(bodyRow, (decimal.Parse(thisRow.TOTAL_INCREASE.ToString()) / decimal.Parse(thisRow.TOTAL.ToString())).ToString("0.0000"), HorizontalAlign.Left);//增长率
                }
                else
                {
                    this.BmBlafTable.AddCell(bodyRow, string.Empty, HorizontalAlign.Left);
                }
                this.BmBlafTable.AddCell(bodyRow, leader.Rows[i][2].ToString(), HorizontalAlign.Left);//股票代码
                this.BmBlafTable.AddCell(bodyRow, leader.Rows[i][3].ToString(), HorizontalAlign.Left);//股票名称
                this.BmBlafTable.AddCell(bodyRow, leader.Rows[i][1].ToString(), HorizontalAlign.Left);//涨幅
                this.BmBlafTable.AddCell(bodyRow, leader.Rows[i][4].ToString(), HorizontalAlign.Left);//标准行业
                this.BmBlafTable.AddCell(bodyRow, leader.Rows[i][5].ToString(), HorizontalAlign.Left);//省份
                this.BmBlafTable.AddCell(bodyRow, leader.Rows[i][6].ToString(), HorizontalAlign.Left);//K线图
            }
        }

        private void LinkButtonQuery_LinkButtonClicked(object sender, EventArgs e)
        {
            this.RecordQueryCondition();
            this.Refresh();
        }
    }
}