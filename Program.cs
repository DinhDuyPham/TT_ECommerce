using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TT_ECommerce.Data;
using TT_ECommerce.Services;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình chuỗi kết nối đến cơ sở dữ liệu
builder.Services.AddDbContext<TT_ECommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Thêm dịch vụ OtpService
builder.Services.AddTransient<OtpService>();

// Cấu hình Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<TT_ECommerceDbContext>()
    .AddDefaultTokenProviders();

// Thêm dịch vụ MVC
builder.Services.AddControllersWithViews();

// Thêm dịch vụ Session
builder.Services.AddDistributedMemoryCache(); // Cung cấp bộ nhớ cache phân phối
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thay đổi thời gian hết hạn của session nếu cần
    options.Cookie.HttpOnly = true; // Cookie chỉ có thể được truy cập qua HTTP
    options.Cookie.IsEssential = true; // Cookie là cần thiết cho ứng dụng
});

var app = builder.Build();

// Cấu hình pipeline yêu cầu HTTP
if (app.Environment.IsDevelopment())
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
app.UseAuthentication();
app.UseAuthorization();

// Kích hoạt middleware Session
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
