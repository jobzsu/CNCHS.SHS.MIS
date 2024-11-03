-- Drop AcademicRecords Triggers
IF OBJECT_ID('AcademicRecords_Insert_Audit', 'TR') IS NOT NULL DROP TRIGGER AcademicRecords_Insert_Audit;
IF OBJECT_ID('AcademicRecords_Update_Audit', 'TR') IS NOT NULL DROP TRIGGER AcademicRecords_Update_Audit;
IF OBJECT_ID('AcademicRecords_Delete_Audit', 'TR') IS NOT NULL DROP TRIGGER AcademicRecords_Delete_Audit;
-- Drop Departments Triggers
IF OBJECT_ID('Departments_Insert_Audit', 'TR') IS NOT NULL DROP TRIGGER Departments_Insert_Audit;
IF OBJECT_ID('Departments_Update_Audit', 'TR') IS NOT NULL DROP TRIGGER Departments_Update_Audit;
IF OBJECT_ID('Departments_Delete_Audit', 'TR') IS NOT NULL DROP TRIGGER Departments_Delete_Audit;
-- Drop InstructorInfos Triggers
IF OBJECT_ID('InstructorInfos_Insert_Audit', 'TR') IS NOT NULL DROP TRIGGER InstructorInfos_Insert_Audit;
IF OBJECT_ID('InstructorInfos_Update_Audit', 'TR') IS NOT NULL DROP TRIGGER InstructorInfos_Update_Audit;
IF OBJECT_ID('InstructorInfos_Delete_Audit', 'TR') IS NOT NULL DROP TRIGGER InstructorInfos_Delete_Audit;
-- Drop PreRequisites Triggers
IF OBJECT_ID('PreRequisites_Insert_Audit', 'TR') IS NOT NULL DROP TRIGGER PreRequisites_Insert_Audit;
IF OBJECT_ID('PreRequisites_Update_Audit', 'TR') IS NOT NULL DROP TRIGGER PreRequisites_Update_Audit;
IF OBJECT_ID('PreRequisites_Delete_Audit', 'TR') IS NOT NULL DROP TRIGGER PreRequisites_Delete_Audit;
-- Drop Schedules Triggers
IF OBJECT_ID('Schedules_Insert_Audit', 'TR') IS NOT NULL DROP TRIGGER Schedules_Insert_Audit;
IF OBJECT_ID('Schedules_Update_Audit', 'TR') IS NOT NULL DROP TRIGGER Schedules_Update_Audit;
IF OBJECT_ID('Schedules_Delete_Audit', 'TR') IS NOT NULL DROP TRIGGER Schedules_Delete_Audit;
-- Drop Sections Triggers
IF OBJECT_ID('Sections_Insert_Audit', 'TR') IS NOT NULL DROP TRIGGER Sections_Insert_Audit;
IF OBJECT_ID('Sections_Update_Audit', 'TR') IS NOT NULL DROP TRIGGER Sections_Update_Audit;
IF OBJECT_ID('Sections_Delete_Audit', 'TR') IS NOT NULL DROP TRIGGER Sections_Delete_Audit;
-- Drop StudentInfos Triggers
IF OBJECT_ID('StudentInfos_Insert_Audit', 'TR') IS NOT NULL DROP TRIGGER StudentInfos_Insert_Audit;
IF OBJECT_ID('StudentInfos_Update_Audit', 'TR') IS NOT NULL DROP TRIGGER StudentInfos_Update_Audit;
IF OBJECT_ID('StudentInfos_Delete_Audit', 'TR') IS NOT NULL DROP TRIGGER StudentInfos_Delete_Audit;
-- Drop StudentSchedules Triggers
IF OBJECT_ID('StudentSchedules_Insert_Audit', 'TR') IS NOT NULL DROP TRIGGER StudentSchedules_Insert_Audit;
IF OBJECT_ID('StudentSchedules_Update_Audit', 'TR') IS NOT NULL DROP TRIGGER StudentSchedules_Update_Audit;
IF OBJECT_ID('StudentSchedules_Delete_Audit', 'TR') IS NOT NULL DROP TRIGGER StudentSchedules_Delete_Audit;
-- Drop Subjects Triggers
IF OBJECT_ID('Subjects_Insert_Audit', 'TR') IS NOT NULL DROP TRIGGER Subjects_Insert_Audit;
IF OBJECT_ID('Subjects_Update_Audit', 'TR') IS NOT NULL DROP TRIGGER Subjects_Update_Audit;
IF OBJECT_ID('Subjects_Delete_Audit', 'TR') IS NOT NULL DROP TRIGGER Subjects_Delete_Audit;
-- Drop UserAccounts Triggers
IF OBJECT_ID('UserAccounts_Insert_Audit', 'TR') IS NOT NULL DROP TRIGGER UserAccounts_Insert_Audit;
IF OBJECT_ID('UserAccounts_Update_Audit', 'TR') IS NOT NULL DROP TRIGGER UserAccounts_Update_Audit;
IF OBJECT_ID('UserAccounts_Delete_Audit', 'TR') IS NOT NULL DROP TRIGGER UserAccounts_Delete_Audit;
-- Drop Users Triggers
IF OBJECT_ID('Users_Insert_Audit', 'TR') IS NOT NULL DROP TRIGGER Users_Insert_Audit;
IF OBJECT_ID('Users_Update_Audit', 'TR') IS NOT NULL DROP TRIGGER Users_Update_Audit;
IF OBJECT_ID('Users_Delete_Audit', 'TR') IS NOT NULL DROP TRIGGER Users_Delete_Audit;
-- Drop ExternalAcademicRecords Triggers
IF OBJECT_ID('ExternalAcademicRecords_Insert_Audit', 'TR') IS NOT NULL DROP TRIGGER ExternalAcademicRecords_Insert_Audit;
IF OBJECT_ID('ExternalAcademicRecords_Update_Audit', 'TR') IS NOT NULL DROP TRIGGER ExternalAcademicRecords_Update_Audit;
IF OBJECT_ID('ExternalAcademicRecords_Delete_Audit', 'TR') IS NOT NULL DROP TRIGGER ExternalAcademicRecords_Delete_Audit;
-- Drop Settings Triggers
IF OBJECT_ID('Settings_Insert_Audit', 'TR') IS NOT NULL DROP TRIGGER Settings_Insert_Audit;
IF OBJECT_ID('Settings_Update_Audit', 'TR') IS NOT NULL DROP TRIGGER Settings_Update_Audit;
IF OBJECT_ID('Settings_Delete_Audit', 'TR') IS NOT NULL DROP TRIGGER Settings_Delete_Audit;