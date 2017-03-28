using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using AISRS.Common.Data;
using AISRS.BusinessFacade;
using AISRS.Common.Function;
using AISRS.Common.Exception;
using AISRS.Common.Framework;
using AISRS.Common.Entity;

namespace StockJob
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //string date = DateTime.Now.Year.ToString() + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()) + (DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString());
            //string date1 = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()) + "-" + (DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString());
            //AISTOCK_STOCK_LOG_DATA data = new AISTOCK_STOCK_LOG_DATA();
            //AISTOCK_STOCK_LOG_DATA.AISTOCK_STOCK_LOGRow row = data.AISTOCK_STOCK_LOG.NewAISTOCK_STOCK_LOGRow();
            //if (IsValidDay(date1))//判断是否休市日
            //{
            //    if (!HasImport(date))
            //    {
            //        ImportData(date);
            //        //暂时没有用处
            //        //CalculateAvgPriceData(date);
            //        RSIData(date, date1);
            //        Qushi(date);
            //        row.RUNNER = "[Robot]";
            //        row.RUNTIME = DateTime.Now;
            //        row.STATUS = "[Robot] finish data collection :)";
            //    }
            //    else
            //    {
            //        row.RUNNER = "[Robot]";
            //        row.RUNTIME = DateTime.Now;
            //        row.STATUS = "[Robot] data had been collected";
            //    }
            //}
            //else
            //{
            //    row.RUNNER = "[Robot]";
            //    row.RUNTIME = DateTime.Now;
            //    row.STATUS = "[Robot] today is holiday :-";
            //}
            //data.AISTOCK_STOCK_LOG.AddAISTOCK_STOCK_LOGRow(row);
            ////删除超过30天之前的数据
            //string dateFrom = DateTime.Now.Year.ToString() + "-01-01";
            //bool hasdelete = false;
            //new StockSystem().DeleteData(dateFrom, date1, out hasdelete);
            //if (hasdelete)
            //{
            //    row = data.AISTOCK_STOCK_LOG.NewAISTOCK_STOCK_LOGRow();
            //    row.RUNNER = "[Robot]";
            //    row.RUNTIME = DateTime.Now;
            //    row.STATUS = "[Robot] destroy data less than " + date1 + " :-";
            //    data.AISTOCK_STOCK_LOG.AddAISTOCK_STOCK_LOGRow(row);
            //}
            //new StockSystem().SaveStockLog(data);

            #region [获取股东人数和十大流通股数据--来自一点仓位]
            AISTOCK_STOCK_IMPORT_DATA baseData = new StockSystem().GetStockImport(true);
            AISTOCK_FIELD_DOMAIN_VALUE_DATA optionDate = new StockSystem().GetDropDownList("date");
            foreach (AISTOCK_STOCK_IMPORT_DATA.AISTOCK_STOCK_IMPORTRow row in baseData.AISTOCK_STOCK_IMPORT.Rows)
            {
                string code = row.STOCK_CODE.ToString();
                if (code.Equals("sz000488")) continue;
                CatchCangWeiData(code);
                foreach (AISTOCK_FIELD_DOMAIN_VALUE_DATA.AISTOCK_FIELD_DOMAIN_VALUERow option in optionDate.AISTOCK_FIELD_DOMAIN_VALUE.Rows)
                {
                    CatchCangWeiData(code, option.FIELD_DOMAIN_VALUE.ToString());
                }
                SaveJobFinished(code);
            }
            #endregion

            this.Dispose();
            this.Close();
        }

        #region [导入数据]
        public bool HasImport(string date) {
            return new StockSystem().HasImport(date);
        }

        public bool IsExistData(string date)
        {
            return new StockSystem().IsExistsData(date);
        }

        public bool IsValidDay(string date)
        {
            return new StockSystem().IsValidDate(date);
        }

        private void Qushi(string date)
        {
            AISTOCK_STOCK_STATS_DATA data = new StockSystem().GetStockData(date, "");
            int HL = 0;
            int LH = 0;
            foreach (var item in data.AISTOCK_STOCK_INFORMATION)
            {                
                if (item.TODAY_BEGIN > item.YESTERDAY_END && item.TODAY_END < item.YESTERDAY_END)
                {
                    HL++;
                }
                if (item.TODAY_BEGIN < item.YESTERDAY_END && item.TODAY_END > item.YESTERDAY_END)
                {
                    LH++;
                }
            } 
            decimal ratio = (decimal)LH / (decimal)(LH + HL) * 100;
            string content = "低开高走" + LH + "家，高开低走" + HL + "家，低开高走占比" + ratio.ToString("0.00") + "%";
            SendEmail("今日趋势", content);
        }

        private void SendEmail(string subject, string content)
        {
            string senderServerIp = "smtp.qq.com";
            string toMailAddress = "476707501@qq.com";
            string fromMailAddress = "476707501@qq.com";
            string subjectInfo = subject;
            string bodyInfo = content;
            string mailPort = "25";
            EmailUtil email = new EmailUtil(senderServerIp, toMailAddress, fromMailAddress, subjectInfo, bodyInfo, mailPort, false, false);
            email.Send();
        }

        private void DataUpdate()
        {
            AISTOCK_STOCK_IMPORT_DATA stocks = new StockSystem().GetStockImport();
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "text/html; charset=gb2312");
            Stream stockData;
            StreamReader reader;
            string returnData;
            int start;
            int end;
            string[] tradeData;
            foreach (AISTOCK_STOCK_IMPORT_DATA.AISTOCK_STOCK_IMPORTRow row in stocks.AISTOCK_STOCK_IMPORT.Rows)
            {
                stockData = client.OpenRead("http://hq.sinajs.cn/list=" + row.STOCK_CODE.ToString());
                reader = new StreamReader(stockData, System.Text.Encoding.GetEncoding("gb2312"));
                returnData = reader.ReadToEnd();
                start = returnData.IndexOf('"');
                end = returnData.LastIndexOf('"');
                returnData = returnData.Substring(start + 1, end - start - 1);

                tradeData = returnData.Split(',');
                if (tradeData.Length == 33)
                {
                    string stockName = tradeData[0].ToString();
                    UpdateData(row.STOCK_CODE.ToString(), stockName);
                }
            }
        }

        private void UpdateData(string code, string name)
        {
            new StockSystem().UpdateData(code, name);
        }

        private void ImportData(string date)
        {            
            AISTOCK_STOCK_STATS_DATA data = new AISTOCK_STOCK_STATS_DATA();
            AISTOCK_STOCK_IMPORT_DATA stocks = new StockSystem().GetStockImport();
            string stockCode = string.Empty;
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "text/html; charset=gb2312");
            Stream stockData;
            StreamReader reader;
            string returnData;
            int start;
            int end;
            string[] tradeData;
            string stockName = string.Empty;
            decimal todayBegin;
            decimal yesterdayEnd;
            decimal current;
            decimal max;
            decimal min;
            decimal buy;
            decimal sell;
            int quantity;
            decimal money;
            decimal[] buyer_price;
            int[] buyer_quantity;
            decimal[] seller_price;
            int[] seller_quanlity;
            foreach (var item in stocks.AISTOCK_STOCK_IMPORT)
            {
                AISTOCK_STOCK_STATS_DATA.AISTOCK_STOCK_INFORMATIONRow row = data.AISTOCK_STOCK_INFORMATION.NewAISTOCK_STOCK_INFORMATIONRow();
                stockCode = item.STOCK_CODE.ToString();
                Guid stock_id = Guid.NewGuid();
                buyer_price = new decimal[5];
                buyer_quantity = new int[5];
                seller_price = new decimal[5];
                seller_quanlity = new int[5];
                stockData = client.OpenRead("http://hq.sinajs.cn/list=" + stockCode);
                reader = new StreamReader(stockData, System.Text.Encoding.GetEncoding("gb2312"));
                returnData = reader.ReadToEnd();
                start = returnData.IndexOf('"');
                end = returnData.LastIndexOf('"');
                returnData = returnData.Substring(start + 1, end - start - 1);

                tradeData = returnData.Split(',');
                if (tradeData.Length == 33)
                {
                    stockName = tradeData[0].ToString();
                    todayBegin = decimal.Parse(tradeData[1].ToString());
                    yesterdayEnd = decimal.Parse(tradeData[2].ToString());
                    current = decimal.Parse(tradeData[3].ToString());
                    max = decimal.Parse(tradeData[4].ToString());
                    min = decimal.Parse(tradeData[5].ToString());
                    buy = decimal.Parse(tradeData[6].ToString());
                    sell = decimal.Parse(tradeData[7].ToString());
                    quantity = int.Parse(tradeData[8].ToString());
                    money = decimal.Parse(tradeData[9].ToString());
                    buyer_quantity[0] = int.Parse(tradeData[10].ToString());
                    buyer_price[0] = decimal.Parse(tradeData[11].ToString());
                    buyer_quantity[1] = int.Parse(tradeData[12].ToString());
                    buyer_price[1] = decimal.Parse(tradeData[13].ToString());
                    buyer_quantity[2] = int.Parse(tradeData[14].ToString());
                    buyer_price[2] = decimal.Parse(tradeData[15].ToString());
                    buyer_quantity[3] = int.Parse(tradeData[16].ToString());
                    buyer_price[3] = decimal.Parse(tradeData[17].ToString());
                    buyer_quantity[4] = int.Parse(tradeData[18].ToString());
                    buyer_price[4] = decimal.Parse(tradeData[19].ToString());
                    seller_quanlity[0] = int.Parse(tradeData[20].ToString());
                    seller_price[0] = decimal.Parse(tradeData[21].ToString());
                    seller_quanlity[1] = int.Parse(tradeData[22].ToString());
                    seller_price[1] = decimal.Parse(tradeData[23].ToString());
                    seller_quanlity[2] = int.Parse(tradeData[24].ToString());
                    seller_price[2] = decimal.Parse(tradeData[25].ToString());
                    seller_quanlity[3] = int.Parse(tradeData[26].ToString());
                    seller_price[3] = decimal.Parse(tradeData[27].ToString());
                    seller_quanlity[4] = int.Parse(tradeData[28].ToString());
                    seller_price[4] = decimal.Parse(tradeData[29].ToString());
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
                    for (int i = 0; i < 5; i++)
                    {
                        buyer_quantity[i] = 0;
                        buyer_price[i] = 0;
                        seller_quanlity[i] = 0;
                        seller_price[i] = 0;
                    }
                }

                reader.Close();
                stockData.Close();

                decimal percent;
                string chart;
                if (yesterdayEnd > 0)
                {
                    percent = (current - yesterdayEnd) / yesterdayEnd * 100;
                }
                else
                {
                    percent = 0;
                }
                chart = this.GetChart(todayBegin, current, max, min, yesterdayEnd);

                row.STOCK_ID = stock_id.ToString().ToUpper();
                row.STOCK_CODE = stockCode;
                row.STOCK_NAME = stockName;
                row.STOCK_DAY = date;
                row.TODAY_BEGIN = todayBegin;
                row.YESTERDAY_END = yesterdayEnd;
                row.TODAY_END = current;
                row.MAX_PRICE = max;
                row.MIN_PRICE = min;
                row.BUY_PRICE = buy;
                row.SELL_PRICE = sell;
                row.QUANTITY = quantity;
                row.TOTAL_MONEY = money;
                row.BUY_ONE_QUANTITY = buyer_quantity[0];
                row.BUY_ONE_PRICE = buyer_price[0];
                row.BUY_TWO_QUANTITY = buyer_quantity[1];
                row.BUY_TWO_PRICE = buyer_price[1];
                row.BUY_THD_QUANTITY = buyer_quantity[2];
                row.BUY_THD_PRICE = buyer_price[2];
                row.BUY_FOU_QUANTITY = buyer_quantity[3];
                row.BUY_FOU_PRICE = buyer_price[3];
                row.BUY_FIF_QUANTITY = buyer_quantity[4];
                row.BUY_FIF_PRICE = buyer_price[4];
                row.SELL_ONE_QUANTITY = seller_quanlity[0];
                row.SELL_ONE_PRICE = seller_price[0];
                row.SELL_TWO_QUANTITY = seller_quanlity[1];
                row.SELL_TWO_PRICE = seller_price[1];
                row.SELL_THD_QUANTITY = seller_quanlity[2];
                row.SELL_THD_PRICE = seller_price[2];
                row.SELL_FOU_QUANTITY = seller_quanlity[3];
                row.SELL_FOU_PRICE = seller_price[3];
                row.SELL_FIF_QUANTITY = seller_quanlity[4];
                row.SELL_FIF_PRICE = seller_price[4];
                row.INCREASE_PERCENT = percent;
                row.CHART = chart;

                data.AISTOCK_STOCK_INFORMATION.Rows.Add(row);
            }
            new StockSystem().ImportStockData(data);

        }

        private void CalculateAvgPriceData(string date)
        {
            AISTOCK_STOCK_AVG_PRICE_DATA data = new AISTOCK_STOCK_AVG_PRICE_DATA();
            AISTOCK_STOCK_IMPORT_DATA stocks = new StockSystem().GetStockImport();
            string stockCode = string.Empty;
            decimal fiveAvg = 0;
            decimal tenAvg = 0;
            //decimal twentyAvg = 0;
            decimal thirtyAvg = 0;
            //decimal sixtyAvg = 0;
            foreach (var item in stocks.AISTOCK_STOCK_IMPORT)
            {
                AISTOCK_STOCK_AVG_PRICE_DATA.AISTOCK_STOCK_AVG_PRICERow row = data.AISTOCK_STOCK_AVG_PRICE.NewAISTOCK_STOCK_AVG_PRICERow();
                Guid stock_id = Guid.NewGuid();
                stockCode = item.STOCK_CODE;
                fiveAvg = new StockSystem().GetAvgPrice(stockCode, date, 5);
                tenAvg = new StockSystem().GetAvgPrice(stockCode, date, 10);
                //twentyAvg = new StockSystem().GetAvgPrice(stockCode, date, 20);
                thirtyAvg = new StockSystem().GetAvgPrice(stockCode, date, 30);
                //sixtyAvg = new StockSystem().GetAvgPrice(stockCode, date, 60);

                row.STOCK_AVG_ID = stock_id.ToString();
                row.STOCK_CODE = stockCode;
                row.STOCK_NAME = item.STOCK_NAME;
                row.STOCK_DAY = date;
                row.FIVE_AVG = fiveAvg;
                row.TEN_AVG = tenAvg;
                //row.TWENTY_AVG = twentyAvg;
                row.THIRTY_AVG = thirtyAvg;
                //row.SIXTY_AVG = sixtyAvg;

                data.AISTOCK_STOCK_AVG_PRICE.Rows.Add(row);
            }
            new StockSystem().InsertAvgPrice(data);
        }

        private void RSIData(string dateTo, string date1)
        {
            int indexDay = 14;
            string date = new StockSystem().GetIndexDay(date1, indexDay);
            date = DateTimeFunction.ConvertDate(DateTime.Parse(date).ToShortDateString());
            AISTOCK_STOCK_INDEX_V_DATA dataRSI = new AISTOCK_STOCK_INDEX_V_DATA();
            dataRSI = new StockSystem().GetStockDataIndex(date, dateTo, "");
            AISTOCK_STOCK_STATS_DATA stockData = new StockSystem().GetStockData(dateTo, "");
            AISTOCK_STOCK_WMS_V_DATA dataWms = new AISTOCK_STOCK_WMS_V_DATA();
            dataWms = new StockSystem().GetStockWmsData(date, dateTo, "");
            if (dataRSI == null)
            {
                return;
            }
            if (dataRSI.AISTOCK_STOCK_INDEX_V.Count <= 0)
            {
                return;
            }
            int count = 0;
            StringBuilder sb = new StringBuilder();
            AISTOCK_STOCK_COMMAND_DATA saveData = new AISTOCK_STOCK_COMMAND_DATA();
            for (int i = 0; i < stockData.AISTOCK_STOCK_INFORMATION.Rows.Count; i++)
            {
                decimal sumA = 0;
                decimal sumB = 0;
                decimal index = 0;
                AISTOCK_STOCK_STATS_DATA.AISTOCK_STOCK_INFORMATIONRow stockRow = ((AISTOCK_STOCK_STATS_DATA.AISTOCK_STOCK_INFORMATIONRow)stockData.AISTOCK_STOCK_INFORMATION.Rows[i]);
                string stockCode = stockRow.STOCK_CODE.ToString();
                DataRow[] rows = dataRSI.AISTOCK_STOCK_INDEX_V.Select("STOCK_CODE = '" + stockCode + "'");
                if (!stockCode.ToLower().StartsWith("sz002")) continue;
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
                if (index == 0 || index > 10) continue;
                
                //WM Index
                DataRow[] wmrow = dataWms.AISTOCK_STOCK_WMS_V.Select("STOCK_CODE = '" + stockCode + "'");
                decimal max_price = 0;
                decimal min_price = 0;
                decimal end_price = 0;
                decimal wmindex = 0;
                max_price = ((AISTOCK_STOCK_WMS_V_DATA.AISTOCK_STOCK_WMS_VRow)wmrow[0]).MAX_PRICE;
                min_price = ((AISTOCK_STOCK_WMS_V_DATA.AISTOCK_STOCK_WMS_VRow)wmrow[0]).MIN_PRICE;
                end_price = stockRow.TODAY_END;
                if (max_price - min_price != 0)
                {
                    wmindex = (max_price - end_price) / (max_price - min_price) * 100;
                }

                if (wmindex > 0) continue;

                int quantity = new StockSystem().GetQuantity(stockCode);
                decimal ratio;
                if (quantity == 0) ratio = -1;
                else
                    ratio = (decimal)stockRow.QUANTITY / (decimal)quantity;
               
                count++;
                AISTOCK_STOCK_COMMAND_DATA.AISTOCK_STOCK_COMMANDRow saveRow = saveData.AISTOCK_STOCK_COMMAND.NewAISTOCK_STOCK_COMMANDRow();
                saveRow.STOCK_ID = Guid.NewGuid().ToString();
                saveRow.STOCK_CODE = stockCode;
                saveRow.STOCK_NAME = stockRow.STOCK_NAME;
                saveRow.STOCK_DAY = stockRow.STOCK_DAY;
                saveRow.TODAY_BEGIN = stockRow.TODAY_BEGIN;
                saveRow.TODAY_END = stockRow.TODAY_END;
                saveRow.MAX_PRICE = stockRow.MAX_PRICE;
                saveRow.MIN_PRICE = stockRow.MIN_PRICE;
                saveRow.RSI = index;
                saveRow.WM = wmindex;
                saveRow.RATIO = ratio;
                saveData.AISTOCK_STOCK_COMMAND.AddAISTOCK_STOCK_COMMANDRow(saveRow);
                sb.Append("@" + stockCode + "@" + index.ToString("0.00") + "\n");                
            }
            if (count > 0)
            {
                new StockSystem().SaveRSIData(saveData);
                SendEmail("敌情资讯", sb.ToString());
            }
        }

        private string GetChart(decimal begin, decimal end, decimal max, decimal min, decimal yesterdayEnd)
        {
            string chart = string.Empty;
            decimal diff = begin - end;
            if (diff < 0)
            {
                chart = "阳线";
                if (begin == min && end == max)
                {
                    chart = "光头光脚大阳线";
                }
                else if (begin == min && end < max)
                {
                    chart = "光脚阳线";
                }
                else if (begin > min && end == max)
                {
                    chart = "光头阳线";
                }

            }
            else if (diff == 0)
            {
                chart = "十字星";
                if (begin == max && max == min)
                {
                    chart = "一字形";
                }
                else if (begin == max && max > min)
                {
                    chart = "T形";
                }
                else if (begin == min && max > min)
                {
                    chart = "塔形";
                }
            }
            else
            {
                chart = "阴线";
                if (begin == max && end == min)
                {
                    chart = "光头光脚大阴线";
                }
                else if (begin < max && end == min)
                {
                    chart = "光脚阴线";
                }
                else if (begin == max && end > min)
                {
                    chart = "光头阴线";
                }
            }
            if (begin - yesterdayEnd > 0)
            {
                if (min >= yesterdayEnd)
                {
                    chart += "-高开高走";
                }
                else
                {
                    if (end < yesterdayEnd)
                    {
                        chart += "-高开低走";
                    }
                    else
                    {
                        chart += "-震荡上行";
                    }
                }
            }
            else if (begin - yesterdayEnd == 0)
            {
                if (end > begin)
                {
                    if (min == begin)
                    {
                        chart += "-平开高走";
                    }
                    else
                    {
                        chart += "-震荡上行";
                    }
                }
                else
                {
                    if (max == begin)
                    {
                        chart += "-平开低走";
                    }
                    else
                    {
                        chart += "-震荡下行";
                    }
                }
            }
            else
            {
                if (end > yesterdayEnd)
                {
                    chart += "-低开高走";
                }
                else
                {
                    if (end > begin)
                    {
                        chart += "-震荡上行";
                    }
                    else
                    {
                        chart += "-低开低走";
                    }
                }
            }
            return chart;
        }
        #endregion

        //股东人数
        private void CatchCangWeiData(string code)
        {
            string url = string.Empty;
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            Byte[] pageData;
            string pageHtml;             
            url = "http://www.yidiancangwei.com/gudong/renshu_code.html";
            url = url.Replace("code", code.Substring(2));
            pageData = MyWebClient.DownloadData(url); //从指定网站下载数据
            pageHtml = Encoding.UTF8.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句

            int index = pageHtml.IndexOf("tableCls tablefirst");
            int index2 = 0;
            if (index > 0)
            {
                pageHtml = pageHtml.Substring(index);
                index2 = pageHtml.IndexOf("</table>");
                if (index2 > 0)
                {
                    pageHtml = pageHtml.Substring(0, index2);
                    GuDongRenShu(pageHtml, code);                    
                }
            }
            MyWebClient.Dispose();
        }

        //股东人数
        private void GuDongRenShu(string pageHtml, string code)
        {
            pageHtml = pageHtml.Replace('\n', ' ').Replace('\t', ' ').Replace('\r', ' ').Replace(" ", "");
            AISTOCK_STOCK_GUDONGRENSHU_DATA data = new AISTOCK_STOCK_GUDONGRENSHU_DATA();
            int index = 0;
            index = pageHtml.IndexOf("<tbody");
            if (index < 0) return;
            pageHtml = pageHtml.Substring(index);
            if (pageHtml.IndexOf("<tr>") > 0 && pageHtml.LastIndexOf("</tr>") > 0)
            {
                pageHtml = pageHtml.Substring(pageHtml.IndexOf("<tr>") + 4);
                pageHtml = pageHtml.Substring(0, pageHtml.LastIndexOf("</tr>"));
                pageHtml = pageHtml.Replace("</td><td>", "|").Replace("</td><tdclass=\"red\">", "|").Replace("</td><tdclass=\"green\">", "|").Replace("</td></tr><tr><td>", "#");
                pageHtml = pageHtml.Replace("<td>", "").Replace("</td>", "");
                string[] rowArr = pageHtml.Split('#');
                int orderNum = 1;
                for (int i = 0; i < rowArr.Length; i++)
                {
                    AISTOCK_STOCK_GUDONGRENSHU_DATA.AISTOCK_STOCK_GUDONGRENSHURow row = data.AISTOCK_STOCK_GUDONGRENSHU.NewAISTOCK_STOCK_GUDONGRENSHURow();
                    string[] columnArr = rowArr[i].Split('|');
                    row.DateIndex = columnArr[0];
                    if (IsExistGuDongRenShu(code, row.DateIndex))
                    {
                        continue;
                    }
                    row.STOCK_CODE = code;
                    row.GuDongQuantity = (columnArr[1].Equals("-")) ? 0 : ChangeDataToD(columnArr[1]);
                    row.Change = (columnArr[2].Equals("-")) ? 0 : decimal.Parse(columnArr[2].Replace("+","").Replace("%",""));
                    row.AvgQuantity = (columnArr[3].Equals("-")) ? 0 : ChangeDataToDecimal(columnArr[3]);
                    row.AvgChange = (columnArr[4].Equals("-")) ? 0 : decimal.Parse(columnArr[4].Replace("+", "").Replace("%", ""));
                    row.AvgIndustry = (columnArr[5].Equals("-")) ? 0 : decimal.Parse(columnArr[5]);
                    row.OrderNum = orderNum++;
                    data.AISTOCK_STOCK_GUDONGRENSHU.AddAISTOCK_STOCK_GUDONGRENSHURow(row);
                }
                new StockSystem().SaveGuDongRenShu(data);
            }            
        }

        //十大流通股
        private void CatchCangWeiData(string code, string dateString)
        {
            if (!new StockSystem().ClearShiDaLiuTongGuData(code, dateString))
            {
                return;
            }
            string url = string.Empty;
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据            
            Byte[] pageData;
            string pageHtml;
            url = "http://www.yidiancangwei.com/gudong/sdlt_code_date.html";
            url = url.Replace("code", code.Substring(2)).Replace("date", dateString);
            pageData = MyWebClient.DownloadData(url); //从指定网站下载数据
            pageHtml = Encoding.UTF8.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句
            int index = pageHtml.IndexOf("tableCls tablefirst");
            int index2 = 0;
            if (index > 0)
            {
                pageHtml = pageHtml.Substring(index);
                index2 = pageHtml.IndexOf("</table>");
                if (index2 > 0)
                {
                    pageHtml = pageHtml.Substring(0, index2);
                    ShiDaLiuTongGu(pageHtml, code);
                }
            }
            MyWebClient.Dispose();
        }

        //十大流通股
        private void ShiDaLiuTongGu(string pageHtml, string code)
        {
            pageHtml = pageHtml.Replace('\n', ' ').Replace('\t', ' ').Replace('\r', ' ').Replace(" ", "");
            AISTOCK_STOCK_ShiDaLiuTongGu_DATA data = new AISTOCK_STOCK_ShiDaLiuTongGu_DATA();
            int index = 0;
            index = pageHtml.IndexOf("<tdclass");
            if (index < 0) return;
            pageHtml = pageHtml.Substring(index);
            if (pageHtml.LastIndexOf("</tr>") > 0)
            {
                pageHtml = pageHtml.Substring(0, pageHtml.LastIndexOf("</tr>"));
                pageHtml = pageHtml.Replace("</td><td>", "|").Replace("</td><tdclass=\"red\">", "|").Replace("</td><tdclass=\"green\">", "|").Replace("</td><tdclass=\"''\">", "|").Replace("</td></tr><tr><tdclass=\"ListNumtL\">", "#");
                pageHtml = pageHtml.Replace("<tdclass=\"ListNumtL\">", "").Replace("</td>", "");
                string[] rowArr = pageHtml.Split('#');
                int orderNum = 1;
                for (int i = 0; i < rowArr.Length; i++)
                {
                    AISTOCK_STOCK_ShiDaLiuTongGu_DATA.AISTOCK_STOCK_ShiDaLiuTongGuRow row = data.AISTOCK_STOCK_ShiDaLiuTongGu.NewAISTOCK_STOCK_ShiDaLiuTongGuRow();
                    string[] columnArr = rowArr[i].Split('|');
                    row.STOCK_CODE = code;
                    row.GuDongName = GuDongName(columnArr[0]);
                    row.GuDongPercent = (columnArr[1].Equals("-")) ? 0 : decimal.Parse(columnArr[1]);
                    row.StockQuantity = (columnArr[2].Equals("-")) ? 0 : decimal.Parse(columnArr[2]);
                    row.CangWeiChange = columnArr[3];
                    row.StockType = columnArr[4].Replace("<br>","|");
                    row.UpdateDate = columnArr[5];
                    row.OrderNum = orderNum++;
                    data.AISTOCK_STOCK_ShiDaLiuTongGu.AddAISTOCK_STOCK_ShiDaLiuTongGuRow(row);
                }
                new StockSystem().SaveShiDaLiuTongGu(data);
            }            
        }

        private void SaveJobFinished(string code)
        {
            new StockSystem().IsJobFinished(code);
        }

        private bool IsExistGuDongRenShu(string code, string dateIndex)
        {
            return new StockSystem().IsExistGuDongRenShu(code, dateIndex);
        }

        private string GuDongName(string name)
        {
            int index = name.IndexOf(">");
            if (index > 0)
            {
                name = name.Substring(index + 1);
                index = name.IndexOf("<");
                if (index > 0)
                {
                    name = name.Substring(0, index);
                }
            }
            return name;
        }

        private int ChangeDataToD(string strData)
        {
            int dData = 0;
            if (strData.ToUpper().Contains("E+"))
            {
                int index = strData.ToUpper().IndexOf("E+");
                double xiaoshu = double.Parse(strData.Substring(0, index));
                int multi = int.Parse(strData.Substring(index + 2));
                switch (multi)
                {
                    case 5: dData = int.Parse((xiaoshu * 100000).ToString()); break;
                    case 6: dData = int.Parse((xiaoshu * 1000000).ToString()); break;
                    case 7: dData = int.Parse((xiaoshu * 10000000).ToString()); break;
                    case 8: dData = int.Parse((xiaoshu * 100000000).ToString()); break;
                    case 9: dData = int.Parse((xiaoshu * 100000000).ToString()); break;
                    default: break;
                }
            }
            else
            {
                if (int.TryParse(strData, out dData))
                {
                    return dData;
                }
            }
            return dData;
        }

        private decimal ChangeDataToDecimal(string strData)
        {
            decimal dData = 0;
            if (strData.ToUpper().Contains("E+"))
            {
                int index = strData.ToUpper().IndexOf("E+");
                double xiaoshu = double.Parse(strData.Substring(0, index));
                int multi = int.Parse(strData.Substring(index + 2));
                switch (multi)
                {
                    case 5: dData = decimal.Parse((xiaoshu * 100000).ToString()); break;
                    case 6: dData = decimal.Parse((xiaoshu * 1000000).ToString()); break;
                    case 7: dData = decimal.Parse((xiaoshu * 10000000).ToString()); break;
                    case 8: dData = decimal.Parse((xiaoshu * 100000000).ToString()); break;
                    case 9: dData = decimal.Parse((xiaoshu * 100000000).ToString()); break;
                    default: break;
                }
            }
            else
            {
                dData = decimal.Parse(strData);
            }
            return dData;
        }        
    }
}
