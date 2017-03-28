using System;
using System.Data;

using AISRS.Common.Data;
using AISRS.DataAccess;
using AISRS.Common.Exception;
using AISRS.Common.Query;

namespace AISRS.BusinessRules
{
    public class Stock
    {
        public AISTOCK_FIELD_DOMAIN_VALUE_DATA GetDropDownList(string param)
        {
            AISTOCK_FIELD_DOMAIN_VALUE_DATA data = new AISTOCK_FIELD_DOMAIN_VALUE_DATA();
            using (DaStock da = new DaStock())
            {
                if (param.Equals("market"))
                {
                    da.LoadAllMarket(data.AISTOCK_FIELD_DOMAIN_VALUE);
                }
                else if (param.Equals("field_main"))
                {
                    da.LoadAllFieldMain(data.AISTOCK_FIELD_DOMAIN_VALUE);
                }
                else if (param.Equals("field"))
                {
                    da.LoadAllField(data.AISTOCK_FIELD_DOMAIN_VALUE);
                }
                else if (param.Equals("province"))
                {
                    da.LoadAllProvince(data.AISTOCK_FIELD_DOMAIN_VALUE);
                }
                else if (param.Equals("chart"))
                {
                    da.LoadAllChart(data.AISTOCK_FIELD_DOMAIN_VALUE);
                }
                else if (param.Equals("index"))
                {
                    da.LoadAllIndex(data.AISTOCK_FIELD_DOMAIN_VALUE);
                }
                else if (param.Equals("buy"))
                {
                    da.LoadAllBuyPoint(data.AISTOCK_FIELD_DOMAIN_VALUE);
                }
                else if (param.Equals("date"))
                {
                    da.LoadAllDateString(data.AISTOCK_FIELD_DOMAIN_VALUE);
                }
            }
            return data;
        }

        public AISTOCK_STOCK_STATS_DATA GetStockData(string date, string code)
        {
            AISTOCK_STOCK_STATS_DATA data = new AISTOCK_STOCK_STATS_DATA();
            using(DaStock da = new DaStock())
            {
                da.LoadStockInfo(data.AISTOCK_STOCK_INFORMATION, date, code);
            }
            return data;
        }

        public AISTOCK_STOCK_COMMAND_DATA GetRSIData(string date)
        {
            AISTOCK_STOCK_COMMAND_DATA data = new AISTOCK_STOCK_COMMAND_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadRSIData(data.AISTOCK_STOCK_COMMAND, date);               
            }
            return data;
        }

        public string GetWords(int id)
        {
            using (DaStock da = new DaStock())
            {
                return da.GetWords(id);
            }
        }

        public int GetTotalWords()
        {
            using (DaStock da = new DaStock())
            {
                return da.GetTotalWords();
            }
        }

        public int GetQuantity(string code)
        {
            using (DaStock da = new DaStock())
            {
                return da.GetQuantity(code);
            }
        }

        public string GetToday(string date)
        {
            using (DaStock da = new DaStock())
            {
                return da.GetToday(date);
            }
        }

