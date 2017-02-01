using Microsoft.AspNetCore.Mvc;
using Stormpath.SDK.Application;
using Stormpath.SDK.Http;
using Stormpath.SDK.Jwt;
using Stormpath.SDK.Saml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StormpathOneLoginWebApp.Controllers
{
    public class LoginRedirectController : Controller
    {
        private IApplication _stormpathApp;

        public LoginRedirectController(IApplication stormpathApp)
        {
            _stormpathApp = stormpathApp;
        }


        public async Task<ActionResult> Index()
        {
            var app = await _stormpathApp.Client.GetApplicationAsync("https://api.stormpath.com/v1/applications/3kmxAP6YTdzAqEXD1e9UN3"); // Your Stormpath Application href

            var incomingRequest = HttpRequests.NewRequestDescriptor()
                .WithMethod(Request.Method)
                .WithUri(Microsoft.AspNetCore.Http.Extensions.UriHelper.GetEncodedUrl(Request))
                .Build();

            var samlHandler = app.NewSamlAsyncCallbackHandler(incomingRequest);

            try
            {
                var accountResult = await samlHandler.GetAccountResultAsync();
                var account = await accountResult.GetAccountAsync();
                var a = account.Email;
                // Success! Do something with the account details
            }
            catch (InvalidJwtException ije)
            {
                // JWT validation failed (see Message property for details)
            }
            catch (SamlException se)
            {
                // SAML exchange failed (see Message property for details)
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
