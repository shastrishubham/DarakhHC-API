

/*
--UpsertReference.Sql

--select * from [dbo].[MS_Reference]

DECLARE @Id					INT				= 0
DECLARE @MS_Comp_Id			INT				= 1
DECLARE @Reference			NVARCHAR(MAX)	= 'ItemComp'

*/


MERGE INTO MS_Reference AS TARGET
USING (SELECT @Id as Id, @Reference as Reference) AS SOURCE
    ON (TARGET.Id = SOURCE.Id) 

WHEN MATCHED THEN
	UPDATE SET 
		Reference			=	@Reference

	WHEN NOT MATCHED THEN
        INSERT (
			 MS_Comp_Id
			,Reference
		   )
        VALUES
           (
			 @MS_Comp_Id
			,@Reference
			)

OUTPUT INSERTED.Id;