
/*

/****** Script for  GetPatientsInfoByDate.Sql ******/

DECLARE @MS_Comp_Id INT = 1
DECLARE @currentDate DATETIME = GETDATE()

*/

SELECT DISTINCT
	   patient.Id
      ,patient.FormDate
	  ,patient.MS_Comp_Id
      ,patient.Patients_Appointment_Id
      ,patient.FName
      ,patient.LName
      ,patient.FullName
      ,patient.DOB
      ,patient.Mobile1
      ,patient.Mobile2
      ,patient.AddressLine1
      ,patient.AddressLine2
      ,patient.MS_City_Id
	  ,city.CityName
      ,patient.MS_State_Id
	  ,state.StateName
      ,patient.PostalCode
      ,patient.MaritialStatus
      ,patient.MS_Reference_Id
	  ,reference.Reference
      ,patient.MS_Treament_Id
	  ,treatment.TreamentName
      ,patient.MS_User_Id
	  ,users.FullName AS UserFullName
	 ,CASE 
			WHEN summary.Id IS NULL THEN 0
			ELSE 1
		END as IsSummaryGenerated
 FROM MS_Patients AS patient
	LEFT JOIN MS_City AS city ON patient.MS_City_Id = city.Id
	LEFT JOIN MS_State AS state ON patient.MS_State_Id = state.Id
	LEFT JOIN MS_Reference AS reference ON patient.MS_Reference_Id = reference.Id
	LEFT JOIN MS_Treament AS treatment ON patient.MS_Treament_Id = treatment.Id
	LEFT JOIN MS_User AS users ON patient.MS_User_Id = users.Id
	LEFT OUTER JOIN Patients_Summary AS summary ON patient.Id = summary.MS_Patients_Id AND CAST(summary.FormDate AS DATE) = CAST(@currentDate AS DATE)
  WHERE patient.MS_Comp_Id = @MS_Comp_Id
	AND CAST(patient.FormDate AS DATE) = CAST(@currentDate AS DATE)
 ORDER BY patient.FormDate DESC

