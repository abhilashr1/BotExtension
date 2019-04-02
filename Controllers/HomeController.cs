using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BotExtension.Models;
using BotExtension.Helper;
using System.Xml;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BotExtension.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                Tuple<string, string> MicrosoftKeys = FetchIDKeys.GetMicrosoftKeys();
                Tuple<string, string,string> LUISKeys= FetchIDKeys.GetLuisKeys();
                Tuple<string, string, string> QnaKeys = FetchIDKeys.GetQnaKeys();

                ViewBag.MicrosoftAppID = MicrosoftKeys.Item1;
                ViewBag.MicrosoftAppPassword = MicrosoftKeys.Item2;

                ViewBag.LuisAppID = LUISKeys.Item1;
                ViewBag.LUISKey = LUISKeys.Item2;
                ViewBag.LUISHostname = LUISKeys.Item3;

                ViewBag.QnaKnowledgebaseID = QnaKeys.Item1;
                ViewBag.QnaKey = QnaKeys.Item2;
                ViewBag.QnaHostname = QnaKeys.Item3;

            }
            catch(Exception ex)
            {
                Trace.Write(ex.ToString());
                throw;
            }
            return View();
        }

        public IActionResult Sink()
        {
            Response.StatusCode = 200;
            return new EmptyResult();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
