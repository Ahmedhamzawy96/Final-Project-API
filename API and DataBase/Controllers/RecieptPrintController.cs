using API_and_DataBase.DTO;
using API_and_DataBase.DTO.Extension_Methods;
using API_and_DataBase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NReco.PdfGenerator;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace API_and_DataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecieptPrintController : Controller
    {
        private readonly CompanyContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RecieptPrintController(CompanyContext context , IWebHostEnvironment WebHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = WebHostEnvironment;
        }

        [HttpGet("ExportRecieptPrint/{Id}")]
        public async Task<IActionResult> PrintEReciept(int Id) 
        {
            var model = await  _context.ExportReciepts.Include(x => x.Customer).Where(x => x.ID == Id && !x.ISDeleted).FirstOrDefaultAsync();
            model.CurrentAccount = (model.CurrentAccount == 0 || model.CurrentAccount == null )? model.Customer.Account : model.CurrentAccount;
            model.PreviousAccount = (model.PreviousAccount == 0 || model.PreviousAccount == null ) ? (model.Customer.Account - model.Remaining) : model.PreviousAccount;
            model.ExportProducts = await _context.ExportProducts.Include(x => x.Product).Where(w => w.ReceiptID == Id).ToListAsync();
            var data = System.IO.File.ReadAllText(_webHostEnvironment.ContentRootPath + "Templates\\ExportReciept.html");
            var products = System.IO.File.ReadAllText(_webHostEnvironment.ContentRootPath + "Templates\\RecieptProducts.html");
            StringBuilder ProductsDiv= new StringBuilder();
            foreach( var item in model.ExportProducts)
            {
                ProductsDiv.Append(

                    $"<tr>\r\n    <td style=\"text-align: center;\">{item.Product.Name}</td>\r\n   " +
                    $" <td style=\"text-align: center;\">{item.Quantity.ToString()}</td>\r\n  " +
                    $"  <td style=\"text-align: center;\">{(item.TotalPrice / item.Quantity).ToString()}</td>\r\n    " +
                    $"<td style=\"text-align: center;\">{item.TotalPrice.ToString()}\r\n</tr>"
                    );
            }
            data = data.Replace("[ExportRecieptDate]", model.Date.ToString("dd-MM-yyyy"))
                .Replace("[ExportRecieptTime]", model.Date.ToString("hh:mm:ss tt"))
                .Replace("[ExportRecieptUserName]", model.UserName)
                .Replace("[CustomerName]", model?.Customer?.Name ?? string.Empty)
                .Replace("[ExportRecieptTotal]", model.Total.ToString())
                .Replace("[ExportRecieptPaid]", model.Paid.ToString())
                .Replace("[Remaining]", model?.PreviousAccount.ToString())
                .Replace("[CustomerAccount]", model?.CurrentAccount.ToString() ?? string.Empty)
                .Replace("[RecieptProducts]", ProductsDiv.ToString());
            HtmlToPdfConverter pdf = new HtmlToPdfConverter()
            {
                Size = PageSize.A6
            };

            byte[] pdfBytes = pdf.GeneratePdf(data);
            return Ok(pdfBytes);
            //return File(pdfBytes,"application/pdf", $"{model.Customer.Name}- رقم الفاتورة: {model.ID}.pdf");
        }

        [HttpGet("ImportRecieptPrint/{Id}")]
        public async Task<IActionResult> PrintIReciept(int Id)
        {
            var model = await _context.ImportReciepts.Include(x => x.Supplier).Where(x => x.ID == Id && !x.ISDeleted).FirstOrDefaultAsync();
            model.ImportProducts = await _context.ImportProducts.Include(x => x.Product).Where(w => w.ReceiptID == Id).ToListAsync();
            var data = System.IO.File.ReadAllText(_webHostEnvironment.ContentRootPath + "Templates\\ImportReciept.html");
            var products = System.IO.File.ReadAllText(_webHostEnvironment.ContentRootPath + "Templates\\RecieptProducts.html");
            StringBuilder ProductsDiv = new StringBuilder();
            foreach (var item in model.ImportProducts)
            {
                ProductsDiv.Append(

                   $"<tr>\r\n    <td style=\"text-align: center;\">{item.Product.Name}</td>\r\n   " +
                   $" <td style=\"text-align: center;\">{item.Quantity.ToString()}</td>\r\n  " +
                   $"  <td style=\"text-align: center;\">{(item.TotalPrice / item.Quantity).ToString()}</td>\r\n    " +
                   $"<td style=\"text-align: center;\">{item.TotalPrice.ToString()}\r\n</tr>"
                   );
            }


            data = data.Replace("[ImportRecieptDate]", model.Date.ToString("dd-MM-yyyy"))
                .Replace("[ImportRecieptTime]", model.Date.ToString("hh:mm:ss tt"))
                .Replace("[ImportRecieptUserName]", model.UserName)
                .Replace("[SuppLierName]", model.Supplier.Name)
                .Replace("[RecieptProducts]", ProductsDiv.ToString())
                .Replace("[ImportRecieptTotal]", model.Total.ToString())
                .Replace("[ImportRecieptPaid]", model.Paid.ToString())
                .Replace("[SupplierAccount]", model.Supplier.Account.ToString())
                .Replace("[ImportRecieptRemaining]", (model.Supplier.Account - model.Remaining).ToString());

            HtmlToPdfConverter pdf = new HtmlToPdfConverter()
            {
                Size = PageSize.A6
            };

            byte[] pdfBytes = pdf.GeneratePdf(data);
            return Ok(pdfBytes);
            //return File(pdfBytes, "application/pdf", $"{model.Supplier.Name}- رقم الفاتورة: {model.ID}.pdf");
        }

        [HttpPost("RecieptPreview")]
        public async Task<IActionResult> RecieptPreview(ExportRecieptDTO recieptDTO)
        {
            var data = System.IO.File.ReadAllText(_webHostEnvironment.ContentRootPath + "Templates\\RecieptPreview.html");
            var products = System.IO.File.ReadAllText(_webHostEnvironment.ContentRootPath + "Templates\\RecieptProducts.html");
            StringBuilder ProductsDiv = new StringBuilder();
            foreach (var item in recieptDTO.Products)
            {
                ProductsDiv.Append(

                    $"<tr>\r\n    <td style=\"text-align: center;\">{item.ProductName}</td>\r\n   " +
                    $" <td style=\"text-align: center;\">{item.Quantity.ToString()}</td>\r\n  " +
                    $"  <td style=\"text-align: center;\">{(item.TotalPrice / item.Quantity).ToString()}</td>\r\n    " +
                    $"<td style=\"text-align: center;\">{item.TotalPrice.ToString()}\r\n</tr>"
                    );
            }
            data = data.Replace("[ExportRecieptDate]", DateTime.Parse(recieptDTO.Date).ToString("dd-MM-yyyy"))
                .Replace("[ExportRecieptTime]", DateTime.Parse(recieptDTO.Date).ToString("hh:mm:ss tt"))
                .Replace("[ExportRecieptUserName]", recieptDTO.UserName)
                .Replace("[ExportRecieptTotal]", recieptDTO.Total.ToString())
                .Replace("[RecieptProducts]", ProductsDiv.ToString());
            HtmlToPdfConverter pdf = new HtmlToPdfConverter()
            {
                Size = PageSize.A6
            };

            byte[] pdfBytes = pdf.GeneratePdf(data);
            return Ok(pdfBytes);
            //return File(pdfBytes,"application/pdf", $"{model.Customer.Name}- رقم الفاتورة: {model.ID}.pdf");
        }



        [HttpGet("ExportCarRecieptPrint/{Id}")]
        public async Task<IActionResult> PrintECarReciept(int Id) 
        {
            var model = await  _context.ExportReciepts.Include(x => x.Car).Where(x => x.ID == Id && !x.ISDeleted).FirstOrDefaultAsync();
            model.ExportProducts = await _context.ExportProducts.Include(x => x.Product).Where(w => w.ReceiptID == Id).ToListAsync();
            var data = System.IO.File.ReadAllText(_webHostEnvironment.ContentRootPath + "Templates\\ExportRecieptforCar.html");
            var products = System.IO.File.ReadAllText(_webHostEnvironment.ContentRootPath + "Templates\\RecieptProducts.html");
            StringBuilder ProductsDiv= new StringBuilder();
            foreach( var item in model.ExportProducts)
            {
                ProductsDiv.Append(

                    $"<tr>\r\n    <td style=\"text-align: center;\">{item.Product.Name}</td>\r\n   " +
                    $" <td style=\"text-align: center;\">{item.Quantity.ToString()}</td>\r\n  " +
                    $"  <td style=\"text-align: center;\">{(item.TotalPrice / item.Quantity).ToString()}</td>\r\n    " +
                    $"<td style=\"text-align: center;\">{item.TotalPrice.ToString()}\r\n</tr>"
                    );
            }
            data = data.Replace("[ExportRecieptDate]", model.Date.ToString("dd-MM-yyyy"))
                .Replace("[ExportRecieptTime]", model.Date.ToString("hh:mm:ss tt"))
                .Replace("[ExportRecieptUserName]", model.UserName)
                .Replace("[CarName]", model?.Car?.Name ?? string.Empty)
                .Replace("[ExportRecieptTotal]", model.Total.ToString())
                .Replace("[ExportRecieptPaid]", model.Paid.ToString())
                .Replace("[ExportRecieptRemain]", model?.Remaining.ToString())
                .Replace("[RecieptProducts]", ProductsDiv.ToString());
            HtmlToPdfConverter pdf = new HtmlToPdfConverter()
            {
                Size = PageSize.A6
            };

            byte[] pdfBytes = pdf.GeneratePdf(data);
            return Ok(pdfBytes);
        }



        [HttpPost("CustomerAccount/{total}/{totalpaid}")]
        public async Task<IActionResult> CustomerAccount(List<TransactionsDTO> model,decimal total, decimal totalpaid)
        {
    


            Customer cust = _context.Customers.FirstOrDefault(w => w.ID == model[0].AccountID);

            var data = System.IO.File.ReadAllText(_webHostEnvironment.ContentRootPath + "Templates\\CustTrans.html");
           StringBuilder TransDiv = new StringBuilder();
            foreach (var item in model)
            {
                if( item.Operation==2)
                {
                    item.UserName = item.OperationID + "فاتورة بيع رقم ";

                }
                else if( item.Operation==5&&item.Type==1)
                {
                    item.UserName = item.ID + "  توريد مبلغ مالى ";

                }
                else if (item.Operation == 5 && item.Type == 0)
                {
                    item.UserName = item.ID + "دفع مبلغ مالى ";

                }


                TransDiv.Append(

                    $"<tr>\r\n  " +
                    $"  <td style=\"text-align: center;\">{item.Name}</td>\r\n   " +
                    $" <td style=\"text-align: center;\">{(item.Paid + item.Remaining).ToString()}</td>\r\n  " +
                    $"  <td style=\"text-align: center;\">{item.Paid .ToString()}</td>\r\n    " +
                    $"<td style=\"text-align: center;\">{item.UserName.ToString()}\r\n" +
                    $" <td style=\"text-align: center;\">{item.Date}</td>" +
                    $" <td style=\"text-align: center;\">{item.Notes}</td>" +
                    $"</tr>"
                    );
            }
            data = data.Replace("[Date]", DateTime.UtcNow.AddHours(2).ToString("dd-MM-yyyy"))
                .Replace("[Time]", DateTime.UtcNow.AddHours(2).ToString("hh:mm:ss tt"))
                .Replace("[CustomerName]", cust.Name)
                .Replace("[Total]", total.ToString())
                .Replace("[Paid]", totalpaid.ToString())
                .Replace("[Account]", cust.Account.ToString())
                .Replace("[CustTrans]", TransDiv.ToString());
            HtmlToPdfConverter pdf = new HtmlToPdfConverter()
            {
                Size = PageSize.A6
            };

            byte[] pdfBytes = pdf.GeneratePdf(data);
            return Ok(pdfBytes);
        }
    }
}
