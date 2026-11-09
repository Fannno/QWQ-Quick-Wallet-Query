using System.ComponentModel.DataAnnotations;

namespace CoinKeep.Web.ViewModels.Account
{
    public class RegisterViewModel
    {
        /// <summary>
        /// 使用者名稱
        /// </summary>
        [Required(ErrorMessage = "請輸入使用者名稱")]
        [Display(Name = "使用者名稱")]
        public string UserName { get; set; } = "";

        /// <summary>
        /// 電子郵件
        /// </summary>
        [Required(ErrorMessage = "請輸入電子郵件")]
        [EmailAddress(ErrorMessage = "電子郵件格式不正確")]
        [Display(Name = "電子郵件")]
        public string Email { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        /// <summary>
        /// 確認密碼
        /// </summary>
        [Required(ErrorMessage = "請再次輸入密碼")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "兩次輸入的密碼不一致")]
        [Display(Name = "確認密碼")]
        public string ConfirmPassword {  get; set; }
    }
}
