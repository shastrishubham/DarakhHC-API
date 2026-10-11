/*
--usp_Dashboard_PatientStats.sql
--Used by api/PatientsInfo/GetDashboardStats (PatientsInfoAccess.GetDashboardStats)

EXEC dbo.usp_Dashboard_PatientStats @CompanyId = 1

Returns the same 6 result sets, column names and row order as before.
RESULT SET 3 used to join every patient of a month with every appointment of the same month
and then COUNT(DISTINCT ...) them; patients and appointments are now counted separately.
*/

ALTER PROCEDURE [dbo].[usp_Dashboard_PatientStats]
    @CompanyId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Now DATETIME = GETDATE();
    DECLARE @Today DATE = CAST(@Now AS DATE);
    DECLARE @Tomorrow DATE = DATEADD(DAY, 1, @Today);
    DECLARE @MonthStart DATE = DATEFROMPARTS(YEAR(@Now), MONTH(@Now), 1);
    DECLARE @NextMonthStart DATE = DATEADD(MONTH, 1, @MonthStart);
    -- First day of the oldest of the last 12 months
    DECLARE @RangeStart DATE = DATEADD(MONTH, -11, @MonthStart);

    DECLARE @TodaysAppointmentCount INT;
    DECLARE @TodaysPatientCount INT;
    DECLARE @ThisMonthPatientCount INT;
    DECLARE @TodaysWalkinCount INT;
    DECLARE @TodaysFollowUpCount INT;
    DECLARE @TodaysSummaryCount INT;

    SELECT @TodaysAppointmentCount = COUNT(*)
    FROM Patients_Appointment
    WHERE MS_Comp_Id = @CompanyId
      AND AppointmentDate >= @Today AND AppointmentDate < @Tomorrow;

    -- Today is always inside the current month, so one pass over this month's patients gives all three
    SELECT
        @ThisMonthPatientCount = COUNT(*),
        @TodaysPatientCount = ISNULL(SUM(CASE WHEN FormDate >= @Today AND FormDate < @Tomorrow THEN 1 ELSE 0 END), 0),
        @TodaysWalkinCount = ISNULL(SUM(CASE WHEN FormDate >= @Today AND FormDate < @Tomorrow
                                              AND ISNULL(Patients_Appointment_Id, 0) = 0 THEN 1 ELSE 0 END), 0)
    FROM MS_Patients
    WHERE MS_Comp_Id = @CompanyId
      AND FormDate >= @MonthStart AND FormDate < @NextMonthStart;

    SELECT @TodaysFollowUpCount = COUNT(*)
    FROM Patients_Summary
    WHERE MS_Comp_Id = @CompanyId
      AND CAST(FollowUpDdate AS DATE) = @Today;

    SELECT @TodaysSummaryCount = COUNT(*)
    FROM Patients_Summary
    WHERE MS_Comp_Id = @CompanyId
      AND FormDate >= @Today AND FormDate < @Tomorrow;

    -- Last 12 months, 0 = current month
    CREATE TABLE #Months
    (
        MonthOffset INT NOT NULL PRIMARY KEY,
        MonthStart DATE NOT NULL
    );

    INSERT INTO #Months (MonthOffset, MonthStart)
    SELECT V.N, DATEADD(MONTH, -V.N, @MonthStart)
    FROM (VALUES (0),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11)) AS V(N);

    /* =================================================
       RESULT SET 1: KPI CARDS
       ================================================= */
    SELECT
        @TodaysAppointmentCount AS TodaysAppointmentCount,
        @TodaysPatientCount AS TodaysPatientCount,
        @ThisMonthPatientCount AS ThisMonthPatientCount,
        @TodaysFollowUpCount AS TodaysFollowUpCount,
        @TodaysSummaryCount AS TodaysSummaryCount;



    /* =================================================
       RESULT SET 2: Hour-wise Patients Appointment
       ================================================= */
    SELECT
        RIGHT('0' + CAST(H.HourValue AS VARCHAR(2)), 2) + ':00' AS HourSlot,
        ISNULL(COUNT(PA.Id), 0) AS PatientCount
    FROM (VALUES (10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(21),(22)) AS H(HourValue)
    LEFT JOIN Patients_Appointment PA
        ON DATEPART(HOUR, PA.AppointmentDate) = H.HourValue
       AND PA.AppointmentDate >= @Today AND PA.AppointmentDate < @Tomorrow
       AND PA.MS_Comp_Id = @CompanyId
    GROUP BY H.HourValue
    ORDER BY H.HourValue;



    /* =================================================
       RESULT SET 3: Month-wise Patients (Last 12 Months)
       ================================================= */
    SELECT
        FORMAT(L.MonthStart, 'MMM yyyy') AS MonthName,
        MONTH(L.MonthStart) AS MonthNumber,
        YEAR(L.MonthStart) AS YearNumber,
        ISNULL(P.PatientCount, 0) AS PatientCount,
        ISNULL(A.AppointmentCount, 0) AS AppointmentCount
    FROM #Months L
    LEFT JOIN (
        SELECT DATEDIFF(MONTH, FormDate, @MonthStart) AS MonthOffset, COUNT(*) AS PatientCount
        FROM MS_Patients
        WHERE MS_Comp_Id = @CompanyId
          AND FormDate >= @RangeStart AND FormDate < @NextMonthStart
        GROUP BY DATEDIFF(MONTH, FormDate, @MonthStart)
    ) P ON P.MonthOffset = L.MonthOffset
    LEFT JOIN (
        SELECT DATEDIFF(MONTH, AppointmentDate, @MonthStart) AS MonthOffset, COUNT(*) AS AppointmentCount
        FROM Patients_Appointment
        WHERE MS_Comp_Id = @CompanyId
          AND AppointmentDate >= @RangeStart AND AppointmentDate < @NextMonthStart
        GROUP BY DATEDIFF(MONTH, AppointmentDate, @MonthStart)
    ) A ON A.MonthOffset = L.MonthOffset
    ORDER BY L.MonthOffset;



    /* =================================================
       RESULT SET 4: Appointment vs Walk-in
       ================================================= */
    SELECT PT.PatientType, PT.PatientCount
    FROM (VALUES (1, 'Appointment', @TodaysAppointmentCount),
                 (2, 'Walk-in', @TodaysWalkinCount)) AS PT(SortOrder, PatientType, PatientCount)
    ORDER BY PT.SortOrder;



    /* =================================================
       RESULT SET 5: Month-wise Treatment Counts (Dynamic)
       ================================================= */
    DECLARE @cols NVARCHAR(MAX)='';
    DECLARE @sql NVARCHAR(MAX)='';

    SELECT @cols = @cols +
        ', ISNULL(SUM(CASE WHEN MT.TreamentName = N''' + REPLACE(T.TreamentName, '''', '''''') + ''' THEN 1 END),0) AS [' + REPLACE(T.TreamentName, ']', ']]') + ']'
    FROM MS_Treament T;

    SET @sql = N'
    SELECT
        FORMAT(L.MonthStart, ''MMM yyyy'') AS MonthName,
        YEAR(L.MonthStart) AS YearNumber,
        MONTH(L.MonthStart) AS MonthNumber
        ' + @cols + N'
    FROM #Months L
    LEFT JOIN MS_Patients MP
        ON DATEDIFF(MONTH, MP.FormDate, @MonthStart) = L.MonthOffset
       AND MP.FormDate >= @RangeStart AND MP.FormDate < @NextMonthStart
       AND MP.MS_Comp_Id = @CompanyId
    LEFT JOIN MS_Treament MT
        ON MT.Id = MP.MS_Treament_Id
    GROUP BY L.MonthOffset, L.MonthStart
    ORDER BY L.MonthOffset;';

    EXEC sp_executesql @sql,
        N'@CompanyId INT, @MonthStart DATE, @RangeStart DATE, @NextMonthStart DATE',
        @CompanyId = @CompanyId, @MonthStart = @MonthStart, @RangeStart = @RangeStart, @NextMonthStart = @NextMonthStart;


    /* =================================================
       RESULT SET 6: Current Month Treatment-wise Count
       ================================================= */
    SELECT
        MT.TreamentName AS TreatmentType,
        ISNULL(COUNT(MP.Id), 0) AS PatientCount
    FROM MS_Treament MT
    LEFT JOIN MS_Patients MP
        ON MP.MS_Treament_Id = MT.Id
       AND MP.MS_Comp_Id = @CompanyId
       AND MP.FormDate >= @MonthStart AND MP.FormDate < @NextMonthStart
    GROUP BY MT.TreamentName
    ORDER BY MT.TreamentName;

END
