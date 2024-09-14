using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TT_ECommerce.Data
{
    public class TT_ECommerceDbContext : IdentityDbContext<IdentityUser>
    {
        public TT_ECommerceDbContext(DbContextOptions<TT_ECommerceDbContext> options)
            : base(options)
        {
        }

        // Thêm các DbSet cho các bảng khác nếu cần
    }
}
