using System;
using System.Data;
using System.Data.SqlClient;

using AISRS.Common.Data;
using AISRS.Common.Exception;
using AISRS.Common.Query;
using AISRS.Common.Function;

namespace AISRS.DataAccess
{
    public class DaStock : DataAccessSqlServerBase
    {
        public void LoadAllMarket(DataTable data)
        {
            string sql = "SELECT * FROM AISTOCK_FIELD_DOMAIN_VALUE WHERE FIELD_DOMAIN = '证券市场' AND IS_VALID = 1 ORDER BY ORDER_NO";
            this.AutoFill(data, sql);
        }

        public void LoadAllFieldMain(DataTable data)
        {
            string sql = "SELECT * FROM AISTOCK_FIELD_DOMAIN_VALUE WHERE FIELD_DOMAIN = '证监会行业' AND IS_VALID = 1 ORDER BY ORDER_NO";
            this.AutoFill(data, sql);
        }

        public void LoadAllField(DataTable data)
        {
            string sql = "SELECT * FROM AISTOCK_FIELD_DOMAIN_VALUE WHERE FIELD_DOMAIN = '标准行业' AND IS_VALID = 1 ORDER BY ORDER_NO";
            this.AutoFill(data, sql);
        }

        public void LoadAllProvince(DataTable data)
        {
            string sql = "SELECT * FROM AISTOCK_FIELD_DOMAIN_VALUE WHERE FIELD_DOMAIN = '所属省份' AND IS_VALID = 1 ORDER BY ORDER_NO";
            this.AutoFill(data, sql);
        }

        public void LoadAllChart(DataTable data)
        {
            string sql = "SELECT * FROM AISTOCK_FIELD_DOMAIN_VALUE WHERE FIELD_DOMAIN = 'K线图' AND IS_VALID = 1 ORDER BY ORDER_NO";
            this.AutoFill(data, sql);
        }

        public void LoadAllIndex(DataTable data)
        {
            string sql = "SELECT * FROM AISTOCK_FIELD_DOMAIN_VALUE WHERE FIELD_DOMAIN = '技术分析指标' AND IS_VALID = 1 ORDER BY ORDER_NO";
            this.AutoFill(data, sql);
        }

        public void LoadAllBuyPoint(DataTable data)
        {
            string sql = "SELECT * FROM AISTOCK_FIELD_DOMAIN_VALUE WHERE FIELD_DOMAIN = '买入信号' AND IS_VALID = 1 ORDER BY ORDER_NO";
            this.AutoFill(data, sql);
        }

        public void LoadAllDateString(DataTable data)
        {
            string sql = "SELECT * FROM AISTOCK_FIELD_DOMAIN_VALUE WHERE FIELD_DOMAIN = '日期字符串' AND IS_VALID = 1 ORDER BY ORDER_NO";
            this.AutoFill(data, sql);
        }

        public void LoadAllCalendar(DataTable data)
        {
            string sql = "SELECT * FROM AISTOCK_CALENDAR ORDER BY THE_DATE";
            this.AutoFill(data, sql);
        }

        public void LoadCalendarByID(DataTable data, string id)
        {
            string sql = "SELECT * FROM AISTOCK_CALENDAR WHERE TIME_ID = '" + id + "'";
            this.AutoFill(data, sql);
        }

        public void SaveDailyReport(DataTable data)
        {
            this.AutoUpdate(data, "AISTOCK_CALENDAR");
        }

        public void LoadStockImport(DataTable data)
        {
            string sql = "select * from AISTOCK_STOCK_IMPORT";
            this.AutoFill(data, sql);
        }

        public void LoadStockImport(DataTable data, bool option)
        {
            string sql = "select * from AISTOCK_STOCK_IMPORT";
            if (option)
            {
                sql = "select * from AISTOCK_STOCK_IMPORT where STOCK_CODE not in (select STOCK_CODE from AISTOCK_STOCK_JOBFINISHED where IsFinished = 1)";
            }
             
            this.AutoFill(data, sql);
        }

        public void IsJobFinished(string code)
        {
            string sql = "select count(*) from AISTOCK_STOCK_JOBFINISHED where STOCK_CODE = '"+code+ "' and IsFinished = '1'";
            if (int.Parse(this.GetScalar(sql).ToString()) == 0)
            {
                sql = "update AISTOCK_STOCK_JOBFINISHED set IsFinished = '1'  where STOCK_CODE = '" + code + "'";
                this.ExecuteNonQuerySql(sql);
            }
        }

        public void UpdateData(string code, string name)
        {
            string sql = "update AISTOCK_STOCK_IMPORT set STOCK_NAME = '"+ name + "' where STOCK_CODE = '"+ code + "'";
            this.ExecuteNonQuerySql(sql);
        }

