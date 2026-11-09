using QWQ.Web.DAL;
using QWQ.Web.Models;
using QWQ.Web.ViewModels.Transaction;

namespace QWQ.Web.BLL
{
    public class TransactionBLL : ITransactionBLL
    {
        private readonly ITransactionDAL transactionDAL;
        private readonly IWalletDAL walletDAL;
        public TransactionBLL(ITransactionDAL transactionDAL, IWalletDAL walletDAL)
        {
            this.transactionDAL = transactionDAL;
            this.walletDAL = walletDAL;
        }

        /// <summary>
        /// 取得使用者的交易紀錄
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>交易紀錄清單</returns>
        public List<TransactionListItemViewModel> GetTransactionsByUserId(int userId)
        {
            return transactionDAL.GetTransactionsByUserId(userId);
        }

        /// <summary>
        /// 取得指定交易編輯資料
        /// </summary>
        /// <param name="transactionId">交易 ID</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>交易編輯資料</returns>
        public TransactionEditorViewModel? GetTransactionById(int transactionId, int userId)
        {
            Transaction? transaction = transactionDAL.GetTransactionById(transactionId, userId);
            if (transaction == null)
            {
                return null;
            }

            TransactionEditorViewModel transactionVM = new TransactionEditorViewModel
            {
                TransactionId = transaction.TransactionId,
                WalletId = transaction.WalletId,
                TransactionType = transaction.TransactionType,
                Title = transaction.Title,
                Amount = transaction.Amount,
                CategoryName = transaction.CategoryName,
                TransactionDate = transaction.TransactionDate.Date,
                Note = transaction.Note
            };
            return transactionVM;
        }

        /// <summary>
        /// 新增交易
        /// </summary>
        /// <param name="transactionVM">新增交易資料</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>新增結果訊息，成功則回傳空字串</returns>
        public string CreateTransaction(TransactionEditorViewModel transactionVM, int userId)
        {
            string[] transactionTypes = { "Income", "Expense" };
            if (!transactionTypes.Contains(transactionVM.TransactionType))
            {
                return "交易類型不正確";
            }

            Wallet? wallet = walletDAL.GetWalletById(transactionVM.WalletId, userId);
            if (wallet == null)
            {
                return "找不到資產或資產已停用";
            }

            Transaction transaction = new Transaction
            {
                UserId = userId,
                WalletId = transactionVM.WalletId,
                TransactionType = transactionVM.TransactionType,
                Title = transactionVM.Title.Trim(),
                Amount = transactionVM.Amount,
                CategoryName = transactionVM.CategoryName.Trim(),
                TransactionDate = transactionVM.TransactionDate!.Value.Date,
                Note = string.IsNullOrWhiteSpace(transactionVM.Note) ? null : transactionVM.Note.Trim()
            };

            bool result = transactionDAL.CreateTransaction(transaction);
            if (!result)
            {
                return "新增交易失敗";
            }
            return "";
        }

        /// <summary>
        /// 更新交易
        /// </summary>
        /// <param name="transactionVM">交易編輯資料</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>更新結果訊息，成功則回傳空字串</returns>
        public string UpdateTransaction(TransactionEditorViewModel transactionVM, int userId)
        {
            if (!transactionVM.TransactionId.HasValue)
            {
                return "找不到交易";
            }

            string[] transactionTypes = { "Income", "Expense" };
            if (!transactionTypes.Contains(transactionVM.TransactionType))
            {
                return "交易類型不正確";
            }

            Transaction? transaction = transactionDAL.GetTransactionById(transactionVM.TransactionId.Value, userId);
            if (transaction == null)
            {
                return "找不到交易";
            }

            Wallet? wallet = walletDAL.GetWalletById(transactionVM.WalletId, userId);
            if (wallet == null)
            {
                return "找不到資產或資產已停用";
            }

            transaction.WalletId = transactionVM.WalletId;
            transaction.TransactionType = transactionVM.TransactionType;
            transaction.Title = transactionVM.Title.Trim();
            transaction.Amount = transactionVM.Amount;
            transaction.CategoryName = transactionVM.CategoryName.Trim();
            transaction.TransactionDate = transactionVM.TransactionDate!.Value.Date;
            transaction.Note = string.IsNullOrWhiteSpace(transactionVM.Note) ? null : transactionVM.Note.Trim();

            bool result = transactionDAL.UpdateTransaction(transaction);
            if (!result)
            {
                return "更新交易失敗";
            }
            return "";
        }

        /// <summary>
        /// 刪除交易
        /// </summary>
        /// <param name="transactionId">交易 ID</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>刪除結果訊息，成功則回傳空字串</returns>
        public string DeleteTransaction(int transactionId, int userId)
        {
            if (transactionId <= 0 || userId <= 0)
            {
                return "交易資料不正確";
            }

            bool result = transactionDAL.DeleteTransaction(transactionId, userId);
            if (!result)
            {
                return "找不到交易紀錄";
            }
            return "";
        }
    }
}