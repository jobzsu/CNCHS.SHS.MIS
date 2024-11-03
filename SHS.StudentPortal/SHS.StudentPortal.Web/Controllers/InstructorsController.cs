using MediatR;
using Microsoft.AspNetCore.Mvc;
using SHS.StudentPortal.App.Queries;

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
            var query = new GetInstructorViewModel(id);

            var result = Task.Run(() => _sender.Send(query)).Result;

            if (result.IsSuccess)
            {
                return PartialView(result.Data);
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }
}
