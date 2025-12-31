

/*
/****** Script for SelectTopNRows command from SSMS  ******/

DECLARE @MS_State_Id INT = 1

*/

SELECT city.Id
      ,city.CityName
      ,city.CityCode
      ,city.MS_State_Id
	  ,state.StateName
      ,city.MS_Country_Id
  FROM MS_City AS city
	INNER JOIN MS_State AS state ON city.MS_State_Id = state.Id
	WHERE city.MS_State_Id = @MS_State_Id
	ORDER BY city.CityName ASC