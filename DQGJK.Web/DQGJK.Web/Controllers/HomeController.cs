﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DQGJK.Web.PageModels;
using DQGJK.Models;
using DQGJK.Web.Contexts;
using System.Xml.Linq;
using System.Collections;
using Microsoft.AspNetCore.Hosting;

namespace DQGJK.Web.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}