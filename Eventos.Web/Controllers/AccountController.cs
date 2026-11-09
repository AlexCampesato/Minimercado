using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eventos.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (remoteError != null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }

            var emailClaim = result.Principal.FindFirst(ClaimTypes.Email);
            var email = emailClaim?.Value ?? "usuario@invitado.com";
            var nombre = result.Principal.FindFirst(ClaimTypes.Name)?.Value ?? email;

            //  ACA DEFINIMOS LOS ROLES:
            // Se debe modificar el correo de Google para ser el Administrador
            string rolAsignado = "Cliente"; // Por defecto todos son clientes

            if (email == "campesatoalexis@gmail.com") // <--- MAIL DEL ADMINISTRADOR
            {
                rolAsignado = "Administrador";
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, nombre),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, rolAsignado) 
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return LocalRedirect(returnUrl ?? Url.Content("~/Productos/Index"));
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}