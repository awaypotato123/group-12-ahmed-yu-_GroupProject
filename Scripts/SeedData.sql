

USE HealthcareDB;
GO


IF NOT EXISTS (SELECT 1 FROM Patients)
BEGIN
    PRINT 'Inserting 10 patients...';
    
    INSERT INTO Patients (FirstName, LastName, Email, PhoneNumber, DateOfBirth, Address, MedicalRecordNumber, CreatedAt, IsActive)
    VALUES
    ('John', 'Doe', 'john.doe@email.com', '555-0101', '1985-03-15', '123 Main St, Toronto ON', 'MRN001', GETDATE(), 1),
    ('Jane', 'Smith', 'jane.smith@email.com', '555-0102', '1990-07-22', '456 Oak Ave, Toronto ON', 'MRN002', GETDATE(), 1),
    ('Michael', 'Brown', 'michael.brown@email.com', '555-0103', '1978-11-30', '789 Pine Rd, Mississauga ON', 'MRN003', GETDATE(), 1),
    ('Emily', 'Johnson', 'emily.johnson@email.com', '555-0104', '1995-01-10', '321 Elm St, Brampton ON', 'MRN004', GETDATE(), 1),
    ('David', 'Williams', 'david.williams@email.com', '555-0105', '1982-05-18', '654 Maple Dr, Toronto ON', 'MRN005', GETDATE(), 1),
    ('Sarah', 'Garcia', 'sarah.garcia@email.com', '555-0106', '1988-09-25', '987 Cedar Ln, Oakville ON', 'MRN006', GETDATE(), 1),
    ('Robert', 'Martinez', 'robert.martinez@email.com', '555-0107', '1992-12-03', '147 Birch Ct, Toronto ON', 'MRN007', GETDATE(), 1),
    ('Lisa', 'Anderson', 'lisa.anderson@email.com', '555-0108', '1975-04-20', '258 Spruce Way, Markham ON', 'MRN008', GETDATE(), 1),
    ('James', 'Taylor', 'james.taylor@email.com', '555-0109', '1987-08-14', '369 Willow Blvd, Toronto ON', 'MRN009', GETDATE(), 1),
    ('Maria', 'Thomas', 'maria.thomas@email.com', '555-0110', '1993-02-28', '741 Ash St, Vaughan ON', 'MRN010', GETDATE(), 1);
    
    PRINT 'Successfully inserted 10 patients!';
END
ELSE
BEGIN
    PRINT 'Patients already exist. Skipping insertion.';
END
GO


SELECT COUNT(*) AS TotalPatients FROM Patients;
SELECT * FROM Patients;
GO

SELECT COUNT(*) AS TotalPatients FROM Patients;