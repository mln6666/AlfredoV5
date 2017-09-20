using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MinimercadoAlfredo.Context;
using MinimercadoAlfredo.Models;
using Org.BouncyCastle.Asn1.X509;

namespace MinimercadoAlfredo.Controllers

{
    public class PDFController : Controller
    {
        public ActionResult GetPDF(string[] arraySelected)
        {
           PdfHelper pdfHerlper = new PdfHelper();

           var file = pdfHerlper.CreatePdfReport(arraySelected);

            return File(file);

        }
    }

}
