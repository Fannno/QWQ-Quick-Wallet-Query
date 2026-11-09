USE QWQ
Go

/* 使用者資金帳戶資料表 */
CREATE TABLE Wallets
(
    WalletId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    WalletName NVARCHAR(50) NOT NULL,
    WalletType NVARCHAR(20) NOT NULL,
    InitialBalance DECIMAL(18,2) NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Wallets_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
);
GO

/* 建立使用者索引，加快依使用者查詢帳戶 */
CREATE INDEX IX_Wallets_UserId
ON Wallets(UserId);
GO