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
    public class RefundController : ControllerBase
    {

        private readonly CompanyContext _context;

        public RefundController(CompanyContext context)
        {
            _context = context;
        }
        [HttpPost("{id}")]
        public async Task<ActionResult<ExportReciept>> RecieptRefund(int id , ExportRecieptDTO recieptDTO)
        {
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
            ExportReciept rec = _context.ExportReciepts.FirstOrDefault(A => A.ID == id);
            foreach (var item in _context.ExportProducts.ToList())
            {
                if(item.ReceiptID == rec.ID)
                {
                    item.ISDeleted = true;
                    Product product = _context.Products.Find(item.ProductID);
                    product.Quantity += item.Quantity;

                }
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
    }
}
