--CREATE DATABASE QuickCourt
--use QuickCourt

-- 1. Users Table
CREATE TABLE Users (
    userId INT IDENTITY(1,1) PRIMARY KEY,
    fullName NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) NOT NULL UNIQUE,
    passwordHash NVARCHAR(255) NOT NULL,
    phone NVARCHAR(20),
    avatarUrl NVARCHAR(255),
    role NVARCHAR(20) CHECK (role IN ('user', 'facilityOwner', 'admin')) NOT NULL DEFAULT 'user',
    isVerified BIT DEFAULT 0,
    isActive BIT DEFAULT 1,
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME DEFAULT GETDATE()
);

-- 2. OtpVerifications Table
CREATE TABLE OtpVerifications (
    otpId INT IDENTITY(1,1) PRIMARY KEY,
    userId INT NOT NULL FOREIGN KEY REFERENCES Users(userId) ON DELETE CASCADE,
    otpCode NVARCHAR(10) NOT NULL,
    purpose NVARCHAR(20) CHECK (purpose IN ('signup','login','resetPassword')) NOT NULL,
    expiresAt DATETIME NOT NULL,
    isUsed BIT DEFAULT 0,
    createdAt DATETIME DEFAULT GETDATE()
);

-- 3. Sports Table
CREATE TABLE Sports (
    sportId INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL UNIQUE,
    iconUrl NVARCHAR(255)
);

-- 4. Facilities Table
CREATE TABLE Facilities (
    facilityId INT IDENTITY(1,1) PRIMARY KEY,
    ownerId INT NOT NULL FOREIGN KEY REFERENCES Users(userId) ON DELETE CASCADE,
    name NVARCHAR(255) NOT NULL,
    description NVARCHAR(MAX),
    address NVARCHAR(MAX) NOT NULL,
    city NVARCHAR(100),
    state NVARCHAR(100),
    status NVARCHAR(20) CHECK (status IN ('pending','approved','rejected')) DEFAULT 'pending',
    rejectionReason NVARCHAR(MAX),
    operatingHoursStart TIME,
    operatingHoursEnd TIME,
    approvedBy INT NULL FOREIGN KEY REFERENCES Users(userId),
    approvalComments NVARCHAR(MAX),
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME DEFAULT GETDATE()
);

-- 5. Courts Table
CREATE TABLE Courts (
    courtId INT IDENTITY(1,1) PRIMARY KEY,
    facilityId INT NOT NULL FOREIGN KEY REFERENCES Facilities(facilityId) ON DELETE CASCADE,
    sportId INT NOT NULL FOREIGN KEY REFERENCES Sports(sportId),
    name NVARCHAR(100) NOT NULL,
    pricePerHour DECIMAL(10,2) CHECK (pricePerHour > 0) NOT NULL,
    capacity INT CHECK (capacity > 0) DEFAULT 1,
    isActive BIT DEFAULT 1,
    createdAt DATETIME DEFAULT GETDATE(),
    updatedAt DATETIME DEFAULT GETDATE()
);

-- 6. TimeSlots Table
CREATE TABLE TimeSlots (
    timeSlotId INT IDENTITY(1,1) PRIMARY KEY,
    courtId INT NOT NULL FOREIGN KEY REFERENCES Courts(courtId) ON DELETE CASCADE,
    slotDate DATE NOT NULL,
    startTime TIME NOT NULL,
    endTime TIME NOT NULL,
    status NVARCHAR(20) CHECK (status IN ('available','blocked','maintenance')) DEFAULT 'available'
);

-- 7. Bookings Table
CREATE TABLE Bookings (
    bookingId INT IDENTITY(1,1) PRIMARY KEY,
    userId INT NOT NULL FOREIGN KEY REFERENCES Users(userId) ON DELETE CASCADE,
    courtId INT NOT NULL FOREIGN KEY REFERENCES Courts(courtId),
    startTime DATETIME NOT NULL,
    endTime DATETIME NOT NULL,
    totalPrice DECIMAL(10,2) CHECK (totalPrice >= 0) NOT NULL,
    status NVARCHAR(20) CHECK (status IN ('confirmed','cancelled','completed')) DEFAULT 'confirmed',
    paymentStatus NVARCHAR(20) CHECK (paymentStatus IN ('pending','paid','failed')) DEFAULT 'pending',
    cancellationReason NVARCHAR(MAX),
    createdAt DATETIME DEFAULT GETDATE()
);

-- 8. Reviews Table
CREATE TABLE Reviews (
    reviewId INT IDENTITY(1,1) PRIMARY KEY,
	userId INT NOT NULL FOREIGN KEY REFERENCES Users(userId) ON DELETE NO ACTION,
    facilityId INT NOT NULL FOREIGN KEY REFERENCES Facilities(facilityId) ON DELETE CASCADE,
    rating INT CHECK (rating BETWEEN 1 AND 5),
    comment NVARCHAR(MAX),
    createdAt DATETIME DEFAULT GETDATE()
);

-- 9. Amenities Table
CREATE TABLE Amenities (
    amenityId INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL UNIQUE
);

-- 10. FacilityAmenities Table
CREATE TABLE FacilityAmenities (
    facilityId INT NOT NULL FOREIGN KEY REFERENCES Facilities(facilityId) ON DELETE CASCADE,
    amenityId INT NOT NULL FOREIGN KEY REFERENCES Amenities(amenityId) ON DELETE CASCADE,
    PRIMARY KEY (facilityId, amenityId)
);

