using CoinKeep.Web.DAL;
using CoinKeep.Web.Models;
using CoinKeep.Web.ViewModels.Account;
using Microsoft.AspNetCore.Identity;

namespace CoinKeep.Web.BLL
{
    public class AccountBLL : IAccountBLL
    {
        private readonly IAccountDAL accountDAL;
        public AccountBLL(IAccountDAL accountDAL) { this.accountDAL = accountDAL; }

        /// <summary>
        /// 註冊使用者
        /// </summary>
        /// <param name="registerVM">註冊資料</param>
        public string Register(RegisterViewModel registerVM)
        {
            if (accountDAL.IsEmailExists(registerVM.Email))
            {
                return "此電子郵件已被註冊";
            }

            User user = new User
            {
                UserName = registerVM.UserName,
                Email = registerVM.Email
            };

            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

            user.PasswordHash = passwordHasher.HashPassword(
                user,
                registerVM.Password
            );

            accountDAL.CreateUser(user);

            return "";
        }

        /// <summary>
        /// 驗證使用者登入資料
        /// </summary>
        /// <param name="loginVM">登入資料</param>
        /// <returns>登入成功回傳使用者資料，失敗回傳 null</returns>
        public User? Login(LoginViewModel loginVM)
        {
            User? user = accountDAL.GetUserByEmail(loginVM.Email);

            if (user == null)
            {
                return null;
            }

            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

            PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                loginVM.Password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return user;
        }
    }
}