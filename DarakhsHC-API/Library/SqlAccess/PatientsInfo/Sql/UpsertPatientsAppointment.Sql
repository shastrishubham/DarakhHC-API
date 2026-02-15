
/*
--UpsertPatientsAppointment.Sql

--select * from Patients_Appointment

DECLARE @Id								INT = 1;
DECLARE @FormDate						DATETIME = GETDATE();
DECLARE @MS_Comp_Id						INT = 1;
DECLARE @Patient_Enquiry_Id				INT = 1;
DECLARE @PatientsName					NVARCHAR(MAX) = 'ABCD';
DECLARE @EnquiryFor						NVARCHAR(MAX) = 'ABCDE';
DECLARE @Mobile							DECIMAL(15,0) = 8454434339;
DECLARE @AppointmentDate				DATETIME = DATEADD(DAY,1, GETDATE());
DECLARE @MS_Reference_Id				INT = 1;
DECLARE @Address						NVARCHAR(MAX) = 'Address';

*/

IF(@AppointmentDate < GETDATE())
BEGIN
		PRINT 'Appointment Date cannot be less than todays date';
		RETURN;
END



MERGE INTO Patients_Appointment AS TARGET
USING (SELECT @Id as Id) AS SOURCE
    ON (TARGET.Id = SOURCE.Id) 

WHEN MATCHED THEN
	UPDATE SET 
	   PatientsName		= @PatientsName
      ,EnquiryFor		= @EnquiryFor
      ,Mobile			= @Mobile
      ,AppointmentDate	= @AppointmentDate
	  ,MS_Reference_Id	= @MS_Reference_Id
	  ,Address			= @Address


WHEN NOT MATCHED THEN
        INSERT (
			 FormDate
			,MS_Comp_Id
			,Patient_Enquiry_Id
			,PatientsName
			,EnquiryFor
			,Mobile
			,AppointmentDate
			,MS_Reference_Id
			,Address
		   )
        VALUES
           (
			 GETDATE()
			,@MS_Comp_Id
			,@Patient_Enquiry_Id
			,@PatientsName
			,@EnquiryFor
			,@Mobile
			,@AppointmentDate
			,@MS_Reference_Id
			,@Address
			)

OUTPUT INSERTED.Id;