        public bool IsExistGuDongRenShu(string code, string dateIndex)
        {
            string sql = "select COUNT(0) from AISTOCK_STOCK_GUDONGRENSHU where STOCK_CODE='"+ code + "' and DateIndex='"+ dateIndex + "'";
            if (int.Parse(this.GetScalar(sql).ToString()) == 0)
                return false;
            else
                return true;
        }

        public void ClearData(string code)
        {
            string sql = "delete from AISTOCK_STOCK_GUDONGRENSHU where STOCK_CODE='"+code+"';delete from AISTOCK_STOCK_ShiDaLiuTongGu where STOCK_CODE='"+code+"'";
            this.ExecuteNonQuerySql(sql);
        }

        public void LoadStockHistory(DataTable data, StockQueryCondition qc)
        {
            string stockCode = qc.StockCode;
            string stockName = qc.StockName;
            string market = qc.Market;
            string field = qc.Field;
            string chart = qc.Chart;
            string province = qc.Province;
            string dateFrom = DateTimeFunction.ConvertDate(qc.DatePickerFrom);
            string dateTo = DateTimeFunction.ConvertDate(qc.DatePickerTo);
            string sql = "SELECT * FROM AISTOCK_STOCK_DAILY_DATA_V WHERE STOCK_DAY >= '"+ dateFrom +"' AND STOCK_DAY <= '" + dateTo +"'";
            if (!string.IsNullOrEmpty(stockCode))
            {
                sql += " AND STOCK_CODE LIKE '%" + stockCode + "%'";
            }
            if (!string.IsNullOrEmpty(stockName))
            {
                sql += " AND STOCK_NAME LIKE '%" + stockName + "%'";
            }
            if (!string.IsNullOrEmpty(market))
            {
                sql += " AND STOCK_ADDR = '" + market + "'";
            }
            if (!string.IsNullOrEmpty(field))
            {
                sql += " AND FIELD = '" + field + "'";
            }
            if (!string.IsNullOrEmpty(province))
            {
                sql += " AND PROVINCE = '" + province + "'";
            }
            if (!string.IsNullOrEmpty(chart))
            {
                sql += " AND CHART = '" + chart + "'";
            }
            sql += " ORDER BY STOCK_CODE";
            this.AutoFill(data, sql);
        }

        public void LoadTodayData(DataTable data, string today)
        {
            string sql = "select STOCK_CODE,STOCK_DAY,TODAY_BEGIN,TODAY_END,INCREASE_PERCENT from AISTOCK_STOCK_INFORMATION where STOCK_DAY = '"+today+"'";
            this.AutoFill(data, sql);
        }

        /// <summary>
        /// 买入股票历史数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="qc"></param>
        public void LoadBuyHistoryData(DataTable data, StockQueryCondition qc)
        {
            string stockCode = qc.StockCode;
            string stockName = qc.StockName;
            string type = qc.BuyPoint;
            string dateFrom = DateTimeFunction.ConvertDate(qc.DatePickerFrom);
            string dateTo = DateTimeFunction.ConvertDate(qc.DatePickerTo);
            string dateSearch = DateTimeFunction.ConvertDate(qc.DatePickerSearch);

            string sql = "select ta.stock_code,ta.stock_name,ta.stock_type,ta.stock_day as search_day,tb.stock_day,"
                       + " tb.today_begin,tb.today_end,tb.max_price,tb.min_price,tb.increase_percent from"
                       + " (select * from aistock_stock_buy_stats b"
                       + " where b.stock_day = '"+dateSearch+"'";
            if (!string.IsNullOrEmpty(type))
            {
                sql += " and b.stock_type = '"+type+"'";
            }

            sql += " ) ta,"
                + " (select * from aistock_stock_information a"
                + " where a.stock_day >= '"+dateFrom+"' and a.stock_day <= '"+dateTo+"') tb"
                + " where ta.stock_code = tb.stock_code";
            if (!string.IsNullOrEmpty(stockCode))
            {
                sql += " and ta.stock_code = '"+stockCode+"'";
            }
            if (!string.IsNullOrEmpty(stockName))
            {
                sql += " and ta.stock_name = '"+stockName+"'";
            }
            sql += " order by ta.stock_code, ta.stock_type, tb.stock_day desc";
            this.AutoFill(data, sql);
        }

        public int IsExistsData(string date)
        {
            string sql = "SELECT COUNT(*) FROM AISTOCK_STOCK_AVG_PRICE WHERE STOCK_DAY = '" + date + "'";
            return int.Parse(this.GetScalar(sql).ToString());
        }

        public int HasImport(string date)
        {
            string sql = "select count(0) from [dbo].[AISTOCK_STOCK_INFORMATION] where STOCK_DAY = '" + date + "'";
            return int.Parse(this.GetScalar(sql).ToString());
        }

