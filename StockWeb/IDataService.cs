using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISRS.Common.Entity;

namespace AISRS.WebUI
{
    public interface IDataService
    {
        StockInfo GetCurrent(string stockCode); 
    }
}
