using API_and_DataBase.DTO;
using API_and_DataBase.DTO.Extension_Methods;
using API_and_DataBase.Models;
using API_and_DataBase.Structures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

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
        public async Task<ActionResult<TransactionsDTO>> TotalTransaction([FromQuery] string sdate, [FromQuery] string edate)
        {
            DateTime date = Convert.ToDateTime(sdate);
            DateTime Edta = Convert.ToDateTime(edate);

            //List<Transactions> transactions = await _context.Transactions.Where(w => w.Date.Date >= date.Date && w.Date.Date <= Edta.Date && w.ISDeleted == false).ToListAsync();
            //transactions = transactions.Where(w => w.Operation != (int)Operation.Expense).ToList();
            //var transactionsss = new List<Transactions>();
            //foreach (var item in transactions)
            //{
            //    if (users.Contains(users.FirstOrDefault(w => w.UserName == item.UserName && w.Type != (int)userType.Car && w.UserName!="محمد")))
            //    {

            //        transactionsss.Add(item);
            //    }
            //}


            List<Transactions> transactionsss = (from t in _context.Transactions
                                                 join u in _context.Users on t.UserName equals u.UserName
                                                 where u.Type != (int)userType.Car && t.Date.Date >= date.Date && t.Date.Date <= Edta.Date && t.ISDeleted == false
                                                 && t.Operation != (int)Operation.Expense
                                                 select t).ToList();


            decimal? Get = transactionsss.Where(w => w.Type == (int)TransType.Get ).Sum(w => w.Paid);
            decimal? Paid = transactionsss.Where(w => w.Type == (int)TransType.Paid).Sum(w => w.Paid);
            decimal? DifPaid = Get - Paid;
            decimal? Total = transactionsss.Sum(w => w.Paid);
            return Ok(new
            {
                Paid = Paid,
                Get = Get,
                Dif = DifPaid,
                Total = Total,
            });

        }
        [HttpGet("{carid}")]
        public async Task<ActionResult<TransactionsDTO>> CarTotalTransaction([FromQuery] string sdate, [FromQuery] string edate,int carid)
        {
            DateTime date = Convert.ToDateTime(sdate);
            DateTime Edta = Convert.ToDateTime(edate);
            List<Transactions> transactionsss = await (from t in _context.Transactions
                                                       join u in _context.Users on t.UserName equals u.UserName
                                                       where u.Type == (int)userType.Car && t.Date.Date >= date.Date && t.Date.Date <= Edta.Date && t.ISDeleted == false
                                                       && u.CarID== carid &&(t.Operation==(int)Operation.ExportReciept || t.Operation == (int)Operation.CustomerTrans)
                                                       select t).ToListAsync();

            decimal? Sell = transactionsss.Where(w => w.Type == (int)TransType.Get && w.Operation == (int)Operation.ExportReciept).Sum(w => w.Paid+w.Remaining);
            decimal? Collect = transactionsss.Where(w => w.Type == (int)TransType.Get && w.Operation == (int)Operation.CustomerTrans).Sum(w => w.Paid);
      
            return Ok(new
            {
                Transactions = transactionsss,
                Paid = Sell,
                Get = Collect
            });

        }

        [HttpGet("StoreTotal")]
        public async Task<ActionResult<TransactionsDTO>> StoreTotalTransaction([FromQuery] string sdate, [FromQuery] string edate)
        {
            DateTime date = Convert.ToDateTime(sdate);
            DateTime Edta = Convert.ToDateTime(edate);
            List<Transactions> transactionsss = await (from t in _context.Transactions
                                                 join u in _context.Users on t.UserName equals u.UserName
                                                 where u.Type != (int)userType.Car && t.Date.Date >= date.Date && t.Date.Date <= Edta.Date && t.ISDeleted == false
                                                 && t.Operation != (int)Operation.Expense
                                                 select t).ToListAsync();

            decimal? Get = transactionsss.Where(w => w.Type == (int)TransType.Get ).Sum(w => w.Paid);
            decimal? Paid = transactionsss.Where(w => w.Type == (int)TransType.Paid ).Sum(w => w.Paid);

            return Ok(new
            {
                transactions= transactionsss,
                Paid = Paid,
                Get = Get,
            });

        }


        [HttpGet("ProfitMargin")]
        public async Task<ActionResult<TransactionsDTO>> ProfitMargin([FromQuery] string sdate, [FromQuery] string edate, int? carid)
        {
            DateTime date = Convert.ToDateTime(sdate);
            DateTime Edta = Convert.ToDateTime(edate);
            var Receipts = await (from er in _context.ExportReciepts join ep in _context.ExportProducts
                                        on er.ID equals ep.ReceiptID
                                        join u in _context.Users on er.UserName equals u.UserName
                                        join P in _context.Products on ep.ProductID equals P.ID
                                        where er.ISDeleted== false && er.Date.Date >= date.Date && er.Date.Date <= Edta.Date
                                  group new { P,ep } by new { u.UserName, u.Type, er.ID, er.CustomerID, u.CarID,er.Date}into g
                                        orderby g.Key.ID
                                        select new
                                        {
                                            User = g.Key.UserName,
                                            Type = g.Key.Type,
                                            ID = g.Key.ID,
                                            CustomerID = g.Key.CustomerID,
                                            SellingPrice = g.Sum(x => x.ep.Price * x.ep.Quantity),
                                            BuyingPrice = g.Sum(x => x.P.BuyingPrice * x.ep.Quantity),
                                            CarID=g.Key.CarID,
                                            Date=g.Key.Date,
                                        }
                                        ).ToListAsync();


            if(carid!=null)
            {
                Receipts = Receipts.Where(x => x.Type == (int)userType.Car ).ToList();
            }
            else
            {
                Receipts = Receipts.Where(x => x.Type != (int)userType.Car).ToList();
            }
            decimal? Buyingprice = Receipts.Sum(w => w.BuyingPrice);
            decimal? SellingPrice = Receipts.Sum(w => w.SellingPrice);

            return Ok(new
            {
                Receipts = Receipts,
                Buyingprice = Buyingprice,
                SellingPrice = SellingPrice,
            });

        }

    }
}
