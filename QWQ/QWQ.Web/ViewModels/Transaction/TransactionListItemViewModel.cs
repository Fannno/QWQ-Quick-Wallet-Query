namespace QWQ.Web.ViewModels.Transaction
{
    public class TransactionListItemViewModel
    {
        /// <summary>
        /// 交易 ID
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// 資產名稱
        /// </summary>
        public string WalletName { get; set; } = "";

        /// <summary>
        /// 交易類型
        /// </summary>
        public string TransactionType { get; set; } = "";

        /// <summary>
        /// 交易名稱
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 交易類別
        /// </summary>
        public string CategoryName { get; set; } = "";

        /// <summary>
        /// 交易日期
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string? Note { get; set; }
    }
}