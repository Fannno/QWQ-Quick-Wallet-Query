# QWQ 資料庫設計

## 資料表總覽

| 資料表 | 中文名稱 | 用途 |
|---|---|---|
| Users | 使用者主檔 | 儲存使用者帳號與登入資料 |
| Wallets | 資產主檔 | 儲存使用者管理的現金、銀行存款與電子錢包等資金來源 |
| Transactions | 交易紀錄 | 儲存使用者的收入與支出紀錄 |
| TransactionCategories | 交易類別 | 儲存使用者自訂的收入與支出類別 |

---

## Users（使用者主檔）

### 用途

儲存使用者帳號與登入相關資料。

### 欄位設計

| 欄位 | 型別 | PK/FK | Null | Default | 說明 |
|---|---|---|---|---|---|
| UserId | INT | PK | NO | IDENTITY | 使用者編號 |
| UserName | NVARCHAR(50) |  | NO |  | 使用者名稱 |
| Email | NVARCHAR(100) | UNIQUE | NO |  | 登入電子郵件 |
| PasswordHash | NVARCHAR(500) |  | NO |  | 密碼雜湊值 |
| CreatedAt | DATETIME |  | NO | GETDATE() | 建立時間 |

### 設計說明

- Email 作為登入識別，因此設定唯一索引。
- UserName 僅作為顯示名稱，因此允許重複。
- 密碼僅保存雜湊值，不保存原始密碼。

---

## Wallets（資產主檔）

### 用途

儲存使用者的現金、銀行存款與電子錢包等資產資料。

### 欄位設計

| 欄位 | 型別 | PK/FK | Null | Default | 說明 |
|---|---|---|---|---|---|
| WalletId | INT | PK | NO | IDENTITY | 資產編號 |
| UserId | INT | FK → Users.UserId | NO |  | 所屬使用者 |
| WalletName | NVARCHAR(50) |  | NO |  | 資產名稱 |
| WalletType | NVARCHAR(20) |  | NO |  | 資產類型 |
| InitialBalance | DECIMAL(18,2) |  | NO | 0 | 初始金額 |
| IsActive | BIT |  | NO | 1 | 是否啟用 |
| CreatedAt | DATETIME |  | NO | GETDATE() | 建立時間 |

### 關聯

Users 1 : N Wallets

### 設計說明

- Wallet 僅保存 InitialBalance。
- 實際餘額由初始金額及後續交易紀錄計算。
- IsActive 用來停用資產，避免直接刪除已有交易紀錄的 Wallet。

---

## TransactionCategories（交易類別）

### 用途

儲存使用者的收入與支出類別，讓使用者可以依照自己的需求管理類別。

### 欄位設計

| 欄位 | 型別 | PK/FK | Null | Default | 說明 |
|---|---|---|---|---|---|
| CategoryId | INT | PK | NO | IDENTITY | 類別編號 |
| UserId | INT | FK → Users.UserId | NO |  | 所屬使用者 |
| CategoryName | NVARCHAR(30) |  | NO |  | 類別名稱 |
| TransactionType | NVARCHAR(10) |  | NO |  | 類別類型（Income / Expense） |
| IsActive | BIT |  | NO | 1 | 是否啟用 |
| SortOrder | INT |  | NO | 0 | 顯示排序 |
| CreatedAt | DATETIME |  | NO | GETDATE() | 建立時間 |
| UpdatedAt | DATETIME |  | YES | NULL | 最後更新時間 |

### 關聯

Users 1 : N TransactionCategories

### 設計說明

- 每位使用者都有自己的交易類別，彼此互不影響。
- 新使用者建立帳號時，系統會建立預設收入與支出類別。
- TransactionType 使用 Income 與 Expense 區分收入及支出類別。
- 同一位使用者不能建立相同交易類型且名稱相同的類別。
- IsActive 用來停用類別，不直接刪除資料。
- 停用類別後，不允許新增交易使用該類別。

---

## Transactions（交易紀錄）

### 用途

儲存使用者的收入與支出紀錄，並關聯所屬使用者、資產及交易類別。

### 欄位設計

| 欄位 | 型別 | PK/FK | Null | Default | 說明 |
|---|---|---|---|---|---|
| TransactionId | INT | PK | NO | IDENTITY | 交易編號 |
| UserId | INT | FK → Users.UserId | NO |  | 所屬使用者 |
| WalletId | INT | FK → Wallets.WalletId | NO |  | 所屬資產 |
| CategoryId | INT | FK → TransactionCategories.CategoryId | NO |  | 交易類別 |
| TransactionType | NVARCHAR(10) |  | NO |  | 交易類型（Income / Expense） |
| Title | NVARCHAR(100) |  | NO |  | 交易名稱 |
| Amount | DECIMAL(18,2) |  | NO |  | 交易金額 |
| TransactionDate | DATE |  | NO |  | 交易日期 |
| Note | NVARCHAR(500) |  | YES | NULL | 備註 |
| IsDeleted | BIT |  | NO | 0 | 是否已刪除 |
| CreatedAt | DATETIME |  | NO | GETDATE() | 建立時間 |
| UpdatedAt | DATETIME |  | YES | NULL | 最後更新時間 |

### 關聯

- Users 1 : N Transactions
- Wallets 1 : N Transactions
- TransactionCategories 1 : N Transactions

### 設計說明

- TransactionType 使用 Income 與 Expense 區分收入及支出。
- Amount 一律保存正數，實際餘額依交易類型進行加減計算。
- 每筆交易都必須關聯使用者、資產及交易類別。
- 新增交易時，必須確認資產及類別都屬於目前登入的使用者。
- 新增交易時，必須確認資產及類別仍為啟用狀態。
- 類別的交易類型必須與交易的 TransactionType 相同。
- IsDeleted 採軟刪除方式，保留交易紀錄。
- 停用資產或類別後，仍保留原有交易紀錄。