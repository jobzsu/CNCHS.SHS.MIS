using MediatR;
using Microsoft.AspNetCore.Mvc;
using SHS.StudentPortal.App.Commands;
using SHS.StudentPortal.App.Queries;
using SHS.StudentPortal.Common.Models;

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

    [HttpGet]
    [Route("/Instructors/New")]
    public IActionResult New()
    {
        try
        {
            var query = new GetInstructorViewModelForCreation();

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

    [HttpPost]
    [Route("/Instructors/New")]
    public IActionResult New(CreateInstructorViewModel model)
    {
        try
        {
            var createdById = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

            var command = new CreateInstructorCommand(Guid.Parse(createdById), model);

            var result = Task.Run(() => _sender.Send(command)).Result;

            return result.IsSuccess ?
                    new JsonResult(JsonResponseModel.Success(null, null)) :
                    new JsonResult(JsonResponseModel.Error(result.Error!.Message));
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }
}
