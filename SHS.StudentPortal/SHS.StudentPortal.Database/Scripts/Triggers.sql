-- AcademicRecords INSERT
GO
CREATE TRIGGER dbo.AcademicRecords_Insert_Audit ON dbo.AcademicRecords
AFTER INSERT
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'ACADEMICRECORD',
		'INSERT',
		CAST(i.CreatedById AS VARCHAR(128)),
		COALESCE(i.CreatedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'AcademicRecord.Id',
				i.Rating AS 'AcademicRecord.Rating',
				i.StudentId AS 'AcademicRecord.StudentId',
				i.CreatedById AS 'AcademicRecord.CreatedById',
				i.CreatedDate AS 'AcademicRecord.CreatedDate',
				i.ModifiedById AS 'AcademicRecord.ModifiedById',
				i.ModifiedDate AS 'AcademicRecord.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- AcademicRecords UPDATE
GO
CREATE TRIGGER dbo.AcademicRecords_Update_Audit ON dbo.AcademicRecords
AFTER UPDATE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'ACADEMICRECORD',
		'UPDATE',
		CAST(i.ModifiedById AS VARCHAR(128)),
		COALESCE(i.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'AcademicRecord.Id',
				i.Rating AS 'AcademicRecord.Rating',
				i.StudentId AS 'AcademicRecord.StudentId',
				i.CreatedById AS 'AcademicRecord.CreatedById',
				i.CreatedDate AS 'AcademicRecord.CreatedDate',
				i.ModifiedById AS 'AcademicRecord.ModifiedById',
				i.ModifiedDate AS 'AcademicRecord.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- AcademicRecords DELETE
GO
CREATE TRIGGER dbo.AcademicRecords_Delete_Audit ON dbo.AcademicRecords
AFTER DELETE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(d.Id AS VARCHAR(128)),
		'ACADEMICRECORD',
		'DELETE',
		CAST(d.ModifiedById AS VARCHAR(128)),
		COALESCE(d.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				d.Id AS 'AcademicRecord.Id',
				d.Rating AS 'AcademicRecord.Rating',
				d.StudentId AS 'AcademicRecord.StudentId',
				d.CreatedById AS 'AcademicRecord.CreatedById',
				d.CreatedDate AS 'AcademicRecord.CreatedDate',
				d.ModifiedById AS 'AcademicRecord.ModifiedById',
				d.ModifiedDate AS 'AcademicRecord.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		deleted AS d;
END;
-- ================================================================
-- ================================================================
-- ================================================================

-- Courses INSERT
/*GO
CREATE TRIGGER dbo.Courses_Insert_Audit ON dbo.Courses
AFTER INSERT
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'COURSE',
		'INSERT',
		CAST(i.CreatedById AS VARCHAR(128)),
		COALESCE(i.CreatedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'Course.Id',
				i.[Name] AS 'Course.Name',
				i.[Description] AS 'Course.Description',
				i.DepartmentId AS 'Course.DepartmentId',
				i.CreatedById AS 'Course.CreatedById',
				i.CreatedDate AS 'Course.CreatedDate',
				i.ModifiedById AS 'Course.ModifiedById',
				i.ModifiedDate AS 'Course.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;*/
-- Courses UPDATE
/*GO
CREATE TRIGGER dbo.Courses_Update_Audit ON dbo.Courses
AFTER UPDATE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'COURSE',
		'UPDATE',
		CAST(i.ModifiedById AS VARCHAR(128)),
		COALESCE(i.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'Course.Id',
				i.[Name] AS 'Course.Name',
				i.[Description] AS 'Course.Description',
				i.DepartmentId AS 'Course.DepartmentId',
				i.CreatedById AS 'Course.CreatedById',
				i.CreatedDate AS 'Course.CreatedDate',
				i.ModifiedById AS 'Course.ModifiedById',
				i.ModifiedDate AS 'Course.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;*/
-- Courses DELETE
/*GO
CREATE TRIGGER dbo.Courses_Delete_Audit ON dbo.Courses
AFTER DELETE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(d.Id AS VARCHAR(128)),
		'COURSE',
		'DELETE',
		CAST(d.ModifiedById AS VARCHAR(128)),
		COALESCE(d.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				d.Id AS 'Course.Id',
				d.[Name] AS 'Course.Name',
				d.[Description] AS 'Course.Description',
				d.DepartmentId AS 'Course.DepartmentId',
				d.CreatedById AS 'Course.CreatedById',
				d.CreatedDate AS 'Course.CreatedDate',
				d.ModifiedById AS 'Course.ModifiedById',
				d.ModifiedDate AS 'Course.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		deleted as d;
END;*/
-- ================================================================
-- ================================================================
-- ================================================================
-- Departments INSERT
-- Departments INSERT
GO
CREATE TRIGGER dbo.Departments_Insert_Audit ON dbo.Departments
AFTER INSERT
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'DEPARTMENT',
		'INSERT',
		CAST(i.CreatedById AS VARCHAR(128)),
		COALESCE(i.CreatedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'Department.Id',
				i.[Name] AS 'Department.Name',
				i.[Description] AS 'Department.Description',
				i.CreatedById AS 'Department.CreatedById',
				i.CreatedDate AS 'Department.CreatedDate',
				i.ModifiedById AS 'Department.ModifiedById',
				i.ModifiedDate AS 'Department.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- Departments UPDATE
GO
CREATE TRIGGER dbo.Departments_Update_Audit ON dbo.Departments
AFTER UPDATE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'DEPARTMENT',
		'UPDATE',
		CAST(i.ModifiedById AS VARCHAR(128)),
		COALESCE(i.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'Department.Id',
				i.[Name] AS 'Department.Name',
				i.[Description] AS 'Department.Description',
				i.CreatedById AS 'Department.CreatedById',
				i.CreatedDate AS 'Department.CreatedDate',
				i.ModifiedById AS 'Department.ModifiedById',
				i.ModifiedDate AS 'Department.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- Departments DELETE
GO
CREATE TRIGGER dbo.Departments_Delete_Audit ON dbo.Departments
AFTER DELETE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(d.Id AS VARCHAR(128)),
		'DEPARTMENT',
		'DELETE',
		CAST(d.ModifiedById AS VARCHAR(128)),
		COALESCE(d.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				d.Id AS 'Department.Id',
				d.[Name] AS 'Department.Name',
				d.[Description] AS 'Department.Description',
				d.CreatedById AS 'Department.CreatedById',
				d.CreatedDate AS 'Department.CreatedDate',
				d.ModifiedById AS 'Department.ModifiedById',
				d.ModifiedDate AS 'Department.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		deleted as d;
END;
-- ================================================================
-- ================================================================
-- ================================================================
-- InstructorInfos INSERT
GO
CREATE TRIGGER dbo.InstructorInfos_Insert_Audit ON dbo.InstructorInfos
AFTER INSERT
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'INSTRUCTORINFO',
		'INSERT',
		CAST(i.CreatedById AS VARCHAR(128)),
		COALESCE(i.CreatedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'InstructorInfo.Id',
				i.EmployeeId AS 'InstructorInfo.EmployeeId',
				i.Major AS 'InstructorInfo.Major',
				i.ContactInformation AS 'InstructorInfo.ContactInformation',
				i.UserId AS 'InstructorInfo.UserId',
				i.DepartmentId AS 'InstructorInfo.DepartmentId',
				i.CreatedById AS 'InstructorInfo.CreatedById',
				i.CreatedDate AS 'InstructorInfo.CreatedDate',
				i.ModifiedById AS 'InstructorInfo.ModifiedById',
				i.ModifiedDate AS 'InstructorInfo.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- InstructorInfos UPDATE
GO
CREATE TRIGGER dbo.InstructorInfos_Update_Audit ON dbo.InstructorInfos
AFTER UPDATE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'INSTRUCTORINFO',
		'UPDATE',
		CAST(i.ModifiedById AS VARCHAR(128)),
		COALESCE(i.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'InstructorInfo.Id',
				i.EmployeeId AS 'InstructorInfo.EmployeeId',
				i.Major AS 'InstructorInfo.Major',
				i.ContactInformation AS 'InstructorInfo.ContactInformation',
				i.UserId AS 'InstructorInfo.UserId',
				i.DepartmentId AS 'InstructorInfo.DepartmentId',
				i.CreatedById AS 'InstructorInfo.CreatedById',
				i.CreatedDate AS 'InstructorInfo.CreatedDate',
				i.ModifiedById AS 'InstructorInfo.ModifiedById',
				i.ModifiedDate AS 'InstructorInfo.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- InstructorInfos DELETE
GO
CREATE TRIGGER dbo.InstructorInfos_Delete_Audit ON dbo.InstructorInfos
AFTER DELETE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(d.Id AS VARCHAR(128)),
		'INSTRUCTORINFO',
		'DELETE',
		CAST(d.ModifiedById AS VARCHAR(128)),
		COALESCE(d.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				d.Id AS 'InstructorInfo.Id',
				d.EmployeeId AS 'InstructorInfo.EmployeeId',
				d.Major AS 'InstructorInfo.Major',
				d.ContactInformation AS 'InstructorInfo.ContactInformation',
				d.UserId AS 'InstructorInfo.UserId',
				d.DepartmentId AS 'InstructorInfo.DepartmentId',
				d.CreatedById AS 'InstructorInfo.CreatedById',
				d.CreatedDate AS 'InstructorInfo.CreatedDate',
				d.ModifiedById AS 'InstructorInfo.ModifiedById',
				d.ModifiedDate AS 'InstructorInfo.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM deleted AS d;
END;
-- ================================================================
-- ================================================================
-- ================================================================
-- PreRequisites INSERT
GO
CREATE TRIGGER dbo.PreRequisites_Insert_Audit ON dbo.PreRequisites
AFTER INSERT
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'PREREQUISITE',
		'INSERT',
		CAST(i.CreatedById AS VARCHAR(128)),
		COALESCE(i.CreatedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'PreRequisite.Id',
				i.PreRequisiteSubjectId AS 'PreRequisite.PreRequisiteSubjectId',
				i.SubjectId AS 'PreRequisite.SubjectId',
				i.CreatedById AS 'PreRequisite.CreatedById',
				i.CreatedDate AS 'PreRequisite.CreatedDate',
				i.ModifiedById AS 'PreRequisite.ModifiedById',
				i.ModifiedDate AS 'PreRequisite.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- PreRequisites UPDATE
GO
CREATE TRIGGER dbo.PreRequisites_Update_Audit ON dbo.PreRequisites
AFTER UPDATE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'PREREQUISITE',
		'UPDATE',
		CAST(i.ModifiedById AS VARCHAR(128)),
		COALESCE(i.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'PreRequisite.Id',
				i.PreRequisiteSubjectId AS 'PreRequisite.PreRequisiteSubjectId',
				i.SubjectId AS 'PreRequisite.SubjectId',
				i.CreatedById AS 'PreRequisite.CreatedById',
				i.CreatedDate AS 'PreRequisite.CreatedDate',
				i.ModifiedById AS 'PreRequisite.ModifiedById',
				i.ModifiedDate AS 'PreRequisite.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- PreRequisites DELETE
GO
CREATE TRIGGER dbo.PreRequisites_Delete_Audit ON dbo.PreRequisites
AFTER DELETE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(d.Id AS VARCHAR(128)),
		'PREREQUISITE',
		'DELETE',
		CAST(d.ModifiedById AS VARCHAR(128)),
		COALESCE(d.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				d.Id AS 'PreRequisite.Id',
				d.PreRequisiteSubjectId AS 'PreRequisite.PreRequisiteSubjectId',
				d.SubjectId AS 'PreRequisite.SubjectId',
				d.CreatedById AS 'PreRequisite.CreatedById',
				d.CreatedDate AS 'PreRequisite.CreatedDate',
				d.ModifiedById AS 'PreRequisite.ModifiedById',
				d.ModifiedDate AS 'PreRequisite.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM deleted AS d;
END;
-- ================================================================
-- ================================================================
-- ================================================================
-- Schedule INSERT
GO
CREATE TRIGGER dbo.Schedules_Insert_Audit ON dbo.Schedules
AFTER INSERT
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'SCHEDULE',
		'INSERT',
		CAST(i.CreatedById AS VARCHAR(128)),
		COALESCE(i.CreatedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'Schedule.Id',
				i.[Day] AS 'Schedule.Day',
				i.TimeStart AS 'Schedule.TimeStart',
				i.TimeEnd AS 'Schedule.TimeEnd',
				i.RoomNumber AS 'Schedule.RoomNumber',
				i.Remarks AS 'Schedule.Remarks',
				i.InstructorId AS 'Schedule.InstructorId',
				i.SubjectId AS 'Schedule.SubjectId',
				i.CreatedById AS 'Schedule.CreatedById',
				i.CreatedDate AS 'Schedule.CreatedDate',
				i.ModifiedById AS 'Schedule.ModifiedById',
				i.ModifiedDate AS 'Schedule.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- Schedule UPDATE
GO
CREATE TRIGGER dbo.Schedules_Update_Audit ON dbo.Schedules
AFTER UPDATE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'SCHEDULE',
		'UPDATE',
		CAST(i.ModifiedById AS VARCHAR(128)),
		COALESCE(i.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'Schedule.Id',
				i.[Day] AS 'Schedule.Day',
				i.TimeStart AS 'Schedule.TimeStart',
				i.TimeEnd AS 'Schedule.TimeEnd',
				i.RoomNumber AS 'Schedule.RoomNumber',
				i.Remarks AS 'Schedule.Remarks',
				i.InstructorId AS 'Schedule.InstructorId',
				i.SubjectId AS 'Schedule.SubjectId',
				i.CreatedById AS 'Schedule.CreatedById',
				i.CreatedDate AS 'Schedule.CreatedDate',
				i.ModifiedById AS 'Schedule.ModifiedById',
				i.ModifiedDate AS 'Schedule.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- Schedule DELETE
GO
CREATE TRIGGER dbo.Schedules_Delete_Audit ON dbo.Schedules
AFTER DELETE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(d.Id AS VARCHAR(128)),
		'SCHEDULE',
		'DELETE',
		CAST(d.ModifiedById AS VARCHAR(128)),
		COALESCE(d.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				d.Id AS 'Schedule.Id',
				d.[Day] AS 'Schedule.Day',
				d.TimeStart AS 'Schedule.TimeStart',
				d.TimeEnd AS 'Schedule.TimeEnd',
				d.RoomNumber AS 'Schedule.RoomNumber',
				d.Remarks AS 'Schedule.Remarks',
				d.InstructorId AS 'Schedule.InstructorId',
				d.SubjectId AS 'Schedule.SubjectId',
				d.CreatedById AS 'Schedule.CreatedById',
				d.CreatedDate AS 'Schedule.CreatedDate',
				d.ModifiedById AS 'Schedule.ModifiedById',
				d.ModifiedDate AS 'Schedule.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		deleted AS d;
END;
-- ================================================================
-- ================================================================
-- ================================================================
-- Sections INSERT
GO
CREATE TRIGGER dbo.Sections_Insert_Audit ON dbo.Sections
AFTER INSERT
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'SECTION',
		'INSERT',
		CAST(i.CreatedById AS VARCHAR(128)),
		COALESCE(i.CreatedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'Section.Id',
				i.[Name] AS 'Section.Name',
				i.AdviserId AS 'Section.AdviserId',
				i.CreatedById AS 'Section.CreatedById',
				i.CreatedDate AS 'Section.CreatedDate',
				i.ModifiedById AS 'Section.ModifiedById',
				i.ModifiedDate AS 'Section.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END
-- Sections UPDATE
GO
CREATE TRIGGER dbo.Sections_Update_Audit ON dbo.Sections
AFTER UPDATE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'SECTION',
		'UPDATE',
		CAST(i.ModifiedById AS VARCHAR(128)),
		COALESCE(i.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'Section.Id',
				i.[Name] AS 'Section.Name',
				i.AdviserId AS 'Section.AdviserId',
				i.CreatedById AS 'Section.CreatedById',
				i.CreatedDate AS 'Section.CreatedDate',
				i.ModifiedById AS 'Section.ModifiedById',
				i.ModifiedDate AS 'Section.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END
-- Sections DELETE
GO
CREATE TRIGGER dbo.Sections_Delete_Audit ON dbo.Sections
AFTER DELETE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(d.Id AS VARCHAR(128)),
		'SECTION',
		'DELETE',
		CAST(d.ModifiedById AS VARCHAR(128)),
		COALESCE(d.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				d.Id AS 'Section.Id',
				d.[Name] AS 'Section.Name',
				d.AdviserId AS 'Section.AdviserId',
				d.CreatedById AS 'Section.CreatedById',
				d.CreatedDate AS 'Section.CreatedDate',
				d.ModifiedById AS 'Section.ModifiedById',
				d.ModifiedDate AS 'Section.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		deleted AS d;
END
-- ================================================================
-- ================================================================
-- ================================================================
-- StudentInfos INSERT
GO
CREATE TRIGGER dbo.StudentInfos_Insert_Audit ON dbo.StudentInfos
AFTER INSERT
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'STUDENTINFO',
		'INSERT',
		CAST(i.CreatedById AS VARCHAR(128)),
		COALESCE(i.CreatedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'StudentInfo.Id',
				i.IdNumber AS 'StudentInfo.IdNumber',
				i.StudentStatus AS 'StudentInfo.StudentStatus',
				i.YearLevel AS 'StudentInfo.YearLevel',
				i.SectionId AS 'StudentInfo.SectionId',
				i.TrackAndStrand AS 'StudentInfo.TrackAndStrand',
				i.UserId AS 'StudentInfo.UserId',
				i.Gender AS 'StudentInfo.Gender',
				i.DateOfBirth AS 'StudentInfo.DateOfBirth',
				i.PlaceOfBirth AS 'StudentInfo.PlaceOfBirth',
				i.Age AS 'StudentInfo.Age',
				i.CivilStatus AS 'StudentInfo.CivilStatus',
				i.Nationality AS 'StudentInfo.Nationality',
				i.Religion AS 'StudentInfo.Religion',
				i.ContactInformation AS 'StudentInfo.ContactInformation',
				i.[Address] AS 'StudentInfo.Address',
				i.CreatedById AS 'StudentInfo.CreatedById',
				i.CreatedDate AS 'StudentInfo.CreatedDate',
				i.ModifiedById AS 'StudentInfo.ModifiedById',
				i.ModifiedDate AS 'StudentInfo.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- StudentInfos UPDATE
GO
CREATE TRIGGER dbo.StudentInfos_Update_Audit ON dbo.StudentInfos
AFTER UPDATE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'STUDENTINFO',
		'UPDATE',
		CAST(i.ModifiedById AS VARCHAR(128)),
		COALESCE(i.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'StudentInfo.Id',
				i.IdNumber AS 'StudentInfo.IdNumber',
				i.StudentStatus AS 'StudentInfo.StudentStatus',
				i.YearLevel AS 'StudentInfo.YearLevel',
				i.SectionId AS 'StudentInfo.SectionId',
				i.TrackAndStrand AS 'StudentInfo.TrackAndStrand',
				i.UserId AS 'StudentInfo.UserId',
				i.Gender AS 'StudentInfo.Gender',
				i.DateOfBirth AS 'StudentInfo.DateOfBirth',
				i.PlaceOfBirth AS 'StudentInfo.PlaceOfBirth',
				i.Age AS 'StudentInfo.Age',
				i.CivilStatus AS 'StudentInfo.CivilStatus',
				i.Nationality AS 'StudentInfo.Nationality',
				i.Religion AS 'StudentInfo.Religion',
				i.ContactInformation AS 'StudentInfo.ContactInformation',
				i.[Address] AS 'StudentInfo.Address',
				i.CreatedById AS 'StudentInfo.CreatedById',
				i.CreatedDate AS 'StudentInfo.CreatedDate',
				i.ModifiedById AS 'StudentInfo.ModifiedById',
				i.ModifiedDate AS 'StudentInfo.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- StudentInfos DELETE
GO
CREATE TRIGGER dbo.StudentInfos_Delete_Audit ON dbo.StudentInfos
AFTER DELETE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(d.Id AS VARCHAR(128)),
		'STUDENTINFO',
		'DELETE',
		CAST(d.ModifiedById AS VARCHAR(128)),
		COALESCE(d.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				d.Id AS 'StudentInfo.Id',
				d.IdNumber AS 'StudentInfo.IdNumber',
				d.StudentStatus AS 'StudentInfo.StudentStatus',
				d.YearLevel AS 'StudentInfo.YearLevel',
				d.SectionId AS 'StudentInfo.SectionId',
				d.TrackAndStrand AS 'StudentInfo.TrackAndStrand',
				d.UserId AS 'StudentInfo.UserId',
				d.Gender AS 'StudentInfo.Gender',
				d.DateOfBirth AS 'StudentInfo.DateOfBirth',
				d.PlaceOfBirth AS 'StudentInfo.PlaceOfBirth',
				d.Age AS 'StudentInfo.Age',
				d.CivilStatus AS 'StudentInfo.CivilStatus',
				d.Nationality AS 'StudentInfo.Nationality',
				d.Religion AS 'StudentInfo.Religion',
				d.ContactInformation AS 'StudentInfo.ContactInformation',
				d.[Address] AS 'StudentInfo.Address',
				d.CreatedById AS 'StudentInfo.CreatedById',
				d.CreatedDate AS 'StudentInfo.CreatedDate',
				d.ModifiedById AS 'StudentInfo.ModifiedById',
				d.ModifiedDate AS 'StudentInfo.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		deleted AS d;
END;
-- ================================================================
-- ================================================================
-- ================================================================
-- StudentSchedules INSERT
GO
CREATE TRIGGER dbo.StudentSchedules_Insert_Audit ON dbo.StudentSchedules
AFTER INSERT
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'STUDENTSCHEDULE',
		'INSERT',
		CAST(i.CreatedById AS VARCHAR(128)),
		COALESCE(i.CreatedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'StudentSchedule.Id',
				i.StudentId AS 'StudentSchedule.StudentId',
				i.ScheduleId AS 'StudentSchedule.ScheduleId',
				i.Semester AS 'StudentSchedule.Semester',
				i.AcademicYear AS 'StudentSchedule.AcademicYear',
				i.CreatedById AS 'StudentSchedule.CreatedById',
				i.CreatedDate AS 'StudentSchedule.CreatedDate',
				i.ModifiedById AS 'StudentSchedule.ModifiedById',
				i.ModifiedDate AS 'StudentSchedule.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- StudentSchedules UPDATE
GO
CREATE TRIGGER dbo.StudentSchedules_Update_Audit ON dbo.StudentSchedules
AFTER UPDATE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'STUDENTSCHEDULE',
		'UPDATE',
		CAST(i.ModifiedById AS VARCHAR(128)),
		COALESCE(i.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'StudentSchedule.Id',
				i.StudentId AS 'StudentSchedule.StudentId',
				i.ScheduleId AS 'StudentSchedule.ScheduleId',
				i.Semester AS 'StudentSchedule.Semester',
				i.AcademicYear AS 'StudentSchedule.AcademicYear',
				i.CreatedById AS 'StudentSchedule.CreatedById',
				i.CreatedDate AS 'StudentSchedule.CreatedDate',
				i.ModifiedById AS 'StudentSchedule.ModifiedById',
				i.ModifiedDate AS 'StudentSchedule.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- StudentSchedules DELETE
GO
CREATE TRIGGER dbo.StudentSchedules_Delete_Audit ON dbo.StudentSchedules
AFTER DELETE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(d.Id AS VARCHAR(128)),
		'STUDENTSCHEDULE',
		'DELETE',
		CAST(d.ModifiedById AS VARCHAR(128)),
		COALESCE(d.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				d.Id AS 'StudentSchedule.Id',
				d.StudentId AS 'StudentSchedule.StudentId',
				d.ScheduleId AS 'StudentSchedule.ScheduleId',
				d.Semester AS 'StudentSchedule.Semester',
				d.AcademicYear AS 'StudentSchedule.AcademicYear',
				d.CreatedById AS 'StudentSchedule.CreatedById',
				d.CreatedDate AS 'StudentSchedule.CreatedDate',
				d.ModifiedById AS 'StudentSchedule.ModifiedById',
				d.ModifiedDate AS 'StudentSchedule.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		deleted AS d;
END;
-- ================================================================
-- ================================================================
-- ================================================================
-- Subjects INSERT
GO
CREATE TRIGGER dbo.Subjects_Insert_Audit ON dbo.Subjects
AFTER INSERT
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'SUBJECT',
		'INSERT',
		CAST(i.CreatedById AS VARCHAR(128)),
		COALESCE(i.CreatedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'Subject.Id',
				i.Code AS 'Subject.Code',
				i.[Name] AS 'Subject.Name',
				i.[Hours] AS 'Subject.Hours',
				i.[Days] AS 'Subject.Days',
				i.[Description] AS 'Subject.Description',
				i.Units AS 'Subject.Units',
				i.Semester AS 'Subject.Semester',
				i.AcademicYear AS 'Subject.AcademicYear',
				i.TrackAndStrand AS 'Subject.TrackAndStrand',
				i.CreatedById AS 'Subject.CreatedById',
				i.CreatedDate AS 'Subject.CreatedDate',
				i.ModifiedById AS 'Subject.ModifiedById',
				i.ModifiedDate AS 'Subject.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- Subjects UPDATE
GO
CREATE TRIGGER dbo.Subjects_Update_Audit ON dbo.Subjects
AFTER UPDATE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'SUBJECT',
		'UPDATE',
		CAST(i.ModifiedById AS VARCHAR(128)),
		COALESCE(i.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'Subject.Id',
				i.Code AS 'Subject.Code',
				i.[Name] AS 'Subject.Name',
				i.[Hours] AS 'Subject.Hours',
				i.[Days] AS 'Subject.Days',
				i.[Description] AS 'Subject.Description',
				i.Units AS 'Subject.Units',
				i.Semester AS 'Subject.Semester',
				i.AcademicYear AS 'Subject.AcademicYear',
				i.TrackAndStrand AS 'Subject.TrackAndStrand',
				i.CreatedById AS 'Subject.CreatedById',
				i.CreatedDate AS 'Subject.CreatedDate',
				i.ModifiedById AS 'Subject.ModifiedById',
				i.ModifiedDate AS 'Subject.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- Subjects DELETE
GO
CREATE TRIGGER dbo.Subjects_Delete_Audit ON dbo.Subjects
AFTER DELETE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(d.Id AS VARCHAR(128)),
		'SUBJECT',
		'DELETE',
		CAST(d.ModifiedById AS VARCHAR(128)),
		COALESCE(d.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				d.Id AS 'Subject.Id',
				d.Code AS 'Subject.Code',
				d.[Name] AS 'Subject.Name',
				d.[Hours] AS 'Subject.Hours',
				d.[Days] AS 'Subject.Days',
				d.[Description] AS 'Subject.Description',
				d.Units AS 'Subject.Units',
				d.Semester AS 'Subject.Semester',
				d.AcademicYear AS 'Subject.AcademicYear',
				d.TrackAndStrand AS 'Subject.TrackAndStrand',
				d.CreatedById AS 'Subject.CreatedById',
				d.CreatedDate AS 'Subject.CreatedDate',
				d.ModifiedById AS 'Subject.ModifiedById',
				d.ModifiedDate AS 'Subject.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		deleted AS d;
END;
-- ================================================================
-- ================================================================
-- ================================================================
-- UserAccounts INSERT
GO
CREATE TRIGGER dbo.UserAccounts_Insert_Audit ON dbo.UserAccounts
AFTER INSERT
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'USERACCOUNT',
		'INSERT',
		CAST(i.CreatedById AS VARCHAR(128)),
		COALESCE(i.CreatedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'UserAccount.Id',
				i.Username AS 'UserAccount.Username',
				i.PasswordHash AS 'UserAccount.PasswordHash',
				i.EmailAddress AS 'UserAccount.EmailAddress',
				i.LastLogin AS 'UserAccount.LastLogin',
				i.LoginRetries AS 'UserAccount.LoginRetries',
				i.IsActive AS 'UserAccount.IsActive',
				i.InactiveReason AS 'UserAccount.InactiveReason',
				i.CreatedById AS 'UserAccount.CreatedById',
				i.CreatedDate AS 'UserAccount.CreatedDate',
				i.ModifiedById AS 'UserAccount.ModifiedById',
				i.ModifiedDate AS 'UserAccount.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- UserAccounts UPDATE
GO
CREATE TRIGGER dbo.UserAccounts_Update_Audit ON dbo.UserAccounts
AFTER UPDATE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'USERACCOUNT',
		'UPDATE',
		CAST(i.ModifiedById AS VARCHAR(128)),
		COALESCE(i.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'UserAccount.Id',
				i.Username AS 'UserAccount.Username',
				i.PasswordHash AS 'UserAccount.PasswordHash',
				i.EmailAddress AS 'UserAccount.EmailAddress',
				i.LastLogin AS 'UserAccount.LastLogin',
				i.LoginRetries AS 'UserAccount.LoginRetries',
				i.IsActive AS 'UserAccount.IsActive',
				i.InactiveReason AS 'UserAccount.InactiveReason',
				i.CreatedById AS 'UserAccount.CreatedById',
				i.CreatedDate AS 'UserAccount.CreatedDate',
				i.ModifiedById AS 'UserAccount.ModifiedById',
				i.ModifiedDate AS 'UserAccount.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- UserAccounts DELETE
GO
CREATE TRIGGER dbo.UserAccounts_Delete_Audit ON dbo.UserAccounts
AFTER DELETE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(d.Id AS VARCHAR(128)),
		'USERACCOUNT',
		'DELETE',
		CAST(d.ModifiedById AS VARCHAR(128)),
		COALESCE(d.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				d.Id AS 'UserAccount.Id',
				d.Username AS 'UserAccount.Username',
				d.PasswordHash AS 'UserAccount.PasswordHash',
				d.EmailAddress AS 'UserAccount.EmailAddress',
				d.LastLogin AS 'UserAccount.LastLogin',
				d.LoginRetries AS 'UserAccount.LoginRetries',
				d.IsActive AS 'UserAccount.IsActive',
				d.InactiveReason AS 'UserAccount.InactiveReason',
				d.CreatedById AS 'UserAccount.CreatedById',
				d.CreatedDate AS 'UserAccount.CreatedDate',
				d.ModifiedById AS 'UserAccount.ModifiedById',
				d.ModifiedDate AS 'UserAccount.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		deleted AS d;
END;
-- ================================================================
-- ================================================================
-- ================================================================
-- Users INSERT
GO
CREATE TRIGGER dbo.Users_Insert_Audit ON dbo.Users
AFTER INSERT
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'USER',
		'INSERT',
		CAST(i.CreatedById AS VARCHAR(128)),
		COALESCE(i.CreatedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'User.Id',
				i.FirstName AS 'User.FirstName',
				i.MiddleName AS 'User.MiddleName',
				i.LastName AS 'User.LastName',
				i.ProfilePicture AS 'User.ProfilePicture',
				i.RoleType AS 'User.RoleType',
				i.UserAccountId AS 'User.UserAccountId',
				i.CreatedById AS 'User.CreatedById',
				i.CreatedDate AS 'User.CreatedDate',
				i.ModifiedById AS 'User.ModifiedById',
				i.ModifiedDate AS 'User.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- Users UPDATE
GO
CREATE TRIGGER dbo.Users_Update_Audit ON dbo.Users
AFTER UPDATE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'USER',
		'UPDATE',
		CAST(i.ModifiedById AS VARCHAR(128)),
		COALESCE(i.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'User.Id',
				i.FirstName AS 'User.FirstName',
				i.MiddleName AS 'User.MiddleName',
				i.LastName AS 'User.LastName',
				i.ProfilePicture AS 'User.ProfilePicture',
				i.RoleType AS 'User.RoleType',
				i.UserAccountId AS 'User.UserAccountId',
				i.CreatedById AS 'User.CreatedById',
				i.CreatedDate AS 'User.CreatedDate',
				i.ModifiedById AS 'User.ModifiedById',
				i.ModifiedDate AS 'User.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- Users DELETE
GO
CREATE TRIGGER dbo.Users_Delete_Audit ON dbo.Users
AFTER DELETE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(d.Id AS VARCHAR(128)),
		'USER',
		'DELETE',
		CAST(d.ModifiedById AS VARCHAR(128)),
		COALESCE(d.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				d.Id AS 'User.Id',
				d.FirstName AS 'User.FirstName',
				d.MiddleName AS 'User.MiddleName',
				d.LastName AS 'User.LastName',
				d.ProfilePicture AS 'User.ProfilePicture',
				d.RoleType AS 'User.RoleType',
				d.UserAccountId AS 'User.UserAccountId',
				d.CreatedById AS 'User.CreatedById',
				d.CreatedDate AS 'User.CreatedDate',
				d.ModifiedById AS 'User.ModifiedById',
				d.ModifiedDate AS 'User.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		deleted AS d;
END;
-- ===============================================================
-- ===============================================================
-- ===============================================================
-- ExternalAcademicRecords INSERT
GO
CREATE TRIGGER dbo.ExternalAcademicRecords_Insert_Audit ON dbo.ExternalAcademicRecords
AFTER INSERT
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'EXTERNALACADEMICRECORD',
		'INSERT',
		CAST(i.CreatedById AS VARCHAR(128)),
		COALESCE(i.CreatedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'ExternalAcademicRecord.Id',
				i.Rating AS 'ExternalAcademicRecord.Rating',
				i.SubjectName AS 'ExternalAcademicRecord.SubjectName',
				i.Semester AS 'ExternalAcademicRecord.Semester',
				i.AcademicYear AS 'ExternalAcademicRecord.AcademicYear',
				i.StudentId AS 'ExternalAcademicRecord.StudentId'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- ExternalAcademicRecords UPDATE
GO
CREATE TRIGGER dbo.ExternalAcademicRecords_Update_Audit ON dbo.ExternalAcademicRecords
AFTER UPDATE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'EXTERNALACADEMICRECORD',
		'UPDATE',
		CAST(i.ModifiedById AS VARCHAR(128)),
		COALESCE(i.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'ExternalAcademicRecord.Id',
				i.Rating AS 'ExternalAcademicRecord.Rating',
				i.SubjectName AS 'ExternalAcademicRecord.SubjectName',
				i.Semester AS 'ExternalAcademicRecord.Semester',
				i.AcademicYear AS 'ExternalAcademicRecord.AcademicYear',
				i.StudentId AS 'ExternalAcademicRecord.StudentId'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- ExternalAcademicRecords DELETE
GO
CREATE TRIGGER dbo.ExternalAcademicRecords_Delete_Audit ON dbo.ExternalAcademicRecords
AFTER DELETE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(d.Id AS VARCHAR(128)),
		'EXTERNALACADEMICRECORD',
		'DELETE',
		CAST(d.ModifiedById AS VARCHAR(128)),
		COALESCE(d.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				d.Id AS 'ExternalAcademicRecord.Id',
				d.Rating AS 'ExternalAcademicRecord.Rating',
				d.SubjectName AS 'ExternalAcademicRecord.SubjectName',
				d.Semester AS 'ExternalAcademicRecord.Semester',
				d.AcademicYear AS 'ExternalAcademicRecord.AcademicYear',
				d.StudentId AS 'ExternalAcademicRecord.StudentId'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		deleted AS d;
END;
-- ===============================================================
-- ===============================================================
-- ===============================================================
-- Settings INSERT
GO
CREATE TRIGGER dbo.Settings_Insert_Audit ON dbo.Settings
AFTER INSERT
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'SETTING',
		'INSERT',
		CAST(i.CreatedById AS VARCHAR(128)),
		COALESCE(i.CreatedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'Setting.Id',
				i.[Name] AS 'Setting.Name',
				i.[Value] AS 'Setting.Value',
				i.CreatedById AS 'Setting.CreatedById',
				i.CreatedDate AS 'Setting.CreatedDate',
				i.ModifiedById AS 'Setting.ModifiedById',
				i.ModifiedDate AS 'Setting.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- Settings UPDATE
GO
CREATE TRIGGER dbo.Settings_Update_Audit ON dbo.Settings
AFTER UPDATE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(i.Id AS VARCHAR(128)),
		'SETTING',
		'UPDATE',
		CAST(i.ModifiedById AS VARCHAR(128)),
		COALESCE(i.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				i.Id AS 'Setting.Id',
				i.[Name] AS 'Setting.Name',
				i.[Value] AS 'Setting.Value',
				i.CreatedById AS 'Setting.CreatedById',
				i.CreatedDate AS 'Setting.CreatedDate',
				i.ModifiedById AS 'Setting.ModifiedById',
				i.ModifiedDate AS 'Setting.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		inserted AS i;
END;
-- Settings DELETE
GO
CREATE TRIGGER dbo.Settings_Delete_Audit ON dbo.Settings
AFTER DELETE
AS
BEGIN
	INSERT INTO dbo.AuditLogs
	(
		Id,
		ObjectId,
		ObjectType,
		[Event],
		ActionById,
		OccurredOn,
		[Data]
	)
	SELECT
		NEWID(),
		CAST(d.Id AS VARCHAR(128)),
		'SETTING',
		'DELETE',
		CAST(d.ModifiedById AS VARCHAR(128)),
		COALESCE(d.ModifiedDate, GETUTCDATE()),
		(
			SELECT
				d.Id AS 'Setting.Id',
				d.[Name] AS 'Setting.Name',
				d.[Value] AS 'Setting.Value',
				d.CreatedById AS 'Setting.CreatedById',
				d.CreatedDate AS 'Setting.CreatedDate',
				d.ModifiedById AS 'Setting.ModifiedById',
				d.ModifiedDate AS 'Setting.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		deleted AS d;
END;
-- ===============================================================
-- ===============================================================
-- ===============================================================