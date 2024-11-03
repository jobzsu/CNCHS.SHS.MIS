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