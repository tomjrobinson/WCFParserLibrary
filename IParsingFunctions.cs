using System;
using System.ServiceModel;


namespace VolaCalcService
{
    [ServiceContract]
    interface IParsingFunctions
    {
        [OperationContract]
        object[,] GetURLData(StockInfo Info);
    }
}
