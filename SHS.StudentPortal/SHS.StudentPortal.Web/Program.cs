using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SHS.StudentPortal.App;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.App.Abstractions.Security;
using SHS.StudentPortal.App.Implementations.Security;
using SHS.StudentPortal.Database;
using SHS.StudentPortal.Database.Persistence;
using SHS.StudentPortal.Database.Repositories;

namespace SHS.StudentPortal.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews()
            .AddJsonOptions(options => {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

        // == WEB SERVICES ==
        // SeriLog
        builder.Host.UseSerilog((context, config) =>
        {
            config.ReadFrom.Configuration(context.Configuration);
        });
        // Authentication
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
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
        // MediatR
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(AssemblyReference.Assembly));

        var app = builder.Build();

        // SeriLog
        app.UseSerilogRequestLogging();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else
        {
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

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