        public void DeleteAvgData(string date)
        {
            string sql = "DELETE FROM AISTOCK_STOCK_AVG_PRICE WHERE STOCK_DAY = '" + date + "'";
            this.ExecuteNonQuerySql(sql);
        }

        public void LoadStockAvgHistory(DataTable data, StockQueryCondition qc)
        {
            string stockCode = qc.StockCode;
            string stockName = qc.StockName;
            string dateFrom = DateTimeFunction.ConvertDate(qc.DatePickerFrom);
            string dateTo = DateTimeFunction.ConvertDate(qc.DatePickerTo);
            string sql = "SELECT * FROM AISTOCK_STOCK_AVG_PRICE WHERE STOCK_DAY <= '" + dateTo + "' AND STOCK_DAY >= '"+dateFrom+"'";
            if (!string.IsNullOrEmpty(stockCode))
            {
                sql += " AND STOCK_CODE LIKE '%" + stockCode + "%'";
            }
            if (!string.IsNullOrEmpty(stockName))
            {
                sql += " AND STOCK_NAME LIKE '%" + stockName + "%'";
            }
            sql += " ORDER BY STOCK_CODE";
            this.AutoFill(data, sql);
        }

        public void LoadStockBaseInfoWithoutCondition(DataTable data)
        {
            string sql = "SELECT * FROM AISTOCK_STOCK_BASEINFO WHERE IS_VALID = 1";
            sql += " ORDER BY PROFIT_PERCENT DESC, STOCK_CODE";
            this.AutoFill(data, sql);
        }

        /// <summary>
        /// 低档五连阳
        /// </summary>
        /// <param name="data"></param>
        /// <param name="date"></param>
        /// <param name="dateTo"></param>
        public void LoadLowFiveData(DataTable data, string date, string dateTo)
        {
            string sql = "select a.stock_code, min(a.increase_percent) as min_percent from aistock_stock_information a"
                       + " where a.stock_day >= '"+date+"' and a.stock_day <= '"+dateTo+"'"
                       + " group by a.stock_code"
                       + " having(min(a.increase_percent) > 0)"
                       + " order by min(a.increase_percent) desc";
            this.AutoFill(data, sql);
        }

        /// <summary>
        /// 双针探底
        /// </summary>
        /// <param name="data"></param>
        /// <param name="date"></param>
        /// <param name="dateTo"></param>
        public void LoadTwoPinData(DataTable data, string date, string dateTo)
        {
            string sql = "select ta.stock_code,"
                       + " ta.stock_name,"
                       + " ta.stock_day as yesterday,"
                       + " ta.today_begin as yesterday_begin,"
                       + " ta.today_end as yesterday_end,"
                       + " ta.min_price as yesterday_min_price,"
                       + " tb.stock_day as today,"
                       + " tb.today_begin as today_begin,"
                       + " tb.today_end as today_end,"
                       + " tb.min_price as today_min_price"
                       + " from ("
                       + " select * from aistock_stock_information a"
                       + " where a.stock_day = '" + date + "'"
                       + " and ((a.today_begin > a.today_end and abs(a.today_end - a.min_price) / abs(a.today_begin - a.today_end) > 2) "
                       + " or (a.today_begin = a.today_end and a.today_begin > a.min_price)"
                       + " or (a.today_begin < a.today_end and abs(a.today_begin - a.min_price) / abs(a.today_end - a.today_begin) > 2))"
                       + " ) ta, ("
                       + " select * from aistock_stock_information b"
                       + " where b.stock_day = '" + dateTo + "'"
                       + " and ((b.today_begin > b.today_end and abs(b.today_end - b.min_price) / abs(b.today_begin - b.today_end) > 2) "
                       + " or (b.today_begin = b.today_end and b.today_begin > b.min_price)"
                       + " or (b.today_begin < b.today_end and abs(b.today_begin - b.min_price) / abs(b.today_end - b.today_begin) > 2))"
                       + " ) tb"
                       + " where ta.stock_code = tb.stock_code"
                       + " and abs(ta.min_price - tb.min_price) / ta.min_price < 0.003"
                       + " and tb.today_end < (select t.thirty_avg"
                       + " from aistock_stock_avg_price t"
                       + " where t.stock_code = tb.stock_code and t.stock_day = tb.stock_day)";
            this.AutoFill(data, sql);
        }

