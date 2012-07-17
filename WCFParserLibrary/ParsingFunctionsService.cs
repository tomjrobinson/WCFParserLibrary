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

        public void GetURLData(string StockName, string Type)
        {
            YahooFinanceDAO yahooDAO = new YahooFinanceDAO(StockName, Type);

            AdjCloseList = yahooDAO.GetAdjCloseData();
        }
    }
}
