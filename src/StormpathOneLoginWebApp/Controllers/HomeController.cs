using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stormpath.SDK.Application;

namespace StormpathOneLoginWebApp.Controllers
{
    public class HomeController : Controller
    {
        private IApplication _stormpathApp;

        public HomeController(IApplication stormpathApp)
        {
            _stormpathApp = stormpathApp;
        }

        public ActionResult Index()
        {
            return View();
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task<ActionResult> LoginViaSaml()
        {
            var app = await _stormpathApp.Client.GetApplicationAsync("https://api.stormpath.com/v1/applications/3kmxAP6YTdzAqEXD1e9UN3"); // Your Stormpath Application href

            var samlUrlBuilder = await app.NewSamlIdpUrlBuilderAsync();
            var redirectUrl = samlUrlBuilder
                .SetCallbackUri("http://localhost:49980/loginredirect") // The URL to your callback controller, see below
                .Build();

            HttpContext.Response.Headers.Add("Cache-control", "no-cache, no-store");
            HttpContext.Response.Headers.Add("Pragma", "no-cache");
            HttpContext.Response.Headers.Add("Expires", "-1");

            return Redirect(redirectUrl);
        }


    }
}