        /// <summary>
        /// 十字星探底
        /// </summary>
        /// <param name="data"></param>
        /// <param name="date"></param>
        /// <param name="dateTo"></param>
        public void LoadSinglePinData(DataTable data, string date, string dateTo)
        {
            string sql = "select ta.stock_code, "
                       + " ta.stock_name, "
                       + " ta.today_begin, "
                       + " ta.today_end,"
                       + " tb.today_begin as pin_begin,"
                       + " tb.max_price as pin_max,"
                       + " tb.min_price as pin_min, "
                       + " (ta.today_end - tb.max_price) as diff, "
                       + " round((ta.today_end - tb.max_price) / ta.today_end , 4 ) as diff_percent "
                       + " from ("
                       + " select * from aistock_stock_information a"
                       + " where a.stock_day = '"+dateTo+"' ) ta,"
                       + " (select b.* from aistock_stock_information b, aistock_stock_avg_price c"
                       + " where b.stock_code = c.stock_code and"
                       + " b.stock_day = c.stock_day and"
                       + " b.stock_day = '"+date+"'"
                       + " and b.today_end <= c.thirty_avg"
                       + " and b.chart = '十字星') tb"
                       + " where ta.stock_code = tb.stock_code"
                       + " and ta.today_end > tb.max_price"
                       + " order by (ta.today_end - tb.max_price) / ta.today_end desc";
            this.AutoFill(data, sql);
        }

        /// <summary>
        /// 锤子线探底
        /// </summary>
        /// <param name="data"></param>
        /// <param name="date"></param>
        /// <param name="dateTo"></param>
        public void LoadHammerData(DataTable data, string date, string dateTo)
        {
            string sql = "select ta.stock_code,"
	                   + " ta.stock_name,"
	                   + " ta.today_begin,"
	                   + " ta.today_end,"
	                   + " tb.today_begin as hammer_begin,"
	                   + " tb.today_end as hammer_end,"
	                   + " tb.min_price as hammer_min,"
	                   + " (ta.today_end - tb.max_price) as diff,"
	                   + " (ta.today_end - tb.max_price) / ta.today_end as diff_percent"	
                       + " from (select * from aistock_stock_information c"
                       + " where c.stock_day = '"+dateTo+"') ta,"
                       + " (select * from aistock_stock_information a"
                       + " where a.stock_day = '"+date+"'"
                       + " and ((a.max_price = a.today_begin and a.today_begin > a.today_end and abs(a.today_end - a.min_price) / abs(a.today_begin - a.today_end) > 2) or" 
                       + " (a.max_price = a.today_end and a.today_end > a.today_begin and abs(a.today_begin - a.min_price) / abs(a.today_end - a.today_begin) > 2))"
                       + " and a.today_end < (select b.thirty_avg from aistock_stock_avg_price b where a.stock_code = b.stock_code and a.stock_day = b.stock_day)) tb"
                       + " where ta.stock_code = tb.stock_code"
                       + " and ta.today_end > tb.max_price"
                       + " order by (ta.today_end - tb.max_price) / ta.today_end desc";
            this.AutoFill(data, sql);
        }

        /// <summary>
        /// 早晨之星
        /// </summary>
        /// <param name="data"></param>
        /// <param name="one_date"></param>
        /// <param name="two_date"></param>
        /// <param name="three_date"></param>
        public void LoadMorningStarData(DataTable data, string one_date, string two_date, string three_date)
        {
            string sql = "select ta.stock_code,"
                       + " ta.stock_name,"
                       + " ta.today_begin as one_begin,"
                       + " ta.today_end as one_end,"
                       + " ta.min_price as one_min_price,"
                       + " ta.increase_percent as one_percent,"
                       + " tb.max_price as two_max_price,"
                       + " tb.today_begin as two_begin,"
                       + " tb.today_end as two_end,"
                       + " tb.min_price as two_min_price,"
                       + " tc.today_begin as three_begin,"
                       + " tc.today_end as three_end,"
                       + " tc.increase_percent as three_percent"
                       + " from"
                       + " (select * from aistock_stock_information c"
                       + " where c.stock_day = '" + one_date + "'"
                       + " and (c.today_begin > c.today_end"
                       + " and c.today_end > c.min_price"
                       + " and abs(c.today_begin - c.today_end) / abs(c.today_end - c.min_price) > 2) or c.chart = '光头光脚大阴线') ta,"
                       + " (select * from aistock_stock_information a"
                       + " where a.stock_day = '" + two_date + "'"
                       + " and ((a.today_begin > a.today_end and abs(a.today_end - a.min_price) / abs(a.today_begin - a.today_end) > 2"
                       + " and abs(a.max_price - a.today_begin) / abs(a.today_begin - a.today_end) > 2) or "
                       + " (a.today_end > a.today_begin and abs(a.today_begin - a.min_price) / abs(a.today_end - a.today_begin) > 2 "
                       + " and abs(a.max_price - a.today_end) / abs(a.today_end - a.today_begin) > 2) or a.chart = '十字星')"
                       + " and a.today_end < (select b.thirty_avg from aistock_stock_avg_price b where a.stock_day = b.stock_day "
                       + " and a.stock_code = b.stock_code)"
                       + " ) tb,"
                       + " (select * from aistock_stock_information d"
                       + " where d.stock_day = '" + three_date + "'"
                       + " and d.today_begin < d.today_end) tc"
                       + " where ta.stock_code = tb.stock_code"
                       + " and ta.stock_code = tc.stock_code"
                       + " and tb.today_begin < ta.today_end"
                       + " and tb.today_end < tc.today_begin"
                       + " and tb.today_end < ta.today_end"
                       + " and tb.today_begin < tc.today_begin";
            this.AutoFill(data, sql);
        }