-- 11. FacilityPhotos Table
CREATE TABLE FacilityPhotos (
    photoId INT IDENTITY(1,1) PRIMARY KEY,
    facilityId INT NOT NULL FOREIGN KEY REFERENCES Facilities(facilityId) ON DELETE CASCADE,
    photoUrl NVARCHAR(255) NOT NULL,
    caption NVARCHAR(255)
);


-- ================================
-- 1. Users
-- ================================
INSERT INTO Users (fullName, email, passwordHash, phone, avatarUrl, role, isVerified, isActive)
VALUES
('Nency Parmar', 'nency@example.com', 'hashedpass1', '9876543210', 'https://cdn.jsdelivr.net/gh/faker-js/assets-person-portrait/male/512/11.jpg', 'facilityOwner', 1, 1),
('Vishal Baraiya', 'vishal@example.com', 'hashedpass2', '9123456780', 'https://cdn.jsdelivr.net/gh/faker-js/assets-person-portrait/male/512/11.jpg', 'user', 1, 1),
('Darshi Kathrani', 'darshi@example.com', 'hashedpass3', '9988776655','https://cdn.jsdelivr.net/gh/faker-js/assets-person-portrait/male/512/11.jpg', 'admin', 1, 1);

-- ================================
-- 2. OtpVerifications
-- ================================
INSERT INTO OtpVerifications (userId, otpCode, purpose, expiresAt, isUsed)
VALUES
(1, '123456', 'signup', DATEADD(MINUTE, 10, GETDATE()), 0),
(2, '654321', 'resetPassword', DATEADD(MINUTE, 5, GETDATE()), 1);

-- ================================
-- 3. Sports
-- ================================
INSERT INTO Sports (name, iconUrl)
VALUES
('Badminton', 'https://cdn.jsdelivr.net/gh/faker-js/assets-person-portrait/male/512/11.jpg'),
('Tennis', 'https://cdn.jsdelivr.net/gh/faker-js/assets-person-portrait/male/512/11.jpg');

-- ================================
-- 4. Facilities
-- ================================
INSERT INTO Facilities (ownerId, name, description, address, city, state, status, operatingHoursStart, operatingHoursEnd)
VALUES
(1, 'City Sports Arena', 'Indoor and outdoor courts available', '123 Main Street', 'Ahmedabad', 'Gujarat', 'approved', '06:00', '22:00'),
(1, 'Sunrise Sports Complex', 'Premium outdoor tennis courts', '45 Sunrise Road', 'Surat', 'Gujarat', 'pending', '07:00', '21:00');

-- ================================
-- 5. Courts
-- ================================
INSERT INTO Courts (facilityId, sportId, name, pricePerHour, capacity, isActive)
VALUES
(1, 1, 'Indoor Badminton Court 1', 300.00, 4, 1),
(1, 2, 'Tennis Court A', 500.00, 4, 1),
(2, 2, 'Outdoor Tennis Court B', 400.00, 4, 1);

-- ================================
-- 6. TimeSlots
-- ================================
INSERT INTO TimeSlots (courtId, slotDate, startTime, endTime, status)
VALUES
(1, '2025-08-12', '06:00', '07:00', 'available'),
(1, '2025-08-12', '07:00', '08:00', 'blocked'),
(2, '2025-08-12', '06:00', '07:30', 'available');

-- ================================
-- 7. Bookings
-- ================================
INSERT INTO Bookings (userId, courtId, startTime, endTime, totalPrice, status, paymentStatus)
VALUES
(2, 1, '2025-08-12 06:00', '2025-08-12 07:00', 300.00, 'confirmed', 'paid'),
(2, 2, '2025-08-12 07:30', '2025-08-12 09:00', 750.00, 'confirmed', 'pending');

-- ================================
-- 8. Reviews
-- ================================
INSERT INTO Reviews (userId, facilityId, rating, comment)
VALUES
(2, 1, 5, 'Great place to play!'),
(2, 2, 4, 'Nice courts but a bit crowded.');

-- ================================
-- 9. Amenities
-- ================================
INSERT INTO Amenities (name)
VALUES
('Parking'),
('Changing Rooms'),
('Washrooms');

-- ================================
-- 10. FacilityAmenities
-- ================================
INSERT INTO FacilityAmenities (facilityId, amenityId)
VALUES
(1, 1),
(1, 2),
(2, 1),
(2, 3);

-- ================================
-- 11. FacilityPhotos
-- ================================
INSERT INTO FacilityPhotos (facilityId, photoUrl, caption)
VALUES
(1, 'https://cdn.jsdelivr.net/gh/faker-js/assets-person-portrait/male/512/11.jpg', 'Main indoor hall'),
(1, 'https://cdn.jsdelivr.net/gh/faker-js/assets-person-portrait/male/512/11.jpg', 'Outdoor area'),
(2, 'https://cdn.jsdelivr.net/gh/faker-js/assets-person-portrait/male/512/11.jpg', 'Tennis court front view');

-- Users
SELECT * FROM Users;

-- OtpVerifications
SELECT * FROM OtpVerifications;

-- Sports
SELECT * FROM Sports;

-- Facilities
SELECT * FROM Facilities;

-- Courts
SELECT * FROM Courts;

-- TimeSlots
SELECT * FROM TimeSlots;

-- Bookings
SELECT * FROM Bookings;

-- Reviews
SELECT * FROM Reviews;

-- Amenities
SELECT * FROM Amenities;

-- FacilityAmenities
SELECT * FROM FacilityAmenities;

-- FacilityPhotos
SELECT * FROM FacilityPhotos;
