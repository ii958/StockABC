using System;
using System.Data;
using AISRS.BusinessRules;
using AISRS.Common.Data;
using AISRS.Common.Query;

namespace AISRS.BusinessFacade
{
    public class StockSystem
    {
        public AISTOCK_FIELD_DOMAIN_VALUE_DATA GetDropDownList(string param)
        {
            return new Stock().GetDropDownList(param);
        }

        public AISTOCK_STOCK_COMMAND_DATA GetRSIData(string date)
        {
            return new Stock().GetRSIData(date);
        }

        public int GetTotalWords()
        {
            return new Stock().GetTotalWords();
        }

        public string GetWords(int id)
        {
            return new Stock().GetWords(id);
        }

        public int GetQuantity(string code)
        {
            return new Stock().GetQuantity(code);
        }

        public AISTOCK_STOCK_STATS_DATA GetStockData(string date, string code)
        {
            return new Stock().GetStockData(date, code);
        }

        public AISTOCK_STOCK_STATS_DATA GetStockDataJingQue(string date, string code)
        {
            return new Stock().GetStockDataJingQue(date, code);
        }

        public string GetToday(string date)
        {
            return new Stock().GetToday(date);
        }

        public void DeleteData(string dateFrom, string dateTo, out bool hasData)
        {
            new Stock().DeleteData(dateFrom, dateTo, out hasData);
        }

        public AISTOCK_STOCK_IMPORT_DATA GetStockImport()
        {
            return new Stock().GetStockImport();
        }

        public AISTOCK_STOCK_IMPORT_DATA GetStockImport(bool option)
        {
            return new Stock().GetStockImport(option);
        }

        public void IsJobFinished(string code)
        {
            new Stock().IsJobFinished(code);
        }

        public void UpdateData(string code, string name)
        {
            new Stock().UpdateData(code,name);
        }

        public bool IsExistGuDongRenShu(string code, string dateIndex)
        {
            return new Stock().IsExistGuDongRenShu(code, dateIndex);
        }

        public void ClearData(string code)
        {
            new Stock().ClearData(code);
        }

        public AISTOCK_STOCK_AVG_PRICE_DATA GetStockAvgData(string date)
        {
            return new Stock().GetStockAvgData(date);
        }

        public decimal GetAvgPrice(string stockCode, string date, int type)
        {
            return new Stock().GetAvgPrice(stockCode, date, type);
        }

        public DataTable GetDates(DateTime dateFrom, DateTime dateTo)
        {
            return new Stock().GetDates(dateFrom, dateTo);
        }

        public string GetIndexDay(string date, int indexDay)
        {
            return new Stock().GetIndexDay(date, indexDay);
        }

        public void SaveRSIData(AISTOCK_STOCK_COMMAND_DATA data)
        {
            new Stock().SaveRSIData(data);
        }

        public AISTOCK_STOCK_INDEX_V_DATA GetStockDataIndex(string dateFrom, string dateTo, string code)
        {
            return new Stock().GetStockDataIndex(dateFrom, dateTo, code);
        }

        public AISTOCK_STOCK_WMS_V_DATA GetStockWmsData(string dateFrom, string dateTo, string code)
        {
            return new Stock().GetStockWmsData(dateFrom, dateTo, code);
        }

        public AISTOCK_STOCK_ADR_INDEX_V_DATA GetStockAdrData(string dateFrom, string dateTo, string code)
        {
            return new Stock().GetStockAdrData(dateFrom, dateTo, code);
        }

        public void ImportStockData(AISTOCK_STOCK_STATS_DATA data)
        {
            new Stock().ImportStockData(data);
        }

        public void SaveStockStatsData(AISTOCK_STOCK_BUY_STATS_DATA data)
        {
            new Stock().SaveStockStatsData(data);
        }
        public void SaveStockLog(AISTOCK_STOCK_LOG_DATA data) {
            new Stock().SaveStockLog(data);
        }
        public void SaveGuDongRenShu(AISTOCK_STOCK_GUDONGRENSHU_DATA data)
        {
            new Stock().SaveGuDongRenShu(data);
        }
        public bool ClearShiDaLiuTongGuData(string code, string date)
        {
            return new Stock().ClearShiDaLiuTongGuData(code, date);
        }
        public void SaveShiDaLiuTongGu(AISTOCK_STOCK_ShiDaLiuTongGu_DATA data)
        {
            new Stock().SaveShiDaLiuTongGu(data);
        }
        public void InsertAvgPrice(AISTOCK_STOCK_AVG_PRICE_DATA data)
        {
            new Stock().InsertAvgPrice(data);
        }

