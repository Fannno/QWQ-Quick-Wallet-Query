CREATE DATABASE QWQ;
GO

USE QWQ;
GO

/* 使用者資料表 */
CREATE TABLE Users
(
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(500) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

/* 建立電子郵件唯一索引，避免相同 Email 重複註冊 */
CREATE UNIQUE INDEX UX_Users_Email
ON Users(Email);
GO
