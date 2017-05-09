using POC_SAP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POC_SAP.Models;

namespace POC_SAP.Controllers
{
    public class SapController : Controller
    {
        // GET: Sap
        public ActionResult Index()
        {
            SapManager sm = new SapManager();
            var result = sm.QueryTableFromSap();
            return View("Index", result);
        }
    }
}