        public void LoadStockBaseInfo(DataTable data, StockQueryCondition qc)
        {
            string stockCode = qc.StockCode;
            string market = qc.Market;
            string field = qc.Field;
            string province = qc.Province;
            string sql = "SELECT * FROM AISTOCK_STOCK_BASEINFO WHERE IS_VALID = 1";
            if (!string.IsNullOrEmpty(stockCode))
            {
                sql += " AND STOCK_CODE = '" + stockCode + "'";
            }
            if (!string.IsNullOrEmpty(market))
            {
                sql += " AND STOCK_ADDR = '" + market + "'";
            }
            if (!string.IsNullOrEmpty(field))
            {
                sql += " AND FIELD = '" + field + "'";
            }
            if (!string.IsNullOrEmpty(province))
            {
                sql += " AND PROVINCE = '" + province + "'";
            }
            sql += " ORDER BY PROFIT_PERCENT DESC, STOCK_CODE";
            this.AutoFill(data, sql);
        }

        public void LoadStockInfo(DataTable data, string date, string code)
        {
            string sql = "SELECT * FROM AISTOCK_STOCK_INFORMATION WHERE STOCK_DAY = '"+ date +"'";
            if (!string.IsNullOrEmpty(code))
            {
                sql += " AND STOCK_CODE LIKE '%" + code + "%'";
            }
            sql += " ORDER BY STOCK_CODE";
            this.AutoFill(data, sql);
        }

        public string GetToday(string date)
        {
            string sql = "select distinct STOCK_DAY from AISTOCK_STOCK_INFORMATION"
                           + " where STOCK_DAY <= '"+date+"'"
                           + " order by STOCK_DAY desc";
            return  this.GetScalar(sql).ToString();
        }

        public void LoadStockInfoJingQue(DataTable data, string date, string code)
        {
            string sql = "SELECT * FROM AISTOCK_STOCK_INFORMATION WHERE STOCK_DAY = '" + date + "'";
            if (!string.IsNullOrEmpty(code))
            {
                sql += " AND STOCK_CODE = '" + code + "'";
            }
            this.AutoFill(data, sql);
        }

        public void LoadStockAvgData(DataTable data, string date)
        {
            string sql = "SELECT * FROM AISTOCK_STOCK_AVG_PRICE WHERE STOCK_DAY = '" + date + "' ORDER BY STOCK_CODE";
            this.AutoFill(data, sql);
        }

        public bool ClearShiDaLiuTongGuData(string code, string date)
        {
            string sql = "select COUNT(0) from AISTOCK_STOCK_ShiDaLiuTongGu where STOCK_CODE='"+code+"' and UpdateDate='"+date+"'";
            if (int.Parse(this.GetScalar(sql).ToString()) == 10)
            {
                return false;
            }
            else if (int.Parse(this.GetScalar(sql).ToString()) > 0)
            {
                sql = "delete from AISTOCK_STOCK_ShiDaLiuTongGu where STOCK_CODE='" + code + "' and UpdateDate='" + date + "'";
                this.ExecuteNonQuerySql(sql);
                return true;
            }
            else
            {
                return true;
            }
        }

        public void SaveStockData(DataTable data)
        {
            this.ForceUpdate(data, "[AISTOCK].[dbo].[AISTOCK_STOCK_INFORMATION]");
        }

        public void SaveRSIData(DataTable data)
        {
            this.ForceUpdate(data, "[AISTOCK].[dbo].[AISTOCK_STOCK_COMMAND]");
        }

        public void SaveStockLog(DataTable data)
        {
            this.ForceUpdate(data, "[AISTOCK].[dbo].[AISTOCK_STOCK_LOG]");
        }

        public void SaveGuDongRenShu(DataTable data)
        {
            this.ForceUpdate(data, "[AISTOCK].[dbo].[AISTOCK_STOCK_GUDONGRENSHU]");
        }

        public void SaveShiDaLiuTongGu(DataTable data)
        {
            this.ForceUpdate(data, "[AISTOCK].[dbo].[AISTOCK_STOCK_ShiDaLiuTongGu]");
        }

        public void SaveStockStatsData(DataTable data)
        {
            this.ForceUpdate(data, "[AISTOCK].[dbo].[AISTOCK_STOCK_BUY_STATS]");
        }

