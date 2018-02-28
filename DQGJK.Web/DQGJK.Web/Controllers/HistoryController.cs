﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DQGJK.Web.Controllers
{
    public class HistoryController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult List()
        {
            return PartialView("List");
        }
    }
}