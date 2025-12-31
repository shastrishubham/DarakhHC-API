 
 /*
 ---Parameters
DECLARE @Id	 INT = 6005;
DECLARE @MS_Comp_Id	INT = 1;
DECLARE @ReceiptNo	 NVARCHAR(50) = 'ReceiptNo';
DECLARE @MS_Patients_Id INT = 1;
DECLARE @VisitDate DATETIME = '2025-12-12';
DECLARE @Remark NVARCHAR(MAX) = 'Remark';
DECLARE @Notes NVARCHAR(MAX) = 'Notes';
DECLARE @NextVisitDate DATETIME = '2026-01-10';
DECLARE @IsFollowUpReq BIT = 1;
DECLARE @FollowUpDdate DATETIME = '2025-12-25';
*/

------------------------------------------------------------
-- CREATE TEMP TABLE FIRST
------------------------------------------------------------
IF OBJECT_ID('tempdb..#SummaryResult') IS NOT NULL
    DROP TABLE #SummaryResult;

CREATE TABLE #SummaryResult
(
    SummaryId INT,
    MS_Comp_Id INT,
    PatientsName NVARCHAR(200),
    Mobile NVARCHAR(50),
    NextVisitDate DATETIME
);

------------------------------------------------------------
-- EXTENDED SOURCE WITH PATIENT DETAILS
------------------------------------------------------------
;WITH SRC AS
(
    SELECT 
        @Id AS Id,
        @MS_Comp_Id AS MS_Comp_Id,
        @ReceiptNo AS ReceiptNo,
        @MS_Patients_Id AS MS_Patients_Id,
        @VisitDate AS VisitDate,
        @Remark AS Remark,
        @Notes AS Notes,
		NULLIF(@NextVisitDate, '1900-01-01') AS NextVisitDate,
        @IsFollowUpReq AS IsFollowUpReq,
		NULLIF(@FollowUpDdate, '1900-01-01') AS FollowUpDdate,
        P.FullName,
        P.Mobile1
    FROM MS_Patients P
    WHERE P.Id = @MS_Patients_Id
)

------------------------------------------------------------
-- MERGE Patients_Summary
------------------------------------------------------------
MERGE INTO Patients_Summary AS TARGET
USING SRC AS SOURCE
    ON TARGET.Id = SOURCE.Id

WHEN MATCHED THEN 
    UPDATE SET
          ReceiptNo         = SOURCE.ReceiptNo
        , MS_Patients_Id    = SOURCE.MS_Patients_Id
        , VisitDate         = ISNULL(SOURCE.VisitDate, TARGET.VisitDate)
        , Remark            = SOURCE.Remark
        , Notes             = SOURCE.Notes
        , NextVisitDate     = ISNULL(SOURCE.NextVisitDate, TARGET.NextVisitDate)
        , IsFollowUpReq     = SOURCE.IsFollowUpReq
        , FollowUpDdate     = ISNULL(SOURCE.FollowUpDdate, TARGET.FollowUpDdate)
        , FormDate          = GETDATE()

WHEN NOT MATCHED THEN
    INSERT (
          FormDate
        , MS_Comp_Id
        , ReceiptNo
        , MS_Patients_Id
        , VisitDate
        , Remark
        , Notes
        , NextVisitDate
        , IsFollowUpReq
        , FollowUpDdate
    )
    VALUES (
          GETDATE()
        , SOURCE.MS_Comp_Id
        , SOURCE.ReceiptNo
        , SOURCE.MS_Patients_Id
		,NULLIF(SOURCE.VisitDate, '1900-01-01')
        ,SOURCE.Remark
        , SOURCE.Notes
		,NULLIF(SOURCE.NextVisitDate, '1900-01-01')
        , SOURCE.IsFollowUpReq
        ,NULLIF(SOURCE.FollowUpDdate, '1900-01-01')
    )

------------------------------------------------------------
-- OUTPUT TO TEMP TABLE
------------------------------------------------------------
OUTPUT
      INSERTED.Id,
      INSERTED.MS_Comp_Id,
      SOURCE.FullName,
      SOURCE.Mobile1,
      INSERTED.NextVisitDate
INTO #SummaryResult (SummaryId, MS_Comp_Id, PatientsName, Mobile, NextVisitDate);

------------------------------------------------------------
-- MERGE Patients_History
------------------------------------------------------------
MERGE INTO Patients_History AS TARGET
USING
(
    SELECT
        R.MS_Comp_Id,
        @MS_Patients_Id AS MS_Patients_Id,
        R.SummaryId AS Patients_Summary_Id
    FROM #SummaryResult R
) AS SOURCE
ON TARGET.Patients_Summary_Id = SOURCE.Patients_Summary_Id

WHEN MATCHED THEN
    UPDATE SET
        TARGET.FormDate = GETDATE()   -- optional, update timestamp

WHEN NOT MATCHED THEN
    INSERT
    (
        FormDate,
        MS_Comp_Id,
        MS_Patients_Id,
        Patients_Summary_Id
    )
    VALUES
    (
        GETDATE(),
        SOURCE.MS_Comp_Id,
        SOURCE.MS_Patients_Id,
        SOURCE.Patients_Summary_Id
    );



------------------------------------------------------------
-- INSERT INTO Patients_Appointment IF Next Visit Date Valid
------------------------------------------------------------
INSERT INTO Patients_Appointment
(
    FormDate,
    MS_Comp_Id,
    PatientsName,
    EnquiryFor,
    Mobile,
    AppointmentDate
)
SELECT 
      GETDATE(),
      R.MS_Comp_Id,
      R.PatientsName,
      'Follow Up',
      R.Mobile,
      R.NextVisitDate
FROM #SummaryResult R
WHERE R.NextVisitDate IS NOT NULL
  AND R.NextVisitDate > '1900-01-01';

  SELECT SummaryId FROM #SummaryResult;
