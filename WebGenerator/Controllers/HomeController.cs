using System.Collections.Generic;
using System.Diagnostics;
using F3R4L.DevPack.Eve.SSO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebGenerator.Models;

namespace WebGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEveOnlineSSOService _eveOnlineSSOService;
        private readonly SSOSettings _ssoSettings;

        public HomeController(ILogger<HomeController> logger, IEveOnlineSSOService eveOnlineSSOService, SSOSettings ssoSettings)
        {
            _logger = logger;
            _eveOnlineSSOService = eveOnlineSSOService;
            _ssoSettings = ssoSettings;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return new RedirectResult(_eveOnlineSSOService.SignOnRedirectUrl(_ssoSettings.CallbackUrl, _ssoSettings.ClientId, new List<string>()));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