        public AISTOCK_STOCK_STATS_DATA GetStockDataJingQue(string date, string code)
        {
            AISTOCK_STOCK_STATS_DATA data = new AISTOCK_STOCK_STATS_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadStockInfoJingQue(data.AISTOCK_STOCK_INFORMATION, date, code);
            }
            return data;
        }

        public void DeleteData(string dateFrom, string dateTo, out bool hasData)
        {
            using (DaStock da = new DaStock())
            {
                da.DeleteData(dateFrom, dateTo, out hasData);
            }
        }
        public AISTOCK_STOCK_IMPORT_DATA GetStockImport()
        {
            AISTOCK_STOCK_IMPORT_DATA data = new AISTOCK_STOCK_IMPORT_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadStockImport(data.AISTOCK_STOCK_IMPORT);
            }
            return data;
        }

        public AISTOCK_STOCK_IMPORT_DATA GetStockImport(bool option)
        {
            AISTOCK_STOCK_IMPORT_DATA data = new AISTOCK_STOCK_IMPORT_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadStockImport(data.AISTOCK_STOCK_IMPORT, option);
            }
            return data;
        }

        public void IsJobFinished(string code)
        {
            using (DaStock da = new DaStock())
            {
                da.IsJobFinished(code);
            }
        }

        public void UpdateData(string code, string name)
        {
            using (DaStock da = new DaStock())
            {
                da.UpdateData(code, name);
            }
        }

        public bool IsExistGuDongRenShu(string code, string dateIndex)
        {
            using (DaStock da = new DaStock())
            {
                return da.IsExistGuDongRenShu(code, dateIndex);
            }
        }

        public void ClearData(string code)
        {
            using (DaStock da = new DaStock())
            {
                da.ClearData(code);
            }
        }

        public AISTOCK_STOCK_AVG_PRICE_DATA GetStockAvgData(string date)
        {
            AISTOCK_STOCK_AVG_PRICE_DATA data = new AISTOCK_STOCK_AVG_PRICE_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadStockAvgData(data.AISTOCK_STOCK_AVG_PRICE, date);
            }
            return data;
        }

        public decimal GetAvgPrice(string stockCode, string date, int type)
        {
            decimal result = 0;
            using (DaStock da = new DaStock())
            {
                if (type == 5)
                {
                    result = da.GetFiveAvgPrice(stockCode, date);
                }
                else if(type == 10)
                {
                    result = da.GetTenAvgPrice(stockCode, date);
                }
                else if (type == 20)
                {
                    result = da.GetTwentyAvgPrice(stockCode, date);
                }
                else if (type == 30)
                { 
                   result = da.GetThirtyAvgPrice(stockCode, date);
                }
                else if (type == 60)
                {
                    result = da.GetSixtyAvgPrice(stockCode, date);
                }
            }
            return result;
        }

        public bool HasImport(string date)
        {
            using (DaStock da = new DaStock())
            {
                int count = da.HasImport(date);
                if (count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsExistsData(string date)
        {
            using (DaStock da = new DaStock())
            {
                int count = da.IsExistsData(date);
                if (count > 0)
                {
                    return true;
                }                
            }
            return false;
        }

        public void DeleteAvgData(string date)
        {
            using (DaStock da = new DaStock())
            {
                da.DeleteAvgData(date);
            }
        }

        public DataTable GetDates(DateTime dateFrom, DateTime dateTo)
        {
            DataTable data = new DataTable();
            using (DaStock da = new DaStock())
            {
                da.LoadDates(data, dateFrom, dateTo);
            }
            return data;
        }

        public AISTOCK_CALENDAR_DATA GetAllCalendarData()
        {
            AISTOCK_CALENDAR_DATA data = new AISTOCK_CALENDAR_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadAllCalendar(data.AISTOCK_CALENDAR);
            }
            return data;
        }

        public string GetIndexDay(string date, int indexDay)
        {
            using (DaStock da = new DaStock())
            {
                return da.GetIndexDay(date, indexDay);
            }
        }

        public AISTOCK_STOCK_INDEX_V_DATA GetStockDataIndex(string dateFrom, string dateTo, string code)
        {
            AISTOCK_STOCK_INDEX_V_DATA data = new AISTOCK_STOCK_INDEX_V_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadStockDataIndex(data.AISTOCK_STOCK_INDEX_V, dateFrom, dateTo, code);
            }
            return data;
        }

        public AISTOCK_STOCK_WMS_V_DATA GetStockWmsData(string dateFrom, string dateTo, string code)
        {
            AISTOCK_STOCK_WMS_V_DATA data = new AISTOCK_STOCK_WMS_V_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadWMSIndex(data.AISTOCK_STOCK_WMS_V, dateFrom, dateTo, code);
            }
            return data;
        }

        public AISTOCK_STOCK_ADR_INDEX_V_DATA GetStockAdrData(string dateFrom, string dateTo, string code)
        {
            AISTOCK_STOCK_ADR_INDEX_V_DATA data = new AISTOCK_STOCK_ADR_INDEX_V_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadADRIndex(data.AISTOCK_STOCK_ADR_INDEX, dateFrom, dateTo);
            }
            return data;
        }

        public AISTOCK_CALENDAR_DATA GetCalendarByID(string id)
        {
            AISTOCK_CALENDAR_DATA data = new AISTOCK_CALENDAR_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadCalendarByID(data.AISTOCK_CALENDAR, id);
            }
            return data;
        }

        public void SaveDailyReport(AISTOCK_CALENDAR_DATA data)
        {
            using (DaStock da = new DaStock())
            {
                da.SaveDailyReport(data.AISTOCK_CALENDAR);
            }
        }

        public void SaveStockLog(AISTOCK_STOCK_LOG_DATA data)
        {
            using (DaStock da = new DaStock())
            {
                da.SaveStockLog(data.AISTOCK_STOCK_LOG);
            }
        }

        public void SaveGuDongRenShu(AISTOCK_STOCK_GUDONGRENSHU_DATA data)
        {
            using (DaStock da = new DaStock())
            {
                da.SaveGuDongRenShu(data.AISTOCK_STOCK_GUDONGRENSHU);
            }
        }

        public bool ClearShiDaLiuTongGuData(string code, string date)
        {
            using (DaStock da = new DaStock())
            {
                return da.ClearShiDaLiuTongGuData(code, date);
            }
        }

        public void SaveShiDaLiuTongGu(AISTOCK_STOCK_ShiDaLiuTongGu_DATA data)
        {
            using (DaStock da = new DaStock())
            {
                da.SaveShiDaLiuTongGu(data.AISTOCK_STOCK_ShiDaLiuTongGu);
            }
        }

        public void SaveRSIData(AISTOCK_STOCK_COMMAND_DATA data)
        {
            using (DaStock da = new DaStock())
            {
                da.SaveRSIData(data.AISTOCK_STOCK_COMMAND);
            }
        }

        public AISTOCK_STOCK_BASEINFO_DATA GetStockBaseInfo(StockQueryCondition qc)
        {
            AISTOCK_STOCK_BASEINFO_DATA data = new AISTOCK_STOCK_BASEINFO_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadStockBaseInfo(data.AISTOCK_STOCK_BASEINFO, qc);
            }
            return data;
        }

        public AISTOCK_STOCK_BASEINFO_DATA GetStockBaseInfoWithoutCondition()
        {
            AISTOCK_STOCK_BASEINFO_DATA data = new AISTOCK_STOCK_BASEINFO_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadStockBaseInfoWithoutCondition(data.AISTOCK_STOCK_BASEINFO);
            }
            return data;
        }

        /// <summary>
        /// 低档五连阳
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public AISTOCK_STOCK_LOW_FIVE_V_DATA GetLowFiveData(string date, string dateTo)
        {
            AISTOCK_STOCK_LOW_FIVE_V_DATA data = new AISTOCK_STOCK_LOW_FIVE_V_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadLowFiveData(data.AISTOCK_STOCK_LOW_FIVE_V, date, dateTo);
            }
            return data;
        }

        /// <summary>
        /// 双针探底
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public AISTOCK_STOCK_TWO_PIN_V_DATA GetTwoPinData(string date, string dateTo)
        {
            AISTOCK_STOCK_TWO_PIN_V_DATA data = new AISTOCK_STOCK_TWO_PIN_V_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadTwoPinData(data.AISTOCK_STOCK_TWO_PIN_V, date, dateTo);
            }
            return data;
        }

        /// <summary>
        /// 十字星探底
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public AISTOCK_STOCK_SINGLE_PIN_V_DATA GetSinglePinData(string date, string dateTo)
        {
            AISTOCK_STOCK_SINGLE_PIN_V_DATA data = new AISTOCK_STOCK_SINGLE_PIN_V_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadSinglePinData(data.AISTOCK_STOCK_SINGLE_PIN_V, date, dateTo);
            }
            return data;
        }

        /// <summary>
        /// 锤子线探底
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public AISTOCK_STOCK_HAMMER_V_DATA GetHammerData(string date, string dateTo)
        {
            AISTOCK_STOCK_HAMMER_V_DATA data = new AISTOCK_STOCK_HAMMER_V_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadHammerData(data.AISTOCK_STOCK_HAMMER_V, date, dateTo);
            }
            return data;
        }

        /// <summary>
        /// 判断是否是休市日
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsValidDate(string date)
        {
            using (DaStock da = new DaStock())
            {
                string result = da.GetFiscalPeriod(date);
                return result.Equals("1") ? true : false;
            }
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
            AISTOCK_STOCK_MORNING_STAR_V_DATA data = new AISTOCK_STOCK_MORNING_STAR_V_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadMorningStarData(data.AISTOCK_STOCK_MORNING_STAR_V, oneDate, twoDate, threeDate);
            }
            return data;
        }

        public AISTOCK_STOCK_DAILY_DATA_V_DATA GetStockHistory(StockQueryCondition qc)
        {
            AISTOCK_STOCK_DAILY_DATA_V_DATA data = new AISTOCK_STOCK_DAILY_DATA_V_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadStockHistory(data.AISTOCK_STOCK_DAILY_DATA_V, qc);
            }
            return data;
        }

        /// <summary>
        /// 查询买入股历史数据
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public AISTOCK_STOCK_BUY_HISTORY_DATA GetBuyHistoryData(StockQueryCondition qc)
        {
            AISTOCK_STOCK_BUY_HISTORY_DATA data = new AISTOCK_STOCK_BUY_HISTORY_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadBuyHistoryData(data.AISTOCK_STOCK_BUY_HISTORY, qc);
            }
            return data;
        }

        public AISTOCK_STOCK_AVG_PRICE_DATA GetStockAvgHistory(StockQueryCondition qc)
        {
            AISTOCK_STOCK_AVG_PRICE_DATA data = new AISTOCK_STOCK_AVG_PRICE_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadStockAvgHistory(data.AISTOCK_STOCK_AVG_PRICE, qc);
            }
            return data;
        }

        public void ImportStockData(AISTOCK_STOCK_STATS_DATA data)
        {
            using (DaStock da = new DaStock())
            {
                try 
                {
                    da.SaveStockData(data.AISTOCK_STOCK_INFORMATION);
                }
                catch (Exception ex)
                {
                    throw new CommonException("保存股票信息数据时出错，请联系管理!" + ex.Message);
                }
            }
        }

        public void SaveStockStatsData(AISTOCK_STOCK_BUY_STATS_DATA data)
        {
            using (DaStock da = new DaStock())
            {
                try
                {
                    da.SaveStockStatsData(data.AISTOCK_STOCK_BUY_STATS);
                }
                catch (Exception ex)
                {
                    throw new CommonException("保存股票信息数据时出错，请联系管理!" + ex.Message);
                }
            }
        }

        public void InsertAvgPrice(AISTOCK_STOCK_AVG_PRICE_DATA data)
        {
            using (DaStock da = new DaStock())
            {
                try
                {
                    da.SaveAvgPrice(data.AISTOCK_STOCK_AVG_PRICE);
                }
                catch (Exception ex)
                {
                    throw new CommonException("保存股票信息数据时出错，请联系管理!" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 根据证监会行业统计当天的增长比率
        /// </summary>
        /// <param name="date"></param>
        public AISTOCK_STATS_PERCENT_V_DATA GetStockStatsVData(string date)
        {
            AISTOCK_STATS_PERCENT_V_DATA data = new AISTOCK_STATS_PERCENT_V_DATA();
            using (DaStock da = new DaStock())
            {
                da.LoadStockStatsVData(data.AISTOCK_STATS_PERCENT_V, date);
            }
            return data;
        }

        public DataTable GetLeaderStock(string date)
        {
            DataTable data = new DataTable();
            using (DaStock da = new DaStock())
            {
                da.LoadLeaderStock(data, date);
            }
            return data;
        }

        public AISTOCK_TEST_DATA Test()
        {
            AISTOCK_TEST_DATA data = new AISTOCK_TEST_DATA();
            using (DaStock da = new DaStock())
            {
                da.Test(data.AISTOCK_TEST);
            }
            return data;
        }
    }
}
