using System.ComponentModel.DataAnnotations;

namespace Nhom9_QLBanMyPham.Models
{
    // Đảm bảo class này không kế thừa từ bất kỳ đâu (không có dấu : )
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}