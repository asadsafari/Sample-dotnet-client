using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ASPNET_OIDC.Models;
using Newtonsoft.Json;

namespace ASPNET_OIDC.Controllers
{
    public class TransactionController : Controller
    {
        public readonly string ClientId = WebConfigurationManager.AppSettings["clientId"];
        // GET: Transaction
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Transaction(string id)
        {
            var token = Session["token"];
            if (token == null)
                return RedirectToAction("SignIn", "Account");
            var transactionList = GetTransactions((JwtSecurityToken)token, id);
            return View(transactionList);
        }

        private RootObject GetTransactions(JwtSecurityToken token, string accountNumber)
        {
            var request =
                            (HttpWebRequest)
                            WebRequest.Create(WebConfigurationManager.AppSettings["transactionServiceUrl"]);
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + token.RawData);
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string jsonn = new JavaScriptSerializer().Serialize(new
                {
                    accountNumber = accountNumber,
                    dateRange = (new { fromDateTime = "2018-03-22T15:58:01.000Z", toDateTime = "2018-07-25T16:01:01.000Z" }),
                    pageable = (new { page = 0, size = 100 })
                });

                streamWriter.Write(jsonn);
            }

            var responsee = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(responsee.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var oMycustomclassname = JsonConvert.DeserializeObject<RootObject>(result);
                return oMycustomclassname;
            }
        }
    }
}