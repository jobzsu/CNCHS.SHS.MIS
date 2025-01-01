using MediatR;
using Microsoft.AspNetCore.Mvc;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.Web.Controllers;

public class SubjectsController : Controller
{
    private readonly ISender _sender;

    public SubjectsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Route("/Subjects/{id}")]
    public IActionResult Index(int id)
    {
        try
        {
            SubjectViewModel subjectViewModel = new SubjectViewModel();

            return PartialView(subjectViewModel);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}
