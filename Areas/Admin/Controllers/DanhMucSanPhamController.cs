using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TT_ECommerce.Areas.Admin.Models;
using TT_ECommerce.Data;
using TT_ECommerce.Models.EF;

namespace TT_ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")] // Correct attribute for areas
    public class DanhMucSanPhamController : Controller
    {
        private readonly TT_ECommerceDbContext _context;

        public DanhMucSanPhamController(TT_ECommerceDbContext context)
        {
            _context = context;
        }
        // Hiển thị danh sách danh mục sản phẩm
        public IActionResult Index()
        {
            var categories = _context.TbProductCategories.ToList();
            return View(categories);
        }
        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TbProductCategory category)
        {
            if (ModelState.IsValid)
            { 
                _context.TbProductCategories.Add(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        [Route("SuaSanPham")]
        [HttpGet]
        public IActionResult SuaSanPham(int id)
        {   
            //ViewBag.RoleName = new SelectList(_context.Roles.ToList(), "CreatedBy", "RoleName");
            //ViewBag.RoleName = new SelectList(_context.Roles.ToList(), "Modifiedby", "RoleName");
            var danhMuc = _context.TbProductCategories.Find(id);
            if (danhMuc == null)
            {
                return NotFound();
            }
            return View(danhMuc);
        }


        [Route("SuaSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(TbProductCategory category)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(category).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


    }

}
