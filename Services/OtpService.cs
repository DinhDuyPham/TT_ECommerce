
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
namespace TT_ECommerce.Services
{
    public class OtpService
    {
        // Tạo một từ điển để lưu trữ OTP cho từng người dùng
        private readonly Dictionary<string, string> _otpStore = new Dictionary<string, string>();

        // Tạo OTP ngẫu nhiên
        public string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        // Gửi OTP qua email và lưu trữ OTP
        public async Task SendOtpAsync(string email, string otp)
        {
            var fromAddress = new MailAddress("example@gmail.com", "YourApp");
            var toAddress = new MailAddress(email);
            const string fromPassword = "examplePassword";
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
                await smtp.SendMailAsync(message);
            }

            // Lưu OTP cho người dùng
            _otpStore[email] = otp;
        }

        // Xác thực OTP
        public bool ValidateOtp(string email, string otp)
        {
            if (_otpStore.TryGetValue(email, out var storedOtp))
            {
                return storedOtp == otp;
            }
            return false;
        }
    }


}
