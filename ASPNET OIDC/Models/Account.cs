using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNET_OIDC.Models
{
    public class Account
    {

        public String accountNumber { get; set; }
        public String accountType { get; set; }
        public decimal balance { get; set; }
        public Boolean hasBlockedAmount { get; set; }

        public Boolean hasSpecialRule { get; set; }

    }

    public class Content
    {
        public int transactionId { get; set; }
        public string accountNumber { get; set; }
        public DateTime transactionDateTime { get; set; }
        public string branchCode { get; set; }
        public string branchName { get; set; }
        public string traceNumber { get; set; }
        public int transactionAmount { get; set; }
        public object hyperLinkType { get; set; }
        public string debitCredit { get; set; }
        public string transactionDescription { get; set; }
        public string transactionChanel { get; set; }
        public object categoryCode { get; set; }
        public object userComment { get; set; }
    }

    public class Response
    {
        public int totalPages { get; set; }
        public int totalElements { get; set; }
        public int numberOfElements { get; set; }
        public int number { get; set; }
        public int size { get; set; }
        public bool hasContent { get; set; }
        public object sort { get; set; }
        public bool first { get; set; }
        public bool last { get; set; }
        public bool next { get; set; }
        public bool previous { get; set; }
        public List<Content> content { get; set; }
    }

    public class RootObject
    {
        public object error { get; set; }
        public Response response { get; set; }
    }
}