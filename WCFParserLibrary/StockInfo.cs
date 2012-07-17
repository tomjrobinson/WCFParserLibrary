using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace VolaCalcService
{
    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    //[DataContract]
    //public class StockObject
    //{
    //    [DataMember]
    //    public string StockSymbol;
    //    [DataMember]
    //    public string DateType;
    //}
    [DataContract]
    public struct StockInfo
    {
        [DataMember]
        public string StockSymbol;
        [DataMember]
        public string DateType;
    }
    [DataContract]
    public struct StockData
    {
        [DataMember]
        public double QuoteDate;
        [DataMember]
        public double AdjClose;
    }
}
