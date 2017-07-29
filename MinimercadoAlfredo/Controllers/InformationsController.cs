using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MinimercadoAlfredo.Context;
using MinimercadoAlfredo.Models;
using MinimercadoAlfredo.ViewModels.ResumenVentas;

namespace MinimercadoAlfredo.Controllers
{
    public class InformationsController : Controller
    {
        private AlfredoContext db = new AlfredoContext();

        // GET: Informations
        public ActionResult DaySales(DateTime date)
        {
            DaySalesVM ds = new DaySalesVM();
            ds.Date = date;
            ds.Sales = db.Sales.ToList().FindAll(x => x.SaleDate == date & x.SaleState == SaleState.Finalizada);
            ds.DaySalesTotal = 0;

            foreach (var sale in ds.Sales)
            {
                foreach (var saleline in sale.SaleLines)
                {
                    ds.DaySalesTotal += saleline.LineTotal;
                }

            }


            return View(ds);
        }

      
        public ActionResult MonthSales(DateTime date)
        {


            MonthSalesVM ms = new MonthSalesVM();
            ms.MDate = date;
            ms.MonthSalesTotal = 0;
            List<DaySalesVM> lista = new List<DaySalesVM>();
            int days = DateTime.DaysInMonth(date.Year, date.Month);

            for (int day = 1; day <= days; day++)
            {
                DaySalesVM ds = new DaySalesVM();
                ds.Date = new DateTime(date.Year, date.Month, day);
                ds.Sales = db.Sales.ToList().FindAll(x => x.SaleDate == ds.Date & x.SaleState == SaleState.Finalizada);
                ds.DaySalesTotal = 0;

                if (ds.Sales.Count > 0)
                {
                    foreach (var item in ds.Sales)
                    {
                        foreach (var saleline in item.SaleLines)
                        {
                            ds.DaySalesTotal += saleline.LineTotal;
                        }
                    }
                    ms.MonthSalesTotal += ds.DaySalesTotal;
                }
                lista.Add(ds);

            }
            ms.DaySales = lista;

            return View(ms);
        }
      

        public ActionResult YearSales(DateTime date)
        {


            YearSalesVM ys = new YearSalesVM();
            ys.YDate = date;
            ys.YearSalesTotal = 0;
            List<MonthSalesVM> lista = new List<MonthSalesVM>();
            int months = 12;

            for (int month = 1; month <= months; month++)
            {
                MonthSalesVM ms = new MonthSalesVM();
                ms.MDate = new DateTime(date.Year, month, 1);
                var salesm = db.Sales.ToList().FindAll(x => x.SaleDate.Year == date.Year & x.SaleDate.Month == month & x.SaleState == SaleState.Finalizada);
                ms.MonthSalesTotal = 0;

                if (salesm.Count() > 0)
                {
                    foreach (var item in salesm)
                    {
                        foreach (var saleline in item.SaleLines)
                        {
                            ms.MonthSalesTotal += saleline.LineTotal;
                        }
                    }
                    ys.YearSalesTotal += ms.MonthSalesTotal;
                }
                lista.Add(ms);

            }
            ys.MonthSales = lista;

            return View(ys);
        }


        public ActionResult Index()
        {

            return View();
        }
    }
}