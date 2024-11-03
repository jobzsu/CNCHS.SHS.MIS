using Microsoft.AspNetCore.Mvc;

namespace SHS.StudentPortal.Web.Controllers
{
    public class AccessDeniedController : Controller
    {
        [Route("/AccessDenied")]
        public IActionResult Index()
        {
            return View("AccessDenied");
        }
    }
}
