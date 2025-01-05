using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SHS.StudentPortal.App.Commands;
using SHS.StudentPortal.Common.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SHS.StudentPortal.Web.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly ISender _sender;

    public AccountController(ILogger<AccountController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        LoginViewModel login = new() { UserAccountType = "student", UserName = "", Password = "" };

        return View(login);
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel loginViewModel, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var command = new AuthenticateLoginViewModelCommand(loginViewModel.UserAccountType, loginViewModel.UserAccountType,
                loginViewModel.UserName, loginViewModel.Password);

            var result = Task.Run(() => _sender.Send(command)).Result;

            if (result.IsSuccess)
            {
                // Write to context.user
                var claims = new List<Claim>()
                {
                    new Claim("UserId", result.Data!.UserId.ToString()),
                    new Claim("Username", result.Data!.Username),
                    new Claim("FullName", $"{result.Data!.Firstname} {result.Data!.Lastname}"),
                    new Claim("FirstName", $"{result.Data!.Firstname}"),
                    new Claim("MiddleName", $"{result.Data!.Middlename}"),
                    new Claim("LastName", $"{result.Data!.Lastname}"),
                    new Claim(ClaimTypes.Role, result.Data!.RoleType)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var userPrincipal = new ClaimsPrincipal(claimsIdentity);

                Task.Run(() => HttpContext.SignInAsync(userPrincipal));

                _logger.LogInformation($"User {result.Data.Username} logged in");

                return RedirectToLocal(returnUrl, result.Data!.RoleType);
            }
            else
            {
                ViewData["UserLoginFailed"] = result.Error!.Message;

                return View("Login", loginViewModel);
            }
        }

        return View("Login", loginViewModel);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        if (HttpContext.User.Identity?.IsAuthenticated ?? false)
        {
            Task.Run(() => HttpContext.SignOutAsync());

            _logger.LogInformation($"User {HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username")!.Value} logged out");
        }

        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult Register()
    {
        var model = new RegisterStudentViewModel()
        {
            YearLevel = 11,
            Track = Track.AcademicTrack.Id,
            Strand = Track.AcademicTrack.Strands.First().Id,
            //ExternalAcademicRecords = new() { new() { SubjectName = "Test", Rating = "99.12", AcademicYear = "2022-2023", Semester = "1st", TempId = 1 } }
            ExternalAcademicRecords = new List<AcademicRecordViewModel>()
        };

        ViewData["Tracks"] = Track.GetAllTracks;
        ViewData["AcademicTrackStrands"] = Strand.GetAllAcademicTrackStrands;
        ViewData["ArtsAndDesignTrackStrands"] = Strand.GetAllArtsAndDesignTrackStrands;
        ViewData["SportsTrackStrands"] = Strand.GetAllSportsTrackStrands;
        ViewData["TVLTrackStrands"] = Strand.GetAllTVLTrackStrands;

        return View(model);
    }

    [HttpPost]
    public IActionResult Register(RegisterStudentViewModel model)
    {
        ViewData["Tracks"] = Track.GetAllTracks;
        ViewData["AcademicTrackStrands"] = Strand.GetAllAcademicTrackStrands;
        ViewData["ArtsAndDesignTrackStrands"] = Strand.GetAllArtsAndDesignTrackStrands;
        ViewData["SportsTrackStrands"] = Strand.GetAllSportsTrackStrands;
        ViewData["TVLTrackStrands"] = Strand.GetAllTVLTrackStrands;

        if (ModelState.IsValid)
        {
            var command = new RegisterStudentCommand(model);

            var result = Task.Run(() => _sender.Send(command)).Result;

            if(result.IsSuccess)
            {
                TempData["SpecialMessage"] = "You have successfully registered!";

                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["RegisterStudentFailed"] = result.Error!.Message;

                return View("Register", model);
            }
        }

        return View("Register", model);
    }

    private IActionResult RedirectToLocal(string returnUrl, string roleType)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            var defaultAction = nameof(HomeController.Index);

            switch (roleType)
            {
                case "admin":
                    defaultAction = nameof(HomeController.Students);
                    break;

            }

            return RedirectToAction(defaultAction, "Home");
        }
    }

    [HttpPost]
    [Route("/Account/UpdatePersonalInfo")]
    public IActionResult UpdatePersonalInfo(UpdatePersonalInfoViewModel model)
    {
        try
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

            var command = new UpdatePersonalInfoCommand(Guid.Parse(userId), model);

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
    [Route("/Account/UpdatePassword")]
    public IActionResult UpdatePassword(string newPassword)
    {
        try
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

            var command = new UpdatePasswordCommand(Guid.Parse(userId), newPassword);

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
