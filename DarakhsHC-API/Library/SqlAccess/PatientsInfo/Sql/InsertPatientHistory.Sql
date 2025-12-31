

/*
---InsertPatientHistory.Sql

DECLARE @compId INT = 1
DECLARE @patientId INT = 1
DECLARE @patientSummaryId INT = 1

*/

INSERT INTO [dbo].[Patients_History]
           ([FormDate]
           ,[MS_Comp_Id]
           ,[MS_Patients_Id]
           ,[Patients_Summary_Id]
           ,[VisitDateTime])
     VALUES
           (GETDATE(), @compId, @patientId, @patientSummaryId, GETDATE())



