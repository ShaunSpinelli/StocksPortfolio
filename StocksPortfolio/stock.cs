using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksPortfolio
{ // contains all relevant information on particular stock,
    class Stock 
    {
        string _stockName;// name of stock
        string _ticker;// ticker off stock, used to retrive information from yahho finance
        int _position; // amount of shares ownwed
        double _purchasePrice;//price paid for the shares
        double _currentPrice;// upadted each time program is opened
        

        public string StockName
        {
            get
            {
                return _stockName.Trim(new Char[] { '"' });
            }
            set
            {
                _stockName = value;
            }
        }

        public string Ticker
        {
            get
            {
                return _ticker.ToUpper();
            }
            set
            {
                _ticker = value;
            }
        }

        public int Positon
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }
        public double PurchasePrice
        {
            get
            {
                return _purchasePrice;
            }
            set
            {
                _purchasePrice = value;
            }
        }

        public double CurrentPrice
        {
            get
            {
                return _currentPrice;
            }
            set
            {
                _currentPrice =value;
            }
        }

       

    }
}
