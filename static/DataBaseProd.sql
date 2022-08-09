USE Prod;
GO;

-- Deletes table if it exists
DROP TABLE IF EXISTS [dbo].Drivers
GO;
DROP TABLE IF EXISTS [dbo].Cars
GO;
DROP TABLE IF EXISTS [dbo].Rentals
GO;

-- Deletes procedure if it exists
DROP PROCEDURE IF EXISTS RentalsInformation;
GO;
DROP PROCEDURE IF EXISTS MakeNewRental;
GO;
DROP PROCEDURE IF EXISTS UpdateCarReturn;
GO;
DROP PROCEDURE IF EXISTS DeleteRentals;
GO;


IF OBJECT_ID('[dbo].Drivers', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].Drivers(
        Id INTEGER NOT NULL IDENTITY (1, 1) PRIMARY KEY,
        Name VARCHAR(50) NOT NULL,
        Surname VARCHAR(50) NOT NULL,
        Email VARCHAR(100),
        PhoneNumber VARCHAR(50) NOT NULL,
    );
END;
IF OBJECT_ID('[dbo].Drivers', 'U') IS NOT NULL
BEGIN
    INSERT INTO [dbo].Drivers(Name, Surname, Email, PhoneNumber) VALUES('John', 'Doe', '', '+380991234567');
    INSERT INTO [dbo].Drivers(Name, Surname, Email, PhoneNumber) VALUES('Jane', 'Ff', '', '+380991234567');
    INSERT INTO [dbo].Drivers(Name, Surname, Email, PhoneNumber) VALUES('Drake', 'Ss', '', '+380991234567');
    INSERT INTO [dbo].Drivers(Name, Surname, Email, PhoneNumber) VALUES('Sussie', 'Doe', '', '+380991234567');
    INSERT INTO [dbo].Drivers(Name, Surname, Email, PhoneNumber) VALUES('Jj', 'Aa', '', '+380991234567');
END;

IF OBJECT_ID('[dbo].Cars', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].Cars(
        Id INTEGER NOT NULL IDENTITY (1, 1) PRIMARY KEY,
        Make VARCHAR(50) NOT NULL,
        Model VARCHAR(50) NOT NULL,
        RegistrationNumber VARCHAR(50) NOT NULL,
    );
END;
IF OBJECT_ID('[dbo].Cars', 'U') IS NOT NULL
BEGIN
    INSERT INTO [dbo].Cars(Make, Model, RegistrationNumber) VALUES('BMW', 'X5', 'AA-AA-AA');
    INSERT INTO [dbo].Cars(Make, Model, RegistrationNumber) VALUES('Audi', 'A4', 'BB-BB-BB');
    INSERT INTO [dbo].Cars(Make, Model, RegistrationNumber) VALUES('Mercedes', 'C class', 'CC-CC-CC');
    INSERT INTO [dbo].Cars(Make, Model, RegistrationNumber) VALUES('Volvo', 'S60', 'DD-DD-DD');
    INSERT INTO [dbo].Cars(Make, Model, RegistrationNumber) VALUES('VW', 'Golf', 'EE-EE-EE');
    INSERT INTO [dbo].Cars(Make, Model, RegistrationNumber) VALUES('Audi', 'A6', 'FF-FF-FF');
    INSERT INTO [dbo].Cars(Make, Model, RegistrationNumber) VALUES('BMW', 'X6', 'GG-GG-GG');
END;

IF OBJECT_ID('[dbo].Rentals', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].Rentals(
        Id INTEGER NOT NULL IDENTITY (1, 1) PRIMARY KEY,
        CarId INTEGER NOT NULL FOREIGN KEY REFERENCES [dbo].Cars(Id),
        DriverId INTEGER NOT NULL FOREIGN KEY REFERENCES [dbo].Drivers(Id),
        RentDate DATE NOT NULL,
        ReturnDate DATE,
        Comments VARCHAR(MAX)
    );
END;
IF OBJECT_ID('[dbo].Rentals', 'U') IS NOT NULL
BEGIN
    INSERT INTO [dbo].Rentals(CarId, DriverId, RentDate, ReturnDate, Comments) VALUES(1, 1, '2019-01-01', '2019-01-02', 'Test');
    INSERT INTO [dbo].Rentals(CarId, DriverId, RentDate, ReturnDate, Comments) VALUES(2, 2, '2019-01-01', '2019-01-02', 'Another test');
    INSERT INTO [dbo].Rentals(CarId, DriverId, RentDate, ReturnDate, Comments) VALUES(3, 3, '2019-01-01', '2019-01-02', 'Third test');
    INSERT INTO [dbo].Rentals(CarId, DriverId, RentDate, ReturnDate, Comments) VALUES(4, 4, '2019-01-01', NULL, '');
    INSERT INTO [dbo].Rentals(CarId, DriverId, RentDate, ReturnDate, Comments) VALUES(5, 5, '2019-01-01', NULL, '');
END;

-- Collects all rentals information
IF OBJECT_ID('[dbo].RentalsInformation', 'P}') IS NULL
CREATE PROCEDURE RentalsInformation
AS
BEGIN
    SELECT
        c.RegistrationNumber,
        c.Make,
        c.Model,
        d.Name as DriverName,
        d.Surname as DriverSurname,
        r.RentDate,
        r.ReturnDate
    FROM [dbo].Rentals r, [dbo].Cars c, [dbo].Drivers d
    WHERE r.CarId = c.Id AND r.DriverId = d.Id
END;

-- Check if Car is available for rent
IF OBJECT_ID('[dbo].MakeNewRental', 'P') IS NULL
BEGIN
    CREATE PROCEDURE MakeNewRental
    (
        @CarId INT,
        @DriverId INT,
        @RentDate DATE,
        @Comments VARCHAR(MAX)
    )
    AS
    BEGIN
        IF NOT EXISTS(SELECT * FROM [dbo].Rentals WHERE CarId = @CarId AND ReturnDate IS NULL)
        BEGIN
            INSERT INTO [dbo].Rentals(CarId, DriverId, RentDate, Comments) VALUES(@CarId, @DriverId, @RentDate, @Comments);
        END
        ELSE
        BEGIN
            RAISERROR('Car is already rented', 16, 1);
        END;
    END;
END;

-- Returns car
IF OBJECT_ID('[dbo].UpdateCarReturn', 'P') IS NULL
BEGIN
    CREATE PROCEDURE UpdateCarReturn
    (
        @CarId INT,
        @ReturnDate DATE,
        @Comments VARCHAR(MAX)
    )
    AS
    BEGIN
        IF EXISTS(SELECT * FROM [dbo].Rentals WHERE CarId = @CarId AND ReturnDate IS NULL)
        BEGIN
            UPDATE [dbo].Rentals SET ReturnDate = @ReturnDate, Comments = @Comments WHERE CarId = @CarId AND ReturnDate IS NULL;
        END
        ELSE
        BEGIN
            RAISERROR('Car is not rented', 16, 1);
        END;
    END;
END;

-- Deletes rental
IF OBJECT_ID('[dbo].DeleteRentals', 'P') IS NULL
BEGIN
    CREATE PROCEDURE DeleteRentals
    (
        @Id INT
    )
    AS
    BEGIN
        IF EXISTS(SELECT * FROM [dbo].Rentals WHERE Id = @Id)
        BEGIN
            DELETE FROM [dbo].Rentals WHERE Id = @Id;
        END
        ELSE
        BEGIN
            RAISERROR('No rental with this id was found', 16, 1);
        END;
    END;
END;