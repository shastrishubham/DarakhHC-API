

--HasPatientExists.Sql
/*
DECLARE @MS_Comp_Id INT = 1
DECLARE @Name NVARCHAR(200) = 'John Doe';
DECLARE @Mobile NVARCHAR(20) = '78946612345';
*/

SELECT 
    CASE 
        WHEN EXISTS (
            SELECT 1 
            FROM MS_Patients
            WHERE MS_Comp_Id = @MS_Comp_Id 
				AND (
                    -- Mobile check only if @Mobile is valid
                    (@Mobile IS NOT NULL AND LTRIM(RTRIM(@Mobile)) <> '' AND @Mobile <> '0'
                        AND (Mobile1 = @Mobile OR Mobile2 = @Mobile)
                    )
                )
                OR 
                (
                    -- Name check always works
                    REPLACE(LOWER(FullName), ' ', '') = REPLACE(LOWER(@Name), ' ', '')
                )
        ) 
        THEN CAST(1 AS BIT)
        ELSE CAST(0 AS BIT)
    END AS IsExists;
