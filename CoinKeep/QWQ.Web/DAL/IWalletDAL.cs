using CoinKeep.Web.Models;

namespace CoinKeep.Web.DAL
{
    public interface IWalletDAL
    {
        /// <summary>
        /// 取得使用者啟用中的資產
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>資產清單</returns>
        List<Wallet> GetWalletsByUserId(int userId);

        /// <summary>
        /// 新增資產
        /// </summary>
        /// <param name="wallet">資產資料</param>
        void CreateWallet(Wallet wallet);

        /// <summary>
        /// 取得指定資產
        /// </summary>
        /// <param name="walletId">資產 ID</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>資產資料</returns>
        Wallet? GetWalletById(int walletId, int userId);

        /// <summary>
        /// 更新資產
        /// </summary>
        /// <param name="wallet">資產資料</param>
        void UpdateWallet(Wallet wallet);
    }
}