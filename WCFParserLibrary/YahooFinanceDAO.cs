using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.ServiceModel;

namespace VolaCalcService
{
    
    public class YahooFinanceDAO
    {
        public const string gYAHOO_HIS_URL = "http://chart.yahoo.com/table.csv?a={0}&b={1}&c={2}&d={3}&e={4}&f={5}&s={6}&y={7}&g={8}";
        StockInfo thisStock;
        int stockType;
        DateTime Start, End;

        public YahooFinanceDAO(string StockName, string Type, DateTime StartDate, DateTime EndDate)
        {
            thisStock.StockSymbol = StockName;
            thisStock.DateType = Type;
            stockType = 6;
            Start = StartDate;
            End = EndDate;
            
        }

        public List<StockData> GetAdjCloseData()
        {
            string sTempURL, sHTMLFileHisDay;
            int iStartDay, iStartYear, iStartMonth, iEndDay, iEndYear, iEndMonth;

            iEndDay = End.Day;
            //This actually sets it to this month, not last month.
            iEndMonth = End.Month - 1;
            iEndYear = End.Year;

            iStartDay = Start.Day;
            //Sets it to January.
            iStartMonth = Start.Month-1;
            iStartYear = Start.Year;

            sTempURL = string.Format(gYAHOO_HIS_URL, iStartMonth, iStartDay, iStartYear, iEndMonth, iEndDay, iEndYear, thisStock.StockSymbol, "0", thisStock.DateType);
            sHTMLFileHisDay = GetURLSource(sTempURL);

            if (sHTMLFileHisDay.IndexOf("Not Found") > 0)
            {
                throw new ArgumentException("Could not retrieve data");
            }
            else
            {
                object[,] objDay;

                objDay = SplitData(sHTMLFileHisDay);
                return GetStockDataList(objDay, stockType);
            }
        }

        protected string GetURLSource(string sURL)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            try
            {
                byte[] raw = wc.DownloadData(sURL);
                string webData = System.Text.Encoding.UTF8.GetString(raw);
                webData = System.Text.RegularExpressions.Regex.Replace(webData, "\\r\\n", "");
                return webData;
            }
            catch
            {
                throw new ArgumentException("An error occured while trying to retrieve data.");
            }

        }

        protected object[,] SplitData(string sData)
        {
            object[] objInterOne, objInterTwo;
            object[,] objResult;
            int intDimX, intDimY;

            objInterOne = Regex.Split(sData, "\\n");
            intDimX = objInterOne.Length - 2;

            objInterTwo = Regex.Split(objInterOne[0].ToString(), ",");
            intDimY = objInterTwo.Length;

            objResult = new object[intDimX, intDimY];
            for (int i = 0; i < intDimX; i++)
            {
                objInterTwo = Regex.Split(objInterOne[i].ToString(), ",");
                for (int j = 0; j < intDimY; j++)
                {
                    if (j > objInterTwo.Length)
                    {
                        objResult[i, j] = "";
                    }
                    else
                    {
                        objResult[i, j] = objInterTwo[j];
                    }
                }
            }
            return objResult;
        }

        protected List<StockData> GetStockDataList(object[,] objInput, int Column)
        {
            List<StockData> List = new List<StockData>();
            StockData Item;

            for (int i = 0; i < objInput.GetLength(0) - 1; i++)
            {
                Item.QuoteDate = Convert.ToDateTime(objInput[i + 1, 0]).ToOADate();
                Item.AdjClose = Convert.ToDouble(objInput[i + 1, Column]);

                List.Add(Item);
            }

            return List;
        }

    }
}