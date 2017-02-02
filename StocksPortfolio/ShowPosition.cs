using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksPortfolio
{
    class ShowPosition
    {
        //Stock stck;
        string x;
        public  ShowPosition(Stock stocks)
        {
            //stck = stocks; // is there a way to do this by saying base or this.
            Stats stockstat = new Stats(stocks);

            x = String.Format("Stock:{0}({1})", stocks.StockName, stocks.Ticker);
            x += String.Format("Shares Owned: {0}", stocks.Positon);
            x += String.Format("Purchase Price:${0}", stocks.PurchasePrice);
            x += String.Format("Current Price:${0}", stocks.CurrentPrice);
            x += String.Format("Current Value:{1} stocks at ${2} = ${0}", stockstat.assestWorth(), stocks.Positon, stocks.CurrentPrice);
            x += String.Format("Assest Change:${0} Percent Change {1}%", stockstat.assestChangeNum(), stockstat.assestChangePercent());// **want to round these to two decimal points

            
        }
    }
}
