

/*
---CheckSameDayAppointment.Sql

DECLARE @PatientsName NVARCHAR(200) = 'Chaitrali Patil ';
DECLARE @Mobile NVARCHAR(20) = '7507282372';
DECLARE @AppointmentDate DATETIME = '2025-12-13 14:30:00';
DECLARE @CompId INT = 1;

*/

SELECT 
    CASE 
        WHEN EXISTS (
            SELECT 1
            FROM Patients_Appointment
            WHERE MS_Comp_Id = @CompId
			  AND PatientsName = @PatientsName
              AND Mobile = @Mobile
              AND CAST(AppointmentDate AS DATE) = CAST(@AppointmentDate AS DATE)
        )
        THEN 1
        ELSE 0
    END AS IsSameDayAppointmentExists;
