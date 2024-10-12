using Microsoft.AspNetCore.Mvc;
using TT_ECommerce.Data;
using TT_ECommerce.Models.EF;

[Area("Admin")] // Đúng thuộc tính cho khu vực admin
[Route("Admin/[controller]")]
public class ThemSanPhamController : Controller
{
    private readonly TT_ECommerceDbContext _context;

    public ThemSanPhamController(TT_ECommerceDbContext context)
    {
        _context = context;
    }

    // Route cho Index
    [HttpGet("")]
    public IActionResult Index()
    {
        var pros = _context.TbProducts.ToList();
        return View(pros);
    }

    // Route cho CreatePro (GET)
    [Route("CreatePro")]
    [HttpGet]
    public IActionResult CreatePro()
    {
        return View();
    }

    // Route cho CreatePro (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreatePro(TbProduct pro, IFormFile? Image)
    {
        if (ModelState.IsValid)
        {
            // Đường dẫn lưu ảnh
            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgProducts");

            // Kiểm tra và tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Kiểm tra nếu có tệp hình ảnh được tải lên
            if (Image != null && Image.Length > 0)
            {
                // Lấy tên file gốc và tạo đường dẫn đầy đủ để lưu
                string fileName = Path.GetFileName(Image.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                // Lưu tệp vào đường dẫn chỉ định
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(fileStream);
                }

                // Lưu đường dẫn tương đối vào cơ sở dữ liệu (để hiển thị trên web)
                pro.Image = "/imgProducts/" + fileName;
            }

            // Thiết lập thời gian tạo và sửa đổi
            pro.CreatedDate = DateTime.Now;
            pro.ModifiedDate = DateTime.Now;

            // Thêm sản phẩm vào DbSet
            _context.TbProducts.Add(pro);
            _context.SaveChanges();

            // Chuyển hướng về trang index
            return RedirectToAction("Index");
        }

        // Nếu có lỗi trong ModelState, trả lại view với thông báo lỗi
        foreach (var modelState in ModelState.Values)
        {
            foreach (var error in modelState.Errors)
            {
                Console.WriteLine(error.ErrorMessage); // In ra lỗi
            }
        }

        return View(pro);
    }
}
