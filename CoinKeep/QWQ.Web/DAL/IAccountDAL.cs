using CoinKeep.Web.Models;

namespace CoinKeep.Web.DAL
{
    /// <summary>
    /// 使用者帳號資料存取介面
    /// </summary>
    public interface IAccountDAL
    {
        /// <summary>
        /// 檢查電子郵件是否已存在
        /// </summary>
        /// <param name="email">電子郵件</param>
        bool IsEmailExists(string email);

        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="user">使用者資料</param>
        void CreateUser(User user);

        /// <summary>
        /// 依電子郵件取得使用者資料
        /// </summary>
        /// <param name="email">電子郵件</param>
        User? GetUserByEmail(string email);
    }
}