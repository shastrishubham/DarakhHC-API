
/*

/****** Script for Getreatments.Sql  ******/

DECLARE @MS_Comp_Id INT = 1

*/

SELECT Id
      ,MS_Comp_Id
      ,TreamentName
  FROM MS_Treament
  WHERE MS_Comp_Id = @MS_Comp_Id
  ORDER BY TreamentName ASC