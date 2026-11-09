using QWQ.Web.Models;
using QWQ.Web.ViewModels.Transaction;
using Microsoft.Data.SqlClient;
using System.Data;

namespace QWQ.Web.DAL
{
    public class TransactionDAL : ITransactionDAL
    {
        /// <summary>
        /// 取得資料庫連線字串
        /// </summary>
        /// <returns>資料庫連線字串</returns>
        private string GetDBConnectionString()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
                .Build();

            return config.GetConnectionString("DBConn");
        }

        /// <summary>
        /// 取得使用者的交易紀錄
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>交易紀錄清單</returns>
        public List<TransactionListItemViewModel> GetTransactionsByUserId(int userId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT
                                T.TransactionId, W.WalletName, T.TransactionType, T.Title,
                                T.Amount, T.CategoryName, T.TransactionDate, T.Note
                           FROM Transactions T
                           INNER JOIN Wallets W ON T.WalletId = W.WalletId
                           WHERE T.UserId = @UserId AND T.IsDeleted = 0
                           ORDER BY T.TransactionDate DESC, T.CreatedAt DESC";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }

            List<TransactionListItemViewModel> transactionList = new List<TransactionListItemViewModel>();
            foreach (DataRow row in dt.Rows)
            {
                TransactionListItemViewModel transaction = new TransactionListItemViewModel
                {
                    TransactionId = Convert.ToInt32(row["TransactionId"]),
                    WalletName = row["WalletName"].ToString(),
                    TransactionType = row["TransactionType"].ToString(),
                    Title = row["Title"].ToString(),
                    Amount = Convert.ToDecimal(row["Amount"]),
                    CategoryName = row["CategoryName"].ToString(),
                    TransactionDate = Convert.ToDateTime(row["TransactionDate"]),
                    Note = row["Note"] == DBNull.Value ? null : row["Note"].ToString()
                };
                transactionList.Add(transaction);
            }
            return transactionList;
        }

        /// <summary>
        /// 取得指定交易
        /// </summary>
        /// <param name="transactionId">交易 ID</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>交易資料</returns>
        public Transaction? GetTransactionById(int transactionId, int userId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT
                                TransactionId, UserId, WalletId, TransactionType, Title,
                                Amount, CategoryName, TransactionDate, Note, IsDeleted, CreatedAt, UpdatedAt
                           FROM Transactions
                           WHERE TransactionId = @TransactionId AND UserId = @UserId AND IsDeleted = 0";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TransactionId", transactionId);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dt.Rows[0];
            Transaction transaction = new Transaction
            {
                TransactionId = Convert.ToInt32(row["TransactionId"]),
                UserId = Convert.ToInt32(row["UserId"]),
                WalletId = Convert.ToInt32(row["WalletId"]),
                TransactionType = row["TransactionType"].ToString(),
                Title = row["Title"].ToString(),
                Amount = Convert.ToDecimal(row["Amount"]),
                CategoryName = row["CategoryName"].ToString(),
                TransactionDate = Convert.ToDateTime(row["TransactionDate"]),
                Note = row["Note"] == DBNull.Value ? null : row["Note"].ToString(),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                UpdatedAt = row["UpdatedAt"] == DBNull.Value ? null : Convert.ToDateTime(row["UpdatedAt"])
            };
            return transaction;
        }

        /// <summary>
        /// 新增交易
        /// </summary>
        /// <param name="transaction">交易資料</param>
        /// <returns>是否新增成功</returns>
        public bool CreateTransaction(Transaction transaction)
        {
            string sql = @"INSERT INTO Transactions
                           (
                               UserId, WalletId, TransactionType, Title, Amount, CategoryName, TransactionDate, Note
                           )
                           SELECT
                               @UserId, @WalletId, @TransactionType, @Title, @Amount, @CategoryName, @TransactionDate, @Note
                           WHERE EXISTS
                           (
                               SELECT 1 FROM Wallets
                               WHERE WalletId = @WalletId AND UserId = @UserId AND IsActive = 1
                           )";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", transaction.UserId);
                    cmd.Parameters.AddWithValue("@WalletId", transaction.WalletId);
                    cmd.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
                    cmd.Parameters.AddWithValue("@Title", transaction.Title);
                    cmd.Parameters.AddWithValue("@Amount", transaction.Amount);
                    cmd.Parameters.AddWithValue("@CategoryName", transaction.CategoryName);
                    cmd.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate.Date);
                    cmd.Parameters.AddWithValue("@Note", (object?)transaction.Note ?? DBNull.Value);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// 更新交易
        /// </summary>
        /// <param name="transaction">交易資料</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateTransaction(Transaction transaction)
        {
            string sql = @"UPDATE Transactions
                           SET WalletId = @WalletId, TransactionType = @TransactionType, Title = @Title,
                               Amount = @Amount, CategoryName = @CategoryName, TransactionDate = @TransactionDate,
                               Note = @Note, UpdatedAt = GETDATE()
                           WHERE TransactionId = @TransactionId AND UserId = @UserId AND IsDeleted = 0
                             AND EXISTS
                             (
                                 SELECT 1 FROM Wallets
                                 WHERE WalletId = @WalletId AND UserId = @UserId AND IsActive = 1
                             )";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TransactionId", transaction.TransactionId);
                    cmd.Parameters.AddWithValue("@UserId", transaction.UserId);
                    cmd.Parameters.AddWithValue("@WalletId", transaction.WalletId);
                    cmd.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
                    cmd.Parameters.AddWithValue("@Title", transaction.Title);
                    cmd.Parameters.AddWithValue("@Amount", transaction.Amount);
                    cmd.Parameters.AddWithValue("@CategoryName", transaction.CategoryName);
                    cmd.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate.Date);
                    cmd.Parameters.AddWithValue("@Note", (object?)transaction.Note ?? DBNull.Value);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// 刪除交易
        /// </summary>
        /// <param name="transactionId">交易 ID</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>是否刪除成功</returns>
        public bool DeleteTransaction(int transactionId, int userId)
        {
            string sql = @"UPDATE Transactions
                           SET IsDeleted = 1, UpdatedAt = GETDATE()
                           WHERE TransactionId = @TransactionId AND UserId = @UserId AND IsDeleted = 0";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TransactionId", transactionId);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}