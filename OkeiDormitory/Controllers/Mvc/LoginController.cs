using Microsoft.AspNetCore.Mvc;

namespace OkeiDormitory.Controllers.Mvc
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LoginController : Controller
    {
        [Route("login")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
