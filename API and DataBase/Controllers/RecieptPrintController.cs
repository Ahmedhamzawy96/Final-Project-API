﻿using API_and_DataBase.DTO;
using API_and_DataBase.DTO.Extension_Methods;
using API_and_DataBase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NReco.PdfGenerator;
using System.Collections.Generic;
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
            data = data.Replace("[ExportRecieptDate]", model.Date.ToString("dd/mm/yyyy"))
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


            data = data.Replace("[ImportRecieptDate]", model.Date.ToString("dd/mm/yyyy"))
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
            data = data.Replace("[ExportRecieptDate]", DateTime.Parse(recieptDTO.Date).ToString("dd/mm/yyyy"))
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
    }
}
