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
        [HttpGet("{carID}")]
        public ActionResult<IEnumerable<CarProductDTO>> GetCarProducts(int carID)
        {

            var CarProducts = from P in _context.Products
                              join CP in _context.CarProducts.Where(A => A.CarID == carID)
                              on P.ID equals CP.ProductID
                              select new CarProductDTO
                              {
                                  CarID = CP.CarID,
                                  ProductID = CP.ProductID,
                                  ProductName = P.Name,
                                  Quantity = CP.Quantity
                              };


            return Ok(CarProducts);
        }            

        // GET: api/CarProduct/5
        [HttpGet("{id}/{carID}")]
        public async Task<ActionResult<CarProductDTO>> GetCarProduct( int id ,int carID)
        {
            var carProduct = await _context.CarProducts.FirstOrDefaultAsync(A=> A.ProductID == id && A.CarID == carID);



            if (carProduct == null)
            {
                return NotFound();
            }

            return carProduct.CarProductToDTO();
        }

        // PUT: api/CarProduct/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/{carID}")]
        public async Task<IActionResult> PutCarProduct(int id,int carID, CarProductDTO carProductDTO)

        {
            CarProduct carProduct = carProductDTO.DTOToCarProduct();
            if (id != carProduct.ProductID && carProduct.CarID != carID)
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
                if (!CarProductExists(id, carID))
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

        [HttpPost]
        public async Task<ActionResult<CarProduct>> PostCarProduct(CarProductDTO carProductDTO)
        {
            CarProduct carProduct = carProductDTO.DTOToCarProduct();
            _context.CarProducts.Add(carProduct);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CarProductExists(carProduct.CarID, carProduct.ProductID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCarProduct", new { id = carProduct.CarID, carID = carProduct.CarID }, carProduct);

        }

        // DELETE: api/CarProduct/5
        [HttpDelete("{id}/{carID}")]
        public async Task<IActionResult> DeleteCarProduct(int id , int carID)
        {
            var carProduct = await _context.CarProducts.FirstOrDefaultAsync(A=> A.ProductID == id && A.CarID == carID);

            if (carProduct == null)
            {
                return NotFound();
            }

            _context.CarProducts.Remove(carProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarProductExists(int id , int carID)

        {
            return _context.CarProducts.Any(e => e.CarID == id && e.CarID == carID);
        }
    }
}
