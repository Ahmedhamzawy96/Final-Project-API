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
using API_and_DataBase.Structures;
using Microsoft.AspNetCore.Authorization;

namespace API_and_DataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

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
            return await _context.ImportReciepts.Where(w=>w.ISDeleted==false).Select(A=>A.ImportRecieptToDTO()).ToListAsync();

        }

        // GET: api/ImportReciept/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImportRecieptDTO>> GetImportReciept(int id)
        {
            var imporReciept = await _context.ImportReciepts.FindAsync(id);
            List<ImportProduct> importproducts = _context.ImportProducts.Where(w => w.ReceiptID == id).ToList();

            if (imporReciept == null|| imporReciept.ISDeleted==true)
            {
                return NotFound();
            }
            ImportRecieptDTO importRecieptDTO = imporReciept.ImportRecieptToDTO();
            importRecieptDTO.importProducts = importproducts.Select(A => A.ImportProductToDTO()).ToArray();
            return importRecieptDTO;
        }

        // PUT: api/ImportReciept/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImportReciept(int id, ImportRecieptDTO importRecieptDTO)
        {
            ImportReciept importReciept = importRecieptDTO.DTOToImportReciept();

            if (id != importReciept.ID || importReciept.ISDeleted == true)
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
        [HttpPost]
        public async Task<ActionResult<ImportReciept>> PostImportReciept(ImportRecieptDTO importRecieptDTO)
        {
            ImportReciept importReciept = importRecieptDTO.DTOToImportReciept();
            _context.ImportReciepts.Add(importReciept);
            await _context.SaveChangesAsync();
            Transactions tr = new Transactions()
            {
                AccountID = importReciept.SupplierID,
                AccountType = (int)AccountType.Supplier,
                Remaining   = importReciept.Remaining,
                Paid   = importReciept.Paid,
                Type = (int)TransType.Paid,
                Date = importReciept.Date,
                OperationID = importReciept.ID,
                Operation = (int)Operation.ImportReciept,
                UserName = importReciept.UserName,

            };
            Supplier sup = _context.Suppliers.Find(importReciept.SupplierID);
            sup.Account += importReciept.Remaining;
            _context.Entry(sup).State = EntityState.Modified;
            _context.Transactions.Add(tr);
            await _context.SaveChangesAsync();
            foreach (var item in importRecieptDTO.importProducts)
            {

                item.ImportReceiptID = importReciept.ID;
                _context.ImportProducts.Add(item.DTOToImportProduct());

                Product product = _context.Products.Find(item.ProductID);
                product.Quantity += item.Quantity;
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
            if (id != importReciept.ID)
            {
                return BadRequest();
            }
            importReciept.ISDeleted = true;
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

        private bool ImportRecieptExists(int id)
        {
            return _context.ImportReciepts.Any(e => e.ID == id&&e.ISDeleted==false);
        }
    }
}
