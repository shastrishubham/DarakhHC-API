
/*

/****** Script for GetPatientsAppointments.Sql  ******/

DECLARE @MS_Comp_Id INT = 1
DECLARE @FromDate DATETIME2 = '2023-01-01'
DECLARE @ToDate   DATETIME2 = '2025-12-31'

*/

SELECT 
      appt.Id,
      appt.FormDate,
      appt.MS_Comp_Id,
      appt.PatientsName,
      appt.EnquiryFor,
      appt.Mobile,
      appt.AppointmentDate,
      appt.MS_Reference_Id,
      ref.Reference AS ReferenceName,
      appt.Address
FROM Patients_Appointment AS appt
LEFT JOIN MS_Reference AS ref 
       ON appt.MS_Reference_Id = ref.Id
WHERE appt.MS_Comp_Id = @MS_Comp_Id
  AND CAST(appt.AppointmentDate AS DATE) BETWEEN CAST(@FromDate AS date) AND CAST(@ToDate AS date)
  AND NOT EXISTS (
        SELECT 1 
        FROM MS_Patients pat
        WHERE pat.Patients_Appointment_Id = appt.Id
          AND pat.Patients_Appointment_Id <> 0
  )
ORDER BY appt.AppointmentDate DESC;
