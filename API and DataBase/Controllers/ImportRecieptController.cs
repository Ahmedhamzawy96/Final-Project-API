﻿using System;
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
    public class ImportRecieptController : ControllerBase
    {
        private readonly CompanyContext _context;

        public ImportRecieptController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/ImportReciept
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportRecieptDTO>>> GetImportReciepts()
        {
            return await _context.ImportReciepts.Select(A=>A.ImportRecieptToDTO()).ToListAsync();

        }

        // GET: api/ImportReciept/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportRecieptDTO>> GetImportReciept(int id)
        {
            var imporReciept = await _context.ImportReciepts.FindAsync(id);
            List<ImportProduct> importproducts = _context.ImportProducts.Where(w => w.ReceiptID == id).ToList();

            if (imporReciept == null)
            {
                return NotFound();
            }
            ImportRecieptDTO importRecieptDTO = imporReciept.ImportRecieptToDTO();
            importRecieptDTO.importProducts = importproducts.Select(A => A.ImportProductToDTO()).ToArray();
            return importRecieptDTO;
        }

        // PUT: api/ImportReciept/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImportReciept(int id, ImportRecieptDTO importRecieptDTO)
        {
            ImportReciept importReciept = importRecieptDTO.DTOToImportReciept();

            if (id != importReciept.ID)
            {
                return BadRequest();
            }

            _context.Entry(importReciept).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImportRecieptExists(id))
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

        // POST: api/ImportReciept
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImportReciept>> PostImportReciept(ImportRecieptDTO importRecieptDTO)
        {
            ImportReciept importReciept = importRecieptDTO.DTOToImportReciept();
            Transactions tr = new Transactions()
            {
                ReceiptID = importReciept.ID,
                Amount = importReciept.Remaining,
                Date = importReciept.Date,
                ReceiptType = "Import",
                User = importReciept.UserName,
                Type = "فاتوره شراء",
                Receiver = _context.Customers.Where(w => w.ID == importReciept.SupplierID).Select(w => w.Name).FirstOrDefault()
            };
            Supplier sup = _context.Suppliers.Find(importReciept.SupplierID);
            sup.Account += importReciept.Remaining;
            _context.Entry(sup).State = EntityState.Modified;
            _context.Transactions.Add(tr);
            _context.ImportReciepts.Add(importReciept);
            await _context.SaveChangesAsync();
            foreach (var item in importRecieptDTO.importProducts)
            {

                item.ImportReceiptID = importReciept.ID;
                _context.ImportProducts.Add(item.DTOToImportProduct());

                Product product = _context.Products.Find(item.ProductID);
                product.Quantity -= item.Quantity;
                _context.Entry(product).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImportReciept", new { id = importReciept.ID }, importReciept.ImportRecieptToDTO());
        }

        // DELETE: api/ImportReciept/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImportReciept(int id)
        {
            var importReciept = await _context.ImportReciepts.FindAsync(id);
            if (importReciept == null)
            {
                return NotFound();
            }

            _context.ImportReciepts.Remove(importReciept);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImportRecieptExists(int id)
        {
            return _context.ImportReciepts.Any(e => e.ID == id);
        }
    }
}
