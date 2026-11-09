using CoinKeep.Web.ViewModels.Account;
﻿using CoinKeep.Web.Models;

namespace CoinKeep.Web.BLL
{
    /// <summary>
    /// 使用者帳號商業邏輯介面
    /// </summary>
    public interface IAccountBLL
    {
        /// <summary>
        /// 註冊使用者
        /// </summary>
        /// <param name="registerVM">註冊資料</param>
        string Register(RegisterViewModel registerVM);

        /// <summary>
        /// 驗證使用者登入資料
        /// </summary>
        /// <param name="loginVM">登入資料</param>
        /// <returns>登入成功回傳使用者資料，失敗回傳 null</returns>
        User? Login(LoginViewModel loginVM);
    }
}