        public void SaveAvgPrice(DataTable data)
        {
            this.ForceUpdate(data, "[AISTOCK].[dbo].[AISTOCK_STOCK_AVG_PRICE]");
        }

        public string GetIndexDay(string date, int indexDay)
        { 
            string sql = "SELECT THE_DATE "
                       + " FROM AISTOCK.DBO.AISTOCK_CALENDAR"
                       + " WHERE TRADE_DAYS = (("
                       + "SELECT TRADE_DAYS " 
                       + " FROM AISTOCK.DBO.AISTOCK_CALENDAR"
                       + " WHERE THE_DATE = '" + DateTime.Parse(date) + "') - '" + indexDay + "' ) and THE_DATE <= '" + DateTime.Parse(date) + "' order by the_date desc";
            return this.GetScalar(sql).ToString();
        }

        public string GetFiscalPeriod(string date)
        {
            string sql = "select fiscal_period from aistock_calendar"
                       + " where the_date = '"+date+"'";
            return this.GetScalar(sql).ToString();
        }

        /// <summary>
        /// 获取一只股票的5日均价
        /// </summary>
        /// <param name="stockCode"></param>
        /// <param name="stockDay"></param>
        /// <returns></returns>
        public decimal GetFiveAvgPrice(string stockCode, string stockDay)
        {
            string sql = "SELECT AVG(A.TODAY_END) AS FIVE_AVG FROM"
                       + " (SELECT TOP 5 STOCK_CODE, STOCK_NAME, TODAY_END"
                       + " FROM [AISTOCK].[dbo].[AISTOCK_STOCK_INFORMATION]"
                       + " WHERE STOCK_CODE = '" + stockCode + "' AND STOCK_DAY <= '" + stockDay + "' AND TODAY_END <> 0 ORDER BY STOCK_DAY DESC) A"
                       + " GROUP BY A.STOCK_CODE, A.STOCK_NAME";
            return decimal.Parse(this.GetScalar(sql) == null ? "0" : this.GetScalar(sql).ToString());
        }

        /// <summary>
        /// 获取一只股票的10日均价
        /// </summary>
        /// <param name="stockCode"></param>
        /// <param name="stockDay"></param>
        /// <returns></returns>
        public decimal GetTenAvgPrice(string stockCode, string stockDay)
        {
            string sql = "SELECT AVG(A.TODAY_END) AS TEN_AVG FROM"
                       + " (SELECT TOP 10 STOCK_CODE, STOCK_NAME, TODAY_END"
                       + " FROM [AISTOCK].[dbo].[AISTOCK_STOCK_INFORMATION]"
                       + " WHERE STOCK_CODE = '" + stockCode + "' AND STOCK_DAY <= '" + stockDay + "' AND TODAY_END <> 0 ORDER BY STOCK_DAY DESC) A"
                       + " GROUP BY A.STOCK_CODE, A.STOCK_NAME";
            return decimal.Parse(this.GetScalar(sql) == null ? "0" : this.GetScalar(sql).ToString());
        }

        /// <summary>
        /// 获取一只股票的20日均价
        /// </summary>
        /// <param name="stockCode"></param>
        /// <param name="stockDay"></param>
        /// <returns></returns>
        public decimal GetTwentyAvgPrice(string stockCode, string stockDay)
        {
            string sql = "SELECT AVG(A.TODAY_END) AS TWENTY_AVG FROM"
                       + " (SELECT TOP 20 STOCK_CODE, STOCK_NAME, TODAY_END"
                       + " FROM [AISTOCK].[dbo].[AISTOCK_STOCK_INFORMATION]"
                       + " WHERE STOCK_CODE = '" + stockCode + "' AND STOCK_DAY <= '" + stockDay + "' AND TODAY_END <> 0 ORDER BY STOCK_DAY DESC) A"
                       + " GROUP BY A.STOCK_CODE, A.STOCK_NAME";
            return decimal.Parse(this.GetScalar(sql) == null ? "0" : this.GetScalar(sql).ToString());
        }

        /// <summary>
        /// 获取一只股票的30日均价
        /// </summary>
        /// <param name="stockCode"></param>
        /// <param name="stockDay"></param>
        /// <returns></returns>
        public decimal GetThirtyAvgPrice(string stockCode, string stockDay)
        {
            string sql = "SELECT AVG(A.TODAY_END) AS THIRTY_AVG FROM"
                       + " (SELECT TOP 30 STOCK_CODE, STOCK_NAME, TODAY_END"
                       + " FROM [AISTOCK].[dbo].[AISTOCK_STOCK_INFORMATION]"
                       + " WHERE STOCK_CODE = '" + stockCode + "' AND STOCK_DAY <= '" + stockDay + "' AND TODAY_END <> 0 ORDER BY STOCK_DAY DESC) A"
                       + " GROUP BY A.STOCK_CODE, A.STOCK_NAME";
            return decimal.Parse(this.GetScalar(sql) == null ? "0" : this.GetScalar(sql).ToString());
        }

