using QWQ.Web.Models;
using QWQ.Web.ViewModels.Transaction;

namespace QWQ.Web.DAL
{
    public interface ITransactionDAL
    {
        /// <summary>
        /// 取得使用者的交易紀錄
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>交易紀錄清單</returns>
        List<TransactionListItemViewModel> GetTransactionsByUserId(int userId);

        /// <summary>
        /// 取得指定交易
        /// </summary>
        /// <param name="transactionId">交易 ID</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>交易資料</returns>
        Transaction? GetTransactionById(int transactionId, int userId);

        /// <summary>
        /// 新增交易
        /// </summary>
        /// <param name="transaction">交易資料</param>
        /// <returns>是否新增成功</returns>
        bool CreateTransaction(Transaction transaction);

        /// <summary>
        /// 更新交易
        /// </summary>
        /// <param name="transaction">交易資料</param>
        /// <returns>是否更新成功</returns>
        bool UpdateTransaction(Transaction transaction);

        /// <summary>
        /// 刪除交易
        /// </summary>
        /// <param name="transactionId">交易 ID</param>
        /// <param name="userId">使用者 ID</param>
        /// <returns>是否刪除成功</returns>
        bool DeleteTransaction(int transactionId, int userId);
    }
}