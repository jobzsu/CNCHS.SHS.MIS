using MediatR;
using Microsoft.AspNetCore.Mvc;
using SHS.StudentPortal.App.Commands;
using SHS.StudentPortal.App.Queries;
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
            var query = new GetSubjectByIdQuery(id);

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
            return BadRequest();
        }
    }

    [HttpPut]
    [Route("/Subjects/{id}")]
    public IActionResult Update(int id, UpdateSubjectViewModel view)
    {
        try
        {
            var updatedById = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

            var command = new UpdateSubjectCommand(view, Guid.Parse(updatedById));

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
