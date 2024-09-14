
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
namespace TT_ECommerce.Services
{

    public class OtpService
    {
        public string GenerateOtp()
        {
            // Tạo mã OTP ngẫu nhiên với 6 chữ số
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task SendOtpAsync(string email, string otp)
        {
            // Cấu hình thông tin SMTP để gửi email 
            var fromAddress = new MailAddress("example@gmail.com", "YourApp"); // Email để gửi OTP tới Mail người dùng
            var toAddress = new MailAddress(email);
            const string fromPassword = "examplePasword"; // Pass Word của Email
            const string subject = "OTP Verification";
            string body = $"Your OTP is: {otp}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                try
                {
                    await smtp.SendMailAsync(message);
                }
                catch (SmtpException ex)
                {
                    // Xử lý lỗi gửi email
                    Console.WriteLine($"SMTP Error: {ex.Message}");
                }
            }
        }
    }

}
