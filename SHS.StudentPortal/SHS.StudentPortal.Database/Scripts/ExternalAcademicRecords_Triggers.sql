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










-- Drop triggers rollback update
-- ===============================================================
-- ===============================================================
-- ===============================================================
-- Drop AcademicRecords Triggers
IF OBJECT_ID('ExternalAcademicRecords_Insert_Audit', 'TR') IS NOT NULL DROP TRIGGER ExternalAcademicRecords_Insert_Audit;
IF OBJECT_ID('ExternalAcademicRecords_Update_Audit', 'TR') IS NOT NULL DROP TRIGGER ExternalAcademicRecords_Update_Audit;
IF OBJECT_ID('ExternalAcademicRecords_Delete_Audit', 'TR') IS NOT NULL DROP TRIGGER ExternalAcademicRecords_Delete_Audit;
-- Drop Departments Triggers
-- ===============================================================
-- ===============================================================
-- ===============================================================