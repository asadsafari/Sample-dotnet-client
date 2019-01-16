using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNET_OIDC.Models
{
    public class CustomerAccount
    {
        public Object error { get; set; }
        public List<Account> response { get; set; }
    }
}