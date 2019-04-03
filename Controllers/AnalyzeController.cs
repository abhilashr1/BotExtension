using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BotExtension.Controllers
{
    public class AnalyzeController : Controller
    {
        public IActionResult Index()
        {
            try
            {


                // Fetch the IIS logfiles
                string relativePath = "../../LogFiles/http/RawLogs";
                string[] logfiles = Directory.GetFiles(relativePath, "*.log",
                                             SearchOption.AllDirectories);
                List<string> filteredLogs = new List<string>();
                foreach (string file in logfiles)
                {
                    using (var fileStream = System.IO.File.OpenRead(file))
                    using (var streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8, true, 512))
                    {
                        String line;
                        while (((line = streamReader.ReadLine()) != null))
                        {
                            // Ignore the IIS log headers and capture only POST activities on bot
                            if (line.StartsWith('#') == false && line.Contains("POST"))
                            {
                                if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_IIS_SITE_NAME")))
                                {
                                    // Ignore logs which aren't from the bot, that is, KUDU Console and this extension included. 

                                    if (line.Contains(Environment.GetEnvironmentVariable("WEBSITE_IIS_SITE_NAME").ToUpper()) == false)
                                    {
                                        filteredLogs.Add(line);
                                    }
                                }
                                else
                                {
                                    filteredLogs.Add(line);
                                }
                            }
                        }
                    }
                }
                ViewData["log"] = filteredLogs;
                ViewData["averageResponse"] = (double)Helper.LogAnalysis.AverageResponseTime(filteredLogs);
                ViewData["timeouts"] = Helper.LogAnalysis.Timeouts(filteredLogs);
                ViewData["non200s"] = Helper.LogAnalysis.Non200Statuses(filteredLogs);
            }
            catch(Exception e)
            {
                ViewData["exception"] = e.Message;
            }

            return View();
        }
    }
}