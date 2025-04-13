using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Serilog;
using SHS.StudentPortal.App;
using SHS.StudentPortal.App.Abstractions;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.App.Abstractions.Security;
using SHS.StudentPortal.App.Implementations;
using SHS.StudentPortal.App.Implementations.Security;
using SHS.StudentPortal.Blazor.Components;
using SHS.StudentPortal.Database;
using SHS.StudentPortal.Database.Persistence;
using SHS.StudentPortal.Database.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// == WEB SERVICES ==
// SeriLog
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

// HttpContextAccess for cookie auth
builder.Services.AddHttpContextAccessor();
// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.AccessDeniedPath = "/AccessDenied";
    });

// == DATABASE SERVICES ==
// EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}, ServiceLifetime.Scoped);
// Repositories
builder.Services.AddScoped<ICourseRepository, CourseRepository>(); // Not used
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<IStudentInfoRepository, StudentInfoRepository>();
builder.Services.AddScoped<IUserAccountRepository, UserAccountRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInstructorInfoRepository, InstructorInfoRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IExternalAcademicRecordRepository, ExternalAcademicRecordRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<ISettingRepository, SettingRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IPreRequisiteRepository, PreRequisiteRepository>();
builder.Services.AddScoped<IStudentScheduleRepository, StudentScheduleRepository>();
builder.Services.AddScoped<IAcademicRecordRepository, AcademicRecordRepository>();
// UnitOfWork
builder.Services.AddScoped<IBaseUnitOfWork, BaseUnitOfWork>();

// == APP SERVICES ==
// BCrypt
builder.Services.AddScoped<IBCryptAuthProvider, BCryptAuthProvider>();
// AuthStateService
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IUserStateService, UserStateService>();
// MediatR
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(AssemblyReference.Assembly));
// Loading Service
builder.Services.AddScoped<ILoadingService, LoadingService>();

builder.Services.AddRadzenComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();

    // Seed database
    using var scope = app.Services.CreateScope();

    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var bCryptProvider = services.GetRequiredService<IBCryptAuthProvider>();

        Seeder.Seed(context, bCryptProvider);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();

        logger.LogError($"Error while seeding database: {ex}");
    }
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.Run();
