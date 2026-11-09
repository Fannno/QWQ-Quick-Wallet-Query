using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QWQ.Web.ViewModels.Transaction
{
    public class TransactionEditorViewModel
    {
        /// <summary>
        /// 交易 ID
        /// </summary>
        public int? TransactionId { get; set; }

        /// <summary>
        /// 所屬資產 ID
        /// </summary>
        [Required(ErrorMessage = "請選擇資產")]
        [Range(1, int.MaxValue, ErrorMessage = "請選擇有效資產")]
        [Display(Name = "所屬資產")]
        public int WalletId { get; set; }

        /// <summary>
        /// 交易類型
        /// </summary>
        [Required(ErrorMessage = "請選擇交易類型")]
        [Display(Name = "交易類型")]
        public string TransactionType { get; set; } = "";

        /// <summary>
        /// 交易名稱
        /// </summary>
        [Required(ErrorMessage = "請輸入交易名稱")]
        [MaxLength(100)]
        [Display(Name = "交易名稱")]
        public string Title { get; set; } = "";

        /// <summary>
        /// 交易金額
        /// </summary>
        [Range(typeof(decimal), "0.01", "9999999999999999.99", ErrorMessage = "交易金額必須大於 0")]
        [Display(Name = "交易金額")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 交易類別
        /// </summary>
        [Required(ErrorMessage = "請輸入交易類別")]
        [MaxLength(30)]
        [Display(Name = "交易類別")]
        public string CategoryName { get; set; } = "";

        /// <summary>
        /// 交易日期
        /// </summary>
        [Required(ErrorMessage = "請選擇交易日期")]
        [DataType(DataType.Date)]
        [Display(Name = "交易日期")]
        public DateTime? TransactionDate { get; set; } = DateTime.Today;

        /// <summary>
        /// 備註
        /// </summary>
        [MaxLength(500)]
        [Display(Name = "備註")]
        public string? Note { get; set; }

        /// <summary>
        /// 可選擇的資產清單
        /// </summary>
        public List<SelectListItem> WalletOptions { get; set; } = new List<SelectListItem>();
    }
}