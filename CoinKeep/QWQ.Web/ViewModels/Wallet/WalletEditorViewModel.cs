using System.ComponentModel.DataAnnotations;

namespace CoinKeep.Web.ViewModels.Wallet
{
    public class WalletEditorViewModel
    {
        /// <summary>
        /// 資產 ID
        /// </summary>
        public int? WalletId { get; set; }

        /// <summary>
        /// 資產名稱
        /// </summary>
        [Required(ErrorMessage = "請輸入資產名稱")]
        [MaxLength(50)]
        [Display(Name = "資產名稱")]
        public string WalletName { get; set; } = "";

        /// <summary>
        /// 資產類型
        /// </summary>
        [Required(ErrorMessage = "請選擇資產類型")]
        [Display(Name = "資產類型")]
        public string WalletType { get; set; } = "";

        /// <summary>
        /// 初始金額
        /// </summary>
        [Range(typeof(decimal), "0", "9999999999999999.99", ErrorMessage = "初始金額不可小於 0")]
        [Display(Name = "初始金額")]
        public decimal InitialBalance { get; set; }
    }
}