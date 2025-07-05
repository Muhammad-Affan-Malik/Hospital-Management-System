CREATE DATABASE HMS;

USE HMS;

CREATE TABLE Patients (
    PatientId INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Age INT NOT NULL,
    Gender VARCHAR(10),
    Disease VARCHAR(100),
    AdmissionDate DATE
);

SELECT * FROM Patients;
