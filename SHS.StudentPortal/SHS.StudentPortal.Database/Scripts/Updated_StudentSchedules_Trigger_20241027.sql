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