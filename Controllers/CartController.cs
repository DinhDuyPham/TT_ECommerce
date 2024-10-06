using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TT_ECommerce.Data;
using TT_ECommerce.Models.EF;

namespace TT_ECommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly TT_ECommerceDbContext _context;

        public CartController(TT_ECommerceDbContext context)
        {
            _context = context;
        }

        // Hiển thị giỏ hàng
        public IActionResult Index()
        {
            // Lấy tất cả các đơn hàng có chi tiết từ cơ sở dữ liệu
            var cartItems = _context.TbOrders
                .Include(o => o.TbOrderDetails) // Giả sử rằng TbOrder có một mối quan hệ với TbOrderDetail
                    .ThenInclude(d => d.Product) // Giả sử TbOrderDetail có một mối quan hệ với TbProduct
                .ToList();

            return View(cartItems);
        }

        // Hiển thị form thêm sản phẩm vào giỏ hàng
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý form thêm sản phẩm vào giỏ hàng
      
        [HttpPost]
        public IActionResult Create(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                ModelState.AddModelError("", "Số lượng phải lớn hơn 0.");
                return View();
            }

            var product = _context.TbProducts.Find(productId); // Thay Id bằng productId

            if (product == null)
            {
                return NotFound();
            }


            var order = new TbOrder
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                TbOrderDetails = new List<TbOrderDetail>
        {
            new TbOrderDetail
            {
                ProductId = productId,
                Quantity = quantity
            }
        }
            };

            // Thêm đơn hàng vào giỏ hàng
            _context.TbOrders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        // Hiển thị form chỉnh sửa sản phẩm trong giỏ hàng
        public IActionResult Edit(int id)
        {
            var order = _context.TbOrders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // Xử lý form chỉnh sửa sản phẩm trong giỏ hàng
        [HttpPost]
        public IActionResult Edit(TbOrder order)
        {
            if (ModelState.IsValid)
            {
                _context.TbOrders.Update(order);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var orderDetail = _context.TbOrderDetails.Include(od => od.Order).FirstOrDefault(od => od.Id == id);

            if (orderDetail != null)
            {
                var order = orderDetail.Order; // Lấy đơn hàng tương ứng với chi tiết đơn hàng

                // Xóa chi tiết đơn hàng
                _context.TbOrderDetails.Remove(orderDetail);

                // Kiểm tra xem đơn hàng có còn chi tiết nào khác không
                if (!order.TbOrderDetails.Any()) // Nếu không còn chi tiết nào
                {
                    _context.TbOrders.Remove(order); // Xóa đơn hàng
                }

                _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCart(Dictionary<int, int> quantities)
        {
            if (quantities == null || !quantities.Any())
            {
                return RedirectToAction("Index"); // Trở lại trang giỏ hàng nếu không có sản phẩm nào
            }

            // Lặp qua từng sản phẩm trong giỏ hàng và cập nhật số lượng
            foreach (var item in quantities)
            {
                var orderDetail = _context.TbOrderDetails.Include(od => od.Order)
                    .ThenInclude(o => o.TbOrderDetails) // Bao gồm các chi tiết đơn hàng
                    .FirstOrDefault(od => od.Id == item.Key); // Tìm orderDetail theo ID

                if (orderDetail != null)
                {
                    // Cập nhật số lượng
                    orderDetail.Quantity = item.Value;

                    // Tính toán lại TotalAmount cho đơn hàng
                    var order = orderDetail.Order; // Lấy đơn hàng tương ứng
                    decimal totalAmount = 0; // Khởi tạo lại tổng số tiền

                    foreach (var detail in order.TbOrderDetails) // Tính toán lại tổng tiền
                    {
                        totalAmount += detail.Price * detail.Quantity; // Cộng dồn
                    }

                    // Cập nhật lại TotalAmount cho đơn hàng
                    order.TotalAmount = totalAmount;

                    // Cập nhật lại orderDetail và order
                    _context.TbOrderDetails.Update(orderDetail);
                    _context.TbOrders.Update(order);
                }
            }
            _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

            return RedirectToAction("Index"); // Quay lại trang giỏ hàng
        }




    }
}
