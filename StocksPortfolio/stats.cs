using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksPortfolio
{
    class Stats
    {
        Stock stock;

        public Stats(Stock stk)
        {
            stock = stk;
        }
        public double assestWorth()
        {
            return stock.Positon * stock.CurrentPrice;
        }

        public double assestChangeNum()
        {
            return (stock.Positon * stock.CurrentPrice) - (stock.Positon * stock.PurchasePrice);

        }

        public double assestChangePercent()
        {
            return (((stock.Positon * stock.CurrentPrice) - (stock.Positon * stock.PurchasePrice)) / (stock.Positon * stock.PurchasePrice)) * 100;

        }

    }
}
