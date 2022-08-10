using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_and_DataBase.Models;
using API_and_DataBase.DTO;
using API_and_DataBase.DTO.Extension_Methods;

namespace API_and_DataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportRecieptController : ControllerBase
    {
        private readonly CompanyContext _context;

        public ExportRecieptController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/ExportReciept
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExportRecieptDTO>>> GetExportReciepts()
        {
            return await _context.ExportReciepts.Select(w=>w.ExportRecieptToDTO()).ToListAsync();
        }

        // GET: api/ExportReciept/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExportRecieptDTO>> GetExportReciept(int id)
        {
            var exportReciept = await _context.ExportReciepts.FindAsync(id);

            if (exportReciept == null)
            {
                return NotFound();
            }

            return exportReciept.ExportRecieptToDTO();
        }

        // PUT: api/ExportReciept/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExportReciept(int id, ExportRecieptDTO exportRecieptDTO)
        {
            ExportReciept exportReciept = exportRecieptDTO.DTOToExportReciept();
            if (id != exportReciept.ID)
            {
                return BadRequest();
            }

            _context.Entry(exportReciept).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExportRecieptExists(id))
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

        // POST: api/ExportReciept
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExportReciept>> PostExportReciept(ExportRecieptDTO exportRecieptDTO)
        {
            ExportReciept exportReciept = exportRecieptDTO.DTOToExportReciept();
            Transactions tr = new Transactions()
            {
                ReceiptID = exportReciept.ID,
                Amount = exportReciept.Remaining,
                Date = exportReciept.Date,
                ReceiptType = "Export",
                User = exportReciept.UserName,
                Type = "فاتوره بيع",
                Receiver = _context.Customers.Where(w => w.ID == exportReciept.CustomerID).Select(w => w.Name).FirstOrDefault()
            };

            Customer Cust = _context.Customers.Find(exportReciept.CustomerID);
            Cust.Account += exportReciept.Remaining;
            _context.Entry(Cust).State = EntityState.Modified;
            _context.ExportReciepts.Add(exportReciept);
            _context.Transactions.Add(tr);
            await _context.SaveChangesAsync();
            foreach (var item in exportRecieptDTO.Products)
            {

                item.ExportReceiptID = exportReciept.ID;
                _context.ExportProducts.Add(item.DTOToExportProduct());

                Product product=_context.Products.Find(item.ProductID);
                product.Quantity -= item.Quantity;
                _context.Entry(product).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExportReciept", new { id = exportReciept.ID }, exportReciept);
        }

        // DELETE: api/ExportReciept/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExportReciept(int id)
        {
            var exportReciept = await _context.ExportReciepts.FindAsync(id);
            if (exportReciept == null)
            {
                return NotFound();
            }

            _context.ExportReciepts.Remove(exportReciept);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExportRecieptExists(int id)
        {
            return _context.ExportReciepts.Any(e => e.ID == id);
        }
    }
}
