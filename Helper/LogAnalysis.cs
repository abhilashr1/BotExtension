using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotExtension.Helper
{
    public class LogAnalysis
    {
        public static double AverageResponseTime(List<string> logs)
        {
            long sum = 0;
            foreach(var line in logs)
            {
                string[] saTimeTaken = line.Split(' ');
                string sTimeTaken = saTimeTaken[saTimeTaken.Count() - 1].Trim();
                long lTimeTaken = 0;
                long.TryParse(sTimeTaken, out lTimeTaken);
                sum += lTimeTaken;
            }
            if (logs.Count > 0)
                return sum / ((logs.Count-1)*1000);
            return -1;
        }

        public static List<string> Timeouts(List<string> logs)
        {
            List<string> timeout = new List<string>();
            foreach (var line in logs)
            {
                string[] saTimeTaken = line.Split(' ');
                string sTimeTaken = saTimeTaken[saTimeTaken.Count() - 1].Trim();
                long lTimeTaken = 0;
                long.TryParse(sTimeTaken, out lTimeTaken);
                if(lTimeTaken > 14500)
                {
                    // Response is greater than 15 seconds, this leads to timeout
                    timeout.Add(line);
                }
            }
            return timeout;
        }

        public static List<string> Non200Statuses(List<string> logs)
        {
            List<string> statuses = new List<string>();
            foreach (var line in logs)
            {
                string[] saStatusCode = line.Split(' ');
                string sStatusCode = saStatusCode[saStatusCode.Count() - 6].Trim();
                long lStatusCode = 0;
                long.TryParse(sStatusCode, out lStatusCode);
                if (lStatusCode > 299)
                {
                    // Response is greater than 15 seconds, this leads to timeout
                    statuses.Add(line);
                }
            }
            return statuses;
        }
    }
}
