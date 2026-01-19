

/*
/****** Script for  GetPatientsFollowUpListByDate.Sql ******/

DECLARE @compId INT = 1
DECLARE @followupdt DATE = '2025-12-31'

*/
----- Sql Starts Here

select 
		patsum.Id, 
		Patsum.ReceiptNo,
		patsum.MS_Patients_Id, 
		pat.FullName,
		CAST(pat.Mobile1 AS VARCHAR(20)) + '/' + CAST(pat.Mobile2 AS VARCHAR(20)) AS 'Mobile',
		pat.MS_Treament_Id,
		treatment.TreamentName,
		pat.AddressLine1,
		pat.AddressLine2,
		city.CityName,
		state.StateName,
		pat.PostalCode,
		patsum.VisitDate,
		pat.Remark,
		patsum.Notes,
		patsum.NextVisitDate,
		pat.MS_Reference_Id,
		ref.Reference,
		patsum.FollowUpDdate,
		ISNULL(patsum.Amount, 0) AS Amount
	from Patients_Summary AS patsum
		INNER JOIN MS_Patients AS pat ON pat.Id = patsum.MS_Patients_Id
		LEFT JOIN MS_City AS city ON city.Id = pat.MS_City_Id
		LEFT JOIN MS_State As state ON state.Id = pat.MS_State_Id
		LEFT JOIN MS_Reference AS ref ON ref.Id = pat.MS_Reference_Id
		LEFT JOIN MS_Treament AS treatment ON treatment.Id = pat.MS_Treament_Id
	WHERE CAST(FollowUpDdate AS DATE) = @followupdt
			 AND patsum.MS_Comp_Id = @compId
	ORDER BY FollowUpDdate asc