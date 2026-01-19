


/*

--UpsertPatients.Sql

--select * from MS_Patients

DECLARE @Id							INT = 6005;
DECLARE @MS_Comp_Id					INT = 1;
DECLARE @Patients_Appointment_Id	INT = 0;
DECLARE @FName						NVARCHAR(MAX) = 'ABCD';
DECLARE @LName						NVARCHAR(MAX) = 'XYZ';
DECLARE @FullName					NVARCHAR(MAX) = 'ABCD XYZ';
DECLARE @DOB						DATE = CAST('2023-06-17' AS DATE)
DECLARE @Mobile1					DECIMAL(12,0) = 8454434339;
DECLARE @Mobile2					DECIMAL(12,0) = 9404524861;
DECLARE @AddressLine1				NVARCHAR(MAX) = 'Address 1';
DECLARE @AddressLine2				NVARCHAR(MAX) = 'Address 2';
DECLARE @MS_City_Id					INT = 1;
DECLARE @MS_State_Id				INT = 1;
DECLARE @PostalCode					DECIMAL(7,0) = 431001;
DECLARE @MaritialStatus				NVARCHAR(50) = 'Single'
DECLARE @Gender						CHAR(1) = 'M'
DECLARE @MS_Reference_Id			INT = 1;
DECLARE @MS_Treament_Id				INT = 1;
DECLARE @MS_User_Id					INT = 1;
DECLARE @Remark						NVARCHAR(MAX) = 'Remark';

*/

MERGE INTO MS_Patients AS TARGET
USING (SELECT @Id as Id, @FullName as FullName) AS SOURCE
    ON (TARGET.Id = SOURCE.Id) 

WHEN MATCHED THEN
	UPDATE SET 
	   FName				= @FName
      ,LName				= @LName
      ,FullName				= @FullName
      ,DOB					= @DOB
      ,Mobile1				= @Mobile1
      ,Mobile2				= @Mobile2
      ,AddressLine1			= @AddressLine1
      ,AddressLine2			= @AddressLine2
      ,MS_City_Id			= @MS_City_Id
      ,MS_State_Id			= @MS_State_Id
      ,PostalCode			= @PostalCode
      ,MaritialStatus		= @MaritialStatus
      ,MS_Reference_Id		= @MS_Reference_Id
      ,MS_Treament_Id		= @MS_Treament_Id
	  ,Remark				= @Remark
	  ,VisitDate			= GETDATE()
	  ,Gender				= @Gender


WHEN NOT MATCHED THEN
        INSERT (
			 FormDate
			,MS_Comp_Id
			,Patients_Appointment_Id
			,FName
			,LName
			,FullName
			,DOB
			,Mobile1
			,Mobile2
			,AddressLine1
			,AddressLine2
			,MS_City_Id
			,MS_State_Id
			,PostalCode
			,MaritialStatus
			,Gender
			,MS_Reference_Id
			,MS_Treament_Id
			,MS_User_Id
			,Remark
			,VisitDate
		   )
        VALUES
           (
			 GETDATE()
			,@MS_Comp_Id
			,@Patients_Appointment_Id
			,@FName
			,@LName
			,@FullName
			,@DOB
			,@Mobile1
			,@Mobile2
			,@AddressLine1
			,@AddressLine2
			,@MS_City_Id
			,@MS_State_Id
			,@PostalCode
			,@MaritialStatus
			,@Gender
			,@MS_Reference_Id
			,@MS_Treament_Id
			,@MS_User_Id
			,@Remark
			,GETDATE()
			)

OUTPUT INSERTED.Id;