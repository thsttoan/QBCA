using Microsoft.AspNetCore.Mvc;
using QBCAWEB.Models;

namespace QBCAWEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop Dell", Description = "Laptop Dell Inspiron 15", Price = 15000000, Category = "Electronics" },
            new Product { Id = 2, Name = "iPhone 15", Description = "Apple iPhone 15 Pro Max", Price = 30000000, Category = "Mobile" },
            new Product { Id = 3, Name = "Samsung TV", Description = "Smart TV Samsung 55 inch", Price = 12000000, Category = "Electronics" }
        };

        [HttpGet]
        public ActionResult<ResponseModel<List<Product>>> GetAll()
        {
            return Ok(ResponseModel<List<Product>>.SuccessResult(_products, "Lấy danh sách sản phẩm thành công"));
        }

        [HttpGet("{id}")]
        public ActionResult<ResponseModel<Product>> GetById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(ResponseModel<Product>.ErrorResult("Không tìm thấy sản phẩm", 404));
            }
            return Ok(ResponseModel<Product>.SuccessResult(product, "Lấy thông tin sản phẩm thành công"));
        }

        [HttpPost]
        public ActionResult<ResponseModel<Product>> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseModel<Product>.ErrorResult("Dữ liệu không hợp lệ"));
            }

            product.Id = _products.Max(p => p.Id) + 1;
            product.CreatedDate = DateTime.Now;
            _products.Add(product);

            return CreatedAtAction(nameof(GetById),
                new { id = product.Id },
                ResponseModel<Product>.SuccessResult(product, "Tạo sản phẩm thành công"));
        }

        [HttpPut("{id}")]
        public ActionResult<ResponseModel<Product>> Update(int id, [FromBody] Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound(ResponseModel<Product>.ErrorResult("Không tìm thấy sản phẩm", 404));
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Category = product.Category;
            existingProduct.IsActive = product.IsActive;

            return Ok(ResponseModel<Product>.SuccessResult(existingProduct, "Cập nhật sản phẩm thành công"));
        }

        [HttpDelete("{id}")]
        public ActionResult<ResponseModel<bool>> Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(ResponseModel<bool>.ErrorResult("Không tìm thấy sản phẩm", 404));
            }

            _products.Remove(product);
            return Ok(ResponseModel<bool>.SuccessResult(true, "Xóa sản phẩm thành công"));
        }
    }
}