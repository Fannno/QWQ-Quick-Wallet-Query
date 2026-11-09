USE QWQ;
GO

-- 建立交易類別資料表
CREATE TABLE TransactionCategories
(
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    CategoryName NVARCHAR(30) NOT NULL,
    TransactionType NVARCHAR(10) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    SortOrder INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL,

    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CHECK (TransactionType IN ('Income', 'Expense'))
);
GO

-- 同一位使用者不能建立相同類型的重複類別
CREATE UNIQUE INDEX IX_TransactionCategories
ON TransactionCategories(UserId, TransactionType, CategoryName);
GO

-- 建立預設收入類別
INSERT INTO TransactionCategories (UserId, CategoryName, TransactionType, SortOrder)
SELECT UserId, N'薪資', 'Income', 1 FROM Users
UNION ALL SELECT UserId, N'獎金', 'Income', 2 FROM Users
UNION ALL SELECT UserId, N'生活費', 'Income', 3 FROM Users
UNION ALL SELECT UserId, N'兼職收入', 'Income', 4 FROM Users
UNION ALL SELECT UserId, N'投資收益', 'Income', 5 FROM Users
UNION ALL SELECT UserId, N'其他收入', 'Income', 6 FROM Users;
GO

-- 建立預設支出類別
INSERT INTO TransactionCategories (UserId, CategoryName, TransactionType, SortOrder)
SELECT UserId, N'餐飲', 'Expense', 1 FROM Users
UNION ALL SELECT UserId, N'交通', 'Expense', 2 FROM Users
UNION ALL SELECT UserId, N'購物', 'Expense', 3 FROM Users
UNION ALL SELECT UserId, N'娛樂', 'Expense', 4 FROM Users
UNION ALL SELECT UserId, N'生活用品', 'Expense', 5 FROM Users
UNION ALL SELECT UserId, N'房租／住宿', 'Expense', 6 FROM Users
UNION ALL SELECT UserId, N'水電瓦斯', 'Expense', 7 FROM Users
UNION ALL SELECT UserId, N'醫療', 'Expense', 8 FROM Users
UNION ALL SELECT UserId, N'教育', 'Expense', 9 FROM Users
UNION ALL SELECT UserId, N'其他支出', 'Expense', 10 FROM Users;
GO

-- 確認交易類別 ID 欄位已存在，不重複新增
-- 建立交易與類別的外鍵關聯
ALTER TABLE Transactions
ADD CONSTRAINT FK_Transactions_TransactionCategories
FOREIGN KEY (CategoryId)
REFERENCES TransactionCategories(CategoryId);
GO
