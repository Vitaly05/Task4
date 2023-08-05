﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Task4.Models;

namespace Task4.Controllers
{
    [Route("")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.Users = await userManager.Users.ToListAsync();
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("registration")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost("singUp")]
        public async Task<IActionResult> SignUp(RegistrationModel model)
        {
            IActionResult result = View("Registration", model);
            if (ModelState.IsValid)
            {
                var user = new User { Name = model.Name,Email = model.Email, UserName = model.Email, RegisterDate = DateTime.Now };
                await registerUser(user, model.Password, onSuccess: () => result = Redirect("/"));
            }
            return result;
        }

        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn(LoginModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
                if (result.Succeeded) return Redirect(ReturnUrl ?? "/");
                else ModelState.AddModelError(string.Empty, "Incorrect email or password");
            }
            return View("Login", model);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }

        private async Task registerUser(User user, string password, Action onSuccess)
        {
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
                onSuccess.Invoke();
            }
            else addModelErrors(result.Errors);
        }

        private void addModelErrors(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
                    ModelState.AddModelError(String.Empty, Regex.Replace(error.Description, "Username", "Email"));
        }
    }
}