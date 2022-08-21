using API_and_DataBase.DTO;
using API_and_DataBase.DTO.Extension_Methods;
using API_and_DataBase.Models;
using API_and_DataBase.Structures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_and_DataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly CompanyContext _context;

        public ReportController(CompanyContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("Customer")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            return await _context.Customers.Select(w => w.CustomerToDTO()).ToListAsync();
        }

        [HttpGet]
        [Route("Supplier")]

        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetSupplier()
        {
            return await _context.Suppliers.Select(w => w.SupplierToDTO()).ToListAsync();
        }
        [HttpGet]
        [Route("Car")]
        public async Task<ActionResult<IEnumerable<CarDTO>>> GetCars()
        {

            return await _context.Cars.Select(w => w.CarToDTO()).ToListAsync();
        }

        [HttpGet("{id}/{type}")]
        public async Task<ActionResult<TransactionsDTO>> CustomerTransaction(int id, int type,[FromQuery] string sdate, [FromQuery] string edate)

        {
            DateTime date = Convert.ToDateTime(sdate);
            DateTime Edta = Convert.ToDateTime(edate);
            bool x = (date.Date.Ticks > Edta.Date.Ticks);



            List<Transactions> transactions = await _context.Transactions.Where(A => A.AccountType == type && A.AccountID == id && A.ISDeleted 
            == false).ToListAsync();
            transactions = transactions.Where(w => w.Date.Ticks >= date.Ticks&&w.Date.Ticks<=Edta.Ticks).ToList();
            foreach (var item in transactions)
            {
                if(item.ISDeleted==true)
                {
                    item.Notes = "تم حذف هذه العملية";
                }
            }
            if (transactions == null)
            {
                return NotFound();
            }
            return Ok(transactions.Select(A => A.TransactionsToDTO()));
        }
        [HttpGet("Total")]
        public async Task<ActionResult<TransactionsDTO>> TotalTransaction( [FromQuery] string sdate, [FromQuery] string edate)

        {
            DateTime date = Convert.ToDateTime(sdate);
            DateTime Edta = Convert.ToDateTime(edate);
            
            List<Transactions> transactions = await _context.Transactions.Where(w=> w.ISDeleted== false).ToListAsync();
            transactions = transactions.Where(w => w.Date.Ticks >= date.Ticks&&w.Date.Ticks<=Edta.Ticks).ToList();

            decimal? Get = transactions.Where(w => w.Type == (int)TransType.Get).Sum(w=>w.Paid);
            decimal? Paid = transactions.Where(w => w.Type == (int)TransType.Paid).Sum(w => w.Paid);
            decimal? DifPaid = Get-Paid;
            decimal? Total = transactions.Sum(w=>w.Paid);
            return Ok(new
            {
                Paid = Paid,
                Get = Get,
                Dif = DifPaid,
                Total = Total,
            });

        }


    }
}
