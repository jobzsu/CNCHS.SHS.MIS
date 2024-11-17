using MediatR;
using Microsoft.AspNetCore.Mvc;
using SHS.StudentPortal.App.Commands;
using SHS.StudentPortal.App.Queries;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.Web.Controllers;

public class DepartmentsController : Controller
{
    private readonly ISender _sender;

    public DepartmentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Route("/Departments/{id}")]
    public IActionResult Index(int id)
    {
        try
        {
            var query = new GetDepartmentViewModel(id);

            var result = Task.Run(() => _sender.Send(query)).Result;

            if (result.IsSuccess)
                return PartialView(result.Data);
            else
                return BadRequest();
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    [Route("/Departments/New")]
    public IActionResult New()
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

    [HttpPost]
    [Route("/Departments/New")]
    public IActionResult New(CreateDepartmentViewModel model)
    {
        try
        {
            var createdById = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

            var command = new CreateDepartmentCommand(model, Guid.Parse(createdById));

            var result = Task.Run(() => _sender.Send(command)).Result;

            return result.IsSuccess ?
                new JsonResult(JsonResponseModel.Success(null, null)) :
                new JsonResult(JsonResponseModel.Error(result.Error!.Message));
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}
