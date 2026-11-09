using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoinKeep.Web.Models
{
    /// <summary>
    /// 使用者資產資料
    /// </summary>
    public class Wallet
    {
        /// <summary>
        /// 資產 ID
        /// </summary>
        [DisplayName("資產 ID")]
        public int WalletId { get; set; }

        /// <summary>
        /// 使用者 ID
        /// </summary>
        [DisplayName("使用者 ID")]
        public int UserId { get; set; }

        /// <summary>
        /// 資產名稱
        /// </summary>
        [Required]
        [MaxLength(50)]
        [DisplayName("資產名稱")]
        public string WalletName { get; set; } = "";

        /// <summary>
        /// 資產類型
        /// </summary>
        [Required]
        [MaxLength(20)]
        [DisplayName("資產類型")]
        public string WalletType { get; set; } = "";

        /// <summary>
        /// 初始金額
        /// </summary>
        [DisplayName("初始金額")]
        public decimal InitialBalance { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [DisplayName("是否啟用")]
        public bool IsActive { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [DisplayName("建立時間")]
        public DateTime CreatedAt { get; set; }
    }
}