using CoinKeep.Web.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoinKeep.Web.DAL
{
    public class WalletDAL : IWalletDAL
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
        /// 取得使用者啟用中的資產
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>資產清單</returns>
        public List<Wallet> GetWalletsByUserId(int userId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT 
                                 WalletId, UserId, WalletName, WalletType, InitialBalance, IsActive, CreatedAt
                           FROM Wallets
                           WHERE UserId = @UserId AND IsActive = 1
                           ORDER BY CreatedAt";

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

            List<Wallet> walletList = new List<Wallet>();
            foreach (DataRow row in dt.Rows)
            {
                Wallet wallet = new Wallet
                {
                    WalletId = Convert.ToInt32(row["WalletId"]),
                    UserId = Convert.ToInt32(row["UserId"]),
                    WalletName = row["WalletName"].ToString(),
                    WalletType = row["WalletType"].ToString(),
                    InitialBalance = Convert.ToDecimal(row["InitialBalance"]),
                    IsActive = Convert.ToBoolean(row["IsActive"]),
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"])
                };
                walletList.Add(wallet);
            }
            return walletList;
        }

        /// <summary>
        /// 新增資產
        /// </summary>
        /// <param name="wallet">資產資料</param>
        public void CreateWallet(Wallet wallet)
        {
            string sql = @"INSERT INTO Wallets
                         (
                            UserId, WalletName, WalletType, InitialBalance
                         )
                         VALUES
                         (
                            @UserId, @WalletName, @WalletType, @InitialBalance
                         )";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", wallet.UserId);
                    cmd.Parameters.AddWithValue("@WalletName", wallet.WalletName);
                    cmd.Parameters.AddWithValue("@WalletType", wallet.WalletType);
                    cmd.Parameters.AddWithValue("@InitialBalance", wallet.InitialBalance);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 取得指定資產
        /// </summary>
        /// <param name="walletId">資產 ID</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>資產資料</returns>
        public Wallet? GetWalletById(int walletId, int userId)
        {
            DataTable dt = new DataTable();

            string sql = @"SELECT
                                WalletId, UserId, WalletName, WalletType, InitialBalance, IsActive, CreatedAt
                           FROM Wallets
                           WHERE WalletId = @WalletId AND UserId = @UserId AND IsActive = 1";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@WalletId", walletId);
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
            Wallet wallet = new Wallet
            {
                WalletId = Convert.ToInt32(row["WalletId"]),
                UserId = Convert.ToInt32(row["UserId"]),
                WalletName = row["WalletName"].ToString(),
                WalletType = row["WalletType"].ToString(),
                InitialBalance = Convert.ToDecimal(row["InitialBalance"]),
                IsActive = Convert.ToBoolean(row["IsActive"]),
                CreatedAt = Convert.ToDateTime(row["CreatedAt"])
            };
            return wallet;
        }

        /// <summary>
        /// 更新資產
        /// </summary>
        /// <param name="wallet">資產資料</param>
        public void UpdateWallet(Wallet wallet)
        {
            string sql = @"UPDATE Wallets
                           SET WalletName = @WalletName, WalletType = @WalletType, InitialBalance = @InitialBalance
                           WHERE WalletId = @WalletId AND UserId = @UserId AND IsActive = 1";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@WalletId", wallet.WalletId);
                    cmd.Parameters.AddWithValue("@UserId", wallet.UserId);
                    cmd.Parameters.AddWithValue("@WalletName", wallet.WalletName);
                    cmd.Parameters.AddWithValue("@WalletType", wallet.WalletType);
                    cmd.Parameters.AddWithValue("@InitialBalance", wallet.InitialBalance);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}