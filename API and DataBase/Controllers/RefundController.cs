using API_and_DataBase.DTO;
using API_and_DataBase.DTO.Extension_Methods;
using API_and_DataBase.Models;
using API_and_DataBase.Structures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_and_DataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class RefundController : ControllerBase
    {

        private readonly CompanyContext _context;

        public RefundController(CompanyContext context)
        {
            _context = context;
        }

        //export reciept refund
        [HttpPost("Export/{id}")]
        public async Task<ActionResult<ExportReciept>> ERecieptRefund(int id, ExportRecieptDTO recieptDTO)
        {
            ExportReciept rec = _context.ExportReciepts.FirstOrDefault(A => A.ID == id);
            ExportReciept refundRec = recieptDTO.DTOToExportReciept();
            _context.ExportReciepts.Add(refundRec);
            await _context.SaveChangesAsync();
            Transactions tr = new Transactions();
            tr = new Transactions()
            {
                AccountID = refundRec.CustomerID,
                AccountType = (int)AccountType.Customer,
                Remaining = refundRec.Remaining,
                Paid = refundRec.Paid,
                Type = (int)TransType.Get,
                Date = refundRec.Date,
                OperationID = refundRec.ID,
                Operation = (int)Operation.ExportReciept,
                UserName = refundRec.UserName,
                Notes = $"{rec.ID} مرتجع فاتورة رقم"
            };
            foreach (var item in recieptDTO.Products)
            {
                item.ExportReceiptID = recieptDTO.ID;
                _context.ExportProducts.Add(item.DTOToExportProduct());
                Product product = _context.Products.Find(item.ProductID);
                product.Quantity -= item.Quantity;
            }
            Customer Cust = _context.Customers.Find(refundRec.CustomerID);
            Cust.Account += refundRec.Remaining;
            _context.Entry(Cust).State = EntityState.Modified;
            _context.Transactions.Add(tr);
            foreach (var item in _context.ExportProducts.Where(A => A.ReceiptID == rec.ID).ToList())
            {
                item.ISDeleted = true;
                Product product = _context.Products.Find(item.ProductID);
                product.Quantity += item.Quantity;

            }
            rec.Notes = rec.Notes + " " + $" {refundRec.ID} فاتورة المرتجع رقم ";
            rec.ISDeleted = true;
            _context.Entry(rec).State = EntityState.Modified;
            var trans = _context.Transactions.FirstOrDefault(A => A.Operation == (int)Operation.ExportReciept && A.OperationID == rec.ID);
            trans.ISDeleted = true;
            _context.Entry(trans).State = EntityState.Modified;
            Cust.Account -= rec.Remaining;
            _context.Entry(Cust).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // import reciept refund
        [HttpPost("Import/{id}")]
        public async Task<ActionResult<ImportReciept>> IRecieptRefund(int id, ImportRecieptDTO recieptDTO)
        {
            ImportReciept rec = _context.ImportReciepts.FirstOrDefault(A => A.ID == id);
            ImportReciept refundRec = recieptDTO.DTOToImportReciept();
            _context.ImportReciepts.Add(refundRec);
            await _context.SaveChangesAsync();
            Transactions tr = new Transactions();
            tr = new Transactions()
            {
                AccountID = refundRec.SupplierID,
                AccountType = (int)AccountType.Supplier,
                Remaining = refundRec.Remaining,
                Paid = refundRec.Paid,
                Type = (int)TransType.Paid,
                Date = refundRec.Date,
                OperationID = refundRec.ID,
                Operation = (int)Operation.ImportReciept,
                UserName = refundRec.UserName,
                Notes = $"{rec.ID} مرتجع فاتورة رقم"
            };
            foreach (var item in recieptDTO.importProducts)
            {
                item.ImportReceiptID = recieptDTO.ID;
                _context.ImportProducts.Add(item.DTOToImportProduct());

                Product product = _context.Products.Find(item.ProductID);
                product.Quantity += item.Quantity;
            }
            Supplier supp = _context.Suppliers.Find(refundRec.SupplierID);
            supp.Account += refundRec.Remaining;
            _context.Entry(supp).State = EntityState.Modified;
            _context.Transactions.Add(tr);
            foreach (var item in _context.ImportProducts.Where(A => A.ReceiptID == rec.ID).ToList())
            {
                item.ISDeleted = true;
                Product product = _context.Products.Find(item.ProductID);
                product.Quantity -= item.Quantity;
            }
            rec.Notes = rec.Notes + " " + $" {refundRec.ID} فاتورة المرتجع رقم ";
            rec.ISDeleted = true;
            _context.Entry(rec).State = EntityState.Modified;
            var trans = _context.Transactions.FirstOrDefault(A => A.Operation == (int)Operation.ImportReciept && A.OperationID == rec.ID);
            trans.ISDeleted = true;
            _context.Entry(trans).State = EntityState.Modified;
            supp.Account -= rec.Remaining;
            _context.Entry(supp).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //reciept from Car Refund

        [HttpPost("Car/{id}")]
        public async Task<ActionResult<ExportReciept>> CarRecieptRefund(int id, ExportRecieptDTO recieptDTO)
        {
            ExportReciept rec = _context.ExportReciepts.FirstOrDefault(A => A.ID == id && A.CarID == recieptDTO.CarID);
            ExportReciept refundRec = recieptDTO.DTOToExportReciept();
            _context.ExportReciepts.Add(refundRec);
            await _context.SaveChangesAsync();
            Transactions tr = new Transactions();
            tr = new Transactions()
            {
                AccountID = refundRec.CustomerID,
                AccountType = (int)AccountType.Customer,
                Remaining = refundRec.Remaining,
                Paid = refundRec.Paid,
                Type = (int)TransType.Get,
                Date = refundRec.Date,
                OperationID = refundRec.ID,
                Operation = (int)Operation.ExportReciept,
                UserName = refundRec.UserName,
                Notes = $"{rec.ID} مرتجع فاتورة رقم"
            };
            foreach (var item in recieptDTO.Products)
            {
                item.ExportReceiptID = recieptDTO.ID;
                _context.ExportProducts.Add(item.DTOToExportProduct());

                CarProduct product = _context.CarProducts.FirstOrDefault(w => w.ProductID == item.ProductID && w.CarID == recieptDTO.CarID);
                product.Quantity -= item.Quantity;
            }
            Customer Cust = _context.Customers.Find(refundRec.CustomerID);
            Cust.Account += refundRec.Remaining;
            _context.Entry(Cust).State = EntityState.Modified;
            _context.Transactions.Add(tr);
            foreach (var item in _context.ExportProducts.Where(A => A.ReceiptID == rec.ID).ToList())
            {
                item.ISDeleted = true;
                CarProduct product = _context.CarProducts.Find(item.ProductID);
                product.Quantity += item.Quantity;

            }
            rec.Notes = rec.Notes + " " + $" {refundRec.ID} فاتورة المرتجع رقم ";
            rec.ISDeleted = true;
            _context.Entry(rec).State = EntityState.Modified;
            var trans = _context.Transactions.FirstOrDefault(A => A.Operation == (int)Operation.ExportReciept && A.OperationID == rec.ID);
            trans.ISDeleted = true;
            _context.Entry(trans).State = EntityState.Modified;
            Cust.Account -= rec.Remaining;
            _context.Entry(Cust).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        //reciept to Car Refund

        [HttpPost("toCar/{id}")]
        public async Task<ActionResult<ExportReciept>> toCarRecieptRefund(int id, ExportRecieptDTO recieptDTO)
        {
            ExportReciept rec = _context.ExportReciepts.FirstOrDefault(A => A.ID == id && A.CarID == recieptDTO.CarID);
            ExportReciept refundRec = recieptDTO.DTOToExportReciept();
            _context.ExportReciepts.Add(refundRec);
            await _context.SaveChangesAsync();
            Transactions tr = new Transactions();
            Car car = _context.Cars.Find(recieptDTO.CarID);
            tr = new Transactions()
            {
                AccountID = refundRec.CarID,
                AccountType = (int)AccountType.Car,
                Remaining = refundRec.Remaining,
                Paid = refundRec.Paid,
                Type = (int)TransType.Get,
                Date = refundRec.Date,
                OperationID = refundRec.ID,
                Operation = (int)Operation.ExportReciept,
                UserName = refundRec.UserName,
                Notes = $"{rec.ID} مرتجع فاتورة رقم"
            };
            foreach (var item in recieptDTO.Products)
            {
                item.ExportReceiptID = recieptDTO.ID;
                if (_context.CarProducts.FirstOrDefault(w => w.ProductID == item.ProductID) == null)
                {
                    CarProduct carpr = new CarProduct()
                    {
                        CarID = recieptDTO.CarID,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                    };
                    _context.CarProducts.Add(carpr);
                }
                else
                {
                    CarProduct carpr = _context.CarProducts.FirstOrDefault(w => w.ProductID == item.ProductID);
                    carpr.Quantity += item.Quantity;
                    _context.Entry(carpr).State = EntityState.Modified;
                }

                CarProduct product = _context.CarProducts.FirstOrDefault(w => w.ProductID == item.ProductID && w.CarID == recieptDTO.CarID);
                product.Quantity -= item.Quantity;
            }
            car.Account += refundRec.Remaining;
            _context.Entry(car).State = EntityState.Modified;
            _context.Transactions.Add(tr);
            foreach (var item in _context.ExportProducts.Where(A => A.ReceiptID == rec.ID).ToList())
            {
                item.ISDeleted = true;
                Product product = _context.Products.Find(item.ProductID);
                product.Quantity += item.Quantity;
                CarProduct Carproduct = _context.CarProducts.Find(item.ProductID);
                Carproduct.Quantity -= item.Quantity;

            }
            rec.Notes = rec.Notes + " " + $" {refundRec.ID} فاتورة المرتجع رقم ";
            rec.ISDeleted = true;
            _context.Entry(rec).State = EntityState.Modified;
            var trans = _context.Transactions.FirstOrDefault(A => A.Operation == (int)Operation.ExportReciept && A.OperationID == rec.ID);
            trans.ISDeleted = true;
            _context.Entry(trans).State = EntityState.Modified;
            car.Account -= rec.Remaining;
            _context.Entry(car).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("ReceiptsFilter/{id}")]
        public async Task<ActionResult<ExportRecieptDTO>> ReceiptsFilter(int id)
        {
            if (id == 0)
            {
                List<ExportRecieptDTO> rec = await _context.ExportReciepts.Where(w => w.UserName == (_context.Users.FirstOrDefault(a => a.UserName == w.UserName && a.Type != (int)userType.Car).UserName) && w.CarID == null && w.ISDeleted == false).Select(w => w.ExportRecieptToDTO()).ToListAsync();
                return Ok(rec);
            }
            else if (id == 2)
            {
                List<ExportRecieptDTO> rec = await _context.ExportReciepts.Where(w => w.UserName == (_context.Users.FirstOrDefault(a => a.UserName == w.UserName && a.Type == (int)userType.Car).UserName) && w.ISDeleted == false).Select(w => w.ExportRecieptToDTO()).ToListAsync();
                return Ok(rec);
            }
            else if (id == 3)
            {
                List<ExportRecieptDTO> rec = await _context.ExportReciepts.Where(w => w.CarID != null && w.ISDeleted == false).Select(w => w.ExportRecieptToDTO()).ToListAsync();
                return Ok(rec);

            }
            else
            {
                return NoContent();
            }
        }
    }
}
