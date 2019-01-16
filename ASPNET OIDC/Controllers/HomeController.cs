using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ASPNET_OIDC.Models;
using Newtonsoft.Json;

namespace ASPNET_OIDC.Controllers
{
    public class HomeController : Controller
    {
        public readonly string ClientId = WebConfigurationManager.AppSettings["clientId"];
        public ActionResult Index()
        {
            return View();
        }

        

        public ActionResult Accounts()
        {
            var token = Session["token"];
            var ssn = Session["ssn"];
            if (token == null)
                return RedirectToAction("SignIn", "Account");
            var transactionList = Accounts((JwtSecurityToken)token, ssn.ToString());
            return View(transactionList);
        }

       

        

        private AccountObject Accounts(JwtSecurityToken token, string ssn)
        {
            var request =
                            (HttpWebRequest)
                            WebRequest.Create(WebConfigurationManager.AppSettings["accountServiceUrl"] + "?key=" + ClientId);
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + token.RawData);
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string jsonn = new JavaScriptSerializer().Serialize(new
                {
                    nationalIdentifier = ssn
                });

                streamWriter.Write(jsonn);
            }

            var responsee = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(responsee.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var oMycustomclassname = JsonConvert.DeserializeObject<AccountObject>(result);
                return oMycustomclassname;
            }
        }
    }
}