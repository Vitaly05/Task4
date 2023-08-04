using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Task4.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext context;

        public HomeController(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Users = await context.Users.ToListAsync();
            return View();
        }
    }
}