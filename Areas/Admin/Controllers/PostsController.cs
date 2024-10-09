using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TT_ECommerce.Areas.Admin.Models;
using TT_ECommerce.Data;
using TT_ECommerce.Models.EF;

namespace TT_ECommerce.Areas.Admin.Controllers
{

    [Area("Admin")] // Đúng thuộc tính cho khu vực admin
    public class PostsController : Controller
    {
        private readonly TT_ECommerceDbContext _context;

        public PostsController(TT_ECommerceDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách bài viết
        public IActionResult Index()
        {

            var posts = _context.TbPosts.ToList();
            return View(posts);
        }

        [Route("CreatePost")]
        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
           
        }

        [Route("CreatePost")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(TbPost post, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                // Đường dẫn vật lý đến thư mục lưu trữ hình ảnh
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgPosts");

                // Kiểm tra nếu có tệp hình ảnh được tải lên
                if (Image != null && Image.Length > 0)
                {
                    // Lấy tên file gốc
                    string fileName = Path.GetFileName(Image.FileName);

                    // Tạo đường dẫn đầy đủ nơi tệp sẽ được lưu
                    string filePath = Path.Combine(uploadPath, fileName);

                    // Lưu tệp vào đường dẫn chỉ định
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        Image.CopyTo(fileStream);
                    }

                    // Lưu đường dẫn tương đối vào cơ sở dữ liệu (để hiển thị trên web)
                    post.Image = "/imgPosts/" + fileName;
                } 
                post.CreatedDate = DateTime.Now;
                post.ModifiedDate = DateTime.Now;

                _context.TbPosts.Add(post);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Debug ModelState
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage); // In ra lỗi
                }
            }
            return View(post);
        }


        [Route("EditPost")]
        [HttpGet]
        public IActionResult EditPost(int id)
        {
            var post = _context.TbPosts.Find(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [Route("EditPost")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(TbPost post, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                var existingPost = _context.TbPosts.Find(post.Id); // Tìm bài viết theo Id
                if (existingPost == null)
                {
                    return NotFound(); // Nếu không tìm thấy bài viết
                }

                // Đường dẫn vật lý đến thư mục lưu trữ hình ảnh
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgPosts");

                // Kiểm tra nếu có tệp hình ảnh được tải lên
                if (Image != null && Image.Length > 0)
                {
                    // Lấy tên file gốc
                    string fileName = Path.GetFileName(Image.FileName);

                    // Tạo đường dẫn đầy đủ nơi tệp sẽ được lưu
                    string filePath = Path.Combine(uploadPath, fileName);

                    // Lưu tệp vào đường dẫn chỉ định
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        Image.CopyTo(fileStream);
                    }

                    // Lưu đường dẫn tương đối vào cơ sở dữ liệu (để hiển thị trên web)
                    existingPost.Image = "/imgPosts/" + fileName;
                }

                // Cập nhật các thuộc tính khác của bài viết
                existingPost.Title = post.Title;
                existingPost.Description = post.Description;
                existingPost.Detail = post.Detail;
                existingPost.SeoTitle = post.SeoTitle;
                existingPost.SeoDescription = post.SeoDescription;
                existingPost.SeoKeywords = post.SeoKeywords;
                existingPost.ModifiedDate = DateTime.Now; // Cập nhật thời gian chỉnh sửa

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.TbPosts.Update(existingPost);
                _context.SaveChanges(); // Thực hiện lưu vào DB

                return RedirectToAction("Index");
            }

            // Nếu ModelState không hợp lệ, hiển thị lại form với lỗi
            return View(post);
        }








        // GET: Admin/Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbPost = await _context.TbPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbPost == null)
            {
                return NotFound();
            }

            return View(tbPost);
        }


        // GET: Admin/Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbPost = await _context.TbPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbPost == null)
            {
                return NotFound();
            }

            return View(tbPost);
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbPost = await _context.TbPosts.FindAsync(id);
            if (tbPost != null)
            {
                _context.TbPosts.Remove(tbPost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbPostExists(int id)
        {
            return _context.TbPosts.Any(e => e.Id == id);
        }
    }
}
