using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.ServiceModel;

namespace VolaCalcService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ParsingFunctions : VolaCalcService.IParsingFunctions
    {
        //http://chart.yahoo.com/table.csv?a=BEGIN_MONTH&b=BEGIN_DAY&c=BEGIN_YEAR&d=END_MONTH&e=END_DAY&f=END_YEAR&s=SYMBOL&y=FORMAT&g=TYPE
        public const string gYAHOO_HIS_URL = "http://chart.yahoo.com/table.csv?a={0}&b={1}&c={2}&d={3}&e={4}&f={5}&s={6}&y={7}&g={8}";
        
        
        public object[,] GetURLData(StockInfo Info)
        {
            int iTickerNum = 1;
            string[] sTempURL = new string[iTickerNum], sHTMLFileHisDay = new string[iTickerNum];
            int iStartDay, iStartYear, iStartMonth, iEndDay, iEndYear, iEndMonth;

            

            iEndDay = DateTime.Now.Day;

            //This actually sets it to this month, not last month.
            iEndMonth = DateTime.Now.Month - 1;
            iEndYear = DateTime.Now.Year;

            iStartDay = 1;
            //Sets it to January.
            iStartMonth = 0;
            iStartYear = 1990;

            for (int i = 0; i < iTickerNum; i++)
            {
                sTempURL[i] = string.Format(gYAHOO_HIS_URL, iStartMonth, iStartDay, iStartYear, iEndMonth, iEndDay, iEndYear, Info.StockSymbol, "0", Info.DateType);
                sHTMLFileHisDay[i] = GetURLSource(sTempURL[i]);
            }

            if (sHTMLFileHisDay[0].IndexOf("Not Found") > 0)
            {
                throw new ArgumentException("Could not retrieve data");
            }
            else
            {
                object[][,] objDay = new object[sHTMLFileHisDay.Length][,];


                for (int i = 0; i < sHTMLFileHisDay.Length; i++)
                {
                    objDay[i] = SplitData(sHTMLFileHisDay[i]);
                    objDay[i] = CleanHisDataArr(objDay[i]);
                }

                return objDay[0];
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

        //ByVal vInput As Variant, ByRef lrows As Long, ByRef lcols As Long
        protected object[,] CleanHisDataArr(object[,] objInput)
        {
            object[,] objResult = new object[objInput.GetLength(0)-1, 2];

            for (int i = 0; i < objResult.GetLength(0); i++)
            {
                objResult[i, 0] = objInput[i+1, 0];
                objResult[i, 1] = objInput[i+1, 6];
            }
            return objResult;
        }

    }
}