

/*
--UpsertTreatment.Sql

--select * from [dbo].[MS_Treament]

DECLARE @Id					INT				= 0
DECLARE @MS_Comp_Id			INT				= 1
DECLARE @Reference			NVARCHAR(MAX)	= 'ItemComp'

*/


MERGE INTO MS_Treament AS TARGET
USING (SELECT @Id as Id, @TreamentName as TreamentName) AS SOURCE
    ON (TARGET.Id = SOURCE.Id) 

WHEN MATCHED THEN
	UPDATE SET 
		TreamentName			=	@TreamentName

	WHEN NOT MATCHED THEN
        INSERT (
			 MS_Comp_Id
			,TreamentName
		   )
        VALUES
           (
			 @MS_Comp_Id
			,@TreamentName
			)

OUTPUT INSERTED.Id;