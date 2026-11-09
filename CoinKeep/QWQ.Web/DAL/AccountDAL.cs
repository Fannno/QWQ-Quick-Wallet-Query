using CoinKeep.Web.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CoinKeep.Web.DAL
{
    public class AccountDAL : IAccountDAL
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
        /// 新增使用者
        /// </summary>
        /// <param name="user">使用者資料</param>
        public void CreateUser(User user)
        {
            string sql = @"INSERT INTO Users
                         ( 
                            UserName, Email, PasswordHash
                         )
                         VALUES
                         (
                            @UserName, @Email, @PasswordHash
                         )";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 檢查電子郵件是否已存在
        /// </summary>
        /// <param name="email">電子郵件</param>
        public bool IsEmailExists(string email)
        {
            string sql = @"SELECT COUNT(Email)
                           FROM Users
                           WHERE Email = @Email";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                int cnt = Convert.ToInt32(cmd.ExecuteScalar());
                return cnt > 0;
            }
        }

        /// <summary>
        /// 依電子郵件取得使用者資料
        /// </summary>
        /// <param name="email">電子郵件</param>
        /// <returns>使用者資料</returns>
        public User? GetUserByEmail(string email)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT UserId, UserName, Email, PasswordHash, CreatedAt
                           FROM Users
                           WHERE Email = @Email";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dt.Rows[0];
            User user = new User
            {
                UserId = Convert.ToInt32(row["UserId"]),
                UserName = row["UserName"].ToString(),
                Email = row["Email"].ToString(),
                PasswordHash = row["PasswordHash"].ToString(),
                CreatedAt = Convert.ToDateTime(row["CreatedAt"])
            };
            return user;
        }
    }
}