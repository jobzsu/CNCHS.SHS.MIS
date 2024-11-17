using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SHS.StudentPortal.App.Commands;
using SHS.StudentPortal.App.Queries;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Common.Utilities.Models;
using SHS.StudentPortal.Domain.Models;
using SHS.StudentPortal.Web.Models;
using System.Diagnostics;

namespace SHS.StudentPortal.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISender _sender;

    public HomeController(ILogger<HomeController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    public IActionResult Index()
    {
        _logger.LogInformation(JsonConvert.SerializeObject(HttpContext.User.Claims, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }));

        return RedirectToAction("Students");
        //return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult AboutUs()
    {
        ViewData["ActiveMenu"] = "aboutUsMenu";

        return View();
    }

    public IActionResult ContactUs()
    {
        ViewData["ActiveMenu"] = "contactUsMenu";

        return View();
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public IActionResult Students(string? searchKeyword,
        int yearLevel,
        string? sectionId,
        string? track,
        string? strand,
        int pageNumber = 1)
    {
        ViewData["ActiveMenu"] = "studentsMenu";

        ViewData["SearchKeyword"] = searchKeyword;
        ViewData["SectionId"] = sectionId;
        ViewData["Track"] = track;
        ViewData["Strand"] = strand;
        ViewData["YearLevel"] = yearLevel == 0 ? "" : yearLevel.ToString();

        ViewData["Tracks"] = Track.GetAllTracks;
        ViewData["AcademicTrackStrands"] = Strand.GetAllAcademicTrackStrands;
        ViewData["ArtsAndDesignTrackStrands"] = Strand.GetAllArtsAndDesignTrackStrands;
        ViewData["SportsTrackStrands"] = Strand.GetAllSportsTrackStrands;
        ViewData["TVLTrackStrands"] = Strand.GetAllTVLTrackStrands;

        var studentListPaginationFilter = StudentPaginationFilter
            .NewStudentListSearch(searchKeyword, yearLevel, sectionId, track, strand, pageNumber);

        var query = new GetPaginatedStudentListQuery(studentListPaginationFilter);
        var sectionListQuery = new GetSectionListQuery();

        var result = Task.Run(() => _sender.Send(query)).Result;

        var sectionListResult = Task.Run(() => _sender.Send(sectionListQuery)).Result;

        if (sectionListResult.IsSuccess)
            ViewData["SectionList"] = sectionListResult.Data;
        else
        {
            ViewData["ErrorMessage"] = result.Error!.Message;

            return RedirectToAction("Error");
        }

        if (result.IsSuccess)
            return View(result.Data);
        else
        {
            ViewData["ErrorMessage"] = result.Error!.Message;

            return RedirectToAction("Error");
        }
    }

    [Authorize(Roles = "admin")]
    public IActionResult Instructors(string? searchKeyword,
        int departmentId = 0,
        int pageNumber = 1)
    {
        ViewData["ActiveMenu"] = "instructorsMenu";

        ViewData["SearchKeyword"] = searchKeyword;
        ViewData["DepartmentId"] = departmentId;

        var instructorListPaginationFilter = InstructorPaginationFilter
            .NewInstructorListSearch(searchKeyword,
                departmentId, 
                pageNumber);

        var query = new GetPaginatedInstructorListQuery(instructorListPaginationFilter);

        var result = Task.Run(() => _sender.Send(query)).Result;

        if (result.IsSuccess)
            return View(result.Data);
        else
        {
            ViewData["ErrorMessage"] = result.Error!.Message;

            return RedirectToAction("Error");
        }
    }

    [Authorize(Roles = "admin")]
    public IActionResult Departments(string? searchKeyword,
        int pageNumber = 1)
    {
        ViewData["ActiveMenu"] = "departmentsMenu";

        ViewData["SearchKeyword"] = searchKeyword;

        var departmentListPaginationFilter = DepartmentPaginationFilter
            .NewDepartmentListSearch(searchKeyword, pageNumber);

        var query = new GetPaginatedDepartmentListQuery(departmentListPaginationFilter);

        var result = Task.Run(() => _sender.Send(query)).Result;

        if (result.IsSuccess)
            return View(result.Data);
        else
        {
            ViewData["ErrorMessage"] = result.Error!.Message;

            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    [Authorize(Roles = "admin,instructor")]
    public IActionResult Subjects(string? searchKeyword,
        string? track,
        string? strand,
        int pageNumber = 1)
    {
        ViewData["ActiveMenu"] = "subjectsMenu";

        ViewData["SearchKeyword"] = searchKeyword;
        ViewData["Track"] = track;
        ViewData["Strand"] = strand;

        ViewData["Tracks"] = Track.GetAllTracks;
        ViewData["AcademicTrackStrands"] = Strand.GetAllAcademicTrackStrands;
        ViewData["ArtsAndDesignTrackStrands"] = Strand.GetAllArtsAndDesignTrackStrands;
        ViewData["SportsTrackStrands"] = Strand.GetAllSportsTrackStrands;
        ViewData["TVLTrackStrands"] = Strand.GetAllTVLTrackStrands;

        var filter = SubjectsPaginationFilter.NewSubjectsListSearch(searchKeyword, track, strand, pageNumber);

        var query = new GetPaginatedSubjectListQuery(filter);

        var result = Task.Run(() => _sender.Send(query)).Result;

        if (result.IsSuccess)
            return View(result.Data);
        else
        {
            ViewData["ErrorMessage"] = result.Error!.Message;
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public IActionResult Schedules(string? selectedDays,
            string? track,
            string? strand,
            int pageNumber = 1)
    {
        ViewData["ActiveMenu"] = "schedulesMenu";
        ViewData["Track"] = track;
        ViewData["Strand"] = strand;
        ViewData["SelectedDays"] = selectedDays ?? string.Empty;

        ViewData["Tracks"] = Track.GetAllTracks;
        ViewData["AcademicTrackStrands"] = Strand.GetAllAcademicTrackStrands;
        ViewData["ArtsAndDesignTrackStrands"] = Strand.GetAllArtsAndDesignTrackStrands;
        ViewData["SportsTrackStrands"] = Strand.GetAllSportsTrackStrands;
        ViewData["TVLTrackStrands"] = Strand.GetAllTVLTrackStrands;

        var filter = SchedulePaginationFilter.NewScheduleListSearch(null, selectedDays ?? string.Empty, track, strand, pageNumber);

        var query = new GetPaginatedScheduleListQuery(filter);

        var result = Task.Run(() => _sender.Send(query)).Result;

        if (result.IsSuccess)
            return View(result.Data);
        else
        {
            ViewData["ErrorMessage"] = result.Error!.Message;

            return RedirectToAction("Error");
        }    
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
