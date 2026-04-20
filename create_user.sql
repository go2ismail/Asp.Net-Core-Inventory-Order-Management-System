SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

USE [WHMS-LTE-FS]
GO

-- First, let's check if the user already exists
IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE Email = 'asd@gmail.com')
BEGIN
    -- Insert new user
    DECLARE @UserId NVARCHAR(450) = NEWID()
    DECLARE @SecurityStamp NVARCHAR(MAX) = NEWID()
    DECLARE @ConcurrencyStamp NVARCHAR(MAX) = NEWID()
    
    INSERT INTO AspNetUsers (
        Id, 
        UserName, 
        NormalizedUserName, 
        Email, 
        NormalizedEmail, 
        EmailConfirmed,
        PasswordHash, 
        SecurityStamp, 
        ConcurrencyStamp, 
        PhoneNumberConfirmed,
        TwoFactorEnabled, 
        LockoutEnabled, 
        AccessFailedCount,
        IsBlocked,
        IsDeleted,
        CreatedAt
    )
    VALUES (
        @UserId,
        'asd@gmail.com',
        'ASD@GMAIL.COM',
        'asd@gmail.com',
        'ASD@GMAIL.COM',
        1,
        'AQAAAAIAAYagAAAAEJ7hZ0Z7qJ9rKxJ0YXxF8g+8Q5K5mF2cP9rJ6nL3wM1vZ7tY8sK4pL9jH3dN2fV1wQ==',
        @SecurityStamp,
        @ConcurrencyStamp,
        0,
        0,
        0,
        0,
        0,
        0,
        GETUTCDATE()
    )
    PRINT 'User created successfully with ID: ' + CAST(@UserId AS NVARCHAR(450))
END
ELSE
BEGIN
    PRINT 'User already exists'
END
GO
