CREATE DATABASE PropertyRentalManagement;
GO

USE PropertyRentalManagement;
GO

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName VARCHAR(50) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Property Owner', 'Property Manager', 'Administrator', 'Potential Tenant')),
	Preferences NVARCHAR(MAX),
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PhoneNumber NVARCHAR(20),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Table: Buildings
CREATE TABLE Buildings (
    BuildingId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    PropertyManagerId INT NOT NULL,
	ImageURL NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (PropertyManagerId) REFERENCES Users(UserId) ON DELETE CASCADE
);

-- Table: Apartments
CREATE TABLE Apartments (
    ApartmentId INT IDENTITY(1,1) PRIMARY KEY,
    BuildingId INT NOT NULL,
    UnitNumber NVARCHAR(20) NOT NULL,
    RentAmount INT NOT NULL,
    Bedrooms INT NOT NULL,
    Bathrooms INT NOT NULL,
	LivingRoom INT NOT NULL,
	ElevatorAccess BIT NOT NULL,
    SquareFootage INT NOT NULL,
    Status NVARCHAR(50) NOT NULL CHECK (Status IN ('Available', 'Occupied', 'Maintenance')) DEFAULT 'Available',
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BuildingId) REFERENCES Buildings(BuildingId) ON DELETE CASCADE
);

-- Table: Appointments
CREATE TABLE Appointments (
    AppointmentId INT IDENTITY(1,1) PRIMARY KEY,
    TenantId INT NOT NULL,
    ApartmentId INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    Status NVARCHAR(50) NOT NULL CHECK (Status IN ('Scheduled', 'Completed', 'Cancelled')) DEFAULT 'Scheduled',
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (TenantId) REFERENCES Users(UserId) ON DELETE NO ACTION,
    FOREIGN KEY (ApartmentId) REFERENCES Apartments(ApartmentId) ON DELETE NO ACTION
);

-- Table: Messages
CREATE TABLE Messages (
    MessageId INT IDENTITY(1,1) PRIMARY KEY,
    SenderId INT,
    ReceiverId INT,
    Content NVARCHAR(MAX) NOT NULL,
    SentAt DATETIME DEFAULT GETDATE(),
	[Read] BIT NOT NULL,
    FOREIGN KEY (SenderId) REFERENCES Users(UserId) ON DELETE NO ACTION,
    FOREIGN KEY (ReceiverId) REFERENCES Users(UserId) ON DELETE NO ACTION
);

-- Table: Reports
CREATE TABLE Reports (
    ReportId INT IDENTITY(1,1) PRIMARY KEY,
    EventDescription NVARCHAR(MAX) NOT NULL,
    PropertyManagerId INT NOT NULL,
    PropertyOwnerId INT NOT NULL,
    ReportedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (PropertyManagerId) REFERENCES Users(UserId) ON DELETE NO ACTION,
    FOREIGN KEY (PropertyOwnerId) REFERENCES Users(UserId) ON DELETE CASCADE
);

INSERT INTO Users (FullName, Password, Role, Email, PhoneNumber, CreatedAt)
VALUES 
('Junyi', '123456', 'Administrator', 'junyi@junyi.com', '123-456-7890', GETDATE());
