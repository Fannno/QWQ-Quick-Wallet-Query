using CoinKeep.Web.Models;
using CoinKeep.Web.ViewModels.Wallet;

namespace CoinKeep.Web.BLL
{
    public interface IWalletBLL
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
        /// <param name="createVM">新增資產資料</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>新增結果訊息，成功則回傳空字串</returns>
        string CreateWallet(WalletEditorViewModel createVM, int userId);

        /// <summary>
        /// 取得指定資產編輯資料
        /// </summary>
        /// <param name="walletId">資產 ID</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>資產編輯資料</returns>
        WalletEditorViewModel? GetWalletById(int walletId, int userId);

        /// <summary>
        /// 更新資產
        /// </summary>
        /// <param name="walletVM">資產編輯資料</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>更新結果訊息，成功則回傳空字串</returns>
        string UpdateWallet(WalletEditorViewModel walletVM, int userId);
    }
}