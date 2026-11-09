USE QWQ;
GO

/* 收支交易資料表 */
CREATE TABLE Transactions
(
    TransactionId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    WalletId INT NOT NULL,
    CategoryId INT NOT NULL,
    TransactionType NVARCHAR(10) NOT NULL,
    Title NVARCHAR(100) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    TransactionDate DATE NOT NULL,
    Note NVARCHAR(500) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,

    CONSTRAINT FK_Transactions_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Transactions_Wallets
        FOREIGN KEY (WalletId)
        REFERENCES Wallets(WalletId),

    CONSTRAINT FK_Transactions_TransactionCategories
        FOREIGN KEY (CategoryId)
        REFERENCES TransactionCategories(CategoryId),

    CONSTRAINT CK_Transactions_Type
        CHECK (TransactionType IN ('Income', 'Expense')),

    CONSTRAINT CK_Transactions_Amount
        CHECK (Amount > 0)
);
GO

/* 加速依使用者查詢交易紀錄 */
CREATE INDEX IX_Transactions_UserId
ON Transactions(UserId);
GO

/* 加速依資產帳戶查詢交易紀錄 */
CREATE INDEX IX_Transactions_WalletId
ON Transactions(WalletId);
GO

/* 加速依交易日期查詢 */
CREATE INDEX IX_Transactions_TransactionDate
ON Transactions(TransactionDate);
GO