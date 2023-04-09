using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_and_DataBase.Models;
using API_and_DataBase.DTO.Extension_Methods;
using API_and_DataBase.DTO;
using Microsoft.AspNetCore.Authorization;

namespace API_and_DataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 

    public class ProductController : ControllerBase
    {
        private readonly CompanyContext _context;

        public ProductController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            return await _context.Products.Where(w => w.ISDeleted == false).Select(A => A.ProductToDTO()).ToListAsync();

        }

        // GET: api/Product inCar
        [HttpGet("car/{CarID:int}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsInCar(int CarID)
        {
            var CarProducts = _context.CarProducts.ToList().Where(A => A.ISDeleted == false && A.CarID == CarID);
            var Prodducts = _context.Products.ToList().Where(A => A.ISDeleted == false);
            var ProductsInCar = new List<ProductDTO>();
            foreach (var item in Prodducts)
            {
                if (CarProducts.Contains(CarProducts.FirstOrDefault(A => A.ProductID == item.ID)))
                {
                    item.Quantity = CarProducts.Where(A => A.ProductID == item.ID).Select(A => A.Quantity).FirstOrDefault();
                    ProductsInCar.Add(item.ProductToDTO());
                }
            }
            return Ok(ProductsInCar);
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product.ProductToDTO();
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductDTO productDTO)
        {
            Product product = productDTO.DTOToProduct();
            if (id != product.ID || product.ISDeleted == true)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }

            return NoContent();
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductDTO productDTO)
        {
            Product product = productDTO.DTOToProduct();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ID }, product);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (id != product.ID)
            {
                return BadRequest();
            }
            product.ISDeleted = true;
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }

            return NoContent();
        }
        [HttpPut("UpdateProductPrice/{Id}")]
        public async Task<ActionResult> UpdateProductPrice(int Id ,ProductPriceDTO productPriceDTO)
        {
            var UpdatedProduct = await _context.Products.FindAsync(Id);
            UpdatedProduct.BuyingPrice = productPriceDTO.buyingPrice;
            UpdatedProduct.SellingPrice = productPriceDTO.SellingPrice;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ID == id&&e.ISDeleted==false);
        }

        [HttpGet("ProductByQuantity/{Quantity}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByQuantity(int Quantity)
        {
            return await _context.Products.Where(x => !x.ISDeleted && x.Quantity <= Quantity).Select(x => x.ProductToDTO()).ToListAsync();
        }
 
    }
}
