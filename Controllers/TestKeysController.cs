using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BotExtension.Helper;
using Microsoft.AspNetCore.Mvc;

namespace BotExtension.Controllers
{
    public class TestKeysController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                Tuple<string, string> MicrosoftKeys = FetchIDKeys.GetMicrosoftKeys();
                Tuple<string, string, string> LUISKeys = FetchIDKeys.GetLuisKeys();
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
            catch (Exception ex)
            {
                Trace.Write(ex.ToString());
            }
            return View();
        }
    }
}