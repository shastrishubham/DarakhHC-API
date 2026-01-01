/****** Script for SelectTopNRows command from SSMS  ******/
/*
DECLARE @compId INT = 1;
DECLARE @FromDate DATETIME2 = '2025-12-06'
DECLARE @ToDate   DATETIME2 = '2025-12-06'
*/


SELECT patSumm.Id
      ,patSumm.MS_Comp_Id
      ,patSumm.FormDate
      ,patSumm.ReceiptNo
      ,patSumm.MS_Patients_Id
	  ,pat.FullName
	  ,CAST(pat.Mobile1 AS VARCHAR(20)) + '/' + CAST(pat.Mobile2 AS VARCHAR(20)) AS 'Mobile'
	  ,pat.MS_Treament_Id
	  ,treat.TreamentName
	  ,pat.AddressLine1
	  ,pat.AddressLine2
	  ,pat.PostalCode
	  ,pat.MS_State_Id
	  ,ste.StateName
	  ,pat.MS_City_Id
	  ,city.CityName
      ,patSumm.VisitDate
      ,patSumm.Remark
      ,patSumm.Notes
      ,patSumm.NextVisitDate
      ,patSumm.IsFollowUpReq
      ,patSumm.FollowUpDdate
  FROM Patients_Summary AS patSumm
	INNER JOIN MS_Patients AS pat ON pat.Id = patSumm.MS_Patients_Id
	INNER JOIN MS_Treament AS treat ON pat.MS_Treament_Id = treat.Id
	LEFT JOIN MS_Reference AS ref ON ref.Id = pat.MS_Reference_Id
	LEFT JOIN MS_State AS ste ON ste.Id = pat.MS_State_Id
	LEFT JOIN MS_City AS city ON city.Id = pat.MS_City_Id
	WHERE patSumm.MS_Comp_Id = @compId
		AND CAST(patSumm.VisitDate AS DATE) BETWEEN CAST(@FromDate AS date) AND CAST(@ToDate AS date)
