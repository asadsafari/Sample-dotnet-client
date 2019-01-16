using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ASPNET_OIDC.Models;
using Newtonsoft.Json;

namespace ASPNET_OIDC.Controllers
{
    public class BalanceController : Controller
    {
        public readonly string ClientId = WebConfigurationManager.AppSettings["clientId"];
        // GET: Balance
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Balance(string id)
        {
            var token = Session["token"];
            if (token == null)
                return RedirectToAction("SignIn", "Account");
            var transactionList = GetBalance((JwtSecurityToken)token, id);
            return View(transactionList);
        }
        private BalanceObject GetBalance(JwtSecurityToken token, string accountNumber)
        {
            var request =
                            (HttpWebRequest)
                            WebRequest.Create(WebConfigurationManager.AppSettings["balanceServiceUrl"] + accountNumber + "?key=" + ClientId);
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + token.RawData);
            request.Method = "GET";

            var responsee = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(responsee.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var oMycustomclassname = JsonConvert.DeserializeObject<BalanceObject>(result);
                return oMycustomclassname;
            }
        }
    }
}