using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;


namespace VolaCalcService
{
    [ServiceContract]
    public interface IParsingFunctions
    {
        [OperationContract]
        List<StockData> GetURLData(string StockName, string Type, DateTime StartDate, DateTime EndDate);
    }
}
