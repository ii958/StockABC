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

namespace AISRS.WebUI.DailyMaintain
{
    public partial class DailyReportBrowse : PageBaseNoPermission
    {
        protected string _urlRoot = string.Empty;
        private string _keyID; //保存数据的主键ID
        private void Page_Load(object sender, EventArgs e)
        {
            this._urlRoot = AISRS.Common.Framework.Configuration.UrlRoot;
            

            if (Request["keyID"] != null && Request["keyID"].Trim().Length > 0)
            {
                this._keyID = Request["keyID"].ToString().Trim();
            }
            else
            {
                this._keyID = string.Empty;
            }
            if (Request["valuedata"] != null && Request["valuedata"].Trim().Length > 0)
            {
                this.hiddenSelectedValue.Value = Request["valuedata"].ToString().Trim();
            }
            else
            {
                this.hiddenSelectedValue.Value = string.Empty;
            }
            if (Request["oper"] != null)
            {
                if (Request["oper"].IndexOf("SaveDailyReportData") >= 0)
                {
                    //修改数据
                    SaveDailyReportData(this._keyID);
                }
                else if (Request["oper"].IndexOf("LoadDailyReportData") >= 0)
                {
                    //根据主键ID取得项目百分比数据
                    LoadDailyReportData(this._keyID);
                }
            }

            Refresh();
        }

        private void Refresh()
        {
            AISTOCK_CALENDAR_DATA data = new AISTOCK_CALENDAR_DATA();
            data = new StockSystem().GetAllCalendarData();

            this.BmBlafTable.Clear();
            this.DrawTableHeader();

            this.DrawTableBody(data);
        }

        private void SaveDailyReportData(string keyID)
        {
            string result = string.Empty;
            string message = string.Empty;
            if (keyID.Trim() != string.Empty)
            {
                try
                {
                    if (this.hiddenSelectedValue.Value.Trim() == string.Empty)
                    {
                        message = "没有要保存的数据！";
                    }
                    else
                    {
                        //根据主键ID取得数据
                        string remarks = this.hiddenSelectedValue.Value.Trim();

                        AISTOCK_CALENDAR_DATA data = new StockSystem().GetCalendarByID(keyID.Trim());                      

                        if (data.AISTOCK_CALENDAR.Count == 1)
                        {
                            #region 修改数据
                            AISTOCK_CALENDAR_DATA.AISTOCK_CALENDARRow row = data.AISTOCK_CALENDAR[0];                            
                            row.remark = remarks;                                
                            #endregion
                        }
                        else
                        {
                            message += "根据项目主键ID无法取得项目信息，请联系管理员！";
                        }
                        new StockSystem().SaveDailyReport(data);                        
                    }
                }
                catch (Exception ex)
                {
                    message += "系统出现未知异常，请联系管理员！" + ex.Message;
                }
            }
            else
            {
                message += "没有要保存的数据！";
            }
            if (message == string.Empty)
            {   //传入参数，用于回调数据，将修改后的数据更新显示于页面中
                result = "{IsSuccess:true, savedKeyID:'" + keyID.Trim() + "'}";
            }
            else
            {
                result = "{IsSuccess:false, Message:'" + message + "'}";
            }

            Response.Write(result);
            Response.End();

            //保存数据之后，将Hidden中的数据清空
            this.hiddenSelectedValue.Value = string.Empty; 
        }

        private void LoadDailyReportData(string keyID)
        {
            string remarks = string.Empty;
			string result = string.Empty;
			string message = string.Empty;

            if (keyID.Trim() != string.Empty)
            {
                try
                {
                    AISTOCK_CALENDAR_DATA data = new StockSystem().GetCalendarByID(keyID.Trim());
                    if (data.AISTOCK_CALENDAR.Rows.Count == 1)
                    {
                        remarks = ((AISTOCK_CALENDAR_DATA.AISTOCK_CALENDARRow)data.AISTOCK_CALENDAR.Rows[0]).remark.Trim();
                    }
                }
                catch (Exception ex)
                {
                    message = "系统出现未知异常，请联系管理员！" + ex.Message;
                }	
            }
            if (message == string.Empty)
            {   //传入参数，用于回调数据，取消修改，恢复原数据
                result = "{IsSuccess:true, keyID:'" + keyID.Trim() + "', projectData:'" + remarks.Trim() + "'}";
            }
            else
            {
                result = "{IsSuccess:false, Message:'" + message + "'}";
            }

            Response.Write(result);
            Response.End();
        }

