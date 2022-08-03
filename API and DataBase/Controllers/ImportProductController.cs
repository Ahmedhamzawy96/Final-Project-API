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
    public class ImportProductController : ControllerBase
    {
        private readonly CompanyContext _context;

        public ImportProductController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/ImportProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportProduct>>> GetImportProducts()
        {
            return await _context.ImportProducts.ToListAsync();
        }

        // GET: api/ImportProduct/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportProduct>> GetImportProduct(int id)
        {
            var importProduct = await _context.ImportProducts.FindAsync(id);

            if (importProduct == null)
            {
                return NotFound();
            }

            return importProduct;
        }

        // PUT: api/ImportProduct/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImportProduct(int id, ImportProduct importProduct)
        {
            if (id != importProduct.ReceiptID)
            {
                return BadRequest();
            }

            _context.Entry(importProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImportProductExists(id))
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

        // POST: api/ImportProduct
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImportProduct>> PostImportProduct(ImportProduct importProduct)
        {
            _context.ImportProducts.Add(importProduct);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ImportProductExists(importProduct.ReceiptID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetImportProduct", new { id = importProduct.ReceiptID }, importProduct);
        }

        // DELETE: api/ImportProduct/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImportProduct(int id)
        {
            var importProduct = await _context.ImportProducts.FindAsync(id);
            if (importProduct == null)
            {
                return NotFound();
            }

            _context.ImportProducts.Remove(importProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImportProductExists(int id)
        {
            return _context.ImportProducts.Any(e => e.ReceiptID == id);
        }
    }
}
