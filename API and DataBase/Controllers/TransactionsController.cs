﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_and_DataBase.Models;
using API_and_DataBase.Structures;
using API_and_DataBase.DTO;
using API_and_DataBase.DTO.Extension_Methods;
using Microsoft.AspNetCore.Authorization;

namespace API_and_DataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class TransactionsController : ControllerBase
    {
        private readonly CompanyContext _context;

        public TransactionsController(CompanyContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionsDTO>>> GetTransactions()
        {
            return await _context.Transactions.Where(w=>w.ISDeleted==false).Select(A => A.TransactionsToDTO()).ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionsDTO>> GetTransactions(int id)
        {
            var transactions = await _context.Transactions.FindAsync(id);

            if (transactions == null||transactions.ISDeleted==true)
            {
                return NotFound();
            }

            return transactions.TransactionsToDTO();
        }
        [HttpGet("{id}/{type}")]
        public async Task<ActionResult<TransactionsDTO>> GetTransactions(int id , int type)
        {
            List<Transactions> transactions = await _context.Transactions.Where(A => A.AccountType == type && A.AccountID == id&&A.ISDeleted==false).ToListAsync();

            if (transactions == null )
            {
                return NotFound();
            }

            return Ok(transactions.Select(A => A.TransactionsToDTO()));
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactions(int id, TransactionsDTO transactionsDTO)
        {
            Transactions transactions = transactionsDTO.DTOTOTransactions();
            if (id != transactions.ID||transactions.ISDeleted==true)
            {
                return BadRequest();
            }

            _context.Entry(transactions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionsExists(id))
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

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult<Transactions>> PostTransactions(TransactionsDTO transactionsDTO)
        {
            Transactions transactions = transactionsDTO.DTOTOTransactions();

            _context.Transactions.Add(transactions);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransactions", new { id = transactions.ID }, transactions);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactions(int id)
        {
            var transactions = await _context.Transactions.FindAsync(id);
            if (id != transactions.ID)
            {
                return BadRequest();
            }
            transactions.ISDeleted = true;
            _context.Entry(transactions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionsExists(id))
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

        private bool TransactionsExists(int id)
        {
            return _context.Transactions.Any(e => e.ID == id &&e.ISDeleted == false);
        }
    }
}
