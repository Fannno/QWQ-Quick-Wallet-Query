using QWQ.Web.ViewModels.Transaction;

namespace QWQ.Web.BLL
{
    public interface ITransactionBLL
    {
        /// <summary>
        /// 取得使用者的交易紀錄
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>交易紀錄清單</returns>
        List<TransactionListItemViewModel> GetTransactionsByUserId(int userId);

        /// <summary>
        /// 取得指定交易編輯資料
        /// </summary>
        /// <param name="transactionId">交易 ID</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>交易編輯資料</returns>
        TransactionEditorViewModel? GetTransactionById(int transactionId, int userId);

        /// <summary>
        /// 新增交易
        /// </summary>
        /// <param name="transactionVM">新增交易資料</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>新增結果訊息，成功則回傳空字串</returns>
        string CreateTransaction(TransactionEditorViewModel transactionVM, int userId);

        /// <summary>
        /// 更新交易
        /// </summary>
        /// <param name="transactionVM">交易編輯資料</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>更新結果訊息，成功則回傳空字串</returns>
        string UpdateTransaction(TransactionEditorViewModel transactionVM, int userId);

        /// <summary>
        /// 刪除交易
        /// </summary>
        /// <param name="transactionId">交易 ID</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>刪除結果訊息，成功則回傳空字串</returns>
        string DeleteTransaction(int transactionId, int userId);
    }
}