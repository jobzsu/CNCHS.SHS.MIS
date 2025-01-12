using MediatR;
using Microsoft.AspNetCore.Mvc;
using SHS.StudentPortal.App.Commands;
using SHS.StudentPortal.App.Queries;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ISender _sender;

        public StudentsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Route("/Students/{id}")]
        public IActionResult Index(Guid id)
        {
            try
            {
                var query = new GetStudentForAdminApprovalQuery(id);

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
            catch(Exception)
            {
                return BadRequest();
            }

            //var jsonResponse = result.IsSuccess ?
            //    JsonResponseModel.Success(result.Data, null) :
            //    JsonResponseModel.Error(result.Error!.Message);

            //return new JsonResult(jsonResponse);
        }

        [HttpPost]
        [Route("/Students/Approve/{id}")]
        public IActionResult Approve(Guid id)
        {
            try
            {
                var approvedById = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

                var command = new ApproveStudentCommand(id, Guid.Parse(approvedById));

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

        [HttpPost]
        [Route("/Students/UpdatePassword/{id}")]
        public IActionResult UpdatePassword(string newPassword, Guid id)
        {
            try
            {
                var passwordUpdatedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

                var command = new UpdateStudentPasswordCommand(newPassword, id, Guid.Parse(passwordUpdatedBy));

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

        [HttpPost]
        [Route("/Students/MarkForEnrollment/{id}")]
        public IActionResult MarkForEnrollment(Guid id)
        {
            try
            {
                var verifiedBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

                var command = new MarkStudentForEnrollmentCommand(id, Guid.Parse(verifiedBy));

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

        [HttpGet]
        [Route("/Students/Enroll/{id}")]
        public IActionResult Enroll(Guid id)
        {
            try
            {
                var query = new GetStudentEnrollmentViewModelQuery(id);

                var result = Task.Run(() => _sender.Send(query)).Result;

                return result.IsSuccess ?
                    PartialView(result.Data) : BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("/Students/Enroll/{id}")]
        public IActionResult Enroll(Guid id, EnrollmentViewModel model)
        {
            try
            {
                var enrolledById = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

                var command = new EnrollStudentCommand(Guid.Parse(enrolledById), model);

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

        [HttpPost]
        [Route("/Students/EncodeGrades/{id}")]
        public IActionResult EncodeGrades([FromBody] List<EncodeStudentGradeViewModel> model, Guid id)
        {
            try
            {
                var encodedById = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

                var command = new EncodeStudentGradesCommand(model, Guid.Parse(encodedById), id);

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
}
