using Microsoft.AspNetCore.Mvc;

namespace OkeiDormitory.Controllers.Mvc
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
