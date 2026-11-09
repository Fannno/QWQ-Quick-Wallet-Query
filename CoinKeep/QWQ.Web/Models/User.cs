using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoinKeep.Web.Models
{
    public class User
    {
        /// <summary>
        /// 使用者 ID
        /// </summary>
        [DisplayName("使用者 ID")]
        public int UserId { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        [Required]
        [MaxLength(50)]
        [DisplayName("使用者名稱")]
        public string UserName { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        [Required]
        [MaxLength(100)]
        [DisplayName("電子郵件")]
        public string Email { get; set; }

        /// <summary>
        /// 密碼雜湊值
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }

        /// <summary>
        /// 新增時間
        /// </summary>
        [DisplayName("新增時間")]
        public DateTime CreatedAt { get; set; }
    }
}
