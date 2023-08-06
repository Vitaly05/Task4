using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Task4.Data;
using Task4.Models;
using Task4.Repositories;

namespace Task4.Controllers
{
    [Route("")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUsersRepository usersRepository;

        private readonly SignInManager<User> signInManager;

        public AccountController(IUsersRepository usersRepository, SignInManager<User> signInManager)
        {
            this.usersRepository = usersRepository;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await usersRepository.GetUserAsync(HttpContext.User);
            await setIndexViewDataAsync(currentUser);
            return View();
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet("registration")]
        public IActionResult Registration()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("singUp")]
        public async Task<IActionResult> SignUp(RegistrationModel model)
        {
            IActionResult result = View("Registration", model);
            if (ModelState.IsValid)
            {
                var user = new User { Name = model.Name,Email = model.Email, UserName = model.Email, RegisterDate = DateTime.Now };
                await registerUserAsync(user, model.Password, onSuccess: () => result = Redirect("/"));
            }
            return result;
        }

        [AllowAnonymous]
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

        [HttpPost("changeStatus/{status}")]
        public async Task<IActionResult> ChangeStatus([FromBody] List<string> usersIds, AccountStatus status)
        {
            if (usersIds.Count == 0) return BadRequest("Please, select at least one user.");
            foreach (var user in usersRepository.GetUsers(usersIds))
                await usersRepository.ChangeStatus(user, status);
            return Ok();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteUser([FromBody] List<string> usersIds)
        {
            if (usersIds.Count == 0) return BadRequest("Please, select at least one user.");
            foreach (var user in usersRepository.GetUsers(usersIds))
                await usersRepository.DeleteAsync(user);
            return Ok();
        }

        private async Task setIndexViewDataAsync(User currentUser)
        {
            ViewData["UserName"] = currentUser.Name;
            ViewData["Email"] = currentUser.Email;
            ViewBag.Users = await usersRepository.GetAllAsync();
        }

        private async Task registerUserAsync(User user, string password, Action onSuccess)
        {
            var result = await usersRepository.CreateAsync(user, password);
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