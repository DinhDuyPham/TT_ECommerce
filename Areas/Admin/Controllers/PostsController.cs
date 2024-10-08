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
            ViewBag.CategoryList = new SelectList(_context.TbCategories, "Id", "Title");
            return View();
           
        }

        [Route("CreatePost")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(TbPost post)
        {
            if (ModelState.IsValid)
            {
                post.CreatedDate = DateTime.Now;
                post.ModifiedDate = DateTime.Now;
               
                _context.TbPosts.Add(post);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
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
        public IActionResult EditPost(TbPost post)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(post).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
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
                .Include(t => t.Category)
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
                .Include(t => t.Category)
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
