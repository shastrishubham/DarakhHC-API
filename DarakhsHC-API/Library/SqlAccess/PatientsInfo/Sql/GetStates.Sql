/****** Script for  GetStates.Sql ******/

SELECT Id
      ,StateName
      ,StateCode
      ,MS_Country_Id
  FROM MS_State
	WHERE MS_Country_Id = 1
	ORDER BY StateName ASC