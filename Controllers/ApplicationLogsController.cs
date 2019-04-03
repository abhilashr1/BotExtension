using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace BotExtension.Controllers
{
    public class ApplicationLogsController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                string relativePath = "../../LogFiles/Application";
                string[] logfiles = Directory.GetFiles(relativePath, "*.txt",
                                             SearchOption.AllDirectories);
                Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
                foreach (string file in logfiles)
                {
                    List<string> text = new List<string>();
                    // Read Line by line to preserve spacing
                    using (var fileStream = System.IO.File.OpenRead(file))
                    using (var streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8, true, 512))
                    {
                        String line;
                        while (((line = streamReader.ReadLine()) != null))
                        {
                            text.Add(line);
                        }
                    }

                    DateTime modification = System.IO.File.GetLastWriteTime(file);
                    if (result.ContainsKey(modification.ToString("MMMM dd, yy HH:mm:ss ")))
                    {
                        result[modification.ToString("MMMM dd, yyyy HH:mm:ss ")].AddRange(text);
                    }
                    else
                    {
                        result[modification.ToString("MMMM dd, yyyy HH:mm:ss ")] = text;
                    }
                }

                ViewData["applogs"] = result;
            }
            catch(Exception e)
            {
                ViewData["exception"] = e.Message;
            }
            return View();
        }
    }
}