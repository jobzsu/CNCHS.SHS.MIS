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
				i.SubjectId AS 'AcademicRecord.SubjectId',
				i.OtherSubjectName AS 'AcademicRecord.OtherSubjectName',
				i.Semester AS 'AcademicRecord.Semester',
				i.AcademicYear AS 'AcademicRecord.AcademicYear',
				i.EncodedById AS 'AcademicRecord.EncodedById',
				i.EncodedDate AS 'AcademicRecord.EncodedDate',
				i.VerifiedById AS 'AcademicRecord.VerifiedById',
				i.VerifiedDate AS 'AcademicRecord.VerifiedDate',
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
				i.SubjectId AS 'AcademicRecord.SubjectId',
				i.OtherSubjectName AS 'AcademicRecord.OtherSubjectName',
				i.Semester AS 'AcademicRecord.Semester',
				i.AcademicYear AS 'AcademicRecord.AcademicYear',
				i.EncodedById AS 'AcademicRecord.EncodedById',
				i.EncodedDate AS 'AcademicRecord.EncodedDate',
				i.VerifiedById AS 'AcademicRecord.VerifiedById',
				i.VerifiedDate AS 'AcademicRecord.VerifiedDate',
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
				d.SubjectId AS 'AcademicRecord.SubjectId',
				d.OtherSubjectName AS 'AcademicRecord.OtherSubjectName',
				d.Semester AS 'AcademicRecord.Semester',
				d.AcademicYear AS 'AcademicRecord.AcademicYear',
				d.EncodedById AS 'AcademicRecord.EncodedById',
				d.EncodedDate AS 'AcademicRecord.EncodedDate',
				d.VerifiedById AS 'AcademicRecord.VerifiedById',
				d.VerifiedDate AS 'AcademicRecord.VerifiedDate',
				d.CreatedById AS 'AcademicRecord.CreatedById',
				d.CreatedDate AS 'AcademicRecord.CreatedDate',
				d.ModifiedById AS 'AcademicRecord.ModifiedById',
				d.ModifiedDate AS 'AcademicRecord.ModifiedDate'
			FOR JSON PATH, INCLUDE_NULL_VALUES
		)
	FROM
		deleted AS d;
END;