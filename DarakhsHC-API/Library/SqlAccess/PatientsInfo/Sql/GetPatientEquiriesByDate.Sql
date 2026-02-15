

/*

---GetPatientEquiriesByDate.Sql

DECLARE @MS_Comp_Id INT = 1
DECLARE @FromDate DATETIME2 = '2023-01-01'
DECLARE @ToDate   DATETIME2 = '2025-12-31'
*/

SELECT patEnq.Id
      ,Formdate
      ,MS_Comp_Id
      ,Name
      ,Mobile
      ,EnquiryFor
      ,Address
      ,patEnq.MS_Country_Id
	  ,country.CountryName
      ,patEnq.MS_State_Id
	  ,state.StateName
      ,patEnq.MS_City_Id
	  ,city.CityName
  FROM Patient_Enquiry AS patEnq
	LEFT JOIN MS_Country AS country ON country.Id = patEnq.MS_Country_Id
	LEFT JOIN MS_State AS state ON state.Id = patEnq.MS_State_Id
	LEFT JOIN MS_City AS city ON city.Id = patEnq.MS_City_Id
  WHERE patEnq.MS_Comp_Id = @MS_Comp_Id
  AND CAST(patEnq.Formdate AS DATE) BETWEEN CAST(@FromDate AS date) AND CAST(@ToDate AS date)
  AND NOT EXISTS (
        SELECT 1
        FROM Patients_Appointment appt
        WHERE appt.Patient_Enquiry_Id = patEnq.Id   -- ⚠️ change column if different
  )
ORDER BY patEnq.Formdate DESC;