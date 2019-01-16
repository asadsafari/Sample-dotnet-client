using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ASPNET_OIDC.Controllers
{

    public class AccountController : Controller
    {
        public readonly string ClientId = WebConfigurationManager.AppSettings["clientId"];
        public readonly string ClientSecret = WebConfigurationManager.AppSettings["clientSecret"];
        public readonly string ClientCallBackUrl = WebConfigurationManager.AppSettings["clientCallbackUrl"];
        public const string ClientUrl = "http://localhost:50764";
        
        public readonly string TokenUrl= WebConfigurationManager.AppSettings["tokenUrl"];

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignIn()
        {
            var state = Guid.NewGuid().ToString("N");
            var nonce = Guid.NewGuid().ToString("N");

            var url = WebConfigurationManager.AppSettings["authUrl"] +
                "?&client_id=" + ClientId +
                "&client_secret=" + ClientSecret +
                "&redirect_uri=" + ClientCallBackUrl +
                "&response_type=code" +
                "&state=" + state +
                "&nonce=" + nonce;
            return Redirect(url);
        }

        public RedirectToRouteResult SignInCallback()
        {
            GetToken();
            return RedirectToAction("Accounts", "Home");
        }
        private JwtSecurityToken GetToken()
        {
            var code = Request.QueryString["code"];

            var cert = new X509Certificate2(Server.MapPath("~/App_Data/cert.pfx"), "1234");
            var signingCredentials = new SigningCredentials(new X509SecurityKey(cert), SecurityAlgorithms.RsaSha256);
            var header = new JwtHeader(signingCredentials);
            var payload = new JwtPayload
            {
                {"iss", ClientId},
                {"sub", ClientSecret},
                {"aud", TokenUrl},
                {"jti", Guid.NewGuid().ToString("N")},
                {"exp", (int) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + 5*60}
            };
            var securityToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            handler.WriteToken(securityToken);

            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["client_id"] = ClientId;
                data["client_secret"] = ClientSecret;
                data["redirect_uri"] = ClientCallBackUrl;
                data["grant_type"] = "authorization_code";
                data["code"] = code;

                var response =
                    wb.UploadValues(
                        TokenUrl, "POST",
                        data);

                var responseString = Encoding.ASCII.GetString(response);
                dynamic tokenResponse = JObject.Parse(responseString);

                var token = handler.ReadToken((String)tokenResponse.access_token) as JwtSecurityToken;
                if (token != null)
                {
                    var ssn = token.Claims.First(c => c.Type == "ssn").Value;
                    Session["ssn"] = ssn;
                    Session["token"] = token;
                }

                return token;
            }
        }

        
    }
}
