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
    public class ExportProductController : ControllerBase
    {
        private readonly CompanyContext _context;

        public ExportProductController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/ExportProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExportProduct>>> GetExportProducts()
        {
            return await _context.ExportProducts.ToListAsync();
        }

        // GET: api/ExportProduct/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExportProduct>> GetExportProduct(int id)
        {
            var exportProduct = await _context.ExportProducts.FindAsync(id);

            if (exportProduct == null)
            {
                return NotFound();
            }

            return exportProduct;
        }

        // PUT: api/ExportProduct/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExportProduct(int id, ExportProduct exportProduct)
        {
            if (id != exportProduct.ReceiptID)
            {
                return BadRequest();
            }

            _context.Entry(exportProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExportProductExists(id))
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

        // POST: api/ExportProduct
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExportProduct>> PostExportProduct(ExportProduct exportProduct)
        {
            _context.ExportProducts.Add(exportProduct);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ExportProductExists(exportProduct.ReceiptID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetExportProduct", new { id = exportProduct.ReceiptID }, exportProduct);
        }

        // DELETE: api/ExportProduct/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExportProduct(int id)
        {
            var exportProduct = await _context.ExportProducts.FindAsync(id);
            if (exportProduct == null)
            {
                return NotFound();
            }

            _context.ExportProducts.Remove(exportProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExportProductExists(int id)
        {
            return _context.ExportProducts.Any(e => e.ReceiptID == id);
        }
    }
}
