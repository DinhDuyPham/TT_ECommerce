using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Sử dụng đúng namespace cho IConfiguration
using Microsoft.Extensions.DependencyInjection;
using TT_ECommerce.Data; // Namespace cho DbContext

namespace TT_ECommerce
{
    public class Startup
    {
        public IConfiguration Configuration { get; } // Thêm thuộc tính Configuration

        // Constructor nhận IConfiguration từ Dependency Injection
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Cấu hình các dịch vụ
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Cấu hình xác thực với cookie
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/LoginAdmin"; // Đường dẫn đến trang đăng nhập
                    options.LogoutPath = "/LoginAdmin/Logout"; // Đường dẫn đến trang đăng xuất
                });

            services.AddControllersWithViews();

            // Sử dụng chuỗi kết nối từ appsettings.json
            services.AddDbContext<TT_ECommerceDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews();
        }

        // Cấu hình middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Đảm bảo sử dụng xác thực
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

<<<<<<< HEAD
                // Đường dẫn cho các khu vực
                endpoints.MapAreaControllerRoute(
                    name: "admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
            });
=======

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
>>>>>>> 396affb8a0232d4269cf98d3dc7d04d15f41ef7b
        }
    }
}