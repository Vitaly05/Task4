using Microsoft.AspNetCore.Mvc;

namespace Task4.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}