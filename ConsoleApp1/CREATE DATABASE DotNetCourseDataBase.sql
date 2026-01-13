CREATE DATABASE DotNetCourseDataBase
GO

USE DotNetCourseDataBase
GO

CREATE SCHEMA TutorialAppSchema
GO

CREATE TABLE TutorialAppSchema.Computer
(
    Id INT PRIMARY KEY IDENTITY,
    Motherboard NVARCHAR(50) NOT NULL,
    CPUCores INT NOT NULL,
    HasWifi BIT NOT NULL,
    HasLTE DECIMAL(18, 4) NOT NULL,
    ReleaseDate DATETIME2 NOT NULL,
    Price DECIMAL(18, 4) NOT NULL,
    VideoCard NVARCHAR(50) NOT NULL 
)
GO

SELECT * FROM TutorialAppSchema.Computer

INSERT INTO TutorialAppSchema.Computer
(
    Motherboard,
    CPUCores,
    HasWifi,
    HasLTE,
    ReleaseDate,
    Price,
    VideoCard
) VALUES (
    'ASUS ROG STRIX B550-F GAMING', 
    8, 
    1, 
    1.5, 
    '2020-06-16 10:00:00', 
    1499.99, 
    'NVIDIA GeForce RTX 3080'
)

DELETE FROM TutorialAppSchema.Computer
WHERE Id = 3

SELECT 
  c.name AS ColumnName,
  t.name AS SqlType
FROM sys.columns c
JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID('TutorialAppSchema.Computer')
ORDER BY c.column_id;


ALTER TABLE TutorialAppSchema.Computer
ALTER COLUMN HasLTE bit NOT NULL;

USE DotNetCourseDatabase;
GO

SELECT  [UserId]
        , [FirstName]
        , [LastName]
        , [Email]
        , [Gender]
        , [Active]
  FROM  TutorialAppSchema.Users;

 SELECT *
  FROM  TutorialAppSchema.UserSalary;

 SELECT  [UserId]
      , [JobTitle]
       , [Department]
   FROM  TutorialAppSchema.UserJobInfo;


SELECT * FROM TutorialAppSchema.Users WHERE UserId = 10

INSERT INTO TutorialAppSchema.Users(
          [FirstName]
        , [LastName]
        , [Email]
        , [Gender]
        , [Active]
        ) VALUES (

        )

DELETE FROM TutorialAppSchema.Users
WHERE UserId = 1001;

CREATE TABLE TutorialAppSchema.Auth
(
    Email NVARCHAR(100),
    PasswordHash VARBINARY(MAX),
    PasswordSalt VARBINARY(MAX),
)

SELECT * FROM TutorialAppSchema.Auth WHERE Email = '';