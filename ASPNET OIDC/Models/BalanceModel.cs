using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNET_OIDC.Models
{
    public class BalanceResponse
    {
        public string accountNumber { get; set; }
        public string effectiveBalance { get; set; }
        public string currentBalance { get; set; }
        public string usableBalance { get; set; }
    }

    public class BalanceObject
    {
        public object error { get; set; }
        public BalanceResponse response { get; set; }
    }
}