using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using AISRS.Common.Entity;
using AISRS.Common.Framework;

namespace AISRS.WebUI
{
    public class ParseStock
    {
        public StockInfo Parse(string content)
        {
            int start = content.IndexOf('"') + 1;
            int end = content.IndexOf('"', start);
            string input = content.Substring(start, end - start);
            string[] temp = input.Split(',');
            if (temp.Length != 32)
            {
                return null;
            }
            StockInfo info = new StockInfo();
            info.Name = temp[0];
            info.TodayOpen = decimal.Parse(temp[1]);
            info.YesterdayClose = decimal.Parse(temp[2]);
            info.Current = decimal.Parse(temp[3]);
            info.High = decimal.Parse(temp[4]);
            info.Low = decimal.Parse(temp[5]);
            info.Buy = decimal.Parse(temp[6]);
            info.Sell = decimal.Parse(temp[7]);
            info.VolAmount = int.Parse(temp[8]);
            info.VolMoney = decimal.Parse(temp[9]);
            info.BuyList = new List<GoodsInfo>(5);
            int index = 10;
            for (int i = 0; i < 5; i++)
            {
                GoodsInfo goods = new GoodsInfo();
                goods.State = GoodsState.Buy;
                goods.Amount = int.Parse(temp[index]);
                index++;
                goods.Price = decimal.Parse(temp[index]);
                index++;
                info.BuyList.Add(goods);
            }
            info.SellList = new List<GoodsInfo>(5);

            for (int i = 0; i < 5; i++)
            {
                GoodsInfo goods = new GoodsInfo();
                goods.State = GoodsState.Sell;
                goods.Amount = int.Parse(temp[index]);
                index++;
                goods.Price = decimal.Parse(temp[index]);
                index++;
                info.SellList.Add(goods);
            }
            info.Time = DateTime.Parse(temp[30] + " " + temp[31]);
            return info;
        }

        public List<Stock> ParseStockXml()
        {           
            string filePath = Configuration.DiskRoot + "\\" + @"DataCenter\StockInfo.xml";
            XmlTextReader reader = new XmlTextReader(filePath);
            List<Stock> stocks ;
            Stock stock;
            ArrayList stockNames = new ArrayList();
            ArrayList stockCodes = new ArrayList();
            ArrayList stockers = new ArrayList();
            ArrayList stockFields = new ArrayList();
            ArrayList stockCategorys = new ArrayList();
            ArrayList stockMarkets = new ArrayList();
            ArrayList provinces = new ArrayList();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("StockCode"))
                {
                    reader.Read();
                    stockCodes.Add(reader.Value);
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("StockName"))
                {
                    reader.Read();
                    stockNames.Add(reader.Value);                    
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("Stocker"))
                {
                    reader.Read();
                    stockers.Add(reader.Value);
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("StockField"))
                {
                    reader.Read();
                    stockFields.Add(reader.Value);
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("StockCategory"))
                {
                    reader.Read();
                    stockCategorys.Add(reader.Value);
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("StockMarket"))
                {
                    reader.Read();
                    stockMarkets.Add(reader.Value);
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("Province"))
                {
                    reader.Read();
                    provinces.Add(reader.Value);
                }
            }
            if (stockNames != null && stockNames.Count > 0)
            {
                stocks = new List<Stock>();
                for (int i = 0; i < stockNames.Count; i++)
                {
                    stock = new Stock();
                    stock.StockCode = stockCodes[i].ToString();
                    stock.StockName = stockNames[i].ToString();
                    stock.Stocker = stockers[i].ToString();
                    stock.StockField = stockFields[i].ToString();
                    stock.StockCategory = stockCategorys[i].ToString();
                    stock.StockMarket = stockMarkets[i].ToString();
                    stock.Province = provinces[i].ToString();
                    stocks.Add(stock);
                }
                return stocks;
            }
            return null;
        }
    }
}