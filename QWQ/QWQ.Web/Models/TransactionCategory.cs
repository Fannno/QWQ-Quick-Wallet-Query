using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QWQ.Web.Models
{
    public class TransactionCategory
    {
        /// <summary>
        /// 類別 ID
        /// </summary>
        [DisplayName("類別 ID")]
        public int CategoryId { get; set; }

        /// <summary>
        /// 使用者 ID
        /// </summary>
        [DisplayName("使用者 ID")]
        public int UserId { get; set; }

        /// <summary>
        /// 類別名稱
        /// </summary>
        [Required]
        [MaxLength(30)]
        [DisplayName("類別名稱")]
        public string CategoryName { get; set; } = "";

        /// <summary>
        /// 交易類型（Income / Expense）
        /// </summary>
        [Required]
        [MaxLength(10)]
        [DisplayName("交易類型")]
        public string TransactionType { get; set; } = "";

        /// <summary>
        /// 是否啟用
        /// </summary>
        [DisplayName("是否啟用")]
        public bool IsActive { get; set; }

        /// <summary>
        /// 排序順序
        /// </summary>
        [DisplayName("排序順序")]
        public int SortOrder { get; set; }

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