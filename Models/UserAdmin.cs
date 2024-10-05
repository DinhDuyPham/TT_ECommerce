namespace TT_ECommerce.Models
{
    public class UserAdmin
    {
        public int Id { get; set; } // ID của người dùng

        public string Username { get; set; } // Tên đăng nhập

        public string Password { get; set; } // Mật khẩu (nên được mã hóa trong thực tế)

        public string Email { get; set; } // Địa chỉ email

        public bool IsActive { get; set; } = true; // Trạng thái hoạt động của tài khoản

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Thời gian tạo tài khoản
    }
}
