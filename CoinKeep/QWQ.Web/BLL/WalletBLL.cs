using CoinKeep.Web.DAL;
using CoinKeep.Web.Models;
using CoinKeep.Web.ViewModels.Wallet;

namespace CoinKeep.Web.BLL
{
    public class WalletBLL : IWalletBLL
    {
        private readonly IWalletDAL walletDAL;
        public WalletBLL(IWalletDAL walletDAL) { this.walletDAL = walletDAL; }

        /// <summary>
        /// 取得使用者啟用中的資產
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>資產清單</returns>
        public List<Wallet> GetWalletsByUserId(int userId)
        {
            return walletDAL.GetWalletsByUserId(userId);
        }

        /// <summary>
        /// 新增資產
        /// </summary>
        /// <param name="createVM">新增資產資料</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>新增結果訊息，成功則回傳空字串</returns>
        public string CreateWallet(WalletEditorViewModel createVM, int userId)
        {
            string[] walletTypes = { "Cash", "Bank", "EWallet" };
            if (!walletTypes.Contains(createVM.WalletType))
            {
                return "資產類型不正確";
            }

            Wallet wallet = new Wallet
            {
                UserId = userId,
                WalletName = createVM.WalletName,
                WalletType = createVM.WalletType,
                InitialBalance = createVM.InitialBalance
            };

            walletDAL.CreateWallet(wallet);
            return "";
        }

        /// <summary>
        /// 取得指定資產編輯資料
        /// </summary>
        /// <param name="walletId">資產 ID</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>資產編輯資料</returns>
        public WalletEditorViewModel? GetWalletById(int walletId, int userId)
        {
            Wallet? wallet = walletDAL.GetWalletById(walletId, userId);
            if (wallet == null)
            {
                return null;
            }

            WalletEditorViewModel walletVM = new WalletEditorViewModel
            {
                WalletId = wallet.WalletId,
                WalletName = wallet.WalletName,
                WalletType = wallet.WalletType,
                InitialBalance = wallet.InitialBalance
            };
            return walletVM;
        }

        /// <summary>
        /// 更新資產
        /// </summary>
        /// <param name="walletVM">資產編輯資料</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>更新結果訊息，成功則回傳空字串</returns>
        public string UpdateWallet(WalletEditorViewModel walletVM, int userId)
        {
            if (!walletVM.WalletId.HasValue)
            {
                return "找不到資產";
            }

            string[] walletTypes = { "Cash", "Bank", "EWallet" };
            if (!walletTypes.Contains(walletVM.WalletType))
            {
                return "資產類型不正確";
            }

            Wallet? wallet = walletDAL.GetWalletById(walletVM.WalletId.Value, userId);

            if (wallet == null)
            {
                return "找不到資產";
            }

            wallet.WalletName = walletVM.WalletName;
            wallet.WalletType = walletVM.WalletType;
            wallet.InitialBalance = walletVM.InitialBalance;

            walletDAL.UpdateWallet(wallet);
            return "";
        }
    }
}