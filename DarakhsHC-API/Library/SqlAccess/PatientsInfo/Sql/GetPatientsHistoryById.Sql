
/*

/****** Script for GetPatientsHistoryById.Sql  ******/

DECLARE @MS_Comp_Id INT = 1
DECLARE @MS_Patient_Id INT = 7007

*/


SELECT psummary.Id
      ,psummary.MS_Comp_Id
      ,psummary.FormDate
      ,psummary.ReceiptNo
      ,psummary.MS_Patients_Id
	  ,patients.FullName AS PatientsName
	  ,ISNULL(patients.Mobile1, patients.Mobile2) AS Mobile
	  ,ISNULL(patients.AddressLine1, patients.AddressLine2) + ' ' + city.CityName + ' ' + state.StateName AS Address
	  ,patients.PostalCode
	  ,psummary.VisitDate
      ,psummary.Remark
      ,psummary.Notes
      ,psummary.NextVisitDate
	  ,ISNULL(psummary.Amount, 0) AS 'Amount'
	  ,patients.Gender
  FROM Patients_Summary AS psummary
	INNER JOIN MS_Patients AS patients ON psummary.MS_Patients_Id = patients.Id 
	LEFT JOIN MS_Reference AS reference ON patients.MS_Reference_Id = reference.Id
	INNER JOIN MS_Treament AS treatment ON treatment.Id = patients.MS_Treament_Id
	LEFT JOIN MS_City AS city ON patients.MS_City_Id = city.Id
	LEFT JOIN MS_State AS state ON patients.MS_State_Id = state.Id
  WHERE psummary.MS_Comp_Id = @MS_Comp_Id
	AND patients.Id = @MS_Patient_Id
  ORDER BY psummary.FormDate DESC
