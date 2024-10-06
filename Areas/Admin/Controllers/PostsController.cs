using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TT_ECommerce.Data;
using TT_ECommerce.Models.EF;

namespace TT_ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    // Gộp cái ThemBaiViet vô đây
    public class PostsController : Controller
    {
        private readonly TT_ECommerceDbContext _context;

        public PostsController(TT_ECommerceDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Posts
        public async Task<IActionResult> Index()
        {
            var tT_ECommerceDbContext = _context.TbPosts.Include(t => t.Category);
            return View(await tT_ECommerceDbContext.ToListAsync());
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

        // GET: Admin/Posts/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "Id", "Id");
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Detail,Image,CategoryId,SeoTitle,SeoDescription,SeoKeywords,CreatedBy,CreatedDate,ModifiedDate,Modifiedby,Alias,IsActive")] TbPost tbPost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "Id", "Id", tbPost.CategoryId);
            return View(tbPost);
        }

        // GET: Admin/Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbPost = await _context.TbPosts.FindAsync(id);
            if (tbPost == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "Id", "Id", tbPost.CategoryId);
            return View(tbPost);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Detail,Image,CategoryId,SeoTitle,SeoDescription,SeoKeywords,CreatedBy,CreatedDate,ModifiedDate,Modifiedby,Alias,IsActive")] TbPost tbPost)
        {
            if (id != tbPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbPostExists(tbPost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.TbCategories, "Id", "Id", tbPost.CategoryId);
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
