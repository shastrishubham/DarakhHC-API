
/*

/****** Script for GetReferences.Sql ******/

DECLARE @MS_Comp_Id INT = 1;

*/

SELECT Id
	  ,MS_Comp_Id
      ,Reference
  FROM MS_Reference
	WHERE MS_Comp_Id = @MS_Comp_Id
	ORDER BY Reference ASC
	