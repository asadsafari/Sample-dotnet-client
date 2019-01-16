using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNET_OIDC.Models
{

    public class AccountModel
    {
        public string accountNumber { get; set; }
        public object accountType { get; set; }
        public bool hasBlockedAmount { get; set; }
        public bool hasSpecialRule { get; set; }
        public int availableBalance { get; set; }
    }

    public class AccountResponse
    {
        public List<AccountModel> accounts { get; set; }
    }

    public class AccountObject
    {
        public object error { get; set; }
        public AccountResponse response { get; set; }
    }
}