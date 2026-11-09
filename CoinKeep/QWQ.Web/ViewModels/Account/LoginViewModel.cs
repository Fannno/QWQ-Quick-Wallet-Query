using System.ComponentModel.DataAnnotations;

namespace CoinKeep.Web.ViewModels.Account
{
    public class LoginViewModel
    {
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
        /// 是否記住登入狀態
        /// </summary>
        [Display(Name = "記住我")]
        public bool RememberMe { get; set; }
    }
}
