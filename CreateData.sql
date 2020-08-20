USE PerformanceReview;

-- Drop Tables
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS EmployeeReviews;
DROP TABLE IF EXISTS Employees;
DROP TABLE IF EXISTS Roles;

-- Employees
CREATE TABLE Employees(
    EmployeeId int NOT NULL UNIQUE
	,Email varchar(100) NOT NULL UNIQUE
	,FirstName varchar(35) NOT NULL
	,LastName varchar(35) NOT NULL
	,ModifiedBy varchar(35) NULL
	,ModifiedDate Datetime2 NULL
);

CREATE UNIQUE INDEX IDX_Employees_EmployeeId_Email ON Employees (EmployeeId ASC, Email ASC);

INSERT INTO Employees (EmployeeId, Email, FirstName, LastName, ModifiedBy, ModifiedDate) VALUES (65301, 'john.doe@example.co.jp', 'John', 'Doe', 'test', GETDATE());
INSERT INTO Employees (EmployeeId, Email, FirstName, LastName, ModifiedBy, ModifiedDate) VALUES (65302, 'jane.doe@example.co.jp', 'Jane', 'Doe', 'test', GETDATE());
INSERT INTO Employees (EmployeeId, Email, FirstName, LastName, ModifiedBy, ModifiedDate) VALUES (65303, 'yamada.taro@example.co.jp', 'Yamada', 'Taro', 'test', GETDATE());
INSERT INTO Employees (EmployeeId, Email, FirstName, LastName, ModifiedBy, ModifiedDate) VALUES (65304, 'yamada.hanako@example.co.jp', 'Yamada', 'Hanako', 'test', GETDATE());

-- Roles
CREATE TABLE Roles(
	RoleId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	RoleName varchar(25) NOT NULL
);

INSERT INTO Roles (RoleName) VALUES ('user');
INSERT INTO Roles (RoleName) VALUES ('admin');

-- Users
CREATE TABLE Users(
	UserId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY 
	,EmployeeId int NULL
	,EncodedKey varchar(500) NOT NULL
	,EncodedSalt varchar(500) NOT NULL
	,RoleId int NOT NULL
	,Username varchar(36) NOT NULL UNIQUE
	,ModifiedBy varchar(35) NULL
	,ModifiedDate Datetime2 NULL
	, CONSTRAINT FK_Users_Employees_EmployeeId FOREIGN KEY (EmployeeId) REFERENCES Employees (EmployeeId)
	, CONSTRAINT FK_Users_Roles_RoleId FOREIGN KEY (RoleId) REFERENCES Roles (RoleId)
);

CREATE UNIQUE INDEX IDX_Users_Email_Password_Salt ON Users (EmployeeId ASC, EncodedKey ASC, EncodedSalt ASC);

INSERT INTO Users (UserId, EmployeeId, EncodedKey, EncodedSalt, RoleId, Username, ModifiedBy, ModifiedDate)
VALUES (
'a07f1b7e-5270-466a-aec4-afc76600997c'
,null
, 'FKRMusrabjxGYs6ECld3g+q10E2zfQ920QM2mIv2tv3zlWTzQGpZnnuxPEIwli38PAkYVr+88aqNIU9KRXGGA/OCFcIqWW9fxpYL/fcOfa5l5zlFH8c3dFeJ+zk3nhMFXmjgeP3x+RKOcHz5BMnwRGAAmjCmpGIVO3F9h/zhX7RTJ4kPY+TeQayK/y/Ji11tVEks3ynFrn7muuZi01sB4WQBnEJyD0X2oPGwdmpvCjnnb1bM01uv37hNEEkEQwXEGJoAdxPIN1FZDlLsMhxw7Ic/BkhmCGcIYDRbPEmMq5DHT4jNTlYCEcDTKp1z70n/Qe2XFM34h4DM7boZcQb9zA=='
, 'N2HJruwNKTcUhfl22C1MDYpnmP7/LHQnCv5anNcgFpyixMMHIZBZHNl8kdrIQXWDjcUIhRiGWVXXtRROCu33gjW2mmdzRyiOTkMwV10BdaUoXc6L5KtiN9dGFyQXEoL1XkG67Mw/zNVjqVMrHQMgB0aDXStFTMkF0FGJ2OuqtIzqHbiEvyFawC/MUFgRZwELp9orURS9B83WB17SjRdnM1yij19XiEPNfHZfg6vke2ZvwDbV5aI/FrG3SqYk0i0g5dkWZa3D222PTiFxcKYh1K3x74KnjmSOYg71cYT+rGoSoQCJBD7L0UwUGPOMs2K4744Lm+03HUDCqGZ4EhDL8A=='
, 2
, 'admin'
, 'test'
, GETDATE());

-- EmployeeReviews
CREATE TABLE EmployeeReviews(
	EmployeeReviewId int Identity(1,1) NOT NULL PRIMARY KEY
	, ReviewerId int NOT NULL
	, EmployeeId int NOT NULL
	, Achievements varchar(2500) NOT NULL
	, AchievementsScore int NOT NULL
	, CommunicationSkills varchar(2500) NOT NULL
	, CommunicationSkillsScore int NOT NULL
	, EmployeeComments varchar(2500) NULL
	, Improvements varchar(2500) NOT NULL
	, ImprovementsScore int NOT NULL
	, OverallPerformance varchar(2500)
	, OverallPerformanceScore int NOT NULL
	, ReviewerComments varchar(2500) NULL
	, ReviewDate DateTime2 NOT NULL
	, ModifiedBy varchar(35) NULL
	, ModifiedDate Datetime2 NULL
	, CONSTRAINT FK_EmployeeReviews_Employees_EmployeeId FOREIGN KEY (EmployeeId) REFERENCES Employees (EmployeeId)
	, CONSTRAINT FK_EmployeeReviewes_Employees_ReviewerId FOREIGN KEY (ReviewerId) REFERENCES Employees (EmployeeId)
);

CREATE UNIQUE INDEX IDX_EmployeeReviews_ReviewerId_EmployeeId ON EmployeeReviews (ReviewerId ASC, EmployeeId ASC);