        public AISTOCK_CALENDAR_DATA GetAllCalendarData()
        {
            return new Stock().GetAllCalendarData();
        }

        public AISTOCK_CALENDAR_DATA GetCalendarByID(string id)
        {
            return new Stock().GetCalendarByID(id);
        }

        public void SaveDailyReport(AISTOCK_CALENDAR_DATA data)
        {
            new Stock().SaveDailyReport(data);
        }

        public AISTOCK_STOCK_BASEINFO_DATA GetStockBaseInfo(StockQueryCondition qc)
        {
            return new Stock().GetStockBaseInfo(qc);
        }

        public AISTOCK_STOCK_BASEINFO_DATA GetStockBaseInfoWithoutCondition()
        {
            return new Stock().GetStockBaseInfoWithoutCondition();
        }

        /// <summary>
        /// 低档五连阳
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public AISTOCK_STOCK_LOW_FIVE_V_DATA GetLowFiveData(string date, string dateTo)
        {
            return new Stock().GetLowFiveData(date, dateTo);
        }

        /// <summary>
        /// 双针探底
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public AISTOCK_STOCK_TWO_PIN_V_DATA GetTwoPinData(string date, string dateTo)
        {
            return new Stock().GetTwoPinData(date, dateTo);
        }

        /// <summary>
        /// 十字星探底
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public AISTOCK_STOCK_SINGLE_PIN_V_DATA GetSinglePinData(string date, string dateTo)
        {
            return new Stock().GetSinglePinData(date, dateTo);
        }

        /// <summary>
        /// 锤子线探底
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public AISTOCK_STOCK_HAMMER_V_DATA GetHammerData(string date, string dateTo)
        {
            return new Stock().GetHammerData(date, dateTo);
        }

        /// <summary>
        /// 早晨之星
        /// </summary>
        /// <param name="oneDate"></param>
        /// <param name="twoDate"></param>
        /// <param name="threeDate"></param>
        /// <returns></returns>
        public AISTOCK_STOCK_MORNING_STAR_V_DATA GetMorningStarData(string oneDate, string twoDate, string threeDate)
        {
            return new Stock().GetMorningStarData(oneDate, twoDate, threeDate);
        }

        public AISTOCK_STOCK_DAILY_DATA_V_DATA GetStockHistory(StockQueryCondition qc)
        {
            return new Stock().GetStockHistory(qc);
        }

        /// <summary>
        /// 查询买入股历史数据
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public AISTOCK_STOCK_BUY_HISTORY_DATA GetBuyHistoryData(StockQueryCondition qc)
        {
            return new Stock().GetBuyHistoryData(qc);
        }

        /// <summary>
        /// 判断是否是休市日
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsValidDate(string date)
        {
            return new Stock().IsValidDate(date);
        }

        public AISTOCK_STOCK_AVG_PRICE_DATA GetStockAvgHistory(StockQueryCondition qc)
        {
            return new Stock().GetStockAvgHistory(qc);
        }

        public bool HasImport(string date)
        {
            return new Stock().HasImport(date);
        }

        public bool IsExistsData(string date)
        {
            return new Stock().IsExistsData(date);
        }

        public void DeleteAvgData(string date)
        {
            new Stock().DeleteAvgData(date);
        }

        /// <summary>
        /// 根据证监会行业统计当天的增长比率
        /// </summary>
        /// <param name="date"></param>
        public AISTOCK_STATS_PERCENT_V_DATA GetStockStatsVData(string date)
        {
            return new Stock().GetStockStatsVData(date);
        }

        public DataTable GetLeaderStock(string date)
        {
            return new Stock().GetLeaderStock(date);
        }

        public AISTOCK_TEST_DATA Test()
        {
            return new Stock().Test();
            //return null;
        }
    }
}
