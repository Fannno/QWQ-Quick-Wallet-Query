using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QWQ.Web.Models
{
    public class Transaction
    {
        /// <summary>
        /// 交易 ID
        /// </summary>
        [DisplayName("交易 ID")]
        public int TransactionId { get; set; }

        /// <summary>
        /// 使用者 ID
        /// </summary>
        [DisplayName("使用者 ID")]
        public int UserId { get; set; }

        /// <summary>
        /// 所屬資產 ID
        /// </summary>
        [DisplayName("資產 ID")]
        public int WalletId { get; set; }

        /// <summary>
        /// 交易類型（Income / Expense）
        /// </summary>
        [Required]
        [MaxLength(10)]
        [DisplayName("交易類型")]
        public string TransactionType { get; set; } = "";

        /// <summary>
        /// 交易名稱
        /// </summary>
        [Required]
        [MaxLength(100)]
        [DisplayName("交易名稱")]
        public string Title { get; set; } = "";

        /// <summary>
        /// 交易金額
        /// </summary>
        [Range(typeof(decimal), "0.01", "9999999999999999")]
        [DisplayName("交易金額")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 交易類別
        /// </summary>
        [Required]
        [MaxLength(30)]
        [DisplayName("交易類別")]
        public string CategoryName { get; set; } = "";

        /// <summary>
        /// 交易日期
        /// </summary>
        [DisplayName("交易日期")]
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [MaxLength(500)]
        [DisplayName("備註")]
        public string? Note { get; set; }

        /// <summary>
        /// 是否已刪除
        /// </summary>
        [DisplayName("是否已刪除")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [DisplayName("建立時間")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        [DisplayName("更新時間")]
        public DateTime? UpdatedAt { get; set; }
    }
}