using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_and_DataBase.Models;

namespace API_and_DataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarProductController : ControllerBase
    {
        private readonly CompanyContext _context;

        public CarProductController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/CarProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarProduct>>> GetCarProducts()
        {
            return await _context.CarProducts.ToListAsync();
        }

        // GET: api/CarProduct/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CarProduct>> GetCarProduct(int id)
        {
            var carProduct = await _context.CarProducts.FindAsync(id);

            if (carProduct == null)
            {
                return NotFound();
            }

            return carProduct;
        }

        // PUT: api/CarProduct/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarProduct(int id, CarProduct carProduct)
        {
            if (id != carProduct.CarID)
            {
                return BadRequest();
            }

            _context.Entry(carProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CarProduct
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CarProduct>> PostCarProduct(CarProduct carProduct)
        {
            _context.CarProducts.Add(carProduct);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CarProductExists(carProduct.CarID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCarProduct", new { id = carProduct.CarID }, carProduct);
        }

        // DELETE: api/CarProduct/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarProduct(int id)
        {
            var carProduct = await _context.CarProducts.FindAsync(id);
            if (carProduct == null)
            {
                return NotFound();
            }

            _context.CarProducts.Remove(carProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarProductExists(int id)
        {
            return _context.CarProducts.Any(e => e.CarID == id);
        }
    }
}
