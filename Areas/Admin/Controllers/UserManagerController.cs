using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using TT_ECommerce.Areas.Admin.Models;
using TT_ECommerce.Models;

namespace TT_ECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserManagerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagerController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // 1. Hiển thị danh sách người dùng
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userRoleViewModels = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoleViewModels.Add(new UserRoleViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList() // Chuyển đổi danh sách các vai trò thành danh sách string
                });
            }

            return View(userRoleViewModels);  // Truyền danh sách UserRoleViewModel vào view
        }
        // 2. Gán vai trò cho người dùng
        [HttpGet]
        public async Task<IActionResult> AssignRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy vai trò hiện tại của người dùng
            var currentRoles = await _userManager.GetRolesAsync(user);

            var model = new AssignRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = _roleManager.Roles.Select(r => new RoleViewModel { RoleId = r.Id, RoleName = r.Name }).ToList(),
                UserRoles = currentRoles.ToList(),
                SelectedRoles = currentRoles.ToList() // Chọn các vai trò hiện tại
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, List<string> selectedRoles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy vai trò hiện tại của người dùng
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Xóa tất cả vai trò hiện tại của người dùng
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                // Nếu có lỗi khi xóa vai trò, tạo lại model và hiển thị thông báo lỗi
                foreach (var error in removeResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(new AssignRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = _roleManager.Roles.Select(r => new RoleViewModel { RoleId = r.Id, RoleName = r.Name }).ToList(),
                    UserRoles = currentRoles.ToList(),
                    SelectedRoles = selectedRoles
                });
            }

            // Gán các vai trò mới
            if (selectedRoles != null && selectedRoles.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, selectedRoles);
                if (addResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                // Nếu có lỗi trong quá trình gán vai trò mới, tạo lại model và hiển thị thông báo lỗi
                foreach (var error in addResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Nếu không có vai trò nào được chọn hoặc có lỗi, tạo lại model để hiển thị trong view
            var model = new AssignRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = _roleManager.Roles.Select(r => new RoleViewModel { RoleId = r.Id, RoleName = r.Name }).ToList(),
                UserRoles = currentRoles.ToList(),
                SelectedRoles = selectedRoles
            };

            return View(model);
        }
        // Phương thức chỉnh sửa thông tin người dùng
        // Phương thức chỉnh sửa thông tin người dùng
        [HttpGet]
        public async Task<IActionResult> EditUser(string? userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditUserModel
        {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName
        };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.Email;
            user.UserName = model.UserName;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index");
        }


    }
}
