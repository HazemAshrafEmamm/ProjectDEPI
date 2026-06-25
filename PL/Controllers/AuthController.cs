
using DAL.Models;
using DAL.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PL.Models;
using PL.Models.IdentityViewModels;
using PL.Utilites;
using System.Threading.Tasks;

namespace PL.Controllers
{
    public class AuthController( SignInManager<ApplicationUser> signInManager,
                                 UserManager<ApplicationUser> userManager) : Controller
    {
        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is not null)
            {
                var isCorrectPass = await userManager.CheckPasswordAsync(user, model.Password);
                if (isCorrectPass)
                {
                    var res = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (res.IsNotAllowed) ModelState.AddModelError(string.Empty, "You are not allowed to login.");
                    if (res.IsLockedOut) ModelState.AddModelError(string.Empty, "Your Account is Locked.");
                    if (res.Succeeded) return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login.");
            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var existingUser = await userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "the email already exist");
                return View(model);
            }
            var user = new ApplicationUser
            {
                 Fullname = model.Fullname,
                UserName = model.UserName,
                Email = model.Email
            };
            var res = await userManager.CreateAsync(user, model.Password);
            if (res.Succeeded) return RedirectToAction("Login");
            else
            {
                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }
        #endregion

        #region ForgetPassword
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordLink(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {

                    //create reset password link
                    // base url/Auth/ResetPasswordLink?email=...&token=...
                    // action name,  controller name, route values (object=>Email,token), protocol(http/https) or Request.scheme(baseURL) 
                    var Token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var ResetPasswordLink = Url.Action("ResetPasswordLink", "Auth", new { email = model.Email, token = Token }, Request.Scheme);

                    //create email 
                    var mail = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = ResetPasswordLink
                    };

                    //send email 
                    var res = EmailSettings.SendEmail(mail);
                    if (res)
                    {
                        return RedirectToAction("CheckYourInbox");
                    }

                }
            }

            ModelState.AddModelError(string.Empty, "Invalid Email Address");
            return View(nameof(ForgetPassword), model);
        }
        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordLink(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordLink(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                var user = await userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var res = await userManager.ResetPasswordAsync(user, token, model.Password);
                    if (res.Succeeded) { return RedirectToAction(nameof(Login)); }
                    else
                    {
                        foreach (var error in res.Errors)
                            ModelState.AddModelError(string.Empty ,error.Description);
                    }

                }
            }
            ModelState.AddModelError(string.Empty, "Invalid Request");
            return View();
        }
        #endregion


    }
}
