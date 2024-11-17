using Microsoft.EntityFrameworkCore;
using SHS.StudentPortal.App.Abstractions.Security;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.Database;

public class Seeder
{
    public static void Seed(AppDbContext appDbContext, IBCryptAuthProvider bCryptAuthProvider)
    {
        ArgumentNullException.ThrowIfNull(nameof(appDbContext));

        appDbContext.Database.EnsureCreated();

        appDbContext.Database.Migrate();

        using (var txn = appDbContext.Database.BeginTransaction())
        {
            try
            {
                var adminUserAccount = appDbContext.UserAccounts
            .AsNoTracking()
            .FirstOrDefault(x => x.Username == "admin");

                if (adminUserAccount == null)
                {
                    var passwordHash = bCryptAuthProvider.EncryptPassword("admin");

                    var newAdminUserAccount = new UserAccount()
                        .CreateAdminOrInstr("admin",
                        passwordHash,
                        "admin.admin@test.com",
                        Constants.SystemGuid);

                    appDbContext.UserAccounts.Add(newAdminUserAccount);

                    appDbContext.SaveChanges();

                    var newAdminUser = new User()
                        .Create("Admin FN",
                            null,
                            "Admin LN",
                            RoleTypes.Admin.Id,
                            newAdminUserAccount.Id,
                            Constants.SystemGuid);

                    appDbContext.Users.Add(newAdminUser);

                    appDbContext.SaveChanges();
                }
                else
                {
                    var adminUser = appDbContext.Users
                        .AsNoTracking()
                        .FirstOrDefault(x => x.UserAccountId == adminUserAccount.Id);

                    if (adminUser == null)
                    {
                        var newAdminUser = new User()
                            .Create("Admin FN",
                                null,
                                "Admin LN",
                                RoleTypes.Admin.Id,
                                adminUserAccount.Id,
                                Constants.SystemGuid);

                        appDbContext.Users.Add(newAdminUser);

                        appDbContext.SaveChanges();
                    }
                }

                var departments = appDbContext.Departments
                    .AsNoTracking()
                    .ToList();

                if (departments == null || departments.Count == 0)
                {
                    var newDepartments = new List<Department>()
            {
                new() { Name = "Science", Description = "Science Department", CreatedById = Constants.SystemGuid, CreatedDate = DateTime.UtcNow },
                new() { Name = "Math", Description = "Math Department", CreatedById = Constants.SystemGuid, CreatedDate = DateTime.UtcNow },
                new() { Name = "English", Description = "English Department", CreatedById = Constants.SystemGuid, CreatedDate = DateTime.UtcNow },
                new() { Name = "Filipino", Description = "Filipino Department", CreatedById = Constants.SystemGuid, CreatedDate = DateTime.UtcNow },
                new() { Name = Constants.NotApplicable, Description = "A Placeholder Department", CreatedById = Constants.SystemGuid, CreatedDate = DateTime.UtcNow },
            };

                    appDbContext.Departments.AddRange(newDepartments);

                    appDbContext.SaveChanges();

                    departments = newDepartments;
                }

                var instructorUserAccountList = appDbContext.UserAccounts
                    .AsNoTracking()
                    .Include(x => x.User)
                    .ThenInclude(x => x.InstructorInfo)
                    .Where(x => x.User!.RoleType == RoleTypes.Instructor.Id)
                    .ToList();

                var instructorInfoList = new List<InstructorInfo>();

                if (instructorUserAccountList is null || instructorUserAccountList.Count == 0)
                {
                    instructorUserAccountList = new();

                    for (int i = 1; i <= 4; i++)
                    {
                        var passwordHash = bCryptAuthProvider.EncryptPassword($"instructor{i}");

                        var newInstructorUserAccount = new UserAccount()
                        .CreateAdminOrInstr($"instructor{i}",
                            passwordHash,
                            $"instructor{1}@instructor{1}.com",
                            Constants.SystemGuid);

                        appDbContext.UserAccounts.Add(newInstructorUserAccount);

                        appDbContext.SaveChanges();

                        var newInstructorUser = new User()
                        .Create($"Instructor{i} FN",
                            null,
                            $"Instructor{i} LN",
                            RoleTypes.Instructor.Id,
                            newInstructorUserAccount.Id,
                            Constants.SystemGuid);

                        appDbContext.Users.Add(newInstructorUser);

                        appDbContext.SaveChanges();

                        var instructorInfoModel = appDbContext.Instructors
                        .AsNoTracking()
                        .FirstOrDefault(x => x.UserId == newInstructorUser.Id);

                        if (instructorInfoModel is null)
                        {
                            var newInstructorInfo = new InstructorInfo()
                                .Create($"{i}000",
                                departments[i].Name,
                                "+1234567890",
                                newInstructorUser.Id,
                                departments[i].Id,
                                Constants.SystemGuid);

                            appDbContext.Instructors.Add(newInstructorInfo);

                            appDbContext.SaveChanges();

                            instructorInfoList.Add(newInstructorInfo);
                        }
                    }
                }

                var sections = appDbContext.Sections
                    .AsNoTracking()
                    .ToList();

                if (sections is null || sections.Count == 0)
                {
                    var sectionList = new List<Section>()
            {
                new Section() { Name = "Section 11-A", AdviserId = instructorInfoList[0].Id, CreatedById = Constants.SystemGuid, CreatedDate = DateTime.UtcNow },
                new Section() { Name = "Section 11-B", AdviserId = instructorInfoList[1].Id, CreatedById = Constants.SystemGuid, CreatedDate = DateTime.UtcNow },
                new Section() { Name = "Section 12-A", AdviserId = instructorInfoList[2].Id, CreatedById = Constants.SystemGuid, CreatedDate = DateTime.UtcNow },
                new Section() { Name = Constants.NotApplicable, AdviserId = instructorInfoList[3].Id, CreatedById = Constants.SystemGuid, CreatedDate = DateTime.UtcNow },
            };

                    appDbContext.Sections.AddRange(sectionList);

                    appDbContext.SaveChanges();
                };

                var currentSemesterSetting = appDbContext.Settings
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Name == Constants.CurrentSemester);

                if (currentSemesterSetting is null)
                {
                    var newCurrentSemesterSetting = new Setting()
                        .Create(Constants.CurrentSemester, "2nd", Constants.SystemGuid);

                    appDbContext.Settings.Add(newCurrentSemesterSetting);

                    appDbContext.SaveChanges();
                }

                var currentAcademicYearSetting = appDbContext.Settings
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Name == Constants.CurrentAcademicYear);

                if (currentAcademicYearSetting is null)
                {
                    var newCurrentAcademicYearSetting = new Setting()
                        .Create(Constants.CurrentAcademicYear, "2022-2023", Constants.SystemGuid);

                    appDbContext.Settings.Add(newCurrentAcademicYearSetting);

                    appDbContext.SaveChanges();
                }

                var subjects = appDbContext.Subjects
                    .AsNoTracking()
                    .ToList();

                if (subjects is null || subjects.Count == 0)
                {
                    var subjectList = new List<Subject>()
            {
                new Subject().Create("ABC123", "Basic Subject 101", 600, 80, "Test Description 1", 3, "1st", "2022-2023", $"{Track.AcademicTrack.Id}-{Track.AcademicTrack.Strands[0].Id}", Constants.SystemGuid),
                new Subject().Create("DEF456", "Basic Subject 102", 600, 80, "Test Description 2", 3, "1st", "2022-2023",  $"{Track.AcademicTrack.Id}-{Track.AcademicTrack.Strands[1].Id}", Constants.SystemGuid),
                new Subject().Create("GHI789", "Basic Education 201", 600, 80, "Test Description 3", 3, "2nd", "2022-2023",  $"{Track.TechnicalVocationalTrack.Id}-{Track.TechnicalVocationalTrack.Strands[0].Id}", Constants.SystemGuid),
                new Subject().Create("JKL012", "Basic Education 202", 600, 80, "Test Description 4", 3, "2nd", "2022-2023",  $"{Track.TechnicalVocationalTrack.Id}-{Track.TechnicalVocationalTrack.Strands[1].Id}", Constants.SystemGuid),
            };

                    appDbContext.Subjects.AddRange(subjectList);

                    appDbContext.SaveChanges();

                    subjects = subjectList;
                }

                var schedules = appDbContext.Schedules
                    .AsNoTracking()
                    .ToList();

                if (schedules is null || schedules.Count == 0)
                {
                    var scheduleList = new List<Schedule>()
            {
                new Schedule().Create("monday", new(10, 30), new(11, 30), "Room 101", "Test Remark 1", instructorInfoList[0].Id, subjects[0].Id, Constants.SystemGuid),
                new Schedule().Create("tuesday", new(7, 30), new(10, 30), "Room 102", "Test Remark 2", instructorInfoList[1].Id, subjects[1].Id, Constants.SystemGuid),
                new Schedule().Create("wednesday", new(13, 00), new(14, 30), "Room 501", "Test Remark 3", instructorInfoList[2].Id, subjects[2].Id, Constants.SystemGuid),
                new Schedule().Create("friday", new(16, 00), new(17, 00), "Room 502", "Test Remark 4", instructorInfoList[3].Id, subjects[3].Id, Constants.SystemGuid),
            };

                    appDbContext.Schedules.AddRange(scheduleList);

                    appDbContext.SaveChanges();

                    schedules = scheduleList;
                }

                txn.Commit();
            }
            catch (Exception)
            {
                txn.Rollback();

                throw;
            }
        }
    }
}
