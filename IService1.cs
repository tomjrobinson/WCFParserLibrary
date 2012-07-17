using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace VolaCalcService
{
    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class StockInfo
    {
        [DataMember]
        public string StockSymbol;
        [DataMember]
        public string DateType;
    }
}