        private void DrawTableHeader()
        {
            TableRow headerRow = this.BmBlafTable.AddHeadRow();

            headerRow.Height = 25;

            this.BmBlafTable.AddHeadCell(headerRow, "日期", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTable.AddHeadCell(headerRow, "星期", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTable.AddHeadCell(headerRow, "周数", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTable.AddHeadCell(headerRow, "季度", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTable.AddHeadCell(headerRow, "交易日数", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTable.AddHeadCell(headerRow, "交易日状态", 70).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTable.AddHeadCell(headerRow, "投资记录", 300).HorizontalAlign = HorizontalAlign.Left;

            this.BmBlafTable.AddHeadCell(headerRow, "操作", 100);

            this.BmBlafTable.Width = "1000";
        }

        private void DrawTableBody(AISTOCK_CALENDAR_DATA data)
        {
            if (data == null)
            {
                return;
            }
            if (data.AISTOCK_CALENDAR.Count <= 0)
            {
                return;
            }

            TableRow bodyRow;
            TableCell tableCell;
            foreach (AISTOCK_CALENDAR_DATA.AISTOCK_CALENDARRow row in data.AISTOCK_CALENDAR.Rows)
            {
                bodyRow = this.BmBlafTable.AddBodyRow();

                this.BmBlafTable.AddCell(bodyRow, row.the_date.ToShortDateString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.the_day.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.week_of_year.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.quarter.ToString(), HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.fiscal_period.Equals("1") ? "正常交易" : "休市", HorizontalAlign.Left);
                this.BmBlafTable.AddCell(bodyRow, row.Istrade_daysNull()? string.Empty : row.trade_days.ToString(), HorizontalAlign.Left);
                tableCell = this.BmBlafTable.AddCell(bodyRow, string.Empty, HorizontalAlign.Left, 300);                
                this.CreateTextBox(row.time_id.Trim(), "remark", row.IsremarkNull() ? string.Empty : row.remark.ToString(), tableCell, false, false);
                tableCell = this.BmBlafTable.AddCell(bodyRow, string.Empty, HorizontalAlign.Left);
                CreateEditButton(row.time_id.ToString().Trim(), "Edit", tableCell, false);
                CreateSaveButton(row.time_id.ToString().Trim(), "Save", tableCell);
                CreateCancelButton(row.time_id.ToString().Trim(), "Cancel", tableCell);
            }
        }

        #region 创建文本框
        void CreateTextBox(string keyID, string controlName, string textValue, TableCell cell, bool isRight, bool isNumberText)
        {
            TextBox textBox = new TextBox();
            textBox.ID = keyID + "_" + controlName + "_TextBox";
            textBox.Text = textValue;
            textBox.Style.Add("width", "100%");
            if (isNumberText) //判断如果是录入数字的文本框，最多17位数字
            {
                textBox.MaxLength = 17;
            }
            else //调整原因文本框可录入2000字
            {
                textBox.MaxLength = 500;
            }

            textBox.ReadOnly = true;
            textBox.Style.Add("border", "none");
            textBox.Style.Add("background-color", "#F7F7E7");
            if (isRight)
            {
                textBox.Style.Add("text-align", "right");
                cell.Style.Add("text-align", "right");
            }
            cell.Controls.Add(textBox);
        }
        #endregion

        //调整按钮控件
        void CreateEditButton(string keyID, string controlName, TableCell cell, bool isDisable)
        {
            System.Web.UI.HtmlControls.HtmlInputButton button = new HtmlInputButton();
            button.Value = "编辑";
            if (isDisable) //判断是否按钮不可用
            {
                button.Disabled = true;
            }

            button.ID = keyID + "_" + controlName;
            button.Style.Add("BORDER-RIGHT", "medium none");
            button.Style.Add("BORDER-TOP", "medium none");
            button.Style.Add("FONT-SIZE", "8pt");
            button.Style.Add("BACKGROUND-IMAGE", "url(../Images/btnShort.gif)");
            button.Style.Add("BORDER-LEFT", "medium none");
            button.Style.Add("WIDTH", "40px");
            button.Style.Add("CURSOR", "hand");
            button.Style.Add("BORDER-BOTTOM", "medium none");
            button.Style.Add("HEIGHT", "25px");
            button.Attributes["onclick"] = string.Format("return EditData('{0}');", keyID);
            cell.Controls.Add(button);
        }
        //保存按钮控件
        void CreateSaveButton(string keyID, string controlName, TableCell cell)
        {
            System.Web.UI.HtmlControls.HtmlInputButton button = new HtmlInputButton();
            button.Value = "保存";
            button.ID = keyID + "_" + controlName;
            button.Style.Add("BORDER-RIGHT", "medium none");
            button.Style.Add("BORDER-TOP", "medium none");
            button.Style.Add("FONT-SIZE", "8pt");
            button.Style.Add("BACKGROUND-IMAGE", "url(../Images/btnShort.gif)");
            button.Style.Add("BORDER-LEFT", "medium none");
            button.Style.Add("WIDTH", "40px");
            button.Style.Add("CURSOR", "hand");
            button.Style.Add("BORDER-BOTTOM", "medium none");
            button.Style.Add("HEIGHT", "25px");
            button.Attributes["onclick"] = string.Format("return SaveData('{0}');", keyID);
            button.Style.Add("display", "none");
            cell.Controls.Add(button);
        }
        //取消按钮控件
        void CreateCancelButton(string keyID, string controlName, TableCell cell)
        {
            System.Web.UI.HtmlControls.HtmlInputButton button = new HtmlInputButton();
            button.Value = "取消";
            button.ID = keyID + "_" + controlName;
            button.Style.Add("BORDER-RIGHT", "medium none");
            button.Style.Add("BORDER-TOP", "medium none");
            button.Style.Add("FONT-SIZE", "8pt");
            button.Style.Add("BACKGROUND-IMAGE", "url(../images/btnShort.gif)");
            button.Style.Add("BORDER-LEFT", "medium none");
            button.Style.Add("WIDTH", "40px");
            button.Style.Add("CURSOR", "hand");
            button.Style.Add("BORDER-BOTTOM", "medium none");
            button.Style.Add("HEIGHT", "25px");
            button.Attributes["onclick"] = string.Format("return CancelData('{0}');", keyID);
            button.Style.Add("display", "none");
            cell.Controls.Add(button);
        }
    }
}