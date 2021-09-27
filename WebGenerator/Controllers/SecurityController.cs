using System;
using System.Threading.Tasks;
using F3R4L.DevPack.Eve.SSO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebGenerator.Models;

namespace WebGenerator.Controllers
{
    public class SecurityController : Controller
    {
        private readonly IEveOnlineSSOService _eveOnlineSSOService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SSOSettings _ssoSettings;

        public SecurityController(IEveOnlineSSOService eveOnlineSSOService, IHttpContextAccessor httpContextAccessor, SSOSettings ssoSettings)
        {
            _eveOnlineSSOService = eveOnlineSSOService;
            _httpContextAccessor = httpContextAccessor;
            _ssoSettings = ssoSettings;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MemberSSOReturn()
        {
            try
            {
                var host = _httpContextAccessor.HttpContext.Request.Host.Value;
                var route = _httpContextAccessor.HttpContext.Request.Path.Value;
                var qString = _httpContextAccessor.HttpContext.Request.QueryString.Value;
                var refreshToken = _eveOnlineSSOService.GetRefreshTokenFromReturnUri(new Uri(string.Concat("http://", host, "/", route, qString)));
                var tokens = await _eveOnlineSSOService.GetTokensFromRefreshTokenAsync(_ssoSettings.ClientId, _ssoSettings.ApplicationKey, refreshToken);

                ViewBag.RefreshToken = refreshToken;
                ViewBag.TokenResponse = tokens;

                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
