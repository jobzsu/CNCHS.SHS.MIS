using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SHS.StudentPortal.Web.Controllers;

public class InstructorsController : Controller
{
    private readonly ISender _sender;

    public InstructorsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Route("/Instructors/{id}")]
    public IActionResult Index(Guid id)
    {
        try
        {

            return PartialView();
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }
}
