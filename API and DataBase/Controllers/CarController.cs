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
 

    public class CarController : ControllerBase
    {
        private readonly CompanyContext _context;
       
        

        public CarController(CompanyContext context)
        {
       
            _context = context;
        }

        // GET: api/Car
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDTO>>> GetCars()
        {

            return await _context.Cars.Where(w=>w.ISDeleted==false).Select(w=>w.CarToDTO()).ToListAsync();
        }
        [HttpGet ]
        [Route("available")]
        public async Task<ActionResult<IEnumerable<CarDTO>>> Getavailable()
        {
            var users = _context.Users.ToList().Where(w => w.ISDeleted == false);
            var cars = _context.Cars.ToList().Where(w=> w.ISDeleted == false);
            var available = new List<Car>();
            foreach (var item in cars)
            {
                if (!users.Contains(users.FirstOrDefault(w => w.CarID == item.ID)))
                {
                    available.Add(item);
                }
            }
            return Ok( available.Select(w => w.CarToDTO()));
        }
        // GET: api/Car/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CarDTO>> GetCar(int id)
        {
            Car car = await _context.Cars.FindAsync(id);
            
            if (car == null || car.ISDeleted==true)
            {
                return NotFound();
            }
            return car.CarToDTO();
        }

        // PUT: api/Car/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id, CarDTO carDTO)
        {
            Car car=carDTO.DTOToCar();
            if (id != car.ID &&car.ISDeleted==true)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
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

        // POST: api/Car

        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(CarDTO carDTO)
        {
            Car car= carDTO.DTOToCar();
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCar", new { id = car.ID }, car);
        }

        // DELETE: api/Car/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (id != car.ID)
            {
                return BadRequest();
            }
            car.ISDeleted = true;
            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
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

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.ID == id&&e.ISDeleted==false);
        }
    }
}
