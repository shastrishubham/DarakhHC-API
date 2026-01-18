


/*

--UpsertPatientEnquiry.Sql

--select * from Patient_Enquiry

DECLARE @Id							INT = 6005;
DECLARE @MS_Comp_Id					INT = 1;
DECLARE @Name						NVARCHAR(MAX) = 'ABCD';
DECLARE @Mobile					DECIMAL(12,0) = 8454434339;
DECLARE @EnquiryFor					NVARCHAR(MAX) = 'EnquiryFor';
DECLARE @Address				NVARCHAR(MAX) = 'Address';
DECLARE @MS_City_Id					INT = 1;
DECLARE @MS_State_Id				INT = 1;
DECLARE @MS_Country_Id			INT = 1;

*/



MERGE INTO Patient_Enquiry AS TARGET
USING (SELECT @Id as Id, @Name	as Name) AS SOURCE
    ON (TARGET.Id = SOURCE.Id) 

WHEN MATCHED THEN
	UPDATE SET 
	   Name				= @Name
      ,Mobile				= @Mobile
      ,EnquiryFor			= @EnquiryFor
      ,Address			= @Address
      ,MS_Country_Id			= @MS_Country_Id
      ,MS_State_Id			= @MS_State_Id
      ,MS_City_Id			= @MS_City_Id
	  ,MS_Comp_Id		= @MS_Comp_Id


WHEN NOT MATCHED THEN
        INSERT (
			 FormDate
			,MS_Comp_Id
			,Name
			,[Mobile]
			,EnquiryFor
			,Address
			,MS_City_Id
			,MS_State_Id
			,MS_Country_Id
		   )
        VALUES
           (
			 GETDATE()
			,@MS_Comp_Id
			,@Name
			,@Mobile
			,@EnquiryFor
			,@Address
			,@MS_City_Id
			,@MS_State_Id
			,@MS_Country_Id
			)

OUTPUT INSERTED.Id;