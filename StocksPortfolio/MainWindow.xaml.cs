using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net;


namespace StocksPortfolio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Read() must be run on start up so that array can be made containing all current stocks, which allows the Prices to be updated (UpdatePrice())
            // and stocks to be displayed(Display())
            InitializeComponent();             
            Read();
            UpdatePrice();
            Display();
            
        }
        // Get a web response.
        public string GetWebResponse(string url) 
        {
            // Found this method online 
            // Make a WebClient.
            WebClient web_client = new WebClient();

            // Get the indicated URL.
            Stream response = web_client.OpenRead(url);

            // Read the result.
            using (StreamReader stream_reader = new StreamReader(response))
            {
                // Get the results.
                string result = stream_reader.ReadToEnd();

                // Close the stream reader and its underlying stream.
                stream_reader.Close();

                // Return the result.
                return result;
            }
        }

        private Stock[] portfolio = new Stock[0];

        private void Write(Stock obj)
        {
            // Writes stock information to text file
            StreamWriter sw = new StreamWriter("PortfolioData.txt");
            sw.WriteLine(portfolio.Length + 1);// portfolio is array containing stock information
            sw.WriteLine(obj.StockName);
            sw.WriteLine(obj.Ticker);
            sw.WriteLine(obj.Positon);
            sw.WriteLine(obj.PurchasePrice);
            sw.WriteLine(obj.CurrentPrice);
            

            
            for (int i = 0; i < portfolio.Length; i++)
            {
                sw.WriteLine(portfolio[i].StockName);
                sw.WriteLine(portfolio[i].Ticker);
                sw.WriteLine(portfolio[i].Positon);
                sw.WriteLine(portfolio[i].PurchasePrice);
                sw.WriteLine(portfolio[i].CurrentPrice);

            }

            sw.Close();
        }

        private void Read()
        {
            // creates array from text document containing stock information for each stock
            StreamReader sr = new StreamReader("PortfolioData.txt");
            portfolio = new Stock[Convert.ToInt32(sr.ReadLine())];
            
            for (int i = 0; i < portfolio.Length; i++)
            {
                portfolio[i] = new Stock();
                portfolio[i].StockName = sr.ReadLine();
                portfolio[i].Ticker = sr.ReadLine();
                portfolio[i].Positon = Convert.ToInt32(sr.ReadLine());
                portfolio[i].PurchasePrice = Convert.ToDouble(sr.ReadLine());
                portfolio[i].CurrentPrice = Convert.ToDouble(sr.ReadLine());
            }

            sr.Close();
        }

        private void ClearForm()
        {
            // clears text in the form
            ticker.Text = String.Empty;
            position.Text = String.Empty;
            price.Text = String.Empty;      

        }

        private void UpdatePrice()
        {
            ///<summary>
            ///update current price for all stocks in portfolio 
            /// </summary>
            /// 

            string ticks = "";
            //create ticker url
            for (int i = 0; i < portfolio.Length; i++)
            {
                ticks += String.Format("{0}+", portfolio[i].Ticker);
            }
           
            string url = "http://download.finance.yahoo.com/d/quotes.csv?s=" + ticks + "&f=l1";
           
            // get updated prices using web response
            string[] updatedPrice = GetWebResponse(url).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            

            //update prices of stocks

            for (int i = 0; i < portfolio.Length; i++)
            {
                portfolio[i].CurrentPrice = Convert.ToDouble(updatedPrice[i]); 
            }
        }

        private void Display()
        {   // shows stock names in list box
            portfoliolist.Items.Clear();

            for (int i = 0; i < portfolio.Length; i++)
            {
                portfoliolist.Items.Add(portfolio[i].StockName);
            }

        }
        
        private void DisplayStock(string stockName)             
        {
            // displays information on single stock       
            string x;
            for (int i = 0; i < portfolio.Length; i++)
            {

                if (stockName == portfolio[i].StockName)
                {
                    x = String.Format("{0} ({1})\n", portfolio[i].StockName, portfolio[i].Ticker);
                    x += String.Format("Shares Owned: {0}\n", portfolio[i].Positon);
                    x += String.Format("Purchase Price: ${0}\n", portfolio[i].PurchasePrice);
                    x += String.Format("Current Price: ${0}\n", portfolio[i].CurrentPrice);
                    x += String.Format("Assest Worth: ${0}\n", portfolio[i].Positon * portfolio[i].CurrentPrice);
                    x += String.Format("Change: ${0}  ", (portfolio[i].Positon * portfolio[i].CurrentPrice) - (portfolio[i].Positon * portfolio[i].PurchasePrice));
                    x += String.Format("  {0}%\n", (((portfolio[i].Positon * portfolio[i].CurrentPrice) - (portfolio[i].Positon * portfolio[i].PurchasePrice)) / (portfolio[i].Positon * portfolio[i].PurchasePrice)) * 100);
                    stockinfo.Text = x;                    
                }

              
            }

            
        }

        private void RemoveStock(string curItem)
        {
            // deletes selected stock from portfolio
            StreamWriter sw = new StreamWriter("PortfolioData.txt");
            sw.WriteLine(portfolio.Length - 1);

            for (int i = 0; i < portfolio.Length; i++)
            {

                if (curItem != portfolio[i].StockName)
                {
                    sw.WriteLine(portfolio[i].StockName);
                    sw.WriteLine(portfolio[i].Ticker);
                    sw.WriteLine(portfolio[i].Positon);
                    sw.WriteLine(portfolio[i].PurchasePrice);
                    sw.WriteLine(portfolio[i].CurrentPrice);
                }

            }

            sw.Close();
            Read();
            Display();
            stockinfo.Text = null;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // Action for "ADD STOCK" button.
            try
            {   // creates new stock object and updates Ticker, Postion and Purchase Price from textbox's on form
                Stock obj = new Stock();
                obj.Ticker = ticker.Text;
                obj.Positon = Convert.ToInt32(position.Text);
                obj.PurchasePrice = Convert.ToDouble(price.Text);
                
                // provides csv file containing name and last trade price
                string url = "http://download.finance.yahoo.com/d/quotes.csv?s=" + obj.Ticker + "&f=nl1";
                
                // processes csv file into array
                string[] webInfo = GetWebResponse(url).Split(',');

                // StockName and CurrentPrice updated to obj from downloaded information
                obj.StockName = webInfo[0];
                obj.CurrentPrice = Convert.ToDouble(webInfo[1]);

                Write(obj);
                Read();
                Display();
                ClearForm();

            }
            catch (Exception)
            {
                // if exception is caught error message is displyed and form is cleared
                MessageBox.Show("Error: Make sure Ticker is Correct and Position and Purchase Price are numbers");
                ClearForm();

            }
            
        

        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the currently selected item in the ListBox.
            // check to see if feild is null incase it has been deleted
            if (portfoliolist.SelectedItem != null)
            {
                string curItem = portfoliolist.SelectedItem.ToString(); 
                DisplayStock(curItem);
            }
            
            
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string curItem = portfoliolist.SelectedItem.ToString();
                RemoveStock(curItem);
            }
            catch (Exception)
            {
                MessageBox.Show("No Stock Selected");               
            }
            
        }

        
            
            
        
    }
}