        /// <summary>
        /// 获取一只股票的60日均价
        /// </summary>
        /// <param name="stockCode"></param>
        /// <param name="stockDay"></param>
        /// <returns></returns>
        public decimal GetSixtyAvgPrice(string stockCode, string stockDay)
        {
            string sql = "SELECT AVG(A.TODAY_END) AS SIXTY_AVG FROM"
                       + " (SELECT TOP 60 STOCK_CODE, STOCK_NAME, TODAY_END"
                       + " FROM [AISTOCK].[dbo].[AISTOCK_STOCK_INFORMATION]"
                       + " WHERE STOCK_CODE = '" + stockCode + "' AND STOCK_DAY <= '" + stockDay + "' AND TODAY_END <> 0 ORDER BY STOCK_DAY DESC) A"
                       + " GROUP BY A.STOCK_CODE, A.STOCK_NAME";
            return decimal.Parse(this.GetScalar(sql) == null ? "0" : this.GetScalar(sql).ToString());
        }

        public void LoadStockDataIndex(DataTable data, string dateFrom, string dateTo, string code)
        {
            string sql = "SELECT * FROM AISTOCK_STOCK_INDEX_V WHERE STOCK_DAY > '" + dateFrom + "' AND STOCK_DAY <= '" + dateTo + "'";
            if (!string.IsNullOrEmpty(code))
            {
                sql += " AND STOCK_CODE LIKE '%" + code + "%'";
            }
            this.AutoFill(data, sql);
        }

        public void LoadWMSIndex(DataTable data, string dateFrom, string dateTo, string code)
        {
            string sql = "SELECT STOCK_CODE, STOCK_NAME, MAX(MAX_PRICE) AS MAX_PRICE, MIN(MIN_PRICE) AS MIN_PRICE"
                + " FROM AISTOCK_STOCK_INFORMATION"
                + " WHERE STOCK_DAY > '" + dateFrom + "' AND STOCK_DAY <= '" + dateTo + "'";
            if (!string.IsNullOrEmpty(code))
            {
                sql += " AND STOCK_CODE LIKE '%" + code + "%'";
            }
            sql += " GROUP BY STOCK_CODE, STOCK_NAME"
                 + " ORDER BY STOCK_CODE";
            this.AutoFill(data, sql);
        }

        public void LoadADRIndex(DataTable data, string dateFrom, string dateTo)
        {
            string sql = "SELECT COUNT(*) AS TOTAL, 'INCREASE' AS DSC FROM AISTOCK_STOCK_INFORMATION"
                       + " WHERE STOCK_DAY >= '"+dateFrom+"' AND STOCK_DAY <= '"+dateTo+"'"
                       + " AND INCREASE_PERCENT > 0"
                       + " UNION ALL"
                       + " SELECT COUNT(*) AS TOTAL, 'DECREASE' AS DSC FROM AISTOCK_STOCK_INFORMATION"
                       + " WHERE STOCK_DAY >= '"+dateFrom+"' AND STOCK_DAY <= '"+dateTo+"'"
                       + " AND INCREASE_PERCENT < 0";
            this.AutoFill(data, sql);
        }

        public void LoadDates(DataTable data, DateTime dateFrom, DateTime dateTo)
        {
            string sql = "SELECT THE_DATE FROM AISTOCK.DBO.AISTOCK_CALENDAR WHERE THE_DATE >= '"+dateFrom+"' AND THE_DATE <= '"+dateTo+"' AND FISCAL_PERIOD = 1 ORDER BY THE_DATE";
            this.AutoFill(data, sql);
        }

        /// <summary>
        /// 根据证监会行业统计当天的增长比率
        /// </summary>
        /// <param name="data"></param>
        /// <param name="date"></param>
        public void LoadStockStatsVData(DataTable data, string date)
        {
            string sql = "SELECT A.FIELD_MAIN, A.TOTAL, B.TOTAL_INCREASE, C.TOTAL_DECREASE FROM "
                       + " (SELECT FIELD_MAIN, COUNT(0) AS TOTAL"
                       + " FROM AISTOCK_STOCK_BASEINFO"
                       + " GROUP BY FIELD_MAIN) A LEFT JOIN"
                       + " (SELECT FIELD_MAIN, COUNT(0) AS TOTAL_INCREASE"
                       + " FROM AISTOCK_STOCK_DAILY_DATA_V"
                       + " WHERE STOCK_DAY = '" + date + "' AND INCREASE_PERCENT > 0"
                       + " GROUP BY FIELD_MAIN) B ON A.FIELD_MAIN = B.FIELD_MAIN"
                       + " LEFT JOIN"
                       + " (SELECT FIELD_MAIN, COUNT(0) AS TOTAL_DECREASE"
                       + " FROM AISTOCK_STOCK_DAILY_DATA_V"
                       + " WHERE STOCK_DAY = '" + date + "' AND INCREASE_PERCENT < 0"
                       + " GROUP BY FIELD_MAIN) C ON A.FIELD_MAIN = C.FIELD_MAIN";
            this.AutoFill(data, sql);
        }

