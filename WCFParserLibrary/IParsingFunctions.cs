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
        void GetURLData(string StockName, string Type);
    }
}
