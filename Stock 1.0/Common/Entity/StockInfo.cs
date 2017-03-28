using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISRS.Common.Entity
{
    public class StockInfo
    {
        public string Name
        {
            get;
            set;
        }

        public decimal TodayOpen
        {
            get;
            set;
        }

        public decimal YesterdayClose
        {
            get;
            set;
        }

        public decimal Current
        {
            get;
            set;
        }

        public decimal High
        {
            get;
            set;
        }

        public decimal Low
        { 
            get; 
            set; 
        }

        /// <summary>
        /// 竟买价 买1
        /// </summary>
        public decimal Buy
        { get; set; }

        /// <summary>
        /// 竟卖价 卖1
        /// </summary>
        public decimal Sell { get; set; }

        /// <summary>
        /// 成交数 单位股数 通常除于100成为手
        /// </summary>
        public int VolAmount { get; set; }

        /// <summary>
        /// 成交多少钱,单位元
        /// </summary>
        public decimal VolMoney { get; set; }

        /// <summary>
        /// 新浪是可以看到5个,5档看盘 ,买1-买5
        /// </summary>
        public List<GoodsInfo> BuyList { get; set; }

        /// <summary>
        /// 卖1－卖5
        /// </summary>
        public List<GoodsInfo> SellList { get; set; }

        /// <summary>
        /// Date and Time
        /// </summary>
        public DateTime Time { get; set; }

        public override string ToString()
        {
            return Name + ": " + VolAmount + ":" + Current;
        }
    }

    public class GoodsInfo
    {
        public int Amount
        { get; set; }
        public decimal Price
        {
            get;
            set;
        }
        public int State { get; set; }
    }

    public class GoodsState
    {
        public static int Buy = 0;

        public static int Sell = 1;        
    }
}