        public void LoadLeaderStock(DataTable data, string date)
        {
            string sql = "SELECT A.FIELD_MAIN, A.INCREASE_PERCENT, B.STOCK_CODE, B.STOCK_NAME, B.FIELD,B.PROVINCE,B.CHART FROM"
                + " (SELECT FIELD_MAIN,MAX(INCREASE_PERCENT) AS INCREASE_PERCENT"
                + " FROM AISTOCK_STOCK_DAILY_DATA_V"
                + " WHERE STOCK_DAY = '" + date + "'"
                + " GROUP BY FIELD_MAIN) A, AISTOCK_STOCK_DAILY_DATA_V B"
                + " WHERE A.INCREASE_PERCENT = B.INCREASE_PERCENT AND B.STOCK_DAY = '" + date + "'"
                + " AND A.FIELD_MAIN = B.FIELD_MAIN"
                + " ORDER BY A.INCREASE_PERCENT DESC";
            this.AutoFill(data, sql);
        }

        public void DeleteData(string dateFrom, string dateTo, out bool hasData)
        {
            string sql = "select top 1 * from ("
                            + " select top 30 the_date from AISTOCK_CALENDAR "
                            + " where the_date <= '"+dateTo+"' and the_date > '"+dateFrom+"' and fiscal_period = 1"
                            + " order by the_date desc) a order by a.the_date ";
            string date = this.GetScalar(sql).ToString();
            
            date = DateTimeFunction.ConvertDate(DateTime.Parse(date).ToShortDateString());
            sql = "select count(0) from AISTOCK_STOCK_INFORMATION where STOCK_DAY < '" + date + "'";
            int count = int.Parse(this.GetScalar(sql).ToString());
            if (count > 0)
            {
                sql = "delete from AISTOCK_STOCK_INFORMATION where STOCK_DAY < '" + date + "'";
                this.ExecuteNonQuerySql(sql);
                sql = "delete from AISTOCK_STOCK_AVG_PRICE where STOCK_DAY < '" + date + "'";
                this.ExecuteNonQuerySql(sql);
                hasData = true;
            }
            else {
                hasData = false;
            }
        }

        public int GetQuantity(string code)
        {
            string sql = "select COUNT(0) from AISTOCK_STOCK_INFORMATION where STOCK_CODE = '"+code+"' and QUANTITY <> 0 ";
            int count = int.Parse(this.GetScalar(sql).ToString());
            count = count / 2;
            if (count == 0) return 0;
            sql = "select top 1 a.QUANTITY from (select top "+count+" QUANTITY from AISTOCK_STOCK_INFORMATION where STOCK_CODE = '"+code+"' and QUANTITY <> 0 order by QUANTITY desc) a order by a.QUANTITY";
            return int.Parse(this.GetScalar(sql).ToString());
        }

        public void LoadRSIData(DataTable data, string date)
        {
            string sql = "select * from AISTOCK_STOCK_COMMAND where STOCK_DAY = '"+date+"'";
            this.AutoFill(data, sql);
        }

        public int GetTotalWords()
        {
            string sql = "select count(WORDS_ID) from AISTOCK_STOCK_WORDS";
            return int.Parse(this.GetScalar(sql).ToString());
        }

        public string GetWords(int id)
        {
            string sql = "select WORDS from AISTOCK_STOCK_WORDS where WORDS_ID = " + id;
            return this.GetScalar(sql).ToString();
        }

        public void Test(DataTable data)
        {
            string sql = "SELECT * FROM AISTOCK_TEST";
            //成功
            //this.AutoFill(data, sql);
            //
            AISTOCK_TEST_DATA test = new AISTOCK_TEST_DATA();
            AISTOCK_TEST_DATA.AISTOCK_TESTRow row = test.AISTOCK_TEST.NewAISTOCK_TESTRow();
            row.NAME = "test";
            test.AISTOCK_TEST.Rows.Add(row);
            row = test.AISTOCK_TEST.NewAISTOCK_TESTRow();
            row.NAME = "test1";
            test.AISTOCK_TEST.Rows.Add(row);
            //成功
            this.AutoUpdate(test.AISTOCK_TEST, "AISTOCK_TEST");
        }


    }
}
