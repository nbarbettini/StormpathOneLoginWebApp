using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stormpath.Owin.Abstractions;
using Stormpath.SDK.Application;
using Stormpath.SDK.Client;

namespace StormpathOneLoginWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApplication _stormpathApp;
        private readonly IClient _stormpathClient;

        public HomeController(IApplication stormpathApp, IClient stormpathClient)
        {
            _stormpathApp = stormpathApp;
            _stormpathClient = stormpathClient;
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
            var stateToken = new StateTokenBuilder(_stormpathClient, _stormpathClient.Configuration.Client.ApiKey)
                .ToString();

            var samlUrlBuilder = await _stormpathApp.NewSamlIdpUrlBuilderAsync();
            var redirectUrl = samlUrlBuilder
                .SetCallbackUri("http://localhost:49980/stormpathCallback") // Make sure this points to your web app URI!
                .SetState(stateToken)
                .Build();

            return Redirect(redirectUrl);
        }


    }
}
