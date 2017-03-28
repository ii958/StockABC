using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AISRS.Common.Function;
using AISRS.BusinessFacade;
using AISRS.Common.Data;

namespace BigBrother
{
    public partial class BigService : ServiceBase
    {
        public BigService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteLog("Big Brother start.");            
            System.Timers.Timer t = new System.Timers.Timer();
            t.Interval = 1000 * 60; //every 10 minutes
            t.Elapsed += new System.Timers.ElapsedEventHandler(ChkSrv);
            t.AutoReset = true;
            t.Enabled = true;
        }

        private void ChkSrv(object source, System.Timers.ElapsedEventArgs e)
        {
            int intHour = e.SignalTime.Hour;
            int intMinute = e.SignalTime.Minute;
            int intSecond = e.SignalTime.Second;
            WriteLog("老大在办事");
            string week = DateTime.Now.ToString("ddd");
            WriteLog(week);
            if ("周六".Equals(week) || "周日".Equals(week))
            {
                return;
            }
            //send words
            if (intHour >= 10 && intHour <= 11 && intMinute == 0)
            { 
                StockWords();
            }
            if (intHour >= 13 && intHour <= 15 && intMinute == 0)
            {
                StockWords();
            }
            //扫盘
            bool scan = false;
            int yushu = intMinute % 30;
            if (yushu == 0) {
                if (intHour == 11)
                {
                    if (intMinute <= 30) scan = true;
                }
                else if (intHour == 10 || intHour == 13 || intHour == 14)
                {
                    scan = true;
                }
            }
            if (scan)
            {
                TimelyAnalyse();
                ScannerBigPan();                        
            }
        }

        private void TimelyAnalyse()
        {
            string file = @"D:\workspace\StockABC\Timely.txt";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(@file))
            {
                string code;
                Stream stockData;
                StreamReader reader;
                string returnData;
                string[] tradeData;
                int start;
                int end;
                decimal todayBegin;
                decimal yesterdayEnd;
                decimal current;
                decimal max;
                decimal min;
                decimal diff;
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "text/html; charset=gb2312");
                StringBuilder sb = new StringBuilder();
                while ((code = sr.ReadLine()) != null)
                {
                    stockData = client.OpenRead("http://hq.sinajs.cn/list=" + code);
                    reader = new StreamReader(stockData, System.Text.Encoding.GetEncoding("gb2312"));
                    returnData = reader.ReadToEnd();
                    start = returnData.IndexOf('"');
                    end = returnData.LastIndexOf('"');
                    returnData = returnData.Substring(start + 1, end - start - 1);
                    tradeData = returnData.Split(',');
                    if (tradeData.Length == 33)
                    {
                        todayBegin = decimal.Parse(tradeData[1].ToString());
                        if (todayBegin == 0) continue;
                        yesterdayEnd = decimal.Parse(tradeData[2].ToString());
                        current = decimal.Parse(tradeData[3].ToString());
                        max = decimal.Parse(tradeData[4].ToString());
                        min = decimal.Parse(tradeData[5].ToString());
                        diff = current - yesterdayEnd;
                        if (diff > 0)
                        {
                            sb.Append(code + "的当前价￥" + current.ToString("0.00") + ",涨了￥" + diff.ToString("0.00") + "\n");
                        }
                        else
                        {
                            sb.Append(code + "的当前价￥" + current.ToString("0.00") + ",跌了￥" + diff.ToString("0.00") + "\n");
                        }
                    }
                }
                SendEmail("实时消息", sb.ToString());
            }
        }

        private void ScannerBigPan()
        {
            string today = new StockSystem().GetIndexDay(DateTime.Now.ToString("yyyy-MM-dd"), 1);
            today = DateTimeFunction.ConvertDate(DateTime.Parse(today).ToString("yyyy-MM-dd"));
            WriteLog("today:" + today);
            AISTOCK_STOCK_COMMAND_DATA data = new StockSystem().GetRSIData(today);
            WriteLog("total today:" + data.AISTOCK_STOCK_COMMAND.Count);   
            string stockCode;
            Stream stockData;
            StreamReader reader;
            string returnData;
            string[] tradeData;
            int start;
            int end;
            decimal todayBegin;
            decimal yesterdayEnd;
            decimal current;
            decimal max;
            decimal min;
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "text/html; charset=gb2312");
            StringBuilder sb = new StringBuilder();
            foreach (var item in data.AISTOCK_STOCK_COMMAND)
            {
                stockCode = item.STOCK_CODE;
                stockData = client.OpenRead("http://hq.sinajs.cn/list=" + stockCode);
                reader = new StreamReader(stockData, System.Text.Encoding.GetEncoding("gb2312"));
                returnData = reader.ReadToEnd();
                start = returnData.IndexOf('"');
                end = returnData.LastIndexOf('"');
                returnData = returnData.Substring(start + 1, end - start - 1);
                tradeData = returnData.Split(',');
                if (tradeData.Length == 33)
                {                    
                    todayBegin = decimal.Parse(tradeData[1].ToString());
                    if (todayBegin == 0) continue;
                    yesterdayEnd = decimal.Parse(tradeData[2].ToString());
                    current = decimal.Parse(tradeData[3].ToString());
                    max = decimal.Parse(tradeData[4].ToString());
                    min = decimal.Parse(tradeData[5].ToString());                    
                    sb.Append("@" + stockCode + "@");
                    if (todayBegin < yesterdayEnd)
                    { 
                        //低开
                        if (current < todayBegin)
                            sb.Append("低开低走");
                        else if (current > yesterdayEnd)
                            sb.Append("低开高走");
                        else if (current > todayBegin)
                            sb.Append("有上升迹象，可操作");
                    }                    
                    else
                    {
                        if (current > todayBegin)
                            sb.Append("高开高走");
                        else if (current < yesterdayEnd)
                            sb.Append("高开低走，观望等下午看看");
                    }
                    sb.Append("\n");
                }
            }
            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                SendEmail("听老大的话", sb.ToString());
            }            
        }

        protected override void OnStop()
        {
            WriteLog("Big Brother stop.");
        }

        private void WriteLog(string message)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("D:\\workspace\\StockABC\\BigBrotherLog.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + message);
            }
        }

        private void StockWords()
        {
            int total = new StockSystem().GetTotalWords();
            Random ran = new Random();
            int id = ran.Next(1, total + 1);
            string words = new StockSystem().GetWords(id);
            SendEmail("战术上的重视表现在对敌的主动权，可以试探，可以深入，可以围而不公，可以坚守以待，每次只做一只，而不致于被敌人牵着鼻子走，若无必胜把握，可撤；凡战，则必胜，可大胜，可小胜，故谓之'常胜'", words);
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
    }
}
