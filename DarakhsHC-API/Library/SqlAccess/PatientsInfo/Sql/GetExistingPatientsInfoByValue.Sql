

/*
/****** Script for  GetExistingPatientsInfoByValue.Sql ******/

DECLARE @MS_Comp_Id INT = 1
DECLARE @value nvarchar(max) = '123456789';

*/
----- Sql Starts Here
DECLARE @mob decimal(12,0) = -1;

Select @mob = CASE WHEN Isnumeric(@value) = 1
THEN CONVERT(DECIMAL(12,0),@value) 
ELSE -1 END 


SELECT 
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
	  ,patient.Gender
      ,patient.MS_Reference_Id
	  ,reference.Reference
      ,patient.MS_Treament_Id
	  ,treatment.TreamentName
      ,patient.MS_User_Id
	  ,users.FullName AS UserFullName
	  ,patient.Remark
	  ,patient.VisitDate
 FROM MS_Patients AS patient
	LEFT JOIN MS_City AS city ON patient.MS_City_Id = city.Id
	LEFT JOIN MS_State AS state ON patient.MS_State_Id = state.Id
	LEFT JOIN MS_Reference AS reference ON patient.MS_Reference_Id = reference.Id
	LEFT JOIN MS_Treament AS treatment ON patient.MS_Treament_Id = treatment.Id
	LEFT JOIN MS_User AS users ON patient.MS_User_Id = users.Id
  WHERE patient.MS_Comp_Id = @MS_Comp_Id
		AND patient.FName + patient.LName + patient.FullName + patient.AddressLine1 + patient.AddressLine2 LIKE '%'+@value+'%'
		OR patient.Mobile1 LIKE CONVERT(DECIMAL(12,0),replace(@mob, ',', '')) 
		OR patient.Mobile2 LIKE CONVERT(DECIMAL(12,0),replace(@mob, ',', '')) 
  ORDER BY patient.FormDate DESC