﻿using System;
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
    public class ImportProductController : ControllerBase
    {
        private readonly CompanyContext _context;

        public ImportProductController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/ImportProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImportProductDTO>>> GetImportProducts()
        {

            return await _context.ImportProducts.Select(A=> A.ImportProductToDTO()).ToListAsync();
        }

        // GET: api/ImportProduct/5


        // GET: api/ImportProduct/5
        [HttpGet("{id}/{ReceiptID}")]
        public async Task<ActionResult<ImportProductDTO>> GetImportProduct(int id, int ReceiptID)
        {
            var importProduct = await _context.ImportProducts.FirstOrDefaultAsync(w=>w.ProductID==id&&w.ReceiptID==ReceiptID);

            if (importProduct == null)
            {
                return NotFound();
            }

            return importProduct.ImportProductToDTO();
        }

        // PUT: api/ImportProduct/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPut("{id}/{ReceiptID}")]
        public async Task<IActionResult> PutImportProduct(int id, int ReceiptID, ImportProductDTO importProductDTO)
        {
            ImportProduct importProduct = importProductDTO.DTOToImportProduct();

            if (id != importProduct.ProductID &&ReceiptID!=importProduct.ReceiptID)
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

                if (!ImportProductExists(id,ReceiptID))
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
        public async Task<ActionResult<ImportProduct>> PostImportProduct(ImportProductDTO importProductDTO)

        {
            ImportProduct importProduct = importProductDTO.DTOToImportProduct();
            _context.ImportProducts.Add(importProduct);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

                if (ImportProductExists(importProduct.ProductID, importProduct.ReceiptID))
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

        [HttpDelete("{id}/{ReceiptID}")]
        public async Task<IActionResult> DeleteImportProduct(int id ,int ReceiptID)
        {
            var importProduct = await _context.ImportProducts.FirstOrDefaultAsync(w => w.ProductID == id && w.ReceiptID == ReceiptID);
            if (importProduct == null)
            {
                return NotFound();
            }

            _context.ImportProducts.Remove(importProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool ImportProductExists(int id, int ReceiptID)
        {
            return _context.ImportProducts.Any(e => e.ProductID == id&&e.ReceiptID==ReceiptID);
        }
    }
}
