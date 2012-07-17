using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace VolaCalcService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ParsingFunctionsService : IParsingFunctions
    {
        List<StockData> AdjCloseList;

        public List<StockData> GetURLData(string StockName, string Type, DateTime StartDate, DateTime EndDate)
        {
            YahooFinanceDAO yahooDAO = new YahooFinanceDAO(StockName, Type, StartDate, EndDate);

            AdjCloseList = yahooDAO.GetAdjCloseData();

            return AdjCloseList;
        }
    }
}
