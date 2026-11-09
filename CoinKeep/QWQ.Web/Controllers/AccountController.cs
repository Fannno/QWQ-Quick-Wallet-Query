using CoinKeep.Web.BLL;
using CoinKeep.Web.Models;
using CoinKeep.Web.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace CoinKeep.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountBLL accountBLL;
        public AccountController(IAccountBLL accountBLL) { this.accountBLL = accountBLL; }

        /// <summary>
        /// 顯示登入頁面
        /// </summary>
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /// <summary>
        /// 處理使用者登入資料
        /// </summary>
        /// <param name="loginVM">登入表單資料</param>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            User? user = accountBLL.Login(loginVM);

            if (user == null)
            {
                ModelState.AddModelError("", "電子郵件或密碼錯誤");
                return View(loginVM);
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            ClaimsIdentity identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            AuthenticationProperties authProperties = new AuthenticationProperties
            {
                IsPersistent = loginVM.RememberMe
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties
            );

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 使用者登出
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction("Login");
        }

        /// <summary>
        /// 顯示註冊頁面
        /// </summary>
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /// <summary>
        /// 處理使用者註冊資料
        /// </summary>
        /// <param name="registerVM">註冊表單資料</param>
        [HttpPost]
        public IActionResult Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
           
            string result = accountBLL.Register(registerVM);
            if (!string.IsNullOrEmpty(result))
            {
                ModelState.AddModelError("Email", result);
                return View(registerVM);
            }
            return RedirectToAction("Login");
        }
    }
}
