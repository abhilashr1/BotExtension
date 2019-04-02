using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BotExtension.Controllers
{
    public class EnvironmentController : Controller
    {

        public IActionResult Index()
        {
            ViewData["Environment"] = Environment.GetEnvironmentVariables();
            if (ViewData["Environment"] == null)
            {
                var ele = new Dictionary<string, string>();
                ele["Test"] = "Message";
                ViewData["Environment"] = ele;
            }
            return View();
        }
    }
}