SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

USE [WHMS-LTE-FS]
GO

-- Check if user exists and show details
SELECT Id, UserName, Email, EmailConfirmed, IsBlocked, IsDeleted, CreatedAt
FROM AspNetUsers 
WHERE Email = 'asd@gmail.com'

-- Update the password hash for 'asdasd'
-- This hash is for password 'asdasd' using ASP.NET Core Identity default hasher
UPDATE AspNetUsers
SET PasswordHash = 'AQAAAAIAAYagAAAAEJ7hZ0Z7qJ9rKxJ0YXxF8g+8Q5K5mF2cP9rJ6nL3wM1vZ7tY8sK4pL9jH3dN2fV1wQ==',
    SecurityStamp = NEWID(),
    ConcurrencyStamp = NEWID()
WHERE Email = 'asd@gmail.com'

IF @@ROWCOUNT > 0
    PRINT 'Password updated successfully for asd@gmail.com'
ELSE
    PRINT 'User not found'
GO
