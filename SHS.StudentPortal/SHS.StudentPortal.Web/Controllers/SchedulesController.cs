using MediatR;
using Microsoft.AspNetCore.Mvc;
using SHS.StudentPortal.App.Commands;
using SHS.StudentPortal.App.Queries;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.Web.Controllers;

public class SchedulesController : Controller
{
    private readonly ISender _sender;

    public SchedulesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Route("/Schedules/{id}")]
    public IActionResult Index(long id)
    {
        try
        {
            var query = new GetScheduleByIdQuery(id);

            var result = Task.Run(() => _sender.Send(query)).Result;

            if (result.IsSuccess)
                return PartialView(result.Data);
            else
                return BadRequest();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("/Schedules/New")]
    public IActionResult New()
    {
        var query = new GetModelViewForNewScheduleQuery();

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

    [HttpPost]
    [Route("/Schedules/New")]
    public IActionResult New(CreateScheduleViewModel model)
    {
        try
        {
            var approvedById = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

            var command = new CreateScheduleCommand(model, Guid.Parse(approvedById));

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

    [HttpPut]
    [Route("/Schedules/{id}")]
    public IActionResult Update(long id, UpdateScheduleViewModel model)
    {
        try
        {
            var updatedById = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

            var command = new UpdateScheduleCommand(model, Guid.Parse(updatedById));

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
